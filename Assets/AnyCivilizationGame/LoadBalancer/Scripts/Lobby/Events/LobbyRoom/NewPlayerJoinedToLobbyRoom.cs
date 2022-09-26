using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerJoinedToLobbyRoom : IEvent
{
    public LobbyPlayer player { get; private set; }

	public NewPlayerJoinedToLobbyRoom(LobbyPlayer player)
	{
		this.player = player;
	}
}
