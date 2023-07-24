using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSetupManager : NetworkManager
{
    // Register the custom network message during server startup
    public override void Awake()
    {
        base.Awake();
        NetworkServer.RegisterHandler<JoinRequestMessage>(OnJoinRequest);
    }

    private void OnJoinRequest(NetworkConnection conn, JoinRequestMessage joinRequest)
    {
        // Handle the 'join' request data here
        // For example, you can create a room or add the player to an existing room.

        // After handling the join request, you need to spawn the player prefab.
        // Here, we assume you have a Player prefab in the Resources folder.
        //GameObject player = Instantiate(playerPrefab);
        //NetworkServer.AddPlayerForConnection(conn, player);
    }
}
