using Singleton;
using UnityEngine;
using UnityEngine.Events;

public class MatchManager : Singleton<MatchManager>
{
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
