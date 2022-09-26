using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ACGAuthentication
{
    public abstract class EventManagerBase
    {
        internal LoadBalancer loadBalancer;
        internal Dictionary<byte, Type> responsesByType = new Dictionary<byte, Type>();
        public abstract LoadBalancerEvent loadBalancerEvent { get; protected set; }
        public EventManagerBase(LoadBalancer loadBalancer)
        {
            this.loadBalancer = loadBalancer;
            responsesByType = initResponseTypes();

        }
        #region Handlers
        internal abstract Dictionary<byte, Type> initResponseTypes();

        internal virtual void HandleClientEvents(NetworkReader reader, ClientPeer client)
        {
            // read message type sequens
            var requestType = reader.ReadByte();
            // get reader by requestType
            Type type = responsesByType[requestType];
            // Invoke generic method for type
            try
            {
                reader.ReadIEvent(type)?.Invoke(this, client);
            }
            catch (Exception ex)
            {
                Debug.LogError("Event invoke have been failed. message: " + ex.Message);
            }

        }

        #endregion
        #region Private SendRequests 

        public virtual void SendServerRequestToClient(ClientPeer client, IEvent request)
        {
            Type type = request.GetType();
            var writer = new NetworkWriter();

            writer.WriteByte((byte)loadBalancerEvent);

            if (responsesByType.TryGetKey(type, out var key))
            {
                writer.WriteByte(key);
                writer.WriteIEvent(type, request);
                client.Send(writer.ToArraySegment());

            }
            else
            {
                throw new Exception("Request type is not found!");
            }
        }
        #endregion
    }
}