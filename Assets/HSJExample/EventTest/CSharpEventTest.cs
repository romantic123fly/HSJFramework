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
public delegate void Mydel(string str);

public class CSharpEventTest : MonoBehaviour
{
    public static event Mydel eventHandler;
    // Start is called before the first frame update

    public void HSjCSharpEvent()
    {
        //eventHandler?.Invoke("搞事情");
        if (eventHandler != null)
        {
			
            eventHandler("搞事情");
        }
    }
   
}
