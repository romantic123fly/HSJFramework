#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             BaseView
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseView : GameMonoBehaviour
{
    public EUiId uiId = EUiId.NullUI;
    public EUIRootType rootType = EUIRootType.Normal;
    public EUiId beforeUiId = EUiId.NullUI;
    public int currentDepth;
    public bool isSingleUse;
    public bool isReturnInfo;

    protected override void Awake()
    {
        base.Awake();
        InitUIData();
        InitUIOnAwake();
        InitEvent();
    }

    protected override void Update()
    {
        base.Update();
        render();
    }
    public  virtual void InitUIData()
    {

    }
    public  virtual void InitUIOnAwake()
    {

    }
    public virtual void InitEvent()
    {

    }
    public  virtual void Render()
    {

    }
}
