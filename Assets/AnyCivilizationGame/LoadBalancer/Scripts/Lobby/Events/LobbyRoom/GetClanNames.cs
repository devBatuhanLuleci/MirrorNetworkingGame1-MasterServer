using System;
using ACGAuthentication;
using UnityEngine;

public class GetClanNames : IResponseEvent {

    public string[] ClanNames;
    //public bool IsNewClanNameCreate;
    public void Invoke (EventManagerBase eventManagerBase, ClientPeer client) {

        var lobbyManager = eventManagerBase as LobbyManager;
        lobbyManager.GetClanNames(client,ClanNames,false);

    }

}
