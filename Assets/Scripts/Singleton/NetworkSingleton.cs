using Fusion;

namespace Singleton
{
    public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
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
