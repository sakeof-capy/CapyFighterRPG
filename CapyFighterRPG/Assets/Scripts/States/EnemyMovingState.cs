using UnityEngine;

public class EnemyMovingState : PausableState
{
    private readonly CombatController _controller;
    private Mover _currentlyMovingEnemy;
    private bool _theTurnIsUsed;

    public EnemyMovingState(CombatController stateMachine)
        : base(stateMachine)
    {
        _controller = stateMachine;
    }

    public override void EnterState()
    {
        base.EnterState();
        var selectedUnit = _controller.GetEnemyAtSlot(_controller.SelectedEnemySlot);
        _currentlyMovingEnemy = selectedUnit.GetComponent<Mover>();
        _theTurnIsUsed = false;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_theTurnIsUsed) return;

        if (!_currentlyMovingEnemy.IsMoving)
        {
            _controller.SwitchStateInSeconds(_controller.HeroTurnState, _controller.TurnDurationSeconds);
            _theTurnIsUsed = true;
        }
    }
}