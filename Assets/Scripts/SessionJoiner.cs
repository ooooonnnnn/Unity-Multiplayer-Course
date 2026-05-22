using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class SessionJoiner : PersistentSingleton<SessionJoiner>
{
    [Header("Custom Session Settings")]
    public string SessionName { get; set; }
    [SerializeField] private int playerCapacity;
    [SerializeField] private int maxCapacity;
    
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
            PlayerCount = playerCapacity
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
    
    public void UpdateAvailableSessions(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        availableSessions = new List<SessionInfo>(sessionList);
        OnAvaliableSessionsChanged.Invoke(availableSessions);
    }
}
