#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Loooto
// Please contact me if you have any questions
// File Name:             AssetFinder
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Threading;
using System;
using System.IO;
//通过指定目标脚本查找Asset目录下的预制体
public class AssetPrefabFinder : EditorWindow
{
    static AssetPrefabFinder window;
    //路径可配置
    public string fullPath ="Assets/Resources";
    MonoScript scriptObj = null;
    int loopCount = 0;
    List<Transform> results = new List<Transform>();

    [MenuItem("HSJ/EditorTools/Finder/AssetFinder")]
    static void Execute()
    {
        if (window == null)
        {
            window = (AssetPrefabFinder)GetWindow(typeof(AssetPrefabFinder));
        }
        window.Show();
    }
    private void OnGUI()
    {
        GUILayout.Label("指定目标脚本:");
        scriptObj = (MonoScript)EditorGUILayout.ObjectField(scriptObj, typeof(MonoScript), true);
        if (GUILayout.Button("Find"))
        {
            results.Clear();
            loopCount = 0;
            Debug.Log("开始查找Assets下带此脚本的预制体.");
            FindScript();
        }
        if (results.Count > 0)
        {
            foreach (Transform t in results)
            {
                EditorGUILayout.ObjectField(t, typeof(Transform), false);
            }
        }
    }
    void FindScript()
    {
        List<string> prefabs_names = new List<string>();

        //获取指定路径下面的所有资源文件
        if (Directory.Exists(fullPath))
        {
            string[] subFolders = Directory.GetDirectories(fullPath);
            foreach (var item in subFolders)
            {
                DirectoryInfo direction = new DirectoryInfo(item);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".prefab"))
                    {
                        prefabs_names.Add(files[i].Name.Split('.')[0]);
                    }
                }
            }
           
        }

        if (scriptObj != null)
        {
            for (int i = 0; i < prefabs_names.Count; i++)
            {
                loopCount++;
                //通过assetbundle名字获取当前ab资源的路径
                string str1 = GetPrefabPath(prefabs_names[i]);
                GameObject go = AssetDatabase.LoadAssetAtPath(str1, typeof(System.Object)) as GameObject;
                if (go != null)
                {
                    if (go.transform.childCount < 1)
                    {
                        if (go.GetComponent(scriptObj.GetClass()) != null)
                        {
                            Debug.Log("<color=darkblue>Find it: " + go.name + "</color>");
                            results.Add(go.transform);
                        }
                    }
                    else
                    {
                        foreach (Transform _child in go.transform)
                        {
                            if (_child.GetComponent(scriptObj.GetClass()) != null)
                            {
                                if (!results.Contains(go.transform))
                                {
                                    results.Add(go.transform);
                                }
                                Debug.Log("<color=darkblue>Find it: " + go.name + "/" + _child.name + "</color>");
                            }
                        }
                    }
                }
            }
        }
        if (results.Count <= 0)
        {
            Debug.LogError("Oops: Cant find that you want !");
        }
    }
    public string GetPrefabPath(string name)
    {
        string[] subFolders = Directory.GetDirectories(fullPath);
        string[] guids = null;
        string[] assetPaths = null;
        int i = 0, iMax = 0; 
        foreach (var folder in subFolders)
        {
            guids = AssetDatabase.FindAssets("t:Prefab", new string[] { fullPath });
            assetPaths = new string[guids.Length];
            for (i = 0, iMax = assetPaths.Length; i < iMax; ++i)
            {
                assetPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (Path.GetFileNameWithoutExtension(assetPaths[i]) == name)
                {
                    return assetPaths[i];
                }
            }
        }
        return null;
    }
}