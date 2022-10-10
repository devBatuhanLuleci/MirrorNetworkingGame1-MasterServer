using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WalletConnectSharp.Core.Events;

public class CreateLobbyRoom : IResponseEvent
{
 
    public CreateLobbyRoom()
    {
    }
    public void Invoke(EventManagerBase authenticationManager, ClientPeer client)
    {
        Debug.Log("StartMatch Invoked.");
        var lobbyManager = authenticationManager as LobbyManager;
        lobbyManager.CreateLobbyRoom(client);
    }
}
