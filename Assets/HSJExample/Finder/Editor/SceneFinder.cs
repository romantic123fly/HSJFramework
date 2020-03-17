#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Loooto
// Please contact me if you have any questions
// File Name:             MonoFinder
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//查找节点及所有子节点中,挂载有指定脚本的物体
public class SceneFinder : EditorWindow
{
    Transform root = null;
    MonoScript scriptObj = null;
    int loopCount = 0;

    List<Transform> results = new List<Transform>();

    [MenuItem("HSJ/EditorTools/Finder/SceneFinder")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(SceneFinder));
    }

    void OnGUI()
    {
        GUILayout.Label("选择场景内某根节点:");
        root = (Transform)EditorGUILayout.ObjectField(root, typeof(Transform), true);
        GUILayout.Label("指定目标脚本类型:");
        scriptObj = (MonoScript)EditorGUILayout.ObjectField(scriptObj, typeof(MonoScript), true);
        if (GUILayout.Button("Find"))
        {
            results.Clear();
            loopCount = 0;
            Debug.Log("开始查找.");
            FindScript(root);
        }
        if (results.Count > 0)
        {
            foreach (Transform t in results)
            {
                EditorGUILayout.ObjectField(t, typeof(Transform), false);
            }
        }
        else
        {
            GUILayout.Label("无数据");
        }
    }

    void FindScript(Transform root)
    {
        if (root != null && scriptObj != null)
        {
            loopCount++;
            Debug.Log("<color=darkblue>.." + loopCount + ":" + root.gameObject.name+ "</color>");
            if (root.GetComponent(scriptObj.GetClass()) != null)
            {
                results.Add(root);
            }
            foreach (Transform t in root)
            {
                FindScript(t);
            }
        }
    }
}
