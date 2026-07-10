using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;

public class DisplayScores : NetworkBehaviour
{
    [SerializeField] private TMP_Text scoresText;
    
    [Rpc]
    public void DisplayScoresTextRPC()
    {
        print("displaying scores");
        
        var scores = ScoreManager.Instance.GetScores();
        scoresText.text = "Scores:\n" +
        string.Join("\n", 
            scores.OrderByDescending(s => s.Value).
                Select(s => $"Player {s.Key.PlayerId}: {s.Value}"));
    }
}
