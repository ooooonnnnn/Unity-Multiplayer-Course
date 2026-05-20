using Fusion;
using UnityEngine;

public class SessionJoiner : MonoBehaviour
{
    [SerializeField] private NetworkRunner networkRunner;
    public string SessionName { get; set; }
    
    public void TestCreateSession()
    {
        networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = SessionName,
            CustomLobbyName = LobbyJoiner.Instance.LobbyName
        });
    }

    public static string SessionNameCreator()
    {
        return Random.Range(0, 1000000).ToString();
    }
}
