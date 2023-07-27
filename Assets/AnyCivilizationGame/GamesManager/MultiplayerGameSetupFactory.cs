using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MultiplayerGameSetupFactory : MonoBehaviour
{
    public int PlayersPerTeam = 2;
    public int TeamsPerGame = 2;
    public float CheckNewPlayersInterval = 1f;

    private PlayerPool playerPool;
    private MultiplayerGamesManager gamesManager;

    private void Awake()
    {
        playerPool = FindObjectOfType<PlayerPool>();
        if (playerPool == null)
        {
            Debug.LogError("PlayerPool component not found in the scene!");
        }

        gamesManager = FindObjectOfType<MultiplayerGamesManager>();
        if (gamesManager == null)
        {
            Debug.LogError("MultiplayerGamesManager component not found in the scene!");
        }

        StartCoroutine(CheckForNewPlayers());
    }

    private IEnumerator CheckForNewPlayers()
    {
        while (true)
        {
            WarbotsPlayer player = playerPool.GetForMultiplayerGame();

            if (player != null)
            {
                bool playerAdded = false;

                foreach (var game in gamesManager.GamesList.Values)
                {
                    foreach (var team in game.Teams.Values)
                    {
                        if (team.Players.Count < PlayersPerTeam)
                        {
                            team.Players.TryAdd(player.Connection.connectionId, player);
                            playerAdded = true;
                            SendJoinStatusMessageToTeamPlayers(team, JoinStatus.Waiting, GetJoinStatusText(team));
                            break;
                        }
                    }

                    if (playerAdded)
                    {
                        break;
                    }
                }

                if (!playerAdded)
                {
                    var newGame = new WarbotsGame();
                    gamesManager.GamesList.TryAdd(newGame.GameId, newGame);

                    var newTeam = new WarbotsTeam();
                    newGame.Teams.TryAdd(newTeam.TeamId, newTeam);

                    newTeam.Players.TryAdd(player.Connection.connectionId, player);
                    SendJoinStatusMessage(player, JoinStatus.Waiting, GetJoinStatusText(newTeam));
                }
            }

            yield return new WaitForSeconds(CheckNewPlayersInterval);
        }
    }

    private void SendJoinStatusMessage(WarbotsPlayer player, JoinStatus status, string text)
    {
        JoinStatusMessage joinStatusMessage = new JoinStatusMessage
        {
            Status = status,
            Text = text
        };

        player.Connection.Send(joinStatusMessage);
    }

    private void SendJoinStatusMessageToTeamPlayers(WarbotsTeam team, JoinStatus status, string text)
    {
        foreach (var player in team.Players.Values)
        {
            SendJoinStatusMessage(player, status, text);
        }
    }

    private string GetJoinStatusText(WarbotsTeam team)
    {
        int currentPlayers = team.Players.Count;
        int totalPlayers = PlayersPerTeam;
        return currentPlayers + " / " + totalPlayers;
    }
}
