#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             Main
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        InitManager();
        InitController();
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

    private void InitManager()
    {
        Debug.Log("当前设备分辨率：" + Screen.width + " X " + Screen.height);
        //UIManager.instance.setup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MessageDispatcher.Dispatch(MessagesType.ShowMainView);
        }
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
