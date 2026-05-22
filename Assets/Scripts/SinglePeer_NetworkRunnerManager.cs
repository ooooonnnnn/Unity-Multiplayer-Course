using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkEvents))]
public class SinglePeer_NetworkRunnerManager : PersistentSingleton<SinglePeer_NetworkRunnerManager>
{
    [field: SerializeField] 
    public NetworkRunner NetworkRunner {get; private set;}
    [SerializeField] private NetworkRunner networkRunnerPrefab;
    [SerializeField] private NetworkEvents networkEvents;
    

    private void OnValidate()
    {
        NetworkRunner = GetComponentInChildren<NetworkRunner>();
        networkEvents = GetComponent<NetworkEvents>();
    }

    public void ReinstantiateRunner()
    {
        if (NetworkRunner) Destroy(NetworkRunner.gameObject);
        
        NetworkRunner = Instantiate(networkRunnerPrefab, transform);
        NetworkRunner.AddCallbacks(networkEvents);
    }
}
