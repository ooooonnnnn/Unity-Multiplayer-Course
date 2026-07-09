using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using ScriptableObjects;
using UnityEngine;
using Singleton;
using UnityEngine.Events;

public class MatchMakingManager : Singleton<MatchMakingManager>
{
    public Map targetMap;
    [SerializeField] private float strictSearchTime = 5f;
    [SerializeField] private UnityEvent OnStartSearch;
    [SerializeField] private UnityEvent OnEndSearch;
    private Coroutine _searchCoroutine;
    private bool _isStrict = true;

    protected override void Awake()
    {
        base.Awake();
        OnStartSearch.AddListener(ListenForNewSessions);
        OnStartSearch.AddListener(() => TryJoin(SessionJoiner.Instance.GetAvailableSessions().ToList()));
        OnEndSearch.AddListener(StopListeningForSessions);
    }

    private void TryJoin(List<SessionInfo> sessionList)
    {
        IEnumerable<SessionInfo> relevantSessions = sessionList;

        if (_isStrict)
            relevantSessions = sessionList.Where(info =>
                info.Properties.ContainsKey(SessionJoiner.MAP_PROPERTY_NAME) &&
                info.Properties[SessionJoiner.MAP_PROPERTY_NAME].PropertyValue.Equals(targetMap.MapName));

        relevantSessions = relevantSessions.ToArray();
        int numberOfSessions = relevantSessions.Count();

        if (numberOfSessions <= 0)
        {
            print("No sessions found");
            return;
        }
        
        SessionJoiner.Instance.JoinSpecificSession(relevantSessions.ToArray()[Random.Range(0, numberOfSessions)]);
    }

    private void ListenForNewSessions() => 
        SessionJoiner.Instance.OnAvaliableSessionsChanged.AddListener(TryJoin);

    private void StopListeningForSessions() => 
        SessionJoiner.Instance.OnAvaliableSessionsChanged.RemoveListener(TryJoin);

    public void LookForMatch()
    {
        _searchCoroutine = StartCoroutine(StartSearching());
    }

    private IEnumerator StartSearching()
    {
        OnStartSearch.Invoke();
        _isStrict = true;
        yield return new WaitForSeconds(strictSearchTime);
        _isStrict = false;
    }

    public void StopSearching()
    {
        if (_searchCoroutine != null)
            StopCoroutine(_searchCoroutine);
        OnEndSearch.Invoke();
    }
}
