using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void QuitGame()
    {
        print("Quitting game");
        Application.Quit();
    }
}
