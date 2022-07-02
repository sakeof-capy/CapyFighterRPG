using System;

[Serializable]
public class Achievement
{
    private AchievementType _type;
    private string _name;

    private int _firstStarPoints;
    private int _secondStarPoints;
    private int _thirdStarPoints;

    private string _firstStarDescription;
    private string _secondStarDescription;
    private string _thirdStarDescription;

    private int _progress;

    private GameStats _gameStats;

    [field: NonSerialized] public event Action OnFirstStarAchieved;
    [field: NonSerialized] public event Action OnSecondStarAchieved;
    [field: NonSerialized] public event Action OnThirdStarAchieved;

    private bool _firstStarBeenShown;
    private bool _secondStarBeenShown;
    private bool _thirdStarBeenShown;

    public Achievement(AchievementData data, GameStats stats)
    {
        _type = data.Type;
        _name = data.Name;

        _firstStarPoints = data.FirstStarPoints;
        _secondStarPoints = data.SecondStarPoints;
        _thirdStarPoints = data.ThirdStarPoints;

        _firstStarDescription = data.FirstStarDescription;
        _secondStarDescription = data.SecondStarDescription;
        _thirdStarDescription = data.ThirdStarDescription;

        _gameStats = stats;

        _progress = 0;

        _firstStarBeenShown = false;
        _secondStarBeenShown = false;
        _thirdStarBeenShown = false;
    }

    public void Attach(GameStats stats)
    {
        _gameStats = stats;
    }

    public bool IsFirstStarFulfilled() => _progress >= _firstStarPoints;

    public bool IsSecondStarFulfilled() => _progress >= _secondStarPoints;

    public bool IsThirdStarFulfilled() => _progress >= _thirdStarPoints;

    public float CurrentStarProgressPercentage() => (float)_progress / CurrentStarTotal();

    public int CurrentStarTotal()
    {
        int total = _firstStarPoints;
        if (IsFirstStarFulfilled()) total = _secondStarPoints;
        if (IsSecondStarFulfilled()) total = _thirdStarPoints;

        return total;
    }

    public string CurrentStarDescription()
    {
        string description = _firstStarDescription;
        if (IsFirstStarFulfilled()) description = _secondStarDescription;
        if (IsSecondStarFulfilled()) description = _thirdStarDescription;
        return description;
    }

    public void UpdateProgress()
    {
        var currParameterValue = ProgressParameterValue();
        if (currParameterValue >= _thirdStarPoints)
            _progress = _thirdStarPoints;
        else
            _progress = currParameterValue;

        if (!_firstStarBeenShown && IsThirdStarFulfilled())
        {
            _firstStarBeenShown = true;
            OnThirdStarAchieved?.Invoke();
        }

        if (!_secondStarBeenShown && IsSecondStarFulfilled())
        {
            _secondStarBeenShown = true;
            OnSecondStarAchieved?.Invoke();
        }

        if (!_thirdStarBeenShown && IsFirstStarFulfilled())
        {
            _thirdStarBeenShown = true;
            OnFirstStarAchieved?.Invoke();
        }
    }

    private int ProgressParameterValue()
    {
        return _type switch
        {
            AchievementType.Damage                  => _gameStats.TotalDamage,

            AchievementType.ShieldDamage            => _gameStats.ShieldDamage,

            AchievementType.DamageReceived          => _gameStats.TotalDamageReceived,

            AchievementType.ShieldDamageReceived    => _gameStats.ShieldDamageReceived,

            AchievementType.EnemiesKilled           => _gameStats.EnemiesKilled,

            AchievementType.LVLReached              => _gameStats.Level,

            AchievementType.LightModeCompleted      => _gameStats.LightModeTimes,

            AchievementType.MediumModeCompleted     => _gameStats.MediumModeTimes,

            AchievementType.HardModeCompleted       => _gameStats.HardModeTimes,

            AchievementType.Died    => _gameStats.DeathCount,

            _ => throw new InvalidOperationException("No such Achievement type"),
        };
    }

    public AchievementType Type => _type;
    public string Name => _name;
    public int FirstStarPoints => _firstStarPoints;
    public int SecondStarPoints => _secondStarPoints;
    public int ThirdStarPoints => _thirdStarPoints;

    public string FirstStarDescription => _firstStarDescription;
    public string SecondStarDescription => _secondStarDescription;
    public string ThirdStarDescription => _thirdStarDescription;

    public int Progress => _progress;
}