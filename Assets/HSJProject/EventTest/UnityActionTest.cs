#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             UnityAction
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityActionTest : MonoBehaviour
{
    //不带参数UnityEvent不是抽象类，可以直接实例化
    public UnityAction myAction;
    // Start is called before the first frame update
    void Start()
    {
        myAction += MyFunction;
        myAction();
    }

    private void MyFunction()
    {
        Debug.Log("测试UnityAction无参");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
