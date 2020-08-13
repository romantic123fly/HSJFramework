#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             BundleEditor
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class BundleEditor 
{
    private static string AbTargetPath = Application.streamingAssetsPath;
    private static string AbConfigPath = "Assets/HSJExample/AssetBundle/Editor/ABConfig.asset";
    //存放所有文件夹的ab路径，key：包名；value：路径
    private static Dictionary<string, string> m_AllFileDir = new Dictionary<string, string>();
    //存放单个Prefab的ab路径，key：包名；value：路径
    private static Dictionary<string, List<string>> m_AllPrefabDir = new Dictionary<string, List<string>>();
    //过滤的Ab列表
    private static List<string> m_AllFileAB = new List<string>();
    //储存所有有效路径
    private static List<string> m_UsefulABList = new List<string>();
    [MenuItem("HSJ/清空AB包名",priority =0)]
    static void ClearABName()
    {
        string[] oldABNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < oldABNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(oldABNames[i], true);
            EditorUtility.DisplayProgressBar("清除Ab包名", "名字：" + oldABNames[i], i / oldABNames.Length);
        }

        Directory.Delete(Application.streamingAssetsPath, true);
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }
    

[MenuItem("HSJ/打AB包/方式一", priority = 0)]
    public static void BuildAb()
    {
        m_AllFileDir.Clear();
        m_AllPrefabDir.Clear();
        m_AllFileAB.Clear();
        m_UsefulABList.Clear();

        ABConfig abConfig = AssetDatabase.LoadAssetAtPath<ABConfig>(AbConfigPath);
        foreach (var item in abConfig.m_AllFileDirAB)
        {
            if (m_AllFileDir.ContainsKey(item.ABName))
            {
                Debug.LogError("ABConfig配置错误");
            }
            else
            {
                m_AllFileDir.Add(item.ABName, item.Path);
                m_AllFileAB.Add(item.Path);
                m_UsefulABList.Add(item.Path);
            }
        }

        string[] allStr = AssetDatabase.FindAssets("t:Prefab",abConfig.m_AllPrefabPath.ToArray());
        for (int i = 0; i < allStr.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allStr[i]);
            m_UsefulABList.Add(path);
            EditorUtility.DisplayProgressBar("查找Prefab", "Prefab:" + path, i * 1.0f / allStr.Length) ;
            //如果m_AllFileDir没有添加过当前AB信息
            if (!ContainAllFileAB(path))
            {
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string[] allDependNames = AssetDatabase.GetDependencies(path);
                List<string> allDependPath = new List<string>();
                for (int j = 0; j < allDependNames.Length; j++)
                {
                    //如果依赖的资源在
                    if (!ContainAllFileAB(allDependNames[j]) && !allDependNames[j].EndsWith(".cs"))
                    {
                        m_AllFileAB.Add(allDependNames[j]);
                        allDependPath.Add(allDependNames[j]);
                    }
                }
                if (m_AllPrefabDir.ContainsKey(obj.name))
                {
                    Debug.LogError("存在相同名字的Prefab："+ obj.name);
                }
                else
                {
                    m_AllPrefabDir.Add(obj.name, allDependPath);
                }
            }
        }
        //设置文件夹的ab包名
        foreach (string name in m_AllFileDir.Keys)
        {
            SetABName(name, m_AllFileDir[name]);
        }
        //设置单个prefab的ab包名
        foreach (string name in m_AllPrefabDir.Keys)
        {
            SetABName(name, m_AllPrefabDir[name]);
        }
        //如果文件非常多就会很耗时
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();



        BuildAssetBundle();
        EditorUtility.ClearProgressBar();
    }
    /// <summary>
    /// 是否包含在已经有的AB包里，做来做AB包冗余剔除
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static bool ContainAllFileAB(string path)
    {
        for (int i = 0; i < m_AllFileAB.Count; i++)
        {
            if (path == m_AllFileAB[i] || (path.Contains(m_AllFileAB[i]) && (path.Replace(m_AllFileAB[i], "")[0] == '/')))
                return true;
        }

        return false;
    }

    static void SetABName(string name,string path)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(path);
        if (assetImporter == null)
        {
            Debug.LogError("不存在此路径文件：" + path);
        }
        else
        {
            assetImporter.assetBundleName = name;
        }
    }
    static void SetABName(string name, List<string> paths)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            SetABName(name, paths[i]);
        }
    }
    static void BuildAssetBundle()
    {
        string[] allBundles = AssetDatabase.GetAllAssetBundleNames();
        //key为全路径，value为包名
        Dictionary<string, string> resPathDic = new Dictionary<string, string>();
        for (int i = 0; i < allBundles.Length; i++)
        {
            string[] allBundlePath = AssetDatabase.GetAssetPathsFromAssetBundle(allBundles[i]);
            for (int j = 0; j < allBundlePath.Length; j++)
            {
                if (allBundlePath[j].EndsWith(".cs"))
                    continue;
                    resPathDic.Add(allBundlePath[j], allBundles[i]);
                    Debug.Log("ab包："+allBundles[i]+"包含的资源文件路径："+allBundlePath[j]);
            }
        }
        if (!Directory.Exists(AbTargetPath))
        {
            Directory.CreateDirectory(AbTargetPath);
        }
        //DeletUselessAB();
        //生成自己的配置表
        WriteData(resPathDic);
        BuildPipeline.BuildAssetBundles(AbTargetPath, BuildAssetBundleOptions.ChunkBasedCompression,EditorUserBuildSettings.activeBuildTarget);
    }

    static void WriteData(Dictionary<string, string> resPathDic)
    {
        AssetBundleConfig assetBundleConfig = new AssetBundleConfig();
        assetBundleConfig.ABList = new List<ABBase>();
        foreach (var path in resPathDic.Keys)
        {
            //if (!m_UsefulABList.Contains(path))
            //    continue;
            ABBase abBase = new ABBase();
            abBase.Path = path;
            abBase.Crc = Crc32.GetCRC32(path);
            abBase.ABName = resPathDic[path];
            abBase.AssetName = path.Remove(0,path.LastIndexOf("/")+1);
            abBase.AbDependce = new List<string>();

            string[] resDependce = AssetDatabase.GetDependencies(path);
            for (int i = 0; i < resDependce.Length; i++)
            {
                string tempPath = resDependce[i];
                if (tempPath == path || path.EndsWith(".cs"))
                    continue;
                string abName = "";
                if (resPathDic.TryGetValue(tempPath,out abName))
                {
                    if (abName == resPathDic[path])
                        continue;
                    if (!abBase.AbDependce.Contains(abName))
                    {
                        abBase.AbDependce.Add(abName);
                    }
                }
            }
            assetBundleConfig.ABList.Add(abBase);
        }
        //写入XML
        string xmlPath = Application.dataPath + "/HSJExample/AssetBundle/ABConfig.Xml";
        if (File.Exists(xmlPath)) File.Delete(xmlPath);
        FileStream fileStream = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xs = new XmlSerializer(assetBundleConfig.GetType());
        xs.Serialize(streamWriter, assetBundleConfig);
        streamWriter.Close();
        fileStream.Close();
        //写入二进
        string bytePath = Application.dataPath +"/HSJExample/AssetBundle/Data/AssetBundleConfig.bytes";
        FileStream fileStream1 = new FileStream(bytePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        BinaryFormatter bf = new BinaryFormatter();  
        bf.Serialize(fileStream1, assetBundleConfig);
        Debug.Log(fileStream1.Length);
        fileStream1.Close();
    }
    //删掉冗余的ab包
    static void DeletUselessAB()
    {
        //获取到所有的ab包名字
        string[] allBundlesName = AssetDatabase.GetAllAssetBundleNames();
      
        DirectoryInfo directoryInfo = new DirectoryInfo(AbTargetPath);
        FileInfo[] files = directoryInfo.GetFiles("*",SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            if (!allBundlesName.Contains(files[i].Name) || files[i].Name.EndsWith(".meta") || files[i].Name.EndsWith(".manifest") || files[i].Name.EndsWith("assetbundleconfig"))
                continue;
            else
            {
                if (File.Exists(files[i].FullName))
                {
                    File.Delete(files[i].FullName);
                    Debug.Log("删掉冗余的ab包:" + files[i].FullName);
                }
            }
        }
    }
}
