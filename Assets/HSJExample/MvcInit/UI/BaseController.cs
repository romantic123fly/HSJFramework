#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             BaseController
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MessageHandler
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {

    }

    protected virtual void InitEvent()
    {

    }

    public override void HandleMessage(IMessages messages)
    {

    }
}
