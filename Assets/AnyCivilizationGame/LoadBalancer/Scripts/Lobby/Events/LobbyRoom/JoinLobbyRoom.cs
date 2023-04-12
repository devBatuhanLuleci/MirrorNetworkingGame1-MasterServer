using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Payla??labilir RoomID ile bir odaya ba?lanmak i�in kullan?l?r. 
/// </summary>
public class JoinLobbyRoom : IResponseEvent
{
    public int RoomCode { get; set; }

    public JoinLobbyRoom(int roomCode)
    {
        RoomCode = roomCode;
    }
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        var lobbyManager = eventManagerBase as LobbyManager;
        lobbyManager.JoinMatchLobbyRoom(client,RoomCode);
    }
}
