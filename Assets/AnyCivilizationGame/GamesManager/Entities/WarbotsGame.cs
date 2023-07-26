using System.Collections.Concurrent;
using System.Collections.Generic;

public class WarbotsGame
{
    public string GameId { get; set; }
    public string GameServerAddress { get; set; }
    public ConcurrentDictionary<string, WarbotsTeam> Teams { get; } = new();
}

public enum WarbotsGameState
{
    NotReady,
    Ready,
    Connected,
    Canceled,
    GameOver
}
