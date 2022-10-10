using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLobbyRoom : IResponseEvent
{
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        var lobbyManager = eventManagerBase as LobbyManager;
        lobbyManager.StartMatch(client);
    }
}
