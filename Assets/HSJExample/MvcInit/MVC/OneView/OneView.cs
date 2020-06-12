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
public class OneView : BaseView {

	public Button last;
	public Button next;
	public override void InitUIData() {
		base.InitUIData();
		rootType = EUIRootType.Normal;
		isReturnInfo = true;
		isSingleUse = true;
		uiId = EUiId.OneView;
	}

	public override void InitUIOnAwake() {
		base.InitUIOnAwake();
		last = GlobalTools.FindTheChild(gameObject, "Last").GetComponent<Button>();
		next = GlobalTools.FindTheChild(gameObject, "Next").GetComponent<Button>();
	}

	public override void InitEvent() {
		base.InitEvent();
		
		last.onClick.AddListener(UIManager.Instance.ClickReturn);
		next.onClick.AddListener(() => {
			MessageDispatcher.Dispatch(MessagesType.ShowTwoView);
		});
	}

	public override void Render() {
		base.Render();
	}

}
