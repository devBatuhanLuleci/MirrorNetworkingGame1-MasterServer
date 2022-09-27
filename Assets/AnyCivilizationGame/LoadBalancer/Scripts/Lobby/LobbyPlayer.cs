using System;
using UnityEngine;

public class LobbyPlayer
{
    public ClientPeer client { get; private set; }
    public bool IsLeader { get; private set; } = false;
    public bool IsReady { get; set; } = false;
    public string UserName { get; private set; }

    public Action<LobbyPlayer> OnClientDisconnected;
    public LobbyPlayer(string userName)
    {
        UserName = userName;
    }
    public LobbyPlayer(ClientPeer client, string userName) : this(userName)
    {
        this.client = client;
        this.client.OnDissconnect += OnPeerDisconnected;
    }

    public LobbyPlayer(ClientPeer client, string userName, bool isLeader) : this(client, userName)
    {
        IsLeader = isLeader;
    }

    public LobbyPlayer(string userName, bool isLeader, bool isReady) : this(userName)
    {
        IsLeader = isLeader;
        IsReady = isReady;
    }

    ~LobbyPlayer()
    {
        OnClientDisconnected -= OnClientDisconnected;
    }
    private void OnPeerDisconnected(ClientPeer clientPeer)
    {
        Debug.Log("LobbyPlayer OnPeerDisconnected");
        if (OnClientDisconnected != null)
            OnClientDisconnected.Invoke(this);
    }

}



