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
using System;

public class LoadingView : BaseView {
	private Slider scheduleSlider;
	private Text scheduleTips;
	public override void InitUIData() {
		base.InitUIData();
		uiId = EUiId.LoadingView;
		
		isSingleUse = true;
	}

	public override void InitUIOnAwake() {
		base.InitUIOnAwake();
		scheduleSlider = GlobalTools.FindTheChild(gameObject,"Slider").GetComponent<Slider>();
		scheduleTips = GlobalTools.FindTheChild(gameObject, "ScheduleTips").GetComponent<Text>();
	}

	public override void InitEvent() {
		base.InitEvent();

	}

	public override void Render() {
		base.Render();

	}
	public void SetSlider(float f)
	{
		scheduleSlider.value = f;
		scheduleTips.text = Mathf.FloorToInt (f)+"";
	}
}
