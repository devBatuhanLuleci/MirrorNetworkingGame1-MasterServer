using ACGAuthentication;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    /// <summary>
    /// Client'lar tarf?ndan bir oda açmak için kullan?l?r. 
    ///  Oday? açan kullan?c? odan?n sahibi olarak belirlenir ve
    ///  kullan?c?ya yeni aç?lan odan?n payala??labilir bir ID’sini gönderir.
    /// </summary>
    /// <param name="client"></param>
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

    /// <summary>
    /// Payla??labilir RoomId ile bir odaya ba?lanmay? sa?lar. 
    /// E?er böylebir oda aç?kde?ilse gerekli hata bilgisi kullan?c?ya gönderirilir.
    /// Oyuncu odaya ba?land?ktan sonra odadaki herkese bilgi gönderilir.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="roomId"></param>
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

    /// <summary>
    /// Daha önceden aç?lm?? olan bir odadaki oynu ba?latmaya yarar. 
    /// Bu methodu ça??ran client’?n odan?n kurucusu olmas? gerekmektedir.
    /// </summary>
    /// <param name="client"></param>
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
    /// <summary>
    /// Oyundan bir clientin ayr?lmas? için kullan?l?r. 
    /// </summary>
    /// <param name="client"></param>
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
    /// <summary>
    /// Belirtilen oday? serverdan kald?r?r. 
    /// </summary>
    /// <param name="room"></param>
    public void RemoveRoom(LobbyRoom room)
    {
        if (Rooms.ContainsKey(room.Id))
        {
            Rooms.Remove(room.Id);
        }
    }
    /// <summary>
    /// Client’i serverdaki Players listedinden ç?kar?r. 
    /// </summary>
    /// <param name="client"></param>
    private void RemovePlayer(LobbyPlayer client)
    {
        if (Players.ContainsKey(client.client))
        {
            Players.Remove(client.client);
        }
    }
    /// <summary>
    /// Oda?n?n haz?r oldu?unu belirtme için kullan?lan method. 
    /// </summary>
    /// <param name="client"></param>
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
    /// <summary>
    /// Client için LobbyMnager daki LobbyPlayer’? getiren method. 
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    private LobbyPlayer GetPlayer(ClientPeer client)
    {
        if (Players.TryGetValue(client, out var lobbyPlayer))
        {
            return lobbyPlayer;
        }
        return null;
    }
    /// <summary>
    /// ClientPeer için E?er varsa LobbyPlayer’? getirir yoksa olu?turur. 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="name"></param>
    /// <param name="isLeader"></param>
    /// <returns></returns>
    private LobbyPlayer GetOrCreatePlayer(ClientPeer client, string name, bool isLeader = false)
    {
        var player = GetPlayer(client);
        if (player == null)
        {
            return new LobbyPlayer(client, name, isLeader);
        }
        return player;
    }
    /// <summary>
    /// ClientPeer ‘?n içinde oldu?u LobbyRoom’u getirir. E?er client hiç bir odada de?ilse null döner. 
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
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



