using System;
using System.Linq;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

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
    public NetworkEvents networkEvents;
    

    private void OnValidate()
    {
        NetworkRunner = GetComponentInChildren<NetworkRunner>();
    }

    // protected override void Awake()
    // {
    //     base.Awake();
    //     if (networkEvents)
    //         NetworkRunner.AddCallbacks(networkEvents);
    //     else
    //         Debug.LogError("No network events to subscribe to");
    //
    //     OnRunnerInstantiated.Invoke(NetworkRunner);
    // }

    private void Start()
    {
        ReinstantiateRunner();
    }

    public void ReinstantiateRunner()
    {
        if (NetworkRunner) Destroy(NetworkRunner.gameObject);
        
        NetworkRunner = Instantiate(networkRunnerPrefab, transform);
        NetworkRunner.AddCallbacks(networkEvents);
        
        OnRunnerInstantiated.Invoke(NetworkRunner);
    }
}
