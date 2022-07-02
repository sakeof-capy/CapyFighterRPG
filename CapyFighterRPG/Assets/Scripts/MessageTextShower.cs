using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageTextShower : MonoBehaviour
{
    [SerializeField] private GameObject _textCanvas;
    [SerializeField] private Text _text;

    public bool IsFaded { get; private set; }

    private void Awake()
    {
        _textCanvas.SetActive(true);
        _text.text = string.Empty;
        IsFaded = true;
    }

    public void ShowMessage(string message, float unfadeDuration, float fadeDuration)
    {
        _text.text = message;
        Color textColor = _text.color;
        textColor.a = 1f;
        _text.color = textColor;
        StartCoroutine(UnfadeAndFadeCoroutine(unfadeDuration, fadeDuration));
    }

    private IEnumerator UnfadeAndFadeCoroutine(float unfadeDuration, float fadeDuration)
    {
        IsFaded = false;
        float timePassed = 0f;
        float initialAlpha = _text.color.a;
        Color textColor;

        while (timePassed < unfadeDuration)
        {
            timePassed += Time.deltaTime;
            textColor = _text.color;
            textColor.a = initialAlpha * timePassed / unfadeDuration;
            _text.color = textColor;
            yield return null;
        }

        StartCoroutine(FadeCoroutine(fadeDuration));
    }

    private IEnumerator FadeCoroutine(float duration)
    {
        float timePassed = 0f;
        float initialAlpha = _text.color.a;
        Color textColor;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            textColor = _text.color;
            textColor.a = initialAlpha * (duration - timePassed) / duration;
            _text.color = textColor;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        IsFaded = true;
    }
}
