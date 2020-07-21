#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             Item
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
//可被序列化的标签
[System.Serializable]
public class Item
{
    //序列化的属性标签
    [XmlAttribute("Id")]
    public int Id { get; set; }
    [XmlAttribute("Name")]
    public string Name { get; set; }
    [XmlElement("List")]
    //[XmlArray("List")]
    public List<int> List { get; set; }
}
