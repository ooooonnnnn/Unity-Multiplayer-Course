using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class LobbyJoiner : PersistentSingleton<LobbyJoiner>
{
    [SerializeField] private NetworkRunner networkRunner;
    public UnityEvent OnJoinedLobby;
    public UnityEvent OnStartJoin;
    public UnityEvent OnCancelJoin;
    [field: SerializeField]
    public string LobbyName { get; private set; }
    private bool busy;

    public void JoinLobby(string lobbyName)
    {
        if (busy) return;
        JoinLobbyAsync(lobbyName);
    }
    
    private async Task JoinLobbyAsync(string lobbyName)
    {
        busy = true;
        
        OnStartJoin.Invoke();
        
        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyName);
        
        if (result.Ok)
        {
            LobbyName = lobbyName;
            OnJoinedLobby.Invoke();
        }
        else
        {
            print(result.ShutdownReason);
            OnCancelJoin.Invoke();
        }
        
        busy = false;
    }
}
