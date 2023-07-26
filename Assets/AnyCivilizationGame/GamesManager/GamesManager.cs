using System.Collections.Concurrent;
using System.Collections.Generic;

using UnityEngine;

public class GamesManager : MonoBehaviour
{
    private static GamesManager instance;

    public ConcurrentDictionary<string, WarbotsGame> GamesList { get; private set; }

    public static GamesManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GamesManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GamesManager");
                    instance = singletonObject.AddComponent<GamesManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GamesList = new();
    }
}
