using UnityEngine;

public class PausableState : State
{
    private readonly CombatController _controller;
    private readonly PauseShower _pauseShower;
    protected bool _isPaused;

    public PausableState(CombatController stateMachine) 
        : base(stateMachine) 
    {
        _controller = stateMachine;
        _pauseShower = _controller.GetComponent<PauseShower>();
        _isPaused = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = true;
            _pauseShower.ShowPause();
            _controller.PauseCombat();
            _stateMachine.SwitchState(_controller.PauseState);
        }
    }
}