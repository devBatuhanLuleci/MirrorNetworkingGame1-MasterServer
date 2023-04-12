using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class NotificationManager : Singleton<NotificationManager> {
    public enum InfoType {
        Info, //standart Debug
        InfoClient, // serverdan Clienta Debug
        InfoServer, //Clienttan servera  Debug
        InfoClientShow //  serverdan Clienta Popup
    }
    public static void Info (string message) {
        Debug.Log (message);
    }
    public static void IncomingInfo (string message, InfoType infoType) {
        switch (infoType) {
            case InfoType.InfoServer:
                Info (message);
                break;
        }
    }
    public static void SendInfoClient (ClientPeer client, string message, InfoType infoType) {
        LoadBalancer.Instance.LobbyManager.SendNotificationInfo (client, message, infoType);
    }

}