using ACGAuthentication;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LobbyRoom
{
    public List<LobbyPlayer> players { get; set; }
    public int Id { get; private set; }
    public int MaxPlayer { get; private set; }


    private LobbyManager lobbyManager;
    public LobbyRoom(LobbyPlayer player, LobbyManager lobbyManager)
    {
        players = new List<LobbyPlayer>() { player };
        this.lobbyManager = lobbyManager;
        MaxPlayer = 3;

    }

    public LobbyRoom(LobbyPlayer player, LobbyManager lobbyManager, int id) : this(player, lobbyManager)
    {
        Debug.Log($"Lobby room created with {id} room id.");
        Id = id;
    }

    public void JoinPlayer(LobbyPlayer joindPlayer)
    {
        if (!CanJoin(joindPlayer)) return;

        RemovePlayer(joindPlayer);
        AddListeners(joindPlayer);

        players.Add(joindPlayer);
        NewPlayerInfoSendToRoom(joindPlayer);
    }




    private bool CanJoin(LobbyPlayer joindPlayer)
    {
        if (players.Count >= MaxPlayer)
        {
            var ev = new MaxPlayerError();
            lobbyManager.SendServerRequestToClient(joindPlayer.client, ev);
            return false;
        }
        return true;
    }

    private void NewPlayerInfoSendToRoom(LobbyPlayer newPlayer)
    {
        var ev = new NewPlayerJoinedToLobbyRoom(newPlayer);
        NotifyRoom(ev, newPlayer);
        var evPlayerJoinedToLobbyRoom = new PlayerJoinedToLobbyRoom(Id, players.ToArray(), newPlayer);
        lobbyManager.SendServerRequestToClient(newPlayer.client, evPlayerJoinedToLobbyRoom);

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
    /// <summary>
    /// Send an IEvent to all players of room
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="excluede"></param>
    public void NotifyRoom(IEvent ev, LobbyPlayer excluede = null)
    {
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            // send info to all players except newPlayer
            if (excluede != null && player.UserName == excluede.UserName) continue;
            lobbyManager.SendServerRequestToClient(player.client, ev);
        }
    }

    #region Listeners
    private void AddListeners(LobbyPlayer joindPlayer)
    {
        joindPlayer.OnClientDisconnected += OnClientDisconnected;

    }
    private void RemoveListeners(LobbyPlayer removedPlayer)
    {
        removedPlayer.OnClientDisconnected -= OnClientDisconnected;
    }
    private void OnClientDisconnected(LobbyPlayer player)
    {
        RemovePlayer(player);
        var ev = new OnDisconnectedLobbyRoom(player);
        NotifyRoom(ev);
    }


    private void RemovePlayer(LobbyPlayer joindPlayer)
    {
        var player = players.Find(el => el.client == joindPlayer.client);
        if (player != null)
        {
            RemoveListeners(player);
            //TODO: send already added event or destroy old object and add new
            players.Remove(player);
        }
    }
    #endregion
}



