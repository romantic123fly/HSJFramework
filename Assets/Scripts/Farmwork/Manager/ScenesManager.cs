using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public delegate void DelAfterLoadScene();
public class ScenesManager : BaseManager<ScenesManager>
{
    private int barProgress;
    private float theProgress;
    public DelAfterLoadScene theDel;
    public string willShowSceneName;
    LoadingView view;
    //异步对象
    AsyncOperation asyn;
    public void LoadNextScene(string sceneName,  bool isLoadingBar = false,DelAfterLoadScene del = null)
    {
        willShowSceneName = sceneName; theDel = del;
        if (isLoadingBar)
        {
            view = (LoadingView)UIManager.Instance.ShowUI(EUiId.LoadingView);
            //开启一个异步任务
            StartCoroutine(LoadScene(sceneName));
        }
        else
        {
            StartCoroutine(LoadScene(sceneName));
        }
    }
    IEnumerator LoadScene(string sceneName)
    {
        barProgress = 0; theProgress = 0;

        asyn = SceneManager.LoadSceneAsync(sceneName);
        asyn.allowSceneActivation = false;
        yield return asyn;
        if (asyn.isDone)
        {
            theDel?.Invoke();
            asyn = null;
        }
    }

    protected override void Update()
    {
        if (asyn!=null)
        {
            //asyn.progress的范围是0~1（最大检测范围到0.9，所以在0.9的时候就可以显示场景了）
            if (asyn.progress < 0.9f)
            {
                //正在加载场景中ing
                theProgress = (int)asyn.progress * 100;
            }
            else
            {
                theProgress = 100;
            }
            if (barProgress <= theProgress)
            {
                barProgress++;
                if (view != null)
                {
                    view.SetSlider(barProgress);
                }
                if (barProgress == 100)
                {
                    asyn.allowSceneActivation = true;
                    UIManager.Instance.HideTheUI(EUiId.LoadingView);
                }
            }
        }
    }
}