using UnityEngine;

public class LossCanvasShower : MonoBehaviour
{
    [SerializeField] private GameObject _lossCanvas;

    private void Awake()
    {
        HideLossCanvas();
    }

    public void ShowLossCanvas() => _lossCanvas.SetActive(true);

    public void HideLossCanvas() => _lossCanvas.SetActive(false);
}