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

public class PoolManager : BaseManager<PoolManager>
{
    // 存储动可重用的GameObject。
    public  List<GameObject> _objectList = new List<GameObject>();
    // 对象池大小
    private int _capacity = 100;

    // 在dormantObjects获取与go类型相同的GameObject,如果没有则new一个。
    public GameObject Spawn(GameObject go)
    {
        GameObject temp = null;
        if (_objectList.Count > 0)
        {
            foreach (var item in _objectList)
            {
                if (item.name == go.name)
                {
                    // Find an available GameObject
                    temp = item;
                    _objectList.Remove(item);
                    //temp.transform.SetParent(null);
                    temp.gameObject.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            temp = Instantiate(go) as GameObject;
            temp.name = go.name;
        }
        return temp;
    }

    // 将用完的GameObject放入dormantObjects中
    public void Despawn(GameObject go)
    {
        go.transform.SetParent(transform);
        go.SetActive(false);
        _objectList.Add(go);
        trim();
    }

    //FIFO 如果dormantObjects大于最大个数则将之前的GameObject都推出来。
    public void trim()
    {
        while (_objectList.Count > _capacity)
        {
            GameObject temp = _objectList[0];
            _objectList.RemoveAt(0);
            Destroy(temp);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.F3))
        {
            //测试对象池机制   
            var go = GameObject.Find("Cube");
            var go1 = Spawn(go);
            StartCoroutine(Recovery(go1));
        }
    }
    IEnumerator Recovery(GameObject go)
    {
        yield return new WaitForSeconds(5);
        Despawn(go);
    }
}
