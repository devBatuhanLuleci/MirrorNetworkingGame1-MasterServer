using ACGAuthentication;
using UnityEngine;

public class CreateLobbyRoom : IResponseEvent
{
 
    public CreateLobbyRoom()
    {
    }
    public void Invoke(EventManagerBase authenticationManager, ClientPeer client)
    {
        Debug.Log("StartMatch Invoked.");
        var lobbyManager = authenticationManager as LobbyManager;
        lobbyManager.CreateLobbyRoom(client);
    }
}
