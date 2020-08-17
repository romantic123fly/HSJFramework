#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             PoolTest
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    GameObject go;
    private void Start()
    {
         go = Resources.Load("Prefabs/Entity/Cube") as GameObject;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //测试对象池机制   
            go = PoolManager.Instance.Spawn(go);
            StartCoroutine(Recovery(go));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //测试类对象池机制   
            go = PoolManager.Instance.Spawn(go);
            StartCoroutine(Recovery(go));
        }
    }
    IEnumerator Recovery(GameObject go)
    {
        yield return new WaitForSeconds(5);
        PoolManager.Instance.Despawn(go);
    }
}
