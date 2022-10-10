using ACGAuthentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LobbyManager : EventManagerBase
{
    public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.Lobby;

    public Dictionary<int, LobbyRoom> Rooms { get; private set; }
    public Dictionary<ClientPeer, LobbyPlayer> Players { get; private set; }

    #region Private fields
    private int rommId = 0;
    #endregion
    public LobbyManager(LoadBalancer loadBalancer) : base(loadBalancer)
    {
        loadBalancer.AddEventHandler(loadBalancerEvent, this);
        Players = new Dictionary<ClientPeer, LobbyPlayer>();
        Rooms = new Dictionary<int, LobbyRoom>();
    }
    ~LobbyManager()
    {
        loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
    }
    internal override Dictionary<byte, Type> initResponseTypes()
    {
        var responseTypes = new Dictionary<byte, Type>();
        responsesByType.Add((byte)LobbyEvent.StartMatnch, typeof(CreateLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.GetPlayers, typeof(GetPlayersEvent));
        responsesByType.Add((byte)LobbyEvent.CreateLobbyRoom, typeof(LobbyRoomCreated));
        responsesByType.Add((byte)LobbyEvent.JoinLobbyRoom, typeof(JoinLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.NewJoinedToLobbyRoom, typeof(NewPlayerJoinedToLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.JoinedToLobbyRoom, typeof(PlayerJoinedToLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.MaxPlayerError, typeof(MaxPlayerError));
        responsesByType.Add((byte)LobbyEvent.OnLeaveLobbyRoom, typeof(OnLeaveLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.ReadyStateChange, typeof(ReadyStateChange));
        responsesByType.Add((byte)LobbyEvent.ReadyStateChanged, typeof(ReadyStateChanged));
        responsesByType.Add((byte)LobbyEvent.LeaveRoom, typeof(LeaveRoom));
        responsesByType.Add((byte)LobbyEvent.ThereIsNoRoom, typeof(ThereIsNoRoom));
        responsesByType.Add((byte)LobbyEvent.StartLobbyRoom, typeof(StartLobbyRoom));
        
        return responsesByType;
    }
  
    public void CreateLobbyRoom(ClientPeer client)
    {
        var randomName = "user" + UnityEngine.Random.Range(1, 999);

        var player = GetOrCreatePlayer(client, randomName, true);
        player.OnDisconnected += RemovePlayer;
        if (Players.ContainsKey(client))
        {
            Players.Remove(client);
        }
        Players.Add(client, player);

        var room = new LobbyRoom(player, this, rommId++);
        Rooms.Add(room.Id, room);

        var ev = new LobbyRoomCreated(room.Id, player);
        SendServerRequestToClient(client, ev);

    }
    public void JoinMatchLobbyRoom(ClientPeer client, int roomId)
    {
        if (Rooms.TryGetValue(roomId, out var room))
        {
            var randomName = "user" + UnityEngine.Random.Range(1, 999);

            var player = GetOrCreatePlayer(client, randomName);
            player.OnDisconnected += RemovePlayer;

            if (Players.ContainsKey(client))
            {
                Players.Remove(client);
            }
            Players.Add(client, player);

            room.JoinPlayer(player);
        }
        else
        {
            // TODO: return error message about no have open room
            var ev = new ThereIsNoRoom(roomId);
            SendServerRequestToClient(client, ev);
        }
    }
   
    public void StartMatch(ClientPeer client)
    {
        var room = GetRoom(client);
        if (room == null)
        {
            Debug.Log("StartMatch room is null");
            // TODO: throw error client haven't a room
            return;
        }
        Debug.Log("StartMatch GetRoom " + room.Id);
        var player = GetPlayer(client);

        if (player != null)
            room.Start(player);
        else
            Debug.Log("StartMatch player is null");
    }
    public void LeaveRoom(ClientPeer client)
    {
        var room = GetRoom(client);
        if (room == null)
        {
            Debug.Log("room is null");
            // TODO: throw error client haven't a room
            return;
        }
        Debug.Log("GetRoom " + room.Id);
        var player = GetPlayer(client);

        if (player != null)
            room.Leave(player);
        else
            Debug.Log("player is null");

    }

    public void RemoveRoom(LobbyRoom room)
    {
        if (Rooms.ContainsKey(room.Id))
        {
            Rooms.Remove(room.Id);
        }
    }
    private void RemovePlayer(LobbyPlayer client)
    {
        if (Players.ContainsKey(client.client))
        {
            Players.Remove(client.client);
        }
    }
    internal void ReadyState(ClientPeer client)
    {
        var room = GetRoom(client);
        if (room == null)
        {
            // TODO: throw error client haven't a room
            return;
        }
        room.Ready(client);
    }
    private LobbyPlayer GetPlayer(ClientPeer client)
    {
        if (Players.TryGetValue(client, out var lobbyPlayer))
        {
            return lobbyPlayer;
        }
        return null;
    }
    private LobbyPlayer GetOrCreatePlayer(ClientPeer client, string name, bool isLeader = false)
    {
        var player = GetPlayer(client);
        if (player == null)
        {
            return new LobbyPlayer(client, name, isLeader);
        }
        return player;
    }
    private LobbyRoom GetRoom(ClientPeer client)
    {
        var lobbyPlayer = GetPlayer(client);
        if (lobbyPlayer == null)
        {
            Debug.Log("GetRoom lobbyPlayer is null");
            return null;
        }
        Debug.Log($"GetRoom lobbyPlayer.RoomId {lobbyPlayer.RoomId}");

        if (Rooms.TryGetValue(lobbyPlayer.RoomId, out var room))
        {
            return room;
        }
        Debug.Log("GetRoom Room is null");

        return null;
    }


}



