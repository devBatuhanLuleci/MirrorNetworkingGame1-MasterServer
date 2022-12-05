using System.IO;
using UnityEngine;

/// <summary>
/// Main class for json based config files
/// </summary>
/// <typeparam name="T"></typeparam>
public class SettingsBase<T> where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null) instance = Init();
            return instance;
        }
    }


    private static T Init()
    {
        string dataPath = Application.dataPath.Replace('/', '\\');
        string workingDirevtory = Path.Combine(Directory.GetParent(dataPath).FullName, @"Configs");

        var tmp = new T();
        var filePath = Path.Combine(workingDirevtory, tmp.GetType().Name + ".config");

        if (!File.Exists(filePath)) { Directory.CreateDirectory(workingDirevtory); }
        return FileManager.Read<T>(filePath);
    }

}