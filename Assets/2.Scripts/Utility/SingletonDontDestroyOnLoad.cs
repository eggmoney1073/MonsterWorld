using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroyOnLoad<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _uniqueInstance;

    public static T _Instance
    {
        get
        {
            return _uniqueInstance;
        }
    }

    protected virtual void Awake()
    {
        if (_uniqueInstance != null)
        {
            if (_uniqueInstance != this)
            {
                Destroy(gameObject);
            }
            return;
        }

        _uniqueInstance = GetComponent<T>();
        DontDestroyOnLoad(gameObject);
    }
}
