using System.Collections;
using System.Threading;
using System.Collections.Generic;

using System.IO;
using System.Diagnostics;
using System.Net;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
            if (this.func != null) func(data);  //回调逻辑层
            Debug.Log(data.evName + data.evParam);
            //facade.SendMessageCommand(data.evName, data.evParam); //通知View层
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
                        catch (System.Exception ex)
                        {
                            UnityEngine.Debug.LogError(ex.Message);
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
                    //client.DownloadFileCompleted += delegate { UnityEngine.Debug.Log("下载完成"); };
                    client.Dispose();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //UnityEngine.Debug.Log(e.ProgressPercentage);
            /*
            UnityEngine.Debug.Log(string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00")));
            */
            //float value = (float)e.ProgressPercentage / 100f;

            string value = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
            Debug.Log(value);
            NotiData data = new NotiData(MessageTypes.UPDATE_PROGRESS, value);
            if (SyncEvent != null) SyncEvent(data);

            if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
            {
                sw.Reset();

                data = new NotiData(MessageTypes.UPDATE_DOWNLOAD, currDownFile);
                if (SyncEvent != null) SyncEvent(data);
            }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        void OnExtractFile(List<object> evParams)
        {
            Debug.LogWarning("Thread evParams: >>" + evParams.Count);

            ///------------------通知更新面板解压完成--------------------
            NotiData data = new NotiData(MessageTypes.UPDATE_DOWNLOAD, null);
            if (SyncEvent != null) SyncEvent(data);
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