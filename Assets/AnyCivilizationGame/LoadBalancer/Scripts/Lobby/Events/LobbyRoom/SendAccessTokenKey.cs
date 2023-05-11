using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAccessTokenKey : IResponseEvent
{
    public string AccessTokenKey;

    public SendAccessTokenKey(string accessTokenKey)
    {
        AccessTokenKey = accessTokenKey;
    }
    
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        var lobbyManager = eventManagerBase as LobbyManager;
        lobbyManager.SendAccessTokenKey(client,AccessTokenKey);
    }
}
