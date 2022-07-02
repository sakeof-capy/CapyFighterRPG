using UnityEngine;
using System;

public class Fighter : MonoBehaviour
{
    private Unit _unit;
    private CombatController _controller;
    private FieldMetricSpace _fieldMetricSpace;
    private int _currentHP;
    private int _currentMP;
    private int _shieldHP;
    private bool _isShielded;

    #region Events
    public event Action<float, int> OnDamageReceived;
    public event Action<float, int> OnShieldDamageReceived;
    public event Action<float> OnManaAmountChanged;
    public event Action<float> OnAttacked;
    public event Action<float> OnSuperAttacked;
    public event Action OnDied;
    public event Action OnShieldEquiped;
    public event Action OnShieldBroken;
    public event Action HurtAnimation;
    public event Action ShieldHurtAnimation;

    public event Action<int> OnTotalDamageReceived;
    public event Action<int> OnTotalShieldDamageReceived;

    #endregion

    #region Properties
    public Unit Unit => _unit;
    #endregion

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        GameObject CombatController = GameObject.FindGameObjectWithTag("CombatController");
        _controller = CombatController.GetComponent<CombatController>();
        _fieldMetricSpace = _controller.GetComponent<FieldMetricSpace>();
    }

    private void Start()
    {
        _currentHP = _unit.MaxHP;
        _currentMP = _unit.MaxMP;
        _shieldHP = 0;
        _isShielded = false;
    }

    public void ReceiveDamage(int damage)
    {
        OnTotalDamageReceived?.Invoke(damage);
        var totalDamage = damage;

        if (_isShielded)
            ReceiveShieldDamage(ref totalDamage);

        SpendHealth(totalDamage);

        if (IsDead())
            OnDied?.Invoke();
        else if (_isShielded)
        {
            OnDamageReceived?.Invoke(HPPercentage(), totalDamage);
        }
        else
        {
            HurtAnimation?.Invoke();
            OnDamageReceived?.Invoke(HPPercentage(), totalDamage);
        }
    }

    public void Attack(Fighter victim)
    {
        SpendMana(_unit.AttackManaCost);
        victim.ReceiveDamage(GetAttackDamageTo(victim));
        OnAttacked?.Invoke(MPPercentage());
    }

    public void SuperAttack(Fighter victim)
    {
        SpendMana(_unit.SuperAttackManaCost);
        victim.ReceiveDamage(GetSuperAttackDamageTo(victim));
        OnSuperAttacked?.Invoke(MPPercentage());
    }

    public void Die()
    {
        OnDied?.Invoke();
    }

    public int GetAttackDamageTo(Fighter victim)
    {
        var thisSlot = _controller.GetUnitSlotByFighter(this);
        var victimSlot = _controller.GetUnitSlotByFighter(victim);

        var damageMultiplier = 1 / _fieldMetricSpace.Metric(thisSlot, victimSlot);
        return (int)(damageMultiplier * _unit.AttackDamage);
    }

    public int GetSuperAttackDamageTo(Fighter victim)
    {
        var thisSlot = _controller.GetUnitSlotByFighter(this);
        var victimSlot = _controller.GetUnitSlotByFighter(victim);

        var damageMultiplier = 1 / _fieldMetricSpace.Metric(thisSlot, victimSlot);
        return (int)(damageMultiplier * _unit.SuperAttackDamage);
    }

    public void EquipShield()
    {
        SpendMana(_unit.ShieldManaCost);
        _isShielded = true;
        _shieldHP = _unit.MaxShieldHP;
        OnShieldEquiped?.Invoke();
    }

    public void BreakTheShield()
    {
        _isShielded = false;
        _shieldHP = 0;
        OnShieldBroken?.Invoke();
    }

    public bool IsDead() => _currentHP < Mathf.Epsilon;

    public void SpendHealth(int hp)
        => _currentHP = hp >= _currentHP ? 0 : _currentHP - hp;
    

    public void SpendMana(int mp)
    {
        if (!HasEnoughManaFor(mp))
            throw new InvalidOperationException("Not enough mana for the operation.");
        _currentMP -= mp;
        OnManaAmountChanged?.Invoke(MPPercentage());
    }

    public void ReceiveShieldDamage(ref int totalDamage)
    {
        OnTotalShieldDamageReceived?.Invoke(totalDamage);
        if(totalDamage >= _shieldHP)
        {
            totalDamage -= _shieldHP;
            BreakTheShield();
        }
        else //All Damage Absorbed
        {
            _shieldHP -= totalDamage;
            OnShieldDamageReceived?.Invoke(ShielHPPercentage(), totalDamage);
            totalDamage = 0;
            ShieldHurtAnimation?.Invoke();
        }
    }

    public bool CanAttack() => HasEnoughManaFor(_unit.AttackManaCost);

    public bool CanSuperAttack() => HasEnoughManaFor(_unit.SuperAttackManaCost);

    public bool CanEquipShield() => HasEnoughManaFor(_unit.ShieldManaCost) && !_isShielded;

    public float HPPercentage() => (float)_currentHP / _unit.MaxHP;

    public float MPPercentage() => (float)_currentMP / _unit.MaxMP;

    public float ShielHPPercentage() => (float)_shieldHP / _unit.MaxShieldHP;

    public bool HasEnoughManaFor(int manaCost) => _currentMP >= manaCost;

    public void RegainMana()
    {
        if (_currentMP + _unit.ManaRegainRate >= _unit.MaxMP)
            _currentMP = _unit.MaxMP;
        else
            _currentMP = _currentMP + _unit.ManaRegainRate;
        OnManaAmountChanged?.Invoke(MPPercentage());
    }
}