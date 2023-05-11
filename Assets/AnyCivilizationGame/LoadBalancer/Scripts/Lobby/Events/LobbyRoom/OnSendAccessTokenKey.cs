using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSendAccessTokenKey : IEvent
{
    public bool IsAccessTokenKey { get; set; }

    public OnSendAccessTokenKey(bool isAccessTokenKey)
    {
        IsAccessTokenKey = isAccessTokenKey;
    }
}
