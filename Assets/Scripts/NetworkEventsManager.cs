using System;
using Fusion;
using Singleton;
using UnityEngine;

[RequireComponent(typeof(NetworkEvents))]
public class NetworkEventsManager : Singleton<NetworkEventsManager>
{
    [SerializeField, HideInInspector] private NetworkEvents networkEvents;
    
    private void OnValidate()
    {
        networkEvents = GetComponent<NetworkEvents>();
    }

    protected void Start()
    {
        SinglePeer_NetworkRunnerManager.Instance.networkEvents = networkEvents;
        SinglePeer_NetworkRunnerManager.Instance.ReinstantiateRunner();
    }
}
