#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             Director
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAdaptive : GameMonoBehaviour {
    // Use this for initialization
    protected override  void Start () {
        float standard_width = 1080;        //初始宽度  
        float standard_height = 1920;       //初始高度  
        float device_width = 0;                //当前设备宽度  
        float device_height = 0;

        float adjustor = 0f;         //屏幕矫正比例  
        //获取设备宽高  
        device_width = Screen.width;
        device_height = Screen.height;
     
        float standard_aspect = standard_width / standard_height;//默认宽高比
        float device_aspect = device_width / device_height;//当前屏幕宽高比
        CanvasScaler canvasScalerTemp = transform.GetComponent<CanvasScaler>();

        //计算矫正比例  
        if (standard_aspect > device_aspect)
        {
            canvasScalerTemp.matchWidthOrHeight = 0;
        }
        else
        {
            canvasScalerTemp.matchWidthOrHeight = 1;
        }
    }
	
}
