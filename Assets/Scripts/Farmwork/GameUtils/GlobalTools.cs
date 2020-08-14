#region 模块信息
// **********************************************************************
// Copyright (C) 2020 Loooto
// Please contact me if you have any questions
// File Name:             GlobalTools
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
#region 模块信息
// **********************************************************************
// Copyright (C) 2019 
// Please contact me if you have any questions
// File Name:             GlobalTools
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class GlobalTools
{
    #region 系统工具

    // 获取本机Mac地址
    public static string GetMacAddress()
    {
        try
        {
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < nis.Length; i++)
            {
                if (nis[i].Name == "本地连接")
                {
                    return nis[i].GetPhysicalAddress().ToString();
                }
            }
            return "null";
        }
        catch
        {
            return "null";
        }
    }

    //获取本机Ip地址
    public static string GetLocalIP()
    {
        //Dns.GetHostName()获取本机名Dns.GetHostAddresses()根据本机名获取ip地址组
        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();  //ipv4
            }
        }
        return null;
    }
    // MD5  32位加密
    public static string UserMd5(string str)
    {
        string cl = str;
        StringBuilder pwd = new StringBuilder();
        MD5 md5 = MD5.Create();//实例化一个md5对像
                               // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
            pwd.Append(s[i].ToString("x2"));
            //pwd = pwd + s[i].ToString("X");
        }
        return pwd.ToString();
    }
    // 计算字符串的MD5值
    public static string Md5String(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = "";
        for (int i = 0; i < md5Data.Length; i++)
        {
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        destString = destString.PadLeft(32, '0');
        return destString;
    }
    // 计算文件的MD5值
    public static string Md5File(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            string size = fs.Length / 1024 + "";
            Debug.Log("当前文件的大小：  " + file + "===>" + (fs.Length / 1024) + "KB");
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb + "|" + size;
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

    // 字节加密/解密 异或处理
    public static void Encypt(ref byte[] targetData, byte m_key)
    {
        //加密，与key异或，解密的时候同样如此
        int dataLength = targetData.Length;
        for (int i = 0; i < dataLength; ++i)
        {
            targetData[i] = (byte)(targetData[i] ^ m_key);
        }
    }
    //获取系统时间
    public static long GetNowTime()
    {
        TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
        return (long)ts.TotalMilliseconds;
    }

    // Base64编码
    public static string Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }

    // Base64解码
    public static string Decode(string message)
    {
        byte[] bytes = Convert.FromBase64String(message);
        return Encoding.GetEncoding("utf-8").GetString(bytes);
    }

    //生成随机字符串
    public static string RandomCharacters()
    {
        string str = string.Empty;
        long num2 = DateTime.Now.Ticks + 0;
        System.Random random = new System.Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> 1)));
        for (int i = 0; i < 20; i++)
        {
            char ch;
            int num = random.Next();
            if ((num % 2) == 0)
            {
                ch = (char)(0x30 + ((ushort)(num % 10)));
            }
            else
            {
                ch = (char)(0x41 + ((ushort)(num % 0x1a)));
            }
            str = str + ch.ToString();
        }
        return str;
    }
    #endregion

    #region 延时执行
    /// <summary>
    /// 延时执行
    /// </summary>
    /// <param name="action">执行的代码</param>
    /// <param name="delaySeconds">延时的秒数</param>
    public static Coroutine DelayExecute(this MonoBehaviour behaviour, Action action, float delaySeconds)
    {
        Coroutine coroutine = behaviour.StartCoroutine(DelayExecute(action, delaySeconds));
        return coroutine;
    }
    private static IEnumerator DelayExecute(Action action, float delaySeconds)
    {
        yield return YieldInstructioner.GetWaitForSeconds(delaySeconds);
        action();
    }

    /// <summary>
    /// 等待执行
    /// </summary>
    /// <param name="action">执行的代码</param>
    /// <param name="waitUntil">等待的WaitUntil,返回True</param>
    public static Coroutine WaitExecute(this MonoBehaviour behaviour, Action action, WaitUntil waitUntil)
    {
        Coroutine coroutine = behaviour.StartCoroutine(WaitExecute(action, waitUntil));
        return coroutine;
    }
    private static IEnumerator WaitExecute(Action action, WaitUntil waitUntil)
    {
        yield return waitUntil;
        action();
    }
    /// <param name="waitUntil">等待的waitWhile,返回false</param>
    public static Coroutine WaitExecute(this MonoBehaviour behaviour, Action action, WaitWhile waitWhile)
    {
        Coroutine coroutine = behaviour.StartCoroutine(WaitExecute(action, waitWhile));
        return coroutine;
    }
    private static IEnumerator WaitExecute(Action action, WaitWhile waitWhile)
    {
        yield return waitWhile;
        action();
    }
    #endregion

    #region UI事件/组件
    /// <summary>
    /// UGUI 控件添加事件监听
    /// </summary>
    /// <param name="target">事件监听目标</param>
    /// <param name="type">事件类型</param>
    /// <param name="callback">回调函数</param>
    public static void AddEventListener(this RectTransform target, EventTriggerType type, UnityAction<BaseEventData> callback)
    {
        EventTrigger trigger = target.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = target.gameObject.AddComponent<EventTrigger>();
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        //定义一个事件
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //设置事件类型
        entry.eventID = type;
        //设置事件回调函数
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(callback);
        //添加事件到事件组
        trigger.triggers.Add(entry);
    }

    // UGUI Button添加点击事件监听
    public static void AddEventListener(this RectTransform target, UnityAction callback)
    {
        Button button = target.GetComponent<Button>();
        if (button)
        {
            button.onClick.AddListener(callback);
        }
        else
        {
            Debug.Log(target.name + " 丢失了组件 Button！");
        }
    }
    // UGUI Button移除所有事件监听
    public static void RemoveAllEventListener(this RectTransform target)
    {
        Button button = target.GetComponent<Button>();
        if (button)
        {
            button.onClick.RemoveAllListeners();
        }
        else
        {
            Debug.Log(target.name + " 丢失了组件 Button！");
        }
    }

    // 当前鼠标是否停留在UGUI控件上
    public static bool IsPointerOverUGUI()
    {
        if (EventSystem.current)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        else
        {
            return false;
        }
    }

    // 限制Text内容的长度在length以内，超过的部分用replace代替
    public static void RestrictLength(this Text tex, int length, string replace)
    {
        if (tex.text.Length > length)
        {
            tex.text = tex.text.Substring(0, length) + replace;
        }
    }
    // 限制Text中指定子字串的字体大小

    public static void ToRichSize(this Text tex, string subStr, int size)
    {
        if (subStr.Length <= 0 || !tex.text.Contains(subStr))
        {
            return;
        }

        string valueRich = tex.text;
        int index = valueRich.IndexOf(subStr);
        if (index >= 0) valueRich = valueRich.Insert(index, "<size=" + size + ">");
        else return;

        index = valueRich.IndexOf(subStr) + subStr.Length;
        if (index >= 0) valueRich = valueRich.Insert(index, "</size>");
        else return;

        tex.text = valueRich;
    }

    // 限制Text中指定子字串的字体颜色
    public static void ToRichColor(this Text tex, string subStr, Color color)
    {
        if (subStr.Length <= 0 || !tex.text.Contains(subStr))
        {
            return;
        }

        string valueRich = tex.text;
        int index = valueRich.IndexOf(subStr);
        if (index >= 0) valueRich = valueRich.Insert(index, "<color=" + color.ToHexSystemString() + ">");
        else return;

        index = valueRich.IndexOf(subStr) + subStr.Length;
        if (index >= 0) valueRich = valueRich.Insert(index, "</color>");
        else return;

        tex.text = valueRich;
    }

    // 限制Text中的指定子字串的字体加粗
    public static void ToRichBold(this Text tex, string subStr)
    {
        if (subStr.Length <= 0 || !tex.text.Contains(subStr))
        {
            return;
        }

        string valueRich = tex.text;
        int index = valueRich.IndexOf(subStr);
        if (index >= 0) valueRich = valueRich.Insert(index, "<b>");
        else return;

        index = valueRich.IndexOf(subStr) + subStr.Length;
        if (index >= 0) valueRich = valueRich.Insert(index, "</b>");
        else return;

        tex.text = valueRich;
    }

    // 限制Text中的指定子字串的字体斜体
    public static void ToRichItalic(this Text tex, string subStr)
    {
        if (subStr.Length <= 0 || !tex.text.Contains(subStr))
        {
            return;
        }

        string valueRich = tex.text;
        int index = valueRich.IndexOf(subStr);
        if (index >= 0) valueRich = valueRich.Insert(index, "<i>");
        else return;

        index = valueRich.IndexOf(subStr) + subStr.Length;
        if (index >= 0) valueRich = valueRich.Insert(index, "</i>");
        else return;

        tex.text = valueRich;
    }

    // 清除所有富文本样式
    public static void ClearRich(this Text tex)
    {
        string value = tex.text;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == '<')
            {
                for (int j = i + 1; j < value.Length; j++)
                {
                    if (value[j] == '>')
                    {
                        int count = j - i + 1;
                        value = value.Remove(i, count);
                        i -= 1;
                        break;
                    }
                }
            }
        }
        tex.text = value;
    }

    // 获取颜色RGB参数的十六进制字符串
    public static string ToHexSystemString(this Color color)
    {
        return "#" + ((int)(color.r * 255)).ToString("x2") +
            ((int)(color.g * 255)).ToString("x2") +
            ((int)(color.b * 255)).ToString("x2") +
            ((int)(color.a * 255)).ToString("x2");
    }
    #endregion

    #region 坐标系坐标转换
    public enum UIType
    {
        /// <summary>
        /// 屏幕UI
        /// </summary>
        Overlay,
        /// <summary>
        /// 摄像机UI
        /// </summary>
        Camera,
        /// <summary>
        /// 世界UI
        /// </summary>
        World
    }
    /// <summary>
    /// 世界坐标转换为UGUI坐标（只针对框架UI模块下的UI控件）
    /// </summary>
    /// <param name="position">世界坐标</param>
    /// <param name="reference">参照物（要赋值的UGUI控件的根物体）</param>
    /// <param name="uIType">UI类型</param>
    /// <returns>基于参照物的局部UGUI坐标</returns>
    public static Vector2 WorldToUGUIPosition(this Vector3 position, RectTransform reference = null, UIType uIType = UIType.Overlay)
    {

        Vector3 screenPos;
        Vector2 anchoredPos = Vector2.zero;
        switch (uIType)
        {
            case UIType.Overlay:
                screenPos = Camera.main.WorldToScreenPoint(position);
                screenPos.z = 0;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(reference, screenPos, null, out anchoredPos);
                break;
            case UIType.Camera:
                screenPos = Camera.main.WorldToScreenPoint(position);
                screenPos.z = 0;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(reference, screenPos, Camera.main, out anchoredPos);
                break;
            case UIType.World:
                break;
        }
        return anchoredPos;
    }
    /// <summary>
    /// 屏幕坐标转换为UGUI坐标（只针对框架UI模块下的UI控件）
    /// </summary>
    /// <param name="position">屏幕坐标</param>
    /// <param name="reference">参照物（要赋值的UGUI控件的根物体）</param>
    /// <param name="uIType">UI类型</param>
    /// <returns>基于参照物的局部UGUI坐标</returns>
    public static Vector2 ScreenToUGUIPosition(this Vector3 position, RectTransform reference = null, UIType uIType = UIType.Overlay)
    {
        Vector2 anchoredPos = Vector2.zero;
        switch (uIType)
        {
            case UIType.Overlay:
                position.z = 0;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(reference, position, null, out anchoredPos);
                break;
            case UIType.Camera:
                position.z = 0;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(reference, position, Camera.main, out anchoredPos);
                break;
            case UIType.World:
                break;
        }
        return anchoredPos;
    }



    #endregion

    #region 查询添加子物体
    //查找物体的方法
    public static Transform FindTheChild(GameObject goParent, string childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }
    //获取子物体的脚本
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        if (searchTrans != null)
        {
            return searchTrans.gameObject.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    // 查找兄弟
    public static GameObject FindBrother(this GameObject obj, string name)
    {
        GameObject gObject = null;
        if (obj.transform.parent)
        {
            Transform tf = obj.transform.parent.Find(name);
            gObject = tf ? tf.gameObject : null;
        }
        else
        {
            GameObject[] rootObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootObj in rootObjs)
            {
                if (rootObj.name == name)
                {
                    gObject = rootObj;
                    break;
                }
            }
        }
        return gObject;
    }

    /// <summary>
    /// 通过组件查找场景中所有的物体，包括隐藏和激活的
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="Result">组件列表</param>
    public static void FindObjectsOfType<T>(List<T> Result) where T : Component
    {
        if (Result == null) Result = new List<T>();
        else Result.Clear();

        List<T> sub = new List<T>();
        GameObject[] rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject rootObj in rootObjs)
        {
            rootObj.transform.GetComponentsInChildren(true, sub);
            Result.AddRange(sub);
        }
    }
    //通过Tag标签查找场景中指定物
    public static GameObject FindObjectByTag(string tagName)
    {
        return GameObject.FindGameObjectWithTag(tagName);
    }

    //添加子物体
    public static void AddChildToParent(Transform parentTrs, Transform childTrs)
    {
        childTrs.SetParent(parentTrs);
        childTrs.localPosition = Vector3.zero;
        childTrs.localScale = Vector3.one;
        childTrs.localEulerAngles = Vector3.zero;
        SetLayer(parentTrs.gameObject.layer, childTrs);
    }

    //设置所有子物体的Layer
    public static void SetLayer(int parentLayer, Transform childTrs)
    {
        childTrs.gameObject.layer = parentLayer;
        for (int i = 0; i < childTrs.childCount; i++)
        {
            Transform child = childTrs.GetChild(i);
            child.gameObject.layer = parentLayer;
            SetLayer(parentLayer, child);
        }
    }

    #endregion

    #region 文件操作
    //写进去
    public static void WriteFile(string path, string name, string info)
    {
        StreamWriter sw;
        FileInfo fi = new FileInfo(path + "/" + name);
        sw = fi.CreateText();
        sw.WriteLine(info);
        sw.Close();
        sw.Dispose();
    }
    //读出来
    public static string ReadFile(string path, string name)
    {
        StreamReader sr;
        FileInfo fi = new FileInfo(path + "/" + name);
        sr = fi.OpenText();
        string info = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();
        return info;
    }
    #endregion

    #region 本地/网络图片加载

    // 加载外部图片
    public static Sprite LoadSprite(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, (int)stream.Length);

        Texture2D tex = new Texture2D(80, 80);
        tex.LoadImage(buffer);
        stream.Close();
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
    }
    //读取本地图片转byte[]
    public static byte[] ReadTexture(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        byte[] buffer = new byte[fileStream.Length]; //创建文件长度的buffer   
        fileStream.Read(buffer, 0, (int)fileStream.Length);

        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        return buffer;
    }



    #endregion

    #region 本地持久化储存
    //封装PlayerPrefs
    public static bool HasKey(string keyName)
    {
        return PlayerPrefs.HasKey(keyName);
    }
    public static int GetInt(string keyName)
    {
        return PlayerPrefs.GetInt(keyName);
    }
    public static void SetInt(string keyName, int valueInt)
    {
        PlayerPrefs.SetInt(keyName, valueInt);
    }

    public static float GetFloat(string keyName)
    {
        return PlayerPrefs.GetFloat(keyName);
    }
    public static void SetFloat(string keyName, float valueFloat)
    {
        PlayerPrefs.SetFloat(keyName, valueFloat);
    }

    public static string GetString(string keyName)
    {
        return PlayerPrefs.GetString(keyName);
    }
    public static void SetString(string keyName, string valueString)
    {
        PlayerPrefs.SetString(keyName, valueString);
    }

    public static void SetBool(string keyName, bool value)
    {
        PlayerPrefs.SetString(keyName, value.ToString());
    }
    public static bool GetBool(string keyName)
    {
        return bool.Parse(PlayerPrefs.GetString(keyName + "Bool"));
    }

    public static void SetIntArray(string keyName, int[] value)
    {

        for (int i = 0; i < value.Length; i++)
        {
            PlayerPrefs.SetInt(keyName + "IntArray_" + i, value[i]);
        }
        PlayerPrefs.SetInt(keyName + "IntArray", value.Length);
    }
    public static int[] GetIntArray(string keyName)
    {
        int[] intArr = new int[1];
        if (PlayerPrefs.GetInt(keyName + "IntArray") != 0)
        {
            intArr = new int[PlayerPrefs.GetInt(keyName + "IntArray")];
            for (int i = 0; i < intArr.Length; i++)
            {
                intArr[i] = PlayerPrefs.GetInt(keyName + "IntArray_" + i);
            }
        }
        return intArr;
    }

    public static void SetVector3(string keyName, Vector3 value)
    {
        PlayerPrefs.SetFloat(keyName + "Vector3X", value.x);
        PlayerPrefs.SetFloat(keyName + "Vector3Y", value.y);
        PlayerPrefs.SetFloat(keyName + "Vector3Z", value.z);
    }
    public static Vector3 GetVector3(string keyName)
    {
        Vector3 vector3;
        vector3.x = PlayerPrefs.GetFloat(keyName + "Vector3X");
        vector3.y = PlayerPrefs.GetFloat(keyName + "Vector3Y");
        vector3.z = PlayerPrefs.GetFloat(keyName + "Vector3Z");
        return vector3;
    }

    public static void DeleteAllKey()
    {
        PlayerPrefs.DeleteAll();
    }
    public static void DeleteTheKey(string keyNmae)
    {
        PlayerPrefs.DeleteKey(keyNmae);
    }

    #endregion

    #region 反射工具
    /// <summary>
    /// 当前的运行时程序集
    /// </summary>
    private static readonly HashSet<string> RunTimeAssemblies = new HashSet<string>() {
            "Assembly-CSharp", "HTFramework.RunTime", "HTFramework.AI.RunTime", "HTFramework.Auxiliary.RunTime",
            "UnityEngine", "UnityEngine.CoreModule", "UnityEngine.UI", "UnityEngine.PhysicsModule" };
    /// <summary>
    /// 从当前程序域的运行时程序集中获取所有类型
    /// </summary>
    public static List<Type> GetTypesInRunTimeAssemblies()
    {
        List<Type> types = new List<Type>();
        Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblys.Length; i++)
        {
            if (RunTimeAssemblies.Contains(assemblys[i].GetName().Name))
            {
                types.AddRange(assemblys[i].GetTypes());
            }
        }
        return types;
    }
    /// <summary>
    /// 从当前程序域的运行时程序集中获取指定类型
    /// </summary>
    public static Type GetTypeInRunTimeAssemblies(string typeName)
    {
        Type type = null;
        foreach (string assembly in RunTimeAssemblies)
        {
            type = Type.GetType(typeName + "," + assembly);
            if (type != null)
            {
                return type;
            }
        }
        Debug.Log("获取类型 " + typeName + " 失败！当前运行时程序集中不存在此类型！");
        return null;
    }
    #endregion

    #region 解析excel,csv
    public static Dictionary<string, Dictionary<string, string>> LoadExcelFormTxt(string cfgPath, string cfgName)
    {
        TextAsset tw = Resources.Load<TextAsset>(cfgPath);
        string strLine = tw.text;
        //开始解析字符串
        return ExplainString(strLine);
    }
    public static Dictionary<string, Dictionary<string, string>> ExplainString(string strLine)
    {
        Dictionary<string, string[]> content = new Dictionary<string, string[]>();
        string[] lineArray = strLine.Replace("\r\n", "*").Split(new char[] { '*' });
        //获取行数
        int rows = lineArray.Length - 1;
        //获取列数
        int Columns = lineArray[0].Split(new char[] { '\t' }).Length;
        //定义一个数组用于存放字段名
        string[] ColumnName = new string[Columns];
        for (int i = 0; i < rows; i++)
        {
            string[] Array = lineArray[i].Split(new char[] { '\t' });
            for (int j = 0; j < Columns; j++)
            {
                //获取Array的列的值
                string nvalue = Array[j].Trim();
                if (i == 0)
                {
                    //存储字段名
                    ColumnName[j] = nvalue;
                    content[ColumnName[j]] = new string[rows - 1];
                }
                else
                {
                    //存储对应字段的默认值//<字段名，默认值>
                    content[ColumnName[j]][i - 1] = nvalue;
                }
            }
        }
        //开始解析
        return ExplainDictionary(content);
    }
    public static Dictionary<string, Dictionary<string, string>> ExplainDictionary(Dictionary<string, string[]> content)
    {
        Dictionary<string, Dictionary<string, string>> DicContent = new Dictionary<string, Dictionary<string, string>>();
        //获取字典中所有的键(字段名)
        Dictionary<string, string[]>.KeyCollection Allkeys = content.Keys;
        //遍历所有的字段名
        foreach (string key in Allkeys)
        {
            //实例化一个hasData的字典//<ID,字段值>
            Dictionary<string, string> hasData = new Dictionary<string, string>();
            string[] Data = content[key];
            for (int i = 0; i < Data.Length; i++)
            {
                //<ID><字段值>
                hasData[content["ID"][i]] = Data[i];
            }
            DicContent[key] = hasData;
        }
        return DicContent;
    }


    #endregion

    #region 数学工具

    /**加*/
    //rate:几率数组（%），  total：几率总和（100%）
    // Debug.Log(rand(new int[] { 10, 5, 15, 20, 30, 5, 5,10 }, 100));
    public static int rand(int[] rate, int total)
    {
        int r = UnityEngine.Random.Range(1, total + 1);
        int t = 0;
        for (int i = 0; i < rate.Length; i++)
        {
            t += rate[i];
            if (r < t)
            {
                return i;
            }
        }
        return 0;
    }
    /**减*/
    //rate:几率数组（%），  total：几率总和（100%）
    // Debug.Log(randRate(new int[] { 10, 5, 15, 20, 30, 5, 5,10 }, 100));
    public static int randRate(int[] rate, int total)
    {
        int rand = UnityEngine.Random.Range(0, total + 1);
        for (int i = 0; i < rate.Length; i++)
        {
            rand -= rate[i];
            if (rand <= 0)
            {
                return i;
            }
        }
        return 0;
    }


    //获取指定范围内的随机整数
    public static int GetRandomInt(int num1, int num2)
    {
        if (num1 < num2)
        {
            return UnityEngine.Random.Range(num1, num2);
        }
        else
        {
            return UnityEngine.Random.Range(num2, num1);
        }
    }
    //是否约等于另一个浮点数
    public static bool Approximately(this float sourceValue, float targetValue)
    {
        return Mathf.Approximately(sourceValue, targetValue);
    }
    //限制value的值在min和max之间,如果value小于min，返回min。 如果value大于max，返回max，否则返回value
    public static float Clamp(float value, float minValue, float MaxValue)
    {
        return Mathf.Clamp(value, minValue, MaxValue);
    }
    //返回两个或更多值中最大的值。
    public static float Max(float a, float b)
    {
        return Mathf.Max(a, b);
    }
    //返回两个或更多值中最小的值。
    public static float Min(float a, float b)
    {
        return Mathf.Min(a, b);
    }
    //返回两个插值。基于浮点数t返回a到b之间的插值，t限制在0～1之间。当t = 0返回from，当t = 1 返回to。当t = 0.5 返回from和to的平均值。
    public static float Lerp(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, t);
    }

    #endregion

}
public static class YieldInstructioner
{
    private static WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    private static WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private static Dictionary<string, WaitForSeconds> _waitForSeconds = new Dictionary<string, WaitForSeconds>();
    private static Dictionary<string, WaitForSecondsRealtime> _waitForSecondsRealtime = new Dictionary<string, WaitForSecondsRealtime>();

    public static WaitForEndOfFrame GetWaitForEndOfFrame()
    {
        return _waitForEndOfFrame;
    }
    public static WaitForFixedUpdate GetWaitForFixedUpdate()
    {
        return _waitForFixedUpdate;
    }
    public static WaitForSeconds GetWaitForSeconds(float second)
    {
        string secondStr = second.ToString("F2");
        if (!_waitForSeconds.ContainsKey(secondStr))
        {
            _waitForSeconds.Add(secondStr, new WaitForSeconds(second));
        }
        return _waitForSeconds[secondStr];
    }
    public static WaitForSecondsRealtime GetWaitForSecondsRealtime(float second)
    {
        string secondStr = second.ToString("F2");
        if (!_waitForSecondsRealtime.ContainsKey(secondStr))
        {
            _waitForSecondsRealtime.Add(secondStr, new WaitForSecondsRealtime(second));
        }
        return _waitForSecondsRealtime[secondStr];
    }
    public static WaitUntil GetWaitUntil(Func<bool> predicate)
    {
        return new WaitUntil(predicate);
    }
    public static WaitWhile GetWaitWhile(Func<bool> predicate)
    {
        return new WaitWhile(predicate);
    }
}