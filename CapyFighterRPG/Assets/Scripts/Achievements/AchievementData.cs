using UnityEngine;

[CreateAssetMenu(menuName = "Achievement", fileName = "Achievement")]
public class AchievementData : ScriptableObject
{
    [SerializeField] private AchievementType _type;
    [SerializeField] private string _name;

    [Header("Points to reach each star")]
    [SerializeField] private int _firstStarPoints;
    [SerializeField] private int _secondStarPoints;
    [SerializeField] private int _thirdStarPoints;

    [Header("Each star's achievement description")]
    [SerializeField] private string _firstStarDescription;
    [SerializeField] private string _secondStarDescription;
    [SerializeField] private string _thirdStarDescription;

    public AchievementType Type => _type;
    public int FirstStarPoints => _firstStarPoints;
    public int SecondStarPoints => _secondStarPoints;
    public int ThirdStarPoints => _thirdStarPoints;
    public string FirstStarDescription => _firstStarDescription;
    public string SecondStarDescription => _secondStarDescription;
    public string ThirdStarDescription => _thirdStarDescription;
    public string Name => _name;
}