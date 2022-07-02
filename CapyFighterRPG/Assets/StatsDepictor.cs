using UnityEngine;
using UnityEngine.UI;

public class StatsDepictor : MonoBehaviour
{
    [Header("Lvl Bar")]
    [SerializeField] private Text _lvlText;
    [SerializeField] private ProgressBar _lvlBar;
    [Header("Stats Values")]
    [SerializeField] private Text _damage;
    [SerializeField] private Text _superDamage;
    [SerializeField] private Text _maxHP;
    [SerializeField] private Text _maxMP;
    [SerializeField] private Text _shieldHP;
    [SerializeField] private Text _MPRegainRate;
    [SerializeField] private Text _attackMC;
    [SerializeField] private Text _superAttackMC;
    [SerializeField] private Text _shieldMC;

    [Header("Capybara Scriptable Obj")]
    [SerializeField] private UnitData _data;

    private GameStats _stats;

    private void Awake()
    {
        var progress = Serializer.Deserialize();
        _stats = progress.Stats;
    }

    private void Start()
    {
        AssignFieldValuesToTexts();
        FillLvlBar();
    }

    private void FillLvlBar()
    {
        _lvlText.text = "LVL " + _stats.Level;
        _lvlBar.SetProgress(GetLevelPercentage(_stats.Level));
    }

    private float GetLevelPercentage(int Level) => _stats.LevelProgress / ExpForLevel(Level + 1);

    private float ExpForLevel(int lvl) => lvl == 0 ? 0 : 1000 * (lvl - 1) + 200;

    private void AssignFieldValuesToTexts()
    {
        _damage.text = AttackDamage.ToString();
        _superDamage.text = SuperAttackDamage.ToString();
        _maxHP.text = MaxHP.ToString();
        _maxMP.text = MaxMP.ToString();
        _shieldHP.text = MaxShieldHP.ToString();
        _MPRegainRate.text = ManaRegainRate.ToString();

        _attackMC.text = AttackManaCost.ToString();
        _superAttackMC.text = SuperAttackManaCost.ToString();
        _shieldMC.text = ShieldManaCost.ToString();
    }

    private float LvlCoeff() => 1f + 0.1f * _stats.Level;
    public int MaxHP => (int)(_data.MaxHP * LvlCoeff());
    public int MaxMP => (int)(_data.MaxMP * LvlCoeff());
    public int AttackDamage => (int)(_data.AttackDamage * LvlCoeff());
    public int AttackManaCost => _data.AttackManaCost;
    public int SuperAttackDamage => (int)(_data.SuperAttackDamage * LvlCoeff());
    public int SuperAttackManaCost => _data.SuperAttackManaCost;
    public int MaxShieldHP => (int)(_data.MaxShieldHP * LvlCoeff());
    public int ShieldManaCost => _data.ShieldManaCost;
    public int ManaRegainRate => (int)(_data.ManaRegainRate * LvlCoeff());
}