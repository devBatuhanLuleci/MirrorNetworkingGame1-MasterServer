using ACGAuthentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResponseEvent : IEvent
{

    /// <summary>
    /// Write server logics
    /// </summary>
    /// <param name="authenticationManager"></param>
    /// <param name="client"></param>
    public void Invoke(EventManagerBase eventManagerBase, ClientPeer client);
}
