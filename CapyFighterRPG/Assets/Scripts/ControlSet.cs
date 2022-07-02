using UnityEngine;
using UnityEngine.UI;
using System;

public class ControlSet : MonoBehaviour
{
    [SerializeField] private GameObject _controlsCanvas;
    [SerializeField] private Button _moveUp;
    [SerializeField] private Button _moveDown;
    [SerializeField] private Button _attack;
    [SerializeField] private Button _equipShield;
    [SerializeField] private Button _superAttack;
    [SerializeField] private Button _skipTurn;

    public Button MoveUpButton => _moveUp;
    public Button MoveDownButton => _moveDown;
    public Button AttackButton => _attack;
    public Button EquipShieldButton => _equipShield;
    public Button SuperAttackButton => _superAttack;
    public Button SkipTurnButton => _skipTurn;

    public bool IsShown { get; private set; }

    private void Awake()
    {
        Disappear();
    }

    public void Appear()
    {
        IsShown = true;
        _controlsCanvas.SetActive(true);
    }   

    public void Disappear()
    {
        IsShown = false;
        _controlsCanvas.SetActive(false);
    }

}
