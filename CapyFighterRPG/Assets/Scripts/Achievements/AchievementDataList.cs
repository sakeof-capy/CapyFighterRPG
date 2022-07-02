using UnityEngine;
using System.Collections.Generic;

public class AchievementDataList : MonoBehaviour
{
    [SerializeField] private List<AchievementData> _achievements;

    public List<AchievementData> Achievements => _achievements;
}
