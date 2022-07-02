using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class CombatController : StateMachine
{
    #region Fields
    private Spawner _spawner;
    private HUD _hud;
    private Dictionary<GameObject, int> _herosToSlots;
    private Dictionary<GameObject, int> _enemiesToSlots;
    private Dictionary<GameObject, Fighter> _herosToFighters;
    private Dictionary<GameObject, Fighter> _enemiesToFighters;
    [Header("Game Mode")]
    [SerializeField] private GameMode _gameMode;
    [Header("Debugging")]
    [SerializeField] private int _selectedHeroSlot;
    [SerializeField] private int _selectedEnemySlot;
    [Header("Durations")]
    [SerializeField] private float _messageUnfadeDuration;
    [SerializeField] private float _messageFadeDuration;
    [SerializeField] private float _turnDurationSeconds;
    [Header("Camera of the game")]
    [SerializeField] private Camera _mainCamera;

    [HideInInspector] public EnemyTurnState EnemyTurnState;
    [HideInInspector] public HeroTurnState HeroTurnState;
    [HideInInspector] public PauseState PauseState;
    [HideInInspector] public HeroMovingState HeroMovingState;
    [HideInInspector] public EnemyMovingState EnemyMovingState;
    [HideInInspector] public WinState WinState;
    [HideInInspector] public LossState LossState;
    #endregion

    #region Properties
    public float TurnDurationSeconds => _turnDurationSeconds;
    public float MessageFadeDuration =>_messageFadeDuration;
    public float MessageUnfadeDuration => _messageUnfadeDuration;
    public Dictionary<GameObject, int> HerosToSlots => _herosToSlots;
    public Dictionary<GameObject, int> EnemiesToSlots => _enemiesToSlots;
    public Dictionary<GameObject, Fighter> HerosToFighters => _herosToFighters;
    public Dictionary<GameObject, Fighter> EnemiesToFighters => _enemiesToFighters;
    public GameMode GameMode => _gameMode;

    public int SelectedHeroSlot
    {
        get => _selectedHeroSlot;
        private set => _selectedHeroSlot = value;
    }

    public int SelectedEnemySlot
    {
        get => _selectedEnemySlot;
        private set => _selectedEnemySlot = value;
    }

    public int HeroCount => _herosToFighters.Count;
    public int EnemyCount => _enemiesToFighters.Count;
    #endregion

    #region MonoBehaviour Methods
    private void Awake()
    {
        _spawner = GetComponent<Spawner>();
        _hud = GetComponent<HUD>();
        _herosToSlots = new Dictionary<GameObject, int>();
        _enemiesToSlots = new Dictionary<GameObject, int>();
        _herosToFighters = new Dictionary<GameObject, Fighter>();
        _enemiesToFighters = new Dictionary<GameObject, Fighter>();

        EnemyTurnState = new EnemyTurnState(this);
        HeroTurnState = new HeroTurnState(this);
        PauseState = new PauseState(this);
        HeroMovingState = new HeroMovingState(this);
        WinState = new WinState(this);
        LossState = new LossState(this);

        SelectedHeroSlot = SelectedEnemySlot = -1;
    }

    private void OnEnable()
    {
        if (!AreUnitArraysCorrect())
        {
            throw new InvalidProgramException("Unaccaptable number of slots");
        }

        base.SetUp();
        SpawnAllUnits();
        _hud.PlaceAllUnitInfos();
        AssignEventsToFighters();
        AssignEventsToMovers();
    }

    private void Update()
    {
        base.UpdateLogic();

        if (Input.GetMouseButtonDown(0))
        {
            SetSelectedUnitsSlots();
        }
    }

    #endregion

    #region CustomMethods

    private bool AreUnitArraysCorrect()
    {
        return  (_spawner.HeroSlots.Length < _spawner.MaxEnemyRowCount || 
                _spawner.HeroSlots.Length % _spawner.MaxEnemyRowCount == 0) &&
                (_spawner.EnemySlots.Length < _spawner.MaxEnemyRowCount ||
                _spawner.EnemySlots.Length % _spawner.MaxEnemyRowCount == 0) &&
                _spawner.EnemySlots.Length == _spawner.HeroSlots.Length;
    }

    private void SpawnAllUnits()
    {
        GameObject spawnedObject;

        for (int i = _spawner.HeroSlots.Length - 1; i >= 0; --i)
        {
            if (_spawner.HeroSlots[i] != null)
            {
                spawnedObject = _spawner.SpawnHeroAtSlot(i);
                HerosToSlots.Add(spawnedObject, i);
                HerosToFighters.Add(spawnedObject, spawnedObject.GetComponent<Fighter>());
            }
        }

        for (int i = _spawner.EnemySlots.Length - 1; i >= 0; --i)
        {
            if (_spawner.EnemySlots[i] != null)
            {
                spawnedObject = _spawner.SpawnEnemyAtSlot(i);
                EnemiesToSlots.Add(spawnedObject, i);
                EnemiesToFighters.Add(spawnedObject, spawnedObject.GetComponent<Fighter>());
            }
        }
    }

    private void AssignEventsToFighters()
    {
        foreach (var pair in HerosToFighters)
        {
            pair.Value.OnDied += () =>
            {
                RefreshSelectedSlots();
                _hud.FightersToUnitInfos.Remove(pair.Value);
                HerosToSlots.Remove(pair.Key);
                HerosToFighters.Remove(pair.Key);
                Destroy(pair.Key);
            };
        }

        foreach (var pair in EnemiesToFighters)
        {
            pair.Value.OnDied += () =>
            {
                RefreshSelectedSlots();
                _hud.FightersToUnitInfos.Remove(pair.Value);
                EnemiesToSlots.Remove(pair.Key);
                EnemiesToFighters.Remove(pair.Key);
                Destroy(pair.Key);
            };
        }
    }

    private void AssignEventsToMovers()
    {
        Mover mover;
        foreach (var pair in HerosToFighters)
        {
            mover = pair.Key.GetComponent<Mover>();
            mover.OnMoving += index =>
            {
                _spawner.HeroPlatformFaders[SelectedHeroSlot].Unfade();
                HerosToSlots[pair.Key] = index;
                SelectedHeroSlot = index;
            };
        }

        foreach (var pair in EnemiesToFighters)
        {
            mover = pair.Key.GetComponent<Mover>();
            mover.OnMoving += index =>
            {
                _spawner.EnemyPlatformFaders[SelectedEnemySlot].Unfade();
                EnemiesToSlots[pair.Key] = index;
                SelectedEnemySlot = index;
            };
        }
    }

    private void SetSelectedUnitsSlots()
    {
        if (!CurrentState.Equals(HeroTurnState))
            return;

        Collider2D detectedCollider = GetColliderIntersectingMouse();

        if (detectedCollider == null)
        {
            RefreshSelectedSlots();
            return;
        }

        if(!detectedCollider.gameObject.CompareTag("Unit"))
        {
            //Not unit collider was hit
            return;
        }

        //try
        //{
        //    if(SelectedHeroSlot != -1)
        //        _spawner.HeroPlatformFaders[SelectedHeroSlot].Unfade();
        //    SelectedHeroSlot = GetHeroSlot(detectedCollider.gameObject);
        //    _spawner.HeroPlatformFaders[SelectedHeroSlot].Fade();
        //}
        //catch (InvalidOperationException)
        //{
        //    if (SelectedEnemySlot != -1)
        //        _spawner.EnemyPlatformFaders[SelectedEnemySlot].Unfade();
        //    SelectedEnemySlot = GetEnemySlot(detectedCollider.gameObject);
        //    _spawner.EnemyPlatformFaders[SelectedEnemySlot].Fade();
        //}
        int selectedSlot;

        try
        {
            selectedSlot = GetHeroSlot(detectedCollider.gameObject);
            if (SelectedHeroSlot != -1)
                _spawner.HeroPlatformFaders[SelectedHeroSlot].Unfade();
            SelectedHeroSlot = selectedSlot;
            _spawner.HeroPlatformFaders[SelectedHeroSlot].Fade();
        }
        catch (InvalidOperationException)
        {
            selectedSlot = GetEnemySlot(detectedCollider.gameObject);
            if (SelectedEnemySlot != -1)
                _spawner.EnemyPlatformFaders[SelectedEnemySlot].Unfade();
            SelectedEnemySlot = selectedSlot;
            _spawner.EnemyPlatformFaders[SelectedEnemySlot].Fade();
        }
    }

    private Collider2D GetColliderIntersectingMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = _mainCamera.nearClipPlane;
        Vector2 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);

        return Physics2D.OverlapBox(mouseWorldPosition, new Vector2(0.2f, 0.2f), 0);
    }

    public int GetHeroSlot(GameObject hero)
    {
        if(HerosToSlots.ContainsKey(hero))
        {
            return HerosToSlots[hero];
        }
        else
        {
            throw new InvalidOperationException("Trying to get slot of non-existent hero");
        }
    }

    public int GetEnemySlot(GameObject enemy)
    {
        if (EnemiesToSlots.ContainsKey(enemy))
        {
            return EnemiesToSlots[enemy];
        }
        else
        {
            throw new InvalidOperationException("Trying to get slot of non-existent enemy");
        }
    }

    public GameObject GetHeroAtSlot(int slot)
    {
        GameObject res = HerosToSlots.FirstOrDefault(hero => hero.Value == slot).Key;
        return res ?? throw new InvalidOperationException("The slot is empty");
    }

    public GameObject GetEnemyAtSlot(int slot)
    {
        GameObject res = EnemiesToSlots.FirstOrDefault(enemy => enemy.Value == slot).Key;
        return res ?? throw new InvalidOperationException("The slot is empty");
    }

    public Fighter GetHeroFighterAtSlot(int slot) => HerosToFighters[GetHeroAtSlot(slot)];

    public Fighter GetEnemyFighterAtSlot(int slot) => EnemiesToFighters[GetEnemyAtSlot(slot)];

    public bool IsHeroSlotOccupied(int slot) 
        => HerosToSlots.FirstOrDefault(hero => hero.Value == slot).Key != null;

    public bool IsEnemySlotOccupied(int slot)
        => EnemiesToSlots.FirstOrDefault(enemy => enemy.Value == slot).Key != null;

    public int GetRowOfHeroSlot(int slot) => slot / _spawner.HeroSlotColsCount;

    public GameObject GetHeroByFighter(Fighter fighter)
    {
        GameObject res = HerosToFighters.FirstOrDefault(hero => hero.Value == fighter).Key;
        return res ?? throw new InvalidOperationException("No hero containing the fighter.");
    }

    public GameObject GetEnemyByFighter(Fighter fighter)
    {
        GameObject res = EnemiesToFighters.FirstOrDefault(enemy => enemy.Value == fighter).Key;
        return res ?? throw new InvalidOperationException("No enemy containing the fighter.");
    }

    public int GetUnitSlotByFighter(Fighter fighter)
    {
        try
        {
            return HerosToSlots[GetHeroByFighter(fighter)];
        }
        catch (InvalidOperationException)
        {
            return EnemiesToSlots[GetEnemyByFighter(fighter)];
        }
    }

    public void RefreshSelectedSlots()
    {
        //if(SelectedHeroSlot != -1)
        //{
        //    _spawner.HeroPlatformFaders[SelectedHeroSlot].Unfade();
        //}

        //if(SelectedEnemySlot != -1)
        //{
        //    _spawner.EnemyPlatformFaders[SelectedEnemySlot].Unfade();
        //}

        //SelectedHeroSlot = SelectedEnemySlot = -1;
        RefreshHeroSelectedSlot();
        RefreshEnemySelectedSlot();
    }

    public void RefreshHeroSelectedSlot()
    {
        if (SelectedHeroSlot != -1)
        {
            _spawner.HeroPlatformFaders[SelectedHeroSlot].Unfade();
        }

        SelectedHeroSlot = -1;
    }

    public void RefreshEnemySelectedSlot()
    {
        if (SelectedEnemySlot != -1)
        {
            _spawner.EnemyPlatformFaders[SelectedEnemySlot].Unfade();
        }

        SelectedEnemySlot = -1;
    }

    public bool IsHeroSlotSelected() => SelectedHeroSlot != -1;

    public bool IsEnemySlotSelected() => SelectedEnemySlot != -1;

    public bool AreSlotsSelected() => IsHeroSlotSelected() && IsEnemySlotSelected();

    public void PauseCombat() => Time.timeScale = 0f;
    
    public void UnpauseCombat() => Time.timeScale = 1f;

    protected override State InitialState() => HeroTurnState;
    #endregion
}