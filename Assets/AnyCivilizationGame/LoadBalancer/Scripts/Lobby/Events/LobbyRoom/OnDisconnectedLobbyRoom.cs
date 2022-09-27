using ACGAuthentication;

public class OnDisconnectedLobbyRoom : IEvent
{
    public LobbyPlayer LobbyPlayer { get; private set; }
	public OnDisconnectedLobbyRoom(LobbyPlayer lobbyPlayer)
	{
		this.LobbyPlayer = lobbyPlayer;
	}
}
