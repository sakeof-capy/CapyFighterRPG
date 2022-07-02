using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Serializer
{
    private static readonly string DirectoryPath = Application.dataPath + "/Saves";
    private static readonly string FileName = "progress.save";
    public static void Serialize(GameProgress progress)
    {
        //if(!Directory.Exists(DirectoryPath))
        //    Directory.CreateDirectory(DirectoryPath);
        //string SerializationPath = Path.Combine(DirectoryPath, FileName);
        //BinaryFormatter formatter = new BinaryFormatter();
        //FileStream stream = new FileStream(SerializationPath, FileMode.Create);
        //GameProgressSave save = new GameProgressSave(progress);
        //formatter.Serialize(stream, save);
        //stream.Close();

        Serialize(new GameProgressSave(progress));
    }

    public static void Serialize(GameProgressSave save)
    {
        if (!Directory.Exists(DirectoryPath))
            Directory.CreateDirectory(DirectoryPath);
        string SerializationPath = Path.Combine(DirectoryPath, FileName);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SerializationPath, FileMode.Create);
        //GameProgressSave save = new GameProgressSave(progress);
        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static GameProgressSave Deserialize()
    {
        string SerializationPath = Path.Combine(DirectoryPath, FileName);

        if (File.Exists(SerializationPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SerializationPath, FileMode.Open);
            GameProgressSave save = formatter.Deserialize(stream) as GameProgressSave;
            stream.Close();
            return save;
        }
        else
        {
            //Debug.LogError("File of save not found!");
            return null;
        }
    }

    public static bool SavedProgressExists() => File.Exists(Path.Combine(DirectoryPath, FileName));
}