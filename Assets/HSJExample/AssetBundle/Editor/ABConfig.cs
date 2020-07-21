#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             ABConfig
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="ABConfig",menuName ="CreatABConfig",order =0 )]
public class ABConfig : ScriptableObject
{
    //单个文件所在的文件夹路径，会遍历这个文件夹下面所有的prefab，必须保证prefab名字唯一性
    public List<string> m_AllPrefabPath = new List<string>();
    public List<FileDirName> m_AllFileDirAB = new List<FileDirName>();
    [System.Serializable]
    public struct FileDirName
    {
        public string ABName;
        public string Path;
    }

}
