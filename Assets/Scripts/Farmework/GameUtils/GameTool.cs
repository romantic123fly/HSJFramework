#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             GameTool
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTool:GameMonoBehaviour
{
    //查找物体的方法
    public static Transform FindTheChild(GameObject goParent, string childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }
    //获取子物体的脚本
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            return searchTrans.gameObject.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    //给子物体添加脚本
    public static T AddTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            T[] theComponentsArr = searchTrans.GetComponents<T>();
            for (int i = 0; i < theComponentsArr.Length; i++)
            {
                if (theComponentsArr[i] != null)
                {
                    Destroy(theComponentsArr[i]);
                }
            }
            return searchTrans.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
    }
    //添加子物体
    public static void AddChildToParent(Transform parentTrs, Transform childTrs)
    {
        childTrs.SetParent(parentTrs);
        childTrs.localPosition = Vector3.zero;
        childTrs.localScale = Vector3.one;
        childTrs.localEulerAngles = Vector3.zero;
        SetLayer(parentTrs.gameObject.layer, childTrs);
    }
    //设置所有子物体的Layer
    public static void SetLayer(int parentLayer, Transform childTrs)
    {
        childTrs.gameObject.layer = parentLayer;
        for (int i = 0; i < childTrs.childCount; i++)
        {
            Transform child = childTrs.GetChild(i);
            child.gameObject.layer = parentLayer;
            SetLayer(parentLayer, child);
        }
    }
}
