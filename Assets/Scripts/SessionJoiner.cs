using System;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class SessionJoiner : PersistentSingleton<SessionJoiner>
{
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
        OnStartJoin.Invoke();
        
        var result = await runner.StartGame(args);
        
        if (result.Ok)
        {
            OnJoined.Invoke(runner);
        }
        else
        {
            SinglePeer_NetworkRunnerManager.Instance.ReinstantiateRunner();
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
