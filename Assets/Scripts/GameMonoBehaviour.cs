#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// File Name:             GameMonoBehaviour
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMonoBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }
    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }
    protected virtual void OnDestroy()
    {
        Destroy(gameObject);
    }
}
