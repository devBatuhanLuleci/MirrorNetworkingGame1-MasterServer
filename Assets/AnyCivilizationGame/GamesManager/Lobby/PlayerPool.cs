using System.Collections.Concurrent;

using UnityEngine;

public class PlayerPool : MonoBehaviour
{
    private ConcurrentDictionary<int, WarbotsPlayer> playerPool = new();

    public void AddPlayerToPool(WarbotsPlayer player)
    {
        playerPool.TryAdd(player.ConnectionId, player);
    }

    public WarbotsPlayer GetPlayerData(int connectionId)
    {
        if (playerPool.TryGetValue(connectionId, out var player))
        {
            return player;
        }
        return null;
    }

    public WarbotsPlayer GetRandomPlayer()
    {
        if (playerPool.IsEmpty)
        {
            return null;
        }

        var playerArray = new WarbotsPlayer[playerPool.Count];
        playerPool.Values.CopyTo(playerArray, 0);

        var randomIndex = UnityEngine.Random.Range(0, playerArray.Length);
        var randomPlayer = playerArray[randomIndex];
        playerPool.TryRemove(randomPlayer.ConnectionId, out _);

        return randomPlayer;
    }
}
