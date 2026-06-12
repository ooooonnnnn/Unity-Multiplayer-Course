using System;
using Fusion;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private SceneAsset gameSceneAsset;

    public void QuitGame()
    {
        print("Quitting game");
        Application.Quit();
    }

    public void StartGame()
    {
        NetworkRunner runner = SinglePeer_NetworkRunnerManager.Instance.NetworkRunner;
        if (!runner.IsSceneAuthority)
        {
            Debug.LogWarning("Cannot start game, not the scene authority");
            return;
        }

        var loadAsyncOp = runner.LoadScene(gameSceneAsset.name, LoadSceneMode.Single);
        
        runner.SessionInfo.IsOpen = false;
        
        UIManager.Instance.ShowWaitingScreen();
    }
}
