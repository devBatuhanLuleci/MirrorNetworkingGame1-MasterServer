using System.Collections.Concurrent;
using System.Collections.Generic;

public class WarbotsTeam
{
    public string TeamId { get; set; }
    public ConcurrentDictionary<int, WarbotsPlayer> Players { get; } = new();
}
