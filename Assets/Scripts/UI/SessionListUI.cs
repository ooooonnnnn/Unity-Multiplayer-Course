using System.Collections.Generic;
using System.Linq;
using Enums;
using Fusion;
using UnityEngine;

public class SessionListUI : MonoBehaviour
{
    [SerializeField] private SessionItemUI sessionItemPrefab;
    [SerializeField] private Transform listContainer;
    private List<SessionInfo> sessionList;

    private GameModes FilterByGameMode
    {
        get => filterByGameMode;
        set
        {
            filterByGameMode = value;
            ShowSessionList();
        }
    }

    private GameModes filterByGameMode = GameModes.Any;
    
    public void ShowSessionList(List<SessionInfo> sessionList)
    {
        this.sessionList = sessionList;
        ShowSessionList();
    }

    private void ShowSessionList()
    {
        ClearList();

        //filter by game mode
        var filteredSessionList = sessionList.Where(SessionGameModeFitsFilter);
        
        foreach (SessionInfo sessionInfo in filteredSessionList)
        {
            if (!sessionInfo.IsVisible)
            {
                Debug.LogWarning("Received hidden session! This shouldn't happen");
                continue;
            }
            
            var newItem = Instantiate(sessionItemPrefab, listContainer);
            
            newItem.SetSessionName(sessionInfo.Name);
            newItem.SetPlayerCount(sessionInfo.PlayerCount, sessionInfo.MaxPlayers);
            
            bool canJoin = sessionInfo.MaxPlayers > sessionInfo.PlayerCount &&
                           sessionInfo.IsOpen;
            newItem.SetCanJoin(canJoin);
            
            newItem.SetCallback(() => SessionJoiner.Instance.JoinSpecificSession(sessionInfo));
        }
    }

    public void SetFilterByGameMode(int gameMode) => FilterByGameMode = (GameModes)gameMode;

    private void ClearList()
    {
        List<GameObject> itemsToDelete = new();
        foreach (Transform child in listContainer)
        {
            itemsToDelete.Add(child.gameObject);
        }
        itemsToDelete.ForEach(Destroy);
    }

    private bool SessionGameModeFitsFilter(SessionInfo session)
    {
        if (FilterByGameMode == GameModes.Any) 
            return true;

        if (!session.Properties.ContainsKey("GameMode"))
        {
            Debug.LogWarning($"Session {session.Name} does not have a GameMode property");
            return true;
        }
        
        GameModes gameMode = (GameModes)session.Properties[SessionJoiner.GAMEMODE_PROPERTY_NAME].PropertyValue;
        return gameMode == FilterByGameMode;
    }
}
