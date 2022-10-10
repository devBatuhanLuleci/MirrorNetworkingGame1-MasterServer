using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThereIsNoRoom : IEvent {
    public int RoomId { get; set; }
	public ThereIsNoRoom(int roomId)
	{
		RoomId = roomId;
	}
}
