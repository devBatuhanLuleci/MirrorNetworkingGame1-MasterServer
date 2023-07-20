using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
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
    private Dictionary<string, List<ClientPeer>> teams = new Dictionary<string, List<ClientPeer>>();
    SpawnServer SpawnServer;
    private bool allPlayersConnected = false;
    #endregion

    #region Instance
    public Process GameServer => gameServer;

    public bool Started { get { return state == RoomState.Started || state == RoomState.Ready; } }

    private Process gameServer;
    #endregion

    #region Constructurs
    public Room(ushort port)
    {
        SpawnServer = LoadBalancer.Instance.SpawnServer;
        Host = ServerSettings.Instance.GameServers[0];
        Port = port;

        //Debug.Log($"ServerSettings Host: {Host}");

        Debug.Log($"a new romm is created at {Port} port and {Host}");
    }
    public Room(ushort port, Process server) : this(port)
    {
        gameServer = server;
    }

    public Room(ushort port, ClientPeer roomClient) : this(port)
    {
        peer = roomClient;
    }
    #endregion

    public void AddPlayer(ClientPeer player, string team)
    {
        // eğer team yoksa oluştur.
        if (!teams.ContainsKey(team)) teams.Add(team, new List<ClientPeer>());


        if (teams.TryGetValue(team, out var teamObject))
        {
            if (teamObject.Contains(player)) return;
            teamObject.Add(player);
            player.OnDissconnect += RemovePlayer;
        }
    }
    public void RemovePlayer(ClientPeer player)
    {
        player.OnDissconnect -= RemovePlayer;

        foreach (var team in teams)
        {
            team.Value.Remove(player);
        }
    }
    public void CloseRoom()
    {
        //GameServer.Kill();
        var ev = new CloseRoomEvent();
        SpawnServer.SendServerRequestToClient(peer, ev);
    }

    internal void ConnectPlayers()
    {
        foreach (var team in teams)
        {
            foreach (var player in team.Value)
            {
                ConnectPlayer(player);
            }
        }

    }

    internal void ConnectPlayer(ClientPeer player)
    {
        SpawnServer.SendServerRequestToClient(player, new ConnectToGameServerEvent(Port, Host));
    }

    public async void Start()
    {
        if (state != RoomState.Started)
        {
            State = RoomState.Started;
            Debug.Log($"{Port} room is starting");
            await Task.Delay(1500);
            SendTeamInfoToGameServer();
            await WaitForAllPlayersConnected();
            ConnectPlayers();
            Debug.Log($"{Port} room is started");
        }
    }
    private async Task WaitForAllPlayersConnected()
    {
        while (!allPlayersConnected)
        {
            await Task.Delay(100);
            Debug.Log("********* connectedPlayerCount " + 4 + " GetTotalPlayerCount() " + GetTotalPlayerCount());
            if (4 == GetTotalPlayerCount())
                allPlayersConnected = true;
        }
    }

    private void SendTeamInfoToGameServer()
    {
        var isA = true;
        var ev = new RoomInfoEvent { };
        foreach (var team in teams)
        {
            var teamTokens = team.Value.Select(it => it.loginData.AccessToken).ToArray();
            SpawnServer.Debug(team.Key);
            foreach (var token in teamTokens)
            {
                SpawnServer.Debug(token);
            }
            if (isA) ev.teamA = teamTokens;
            else ev.teamB = teamTokens;

            foreach (var token in teamTokens)
            {
                var teamText = isA ? "TeamA" : "TeamB";
                Debug.Log($"{teamText}: {token}");
            }
            isA = false;
        }

        SpawnServer.SendServerRequestToClient(peer, ev);
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

    private int GetTotalPlayerCount()
    {
        Debug.Log("***** teams.Count " + teams.Count);
        int count = 0;
        foreach (var team in teams)
        {
            count += team.Value.Count;
            Debug.Log("***** team.Value.Coun " + team.Value.Count);
        }
        return count ;
    }
}
