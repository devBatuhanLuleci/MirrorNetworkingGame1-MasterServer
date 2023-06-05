﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            team.Value.ForEach(el => ConnectPlayer(el));
        }
    }

    internal void ConnectPlayer(ClientPeer player)
    {
        SpawnServer.SendServerRequestToClient(player, new ConnectToGameServerEvent(Port, Host));
    }

    public void Start()
    {
        Debug.Log($"{Port} room is started");
        //TODO: send user info to gameserver.
        SendTeamInfoToGameServer();
        ConnectPlayers();
        State = RoomState.Started;
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
}
