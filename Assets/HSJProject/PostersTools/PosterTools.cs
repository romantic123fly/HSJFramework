#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             PosterTools
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Application;
using Button = UnityEngine.UI.Button;

public class PosterTools : MonoBehaviour
{
    public Image mainBg;
    public Image logo;
    public Image qRCode;
    public Text phoneNum;
    public Text address;
    public Text content;
    public InputField contentInput;
    public Button selectBg;
    public Button screenShot;
    public GameObject operationView;

    private Texture2D img = null;
    private Sprite sprite;
    // Use this for initialization
    void Start()
    {
       
        selectBg.onClick.AddListener(SelectMainBg);
        screenShot.onClick.AddListener(() => { StartCoroutine(CreatePosters()); });
        if (PostersManager.GetInstance().theCurrentFestival == "营销海报")
        {
            contentInput.gameObject.SetActive(true);
        }
    }

    public void SelectMainBg()
    {
        try
        {
            Debug.Log(0);
            OpenFileDialog od = new OpenFileDialog();
            od.Title = "请选择海报背景";
            od.Multiselect = false;
            od.Filter = "图片文件(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
            if (od.ShowDialog() == DialogResult.OK)
            {
                Debug.Log(1);
                //Debug.Log(od.FileName);
                StartCoroutine(GetTexture("file://" + od.FileName, mainBg));
            }
        }
        catch (Exception e)
        {

            Debug.Log(e.Message);
        }
    }
    IEnumerator GetTexture(string url,Image image)
    {
        Debug.Log(2);
        WWW www = new WWW(url);
        Debug.Log(url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            img = www.texture;
            sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
            //Debug.Log(img.width+" "+img.height);
            byte[] date = img.EncodeToPNG();
        }
        else
        {
            Debug.Log(www.error);
        }
    }
    private void SetImage(string path,Image image)
    {
        DirectoryInfo root = new DirectoryInfo(path);
        if (root.GetFiles().Length > 0)
        {
            string fileName = root.GetFiles()[0].Name;
            if (fileName.ToUpper().EndsWith(".JPG") || fileName.ToUpper().EndsWith(".PNG"))
            {
                path += fileName;
            }
        }
        else
        {
            Debug.LogError(path.Split('/')[5] + "--" + path.Split('/')[6] + "-未设置");
            return;
        }
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        //释放本机屏幕资源
        fileStream.Dispose();
        fileStream = null;
        img = new Texture2D(200,100);
        img.LoadImage(bytes);
        //创建Sprite
        Sprite sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
    }
    //生成海报
    IEnumerator CreatePosters() {
        foreach (var item in PostersManager.GetInstance().outpatientInfoList)
        {
            string logoPath = Application.streamingAssetsPath + "/门诊信息/" + item.name + "/Logo/";
            string QrCodePath = Application.streamingAssetsPath + "/门诊信息/" + item.name + "/二维码/";
            SetImage(logoPath, logo);
            SetImage(QrCodePath, qRCode);
            address.text = item.address;
            phoneNum.text = "联系方式： " + item.phoneNum;
            if (PostersManager.GetInstance().theCurrentFestival == "营销海报")
            {
                if (contentInput.text!="")
                {
                    content.text = contentInput.text;
                }
                else
                {
                    content.text = item.content;
                }
            }
            ScreenShot(item.name);
            yield return new WaitUntil(() => true);
        }
        operationView.gameObject.SetActive(true);
    }
    
    public void ScreenShot(string outpatientName)
    {
        System.DateTime now = System.DateTime.Now;
        string times = now.ToString();
        times = times.Trim();
        times = times.Replace("/", "-");

        operationView.gameObject.SetActive(false);
 
        string pathSave = PostersManager.GetInstance().GetPosterPath(outpatientName)+"/1.png";
      
        StartCoroutine(SaveImage(pathSave));
    }


    IEnumerator SaveImage(string path)
    {
        ScreenCapture.CaptureScreenshot(path, 5);
        yield return new WaitUntil(() => true);
        //yield return new WaitForSeconds(1);
        Debug.Log(path.Split('/')[5]+"--海报生成成功！！");
    }


    private void Update()
    {
       
        if (PostersManager.GetInstance().theCurrentFestival =="营销海报")
        {
            if (contentInput.text!="")
            {
                content.text = contentInput.text;
            }
        }
    }
}
