#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             MVCMain
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVCMain : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        PlatformDefines();
        InitManager();
        InitController();
        MessageDispatcher.Dispatch(MessagesType.ShowMainView);
    }
    private void InitManager()
    {
        Debug.Log("当前设备分辨率：" + Screen.width + " X " + Screen.height);
        UIManager.Instance.setup();
    }
    private void InitController()
    {
        GameObject controller = new GameObject("Controller");
        controller.AddComponent<MainViewController>();
        controller.AddComponent<OneViewController>();
        controller.AddComponent<TwoViewController>();
        DontDestroyOnLoad(controller);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MessageDispatcher.Dispatch(MessagesType.ShowOneView);
        }
    }

    void PlatformDefines()
    {
#if UNITY_EDITOR
        Debug.Log("UNITY_EDITOR");
#endif
#if UNITY_EDITOR_WIN
        Debug.Log("UNITY_EDITOR_WIN");
#endif
#if UNITY_EDITOR_OSX
            Debug.Log("UNITY_EDITOR_OSX");
#endif

#if UNITY_IOS
            Debug.Log("UNITY_IPHONE");
#endif

#if UNITY_ANDROID
            Debug.Log("UNITY_ANDROID");
#endif

#if UNITY_STANDALONE_OSX
        Debug.Log("UNITY_STANDALONE_OSX");
#endif

#if UNITY_STANDALONE_WIN
        Debug.Log("UNITY_STANDALONE_WIN");
#endif
    }
}

