using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class Room
{
    #region Public fields
    public ushort Port { get; private set; }
    public string Host { get; private set; } = "localhost";
    public ClientPeer peer { get; private set; }

    public RoomState State
    {
        get { return state; }
        private set
        {
            state = value;
            if (OnRoomStateChange != null)
            {
                OnRoomStateChange(state);
            }
        }
    }
    private RoomState state = RoomState.Preparing;

    public Action<RoomState> OnRoomStateChange;

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
    SpawnServer SpawnServer;

    #endregion

    #region Instance
    public Process GameServer => gameServer;

    public bool Started { get { return state == RoomState.Started || state == RoomState.Ready; } }

    private Process gameServer;
    #endregion

    #region Constructurs
    public Room()
    {
        SpawnServer = LoadBalancer.Instance.SpawnServer;
        Host = ServerSettings.Instance.GameServers[0];
        //Debug.Log($"ServerSettings Host: {Host}");

        Debug.Log($"a new romm is created at {Port} port and {Host}");
    }
    public Room(ushort port, Process server) : this()
    {
        Port = port;
        gameServer = server;
    }

    public Room(ushort port, ClientPeer roomClient) : this()
    {
        Port = port;
        peer = roomClient;
    }
    #endregion

    public void AddPlayer(ClientPeer player)
    {
        players.Add(player);
        player.OnDissconnect += RemovePlayer;
        //Debug.Log($"{player} add to {Port} room");
    }
    public void RemovePlayer(ClientPeer player)
    {
        player.OnDissconnect -= RemovePlayer;
        players.Remove(player);
    }
    public void CloseRoom()
    {
        //GameServer.Kill();
        var ev = new CloseRoomEvent();
        SpawnServer.SendServerRequestToClient(peer, ev);
    }

    internal void ConnectPlayers()
    {
        players.ForEach(el => ConnectPlayer(el));
    }

    internal void ConnectPlayer(ClientPeer player)
    {
        SpawnServer.SendServerRequestToClient(player, new ConnectToGameServerEvent(Port, Host));
    }

    public void Start()
    {
        // TODO: Redirect players and start server.   
        ConnectPlayers();
    }

    public void Ready()
    {
        State = RoomState.Ready;
        Debug.Log($"{Port} room is ready");
    }

    internal void Ready(ClientPeer roomPlayer)
    {
        peer = roomPlayer;
        Ready();
    }
}
