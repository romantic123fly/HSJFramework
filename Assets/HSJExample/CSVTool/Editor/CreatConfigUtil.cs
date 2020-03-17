#region 模块信息
// **********************************************************************
// Copyright (C) 2018 Blazors
// Please contact me if you have any questions
// File Name:             CreatConfigUtil
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreatConfigUtil
{
    public static void CreatConfigFile(Object selectObj,string writePath)
    {
        string fileName = selectObj.name;
        string className = fileName;
        //创建.csv文件到指定位置
        StreamWriter sw = new StreamWriter(Application.dataPath+writePath+className+".cs");
        //写入命名空间
        sw.WriteLine("using UnityEngine;\nusing System.Collections;");
        sw.WriteLine("//幻世界 "+System.DateTime.Now.ToString());
        sw.WriteLine("public  partial  class" +" "+ className +" : GameConfigDataBase");
        sw.WriteLine("{");

        string filePath = AssetDatabase.GetAssetPath(selectObj);
        //解析csv文件
        CsvStreamReader csr = new CsvStreamReader(filePath);

        for (int colNum = 1; colNum < csr.ColCount+1; colNum++)
        {
            string filedName = csr[1, colNum];
            string filedType = csr[2, colNum];
            sw.WriteLine("\t"+"public"+ " "+filedType + " "+filedName+";");
        }

        sw.WriteLine("\t"+"protected override string getFilePath()");
        sw.WriteLine("\t"+"{");
        sw.WriteLine("\t\t"+"return"+"\""+fileName+"\";");
        sw.WriteLine("\t"+"}");
        sw.WriteLine("}");
        sw.Flush();
        sw.Close();
        AssetDatabase.Refresh();
    }

}
