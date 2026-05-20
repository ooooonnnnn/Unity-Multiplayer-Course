using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SessionListUI : MonoBehaviour
{
    [SerializeField] private SessionItemUI sessionItemPrefab;
    [SerializeField] private Transform listContainer;
    
    public void ShowSessionList(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        print($"Updating session list");
        
        ClearList();

        foreach (SessionInfo sessionInfo in sessionList)
        {
            if (!sessionInfo.IsVisible)
            {
                print("Session invisible");
                continue;
            }
            
            var newItem = Instantiate(sessionItemPrefab, listContainer);
            
            newItem.SetSessionName(sessionInfo.Name);
            newItem.SetPlayerCount(sessionInfo.PlayerCount, sessionInfo.MaxPlayers);
            
            bool canJoin = sessionInfo.MaxPlayers > sessionInfo.PlayerCount &&
                           sessionInfo.IsOpen;
            newItem.SetCanJoin(canJoin);
            
            newItem.SetCallback(() => TestJoinSession(runner, sessionInfo));
        }
    }

    private void ClearList()
    {
        List<GameObject> itemsToDelete = new();
        foreach (Transform child in listContainer)
        {
            itemsToDelete.Add(child.gameObject);
        }
        itemsToDelete.ForEach(Destroy);
    }

    private async void TestJoinSession(NetworkRunner runner, SessionInfo sessionInfo)
    {
        print($"Joining session: {sessionInfo.Name}");
                
        var result = await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = sessionInfo.Name,
            IsVisible = sessionInfo.IsVisible,
            IsOpen = sessionInfo.IsOpen,
            
        });
        if (result.Ok) print("Joined session");
        print(result.ErrorMessage);
    }
}
