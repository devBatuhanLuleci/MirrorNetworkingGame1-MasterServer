using ProtoBuf;

[ProtoContract]
public class JoinGameResponse
{
    [ProtoMember(1)]
    public bool Success { get; set; }

    [ProtoMember(2)]
    public string Message { get; set; }

    [ProtoMember(3)]
    public string TeamName { get; set; }

    [ProtoMember(4)]
    public string GameServerAddress { get; set; }

    [ProtoMember(5)]
    public int JoinedPlayerCount { get; set; }

    [ProtoMember(6)]
    public GameStatus Status { get; set; }
}

[ProtoContract]
public enum GameStatus
{
    [ProtoEnum(Name = "WaitingForPlayers")]
    WaitingForPlayers = 0,

    [ProtoEnum(Name = "ReadyToStart")]
    ReadyToStart = 1,

    [ProtoEnum(Name = "InProgress")]
    InProgress = 2,

    [ProtoEnum(Name = "Completed")]
    Completed = 3,

    [ProtoEnum(Name = "Cancelled")]
    Cancelled = 4
}
