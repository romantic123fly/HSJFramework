#region 模块信息
// **********************************************************************
// Copyright (C) 2019 
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

public enum EUIRootType
{
    KeepAbove,//中间层
    Normal,//底层
    TopUIRoot//最上层
}

public enum EUiId
{
    NullUI,
    MainView,
    OneView,
    TwoView,
    LoadingView,
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
    public Stack<UIReturnInfor> _stackReturnInfor;

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
            //_uiRootTrans = Instantiate(Resources.Load<Transform>("Prefabs/UI/Canvas"));
            _uiRootTrans = GameObject.Find("Canvas").transform;
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
    //获取当前窗体的根节点
    public Transform GetUIRoot(BaseView baseUI)
    {
        if (baseUI.rootType == EUIRootType.Normal) return _normalUIRoot;
        if (baseUI.rootType == EUIRootType.KeepAbove) return _keepAboveUIRoot;
        if (baseUI.rootType == EUIRootType.TopUIRoot) return _topUIRoot;
        else return _uiRootTrans;
    }
    public BaseView ShowUI(EUiId uiId)
    {
        //先判断窗体是不是正在显示（已经显出）
        if (_dicShownUI.ContainsKey(uiId))
        {
            return null;
        }
        BaseView baseUI = JudgeShowUI(uiId);
        if (baseUI != null)
        {
            _dicShownUI[uiId] = baseUI;
            _dicShownUI[uiId].gameObject.SetActive(true);
            if (uiId == EUiId.MainView)
            {
                //重置反向切换的信息（当显示的是Mainui的时候就需要重置）
                _stackReturnInfor.Clear();
            }
        }
        return _currentUI = baseUI;
    }
    public BaseView JudgeShowUI(EUiId uiId)
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
        }        UpDateStack(baseUI);
        return baseUI;
    }
    
    //把窗体添加进栈stackReturnInfor
    public void UpDateStack(BaseView baseUI)
    {
        if (baseUI.isReturnInfo)
        {
            baseUI.currentDepth = _currentUI.currentDepth + 1;
            baseUI.beforeUiId = _currentUI.uiId;
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
                    if (!item.Value.isReturnInfo)
                    {
                        removeKey.Add(item.Key);
                    }
                    else
                    {
                        maxToMinUi.Add(item.Value);
                    }
                }
                if (removeKey != null)
                {
                    for (int i = 0; i < removeKey.Count; i++)
                    {
                        HideTheUI(removeKey[i]);
                    }
                }
                //把进栈的窗体按照depth值从大到小排序
                if (maxToMinUi.Count > 0)
                {
                    maxToMinUi.Sort(delegate (BaseView a, BaseView b) { return a.currentDepth.CompareTo(b.currentDepth); });
                    for (int i = 0; i < maxToMinUi.Count; i++)
                    {
                        newList.Add(maxToMinUi[i].uiId);
                    }

                    uiReturnInfor._willBeShowUI = baseUI;
                    uiReturnInfor._listReturn = newList;
                    _stackReturnInfor.Push(uiReturnInfor);
                }
            }
        }
        CheckStack(baseUI);
    }
    //检测栈的顺序是否被打乱。如果被打乱了就清空栈
    public void CheckStack(BaseView baseUi)
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
    //点击返回按钮
    public void ClickReturn()
    {
        //说明没有反向切换的信息
        if (_stackReturnInfor.Count == 0)
        {
            if (_currentUI.beforeUiId != EUiId.NullUI)
            {
                HideTheUI(_currentUI.uiId, delegate { ShowUI(_currentUI.beforeUiId); });
            }
        }
        //说明有反向切换信息
        else
        {
            UIReturnInfor uiReturnInfor = _stackReturnInfor.Peek();
            if (uiReturnInfor != null)
            {
                //获得当前窗体的ui
                EUiId theId = uiReturnInfor._willBeShowUI.uiId;
                if (_dicShownUI.ContainsKey(theId))
                {
                    HideTheUI(theId, delegate
                    {
                        BaseView baseUI = _dicAllUI.FirstOrDefault(t => t.Key == uiReturnInfor._listReturn[0]).Value;
                        _dicShownUI[baseUI.uiId] = baseUI;
                        baseUI.gameObject.SetActive(true);
                        _currentUI = baseUI;
                        //pop是把栈顶元素删除
                        _stackReturnInfor.Pop();

                    });
                }
            }
        }
    }
    //隐藏该窗体
    public void HideTheUI(EUiId uiId, DelAfterHideUI del = null)
    {
        del?.Invoke();
        if (_dicShownUI.ContainsKey(uiId))
        {
            if (_dicShownUI[uiId].isSingleUse)
            {
                Destroy(_dicShownUI[uiId].gameObject);
                _dicAllUI.Remove(uiId);
                _dicShownUI.Remove(uiId);
            }
            else
            {
                _dicShownUI[uiId].gameObject.SetActive(false);
                _dicShownUI.Remove(uiId);
            }
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
        List<EUiId> removeList = new List<EUiId>();
        if (isHideAboveUI)//需要隐藏最前端ui
        {
            foreach (KeyValuePair<EUiId, BaseView> uiItem in _dicShownUI)
            {
                uiItem.Value.gameObject.SetActive(false);
                removeList.Add(uiItem.Key);
            }
        }
        else
        {
            foreach (KeyValuePair<EUiId, BaseView> uiItem in _dicShownUI)
            {
                if (uiItem.Value.rootType != EUIRootType.KeepAbove)
                {
                    removeList.Add(uiItem.Key);
                    uiItem.Value.gameObject.SetActive(false);
                }
            }
        }
        //把隐藏的窗体从dicShowUI中移除
        if (removeList != null)
        {
            for (int i = 0; i < removeList.Count; i++)
            {
                _dicShownUI.Remove(removeList[i]);
            }
        }
    }
}

