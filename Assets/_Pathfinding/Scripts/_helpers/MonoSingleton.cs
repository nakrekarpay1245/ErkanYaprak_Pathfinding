using UnityEngine;

namespace _Pathfinding._helpers
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T singleton;

        protected virtual void OnEnable()
        {
            if (!singleton)
            {
                singleton = (T)this;
            }
            else
            {
                Destroy(singleton);
            }
        }

        protected virtual void OnDestroy()
        {
            if (singleton == this)
            {
                singleton = null;
            }
        }
    }
}