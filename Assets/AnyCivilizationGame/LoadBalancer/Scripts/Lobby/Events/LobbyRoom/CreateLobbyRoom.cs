using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLobbyRoom : IEvent
{
    public int RoomCode { get; private set; }
    public LobbyPlayer LobbyPlayer;
    public CreateLobbyRoom(int roomCode,LobbyPlayer lobbyPlayer)
    {
        RoomCode = roomCode;

        LobbyPlayer = lobbyPlayer;
    }
}
