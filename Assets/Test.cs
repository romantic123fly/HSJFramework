#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             Test
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Dictionary<string, string> a = new Dictionary<string, string>();
    // Start is called before the first frame update
    void Start()
    {
        a.Add("ID","ppp");
        a.Add("PP","aa");
        Debug.Log(a["ID"]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
