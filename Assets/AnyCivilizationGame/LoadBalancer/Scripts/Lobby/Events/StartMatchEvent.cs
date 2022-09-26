using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WalletConnectSharp.Core.Events;

public class StartMatchEvent : IResponseEvent
{
 
    public StartMatchEvent()
    {
    }
    public void Invoke(EventManagerBase authenticationManager, ClientPeer client)
    {
        Debug.Log("StartMatch Invoked.");
        var lobbyManager = authenticationManager as LobbyManager;
        lobbyManager.StartMatchLobby(client);
    }
}
