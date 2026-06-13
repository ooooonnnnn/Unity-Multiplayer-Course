using System;
using UnityEngine;

public class GameManagerProxy : MonoBehaviour, IGameManager
{
    private GameManager gameManagerInstance;
    
    private void Start()
    {
        gameManagerInstance = GameManager.Instance;
        if (!gameManagerInstance)
            Debug.LogError("GameManager not found");
    }

    public void QuitGame() => gameManagerInstance.QuitGame();

    public void StartGame() => gameManagerInstance.StartGame();

    public void ReturnToMenu() => gameManagerInstance.ReturnToMenu();
}
