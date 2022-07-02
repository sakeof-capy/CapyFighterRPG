using UnityEngine;
using System;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject _fill;

    private RectTransform _fillTransform;
    private Vector2 _currentScale;

    private void Awake()
    {
        _fillTransform = _fill.GetComponent<RectTransform>();
        _currentScale = _fillTransform.localScale;
    }

    public void SetProgress(float progress)
    {
        if (progress < 0f || progress > 1f)
            throw new InvalidOperationException("Progress varies beetween 0f and 1f.");
        
        _fillTransform.localScale = new Vector2(progress, _currentScale.y);
    }
}
