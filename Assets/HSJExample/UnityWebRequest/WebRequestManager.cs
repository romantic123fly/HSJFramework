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
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Networking;


public class WebRequestManager : BaseManager<WebRequestManager>
{
    /*
    UnityWebRequest uwr = new UnityWebRequest();
    uwr.url = "http://www.mysite.com";
    uwr.method = UnityWebRequest.kHttpVerbGET;   // can be set to any custom method, common constants privided
    uwr.useHttpContinue = false;
    uwr.chunkedTransfer = false;
    uwr.redirectLimit = 0;  // disable redirects
    uwr.timeout = 60;       // don't make this small, web requests do take some time 
    */

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="action"></param>
    public void Get(string url, Action<UnityWebRequest> actionResult)
    {
        StartCoroutine(_Get(url, actionResult));
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
    {
        StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
    }

    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    public void GetTexture(string url, Action<Texture2D> actionResult)
    {
        StartCoroutine(_GetTexture(url, actionResult));
    }

    /// <summary>
    /// 请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    public void GetAssetBundle(string url, Action<AssetBundle> actionResult)
    {
        StartCoroutine(_GetAssetBundle(url, actionResult));
    }

    /// <summary>
    /// 请求服务器地址上的音效
    /// </summary>
    /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    public void GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
    {
        StartCoroutine(_GetAudioClip(url, actionResult, audioType));
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="lstformData">form表单参数</param>
    /// <param name="lstformData">处理返回结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void Post(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        StartCoroutine(_Post(serverURL, lstformData, actionResult));
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
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
    /// <param name="url">请求地址,like 'http://www.my-server.com/ '</param>
    /// <param name="action">请求发起后处理回调结果的委托</param>
    /// <returns></returns>
    IEnumerator _Get(string url, Action<UnityWebRequest> actionResult)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();
            if (actionResult != null)
            {
                actionResult(uwr);
            }
        }
    }

    /// 下载文件
    UnityWebRequest uwr;
    IEnumerator _DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
    {
        uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);

        yield return uwr.SendWebRequest();
        if (actionResult != null)
        {
            actionResult(uwr);
        }
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
        if (actionResult != null)
        {
            actionResult(t);
        }
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
        if (actionResult != null)
        {
            actionResult(bundle);
        }
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

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="lstformData">form表单参数</param>
    /// <param name="lstformData">处理返回结果的委托</param>
    /// <returns></returns>
    IEnumerator _Post(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        UnityWebRequest uwr = UnityWebRequest.Post(serverURL, lstformData);
        yield return uwr.SendWebRequest();
        if (actionResult != null)
        {
            actionResult(uwr);
        }
    }
    //如果表单中传入的是中文，那么服务器接收到的是乱码，解决办法Debug.Log(PostWebRequest("http://192.168.1.1/EntryformServlet", "weixinid=测试&pwd=1234567" ));
    //private string PostWebRequest(string postUrl, string paramData)
    //{
    //    // 把字符串转换为bype数组
    //    byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(paramData);

    //    HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
    //    webReq.Method = "POST";
    //    webReq.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
    //    webReq.ContentLength = bytes.Length;
    //    using (Stream newStream = webReq.GetRequestStream())
    //    {
    //        newStream.Write(bytes, 0, bytes.Length);
    //    }
    //    using (WebResponse res = webReq.GetResponse())
    //    {
    //        //在这里对接收到的页面内容进行处理
    //        Stream responseStream = res.GetResponseStream();
    //        StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
    //        string str = streamReader.ReadToEnd();
    //        streamReader.Close();
    //        responseStream.Close();
    //        //返回：服务器响应流 
    //        return str;
    //    }
    //}

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <param name="resultAction">设置header文件中的Content-Type属性</param>
    /// <returns></returns>
    IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult, string contentType = "application/octet-stream")
    {
        UnityWebRequest uwr = new UnityWebRequest();
        UploadHandler uploader = new UploadHandlerRaw(contentBytes);

        // Sends header: "Content-Type: custom/content-type";
        uploader.contentType = contentType;

        uwr.uploadHandler = uploader;

        yield return uwr.SendWebRequest();

        bool res = true;
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            res = false;
        }
        if (actionResult != null)
        {
            actionResult(res);
        }
    }
}

