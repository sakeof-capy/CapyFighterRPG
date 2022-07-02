using UnityEngine;

public class PauseShower : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;

    private void Awake()
    {
        HidePause();
    }

    public void ShowPause() => _pauseCanvas.SetActive(true);

    public void HidePause() => _pauseCanvas.SetActive(false);
}
