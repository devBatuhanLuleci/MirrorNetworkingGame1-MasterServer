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

    /// <summary>
    /// Read <typeparamref name="T"/> based class file.
    /// or create a new file when file is not found
    /// </summary>
    private static T Init()
    {
        string dataPath = Application.dataPath.Replace('/', '\\');
        string workingDirevtory = Path.Combine(Directory.GetParent(dataPath).FullName, @"Configs");
        var type = typeof(T);
        var fileName = type.FullName + ".config";
        // Get prefix from Environment.
        // when T is not an Environment instance
        if (type != typeof(Envairment))
        {
            fileName = Envairment.Instance.DevelopmentMode + fileName;
        }

        var filePath = Path.Combine(workingDirevtory, fileName);

        if (!File.Exists(filePath)) { Directory.CreateDirectory(workingDirevtory); }
        return FileManager.Read<T>(filePath);
    }

}
