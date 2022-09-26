using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToGameServerEvent : IEvent
{
    public ushort Port { get; private set; }
    public string Host { get; private set; }

    public ConnectToGameServerEvent(ushort port, string host)
    {
        Port = port;
        Host = host;
    }
}
