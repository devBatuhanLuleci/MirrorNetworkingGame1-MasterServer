using ACGAuthentication;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LobbyRoom
{
    public List<LobbyPlayer> Players { get; set; }
    public int Id { get; private set; }
    public int MaxPlayer { get; private set; }


    private LobbyManager lobbyManager;
    private Room roomInstance;
    public LobbyRoom(int id, LobbyManager lobbyManager)
    {
        Id = id;
        MaxPlayer = 2;
        Players = new List<LobbyPlayer>();
        this.lobbyManager = lobbyManager;
    }

    public LobbyRoom(LobbyPlayer player, LobbyManager lobbyManager, int id) : this(id, lobbyManager)
    {
        Debug.Log($"Lobby room created with {id} room id.");
        AddNewPlayer(player);
    }
    ~LobbyRoom()
    {
        if(roomInstance != null)
        {
            roomInstance.OnRoomStateChange -= OnRoomStateChange;    
        }
    }

    public void JoinPlayer(LobbyPlayer joindPlayer)
    {
        if (!CanJoin(joindPlayer)) return;

        AddNewPlayer(joindPlayer);
        NewPlayerInfoSendToRoom(joindPlayer);
    }

    public void Start(LobbyPlayer player)
    {
        if (Players.Contains(player) && player.IsLeader && roomInstance == null)
        {
            // TODO: start room
            roomInstance = LoadBalancer.Instance.SpawnServer.NewMatch(this);
            roomInstance.OnRoomStateChange += OnRoomStateChange;
            for (int i = 0; i < Players.Count; i++)
            {
                var playerClient = Players[i].client;
                roomInstance.AddPlayer(playerClient);
            }
        }
        else
        {
            // TODO: return you cant start room response
        }
    }



    private bool CanJoin(LobbyPlayer joindPlayer)
    {
        if (Players.Count >= MaxPlayer)
        {
            var ev = new MaxPlayerError();
            lobbyManager.SendServerRequestToClient(joindPlayer.client, ev);
            return false;
        }
        return true;
    }
    private void AddNewPlayer(LobbyPlayer lobbyPlayer, bool isLeader = false)
    {
        RemovePlayer(lobbyPlayer);
        AddListeners(lobbyPlayer);

        Players.Add(lobbyPlayer);
        lobbyPlayer.RoomId = Id;
        Debug.Log($"AddNewPlayer RoomId {lobbyPlayer.RoomId} Id: {Id}");
    }
    private void NewPlayerInfoSendToRoom(LobbyPlayer newPlayer)
    {
        var ev = new NewPlayerJoinedToLobbyRoom(newPlayer);
        NotifyRoom(ev, newPlayer);
        var evPlayerJoinedToLobbyRoom = new PlayerJoinedToLobbyRoom(Id, Players.ToArray(), newPlayer);
        lobbyManager.SendServerRequestToClient(newPlayer.client, evPlayerJoinedToLobbyRoom);

    }

    public void Ready(ClientPeer client)
    {
        var player = Players.Find(el => el.client == client);
        if (player != null)
        {
            player.IsReady = !player.IsReady;
        }

        // TODO send all players to changed player status
        var ev = new ReadyStateChanged(player);
        NotifyRoom(ev);
    }
    /// <summary>
    /// Send an IEvent to all players of room
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="excluede"></param>
    public void NotifyRoom(IEvent ev, LobbyPlayer excluede = null)
    {
        for (int i = 0; i < Players.Count; i++)
        {
            var player = Players[i];
            // send info to all players except newPlayer
            if (excluede != null && player.UserName == excluede.UserName) continue;
            lobbyManager?.SendServerRequestToClient(player.client, ev);
        }
    }

    #region Listeners
    private void AddListeners(LobbyPlayer joindPlayer)
    {
        joindPlayer.OnDisconnected += OnClientDisconnected;

    }
    private void RemoveListeners(LobbyPlayer removedPlayer)
    {
        removedPlayer.OnDisconnected -= OnClientDisconnected;
    }
    private void OnClientDisconnected(LobbyPlayer player)
    {
        RemovePlayer(player);
        var ev = new OnLeaveLobbyRoom(player);
        NotifyRoom(ev);
    }
    private void RemovePlayer(LobbyPlayer joindPlayer)
    {
        var player = Players.Find(el => el.client == joindPlayer.client);
        if (player != null)
        {
            RemoveListeners(player);
            //TODO: send already added event or destroy old object and add new
            Players.Remove(player);
            player.Reset();
        }
    }

    internal void Leave(LobbyPlayer lobbyPlayer)
    {
        Debug.Log("Room player Leave");
        if (lobbyPlayer.IsLeader)
        {
            Debug.Log("Room player is Leader");
            Players.ForEach(el =>
            {
                var ev = new OnLeaveLobbyRoom(el);
                lobbyManager.SendServerRequestToClient(el.client, ev);
                lobbyPlayer.Reset();
            });
            Players.Clear();
            lobbyManager.RemoveRoom(this);
        }
        else
        {
            Debug.Log("Room player isn't Leader");

            RemovePlayer(lobbyPlayer);
            var ev = new OnLeaveLobbyRoom(lobbyPlayer);
            NotifyRoom(ev);
        }
    }


    #endregion

    #region RoomInstance
    public void OnRoomStateChange(RoomState state)
    {
        switch (state)
        {
            case RoomState.Preparing:               
                break;
            case RoomState.Ready:
                roomInstance.Start();

                break;
            case RoomState.Started:
                break;
            case RoomState.Disconnected:
                break;
            default:
                break;
        }
    }
    #endregion
}