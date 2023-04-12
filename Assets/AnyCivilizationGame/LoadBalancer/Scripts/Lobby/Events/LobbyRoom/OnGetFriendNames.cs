using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnGetFriendNames : IEvent {
    public string[] FriendNames;
    public bool IsNewFriendNameAdd;

    public OnGetFriendNames (string[] friendNames, bool isNewFriendNameAdd) {
        FriendNames = friendNames;
        IsNewFriendNameAdd = isNewFriendNameAdd;
    }
}