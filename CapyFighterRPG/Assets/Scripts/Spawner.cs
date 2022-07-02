using UnityEngine;
using System;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private GameObject _platformPrefab;

    [Header("GameObject to store all units in. Not a unit prefab!")]
    [SerializeField] private GameObject _unitContainer;

    [Header("Slot Count Boundaries")]
    [Tooltip("Max number of hero slots in one column")]
    [SerializeField] private int _maxHeroRowCount;
    [Tooltip("Max number of enemy slots in one column")]
    [SerializeField] private int _maxEnemyRowCount;

    [Header("Boundary Points For the Field of Hero Slots :")]
    [SerializeField] private Vector3 _heroOrigin;
    [SerializeField] private Vector3 _heroOriginsNeighbour;
    [SerializeField] private Vector3 _heroDiagonalToOrigin;

    [Header("Boundary Points For the Field of Enemy Slots :")]
    [SerializeField] private Vector3 _enemyOrigin;
    [SerializeField] private Vector3 _enemyOriginsNeighbour;
    [SerializeField] private Vector3 _enemyDiagonalToOrigin;

    [Header("Heros at slots (not all slots have to be filled)")]
    [SerializeField] private UnitData[] _heroSlots;

    [Header("Enemies at slots (not all slots have to be filled)")]
    [SerializeField] private UnitData[] _enemySlots;

    [Header("Debug")]
    [SerializeField] private GameObject _debugPoint;
    [SerializeField] private bool _debugEnabled;

    private Vector3[] _heroSlotPositions;
    private Vector3[] _enemySlotPositions;
    private PlatformFader[] _heroPlatformFaders;
    private PlatformFader[] _enemyPlatformFaders;


    public int HeroSlotCount => _heroSlots.Length;
    public int EnemySlotCount => _enemySlots.Length;
    public int MaxHeroRowCount => _maxHeroRowCount;
    public int MaxEnemyRowCount => _maxEnemyRowCount;
    public int MaxHeroColumnCount => _heroSlots.Length / _maxHeroRowCount;
    public int MaxEnemyColumnCount => _enemySlots.Length / _maxEnemyRowCount;
    public UnitData[] HeroSlots => _heroSlots;
    public UnitData[] EnemySlots => _enemySlots;
    public Vector3[] HeroSlotPositions => _heroSlotPositions;
    public Vector3[] EnemySlotPositions => _enemySlotPositions;
    public PlatformFader[] HeroPlatformFaders => _heroPlatformFaders;
    public PlatformFader[] EnemyPlatformFaders => _enemyPlatformFaders;
    public int HeroSlotRowsCount => GetSlotRowsCount(HeroSlots.Length, MaxHeroRowCount);
    public int EnemySlotRowsCount => GetSlotRowsCount(EnemySlots.Length, MaxEnemyRowCount);
    public int HeroSlotColsCount => GetSlotColsCount(HeroSlots.Length, MaxHeroRowCount);
    public int EnemySlotColsCount => GetSlotColsCount(EnemySlots.Length, MaxEnemyRowCount);



    public void Awake()
    {
        _heroPlatformFaders = new PlatformFader[HeroSlots.Length];
        _enemyPlatformFaders = new PlatformFader[HeroSlots.Length];

        EvaluateSlotPositions();
        PlacePlatforms();
    }

    public GameObject SpawnHeroAtSlot(int slot)
    {
        if (slot >= HeroSlots.Length)
            throw new IndexOutOfRangeException($"{slot}");
        if (HeroSlots[slot] == null)
            throw new InvalidOperationException($"Trying to instantiate a hero at an empty slot : {slot}");

        GameObject spawned =
            Instantiate(_unitPrefab, _heroSlotPositions[slot], Quaternion.identity, _unitContainer.transform);

        spawned.GetComponent<Unit>().Init(HeroSlots[slot]);
        spawned.GetComponent<SpriteRenderer>().flipX = false;
        spawned.GetComponent<Mover>().GetPositionOfSlot += slot =>  HeroSlotPositions[slot];

        return spawned;
    }

    public GameObject SpawnEnemyAtSlot(int slot)
    {
        if (slot >= EnemySlots.Length)
            throw new IndexOutOfRangeException($"{slot}");
        if (EnemySlots[slot] == null)
            throw new InvalidOperationException($"Trying to instantiate an enemy at an empty slot : {slot}");

        GameObject spawned =
            Instantiate(_unitPrefab, _enemySlotPositions[slot], Quaternion.identity, _unitContainer.transform);

        spawned.GetComponent<Unit>().Init(EnemySlots[slot]);
        spawned.GetComponent<SpriteRenderer>().flipX = true;
        spawned.GetComponent<Mover>().GetPositionOfSlot += slot => EnemySlotPositions[slot];

        return spawned;
    }

    public void PlacePlatforms()
    {
        //foreach (var position in HeroSlotPositions)
        //{
        //    _platformPrefab.transform.position = position;
        //    Instantiate(_platformPrefab);
        //}

        //foreach (var position in EnemySlotPositions)
        //{
        //    _platformPrefab.transform.position = position;
        //    Instantiate(_platformPrefab);
        //}

        for (int i = 0; i < _heroSlotPositions.Length; ++i)
        {
            _platformPrefab.transform.position = _heroSlotPositions[i];
            HeroPlatformFaders[i] = Instantiate(_platformPrefab).GetComponent<PlatformFader>();
        }

        for(int i = 0; i < _enemySlotPositions.Length; ++i)
        {
            _platformPrefab.transform.position = _enemySlotPositions[i];
            EnemyPlatformFaders[i] = Instantiate(_platformPrefab).GetComponent<PlatformFader>();
        }
    }
    private void EvaluateSlotPositions()
    {
        VectorGrid grid;

        grid = new VectorGrid
            (
            _heroOrigin,
            _heroOriginsNeighbour,
            _heroDiagonalToOrigin,
            GetSlotRowsCount(HeroSlots.Length, MaxHeroRowCount),
            GetSlotColsCount(HeroSlots.Length, MaxHeroRowCount)
            );

        _heroSlotPositions = grid.CalculateCellCentres();

        SetDebugPointBoundaries(grid);

        grid = new VectorGrid
            (
            _enemyOrigin,
            _enemyOriginsNeighbour,
            _enemyDiagonalToOrigin,
            GetSlotRowsCount(EnemySlots.Length, MaxEnemyRowCount),
            GetSlotColsCount(EnemySlots.Length, MaxEnemyRowCount)
            );

        _enemySlotPositions = grid.CalculateCellCentres();

        SetDebugPointBoundaries(grid);
        DebugPointSettings();
    }

    private int GetSlotRowsCount(int totalSlotCount, int maxRowCount)
    {
        if (totalSlotCount < maxRowCount)
            return totalSlotCount;
        else
            return maxRowCount;
    }

    private int GetSlotColsCount(int totalSlotCount, int maxRowCount)
    {
        if (totalSlotCount < maxRowCount)
            return 1;
        else
        {
            if (totalSlotCount % maxRowCount != 0)
                throw new InvalidOperationException
                    ("Max number of rows must devide the total number of slots provided " +
                    "at least one column is filled.");
            return totalSlotCount / maxRowCount;
        }
    }

    private void DebugPointSettings()
    {
        if (_debugEnabled)
        {
            foreach (var position in _heroSlotPositions)
            {
                SetDebugPoint(position);
            }

            foreach (var position in _enemySlotPositions)
            {
                SetDebugPoint(position);
            }
        }
    }

    private void SetDebugPointBoundaries(VectorGrid grid)
    {
        if(_debugEnabled)
        {
            SetDebugPoint(grid.Origin);
            SetDebugPoint(grid.OriginsNeighbour);
            SetDebugPoint(grid.DiagonalToOrigin);
            SetDebugPoint(grid.OtherNeighbour);
        }
    }

    private void SetDebugPoint(Vector3 position)
    {
        GameObject point = Instantiate(_debugPoint);
        point.transform.position = position;
    }
}