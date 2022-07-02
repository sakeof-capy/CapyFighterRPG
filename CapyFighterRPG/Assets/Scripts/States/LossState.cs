public class LossState : State
{
    private readonly CombatController _controller;
    private readonly LossCanvasShower _lossCanvas;
    private readonly AudioManager _audioManager;

    public LossState(StateMachine stateMachine)
        : base(stateMachine)
    {
        _controller = (CombatController)stateMachine;
        _lossCanvas = _controller.GetComponent<LossCanvasShower>();
        _audioManager = _controller.GetComponent<AudioManager>();
    }

    public override void EnterState()
    {
        base.EnterState();
        _audioManager.PlayLossSound();
        _lossCanvas.ShowLossCanvas();
    }

    public override void ExitState()
    {
        base.ExitState();
        _lossCanvas.HideLossCanvas();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
