using System;
using UnityEngine;

namespace Singleton
{
    /// <summary>
    /// Non-persistent singleton (without don't destroy on load)
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (!Instance)
            {
                Instance = (T)this;
                OnSetInstance();
                return;
            }
        
            if (Instance != this) Destroy(gameObject);
        }

        protected virtual void OnSetInstance(){}
    }
}
