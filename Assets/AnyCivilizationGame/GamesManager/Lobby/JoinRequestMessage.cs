using Mirror;

public struct JoinRequestMessage : NetworkMessage
{
    public string walletId;
    public string accessToken;
    public string refreshToken;
    public string invitationCode;

    public JoinRequestMessage(
        string walletId,
        string accessToken,
        string refreshToken,
        string invitationCode)
    {
        this.walletId = walletId;
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
        this.invitationCode = invitationCode;
    }
}
