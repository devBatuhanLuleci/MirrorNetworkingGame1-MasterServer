using ACGAuthentication;
using UnityEngine;

public class LeaveRoom : IResponseEvent
{
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client)
    {
        Debug.Log("LeaveRoom Invoke " + client.ConnectionId);
        var lobbyManager = (LobbyManager)eventManagerBase;
        lobbyManager.LeaveRoom(client);
    }
}
