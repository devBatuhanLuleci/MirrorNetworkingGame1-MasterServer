using ACGAuthentication;
using Assets.AnyCivilizationGame.LoadBalancer.Scripts.Authentication.Requests;
using System;
using UnityEditor.Experimental.GraphView;

public class DateMessageResponseEvent : IResponseEvent
{
   
    public string accessToken;


  

    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        
        //TODO : send a message to game-server
         DateTime now = DateTime.Now;
        var ev = new DateMessageRequestEvent(now.ToString(),accessToken);
        (eventManagerBase as SpawnServer).SendServerRequestToClient(client, ev);

    }
}