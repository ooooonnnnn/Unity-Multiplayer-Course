using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using ScriptableObjects;
using UnityEngine;
using Singleton;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class MatchMakingManager : Singleton<MatchMakingManager>
{
    [SerializeField] private string targetMapName;
    [SerializeField] private MapList mapList;
    [SerializeField] private float strictSearchTime = 5f;
    [SerializeField] private UnityEvent OnStartSearch;
    [SerializeField] private UnityEvent OnEndSearch;
    [SerializeField] private UnityEvent<float> OnSearchTime;
    private Coroutine _searchCoroutine;
    private bool _isStrict = true;

    private void OnValidate()
    {
        targetMapName = mapList.GetMapNames().First();
    }

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
                info.Properties[SessionJoiner.MAP_PROPERTY_NAME].PropertyValue.Equals(targetMapName));

        relevantSessions = relevantSessions.ToArray();
        int numberOfSessions = relevantSessions.Count();

        if (numberOfSessions <= 0)
        {
            print("No sessions found");
            return;
        }
        
        StopSearching();
        
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
        
        float startTime = Time.time;
        while (Time.time - startTime < strictSearchTime)
        {
            yield return null;
            OnSearchTime.Invoke(Time.time - startTime);
        }
        
        _isStrict = false;
        
        while (true)
        {
            yield return null;
            OnSearchTime.Invoke(Time.time - startTime);
        }
    }

    public void StopSearching()
    {
        if (_searchCoroutine != null)
            StopCoroutine(_searchCoroutine);
        OnEndSearch.Invoke();
    }
    
    public void SetMapNameFromIndex(int index) => targetMapName = mapList.GetMapNames().ToArray()[index];
}
