﻿#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             UnityActionAndEvent
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityActionAndEvent : MonoBehaviour
{
    public UnityAction action;
    //不带参数UnityEvent非抽象类 可以直接实例化
    public UnityEvent myEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        action = new UnityAction(MyAction);
        action += MyAction2;

      
    }

    private void MyAction2()
    {
        Debug.Log("MyAction2");
    }

    private void MyAction()
    {
        Debug.Log("MyAction");
    }
    public void ActionEvent() {
        myEvent.AddListener(action);
        myEvent.Invoke();
    }
}
