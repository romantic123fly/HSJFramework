#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             Test
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CSVTest : MonoBehaviour
{
    public Text text1;
    public Text text2;
    public Text text3;
    private List<CarConfigData> carList;
    // Start is called before the first frame update
    void Start()
    {
        //示例
        carList = GameConfigDataBase.GetConfigDatas<CarConfigData>();
        text1.text = carList[1].carName;
        text2.text = carList[1].description;
        text3.text = carList[1].beginAcc.ToString();
    }
}
