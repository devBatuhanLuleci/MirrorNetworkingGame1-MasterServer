using ProtoBuf;

[ProtoContract]
public class JoinGameRequest
{
    [ProtoMember(1)]
    public string WalletId { get; set; }

    [ProtoMember(2)]
    public string AccessToken { get; set; }

    [ProtoMember(3)]
    public string RefreshToken { get; set; }

    [ProtoMember(4)]
    public string InvitationCode { get; set; }
}
