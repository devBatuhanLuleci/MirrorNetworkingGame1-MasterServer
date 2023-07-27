using System.Collections.Concurrent;

using UnityEngine;

public class PlayerPool : MonoBehaviour
{
    private ConcurrentQueue<WarbotsPlayer> singlePlayerQueue = new();
    private ConcurrentQueue<WarbotsPlayer> multiplayerQueue = new();

    public void AddForSinglePlayerGame(WarbotsPlayer player)
    {
        singlePlayerQueue.Enqueue(player);
        Debug.Log("Player " + player.Connection.connectionId + " added to single-player queue.");
    }

    public void AddForMultiplayerGame(WarbotsPlayer player)
    {
        multiplayerQueue.Enqueue(player);
        Debug.Log("Player " + player.Connection.connectionId + " added to multiplayer queue.");
    }

    public WarbotsPlayer GetForSinglePlayerGame()
    {
        if (singlePlayerQueue.IsEmpty)
        {
            return null;
        }

        if (singlePlayerQueue.TryDequeue(out var player))
        {
            Debug.Log("Player " + player.Connection.connectionId + " dequeued from single-player queue.");
            return player;
        }

        return null;
    }

    public WarbotsPlayer GetForMultiplayerGame()
    {
        if (multiplayerQueue.IsEmpty)
        {
            return null;
        }

        if (multiplayerQueue.TryDequeue(out var player))
        {
            Debug.Log("Player " + player.Connection.connectionId + " dequeued from multiplayer queue.");
            return player;
        }

        return null;
    }
}
