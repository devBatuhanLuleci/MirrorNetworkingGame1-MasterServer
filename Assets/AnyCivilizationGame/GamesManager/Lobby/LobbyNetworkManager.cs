using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyNetworkManager : NetworkManager
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
            NetworkServer.RegisterHandler<JoinGameMessage>(OnJoinRequest);
        }
    }

    private void OnJoinRequest(
        NetworkConnection conn,
        JoinGameMessage joinRequest)
    {
        var newPlayer = new WarbotsPlayer
        { 
            //ConnectionId = conn.ConnectionId,
        };

        if (joinRequest.gameMode == GameMode.SinglePlayer)
        {
            playerPool.AddForSinglePlayerGame(newPlayer);
        }
        else if (joinRequest.gameMode == GameMode.MultiPlayer)
        {
            playerPool.AddForMultiplayerGame(newPlayer);
        }
        else
        {
            throw new NotImplementedException("Game-mode is not supported!");
        }
    }
}
