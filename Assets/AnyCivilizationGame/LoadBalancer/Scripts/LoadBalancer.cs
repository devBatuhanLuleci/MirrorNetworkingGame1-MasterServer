
using ACGAuthentication;
using log4net;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LoadBalancer : Singleton<LoadBalancer>
{
    [Header("Listener Setup")]
    [SerializeField] private bool startServerOnStart = true;
    [Space]
    [SerializeField] private Transport transport;
    private static readonly ILog log = LogManager.GetLogger(typeof(LoadBalancer));


    private bool isServer = false;
    private bool isClient = false;

    #region Public Fields
    // store all users of connected to lobby
    public Dictionary<int, ClientPeer> clients { get; private set; } = new Dictionary<int, ClientPeer>();
    #endregion

    private Dictionary<byte, EventManagerBase> eventHandlers = new Dictionary<byte, EventManagerBase>();

    #region Managers
    public ACGAuthenticationManager AuthenticationManager { get; private set; }
    public SpawnServer SpawnServer { get; private set; }
    public LobbyManager LobbyManager { get; private set; }

    #endregion

    #region MonoBehavior Callcks

    private void Start()
    {
        log.Debug($"Loadbalancer Started");

        Application.runInBackground = true;
        if (transport == null)
        {
            Debug.LogError("Transport not found!");
            return;
        }
        // setup client and server listeners callbacks
        SetupLinstener();


        if (startServerOnStart)
        {
            isServer = true;
            transport.ServerStart();
        }

        SetupManagers();

    }

    private void SetupManagers()
    {
        AuthenticationManager = new ACGAuthenticationManager(this);
        SpawnServer = new SpawnServer(this);
        LobbyManager = new LobbyManager(this);
    }

    private void Update()
    {
        if (isServer)
        {
            transport.ServerEarlyUpdate();
        }
        if (isClient)
        {
            transport.ClientEarlyUpdate();
        }
    }
    private void LateUpdate()
    {
        if (isServer)
        {
            transport.ServerLateUpdate();
        }
        if (isClient)
        {
            transport.ClientLateUpdate();
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    // called when quitting the application by closing the window / pressing
    // stop in the editor. virtual so that inheriting classes'
    // OnApplicationQuit() can call base.OnApplicationQuit() too
    public virtual void OnApplicationQuit()
    {
        // stop client first
        // (we want to send the quit packet to the server instead of waiting
        //  for a timeout)
        if (isServer)
        {
            transport.ServerStop();
        }
    }

    #endregion

    #region SetupListeners

    #region Remove
    private void RemoveListeners()
    {
        RemoveServerListeners();
    }
    private void RemoveServerListeners()
    {
        transport.OnServerConnected -= OnServerConnected;
        transport.OnServerDataReceived -= OnServerDataReceived;
        transport.OnServerDataSent -= OnServerDataSent;
        transport.OnServerDisconnected -= OnServerDisconnected;
        transport.OnServerError -= OnServerError;
    }

    #endregion
    private void SetupLinstener()
    {
        AddServerListeners();
    }

    private void AddServerListeners()
    {
        transport.OnServerConnected += OnServerConnected;
        transport.OnServerDataReceived += OnServerDataReceived;
        transport.OnServerDataSent += OnServerDataSent;
        transport.OnServerDisconnected += OnServerDisconnected;
        transport.OnServerError += OnServerError;
    }

    private void OnServerError(int arg1, Exception ex)
    {
        Debug.LogError("Server Error. Message: " + ex.Message);
    }

    private void OnServerDisconnected(int connectionId)
    {
        if (clients.TryGetValue(connectionId, out var client))
        {
            client.OnDisconnected();
            clients.Remove(connectionId);
        }
        //Debug.Log("Client DisConnected: " + connectionId.ToString());

    }

    private void OnServerDataSent(int connectionId, ArraySegment<byte> data, int arg3)
    {
    }

    private void OnServerDataReceived(int connectionId, ArraySegment<byte> data, int arg3)
    {
        Debug.Log("OnServerDataReceived");

        if (!clients.TryGetValue(connectionId, out var client))
        {
            Debug.LogError("Unknow client");
            client.Disconnect();
            return;
        }

        if (!HandleAuth(client, data)) return;

        var reader = new NetworkReader(data);
        var type = reader.ReadByte(); // read message type sequens
        if (eventHandlers.TryGetValue(type, out var handler))
        {

            handler.HandleClientEvents(reader, client);
        }
        else
        {
            throw new Exception($"Event handler not found! type: {type}");
        }

    }

    private bool HandleAuth(ClientPeer client, ArraySegment<byte> data)
    {
        var reader = new NetworkReader(data);
        var typeHandler = reader.ReadByte(); // read message type sequens
        var typeReq = reader.ReadByte(); // read message type sequens


        if (!client.IsAuth && typeHandler != (byte)LoadBalancerEvent.Authentication && typeReq != (byte)AuthenticationEvent.Login)
        {
            Debug.Log("Client not auth!");
            client.Disconnect();
            return false;
        }
        return true;
    }

    private void OnServerConnected(int connectionId)
    {
        // if client allready connected
        // Disconnect old client
        if (clients.ContainsKey(connectionId))
        {
            clients[connectionId].OnDisconnected();
            clients.Remove(connectionId);
        }

        var client = new ClientPeer(this, connectionId);
        clients.Add(connectionId, client);
        Debug.Log("Client connected: " + connectionId.ToString());
    }


    #endregion

    #region Public Methods

    public void ServerSend(int connectionId, ArraySegment<byte> segment, int channelId = Channels.Reliable)
    {
        transport.ServerSend(connectionId, segment, channelId);
    }
    public void ServerDisconnect(int connectionId)
    {
        transport.ServerDisconnect(connectionId);
    }
    #endregion

    #region EventManagerHandlers
    public void AddEventHandler(LoadBalancerEvent eventKey, EventManagerBase eventValue)
    {
        if (eventHandlers.ContainsKey((byte)eventKey))
        {
            eventHandlers[(byte)eventKey] = eventValue;
        }
        else
        {
            eventHandlers.Add((byte)eventKey, eventValue);
        }
    }
    public void RemoveEventHandler(LoadBalancerEvent eventKey, EventManagerBase eventValue)
    {
        if (eventHandlers.ContainsKey((byte)eventKey))
        {
            eventHandlers.Remove((byte)eventKey);
        }
    }
    public T GetEventHandler<T>(LoadBalancerEvent eventKey) where T : EventManagerBase
    {
        if (eventHandlers.TryGetValue((byte)eventKey, out var eventManagerBase))
        {
            return eventManagerBase as T;
        }
        return null;
    }

    internal object FindMatch(LobbyRoom lobbyRoom)
    {
        throw new NotImplementedException();
    }
    #endregion


}
