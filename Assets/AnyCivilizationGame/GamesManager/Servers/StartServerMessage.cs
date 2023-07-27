using System.Collections.Generic;

using Mirror;

/// <summary>
/// Message to create a game in a game-server
/// </summary>
public struct StartServerMessage : NetworkMessage
{
    public GameData gameData;
}

public struct TeamPlayerData
{
    public string playerId;
    public string nickName;
}

public struct TeamData
{
    public string teamId;
    public List<TeamPlayerData> players;
}

public struct GameData
{
    public string gameId;
    public string gameServerAddress;
    public List<TeamData> teams;
}