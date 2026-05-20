using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SessionListUI : MonoBehaviour
{
    public void ShowSessionList(List<SessionInfo> sessionList)
    {
        foreach (var session in sessionList)
            print(session.Name);
    }
}
