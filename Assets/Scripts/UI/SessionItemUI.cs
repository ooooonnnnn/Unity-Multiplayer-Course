using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SessionItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text sessionName;
    [SerializeField] private TMP_Text playerCount;
    [SerializeField] private Button joinButton;
    
    public void SetSessionName(string name)
    {
        sessionName.text = name;
    }

    public void SetPlayerCount(int current, int max)
    {
        playerCount.text = $"{current}/{max} players";
    }

    public void SetCanJoin(bool canJoin)
    {
        joinButton.interactable = canJoin;
    }

    public void SetCallback(UnityAction callback)
    {
        joinButton.onClick.AddListener(callback);
    }
}
