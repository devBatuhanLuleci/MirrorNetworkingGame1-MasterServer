using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSendClanName : IEvent
{
    public string ClanName { get; private set; }

    public OnSendClanName(string clanName)
    {
        ClanName = clanName;
    }
}
