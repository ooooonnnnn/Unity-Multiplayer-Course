using System;
using Fusion;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>, IGameManager
{
    [SerializeField] private SceneAsset gameSceneAsset;
    [SerializeField] private SceneAsset connectionSceneAsset;

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

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(connectionSceneAsset.name);
        SinglePeer_NetworkRunnerManager.Instance.ReinstantiateRunner();
    }
}
