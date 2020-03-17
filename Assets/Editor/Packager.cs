using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Diagnostics;

public class Packager
{
    public static string platform = string.Empty;
    static List<string> paths = new List<string>();
    static List<string> files = new List<string>();

    [MenuItem("HSJ/EditorTools/BuildAB/IOSResource", false, 1)]
    public static void BuildiPhoneResource()
    {
        BuildTarget target;
        target = BuildTarget.iOS;
        BuildAssetResource(target, false);
    }

    [MenuItem("HSJ/EditorTools/BuildAB/AndroidResource",false ,2)]
    public static void BuildAndroidResource()
    {
        BuildAssetResource(BuildTarget.Android, true);
    }

    [MenuItem("HSJ/EditorTools/BuildAB/WindowsResource", false, 3)]
    public static void BuildWindowsResource()
    {
        BuildAssetResource(BuildTarget.StandaloneWindows, true);
    }

    /// <summary>
    /// 生成绑定素材
    /// </summary>
    public static void BuildAssetResource(BuildTarget target, bool isWin)
    {
        try
        {
            string assetfile = string.Empty;  //素材文件名
            string resPath = Application.streamingAssetsPath + "/";

            if (!Directory.Exists(resPath)) Directory.CreateDirectory(resPath);
            var a = BuildPipeline.BuildAssetBundles(resPath, BuildAssetBundleOptions.None, target);
            EditorUtility.ClearProgressBar();

            // ----------------------创建文件列表---------------------- -
            string newFilePath = resPath + "/files.txt";
            if (File.Exists(newFilePath)) File.Delete(newFilePath);

            paths.Clear(); files.Clear();
            Recursive(resPath);

            FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);
            for (int i = 0; i < files.Count; i++)
            {
                string filePath = files[i];
                string ext = Path.GetExtension(filePath);
                if (filePath.EndsWith(".meta") || filePath.Contains(".DS_Store")) continue;
                string md5 = md5file(filePath);
                string value = filePath.Replace(resPath, string.Empty);
                sw.WriteLine(value + "|" + md5);

                #region   //-----------加密---------------
                byte[] temp = File.ReadAllBytes(filePath);
                //Encypt(ref temp, 188);
                File.ReadAllBytes(filePath);
                File.WriteAllBytes(filePath, temp);
                #endregion
            }
            sw.Close(); fs.Close();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path)
    {
        //获取多个类型格式的文件
        string[] names = Directory.GetFiles(path);
        //要搜索的目录的相对或绝对路径。此字符串不区分大小写。
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }
    public static string DataPath
    {
        get
        {
            if (Application.isMobilePlatform)
            {
                return Application.persistentDataPath + "/测试/";
            }
            return "c:/测试/";
        }
    }
    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            string size = fs.Length / 1024 + "";
            UnityEngine.Debug.Log("当前文件的大小：  " + file + "===>" + (fs.Length / 1024) + "KB");
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
    /// <summary>
    /// 加密/解密
    /// </summary>
    /// <param name="targetData">文件流</param>
    public static void Encypt(ref byte[] targetData, byte m_key)
    {
        //加密，与key异或，解密的时候同样如此
        int dataLength = targetData.Length;
        for (int i = 0; i < dataLength; ++i)
        {
            targetData[i] = (byte)(targetData[i] ^ m_key);
        }
    }
}
