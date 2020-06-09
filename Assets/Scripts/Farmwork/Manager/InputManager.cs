#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             InputManager
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : BaseManager<InputManager> {

    protected override void Awake()
    {
        base.Awake();
    }

    public Vector3 mousePosition
    {
        get
        {
            return Input.mousePosition;
        }
    }

    public Vector3 getDirection()
    {
        var direction = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1.0f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1.0f;
        }
        return direction;
    }

    protected override void Update()
    {
        base.Update();

    }
}
