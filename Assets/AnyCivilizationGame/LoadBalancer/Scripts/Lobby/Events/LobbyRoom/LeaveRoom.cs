using ACGAuthentication;
using UnityEngine;

/// <summary>
/// Bir oyuncun odadan ayr?lma iste?inde bulunmas?n? sa?lar. 
/// </summary>
public class LeaveRoom : IResponseEvent
{
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        Debug.Log("LeaveRoom Invoke " + client.ConnectionId);
        var lobbyManager = (LobbyManager)eventManagerBase;
        lobbyManager.LeaveRoom(client);
    }
}
