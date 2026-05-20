using System;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class SessionJoiner : MonoBehaviour
{
    [SerializeField] private NetworkRunner networkRunner;
    public string SessionName { get; set; }
    [SerializeField] private int playerCapacity;
    [SerializeField] private int maxCapacity;
    public UnityEvent<int> OnCapacityChanged;

    private void Start()
    {
        OnCapacityChanged.Invoke(playerCapacity);
    }

    public void TestCreateSession()
    {
        networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = SessionName,
            CustomLobbyName = LobbyJoiner.Instance.LobbyName,
            PlayerCount = playerCapacity
        });
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
