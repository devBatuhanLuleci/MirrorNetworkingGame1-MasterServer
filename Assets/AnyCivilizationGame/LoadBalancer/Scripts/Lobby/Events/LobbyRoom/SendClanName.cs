using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendClanName : IResponseEvent
{
    public string ClanName;

    public SendClanName(string clanName)
    {
        ClanName = clanName;
    }
    
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        var lobbyManager = eventManagerBase as LobbyManager;
        lobbyManager.SendClanName(client,ClanName);
    }
}
