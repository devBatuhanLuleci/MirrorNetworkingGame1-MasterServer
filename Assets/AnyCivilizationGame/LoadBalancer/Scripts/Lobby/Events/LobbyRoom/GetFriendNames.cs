using System;
using ACGAuthentication;
using UnityEngine;

public class GetFriendNames : IResponseEvent
{
    public string[] FriendNames;
   
    public void Invoke (EventManagerBase eventManagerBase, ClientPeer client) {

        var lobbyManager = eventManagerBase as LobbyManager;
        lobbyManager.GetFriendNames(client,FriendNames,false);

    }
}
