#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             GameDefine
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDefine : MonoBehaviour
{
    public const int GameFrameRate = 30;                       //游戏帧频
    public const string AppName = "Hsj";           //应用程序名称
    public const string ExtName = ".assetbundle";              //素材扩展名
    public const string AssetDirname = "StreamingAssets";      //素材目录 
    
    // ab本地存放目录
    public static string GetAbDataPath()
    {
        if (Application.isMobilePlatform)
        {
            return Application.persistentDataPath + "/" + AppName + "/StreamingAssets/";
        }
        else
        {
            return Directory.GetParent(Application.dataPath) + "/"+AppName + "/StreamingAssets/";
        }
    }

  
}
