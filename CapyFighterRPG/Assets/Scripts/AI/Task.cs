using System;

public class Task
{
    private EnemyAI _enemyAI;

    private AIObject _taskDoer;
    private AIObject _target;
    private TaskType _type;
    private bool _isAssigned;
    private Fighter _targetFighter;
    private float[] _factorValues;
    private float[] _factorWeights;

    public int Priority { get; private set; }
    public AIObject TaskDoer => _taskDoer;
    public AIObject Target => _target;
    public TaskType Type => _type;

    //The order matters
    public enum TaskType
    {
        SkipTurn = 1,
        Move,
        Attack,
        SuperAttack,
        EquipShield,
    }

    public Task(EnemyAI enemyAI, TaskType taskType, AIObject target = null)
    {
        _enemyAI = enemyAI;
        _type = taskType;
        Priority = (int)taskType;
        _isAssigned = false;
        _targetFighter = target?.GetComponent<Fighter>();
        FillFactorArrays();

        switch (taskType)
        {
            case TaskType.Attack:
                _target = target ?? throw new InvalidOperationException("Attack requires a target.");
                break;
            case TaskType.SuperAttack:
                _target = target ?? throw new InvalidOperationException("Super attack requires a target.");
                break;
            case TaskType.EquipShield:
                break;
            case TaskType.Move:
                break;
            case TaskType.SkipTurn:
                break;
            default:
                throw new InvalidOperationException("No such task type.");
        }
    }

    private void FillFactorArrays()
    {
        _factorValues = new float[]
        {
            TargetHPSaturation(),
            TargetMPSaturation(),
        };

        _factorWeights = new float[]
        {
            _enemyAI.HeroHPWeight,
            _enemyAI.HeroMPWeight,
        };
    }

    private float TargetHPSaturation()
    {
        return Type switch
        {
            TaskType.Attack => _targetFighter.HPPercentage(),
            TaskType.SuperAttack => _targetFighter.HPPercentage(),
            TaskType.EquipShield => 1f,
            TaskType.Move => 1f,
            TaskType.SkipTurn => 1f,
            _ => throw new InvalidOperationException("No such task type."),
        };
    }

    private float TargetMPSaturation()
    {
        return Type switch
        {
            TaskType.Attack => _targetFighter.MPPercentage(),
            TaskType.SuperAttack => _targetFighter.MPPercentage(),
            TaskType.EquipShield => 1f,
            TaskType.Move => 1f,
            TaskType.SkipTurn => 1f,
            _ => throw new InvalidOperationException("No such task type."),
        };
    }

    public void Do()
    {
        if (!IsAssigned())
            throw new InvalidOperationException("Doint an unassigned task is impossible.");

        switch(_type)
        {
            case TaskType.Attack:
                _taskDoer.Fighter.Attack(_target.Fighter);
                break;
            case TaskType.SuperAttack:
                _taskDoer.Fighter.SuperAttack(_target.Fighter);
                break;
            case TaskType.EquipShield:
                _taskDoer.Fighter.EquipShield();
                break;
            case TaskType.Move:
                //TODO
                break;
            case TaskType.SkipTurn:
                break;
            default:
                throw new InvalidOperationException("No such task type.");
        }
    }

    public void Assign(AIObject obj)
    {
        _taskDoer = obj;
        _isAssigned = true;
    }

    public bool IsAssigned() => _isAssigned;

    public float PriorityMultiplier()
    {
        var res = 0f;
        var sumOfWeights = 0f;
        for (int i = 0; i < _factorValues.Length; ++i)
        {
            res += _factorWeights[i] * _factorValues[i];
            sumOfWeights += _factorWeights[i];
        }

        return 1f + res / sumOfWeights;
    }
}