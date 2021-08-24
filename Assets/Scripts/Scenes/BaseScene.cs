using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public SceneType.Scene sceneType { get; protected set; } = SceneType.Scene.Default;
    public EventSystem eventSystem;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = FindObjectOfType(typeof(EventSystem));
        if(obj == null)
        {
            obj = Instantiate(eventSystem);
        }
    }

    public abstract void Clear(); 
}
