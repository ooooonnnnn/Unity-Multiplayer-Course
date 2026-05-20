using System.Threading.Tasks;
using Fusion;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private NetworkRunner networkRunner;

    public void JoinLobby(string lobbyName)
    {
        JoinLobbyAsync(lobbyName);
        networkRunner
    }
    
    public async Task JoinLobbyAsync(string lobbyName)
    {
        var result =  await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyName);

        if (result.Ok)
        {
            print($"Joined {lobbyName}");
        }
    }
}
