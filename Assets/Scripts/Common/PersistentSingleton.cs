using UnityEngine;

public abstract class PersistentSingleton<T> : MonoSingleton<T> where T : MonoBehaviour
{
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            Initialize();
        }
        else if (_instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    protected abstract void Initialize();
}