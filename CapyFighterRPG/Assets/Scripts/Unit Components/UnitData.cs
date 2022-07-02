using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData")]
public class UnitData : ScriptableObject
{
    [SerializeField] private RuntimeAnimatorController _animator;
    [SerializeField] private Sprite _avatarIcon;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _maxMP;
    [SerializeField] private int _attackDamage;
    [SerializeField] private int _attackManaCost;
    [SerializeField] private int _superAttackDamage;
    [SerializeField] private int _superAttackManaCost;
    [SerializeField] private int _maxShieldHP;
    [SerializeField] private int _shieldManaCost;
    [SerializeField] private int _manaRegainRate;
    [SerializeField] private Vector3 _scale;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _superAttackSound;
    [SerializeField] private AudioClip _shieldEquipedSound;
    [SerializeField] private AudioClip _shieldBrokenSound;
    [SerializeField] private AudioClip _shieldHurtSound;
    [SerializeField] private AudioClip _movingSound;
    [SerializeField] private AudioClip _hurtSound;
    [SerializeField] private AudioClip _dieSound;


    public RuntimeAnimatorController Animator => _animator;
    public Sprite AvatarIcon => _avatarIcon;
    public Vector3 Scale => _scale;
    public int MaxHP => _maxHP;
    public int MaxMP => _maxMP;
    public int AttackDamage => _attackDamage;
    public int AttackManaCost => _attackManaCost;
    public int SuperAttackDamage => _superAttackDamage;
    public int SuperAttackManaCost => _superAttackManaCost;
    public int MaxShieldHP => _maxShieldHP;
    public int ShieldManaCost => _shieldManaCost;
    public int ManaRegainRate => _manaRegainRate;
    public AudioClip AttackSound => _attackSound;
    public AudioClip SuperAttackSound => _superAttackSound;
    public AudioClip ShieldEquipedSound => _shieldEquipedSound;
    public AudioClip ShieldBrokenSound => _shieldBrokenSound;
    public AudioClip ShieldHurtSound => _shieldHurtSound;
    public AudioClip MovingSound => _movingSound;
    public AudioClip HurtSound => _hurtSound;
    public AudioClip DieSound => _dieSound;
}   