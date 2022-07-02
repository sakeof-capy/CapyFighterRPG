using System.Collections.Generic;

[System.Serializable]
public class GameProgressSave
{
    public List<Achievement> Achievements;
    public GameStats Stats;

    public GameProgressSave(GameProgress progress)
    {
        Achievements = progress.Achievements;
        Stats = progress.GameStats;
    }

    public GameProgressSave(List<Achievement> achievements)
    {
        Achievements = achievements;
        Stats = new GameStats();
    }
}