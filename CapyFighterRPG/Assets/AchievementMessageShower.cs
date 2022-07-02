using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AchievementMessageShower : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _firstStarImage;
    [SerializeField] private Image _secondStarImage;
    [SerializeField] private Image _thirdStarImage;
    [SerializeField] private Sprite _emptyStar;
    [SerializeField] private Sprite _filledStar;
    [SerializeField] private Text _name;

    [Header("Custom Parameters")]
    [SerializeField] private float _unfadeTimeSeconds;
    [SerializeField] private float _fadeTimeSeconds;

    private CanvasGroup _panelCanvasGroup;
    private Queue<Achievement> _toShow;
    private bool _isShowing;

    private void Awake()
    {
        _canvas.SetActive(true);
        _panelCanvasGroup = _panel.GetComponent<CanvasGroup>();
        _panelCanvasGroup.alpha = 0f;
        _name.text = "Achievement";
        _toShow = new Queue<Achievement>();
        _isShowing = false;
    }

    public void ShowAchievementMessage(Achievement ach)
    {
        _toShow.Enqueue(ach);
        if(!_isShowing)
            StartCoroutine(UnfadeFadeCoroutine());
    }

    private void SetFirstStarImageFulfilled(bool active)
    {
        if (active)
            _firstStarImage.sprite = _filledStar;
        else
            _firstStarImage.sprite = _emptyStar;
    }

    private void SetSecondStarImageFulfilled(bool active)
    {
        if (active)
            _secondStarImage.sprite = _filledStar;
        else
            _secondStarImage.sprite = _emptyStar;
    }

    private void SetThirdStarImageFulfilled(bool active)
    {
        if (active)
            _thirdStarImage.sprite = _filledStar;
        else
            _thirdStarImage.sprite = _emptyStar;
    }

    private IEnumerator UnfadeFadeCoroutine()
    {
        _isShowing = true;

        while(_toShow.Count != 0)
        {
            Achievement ach = _toShow.Dequeue();

            _name.text = ach.Name;
            SetFirstStarImageFulfilled(ach.IsFirstStarFulfilled());
            SetSecondStarImageFulfilled(ach.IsSecondStarFulfilled());
            SetThirdStarImageFulfilled(ach.IsThirdStarFulfilled());

            _panelCanvasGroup.alpha = 0f;

            while (_panelCanvasGroup.alpha < 1f)
            {
                _panelCanvasGroup.alpha += Time.deltaTime / _unfadeTimeSeconds;
                yield return null;
            }
            _panelCanvasGroup.alpha = 1f;

            while (_panelCanvasGroup.alpha > 0f)
            {
                _panelCanvasGroup.alpha -= Time.deltaTime / _fadeTimeSeconds;
                yield return null;
            }

            _panelCanvasGroup.alpha = 0f;
        }
    }
}
