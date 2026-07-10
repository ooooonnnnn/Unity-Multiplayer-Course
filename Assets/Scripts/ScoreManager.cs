using System.Collections.Generic;
using Fusion;
using Singleton;
using UnityEngine;

public class ScoreManager : NetworkSingleton<ScoreManager>
{
    [Networked, Capacity(20)] 
    private NetworkDictionary<PlayerRef, int> Scores { get; } = 
        MakeInitializer<PlayerRef, int>(new Dictionary<PlayerRef, int>());
    [SerializeField] private int scoreForHit = 1;

    public void AddScoreForHit_Client()
    {
        var runner = SinglePeer_NetworkRunnerManager.Instance.NetworkRunner;
        if (!Scores.ContainsKey(runner.LocalPlayer))
            Scores.Add(runner.LocalPlayer, scoreForHit);
        else
        {
            var prevScore = Scores.Get(runner.LocalPlayer);
            Scores.Set(runner.LocalPlayer, prevScore + scoreForHit);
        }
    }
    
    public Dictionary<PlayerRef, int> GetScores() => new(Scores);
    
    [ContextMenu("Print Scores")]
    private void PrintScores()
    {
        foreach (var (player, score) in Scores)
            print($"{player.PlayerId}: {score}");
    }
}
