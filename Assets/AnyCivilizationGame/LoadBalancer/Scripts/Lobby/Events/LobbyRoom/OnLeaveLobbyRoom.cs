using ACGAuthentication;

public class OnLeaveLobbyRoom : IEvent
{
    public LobbyPlayer LobbyPlayer { get; private set; }
	public OnLeaveLobbyRoom(LobbyPlayer lobbyPlayer)
	{
		this.LobbyPlayer = lobbyPlayer;
	}
}
