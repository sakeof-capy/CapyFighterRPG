using UnityEngine;
using System.Collections.Generic;
using System;

public class ProgressSerializer : MonoBehaviour
{
    private GameProgress _currentProgress;

    private void Awake()
    {
        _currentProgress = GetComponent<GameProgress>();
        LoadProgress();
    }

    public void SaveProgress()
    {
        Serializer.Serialize(_currentProgress);
    }

    private void LoadProgress()
    {
        GameProgressSave save = Serializer.Deserialize();
        if (save == null)
            throw new InvalidOperationException("There are no files to load from.");
        _currentProgress.Init(save);
    }

    //TODO make to do this when "new game" button is hit!!!
    private void SerializeAndLoadNewGameProgress()
    {
        var achievementsDatas = GetComponent<AchievementDataList>().Achievements;
        var listOfAchievements = new List<Achievement>();
        foreach (var data in achievementsDatas)
        {
            listOfAchievements.Add(new Achievement(data, null));
        }
        _currentProgress.Init(new GameProgressSave(listOfAchievements));
        SaveProgress();
    }
}