using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    protected bool _isDestroyed;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogError($"No instance of {typeof(T)} found in the scene.");
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            _isDestroyed = true;
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}
