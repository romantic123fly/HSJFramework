#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             UnityEventTest
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]//事件可以拖拽
public class HsjUnityEvent : UnityEvent<string> { }
//带参数UnityEvent是抽象类，需要自己声明一个类
public class UnityEventTest : MonoBehaviour
{
    public HsjUnityEvent unityEvent;
    // Start is called before the first frame update
    void Start()
    {
        if (unityEvent==null)
        {
            unityEvent = new  HsjUnityEvent();
        }
        unityEvent.Invoke("unityEvent");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
