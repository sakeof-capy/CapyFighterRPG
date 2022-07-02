using UnityEngine;

public class EnemyTurnState : PausableState
{
    private readonly CombatController _controller;
    private readonly EnemyAI _enemyAI;
    private readonly MessageTextShower _messageTextShower;
    private bool _theTurnIsUsed;

    private bool IsFaded => _messageTextShower.IsFaded;


    public EnemyTurnState(CombatController stateMachine)
        : base(stateMachine)
    {
        _controller = stateMachine;
        _enemyAI = _controller.GetComponent<EnemyAI>();
        _messageTextShower = _controller.GetComponent<MessageTextShower>();
        _theTurnIsUsed = true;
    }

    public override void EnterState()
    {
        base.EnterState();
        _controller.RefreshSelectedSlots();

        if (_isPaused)
            _isPaused = false;
        else
        {
            _messageTextShower.ShowMessage("Enemy's Turn",
                _controller.MessageUnfadeDuration, _controller.MessageFadeDuration);
            _theTurnIsUsed = false;
        }

        foreach (var fighter in _controller.EnemiesToFighters.Values)
        {
            fighter.RegainMana();
        }
    }

    public override void ExitState()
    {
        base.ExitState(); 
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_theTurnIsUsed || !IsFaded) return;

        Task taskToDo = _enemyAI.NextTurnTask();
        taskToDo.Do();
        if (_controller.HeroCount == 0)
            _controller.SwitchStateInSeconds(_controller.LossState, _controller.TurnDurationSeconds);
        else if(taskToDo.Type == Task.TaskType.Move)
            _controller.SwitchState(_controller.EnemyMovingState);
        else
            _controller.SwitchStateInSeconds(_controller.HeroTurnState, _controller.TurnDurationSeconds);
        _theTurnIsUsed = true;
    }
}