using System;

public class State
{
    protected readonly StateMachine _stateMachine;
    public event Action OnEntered;
    public event Action OnExited;


    protected State(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual void EnterState()    { OnEntered?.Invoke(); }

    public virtual void UpdateLogic()   {}

    public virtual void ExitState()     { OnExited?.Invoke(); }
}