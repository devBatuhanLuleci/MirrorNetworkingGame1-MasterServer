using ACGAuthentication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class SpawnServer : EventManagerBase
{
    public Dictionary<ushort, Room> rooms = new Dictionary<ushort, Room>();
    public const ushort START_PORT = 3000;
    private ushort currentPort = START_PORT;
    public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.SpawnServer;

    public SpawnServer(LoadBalancer loadBalancer) : base(loadBalancer)
    {
        loadBalancer.AddEventHandler(loadBalancerEvent, this);
    }
    ~SpawnServer()
    {
        loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
    }

    internal override Dictionary<byte, Type> initResponseTypes()
    {
        var responseTypes = new Dictionary<byte, Type>();
        responseTypes.Add((byte)SpawnServerEvent.Ready, typeof(OnReadyEvent));
        responseTypes.Add((byte)SpawnServerEvent.ConnectToGameServer, typeof(ConnectToGameServerEvent));
        return responseTypes;
    }

    public void SetRoomReady(int port, ClientPeer roomPlayer)
    {
        if (!rooms.ContainsKey((ushort)port))
        {
            var newRoom = new Room((ushort)port, roomPlayer);
            rooms.Add((ushort)port, newRoom);
        }
        rooms[(ushort)port].Ready();
    }


    public Process StartGameServer(int port)
    {
        return ExecuteManager.ExecuteCommand(port.ToString());
    }
    #region Match
    public void NewMatch(ClientPeer client)
    {
        // TODO:  Start new game server and forward players to server
        // match players and start room
        // after room ready redirect the players.
        foreach (var room in rooms)
        {
            var roomObj = room.Value;
            if (roomObj.CanJoin)
            {
                roomObj.AddPlayer(client);
                roomObj.Start();
                return;
            }
        }
        var newRoom = StartNewRoom();
        newRoom.AddPlayer(client);     

    }

    private Room StartNewRoom()
    {
        // if not match any room start new room.
        var gameServer = StartGameServer(currentPort);

        if (gameServer != null)
        {
            var newRoom = new Room(currentPort, gameServer);
            rooms.Add(currentPort, newRoom);
            currentPort++;
            return newRoom;
        }
        else
        {
            throw new Exception("Game Server can't start.");
        }
    }

    internal Room NewMatch(LobbyRoom room)
    {
        // TODO: if there is no opened room
        // Start new server and add all players to this room
        // else create and add all players to this room
        var newRoom = StartNewRoom();

    
        for (int i = 0; i < room.Players.Count; i++)
        {
            var player = room.Players[i];
            newRoom.AddPlayer(player.client);
        }
        return newRoom;
    }
    #endregion

}
