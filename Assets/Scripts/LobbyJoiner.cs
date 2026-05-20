using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class LobbyJoiner : PersistentSingleton<LobbyJoiner>
{
    [SerializeField] private NetworkRunner networkRunner;
    public UnityEvent OnJoinedLobby;
    [field: SerializeField]
    public string LobbyName { get; private set; }

    public void JoinLobby(string lobbyName)
    {
        JoinLobbyAsync(lobbyName);
    }
    
    private async Task JoinLobbyAsync(string lobbyName)
    {
        var result =  await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyName);
        
        
        if (result.Ok)
        {
            print($"Joined {lobbyName}");
            LobbyName = lobbyName;
            OnJoinedLobby.Invoke();
        }
    }
}
