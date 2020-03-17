#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             JsonParser
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using LitJson;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonParser
{
    #region Unity自己的工具JsonUtility

    public static string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj); ;
    }
    public static T JsonToObject<T>(string  jsonStr)
    {
        return JsonUtility.FromJson<T>(jsonStr);
    }
    #endregion

    #region LitJson用法
    //对象序列化Json
    public static string JsonSerialize(object obj)
    {
        return JsonMapper.ToJson(obj);
    }

    //Json反序列化
    public static object JsonDeSerialize(string jsonStr)
    {
        return JsonMapper.ToObject(jsonStr);
    }
    public static T JsonDeSerialize<T>(string jsonStr)
    {
        return JsonMapper.ToObject<T>(jsonStr);
    }
    //创建Json数据
    public static string CreatJsonData(string[] keys, string[] values)
    {
        JsonData jsonData = new JsonData();
        for (int i = 0; i < keys.Length; i++)
        {
            jsonData[keys[i]] = values[i];
        }
        return jsonData.ToJson();
    }
    /// <summary>
    /// 创建Json数据 数据嵌套
    /// </summary>
    /// <param name="keys">正常key</param>
    /// <param name="index">嵌套key下标</param>
    /// <param name="values">正常value</param>
    /// <param name="nestKeys">嵌套keys</param>
    /// <param name="nestValues">嵌套values</param>
    /// <returns></returns>
    public static string CreatJsonData(string[] keys,int index, string[] values, string [] nestKeys,  string [] nestValues)
    {
        JsonData jsonData = new JsonData();
        for (int i = 0; i < keys.Length; i++)
        {
            if (i==index)
            {
                jsonData[keys[i]] = new JsonData();
                for (int j = 0; j < nestValues.Length; j++)
                {
                    jsonData[keys[i]][nestKeys[j]] = nestValues[j];
                }
            }
            else
            {
                jsonData[keys[i]] = values[i];
            }
        }
        return jsonData.ToJson();
    }

    #endregion


    #region Newtonsoft 用法
    public static string NJsonSerialize(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T NJsonDeSerialize<T>(string jsonStr)
    {
        return JsonConvert.DeserializeObject<T>(jsonStr);
    }
    #endregion
}
