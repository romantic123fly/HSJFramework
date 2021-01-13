#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             WebRequestManager
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class WebRequestManager : BaseManager<WebRequestManager>
{

    public void Get(string url, Action<UnityWebRequest> actionResult)
    {
        StartCoroutine(_Get(url, actionResult));
    }

    public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
    {
        StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
    }

    public void GetTexture(string url, Action<Texture2D> actionResult)
    {
        StartCoroutine(_GetTexture(url, actionResult));
    }

    public void GetAssetBundle(string url, Action<AssetBundle> actionResult)
    {
        StartCoroutine(_GetAssetBundle(url, actionResult));
    }

    public void GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
    {
        StartCoroutine(_GetAudioClip(url, actionResult, audioType));
    }

    public void Post(string serverURL, WWWForm wWWForm, Action<UnityWebRequest> actionResult)
    {
        wWWForm.AddField("test","我是测试内容");
        StartCoroutine(_Post(serverURL, wWWForm, actionResult));
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <returns></returns>
    public void UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult)
    {
        StartCoroutine(_UploadByPut(url, contentBytes, actionResult, ""));
    }

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="action">请求发起后处理回调结果的委托</param>
    /// <returns></returns>
    IEnumerator _Get(string url, Action<UnityWebRequest> actionResult)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();
            actionResult?.Invoke(uwr);
        }
    }

    /// 下载文件
    UnityWebRequest uwr;
    IEnumerator _DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
    {
        uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);

        yield return uwr.SendWebRequest();
        actionResult?.Invoke(uwr);
    }
    float a;
    
    protected override void Update()
    {
        if (uwr!=null)
        {
            Debug.Log(uwr.downloadProgress);
        }
    }
    /// 请求图片
    IEnumerator _GetTexture(string url, Action<Texture2D> actionResult)
    {
        UnityWebRequest uwr = new UnityWebRequest(url);
        DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
        uwr.downloadHandler = downloadTexture;
        yield return uwr.SendWebRequest();
        Texture2D t = null;
        if (!(uwr.isNetworkError || uwr.isHttpError))
        {
            t = downloadTexture.texture;
        }
        actionResult?.Invoke(t);
    }

    /// 请求AssetBundle
    IEnumerator _GetAssetBundle(string url, Action<AssetBundle> actionResult)
    {
        UnityWebRequest www = new UnityWebRequest(url);
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
        www.downloadHandler = handler;
        yield return www.SendWebRequest();
        AssetBundle bundle = null;
        if (!(www.isNetworkError || www.isHttpError))
        {
            bundle = handler.assetBundle;
        }
        actionResult?.Invoke(bundle);
    }

    //音效
    IEnumerator _GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            yield return uwr.SendWebRequest();
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                actionResult?.Invoke(DownloadHandlerAudioClip.GetContent(uwr));
            }
        }
    }

    /// 向服务器提交post请求
    IEnumerator _Post(string serverURL, WWWForm wWWForm, Action<UnityWebRequest> actionResult)
    {   
        UnityWebRequest uwr = UnityWebRequest.Post(serverURL, wWWForm);
        yield return uwr.SendWebRequest();
        actionResult?.Invoke(uwr);
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <param name="resultAction">设置header文件中的Content-Type属性</param>
    /// <returns></returns>
    IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult, string contentType = "application/octet-stream")
    {
        UnityWebRequest uwr = new UnityWebRequest();
        UploadHandler uploader = new UploadHandlerRaw(contentBytes);

        uploader.contentType = contentType;

        uwr.uploadHandler = uploader;

        yield return uwr.SendWebRequest();

        bool res = true;
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            res = false;
        }
        actionResult?.Invoke(res);
    }
}

