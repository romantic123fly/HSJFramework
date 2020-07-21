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
    public GameObject target;
 
    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
        float ver = Input.GetAxis("Vertical");
        target.transform.position += target.transform.forward * ver * Time.deltaTime * 5;

        float hor = Input.GetAxis("Horizontal");
        target.transform.Rotate(target.transform.up * hor * Time.deltaTime * 100,Space.Self);
    }
    //单击
    public void Click()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            Debug.Log(raycastHit.collider.name);
        }
    }
}
