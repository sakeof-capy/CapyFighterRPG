using UnityEngine;
using UnityEngine.UI;

public class UnitInfo : MonoBehaviour
{
    [SerializeField] private GameObject _HPBarPrefab;
    [SerializeField] private GameObject _MPBarPrefab;
    [SerializeField] private Image     _avatarImage;

    private ProgressBar _HPBar;
    private ProgressBar _MPBar;

    private void Awake()
    {
        _HPBar = _HPBarPrefab.GetComponent<ProgressBar>();
        _MPBar = _MPBarPrefab.GetComponent<ProgressBar>();
    }

    public void SetAvatarImage(Sprite image) => _avatarImage.sprite = image;

    public void SetHP(float amount)
    {
        _HPBar.SetProgress(amount);
    }

    public void SetMP(float amount)
    {
        _MPBar.SetProgress(amount);
    }
}