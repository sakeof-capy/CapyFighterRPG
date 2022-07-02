using UnityEngine;

public class HeroMovingState : PausableState
{
    private readonly CombatController _controller;
    private Mover _currentlyMovingHero;
    private bool _theTurnIsUsed;

    public HeroMovingState(CombatController stateMachine)
        : base(stateMachine)
    {
        _controller = stateMachine;
    }

    public override void EnterState()
    {
        base.EnterState();
        var selectedHero =  _controller.GetHeroAtSlot(_controller.SelectedHeroSlot);
        _currentlyMovingHero = selectedHero.GetComponent<Mover>();
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

        if (!_currentlyMovingHero.IsMoving)
        {
            _controller.SwitchStateInSeconds(_controller.EnemyTurnState, _controller.TurnDurationSeconds);
            _theTurnIsUsed = true;
        }
    }
}