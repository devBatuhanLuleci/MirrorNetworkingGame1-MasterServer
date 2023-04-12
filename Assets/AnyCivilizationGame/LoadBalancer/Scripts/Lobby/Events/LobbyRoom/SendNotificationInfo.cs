using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class SendNotificationInfo : IResponseEvent {
    public string NotificationInfo;
    public NotificationManager.InfoType InfoType;

    public SendNotificationInfo (string notificationInfo, NotificationManager.InfoType infoType) {
        NotificationInfo = notificationInfo;
        InfoType = infoType;
    }

    public void Invoke (EventManagerBase eventManagerBase, ClientPeer client) {
        NotificationManager.IncomingInfo (NotificationInfo, InfoType);
        /// var lobbyManager = eventManagerBase as LobbyManager;
        /// lobbyManager.SendNotificationInfo (client, NotificationInfo,InfoType);
    }
}