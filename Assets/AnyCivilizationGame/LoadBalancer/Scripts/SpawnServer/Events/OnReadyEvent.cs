using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReadyEvent : IResponseEvent
{
    public int Port { get; private set; }
    public OnReadyEvent(int port)
    {
        Port = port;
    }
    public void Invoke(EventManagerBase authenticationManager, ClientPeer client)
    {
        var spawnServer = authenticationManager as SpawnServer;
        Debug.Log($"OnReadyEvent Invoked. Port: {Port}");
        spawnServer.SetRoomReady(Port, client);
    }
}
