#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             MessageHandler
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler : GameMonoBehaviour, IMessageHandler
{
    protected override void Awake()
    {
        base.Awake();
        MessageDispatcher.Attach(this);
    }

    public virtual void HandleMessage(IMessages messages)
    {

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        MessageDispatcher.Detach(this);
    }
}
