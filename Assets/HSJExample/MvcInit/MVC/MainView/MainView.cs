#region 模块信息
// **********************************************************************
// Copyright (C) 2019 Blazors
// Please contact me if you have any questions
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainView : BaseView
{
	private Button parseCSVBtn;
	private Button findScriptsBtn;
	private Button parseJsonBtn;
	private Button eventActionBtn;
	private Button knapsackBtn;
	private Button mySqlBtn;


	public override void InitUIData()
	{
		base.InitUIData();
		rootType = EUIRootType.Normal;
		uiId = EUiId.MainView;
		currentDepth = 1;
	}

	public override void InitUIOnAwake()
	{
		base.InitUIOnAwake();
		parseCSVBtn = GlobalTools.FindTheChild(gameObject, "ParseCSV").GetComponent<Button>();
		findScriptsBtn = GlobalTools.FindTheChild(gameObject, "FindScripts").GetComponent<Button>();
		parseJsonBtn = GlobalTools.FindTheChild(gameObject, "ParseJson").GetComponent<Button>();
		eventActionBtn = GlobalTools.FindTheChild(gameObject, "EventAction").GetComponent<Button>();
		knapsackBtn = GlobalTools.FindTheChild(gameObject, "Knapsack").GetComponent<Button>();
		mySqlBtn = GlobalTools.FindTheChild(gameObject, "MySql").GetComponent<Button>();
	}

	public override void InitEvent()
	{
		base.InitEvent();
		parseCSVBtn.onClick.AddListener(()=> { ButtonOnClick(parseCSVBtn.name); });
		findScriptsBtn.onClick.AddListener(()=> { ButtonOnClick(findScriptsBtn.name); });
		parseJsonBtn.onClick.AddListener(()=> { ButtonOnClick(parseJsonBtn.name); });
		eventActionBtn.onClick.AddListener(()=> { ButtonOnClick(eventActionBtn.name); });
		knapsackBtn.onClick.AddListener(()=> { ButtonOnClick(knapsackBtn.name); });
		mySqlBtn.onClick.AddListener(()=> { ButtonOnClick(mySqlBtn.name); });
	}

	public override void Render()
	{
		base.Render();

	}
	private void ButtonOnClick(string btnName)
	{
		ScenesManager.Instance.LoadNextScene(btnName, true,()=> {
			Debug.Log("进入场景："+ btnName);
		
		});
	}


}
