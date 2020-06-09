using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Text;

public class BaseStorage : GameMonoBehaviour
{
    //数据缓存
    protected Dictionary<int, object> _data;
    //文件名
    protected string _fileName;
    //数据类型
    protected string _dataType;

    protected XmlDocument _doc;

    protected bool _isLoaded;

    protected string _name;

    protected override void Awake()
    {
        base.Awake();

        _isLoaded = false;
        _data = new Dictionary<int, object>();
    }

    public virtual void init()
    {
        StartCoroutine(loadSync());
    }

    protected virtual void load()
    {
        var data = ResourceManager.load(GameDefine.DATA_PATH + _fileName).ToString();

        parseData(data);
    }

    IEnumerator loadSync()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        string filePath = Application.dataPath + "/../../../Assets/Resources/" + GameDefine.DATA_PATH + _fileName + ".xml";
        //string filePath = Application.streamingAssetsPath + "/" + GameDefine.DATA_PATH + _fileName + ".xml";
        //Debug.Log(filePath);
#else
        string filePath = ResourceManager.getDataPath(GameDefine.DATA_PATH + _fileName + ".xml");
#endif
        var result = "";
        if (filePath.Contains("://"))
        {
            //1.加载OS本地的路径签名要加file://
            WWW www = new WWW(filePath);
            yield return www;

            if (www.isDone && www.error == null)
            {
                result = www.text;
            }
        }
        else
        {
            result = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
        }

        parseData(result);
    }

    protected virtual void parseData(string data)
    {
        _doc = new XmlDocument();
        _doc.LoadXml(data);
        
        XmlNodeList xmlNodeList = _doc.GetElementsByTagName(_dataType);
        foreach (XmlElement element in xmlNodeList)
        {
            parseElement(element);
        }

        _isLoaded = true;
        Debug.Log(_name + " 解析完成");
    }

    protected virtual void parseElement(XmlElement element)
    {
    }

    public virtual Dictionary<int, object> data
    {
        get
        {
            return _data;
        }
    }

    public bool isLoaded
    {
        get
        {
            return _isLoaded;
        }
        set
        {
            _isLoaded = value;
        }
    }
}
