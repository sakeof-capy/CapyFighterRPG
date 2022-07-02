using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HintShower : MonoBehaviour
{
    [SerializeField] private GameObject _hintCanvas;
    [SerializeField] private GameObject _hintPanel;
    [SerializeField] private Text _hintText;
    [SerializeField] private float _unfadeTimeSeconds;
    [SerializeField] private float _fadeTimeSeconds;
    private CanvasGroup _panelCanvasGroup;

    private void Awake()
    {
        _hintCanvas.SetActive(true);
        _panelCanvasGroup = _hintPanel.GetComponent<CanvasGroup>();
        _panelCanvasGroup.alpha = 0f;
        _hintText.text = "Hint";
    }

    public void ShowHint(string hint)
    {
        StopAllCoroutines();
        _hintText.text = hint;
        StartCoroutine(UnfadeFadeCoroutine());
    }
    
    private IEnumerator UnfadeFadeCoroutine()
    {
        _panelCanvasGroup.alpha = 0f;

        while (_panelCanvasGroup.alpha < 1f)
        {
            _panelCanvasGroup.alpha += Time.deltaTime / _unfadeTimeSeconds;
            yield return null;
        }
        _panelCanvasGroup.alpha = 1f;

        while(_panelCanvasGroup.alpha > 0f)
        {
            _panelCanvasGroup.alpha -= Time.deltaTime / _fadeTimeSeconds;
            yield return null;
        }

        _panelCanvasGroup.alpha = 0f;
    }
}