using System.Collections;
using System.Threading;
using System.Collections.Generic;

using System.IO;
using System.Diagnostics;
using System.Net;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.ComponentModel;

public class ThreadEvent {
    public string Key;
    public List<object> evParams = new List<object>();
}

public class NotiData {
    public string evName;
    public object evParam;

    public NotiData(string name, object param) {
        this.evName = name;
        this.evParam = param;
    }
}

public struct MessageTypes
{
    public const string CONNECT_SUCCESS = "connect_success";

    public const string ENTER_AR_SCENE = "enter_ar_scene";
    public const string ENTER_LOGIN_SCENE = "enter_login_scene";

    public const string Click_Upgrade = "click_upgrade";
    public const string UPDATE_MESSAGE = "UpdateMessage";           //更新消息
    public const string UPDATE_EXTRACT = "UpdateExtract";           //更新解包
    public const string UPDATE_DOWNLOAD = "UpdateDownload";         //更新下载
    public const string UPDATE_PROGRESS = "UpdateProgress";         //更新进度
}
namespace SimpleFramework.Manager
{
    /// <summary>
    /// 当前线程管理器，同时只能做一个任务
    /// </summary>
    public class ThreadManager : BaseManager<ThreadManager>
    {
        private Thread thread;
        private Action<NotiData> func;
        private Stopwatch sw = new Stopwatch();
        private string currDownFile = string.Empty;

        static readonly object m_lockObj = new object();
        static Queue<ThreadEvent> events = new Queue<ThreadEvent>();

        delegate void ThreadSyncEvent(NotiData data);
        private ThreadSyncEvent SyncEvent;

        private void Awake()
        {
            SyncEvent = OnSyncEvent;
            thread = new Thread(OnUpdate);
        }

        // Use this for initialization
        private void Start()
        {
            thread.Start();
        }

        /// <summary>
        /// 添加到事件队列
        /// </summary>
        public void AddEvent(ThreadEvent ev, Action<NotiData> func)
        {
            lock (m_lockObj)
            {
                this.func = func;
                events.Enqueue(ev);
            }
        }

        /// <summary>
        /// 通知事件
        /// </summary>
        /// <param name="state"></param>
        private void OnSyncEvent(NotiData data)
        {
            func?.Invoke(data);  //回调逻辑层
        }

        // Update is called once per frame
        void OnUpdate()
        {
            while (true)
            {
                lock (m_lockObj)
                {
                    if (events.Count > 0)
                    {
                        ThreadEvent e = events.Dequeue();
                        try
                        {
                            switch (e.Key)
                            {
                                case MessageTypes.UPDATE_PROGRESS:
                                    {
                                        Debug.LogError(e.evParams);
                                    }
                                    break;
                                case MessageTypes.UPDATE_EXTRACT:
                                    {     //解压文件
                                        OnExtractFile(e.evParams);
                                    }
                                    break;
                                case MessageTypes.UPDATE_DOWNLOAD:
                                    {    //下载文件
                                        OnDownloadFile(e.evParams); 
                                    }
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                           Debug.LogError(ex.Message);
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        void OnDownloadFile(List<object> evParams)
        {
            string url = evParams[0].ToString();
            currDownFile = evParams[1].ToString();

            try
            {
                using (WebClient client = new WebClient())
                {
                    sw.Start();
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    client.DownloadFileAsync(new System.Uri(url), currDownFile);
                    client.DownloadFileCompleted += DownloadFileCompleted;
                    client.Dispose();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Debug.Log("下载完成");
            NotiData data = new NotiData(MessageTypes.UPDATE_DOWNLOAD, currDownFile);
            SyncEvent?.Invoke(data);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //float pro = (float)e.ProgressPercentage / 100f;
            //Debug.LogWarning(pro);
            string value = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
            NotiData data = new NotiData(MessageTypes.UPDATE_PROGRESS, value);
            SyncEvent?.Invoke(data);
          
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        void OnExtractFile(List<object> evParams)
        {
            ///------------------通知更新面板解压完成--------------------
            NotiData data = new NotiData(MessageTypes.UPDATE_EXTRACT, null);
            SyncEvent?.Invoke(data);
        }

        /// <summary>
        /// 应用程序退出
        /// </summary>
        private void OnDestroy()
        {
            thread.Abort();
        }
    }
}