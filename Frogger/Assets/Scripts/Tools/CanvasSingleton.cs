using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = GameObject.FindObjectOfType<T>();
        if (_instance != null && _instance.gameObject.GetInstanceID() == GetInstanceID())
            Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

}
