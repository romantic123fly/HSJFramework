#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : BaseState {
    public RunState( ) : base()
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("进入跑模式......");
    }
    public override void Excute()
    {
        base.Excute();
        Debug.Log("执行跑模式......");

    }
    public override void Exit()
    {
        base.Exit();
        Debug.Log("退出跑模式......");

    }
}
