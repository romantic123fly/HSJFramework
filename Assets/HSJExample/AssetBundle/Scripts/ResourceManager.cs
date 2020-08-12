#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Text;
using UnityEngine;

public class ResourceManager:BaseManager<ResourceManager>
{
    public static Object load(string path)
    {
        var res = Resources.Load(path);

        if (res == null)
        {
            return new Object();
        }
        return res;
    }

    public static Sprite loadSprite(string path)
    {
        var res = Resources.Load(path, typeof(Sprite)) as Sprite;
        if (res == null)
        {
            return new Object() as Sprite;
        }
        return res;
    }

    public static Object load(string path, System.Type systemTypeInstance)
    {
        return Resources.Load(path, systemTypeInstance);
    }

    public static GameObject loadPrefab(string path, Transform parent = null)
    {
        var prefab = load(path);
        if (prefab == null)
        {
            Debug.LogWarning("Can't find prefab:" + path);
            prefab = new GameObject("Unknown");
        }
        var go = GameObject.Instantiate(prefab) as GameObject;
        go.transform.SetParent(parent);
        go.transform.localPosition = Vector3.zero;
        //go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        return go;
    }

    public static string getDataPath(string path)
    {
        return string.Format("{0}/Resources/{1}",  Application.dataPath, path);
    }

    public static string getPersistentDataPath(string path)
    {
        return string.Format("{0}/{1}", Application.persistentDataPath, path);
    }

    public static string getStreamingAssetsPath(string path)
    {
        return string.Format("{0}/{1}", Application.streamingAssetsPath, path);
    }
}
