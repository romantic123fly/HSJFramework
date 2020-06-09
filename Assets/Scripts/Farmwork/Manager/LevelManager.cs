#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : BaseManager<LevelManager>
{
    private string currentScene;
    //将要切换到的场景名
    protected string _nextSceneName;
    //场景类型
    protected int _currentSceneType;
    protected int _lastSceneType;
    bool _isShowLoading = false;
    protected override void initEvent()
    {
        base.initEvent();

        //NetworkManager.instance.addEventListener(Protocol.ENTER_SCENE_RESPONSE, enterSceneResponse);
    }

    public void changeScene(string value,bool isShowLoading)
    {
        _isShowLoading = isShowLoading;
        _nextSceneName = value;
        if (isShowLoading)
        {
            StartCoroutine(loadScene(SceneName.LoadingScene));
        }
        else
        {                 
            StartCoroutine(loadScene(value));
         }             
    }

    private IEnumerator loadScene(string sceneName)
    {
        Debug.Log("1");
        var sync = SceneManager.LoadSceneAsync(sceneName);       
        yield return sync;

        if (sync.isDone)
        {
            Debug.Log("2");

            setupScene(sceneName, _isShowLoading);
        }
    }

    public void setupScene(string nextSceneName,bool isShowLoading)
    {
        if (!isShowLoading)
        {
            _lastSceneType = _currentSceneType;
            Debug.Log(currentScene);
            SceneManager.UnloadSceneAsync(currentScene);
        }
        else
        {
            if (CurrentScene != null)
            {
                _lastSceneType = _currentSceneType;
                SceneManager.UnloadSceneAsync(SceneName.LoadingScene);
            }
        }
       

        GameObject go = new GameObject();
        go.name = nextSceneName;

        switch (nextSceneName)
        {
            case SceneName.LoadingScene:
            {
               //_currentScene = go.AddComponent<LoadingScene>();                  
                break;
            }
            case SceneName.PreloadScene:
            {
                //_currentScene = go.AddComponent<PreloadScene>();
                break;
            }
            case SceneName.LoginScene:
            {
                //_currentScene = go.AddComponent<LoginScene>();
                break;
            }
            case SceneName.CityScene:
            {
                //_currentScene = go.AddComponent<CityScene>();
                //var scene = _currentScene as CityScene;
                //_currentSceneType = scene.sceneData.type;
                break;
            }
            case SceneName.DramaScene:
            {
                //_currentScene = go.AddComponent<DramaScene>();
                break;
            }
            case SceneName.TestScene:
            {
                break;
            }
            default:
            {
                break;
            }
        }
    }

    public string nextSceneName
    {
        get
        {
            return _nextSceneName;
        }
    }

    public int lastSceneType
    {
        get
        {
            return _lastSceneType;
        }
    }

    public string CurrentScene
    {
        get
        {
            return currentScene;
        }

        set
        {
            currentScene = value;
        }
    }
}
