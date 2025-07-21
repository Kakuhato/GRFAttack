using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RegulatorSingleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public float InitialTime { get; private set;}

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    // 此时 Awake() 还没执行，直到 GameObject 被创建出来后才执行 Awake()
                    var obj = new GameObject(typeof(T).Name + " Singleton");
                    obj.hideFlags = HideFlags.DontSave;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        InitialSingleton();
    }
    
    protected virtual void InitialSingleton()
    {
        // if(!Application.isPlaying) return;
        
        InitialTime = Time.time;
        DontDestroyOnLoad(this.gameObject);

        T[] oldInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
        foreach (T old in oldInstances)
        {
            if(old.GetComponent<RegulatorSingleton<T>>().InitialTime < this.InitialTime)
            {
                Destroy(old.gameObject);
                // TODO: 单独写一个销毁函数，释放上一个实例中的内容
            }
        }

        if (instance == null)
        {
            instance = this as T;
        }
    }
}

