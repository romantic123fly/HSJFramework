#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             EventTest
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HsjDel();
public delegate void HsjDel1(string str);
public delegate void HsjDel<T>(T t);
public delegate void HsjDel<T1,T2>(T1 t1,T2 t2);

public class DelegateTest : MonoBehaviour
{
    HsjDel hsjDel;
    HsjDel1 hsjDel1;
    HsjDel<GameObject> hsjDel2;
    HsjDel<string, int> hsjDel3;

    // Start is called before the first frame update
    void Start()
    {
        hsjDel += () => { Debug.Log("HsjDel"); };
        hsjDel1 += (a) => { Debug.Log(a); };
        hsjDel2 += (go) => { Debug.Log(go.name); };
        hsjDel3 += (a, b) => { Debug.Log(a + b); };

    }

    public void HsjDelegate()
    {
        hsjDel();
        hsjDel1("HsjDel1");
        hsjDel2(gameObject);
        hsjDel3("夺命", 1000);
    }
}
