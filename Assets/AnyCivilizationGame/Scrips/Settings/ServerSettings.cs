using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class ServerSettings : SettingsBase<ServerSettings>
{
    public List<string> GameServers= new List<string>() { "40.117.113.124" };
}
