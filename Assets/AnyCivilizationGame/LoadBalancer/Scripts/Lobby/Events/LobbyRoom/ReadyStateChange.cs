using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyStateChange : IResponseEvent
{
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        Debug.Log("ReadyStateChange Invoded");
        var lobbyManager = (LobbyManager)eventManagerBase;
        lobbyManager.ReadyState(client);
    }
}
