
using Singleton;
using UnityEngine;
using UnityEngine.Events;

public class MatchManager : Singleton<MatchManager>
{
    [SerializeField]
    private SpawnPoint[] spawnPoints;

    async void Start()
    {
        if (spawnPoints.Length == 0) return;

        foreach (var player in SinglePeer_NetworkRunnerManager.Instance.NetworkRunner.ActivePlayers)
        {
            var selectedSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

            await selectedSpawn.SpawnGivenPlayer(player);
        }
    }

    [SerializeField] private UnityEvent OnMatchEnded;

    public void EndMatch()
    {
        if (!SinglePeer_NetworkRunnerManager.Instance.NetworkRunner.IsSceneAuthority)
        {
            print("Can't end match, not the scene authority");
            return;
        }
        
        OnMatchEnded.Invoke();
    }
}
