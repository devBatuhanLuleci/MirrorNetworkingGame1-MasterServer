using Assets.AnyCivilizationGame.LoadBalancer.Scripts.Authentication.Requests;
using Mirror;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ACGAuthentication
{
    public class ACGAuthenticationManager : EventManagerBase
    {
        public override LoadBalancerEvent loadBalancerEvent { get; protected set; } = LoadBalancerEvent.Authentication;

        public ACGAuthenticationManager(LoadBalancer loadBalancer) : base(loadBalancer)
        {
            loadBalancer.AddEventHandler(loadBalancerEvent, this);
        }
        ~ACGAuthenticationManager()
        {
            loadBalancer.RemoveEventHandler(loadBalancerEvent, this);
        }


        internal override Dictionary<byte, Type> initResponseTypes()
        {
            var responseTypes = new Dictionary<byte, Type>();
            responseTypes.Add((byte)AuthenticationEvent.Login, typeof(LoginEvent));
            responseTypes.Add((byte)AuthenticationEvent.Create, typeof(RegisterRequest));
            responseTypes.Add((byte)AuthenticationEvent.LoginResultEvent, typeof(LoginResultEvent));
            
            return responseTypes;
        }


    }
}