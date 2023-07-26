using Mirror;

public struct JoinGameMessage : NetworkMessage
{
    public string walletId;
    public string accessToken;
    public string refreshToken;
    public GameMode gameMode;

    public JoinGameMessage(
        string walletId,
        string accessToken,
        string refreshToken,
        GameMode gameMode)
    {
        this.walletId = walletId;
        this.accessToken = accessToken;
        this.refreshToken = refreshToken;
        this.gameMode = gameMode;
    }
}

public enum GameMode
{
    SinglePlayer,
    MultiPlayer
}