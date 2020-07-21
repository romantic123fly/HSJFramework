#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             XmlSerilize
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class SerilizeTool : MonoBehaviour
{
    private void Start()
    {
        //XmlSerilize();
        //XmlDeSerialize();
        //BinarySer();
        //BinaryDeserializer();
        ReadAssetData();

    }
    //Xml序列化
    public void XmlSerilize()
    {
        Item item = new Item();
        item.Id = 1001;
        item.Name = "第一";
        item.List = new List<int> { 0, 1, 2, 3 };

        FileStream fileStream = new FileStream(Application.dataPath + "/HSJExample/XmlTool/Item.Xml", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xml = new XmlSerializer(item.GetType());
        xml.Serialize(sw, item);
        sw.Close();
        fileStream.Close();
    }
    //Xml反序列化
    public void XmlDeSerialize()
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/HSJExample/XmlTool/Item.Xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xs = new XmlSerializer(typeof(Item));
        Item item = xs.Deserialize(fileStream) as Item;
        Debug.Log(item.Id);
        Debug.Log(item.Name);
        Debug.Log(item.List.Count);

        fileStream.Close();
    }

    void BinarySer()
    {
        Item item = new Item();
        item.Id = 1002;
        item.Name = "第二";
        item.List = new List<int> { 0, 10, 20, 30 };
        BinarySerialize(item);
    }
    //类转二进制序列化
    void BinarySerialize(Item item)
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/HSJExample/XmlTool/Item.bytes", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, item);
        fileStream.Close();
    }
    //二进制数据反序列化
    void BinaryDeserializer()
    {
        //FileStream fileStream = new FileStream(Application.dataPath + "/HSJExample/XmlTool/Item.bytes", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        //BinaryFormatter binaryFormatter = new BinaryFormatter();
        //Item item=  binaryFormatter.Deserialize(fileStream) as Item;
        //Debug.Log(item.Id);
        //Debug.Log(item.Name);
        //Debug.Log(item.List.Count);
        //fileStream.Close();

        //另外一种写法
        TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/HSJExample/XmlTool/Item.bytes");
        MemoryStream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        Item item1 = (Item)bf.Deserialize(stream);
        Debug.Log(item1.Id);
        Debug.Log(item1.Name);
        Debug.Log(item1.List.Count);
        stream.Close();
    }

    void ReadAssetData()
    {
        AssetsData assets = UnityEditor.AssetDatabase.LoadAssetAtPath<AssetsData>("Assets/HSJExample/XmlTool/Assets.asset");
        Debug.Log(assets.id);
        Debug.Log(assets.name);
        Debug.Log(assets.list.Count);
    }
}
