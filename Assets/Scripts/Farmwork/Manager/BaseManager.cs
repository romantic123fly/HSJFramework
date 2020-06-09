#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using UnityEngine;

public abstract class BaseManager<T> : Singleton<T>, IMessageHandler where T : BaseManager<T>
{
    protected override void Awake()
    {
        base.Awake();
        MessageDispatcher.Attach(this);
    }

    public virtual void setup()
    {
    }

    protected override void OnDestroy()
    {
        MessageDispatcher.Detach(this);

        base.OnDestroy();
    }

    public void HandleMessage(IMessages messages)
    {
        throw new System.NotImplementedException();
    }
}
