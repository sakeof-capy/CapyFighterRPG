using UnityEngine;

public class VictoryCanvasShower : MonoBehaviour
{
    [SerializeField] private GameObject _victoryCanvas;

    private void Awake()
    {
        HideVictoryCanvas();
    }

    public void ShowVictoryCanvas() => _victoryCanvas.SetActive(true);

    public void HideVictoryCanvas() => _victoryCanvas.SetActive(false);
}
