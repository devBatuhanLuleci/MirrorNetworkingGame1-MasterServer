using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameLobbyManager : NetworkManager
{
    private PlayerPool playerPool;

    public override void Awake()
    {
        playerPool = FindObjectOfType<PlayerPool>();

        if (playerPool == null)
        {
            Debug.LogError("PlayerPool component not found in the scene!");
        }
        else
        {
            NetworkServer.RegisterHandler<JoinRequestMessage>(OnJoinRequest);
        }
    }

    private void OnJoinRequest(NetworkConnection conn, JoinRequestMessage joinRequest)
    {
        var newPlayer = new WarbotsPlayer
        { 
            //ConnectionId = conn.ConnectionId,
        };

        playerPool.AddPlayerToPool(newPlayer);
    }
}
