using System;
using Fusion;
using UnityEngine;

public class ObjectActiveByRunner : MonoBehaviour
{
    [SerializeField] private GameObject[] sceneAuthExclusiveObjs;

    public void HandleNewRunner(NetworkRunner newRunner)
    {
        var isSceneAuth = newRunner.IsSceneAuthority;
        foreach (var obj in sceneAuthExclusiveObjs)
            obj.SetActive(isSceneAuth);
    }

    private void OnEnable()
    {
        if (SinglePeer_NetworkRunnerManager.Instance)
            HandleNewRunner(SinglePeer_NetworkRunnerManager.Instance.NetworkRunner);
    }
}
