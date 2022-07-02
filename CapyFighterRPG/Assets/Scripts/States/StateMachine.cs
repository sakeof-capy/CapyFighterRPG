using System.Collections;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State CurrentState   { get; private set; }
    public State PreviousState  { get; private set; }


    protected void SetUp()
    {
        CurrentState = InitialState();
        CurrentState?.EnterState();
        PreviousState = null;
    }

    protected void UpdateLogic()
    {
        CurrentState?.UpdateLogic();
    }

    protected abstract State InitialState();

    public void SwitchState(State newState)
    {
        PreviousState = CurrentState;
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }

    public void SwitchToPreviousState() => SwitchState(PreviousState);

    public void SwitchStateInSeconds(State newState, float seconds)
    {
        StartCoroutine(SwitchStateCoroutine(newState, seconds));
    }

    private IEnumerator SwitchStateCoroutine(State newState, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        PreviousState = CurrentState;
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}