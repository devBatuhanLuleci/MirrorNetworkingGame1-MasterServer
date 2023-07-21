using System.Collections.Generic;

public class WarbotsTeam
{
    public string TeamName { get; set; }
    public List<WarbotsPlayer> Players { get; } = new();
}
