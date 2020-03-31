#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             PosterTools
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Application;
using Button = UnityEngine.UI.Button;
using Screen = UnityEngine.Screen;

public class PosterTools : MonoBehaviour
{
    public Image mainBg;
    public Image logo;
    public Text content;
    public Text address;
    public Button selectBg;
    public Button selectLogo;
    public Button screenShot;
    public Button textColor;
    public GameObject operationView;

    private Texture2D img = null;
    private Sprite sprite;
    // Use this for initialization
    void Start()
    {
        Debug.Log(Screen.width+"/" + Screen.height);
        //canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1080*a, Screen.height);
        UnityEngine.Screen.SetResolution(607, Screen.height, false);
        Debug.Log(Screen.width + "/" + Screen.height);
        selectBg.onClick.AddListener(SelectBg);
        selectLogo.onClick.AddListener(SelectLogo);
        screenShot.onClick.AddListener(ScreenShot);

        PostersManager.GetInstance().textList.Add(content);
        PostersManager.GetInstance().textList.Add(address);
    }

    public void SelectBg()
    {
        OpenFileDialog od = new OpenFileDialog();
        od.Title = "请选择海报背景";
        od.Multiselect = false;
        od.Filter = "图片文件(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
        if (od.ShowDialog() == DialogResult.OK)
        {
            //Debug.Log(od.FileName);
            StartCoroutine(GetTexture("file://" + od.FileName,mainBg));
        }
        /*if (img != null) {
        //GUI.DrawTexture(new Rect(0,20,img.width,img.height),img);
        image.sprite=sprite;
        }*/
    }
    private void SelectLogo()
    {
        OpenFileDialog od = new OpenFileDialog();
        od.Title = "请选择Logo图片";
        od.Multiselect = false;
        od.Filter = "图片文件(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
        if (od.ShowDialog() == DialogResult.OK)
        {
            //Debug.Log(od.FileName);
            StartCoroutine(GetTexture("file://" + od.FileName, logo));
        }
    }

    IEnumerator GetTexture(string url,Image image)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            img = www.texture;
            sprite = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
            //Debug.Log(img.width+" "+img.height);
            byte[] date = img.EncodeToPNG();
        }
    }

    public void ScreenShot()
    {
        System.DateTime now = System.DateTime.Now;
        string times = now.ToString();
        times = times.Trim();
        times = times.Replace("/", "-");
        string fileName = "ScreenShot" + times + ".png";

        //不包含UI的截屏
        //设置为arcamera的渲染
        //RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 1);
        //Camera.main.targetTexture = renderTexture;
        //Camera.main.Render();
        //RenderTexture.active = renderTexture;

        //Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        //texture.ReadPixels(new Rect(0, 0, UnityEngine.Screen.width, Screen.height), 0, 0);
        //texture.Apply();


        //byte[] bytes = texture.EncodeToPNG();
        operationView.gameObject.SetActive(false);
        //operationView.transform.position = new Vector3 (operationView.transform.position.x, operationView.transform.position.y-200f);
        if (!Directory.Exists(Directory.GetParent(Application.dataPath) + "/门诊成品海报/"))
        {
            Directory.CreateDirectory(Directory.GetParent(Application.dataPath) + "/门诊成品海报/");
        }
        string pathSave = Directory.GetParent(Application.dataPath) + "/门诊成品海报/1.png";

        if (!File.Exists(pathSave))
        {
            File.Create(pathSave);
        }
        StartCoroutine(SaveImage(pathSave));
    }
    AsyncOperation asyn;
    IEnumerator SaveImage(string path)
    {
        ScreenCapture.CaptureScreenshot(path, 5);
        yield return new WaitUntil(() => true);
        operationView.gameObject.SetActive(true);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.Application.Quit();
        }
    }
}
