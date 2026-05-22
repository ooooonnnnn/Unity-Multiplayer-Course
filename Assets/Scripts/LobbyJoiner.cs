using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

public class LobbyJoiner : PersistentSingleton<LobbyJoiner>
{
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

    public void ExitToLobby()
    {
        SinglePeer_NetworkRunnerManager.Instance.ReinstantiateRunner();
        JoinLobby(LobbyName);
    }
    
    private async Task JoinLobbyAsync(string lobbyName)
    {
        busy = true;
        
        OnStartJoin.Invoke();

        var networkRunner = SinglePeer_NetworkRunnerManager.Instance.NetworkRunner;
        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom, lobbyName);
        
        if (result.Ok)
        {
            LobbyName = lobbyName;
            OnJoinedLobby.Invoke();
        }
        else
        {
            print(result.ShutdownReason);
            SinglePeer_NetworkRunnerManager.Instance.ReinstantiateRunner();
            OnCancelJoin.Invoke();
        }
        
        busy = false;
    }
}
