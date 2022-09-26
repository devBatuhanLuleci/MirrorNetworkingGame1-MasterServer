using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetPlayersEvent : IResponseEvent
{
    public void Invoke(EventManagerBase authenticationManager, ClientPeer client)
    {
        Debug.Log("GetFriendsEvent Invoked.");
        var clients = LoadBalancer.Instance.clients;
        var count = clients.Count;
        var players = new int[count];
        for (int i = 0; i < count; i++)
        {
            var connectionId = clients.ElementAt(i).Value.ConnectionId;
            players[i] = connectionId;
            Debug.Log("player: " + players[i]);

        }
        Debug.Log("Method not implement");
        //authenticationManager.loadBalancer.LobbyManager.SendServerRequestToClient(client, ev);
    }
}
