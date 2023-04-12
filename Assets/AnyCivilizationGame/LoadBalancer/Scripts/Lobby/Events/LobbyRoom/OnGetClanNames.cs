using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnGetClanNames : IEvent {
    public string[] ClanNames;
    public bool IsNewClanNameCreate;
    public OnGetClanNames (string[] clanNames, bool isNewClanNameCreate) {
        ClanNames = clanNames;
        IsNewClanNameCreate = isNewClanNameCreate;
    }
}