#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             Singleton
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : GameMonoBehaviour where T : Singleton<T>
{
    public static T _instance;
    public static T GetInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject(typeof(T).Name).AddComponent(typeof(T)) as T;
            }
            return _instance;
        }
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}