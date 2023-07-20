using ACGAuthentication;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PeerBase
{
    #region Public Fiealds
    public int ConnectionId { get; private set; }
    public Action OnPeerDisconnected { get; set; }

    #endregion


    #region Private Properties
    private LoadBalancer loadBalancer;
    #endregion


    public PeerBase(LoadBalancer loadBalancer, int connectionId)
    {
        this.loadBalancer = loadBalancer;
        this.ConnectionId = connectionId;
    }

    public void Send(ArraySegment<byte> segment, int channelId = Channels.Reliable)
    {
        // NetworkReader and NetworkReader will using fore read and write
        loadBalancer.ServerSend(ConnectionId, segment, channelId);
    }
    public void Disconnect()
    {
        Debug.Log("PeerBase Disconnect");
        loadBalancer.ServerDisconnect(ConnectionId);
        if (OnPeerDisconnected != null)
            OnPeerDisconnected.Invoke();
    }




    public virtual void OnDisconnected()
    {
        Debug.Log("PeerBase OnDisconnected");
        if (OnPeerDisconnected != null)
            OnPeerDisconnected.Invoke();
    }

}
