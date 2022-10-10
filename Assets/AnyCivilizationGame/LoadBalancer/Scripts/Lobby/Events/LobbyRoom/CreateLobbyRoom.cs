using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRoomCreated : IEvent
{
    public int RoomCode { get; private set; }
    public LobbyPlayer LobbyPlayer;
    public LobbyRoomCreated(int roomCode,LobbyPlayer lobbyPlayer)
    {
        RoomCode = roomCode;

        LobbyPlayer = lobbyPlayer;
    }
}
