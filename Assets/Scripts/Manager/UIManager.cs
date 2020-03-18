#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             UIManager
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public delegate void DelAfterHideUI();
public delegate void DelAfterShowUI();
public class UIType
{
    //是否需要重置窗体反向切换的信息
    public bool isResetReturnUIInfor = false;
    public EUIRootType rootType = EUIRootType.Normal;
    public EShowUIMode showMode = EShowUIMode.DoNoting;
}
public enum EUIRootType
{
    KeepAbove,//中间层
    Normal,//底层
    TopUIRoot//最上层
}
//窗体的打开模式
public enum EShowUIMode
{
    DoNoting,
    HideOther,//需要反向切换（隐藏其他窗体）
    NoReturn//不需要反向切换(隐藏所有窗体包括最前方的窗体)
}
public enum EUiId
{
    NullUI,
    MainView,
    OneView,
    TwoView
}
public class UIReturnInfor
{
    //将要被显示的窗体
    public BaseView _willBeShowUI;
    //存放反向切换窗体id的列表
    public List<EUiId> _listReturn;
}

public class UIManager : BaseManager<UIManager>
{
    //缓存所有窗体
    public Dictionary<EUiId, BaseView> _dicAllUI;

    //缓存已经打开的窗体（正在显示的窗体）
    public Dictionary<EUiId, BaseView> _dicShownUI;

    //缓存具有反向切换信息的UI栈体
    private Stack<UIReturnInfor> _stackReturnInfor;

    //缓存当前窗体的ID
    public BaseView _currentUI = null;

    //缓存UI的根节点
    public Transform _uiRootTrans;
    //保持在最前方窗体的父节点
    private Transform _keepAboveUIRoot;
    //普通窗体的父节点     
    private Transform _normalUIRoot;
    //弹窗
    private Transform _topUIRoot;

    protected override void Awake()
    {
        base.Awake();
        if (_uiRootTrans == null)
        {
            _uiRootTrans = Instantiate(Resources.Load<Transform>("Prefabs/UI/Canvas"));
            DontDestroyOnLoad(_uiRootTrans);
        }
        _dicAllUI = new Dictionary<EUiId, BaseView>();
        _dicShownUI = new Dictionary<EUiId, BaseView>();
        _stackReturnInfor = new Stack<UIReturnInfor>();

        _normalUIRoot = new GameObject("NormalUIRoot").transform;
        GlobalTools.AddChildToParent(_uiRootTrans, _normalUIRoot);
        GlobalTools.SetLayer(_uiRootTrans.gameObject.layer, _normalUIRoot);

        _keepAboveUIRoot = new GameObject("KeepAboveUIRoot").transform;
        GlobalTools.AddChildToParent(_uiRootTrans, _keepAboveUIRoot);
        GlobalTools.SetLayer(_uiRootTrans.gameObject.layer, _keepAboveUIRoot);

        _topUIRoot = new GameObject("TopUIRoot").transform;
        GlobalTools.AddChildToParent(_uiRootTrans, _topUIRoot);
        GlobalTools.SetLayer(_uiRootTrans.gameObject.layer, _topUIRoot);
    }

