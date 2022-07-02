using UnityEngine;

public class PlatformFader : MonoBehaviour
{
    [SerializeField] float _fadeAlpha;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Fade()
    {
        Color color = _spriteRenderer.color;
        color.a = _fadeAlpha;
        _spriteRenderer.color = color;
    }

    public void Unfade()
    {
        Color color = _spriteRenderer.color;
        color.a = 1f;
        _spriteRenderer.color = color;
    }
}
