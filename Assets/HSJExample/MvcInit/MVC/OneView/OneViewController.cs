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
public class OneViewController : BaseController {

	protected override void Awake() {
		base.Awake();

	}

	protected override void Start() {
		base.Start();

	}

	protected override void InitEvent() {
		base.InitEvent();

	}
	public override void HandleMessage(IMessages messages)
	{
		base.HandleMessage(messages);
		switch (messages.Type)
		{
			case MessagesType.ShowOneView:
				UIManager.Instance.ShowUI(EUiId.OneView); break;
			case MessagesType.HideOneView:
				UIManager.Instance.HideTheUI(EUiId.OneView); break;
			default:
				break;
		}
	}
}