    public void ShowUI(EUiId uiId)
    {
        BaseView baseUI = JudgeShowUI(uiId);
        if (baseUI!=null)
        {
            _dicShownUI[uiId] = baseUI;
            if (baseUI.uiType.isResetReturnUIInfor)
            {
                //重置反向切换的信息（当显示的是Mainui的时候就需要重置）
                if (_stackReturnInfor != null)
                {
                    _stackReturnInfor.Clear();
                }
            }
            _currentUI = baseUI;
        }
    }
    public BaseView JudgeShowUI(EUiId uiId)
    {
        //先判断窗体是不是正在显示（已经显出）
        if (_dicShownUI.ContainsKey(uiId))//已经显示出来了
        {
            GetViewByUiID(uiId).Render();
            return null;
        }
        else
        {
            //First、FirstOrDefault的区别在于：当没有元素满足条件时，一个抛出异常，一个返回默认值。
            BaseView baseUI = _dicAllUI.FirstOrDefault(t => t.Key == uiId).Value;
            //如果baseui在字典里面没有查找到，说明该窗体还没有加载过
            if (baseUI == null)
            {
                Type type = Type.GetType(uiId.ToString());
                BaseView theUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/" + uiId.ToString())).AddComponent(type) as BaseView;
                if (theUI != null)
                {
                    Transform theRoot = GetUIRoot(theUI);
                    GlobalTools.AddChildToParent(theRoot, theUI.transform);
                    _dicAllUI[uiId] = theUI;
                    baseUI = theUI;
                }
                else
                {
                    Debug.LogError("Load UI Null");
                }
            }
            UpDateStack(baseUI);
            return baseUI;
        }
    }
    //把窗体添加进栈stackReturnInfor
    public void UpDateStack(BaseView baseUI)
    {
        if (baseUI.isNeedUpdateStack)
        {
            //将要移除的窗体的id列表
            List<EUiId> removeKey = new List<EUiId>(); ;
            //存放需要添加进栈的窗体列表
            List<BaseView> maxToMinUi = new List<BaseView>();
            //存放depth值从大到小排序后的窗体id
            List<EUiId> newList = new List<EUiId>();
            //新建栈体信息
            UIReturnInfor uiReturnInfor = new UIReturnInfor();
            if (_dicShownUI.Count > 0)
            {
                foreach (KeyValuePair<EUiId, BaseView> item in _dicShownUI)
                {
                    if (item.Value.uiType.rootType != EUIRootType.KeepAbove)
                    {
                        item.Value.gameObject.SetActive(false);
                        removeKey.Add(item.Key);
                    }
                    if (item.Value.isNeedUpdateStack)
                    {
                        maxToMinUi.Add(item.Value);
                    }
                }
                if (removeKey != null)
                {
                    for (int i = 0; i < removeKey.Count; i++)
                    {
                        _dicShownUI.Remove(removeKey[i]);
                    }
                }
                //把进栈的窗体按照depth值从大到小排序
                if (maxToMinUi.Count > 0)
                {
                    maxToMinUi.Sort(delegate (BaseView a, BaseView b) { return a._currentDepth.CompareTo(b._currentDepth); });
                    for (int i = 0; i < maxToMinUi.Count; i++)
                    {
                        newList.Add(maxToMinUi[i]._uiId);
                    }

                    uiReturnInfor._willBeShowUI = baseUI;
                    uiReturnInfor._listReturn = newList;
                    _stackReturnInfor.Push(uiReturnInfor);
                }
            }
        }
        else
        {
            if (baseUI.uiType.showMode == EShowUIMode.NoReturn)
            {
                HideAllUI(true);
            }
        }
        CheckStack(baseUI);
    }
    //获取当前窗体的根节点
    public Transform GetUIRoot(BaseView baseUI)
    {
        if (baseUI.uiType.rootType == EUIRootType.Normal)
        {
            return _normalUIRoot;
        }
        if (baseUI.uiType.rootType == EUIRootType.KeepAbove)
        {
            return _keepAboveUIRoot;
        }
        if (baseUI.uiType.rootType == EUIRootType.TopUIRoot)
        {
            return _topUIRoot;
        }
        else
        {
            return _uiRootTrans;
        }
    }
    //获取指定已显示的窗体
    public BaseView GetViewByUiID(EUiId eUiId)
    {
        BaseView tempView = null;
        foreach (var item in _dicShownUI)
        {
            if (item.Key == eUiId)
            {
                tempView = _dicShownUI[eUiId];
                break;
            }
        }
        return tempView;
    }
    //隐藏所有窗体
    public void HideAllUI(bool isHideAboveUI)
    {
        List<EUiId> listRemove = null;
        if (isHideAboveUI)//需要隐藏最前端ui
        {
            foreach (KeyValuePair<EUiId, BaseView> uiItem in _dicShownUI)
            {
                uiItem.Value.gameObject.SetActive(false);
            }
            _dicShownUI.Clear();
        }
        else
        {
            foreach (KeyValuePair<EUiId, BaseView> uiItem in _dicShownUI)
            {
                if (uiItem.Value.uiType.rootType == EUIRootType.KeepAbove)
                {
                    continue;
                }
                else
                {
                    if (listRemove == null)
                    {
                        listRemove = new List<EUiId>();
                        listRemove.Add(uiItem.Key);
                        uiItem.Value.gameObject.SetActive(false);
                    }
                }
            }
        }
        //把隐藏的窗体从dicShowUI中移除
        if (listRemove != null)
        {
            for (int i = 0; i < listRemove.Count; i++)
            {
                _dicShownUI.Remove(listRemove[i]);
            }
        }
    }
    //检测栈的顺序是否被打乱。如果被打乱了就清空栈
    public void CheckStack(BaseView baseUi)
    {
        if (baseUi.isNeedUpdateStack)
        {
            if (_stackReturnInfor.Count > 0)
            {
                //peek是取出栈顶的栈顶元素
                UIReturnInfor uiReturnInfor = _stackReturnInfor.Peek();
                if (uiReturnInfor._willBeShowUI != baseUi)//说明栈被打乱
                {
                    _stackReturnInfor.Clear();
                }
            }
        }
    }
    //点击返回按钮
    public void ClickReturn()
    {
        //说明没有反向切换的信息
        if (_stackReturnInfor.Count == 0)
        {
            if (_currentUI._beforeUiId != EUiId.NullUI)
            {
                HideTheUI(_currentUI._uiId, delegate { ShowUI(_currentUI._beforeUiId); });
            }
        }
        //说明有反向切换信息
        else
        {
            UIReturnInfor uiReturnInfor = _stackReturnInfor.Peek();
            if (uiReturnInfor != null)
            {
                //获得当前窗体的ui
                EUiId theId = uiReturnInfor._willBeShowUI._uiId;
                if (_dicShownUI.ContainsKey(theId))
                {
                    HideTheUI(theId, delegate {
                        //如果是第一个窗体（depth值最大的窗体），并且没有显示出来
                        if (!_dicShownUI.ContainsKey(uiReturnInfor._listReturn[0]))
                        {
                            BaseView baseUI = _dicAllUI.FirstOrDefault(t=>t.Key == uiReturnInfor._listReturn[0]).Value;
                            _dicShownUI[baseUI._uiId] = baseUI;
                            baseUI.gameObject.SetActive(true);
                            _currentUI = baseUI;
                            //pop是把栈顶元素删除
                            _stackReturnInfor.Pop();
                        }
                    });
                }
            }
        }
    }
    //隐藏该窗体
    public void HideTheUI(EUiId uiId, DelAfterHideUI del)
    {
        del?.Invoke();
        if (_dicShownUI.ContainsKey(uiId))
        {
            Destroy(_dicShownUI[uiId].gameObject);
            _dicAllUI.Remove(uiId);
            _dicShownUI.Remove(uiId);
        }
    }
}
