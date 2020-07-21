#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             TestUGUIEvent
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUGUIEvent : MonoBehaviour
{
    public  GameObject image;

    // Use this for initialization
    private void Awake()
    {
        UGUIEventListener.Get(image).onLongPress = OnLongPress;
        UGUIEventListener.Get(image).onClick = OnClick;
        UGUIEventListener.Get(image).onDown = OnDown;
        UGUIEventListener.Get(image).onUp = OnUp;
        UGUIEventListener.Get(image).onEnter = OnEnter;
        UGUIEventListener.Get(image).onExit = OnExit;
    }

    private void OnExit(GameObject go)
    {
        Debug.Log("OnExit");
    }

    private void OnEnter(GameObject go)
    {
        Debug.Log("OnEnter");
    }

    private void OnUp(GameObject go)
    {
        Debug.Log("OnUp");
    }

    private void OnDown(GameObject go)
    {
        Debug.Log("OnDown");
    }

    private void OnClick(GameObject go)
    {
        Debug.Log("OnClick");
    }

    public void OnLongPress(GameObject go)
    {
        Debug.Log("OnLongPress");
    }
}
