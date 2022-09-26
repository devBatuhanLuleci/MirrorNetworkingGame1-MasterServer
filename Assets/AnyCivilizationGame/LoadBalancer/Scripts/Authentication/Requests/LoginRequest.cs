using Mirror;
using UnityEngine;
namespace ACGAuthentication
{

    public class LoginEvent : IResponseEvent
    {

        public string UserName { get; set; }
        public string MoralisId { get; set; }

        public LoginEvent(string userName, string moralisId)
        {
            UserName = userName;
            MoralisId = moralisId;
        }

     
        public void Invoke(EventManagerBase authenticationManager, ClientPeer client)
        {
            Debug.Log($"msg recived from user : {UserName}");
            authenticationManager.SendServerRequestToClient(client, new RegisterRequest("server@server.com", "server", "server", "server"));
        }
        
    
    }

}
