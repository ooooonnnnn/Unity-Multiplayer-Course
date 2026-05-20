using Fusion;
using UnityEngine;

public class SessionJoiner : MonoBehaviour
{
    [SerializeField] private NetworkRunner networkRunner;
    
    public void TestCreateSession()
    {
        networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "TestSession",
            CustomLobbyName = LobbyJoiner.Instance.LobbyName
        });
    }
}
