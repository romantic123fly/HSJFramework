#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             AssetsSerialize
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Assets",menuName ="CreatAsset",order =0)]
public class AssetsData : ScriptableObject
{
    public int id;
    public string name;
    public List<int> list;
}
