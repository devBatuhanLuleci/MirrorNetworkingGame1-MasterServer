using System.Collections;
using System.Collections.Generic;
using ACGAuthentication;
using UnityEngine;

public class OnSendNotificationInfo : IEvent {
    public string NotificationInfo;
    public NotificationManager.InfoType InfoType;
    public OnSendNotificationInfo (string notificationInfo,NotificationManager.InfoType infoType) {
        NotificationInfo = notificationInfo;
        InfoType=infoType;
    }
}