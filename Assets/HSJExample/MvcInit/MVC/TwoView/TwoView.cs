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
public class TwoView : BaseView {
	private Button last;
	public override void InitUIData() {
		base.InitUIData();
		rootType = EUIRootType.Normal;
		isReturnInfo = true;
		isSingleUse = true;
		uiId = EUiId.TwoView;
	}

	public override void InitUIOnAwake() {
		base.InitUIOnAwake();
		last = GlobalTools.FindTheChild(gameObject,"Last").GetComponent<Button>();
	}

	public override void InitEvent() {
		base.InitEvent();
		last.onClick.AddListener(UIManager.Instance.ClickReturn);
	}

	public override void Render() {
		base.Render();

	}

}
