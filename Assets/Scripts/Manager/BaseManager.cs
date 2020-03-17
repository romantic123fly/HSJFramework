#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             BaseManager
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseManager<T> : Singleton<T> where T : BaseManager<T>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        Debug.Log(gameObject.name + "...InitOK");
    }
    public virtual void Init()
    {
      
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log(name + "...Destroy");
    }
}
