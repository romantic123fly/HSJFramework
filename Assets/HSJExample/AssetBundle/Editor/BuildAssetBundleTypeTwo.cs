using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;

public class BuildAssetBundleTypeTwo
{
    //设置所有美术资源的ab名字并打包
    [MenuItem("HSJ/打AB包/方式二")]
    public static  void BuildiPhoneResource()
    {
        BuildTarget target;
        target = BuildTarget.iOS;
        BuildAssetResource(EditorUserBuildSettings.activeBuildTarget);
    }

    /// <summary>
    /// 生成绑定素材
    /// </summary>
    public static void BuildAssetResource(BuildTarget target)
    {
        try
        {
            SetAssetbundleName();
            string assetfile = string.Empty;  //素材文件名
            string resPath = Application.streamingAssetsPath + "/";

            if (!Directory.Exists(resPath)) Directory.CreateDirectory(resPath);
            var a = BuildPipeline.BuildAssetBundles(resPath, BuildAssetBundleOptions.None, target);
            EditorUtility.ClearProgressBar();

            // ----------------------创建文件列表---------------------- -
            string newFilePath = resPath + "/files.txt";
            if (File.Exists(newFilePath)) File.Delete(newFilePath);
            List<string> files = new List<string>();
            Recursive(resPath, files);

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
    //设置ab名字
    public static void SetAssetbundleName()
    {
        string[] abPath = new string[] { "Prefabs", "Materials", "Model", "Shaders", "Texture" };
        string rootPath = Application.dataPath + "/HSJExample/AssetBundle/";
        string abName = string.Empty;
        List<string> filePaths = new List<string>();
        foreach (var item in abPath)
        {
            Recursive(rootPath + item, filePaths);
        }

        foreach (var filePath in filePaths)
        {
            string assetPath = "Assets" + filePath.Replace(Application.dataPath, "");
            abName = filePath.Replace(rootPath, "").ToLower();

            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            if (assetImporter == null)
            {
                UnityEngine.Debug.LogError("不存在此路径文件：" + assetPath);
            }
            else
            {
                assetImporter.assetBundleName = abName.Split('.')[0]+".assetbundle";
                Debug.Log(assetImporter.assetBundleName);
            }
        }
    }
    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path, List<string> files)
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
            Recursive(dir, files);
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
