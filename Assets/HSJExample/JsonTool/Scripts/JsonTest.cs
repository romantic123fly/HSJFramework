#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             JsonTest
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    private string JsonDataPath = "/JsonTool/Resources/JsonData";
    // Start is called before the first frame update
    void Start()
    {
        Dog dog = new Dog("旺财","黄","2","公","15");
        //对象转换为json
        //var dogJsonData = JsonParser.ObjectToJson(dog);
        //var dogJsonData = JsonParser.JsonSerialize(dog);
        var dogJsonData = JsonParser.NJsonSerialize(dog);
        if (!File.Exists(Application.dataPath+ JsonDataPath+"/Dog.json"))
        {
            File.Create(Application.dataPath + JsonDataPath + "/Dog.json");
        }

        //存储Json文件
        FileStream fs = File.Open(Application.dataPath + JsonDataPath + "/Dog.json", FileMode.OpenOrCreate);
        byte[] msgAsByteArray = System.Text.Encoding.Default.GetBytes(dogJsonData);
        //写入文件
        fs.Write(msgAsByteArray, 0, msgAsByteArray.Length);
        //重置流的内部位置
        fs.Position = 0;
        fs.Close();
        fs.Dispose();
        Debug.Log(dogJsonData);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            string data =  File.ReadAllText(Application.dataPath + JsonDataPath + "/Dog.json", System.Text.Encoding.UTF8);
            var dogJsonData = JsonParser.JsonToObject<Dog>(data);
            //var dogJsonData = JsonParser.JsonDeSerialize<Dog>(data);
            //var dogJsonData = JsonParser.NJsonDeSerialize<Dog>(data);
            Debug.Log(dogJsonData.name);
            Debug.Log(dogJsonData.color);
            Debug.Log(dogJsonData.age);
        }
    }
}
public class Dog
{
    public string name;
    public string color;
    public string age;
    public string gender;
    public string weight;

    public Dog()
    {
    }

    public Dog(string name, string color, string age, string gender, string weight)
    {
        this.name = name;
        this.color = color;
        this.age = age;
        this.gender = gender;
        this.weight = weight;
    }
}
