using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SessionListUI : MonoBehaviour
{
    [SerializeField] private SessionItemUI sessionItemPrefab;
    [SerializeField] private Transform listContainer;
    
    public void ShowSessionList(List<SessionInfo> sessionList)
    {
        ClearList();

        foreach (SessionInfo sessionInfo in sessionList)
        {
            if (!sessionInfo.IsVisible)
            {
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

    private void ClearList()
    {
        List<GameObject> itemsToDelete = new();
        foreach (Transform child in listContainer)
        {
            itemsToDelete.Add(child.gameObject);
        }
        itemsToDelete.ForEach(Destroy);
    }
}
