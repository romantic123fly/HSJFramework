#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             EventUser
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSharpEventUser : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        CSharpEventTest.eventHandler += (a) => { Debug.Log(a+ "CSharpEventUser"); };
    }

  
}
