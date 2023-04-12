using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnSendFriendName : IEvent {

    public string FriendName { get; private set; }

    public OnSendFriendName (string friendName) {
        FriendName = friendName;
    }
}