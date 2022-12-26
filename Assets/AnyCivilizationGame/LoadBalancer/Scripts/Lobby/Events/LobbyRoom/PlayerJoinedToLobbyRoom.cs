using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Oday? kuran client’e oda ile ilgili bilgileri gönderir. 
/// </summary>
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
