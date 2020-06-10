#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             EquipSlot
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public int slotID;//槽位id
	EquipmentManager inv;
	// Use this for initialization
	void Start()
	{
		inv = EquipmentManager.GetInstance();
	}

	public void OnDrop(PointerEventData eventData)
	{
		EquipItem droppenItem = eventData.pointerDrag.GetComponent<EquipItem>();
		if (droppenItem == null)
		{
			Debug.Log("当前拖拽无效");
			return;
		}
		if (transform.childCount == 0 || inv.itemEquipList[slotID].ID == -1)
		{
			//把拖拽的item对应的槽位赋值一个新的item
			inv.itemEquipList[droppenItem.slotIndex] = new ItemData();
			droppenItem.slotIndex = slotID;
			//把拖拽的item赋值给当前落下的槽位
			inv.itemEquipList[slotID] = droppenItem.itemData;
		}
		//交换对象，位置
		else if (droppenItem.slotIndex != slotID)
		{
			Transform item = this.transform.GetChild(0);
			item.GetComponent<EquipItem>().slotIndex = droppenItem.slotIndex;
			item.transform.SetParent(inv.slotEquipList[droppenItem.slotIndex].transform);
			item.transform.position = item.transform.parent.position;
			inv.itemEquipList[droppenItem.slotIndex] = item.GetComponent<EquipItem>().itemData;
			droppenItem.slotIndex = slotID;
			inv.itemEquipList[slotID] = droppenItem.itemData;
		}
	}
	float temp = 0.5f;
	public bool isEnter;
	private void Update()
	{
		if (isEnter)
		{
			//鼠标悬停0.5f秒钟显示描述界面
			temp -= Time.deltaTime;
			if (temp <= 0)
			{
				string text = inv.GetDescribe(inv.itemEquipList[slotID]);
				KnapsackManager.GetInstance().ShowToolTilePanel(text);
				temp = 0.5f;
			}
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("鼠标进入物品槽:" + slotID);
		if (this.transform.childCount > 0)
		{
			isEnter = true; temp = 0.5f;
			//获得描述文本
		}
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		if (this.transform.childCount > 0)
		{
			isEnter = false;
			KnapsackManager.GetInstance().HideToolTilePanel();
		}
	}
}
