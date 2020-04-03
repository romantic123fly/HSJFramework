#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             ClearOops
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class ClearOops : MonoBehaviour
{
    private Thread CloseOops;         //新建一个子线程
    private bool ThreadRunning = true;//标识子线程中的循环是否继续进行

    // Start is called before the first frame update
    void Start()
    {
        //初始化子线程：
        CloseOops = new Thread(ClearOopsWindows)
        {
            IsBackground = true//设为当前进程的后台进程，使得在退出程序时该子线程一并结束
        };

        CloseOops.Start();//开始运行线程
    }

    // Update is called once per frame
    void Update()
    {

    }

    //线程运行时执行的方法：
    void ClearOopsWindows()
    {
        //当需要线程运行时：
        while (ThreadRunning)
        {
            Clear.FindAndCloseWindow();
        }
    }

    //退出程序时结束子线程：
    void OnApplicationQuit()
    {
        ThreadRunning = false;
    }


    //调用win32API中的方法关闭Oops弹出框
    class Clear
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        public static void FindAndCloseWindow()
        {
            IntPtr lHwnd = FindWindow(null, "Oops");
            if (lHwnd != IntPtr.Zero)
            {
                SendMessage(lHwnd, WM_SYSCOMMAND, SC_CLOSE, 0);
            }
        }
    }
}
