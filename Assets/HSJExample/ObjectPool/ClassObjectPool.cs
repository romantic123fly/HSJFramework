#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             ClassObjectPool
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassObjectPool<T> where T : class, new()
{
    //池子
    public Stack<T> m_Pool = new Stack<T>();
    //最大对象数量 0表示无限
    protected int m_MaxCount = 0;
    //未回收的对象数
    protected int m_NoRecycleCount = 0;

    public ClassObjectPool(int maxCount)
    {
        m_MaxCount = maxCount;
        for (int i = 0; i < m_MaxCount; i++)
        {
            m_Pool.Push(new T());
        }
    }
    //从池子里面取类对象
    public T Spawn(bool isCreat)
    {
        if (m_Pool.Count > 0)
        {
            T t = m_Pool.Pop();
            if (t == null)
            {
                if (isCreat)
                {
                    t = new T();
                    m_NoRecycleCount++;
                }
                else
                {
                    Debug.LogError(nameof(t) + "：不在池子里面且不允许新建");
                }
            }
            return t;
        }
        else
        {
            if (isCreat)
            {
                T t = new T();
                m_NoRecycleCount++;
                return t;
            }
            else
            {
                Debug.LogError("池子为空，且不允许新建");
            }
        }
        return null;
    }
    //回收对象到池子里
    public bool Recycle(T t)
    {
        if (t == null)
            return false;
        m_NoRecycleCount--;
        if (m_Pool.Count>=m_MaxCount && m_MaxCount>0)
        {
            t = null;
            return false;
        }
        m_Pool.Push(t);
        return true;
    }

}
