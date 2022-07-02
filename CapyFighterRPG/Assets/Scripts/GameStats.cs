[System.Serializable]
public class GameStats
{
    private int _totalDamage;
    private int _shieldDamage;
    private int _totalDamageReceived;
    private int _shieldDamageReceived;
    private int _enemiesKilled;
    private int _level;
    private int _lightModeTimes;
    private int _mediumModeTimes;
    private int _hardModeTimes;
    private int _deathCount;

    private int _levelProgress;

    public GameStats()
    {
        _totalDamage = 0;
        _shieldDamage = 0;
        _totalDamageReceived = 0;
        _shieldDamageReceived = 0;
        _enemiesKilled = 0;
        _lightModeTimes = 0;
        _mediumModeTimes = 0;
        _hardModeTimes = 0;
        _deathCount = 0;
    }

    public int TotalDamage
    {
        get => _totalDamage;
        set
        {
            if(value < 0) _totalDamage = 0;
            else _totalDamage = value;
        }
    }

    public int ShieldDamage
    {
        get => _shieldDamage;
        set
        {
            if (value < 0) _shieldDamage = 0;
            else _shieldDamage = value;
        }
    }

    public int TotalDamageReceived
    {
        get => _totalDamageReceived;
        set
        {
            if (value < 0) _totalDamageReceived = 0;
            else _totalDamageReceived = value;
        }
    }


    public int ShieldDamageReceived
    {
        get => _shieldDamageReceived;
        set
        {
            if (value < 0) _shieldDamageReceived = 0;
            else _shieldDamageReceived = value;
        }
    }

    public int EnemiesKilled
    {
        get => _enemiesKilled;
        set
        {
            if (value < 0) _enemiesKilled = 0;
            else _enemiesKilled = value;
        }
    }

    public int Level
    {
        get => _level;
        set
        {
            if (value < 0) _level = 0;
            else _level = value;
        }
    }

    public int LevelProgress
    {
        get => _levelProgress;
        set
        {
            if (value < 0) _levelProgress = 0;
            else _levelProgress = value;
        }
    }

    public int LightModeTimes
    {
        get => _lightModeTimes;
        set
        {
            if (value < 0) _lightModeTimes = 0;
            else _lightModeTimes = value;
        }
    }

    public int MediumModeTimes
    {
        get => _mediumModeTimes;
        set
        {
            if (value < 0) _mediumModeTimes = 0;
            else _mediumModeTimes = value;
        }
    }

    public int HardModeTimes
    {
        get => _hardModeTimes;
        set
        {
            if (value < 0) _hardModeTimes = 0;
            else _hardModeTimes = value;
        }
    }

    public int DeathCount
    {
        get => _deathCount;
        set
        {
            if (value < 0) _deathCount = 0;
            else _deathCount = value;
        }
    }
}