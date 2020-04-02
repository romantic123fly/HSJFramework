#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             UnityEventUser
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityEventUser : MonoBehaviour
{
    private UnityEventTest  unityEventTest;
    // Start is called before the first frame update
    void Start()
    {
        unityEventTest = gameObject.GetComponent<UnityEventTest>();
        unityEventTest.unityEvent.AddListener((str)=> { Debug.Log(str+"1"); });
        unityEventTest.unityEvent.AddListener((str)=> { Debug.Log(str+"2"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
