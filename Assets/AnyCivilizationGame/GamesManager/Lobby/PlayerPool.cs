using System.Collections.Concurrent;

using UnityEngine;

public class PlayerPool : MonoBehaviour
{
    private ConcurrentQueue<WarbotsPlayer> playerQueue = new();

    public void AddToPool(WarbotsPlayer player)
    {
        playerQueue.Enqueue(player);
        Debug.Log("Player " + player.Connection.connectionId + " added to multiplayer queue.");
    }

    public WarbotsPlayer GetFromPool()
    {
        if (playerQueue.IsEmpty)
        {
            return null;
        }

        if (playerQueue.TryDequeue(out var player))
        {
            Debug.Log("Player " + player.Connection.connectionId + " dequeued from multiplayer queue.");
            return player;
        }

        return null;
    }
}
