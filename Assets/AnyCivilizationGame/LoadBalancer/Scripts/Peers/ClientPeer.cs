using ACGAuthentication;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPeer : PeerBase
{
    public Action<ClientPeer> OnDissconnect { get; set; }

    public ClientPeer(LoadBalancer loadBalancer, int connectionId) : base(loadBalancer, connectionId)
    {

    }

    #region Public Fiealds
    public ILoginData loginData { get; set; }
    public bool IsLogin
    {
        get => !(loginData == null);
    }
    #endregion


    public override void OnDisconnected()
    {
        base.OnDisconnected();
        Debug.Log("ClientPeer OnDisconnected");
        if (OnDissconnect != null)
            OnDissconnect.Invoke(this);
    }
}
