using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class ActivePlayersUI : MonoBehaviour
{
    [SerializeField] private Transform playerListContainer;
    [SerializeField] private TMP_Text playerItemPrefab;
    
    public void HandlePlayerJoinLeave(NetworkRunner runner, PlayerRef player) => UpdateActivePlayers(runner);

    public void UpdateActivePlayers(NetworkRunner runner)
    {
        ClearList();

        foreach (var player in runner.ActivePlayers)
        {
            var newItem = Instantiate(playerItemPrefab, playerListContainer);
            newItem.text = $"Player ID: {player.PlayerId}";
        }
    }

    private void ClearList()
    {
        List<GameObject> objsToDelete = new();
        foreach (Transform child in playerListContainer)
        {
            objsToDelete.Add(child.gameObject);
        }
        objsToDelete.ForEach(Destroy);
    }
}
