using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[Tooltip("A singleton that will be destroyed if an older one with the same name exists")]
public class PersistentSingleton<T> : MonoBehaviour where T : PersistentSingleton<T>
{
    public static T Instance { get; protected set; }

    protected virtual void Awake()
    {
        if (!Instance)
        {
            Instance = (T)this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        
        if (Instance != this) Destroy(gameObject);
    }
}