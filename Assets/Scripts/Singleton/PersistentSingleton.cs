using Singleton;
using UnityEngine;

[Tooltip("A singleton that will be destroyed if an older one with the same name exists")]
public abstract class PersistentSingleton<T> : Singleton<T> where T : PersistentSingleton<T>
{
    protected override void OnSetInstance() => DontDestroyOnLoad(gameObject);

    protected override void Awake()
    {
        //This allows DontDestroyOnLoad 
        transform.SetParent(null);
        
        base.Awake();
    }
}