using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static bool IsInputEnable = true;

    static Managers s_Instance;
    public static Managers Instance { get { Init(); return s_Instance; } }

    SoundManager _sound = new SoundManager();
    SceneManagerEx _scene = new SceneManagerEx();
    DataManager _data = new DataManager();
    PoolManager _pool = new PoolManager();

    public static SoundManager Sound { get { return Instance._sound; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static DataManager Data { get { return Instance._data; } }
    public static PoolManager Pool { get { return Instance._pool; } }

    void Start()
    {
        Init();
        Application.targetFrameRate = 60;
    }

    void Update()
    {
    
    }

    static void Init()
    {
        if(s_Instance == null)
        {
            GameObject obj = GameObject.Find("Managers");
            if(obj == null)
            {
                obj = new GameObject { name = "Managers" };
                obj.AddComponent<Managers>();
            }

            DontDestroyOnLoad(obj);
            s_Instance = obj.GetComponent<Managers>();

            s_Instance._sound.Init();
            s_Instance._data.Init();
            s_Instance._pool.Init();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        Pool.Clear();
    }
}
