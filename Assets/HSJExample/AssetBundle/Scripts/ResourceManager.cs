#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using SimpleFramework.Manager;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager:BaseManager<ResourceManager>
{
    private string[] m_Variants = { };
    public AssetBundleManifest manifest;
    public AssetBundle assetbundle;
    private Dictionary<string, AssetBundle> bundles;
    public List<string> DownloadFiles = new List<string>();//资源更新列表
    /// <summary>
    ///资源初始化
    /// </summary>
    public void Initialize()
    {
        byte[] stream = null;
        bundles = new Dictionary<string, AssetBundle>();
        string filePath =Application.streamingAssetsPath+"/"+ GameDefine.AssetDirname;
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            stream = File.ReadAllBytes(filePath);
            assetbundle = AssetBundle.LoadFromMemory(stream);
            manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        Debug.Log("资源初始化结束，开始游戏。。。");
    }

    /// <summary>
    /// 载入素材
    /// </summary>
    public GameObject LoadAsset(string abname, string assetname)
    {
        abname = abname.ToLower();

        AssetBundle bundle = LoadAssetBundle(abname);
        return bundle.LoadAsset<GameObject>(assetname);
    }
    //载入sprite
    public Sprite LoadSprite(string abname, string spriteName = "")
    {
        abname = abname.ToLower();
        AssetBundle bundle = LoadAssetBundle(abname);
        return bundle.LoadAsset<Sprite>(spriteName);
    }
    //载入Material
    public Material LoadMaterial(string abname, string materialName)
    {
        abname = abname.ToLower();
        AssetBundle bundle = LoadAssetBundle(abname);
        return bundle.LoadAsset<Material>(materialName);
    }
    /// <summary>
    /// 载入AssetBundle
    /// </summary>
    /// <param name="abname"></param>
    /// <returns></returns>
     public   AssetBundle LoadAssetBundle(string abname)
    {
        if (!abname.EndsWith(GameDefine.ExtName))
        {
            abname += GameDefine.ExtName;
        }
        AssetBundle bundle = null;
        if (!bundles.ContainsKey(abname))
        {
            byte[] stream = null;
            string uri = GameDefine.GetAbDataPath() + abname;
            Debug.LogWarning("LoadFile::>> " + uri);
            LoadDependencies(abname);

            stream = File.ReadAllBytes(uri);
            bundle = AssetBundle.LoadFromMemory(stream); //关联数据的素材绑定
            bundles.Add(abname, bundle);
        }
        else
        {
            bundles.TryGetValue(abname, out bundle);
        }
        return bundle;
    }
    public TextAsset LoadCfg(string abName, string assetName)
    {
        abName = abName.ToLower();

        AssetBundle bundle = LoadAssetBundle(abName);
        return bundle.LoadAsset<TextAsset>(assetName);
    }
    /// <summary>
    /// 载入依赖
    /// </summary>
    /// <param name="name"></param>
    void LoadDependencies(string name)
    {
        if (manifest == null)
        {
            Debug.LogError("Please initialize AssetBundleManifest by calling ResourceManager.Initialize()");
            return;
        }
        // Get dependecies from the AssetBundleManifest object..
        string[] dependencies = manifest.GetAllDependencies(name);
        if (dependencies.Length == 0) return;

        for (int i = 0; i < dependencies.Length; i++)
            dependencies[i] = RemapVariantName(dependencies[i]);

        // Record and load all dependencies.
        for (int i = 0; i < dependencies.Length; i++)
        {
            LoadAssetBundle(dependencies[i]);
        }
    }

    // Remaps the asset bundle name to the best fitting asset bundle variant.
    string RemapVariantName(string assetBundleName)
    {
        string[] bundlesWithVariant = manifest.GetAllAssetBundlesWithVariant();

        // If the asset bundle doesn't have variant, simply return.
        if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
            return assetBundleName;

        string[] split = assetBundleName.Split('.');

        int bestFit = int.MaxValue;
        int bestFitIndex = -1;
        // Loop all the assetBundles with variant to find the best fit variant assetBundle.
        for (int i = 0; i < bundlesWithVariant.Length; i++)
        {
            string[] curSplit = bundlesWithVariant[i].Split('.');
            if (curSplit[0] != split[0])
                continue;

            int found = System.Array.IndexOf(m_Variants, curSplit[1]);
            if (found != -1 && found < bestFit)
            {
                bestFit = found;
                bestFitIndex = i;
            }
        }
        if (bestFitIndex != -1)
            return bundlesWithVariant[bestFitIndex];
        else
            return assetBundleName;
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (manifest != null) manifest = null;
    }

    //加载并获取View资源
    public GameObject LoadView(string name)
    {
        var a = name.Split('/');
        string assetname = a[a.Length - 1];
        GameObject prefab = LoadAsset(name, assetname);
        if (GameObject.Find(name) != null || prefab == null)
        {
            return null;
        }
        GameObject go = Instantiate(prefab) as GameObject;
        go.name = assetname;
        go.layer = LayerMask.NameToLayer("Default");
        go.transform.SetParent(GameObject.Find("Canvas").transform);
        go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
        go.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        go.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
        go.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(720, 0);
        return go;
        //go.transform.localScale = Vector3.one;
        //go.transform.localPosition = Vector3.zero;
    }
    public GameObject LoadPrefab(string abName, Transform parent = null, string assetname = "", bool isABLoad = true)
    {
        GameObject prefab = null;
        if (isABLoad)
        {
            assetname = assetname == "" ? abName : assetname;
            prefab = LoadAsset(abName, assetname);
        }
        else
        {
            prefab = Load(abName) as GameObject;
        }
        if (prefab == null)
        {
            Debug.LogWarning("Can't find prefab:" + abName);
            return null;
        }
        var go = UnityEngine.Object.Instantiate(prefab) as GameObject;
        go.transform.SetParent(parent);
        if (abName != "PetModel")
        {
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
        }
        return go;
    }
    public AudioClip LoadAudioClip(string abName, string assetname)
    {
        abName = abName.ToLower();
        AssetBundle bundle = LoadAssetBundle(abName);
        return bundle.LoadAsset<AudioClip>(assetname);
    }
    public static UnityEngine.Object Load(string path)
    {
        var res = Resources.Load(path);
        if (res == null)
        {
            return new UnityEngine.Object();
        }
        return res;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void CheckExtractResource()
    {
        //如果游戏资源已经释放到本地
        if (Directory.Exists(GameDefine.GetAbDataPath()) && File.Exists(GameDefine.GetAbDataPath() + "files.txt"))
        {
            //需检查版本号是否需要更新
            Initialize();
        }
        else//如果游戏资源未释放到本地就需钥执行释放资源的操作
        {
            StartCoroutine(OnExtractResource());    //启动释放协成 
        }
    }

    //释放资源
    IEnumerator OnExtractResource()
    {
        Debug.Log("开始释放资源");
        string dataPath = GameDefine.GetAbDataPath();  //数据目录
        string resPath = Application.streamingAssetsPath+"/"; //游戏包资源目录

        if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);

        Directory.CreateDirectory(dataPath);
       
        string outfile = dataPath + "files.txt";
        string infile = resPath + "files.txt";
        if (File.Exists(outfile)) File.Delete(outfile);

        Debug.Log(infile);
        Debug.Log(outfile);
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;

            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return 0;
        }
        else File.Copy(infile, outfile, true);
        yield return new WaitForEndOfFrame();


        //释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);
        foreach (var file in files)
        {
            string[] fs = file.Split('|');
            infile = resPath+ fs[0];  //
            outfile = dataPath + fs[0];

            Debug.Log("+++正在解包文件:>" + infile);
            Debug.Log("---正在解包文件:>" + outfile);

            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                if (File.Exists(outfile))
                {
                    File.Delete(outfile);
                }
                File.Copy(infile, outfile, true);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);

        Debug.Log("解包完成!!!");
        Initialize();

        //释放完成，开始启动更新资源
        //StartCoroutine(OnUpdateResource());
    }

    /// <summary>
    /// 启动更新下载，这里只是个思路演示，此处可启动线程下载更新
    /// </summary>
    public IEnumerator OnUpdateResource()
    {

        string dataPath = GameDefine.GetAbDataPath();  //数据目录
        string url ="www.hsj.com";
        Debug.Log("///////////////////////////////////////" + url);
        string message = string.Empty;
        //string random = DateTime.Now.ToString("yyyymmddhhmmss");
        string listUrl = url + "files.txt";
        Debug.LogWarning("LoadUpdate---->>>" + listUrl);

        WWW www = new WWW(listUrl); yield return www;
        if (www.error != null)
        {
            OnUpdateFailed(string.Empty);
            yield break;
        }
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        File.WriteAllBytes(dataPath + "files.txt", www.bytes);
        string filesText = www.text;
        string[] files = filesText.Split('\n');
      
        for (int i = 0; i < files.Length; i++)
        {
      
            if (string.IsNullOrEmpty(files[i])) continue;
            string[] keyValue = files[i].Split('|');
            string f = keyValue[0];
            string localfile = (dataPath + f).Trim();
            string path = Path.GetDirectoryName(localfile);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileUrl = url + f;
            bool canUpdate = !File.Exists(localfile);
            if (!canUpdate)
            {
                string remoteMd5 = keyValue[1].Trim();
                string localMd5 = GlobalTools.Md5File(localfile);
                canUpdate = !remoteMd5.Equals(localMd5.Split('|')[0]);
                if (canUpdate) File.Delete(localfile);
            }
            if (canUpdate)
            {   //本地缺少文件
                Debug.Log(fileUrl);
                message = "downloading>>" + fileUrl;
                //facade.SendMessageCommand(NotiConst.UPDATE_MESSAGE, message);

                //这里都是资源文件，用线程下载
                BeginDownload(fileUrl, localfile);
                while (!(IsDownOK(localfile))) { yield return new WaitForEndOfFrame(); }
            }
        }
       
        yield return new WaitForSeconds(2f);
    }

    void OnUpdateFailed(string file)
    {
        Debug.Log("更新失败!!");
    }

    /// <summary>
    /// 是否下载完成
    /// </summary>
    bool IsDownOK(string file)
    {
        return DownloadFiles.Contains(file);
    }

    /// <summary>
    /// 线程下载
    /// </summary>
    void BeginDownload(string url, string file)
    {     //线程下载
        object[] param = new object[2] { url, file };

        ThreadEvent ev = new ThreadEvent();
        ev.Key = MessageTypes.UPDATE_DOWNLOAD;
        ev.evParams.AddRange(param);
        ThreadManager.Instance.AddEvent(ev, OnThreadCompleted);   //线程下载
    }

    /// <summary>
    /// 线程完成
    /// </summary>
    /// <param name="data"></param>
    /// 
    void OnThreadCompleted(NotiData data)
    {
        switch (data.evName)
        {
            case MessageTypes.UPDATE_EXTRACT:  //解压一个完成
                
                Debug.Log(data.evParam.ToString());
                break;
            case MessageTypes.UPDATE_DOWNLOAD: //下载一个完成
                DownloadFiles.Add(data.evParam.ToString());
                Debug.Log(data.evParam.ToString());
                break;
        }
    }
    public IEnumerator WaitFrame(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }
}
