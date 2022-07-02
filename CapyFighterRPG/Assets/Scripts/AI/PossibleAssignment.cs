using System;

public class PossibleAssignment : IComparable<PossibleAssignment>
{
    private EnemyAI _enemyAI;

    private float _score;
    private AIObject _possibleTaskDoer;
    private Fighter _fighter;

    private float[] _factorValues;
    private float[] _factorWeights;


    public Task TaskToDo { get; private set; }

    public PossibleAssignment(Task taskToDo, AIObject taskDoer, EnemyAI enemyAI)
    {
        if (taskToDo == null)
            throw new ArgumentNullException("Cannot instantiate assignment with a null task.");
        if (taskDoer == null)
            throw new ArgumentNullException("Cannot instantiate assignment with a null task doer.");

        TaskToDo = taskToDo;
        _possibleTaskDoer = taskDoer;
        _enemyAI = enemyAI;
        _fighter = _possibleTaskDoer.Fighter;
        FillTheArrayOfFactors();
        _score = EvaluateScore();
    }

    public void Assign()
    {
        if (TaskToDo.IsAssigned()) return;
        TaskToDo.Assign(_possibleTaskDoer);
        _possibleTaskDoer?.Assign(this);
    }

    private float EvaluateScore()
        => TaskToDo.Priority * 
        TaskToDo.PriorityMultiplier() * 
        this.PriorityMultiplier();
    

    private float PriorityMultiplier()
    {
        var res = 0f;
        var sumOfWeights = 0f;
        for(int i = 0; i < _factorValues.Length; ++i)
        {
            res += _factorWeights[i] * _factorValues[i];
            sumOfWeights += _factorWeights[i];
        }

        return res / sumOfWeights;
    }

    private void FillTheArrayOfFactors()
    {
        _factorValues = new float[]
        {
            PossibleDamage(),
            PossibleHPSave(),
            PossibleMPCost(),
        };

        _factorWeights = new float[]
        {
            _enemyAI.PossibleDamageWeight * DamageImportanceMultiplier(),
            _enemyAI.PossibleHPSaveWeight * HPImportanceMultiplier(),
            _enemyAI.PossibleMPCostWeight * MPImportanceMultiplier(),
        };

        if (_factorValues.Length != _factorValues.Length)
            throw new InvalidOperationException("Factor count must be equal to the count of factor weights.");
    }

    private float PossibleDamage()
    {
        return TaskToDo.Type switch
        {
            Task.TaskType.Attack 
            => _fighter.GetAttackDamageTo(TaskToDo.Target.Fighter), 
            Task.TaskType.SuperAttack 
            => _fighter.GetSuperAttackDamageTo(TaskToDo.Target.Fighter),
            Task.TaskType.EquipShield => 0f,
            Task.TaskType.Move => 0f,
            Task.TaskType.SkipTurn => 0f,
            _ => 0f
        };
    }

    private float PossibleHPSave()
    {
        return TaskToDo.Type switch
        {
            Task.TaskType.Attack => 0f,
            Task.TaskType.SuperAttack => 0f,
            Task.TaskType.EquipShield => _fighter.Unit.MaxShieldHP,
            Task.TaskType.Move => 0f,
            Task.TaskType.SkipTurn => 0f,
            _ => 0f
        };
    }

    private float PossibleMPCost()
    {
        return TaskToDo.Type switch
        {
            Task.TaskType.Attack => _fighter.Unit.AttackManaCost,
            Task.TaskType.SuperAttack => _fighter.Unit.SuperAttackManaCost,
            Task.TaskType.EquipShield => _fighter.Unit.ShieldManaCost,
            Task.TaskType.Move => 0f,
            Task.TaskType.SkipTurn => 0f,
            _ => 0f
        };
    }

    private float DamageImportanceMultiplier()
    {
        return 1f;
    }

    private float HPImportanceMultiplier()
    {
        return (1f - _fighter.HPPercentage());
    }

    private float MPImportanceMultiplier()
    {
        return (1f - _fighter.MPPercentage());
    }

    public int CompareTo(PossibleAssignment other)
    {
        return -_score.CompareTo(other._score);
    }
}