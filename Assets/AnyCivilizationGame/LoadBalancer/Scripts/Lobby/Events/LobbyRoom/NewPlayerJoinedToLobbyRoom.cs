using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Odaya ba?l? olan client’lara odaya yenigelen ki?i hakka?nda bilgi gönderiri. 
/// </summary>
public class NewPlayerJoinedToLobbyRoom : IEvent
{
    public LobbyPlayer player { get; private set; }

	public NewPlayerJoinedToLobbyRoom(LobbyPlayer player)
	{
		this.player = player;
	}
}
