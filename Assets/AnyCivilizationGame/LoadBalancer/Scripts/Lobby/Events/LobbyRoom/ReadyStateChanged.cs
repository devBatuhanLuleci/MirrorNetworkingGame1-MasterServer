using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyStateChanged : IEvent
{
    public LobbyPlayer lobbyPlayer { get; private set; }
	public ReadyStateChanged(LobbyPlayer lobbyPlayer)
	{
		this.lobbyPlayer = lobbyPlayer;
	}
}
