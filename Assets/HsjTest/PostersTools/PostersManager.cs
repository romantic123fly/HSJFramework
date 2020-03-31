#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             PostersManager
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PostersManager : MonoBehaviour
{
    private static PostersManager instance;
    public static PostersManager GetInstance(){
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }
    private Color textColor;
    public List<Text> textList =new List<Text> ();
    public List<OutpatientInfo> outpatientInfoList = new List<OutpatientInfo> ();
    public Dictionary<string, Dictionary<string, string>> cfgData;
    public Color TextColor {
        get => textColor;
        set
        {
            textColor = value;
            if (textList.Count > 0)
            {
                foreach (var item in textList)
                {
                    item.color = textColor;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
        string cfgPath = Application.streamingAssetsPath + "/1.txt";
        if (File.Exists(cfgPath))
        {
            StartCoroutine(LoadCfg(cfgPath));
        }
    }

    public IEnumerator LoadCfg(string cfgPath)
    {
        WWW www = new WWW(cfgPath);
        yield return www;
        if (www.isDone)
        {
           var a= ExplainString(www.text);
        }
    }
    public static Dictionary<string, Dictionary<string, string>> ExplainString(string strLine)
    {
        Dictionary<string, string[]> content = new Dictionary<string, string[]>();
        string[] lineArray = strLine.Replace("\r\n", "*").Split(new char[] { '*' });
        //获取行数
        int rows = lineArray.Length - 1;
        //获取列数
        int Columns = lineArray[0].Split(new char[] { '\t' }).Length;
        //定义一个数组用于存放字段名
        string[] ColumnName = new string[Columns];
        for (int i = 0; i < rows; i++)
        {
            string[] Array = lineArray[i].Split(new char[] { '\t' });
            for (int j = 0; j < Columns; j++)
            {
                //获取Array的列的值
                string nvalue = Array[j].Trim();
                if (i == 0)
                {
                    //存储字段名
                    ColumnName[j] = nvalue;
                    content[ColumnName[j]] = new string[rows - 1];
                }
                else
                {
                    //存储对应字段的默认值//<字段名，默认值>
                    content[ColumnName[j]][i - 1] = nvalue;
                }
            }
        }
        //开始解析
        return ExplainDictionary(content);
    }
    public static Dictionary<string, Dictionary<string, string>> ExplainDictionary(Dictionary<string, string[]> content)
    {
        Dictionary<string, Dictionary<string, string>> DicContent = new Dictionary<string, Dictionary<string, string>>();
        //获取字典中所有的键(字段名)
        Dictionary<string, string[]>.KeyCollection Allkeys = content.Keys;
        //遍历所有的字段名
        foreach (string key in Allkeys)
        {
            //实例化一个hasData的字典//<ID,字段值>
            Dictionary<string, string> hasData = new Dictionary<string, string>();
            string[] Data = content[key];
            for (int i = 0; i < Data.Length; i++)
            {
    
                //<ID><字段值>
                hasData[content[key][i]] = Data[i];
            }
            DicContent[key] = hasData;
        }
        return DicContent;
    }

}
