using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = GameObject.FindObjectOfType<T>();
        //_instance = GetComponent<T>();
        if (_instance != null && _instance.gameObject.GetInstanceID() == GetInstanceID())
            Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }
}
