﻿#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             LoadAB
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadPrefabByAB : MonoBehaviour
{
    private void Awake()
    {
        ResourceManager.Instance.CheckExtractResource();
    }
    void Start()
    {
        //第一种方式
        //LoadAssetBundle();
        //第二种方式
        var bundle = ResourceManager.Instance.LoadAssetBundle("prefabs/giraffe");
        Instantiate(bundle.LoadAsset<GameObject>("giraffe"));
        var bundle1 = ResourceManager.Instance.LoadAssetBundle("prefabs/cube");
        Instantiate(bundle1.LoadAsset<GameObject>("cube"));
    }

    // Update is called once per frame
    void LoadAssetBundle()
    {
        //加载ab包信息配置列表
        AssetBundle configAB = AssetBundle.LoadFromFile( Application.streamingAssetsPath + "/assetbundleconfig");
     
        TextAsset textAsset = configAB.LoadAsset<TextAsset>("AssetBundleConfig");
        Debug.Log(textAsset.bytes.Length);
        MemoryStream stream = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        AssetBundleConfig abc = bf.Deserialize(stream) as AssetBundleConfig;
        stream.Close();

        string path = "Assets/HSJExample/AssetBundle/Prefabs/Giraffe.prefab";
        uint crc = Crc32.GetCRC32(path);
        Debug.Log(crc);
        ABBase abBase = null;
        for (int i = 0; i < abc.ABList.Count; i++)
        {
            Debug.Log(abc.ABList[i].Crc + abc.ABList[i].Path);
            if (abc.ABList[i].Crc ==crc)
            {
                abBase = abc.ABList[i];
            }
        }
        for (int i = 0; i < abBase.AbDependce.Count; i++)
        {
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + abBase.AbDependce[i]);
        }

        AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath+"/"+abBase.ABName);
        GameObject obj = Instantiate(assetBundle.LoadAsset<GameObject>("Giraffe"));
    }
}
