using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }

    // This awake method will only be visible to classes that inherit from it
    protected virtual void Awake()
    {
        if(instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = (T)this;
        }

        DontDestroyOnLoad(gameObject);
    }
}
