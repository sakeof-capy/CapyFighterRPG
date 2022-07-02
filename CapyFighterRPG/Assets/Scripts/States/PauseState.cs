using UnityEngine;

public class PauseState : State
{
    private readonly CombatController _controller;
    private readonly PauseShower _pauseShower;

    public PauseState(StateMachine stateMachine)
        : base(stateMachine)
    {
        _controller = (CombatController) stateMachine;
        _pauseShower = _controller.GetComponent<PauseShower>();
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Pause Entered");
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("Pause Exited");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseShower.HidePause();
            _controller.UnpauseCombat();
            _stateMachine.SwitchToPreviousState();
        }
    }
}
