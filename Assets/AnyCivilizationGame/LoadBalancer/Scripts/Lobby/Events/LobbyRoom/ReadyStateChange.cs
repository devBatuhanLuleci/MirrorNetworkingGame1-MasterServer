using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Odadaki bir oyuncunun haz?r oldu?u bilgisini odaya iletir. 
/// </summary>
public class ReadyStateChange : IResponseEvent
{
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        Debug.Log("ReadyStateChange Invoded");
        var lobbyManager = (LobbyManager)eventManagerBase;
        lobbyManager.ReadyState(client);
    }
}
