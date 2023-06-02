using Assets.AnyCivilizationGame.LoadBalancer.Scripts.Authentication.Requests;
using log4net;
using Mirror;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ACGAuthentication
{

    public class LoginEvent : IResponseEvent
    {

        public string AccessToken { get; set; }

        public LoginEvent(string accessToken)
        {
            AccessToken = accessToken;
        }


        public void Invoke(EventManagerBase authenticationManager, ClientPeer client)
        {
            Debug.Log($"msg recived from user : {AccessToken}");
            // TODO: get login data with AccessToken and set
            client.loginData = new LoginData();

            var req = new LoginRequest(AccessToken);
            HttpClient.Instance.Get<UserDTO>(req, (res) => OnLoginSuccess(res, client), (res) => OnLoginFail(res, client));
        }

        private void OnLoginFail(UnityWebRequest res, ClientPeer client)
        {
            Debug.LogError(res.error);
            var ev = new LoginResultEvent(false);
            LoadBalancer.Instance.AuthenticationManager.SendServerRequestToClient(client, ev);
            client.Disconnect();
        }

        private void OnLoginSuccess(UserDTO res, ClientPeer client)
        {
            client.loginData = new LoginData
            {
                Email = res.email,
                UserName = res.userName,
                WalletId = res.walletId,
                AccessToken = res.accessToken,
                Id = res.id
            };
            var ev = new LoginResultEvent(true);
            LoadBalancer.Instance.AuthenticationManager.SendServerRequestToClient(client, ev);
        }

    }


}
