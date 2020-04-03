#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             SetFestivalView
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetFestivalView : MonoBehaviour
{
    public  Dropdown dp;
    public GameObject MainView;
    // Start is called before the first frame update
    void Start()
    {
        dp.onValueChanged.AddListener((a)=> {
            PostersManager.GetInstance().theCurrentFestival = dp.options[a].text;
            PostersManager.GetInstance().CreateInfoPath();
            gameObject.SetActive(false);
            MainView.SetActive(true);
        });
    }
}
