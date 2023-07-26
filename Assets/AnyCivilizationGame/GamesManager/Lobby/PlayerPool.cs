using System.Collections.Concurrent;

using UnityEngine;

public class PlayerPool : MonoBehaviour
{
    private ConcurrentQueue<WarbotsPlayer> singlePlayerQueue = new();
    private ConcurrentQueue<WarbotsPlayer> multiplayerQueue = new();

    public void AddForSinglePlayerGame(WarbotsPlayer player)
    {
        singlePlayerQueue.Enqueue(player);
    }

    public void AddForMultiplayerGame(WarbotsPlayer player)
    {
        multiplayerQueue.Enqueue(player);
    }

    public WarbotsPlayer GetForSinglePlayerGame()
    {
        if (singlePlayerQueue.IsEmpty)
        {
            return null;
        }

        if (singlePlayerQueue.TryDequeue(out var player))
        {
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
            return player;
        }

        return null;
    }
}
