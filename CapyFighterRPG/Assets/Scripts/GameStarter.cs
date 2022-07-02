using UnityEngine;
using System.Collections.Generic;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private List<AchievementData> _achievementDatas;

    private const string _tagButtonToBlockProvidedNoSavings = "ButtonToBlockProvidedNoSavings";

    private void Awake()
    {
        AssignPredicateBlocksToButtons();
    }

    public void SerializeNewGameProgress()
    {
        var listOfAchievements = new List<Achievement>();
        foreach (var data in _achievementDatas)
        {
            listOfAchievements.Add(new Achievement(data, null));
        }
        Serializer.Serialize(new GameProgressSave(listOfAchievements));
    }

    public bool SavedProgressExists() => Serializer.SavedProgressExists();

    private void AssignPredicateBlocksToButtons()
    {
        var buttonObjs = GameObject.FindGameObjectsWithTag(_tagButtonToBlockProvidedNoSavings);
        foreach(var buttonObj in buttonObjs)
        {
            buttonObj.GetComponent<DynamicButtonBlocker>().CanInteract = _ => SavedProgressExists();
        }
    }
}
