using ACGAuthentication;

/// <summary>
/// Odadakilere odadan bir client’?n ayr?ld???n? bildirir. 
/// </summary>
public class OnLeaveLobbyRoom : IEvent
{
    public LobbyPlayer LobbyPlayer { get; private set; }
	public OnLeaveLobbyRoom(LobbyPlayer lobbyPlayer)
	{
		this.LobbyPlayer = lobbyPlayer;
	}
}
