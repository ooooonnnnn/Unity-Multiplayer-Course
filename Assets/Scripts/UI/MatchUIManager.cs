using System;
using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;

public class MatchUIManager : NetworkBehaviour
{
    [SerializeField] private NetworkObjectActiveState matchSummaryActive;
    [SerializeField] private DisplayScores displayScores;
    
    public void ShowMatchOverScreen()
    {
        matchSummaryActive.IsActive = 1;
        
        print("calling show scores");
        displayScores.DisplayScoresTextRPC();
    }
}
