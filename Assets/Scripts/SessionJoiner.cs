using System;
using System.Collections.Generic;
using Enums;
using Fusion;
using UnityEngine;
using UnityEngine.Events;
using Singleton;

public class SessionJoiner : Singleton<SessionJoiner>
{
    public const string GAMEMODE_PROPERTY_NAME = "GameMode";

    [Header("Custom Session Settings")]
    public string SessionName { get; set; }
    [SerializeField] private int playerCapacity;
    [SerializeField] private int maxCapacity;
    [SerializeField] private bool isVisible;
    [SerializeField] private GameModes gameMode = GameModes.Fun;
    
    [Header("Events")]
    public UnityEvent<int> OnCapacityChanged;
    public UnityEvent OnStartJoin;
    public UnityEvent<NetworkRunner> OnJoined;
    public UnityEvent OnCancelJoin;
    public UnityEvent<List<SessionInfo>> OnAvaliableSessionsChanged;
    
    private List<SessionInfo> availableSessions;

    private void Start()
    {
        OnCapacityChanged.Invoke(playerCapacity);
    }

    public void JoinCustomSession()
    {
        print($"Joining session: {SessionName}");
        var networkRunner = SinglePeer_NetworkRunnerManager.Instance.NetworkRunner;
        JoinSession(networkRunner, new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = SessionName,
            CustomLobbyName = LobbyJoiner.Instance.LobbyName,
            PlayerCount = playerCapacity,
            IsVisible = isVisible,
            SessionProperties = new Dictionary<string, SessionProperty>
            {
                {GAMEMODE_PROPERTY_NAME, (int)gameMode}
            }
        });
    }

    public void JoinSpecificSession(SessionInfo sessionInfo)
    {
        var runner = SinglePeer_NetworkRunnerManager.Instance.NetworkRunner;
        JoinSession(runner, new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = sessionInfo.Name,
            CustomLobbyName = LobbyJoiner.Instance.LobbyName
        });
    }

    private async void JoinSession(NetworkRunner runner, StartGameArgs args)
    {
        OnStartJoin.Invoke();
        
        var result = await runner.StartGame(args);
        
        if (result.Ok)
        {
            OnJoined.Invoke(runner);
        }
        else
        {
            OnCancelJoin.Invoke();
        }
    }

    public void IncreasePlayerCapacity()
    {
        playerCapacity = Mathf.Clamp(playerCapacity + 1, 1, maxCapacity);
        OnCapacityChanged.Invoke(playerCapacity);
    }
    
    public void DecreasePlayerCapacity()
    {
        playerCapacity = Mathf.Clamp(playerCapacity - 1, 1, maxCapacity);
        OnCapacityChanged.Invoke(playerCapacity);
    }
    
    public void SetVisibleFromPrivate(bool isPrivate) => isVisible = !isPrivate;

    public void SetGameMode(GameModes gameMode)
    {
        if (gameMode == GameModes.Any)
            throw new ArgumentOutOfRangeException(nameof(gameMode), "Cannot set game mode to Any");
        
        this.gameMode = gameMode;
    }
    
    public void UpdateAvailableSessions(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        availableSessions = new List<SessionInfo>(sessionList);
        OnAvaliableSessionsChanged.Invoke(availableSessions);
    }
}
