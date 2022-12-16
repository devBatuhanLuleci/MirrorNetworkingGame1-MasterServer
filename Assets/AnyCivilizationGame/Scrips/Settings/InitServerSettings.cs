using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitServerSettings : MonoBehaviour
{
    public ServerSettings serverSettings;
    void Awake()
    {
        serverSettings = ServerSettings.Instance;
    }

}
