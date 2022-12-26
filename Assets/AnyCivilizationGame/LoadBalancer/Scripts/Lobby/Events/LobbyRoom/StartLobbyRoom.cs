using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Tüm oyuncular?n haz?r oldu?u bir odada kurucunun verdi?i start komutudur. 
/// </summary>
public class StartLobbyRoom : IResponseEvent
{
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        var lobbyManager = eventManagerBase as LobbyManager;
        lobbyManager.StartMatch(client);
    }
}
