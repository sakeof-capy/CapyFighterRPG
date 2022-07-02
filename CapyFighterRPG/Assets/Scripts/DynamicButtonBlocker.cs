using UnityEngine;
using UnityEngine.UI;
using System;

public class DynamicButtonBlocker : MonoBehaviour
{
    public Predicate<int> CanInteract;
    private Button _button;
    private const int _Arg = 0;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        if (CanInteract.Invoke(_Arg))
        {
            _button.interactable = true;
        }
        else
        {
            _button.interactable = false;
        }
    }
}
