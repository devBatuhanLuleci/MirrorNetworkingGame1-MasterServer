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
        responsesByType.Add((byte)LobbyEvent.JoinedToLobbyRoom, typeof(NewPlayerJoinedToLobbyRoom));


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

public class LobbyRoom
{
    public List<LobbyPlayer> players { get; set; }
    public int Id { get; private set; }

    private LobbyManager lobbyManager;
    public LobbyRoom(LobbyPlayer player, LobbyManager lobbyManager)
    {
        players = new List<LobbyPlayer>() { player };
        this.lobbyManager = lobbyManager;
    }

    public LobbyRoom(LobbyPlayer player, LobbyManager lobbyManager, int id) : this(player, lobbyManager)
    {
        Debug.Log($"Lobby room created with {id} room id.");
        Id = id;
    }

    public void JoinPlayer(LobbyPlayer joindPlayer)
    {
        var player = players.Find(el => el.client == joindPlayer.client);
        if (player != null)
        {
            //TODO: send already added event or destroy old object and add new
            players.Remove(player);
        }
        players.Add(joindPlayer);
        NewPlayerInfoSendToRoom(joindPlayer);
    }

    private void NewPlayerInfoSendToRoom(LobbyPlayer newPlayer)
    {
        var ev = new NewPlayerJoinedToLobbyRoom(newPlayer);
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            // send info to all players except newPlayer
            if (player.UserName != newPlayer.UserName)
                lobbyManager.SendServerRequestToClient(player.client, ev);
        }

    }

    public void Ready(ClientPeer client)
    {
        var player = players.Find(el => el.client == client);

        if (player != null)
        {
            player.IsReady = true;
        }

        // TODO send all players to changed player status

    }
}
public class LobbyPlayer
{
    public ClientPeer client { get; private set; }
    public bool IsLeader { get; private set; } = false;
    public bool IsReady { get; set; } = false;
    public string UserName { get; private set; }

    public LobbyPlayer(string userName)
    {
        UserName = userName;
    }
    public LobbyPlayer(ClientPeer client, string userName) : this(userName)
    {
        this.client = client;
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


}



