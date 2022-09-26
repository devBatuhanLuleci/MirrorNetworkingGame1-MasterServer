using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobbyEvent : byte
{
    StartMatnch = 0x01,
    GetPlayers = 0x2,
    CreateLobbyRoom = 0x3,
    JoinLobbyRoom = 0x4,
    JoinedToLobbyRoom = 0x5,
}
