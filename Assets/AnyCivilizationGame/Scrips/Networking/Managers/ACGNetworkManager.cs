using kcp2k;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACGNetworkManager : NetworkManager
{
    private static ACGNetworkManager instance;
    public static ACGNetworkManager Instance { get { return instance; } }
    public override void Awake()
    {
        base.Awake();
        InitSingleton();
    }

    private void InitSingleton()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void InitServer()
    {
        StartServer();
        string msg = $" <color=green> Server listining on </color> {networkAddress}:{(transport as KcpTransport)?.Port}";
        Debug.Log(msg);
    }
}
