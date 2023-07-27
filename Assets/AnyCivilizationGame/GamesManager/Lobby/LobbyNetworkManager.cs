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
        JoinGameMessage joinMessage)
    {
        var newPlayer = new WarbotsPlayer
        { 
            Connection = conn,
            WalletId = joinMessage.walletId,
            AccessToken = joinMessage.accessToken,
            RefreshToken = joinMessage.refreshToken
        };

        switch (joinMessage.gameMode)
        {
            case GameMode.SinglePlayer:
                playerPool.AddForSinglePlayerGame(newPlayer);
                break;
            case GameMode.MultiPlayer:
                playerPool.AddForMultiplayerGame(newPlayer);
                break;
            default:
                throw new NotImplementedException("Game-mode is not supported!");
        }
    }
}
