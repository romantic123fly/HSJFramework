#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             MessageDispatcher
// Author:                romantic
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDispatcher{
    private static  List<IMessageHandler> messageHandlers = new List<IMessageHandler>();

    /// <summary>
    /// 注册消息
    /// </summary>
    /// <param name="messageHandler"></param>
    public static void Attach(IMessageHandler messageHandler)
    {
        if (messageHandlers.IndexOf(messageHandler) == -1)
        {
            messageHandlers.Add(messageHandler);
        }
    }
    /// <summary>
    /// 消息事件分发
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
  public static void Dispatch(string type,object data = null)
    {
        Messages msg = new Messages(type, data);
        for (int i = 0; i < messageHandlers.Count; i++)
        {
            messageHandlers[i].HandleMessage(msg);
        }
    }
    /// <summary>
    /// 移除注册的消息
    /// </summary>
    /// <param name="handler"></param>
    public static void Detach(IMessageHandler handler)
    {
        if (messageHandlers != null)
        {
            messageHandlers.Remove(handler);
        }
    }
}
