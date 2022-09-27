using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoinedToLobbyRoom : IEvent
{
    public int RoomCode { get; private set; }
    public LobbyPlayer LobbyPlayer;
    public LobbyPlayer[] LobbyPlayers;

    public PlayerJoinedToLobbyRoom(int roomCode, LobbyPlayer[] lobbyPlayers, LobbyPlayer lobbyPlayer)
    {
        RoomCode = roomCode;
        LobbyPlayers = lobbyPlayers;
        LobbyPlayer = lobbyPlayer;
    }
}
