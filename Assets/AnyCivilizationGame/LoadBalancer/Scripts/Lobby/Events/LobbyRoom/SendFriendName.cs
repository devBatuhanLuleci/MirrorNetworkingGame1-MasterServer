using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class SendFriendName : IResponseEvent {

    public string FriendName;

    public SendFriendName (string friendName) {
        FriendName = friendName;
    }

    public void Invoke (EventManagerBase eventManagerBase, ClientPeer client) {
        var lobbyManager = eventManagerBase as LobbyManager;
        lobbyManager.SendFriendName (client, FriendName);
    }
}