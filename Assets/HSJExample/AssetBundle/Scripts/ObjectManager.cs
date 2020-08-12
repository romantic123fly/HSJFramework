#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             ObjectManager
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : BaseManager<ObjectManager>
{
    protected Dictionary<Type, object> m_ClassPoolDic = new Dictionary<Type, object>();
    /// <summary>
    /// 创建类的对象池，之后在外面可以保存ClassObjectPool<T>,然后可以调用Spaw和Recycle方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="maxCount"></param>
    /// <returns></returns>
    public ClassObjectPool<T> GetOrCreatClassPool<T>(int maxCount) where T:class ,new()
    {
        Type type = typeof(T);
        object outObj = null;
        if (!m_ClassPoolDic.TryGetValue(type,out outObj)||outObj ==null)
        {
            ClassObjectPool<T> newPool = new ClassObjectPool<T>(maxCount);
            m_ClassPoolDic.Add(type,newPool);
            return newPool;
        }
        return outObj as ClassObjectPool<T>;
    }

    public T NewClassObjectFromPool<T>(int maxCount) where T:class,new()
    {
        ClassObjectPool<T> pool = GetOrCreatClassPool<T>(maxCount);
        if (pool ==null)
        {
            return null;
        }
        return pool.Spawn(true);
    }

}
