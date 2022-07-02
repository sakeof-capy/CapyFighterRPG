using UnityEngine;
using System.Collections.Generic;
using System;

public class GameProgress : MonoBehaviour
{
    private CombatController _combatController;
    private AchievementMessageShower _achievementMessageShower;

    public bool IsInitialized { get; private set; }
    public int Level => GameStats.Level;

    #region Fields to Serialize
    public GameStats GameStats { get; private set; }
    public List<Achievement> Achievements { get; private set; }
    #endregion

    #region Setings
    [SerializeField] private int _expForKill;
    [SerializeField] private int _damageForOneExp;
    #endregion

    #region Events
    public event Action<int> OnLevelReached;
    #endregion

    private void Awake()
    {
        _combatController = GetComponent<CombatController>();
        _achievementMessageShower = GetComponent<AchievementMessageShower>();
        IsInitialized = true;
    }

    private void Start()
    {
        AssignEventsToFighters();
        AssignLocalEvents();
        AssignGameStateEvents();
    }

    public void Init(GameProgressSave save)
    {
        Achievements = save.Achievements;
        GameStats = save.Stats;

        foreach (var ach in Achievements)
        {
            ach.Attach(GameStats);
            ach.OnFirstStarAchieved +=  () => _achievementMessageShower.ShowAchievementMessage(ach);
            ach.OnSecondStarAchieved += () => _achievementMessageShower.ShowAchievementMessage(ach);
            ach.OnThirdStarAchieved +=  () => _achievementMessageShower.ShowAchievementMessage(ach);
        }
    }

    private void AssignEventsToFighters()
    {
        foreach (var enemyFighter in _combatController.EnemiesToFighters.Values)
        {
            enemyFighter.OnTotalDamageReceived += (damage) =>
            {
                GameStats.TotalDamage += damage;
                GetExperience(damage);
                UpdateAllAchievements();
            };

            enemyFighter.OnTotalShieldDamageReceived += (damage) =>
            {
                GameStats.ShieldDamage += damage;
                GetExperience(damage);
                UpdateAllAchievements();
            };

            enemyFighter.OnDied += () =>
            {
                ++GameStats.EnemiesKilled;
                GetExperience(_expForKill);
                UpdateAllAchievements();
            };
        }

        foreach(var heroFighter in _combatController.HerosToFighters.Values)
        {
            heroFighter.OnTotalDamageReceived += damage =>
            {
                GameStats.TotalDamageReceived += damage;
                UpdateAllAchievements();
            };

            heroFighter.OnTotalShieldDamageReceived += damage =>
            {
                GameStats.ShieldDamageReceived += damage;
                UpdateAllAchievements();
            };

            heroFighter.OnDied += () =>
            {
                ++GameStats.DeathCount;
                UpdateAllAchievements();
            };
        }
    }

    private void AssignLocalEvents()
    {
        OnLevelReached += level => UpdateAllAchievements();
    }

    private void AssignGameStateEvents()
    {
        _combatController.WinState.OnEntered += () =>
        {
            switch (_combatController.GameMode)
            {
                case GameMode.Light:
                    ++GameStats.LightModeTimes;
                    break;
                case GameMode.Medium:
                    ++GameStats.MediumModeTimes;
                    break;
                case GameMode.Hard:
                    ++GameStats.HardModeTimes;
                    break;
            }

            UpdateAllAchievements();
        };
    }

    private void UpdateAllAchievements()
    {
        foreach (var ach in Achievements)
        {
            ach.UpdateProgress();
        }
    }

    public void GetExperience(int damage)
    {
        var exp = damage / _damageForOneExp;
        GameStats.LevelProgress += exp;

        var prevLvl = GameStats.Level;
        GameStats.Level = CalculateLvlFromExp(GameStats.LevelProgress);
        if(GameStats.Level > prevLvl)
            OnLevelReached?.Invoke(GameStats.Level);
    }

    public int CalculateLvlFromExp(int exp)
    {
        if (exp < 200) return 0;
        return 1 + (exp - 200) / 1000;
    }
}