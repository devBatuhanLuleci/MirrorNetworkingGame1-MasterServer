using Mirror;

public enum JoinStatus
{
    Waiting,
    Connecting,
    Connected,
    Disconnected
}

public struct JoinStatusMessage : NetworkMessage
{
    public JoinStatus Status;
    public string Text;
}
