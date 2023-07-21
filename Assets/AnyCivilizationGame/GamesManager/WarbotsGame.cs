using System.Collections.Generic;

public class WarbotsGame
{
    public string GameId { get; set; }
    public string GameServerAddress { get; set; }
    public List<WarbotsTeam> Teams { get; } = new();
}
