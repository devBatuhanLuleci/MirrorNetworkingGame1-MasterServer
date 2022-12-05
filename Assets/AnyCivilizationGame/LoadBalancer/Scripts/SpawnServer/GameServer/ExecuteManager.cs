using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;


public class ExecuteManager
{

    public static Process ExecuteCommand(string commands)
    {
        string dataPath = Application.dataPath.Replace('/', '\\');
#if UNITY_EDITOR
        UnityEngine.Debug.Log("dataPath" + dataPath);
        string workingDirevtory = Path.Combine(Directory.GetParent(dataPath).FullName, @"Builds\Windows\GameServer\GameServer.exe");

#else
        string   workingDirevtory = Path.Combine(Directory.GetParent(Directory.GetParent(dataPath).FullName).FullName, @"GameServer\GameServer.exe");

#endif
        UnityEngine.Debug.Log("workingDirevtory: " + workingDirevtory + " Commands: "+commands);

        try
        {   
            ProcessStartInfo ps1 = new ProcessStartInfo(workingDirevtory);
            ps1.Arguments = commands; 
            return Process.Start(ps1);

        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Run error" + e.ToString()); // or throw new Exception
            return null;
        }

    }


}
