using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum ServerMode
{
    Debug,
    Local,
    Container
}

public class GameServerFactory : MonoBehaviour
{
    public ServerMode ServerMode;
    public string DebugServerAddress;
    public float CheckGameSetupsInterval = 1f;

    private MultiplayerGamesManager gamesManager;
    private MultiplayerGameSetupFactory gameSetupFactory;

    private void Awake()
    {
        gamesManager = FindObjectOfType<MultiplayerGamesManager>();
        gameSetupFactory = FindObjectOfType<MultiplayerGameSetupFactory>();
        if (gamesManager == null)
        {
            Debug.LogError("MultiplayerGamesManager component not found in the scene!");
            return;
        }
        if (gameSetupFactory == null)
        {
            Debug.LogError("MultiplayerGameSetupManager component not found in the scene!");
            return;
        }

        StartCoroutine(CreateGameServersForGameSetups());
    }

    private IEnumerator CreateGameServersForGameSetups()
    {
        while (true)
        {
            foreach (var game in gamesManager.GamesList.Values)
            {
                if (IsGameReady(game))
                {
                    CreateGameServer(game);
                }
            }

            yield return new WaitForSeconds(CheckGameSetupsInterval);
        }
    }

    private bool IsGameReady(WarbotsGame game)
    {
        if (game.Teams.Count != gameSetupFactory.TeamsPerGame)
        {
            return false;
        }

        foreach (var team in game.Teams.Values)
        {
            if (team.Players.Count != gameSetupFactory.PlayersPerTeam)
            {
                return false;
            }
        }

        return true;
    }

    private void CreateGameServer(WarbotsGame game)
    {
        Debug.Log("Creating game server for game: " + game.GameId);
    }
}
