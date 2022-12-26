using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Odadaki bir oyuncunun haz?r oldu?u bilgisini bir oyuncuya bildirir. 
/// </summary>
public class ReadyStateChanged : IEvent
{
    public LobbyPlayer lobbyPlayer { get; private set; }
	public ReadyStateChanged(LobbyPlayer lobbyPlayer)
	{
		this.lobbyPlayer = lobbyPlayer;
	}
}
