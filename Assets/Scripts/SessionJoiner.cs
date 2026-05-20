using System;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class SessionJoiner : PersistentSingleton<SessionJoiner>
{
    [SerializeField] private NetworkRunner networkRunner;
    public string SessionName { get; set; }
    [SerializeField] private int playerCapacity;
    [SerializeField] private int maxCapacity;
    public UnityEvent<int> OnCapacityChanged;
    public UnityEvent OnStartJoin;
    public UnityEvent<NetworkRunner> OnJoined;
    public UnityEvent OnCancelJoin;

    private void Start()
    {
        OnCapacityChanged.Invoke(playerCapacity);
    }

    public void JoinSessionFromSettings()
    {
        JoinSession(networkRunner, new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = SessionName,
            CustomLobbyName = LobbyJoiner.Instance.LobbyName,
            PlayerCount = playerCapacity
        });
    }

    public void JoinSpecificSession(NetworkRunner runner, SessionInfo sessionInfo)
    {
        JoinSession(runner, new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = sessionInfo.Name,
            CustomLobbyName = LobbyJoiner.Instance.LobbyName
        });
    }

    private async void JoinSession(NetworkRunner runner, StartGameArgs args)
    {
        var result = runner.StartGame(args);
        OnStartJoin.Invoke();
        
        await result;
        
        if (result.Result.Ok)
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
}
