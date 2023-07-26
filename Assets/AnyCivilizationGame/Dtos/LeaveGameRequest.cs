using ProtoBuf;

[ProtoContract]
public class LeaveGameRequest
{
    [ProtoMember(1)]
    public string WalletId { get; set; }

    [ProtoMember(2)]
    public string AccessToken { get; set; }
}
