using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CanvasBase : MonoBehaviour
{
    protected virtual void Awake() { }
    protected virtual void OnDestroy() { }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}


public class CanvasSingleton<T>: CanvasBase where T : CanvasSingleton<T> 
{
    private static T _instance;
    public static T Instance
    {
        get 
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<T>();

            return _instance;
        }
    }

    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }

    protected override void OnDestroy()
    {
        _instance = null;
        base.OnDestroy();
    }

}


