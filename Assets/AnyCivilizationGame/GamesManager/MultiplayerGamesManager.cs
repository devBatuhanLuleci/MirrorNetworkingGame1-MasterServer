using System.Collections.Concurrent;
using System.Collections.Generic;

using UnityEngine;

public class MultiplayerGamesManager : MonoBehaviour
{
    public ConcurrentDictionary<string, WarbotsGame> GamesList { get; private set; }

    private void Awake()
    {
        GamesList = new ConcurrentDictionary<string, WarbotsGame>();
    }
}
