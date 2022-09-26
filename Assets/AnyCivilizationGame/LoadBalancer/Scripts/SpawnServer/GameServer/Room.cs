using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using UnityEditor.EditorTools;
using UnityEngine;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class Room
{
    #region Public fields
    public ushort Port { get; private set; }
    public string Host { get; private set; } = "localhost";
    public ClientPeer peer { get; private set; }

    public RoomState State { get; private set; } = RoomState.Preparing;

    public bool CanJoin
    {
        get
        {
            // room join logics must add here
            return State == RoomState.Ready;
        }
    }
    #endregion

    #region Private Fields
    private List<ClientPeer> players = new List<ClientPeer>();

    #endregion

    #region Instance
    public Process GameServer => gameServer;
    private Process gameServer;
    #endregion

    #region Constructurs
    public Room()
    {
    }
    public Room(ushort port, Process server)
    {
        Port = port;
        gameServer = server;
    }

    public Room(ushort port, ClientPeer roomClient)
    {
        Port = port;
        peer = roomClient;
    }
    #endregion

    public void AddPlayer(ClientPeer player)
    {
        players.Add(player);
        player.OnDissconnect += RemovePlayer;
        Debug.Log($"{player} add to {Port} room");
    }
    public void RemovePlayer(ClientPeer player)
    {
        player.OnDissconnect -= RemovePlayer;
        players.Remove(player);
    }
    public void CloseRoom()
    {
        // TODO: not direct kill send kill message
        // if can't response from room kill the room instance
        GameServer.Kill();

    }

    internal void ConnectPlayers(SpawnServer spawnServer)
    {
        players.ForEach(el => ConnectPlayer(el, spawnServer));
        if (State == RoomState.Ready)
        {
            State = RoomState.Started;
        }
    }

    internal void ConnectPlayer(ClientPeer player, SpawnServer spawnServer)
    {
        spawnServer.SendServerRequestToClient(player, new ConnectToGameServerEvent(Port, Host));      
    }

    public void Start(SpawnServer spawnServer)
    {
        // TODO: Redirect players and start server.   
        ConnectPlayers(spawnServer);   
    }

    public void Ready()
    {
        State = RoomState.Ready;
        Debug.Log($"{Port} room is ready");
    }
}
