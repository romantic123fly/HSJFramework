#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             WebRequestDemo
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebRequestDemo : MonoBehaviour
{
    public RawImage rawImage;
    // Start is called before the first frame update
    void Start()
    {
        //访问网站
        //WebRequestManager.Instance.Get("http://www.huanshj.com/",(a)=> { Debug.LogError(a.isDone); });
        //下载服务器视频文件
        //WebRequestManager.Instance.DownloadFile("http://www.huanshj.com/World/Videos/1.mp4", Application.dataPath + "/HSJExample/UnityWebRequest/aaa.mp4", (a) => { Debug.LogError(a.downloadProgress); });
        //下载服务器图片文件
        WebRequestManager.Instance.GetTexture("http://www.huanshj.com/Private/Wedding/Pictures/Xu001.jpg", (a) => { rawImage.texture = a; });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
