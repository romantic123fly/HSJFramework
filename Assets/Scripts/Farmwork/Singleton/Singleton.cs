#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using UnityEngine;
public abstract class Singleton<T> : GameMonoBehaviour where T : Singleton<T>
{
    //泛型单例约束
    private static GameObject _container;

    protected static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_container == null)
            {
                _container = new GameObject();
            }

            if (_instance == null)
            {
                string name = typeof(T).Name;
                if (name != null)
                {
                    _instance = _container.AddComponent(typeof(T)) as T;
                    _instance.name = name;
                }
                else
                {
                    Debug.LogWarning("Singleton Type ERROR! (" + name + ")");
                }
            }
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected override void OnDestroy()
    {
        Debug.Log(name + " onDestroy");
    }
}