using System;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkEvents))]
public class SinglePeer_NetworkRunnerManager : PersistentSingleton<SinglePeer_NetworkRunnerManager>
{
    [SerializeField, Tooltip("Passed with the new runner")]
    private UnityEvent<NetworkRunner> OnRunnerInstantiated;
    
    public NetworkRunner NetworkRunner
    {
        get
        {
            if (!networkRunner)
                ReinstantiateRunner();
            return networkRunner;
        }
        private set => networkRunner = value;
    }
    [SerializeField] private NetworkRunner networkRunner;

    [SerializeField] private NetworkRunner networkRunnerPrefab;
    [SerializeField] private NetworkEvents networkEvents;
    

    private void OnValidate()
    {
        NetworkRunner = GetComponentInChildren<NetworkRunner>();
        networkEvents = GetComponent<NetworkEvents>();
    }

    protected override void Awake()
    {
        base.Awake();
        NetworkRunner.AddCallbacks(networkEvents);
        OnRunnerInstantiated.Invoke(NetworkRunner);
    }

    public void ReinstantiateRunner()
    {
        if (NetworkRunner) Destroy(NetworkRunner.gameObject);
        
        NetworkRunner = Instantiate(networkRunnerPrefab, transform);
        NetworkRunner.AddCallbacks(networkEvents);
        
        OnRunnerInstantiated.Invoke(NetworkRunner);
    }
}
