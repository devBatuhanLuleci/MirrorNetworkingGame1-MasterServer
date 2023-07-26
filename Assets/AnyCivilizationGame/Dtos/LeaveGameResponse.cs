using ProtoBuf;

[ProtoContract]
public class LeaveGameResponse
{
    [ProtoMember(1)]
    public bool Success { get; set; }

    [ProtoMember(2)]
    public string Message { get; set; }
}
