using ACGAuthentication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : EventManagerBase
{
    public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.Lobby;

    public Dictionary<int, LobbyRoom> Rooms { get; private set; }

    #region Private fields
    private int rommId = 0;
    #endregion
    public LobbyManager(LoadBalancer loadBalancer) : base(loadBalancer)
    {
        loadBalancer.AddEventHandler(loadBalancerEvent, this);
        Rooms = new Dictionary<int, LobbyRoom>();
    }
    ~LobbyManager()
    {
        loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
    }
    internal override Dictionary<byte, Type> initResponseTypes()
    {
        var responseTypes = new Dictionary<byte, Type>();
        responsesByType.Add((byte)LobbyEvent.StartMatnch, typeof(StartMatchEvent));
        responsesByType.Add((byte)LobbyEvent.GetPlayers, typeof(GetPlayersEvent));
        responsesByType.Add((byte)LobbyEvent.CreateLobbyRoom, typeof(CreateLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.JoinLobbyRoom, typeof(JoinLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.NewJoinedToLobbyRoom, typeof(NewPlayerJoinedToLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.JoinedToLobbyRoom, typeof(PlayerJoinedToLobbyRoom));
        responsesByType.Add((byte)LobbyEvent.MaxPlayerError, typeof(MaxPlayerError));
        responsesByType.Add((byte)LobbyEvent.OnDisconnectedLobbyRoom, typeof(OnDisconnectedLobbyRoom));        

        return responsesByType;
    }
    public void StartMatchLobby(ClientPeer client)
    {
        var randomName = "user" + UnityEngine.Random.Range(1, 999);
        var player = new LobbyPlayer(client, randomName);
        var room = new LobbyRoom(player, this, rommId++);
        if (Rooms.ContainsKey(room.Id))
        {
            Rooms.Remove(room.Id);
        }
        Rooms.Add(room.Id, room);

        var ev = new CreateLobbyRoom(room.Id, player);
        SendServerRequestToClient(client, ev);

    }
    public void JoinMatchLobbyRoom(ClientPeer client, int roomId)
    {
        if (Rooms.TryGetValue(roomId, out var room))
        {
            var randomName = "user" + UnityEngine.Random.Range(1, 999);
            var player = new LobbyPlayer(client, randomName);
            room.JoinPlayer(player);
        }
        else
        {
            // TODO: return error message about no have open room
        }
    }
    public void StartNewMatch(ClientPeer client)
    {
        Debug.Log("Methond not implemted!");
        //var spawnServer = loadBalancer.GetEventHandler<SpawnServer>(LoadBalancerEvent.SpawnServer);
        //if (Rooms.TryGetValue(client, out var room))
        //{
        //    spawnServer.NewMatch(room);
        //}
        //else
        //{
        //    spawnServer.NewMatch(client);
        //}
    }

}



