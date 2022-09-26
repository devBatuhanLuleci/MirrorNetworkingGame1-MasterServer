using ACGAuthentication;
using UnityEngine;

public class RoomReadyEvent : IEvent
{
    public string Address { get; private set; }
    public int Port { get; private set; }

    public RoomReadyEvent(string address, int port)
    {
        Address = address;
        Port = port;
    }
    
    public void Invoke(EventManagerBase authenticationManager, ClientPeer client)
    {
        Debug.Log("Oda haz?r!");
    }
}
