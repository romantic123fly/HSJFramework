#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             Messages
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messages: IMessages{
    public string Type { get; private set; }
    public object Data { get; private set; }

    public Messages()
    {

    }
    public Messages(string type,object data) {
        this.Type = type;
        this.Data = data;
    }
}
