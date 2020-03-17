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
    public UIType uiType = new UIType();
    public EUIRootType rootType = EUIRootType.Normal;
    public EShowUIMode showMode = EShowUIMode.NoReturn;
    public EUiId _uiId = EUiId.NullUI;
    public EUiId _beforeUiId = EUiId.NullUI;
    public int _currentDepth = 0;
    public bool _isSingleUse = false;
    public bool _isNeedUpdateStack;

    //是否需要添加到反向切换的信息里面，如果能就需要更新stackReturnInfor
    public bool isNeedUpdateStack
    {
        get
        {
            if (this.uiType.rootType == EUIRootType.KeepAbove)
            {
                return false;
            }
            if (this.uiType.showMode == EShowUIMode.NoReturn)
            {
                return false;
            }
            return true;
        }
    }
    protected override void Awake()
    {
        base.Awake();
        InitUIData();
        InitUIOnAwake();
        InitEvent();
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
