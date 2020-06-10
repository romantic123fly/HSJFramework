#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             EquipmentManager
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
	private static EquipmentManager instance;
	public static EquipmentManager GetInstance()
	{
		return instance;
	}
	public GameObject slot;
	public GameObject item;

	//当前背包的所有物品槽
	public List<GameObject> slotEquipList = new List<GameObject>();
	//当前背包内的所有物品Item,包括空的槽内的
	public List<ItemData> itemEquipList = new List<ItemData>();

	//配置文件读取到的数据
	public List<ItemData> itemJsonDataList = new List<ItemData>();
	//背包父节点
	GameObject slotEquipParent;
	private DescripPanel toolTilePanel;

	void Start()
	{
		instance = this;
		toolTilePanel = GameObject.Find("DescripPanel").GetComponent<DescripPanel>();

		slotEquipParent = GameObject.Find("Canvas/EquipmentPanel/Viewport/SlotParent");

		GetItemJsonConfiguration();
		InitEquip();
	}

	private void InitEquip()
	{
		//初始化背包槽位
		for (int i = 0; i < 12; i++)
		{
			slotEquipList.Add(Instantiate(slot));
			slotEquipList[i].AddComponent<EquipSlot>();
			slotEquipList[i].transform.SetParent(slotEquipParent.transform);
			slotEquipList[i].GetComponent<EquipSlot>().slotID = i;
			itemEquipList.Add(new ItemData());
		}
		//初始化物品
		for (int i = 0; i < 5; i++)
		{
			GetGoods();
		}
	}

	private void GetItemJsonConfiguration()
	{
		//解析json数据，初始化itemJsonDataList
		JsonData jsonData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/HSJExample/JsonTool/Resources/JsonData/items.json"));
		for (int i = 0; i < jsonData.Count; i++)
		{
			//存储在database
			itemJsonDataList.Add(new ItemData(
				(int)jsonData[i]["id"],
				jsonData[i]["title"].ToString(),
				(int)jsonData[i]["value"],
				jsonData[i]["description"].ToString(),
				jsonData[i]["madeby"].ToString(),
				jsonData[i]["slug"].ToString(),
				(bool)jsonData[i]["stackavle"],
				(int)jsonData[i]["stackMax"])
				);
		}
	}

	public void GetGoods()
	{
		Additem(Random.Range(0, itemJsonDataList.Count));
	}
	public void Additem(int itemId)
	{
		var tempItem = itemJsonDataList.FirstOrDefault(t => t.ID == itemId);
		ItemData itemToAdd = tempItem == null ? null : tempItem;

		//判断当前物品属性是否可叠加且
		if (itemToAdd.Stackable == true)
		{
			tempItem = itemEquipList.FirstOrDefault(t => t.ID == itemId);
			bool isExist = tempItem == null ? false : true;
			//当前背包内是否已经存在同类型
			if (isExist)
			{
				for (int i = 0; i < itemEquipList.Count; i++)
				{
					if (itemEquipList[i].ID == itemId)
					{
						GoodItem data = slotEquipList[i].transform.GetChild(0).GetComponent<GoodItem>();
						if (data.amount < itemToAdd.StackMax)
						{
							data.amount++;
							data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
							return;
						}
					}
				}
				CreatNewItem(itemToAdd);
			}
			else
			{
				CreatNewItem(itemToAdd);
			}
		}
		else
		{
			CreatNewItem(itemToAdd);
		}
	}

	void CreatNewItem(ItemData itemToAdd)
	{
		if (itemEquipList.FirstOrDefault(t => t.ID == -1) == null)
		{
			Debug.LogError("存储已满");
			return;
		}
		Debug.Log("获取物品：" + itemToAdd.Title);
		for (int i = 0; i < itemEquipList.Count; i++)
		{
			if (itemEquipList[i].ID == -1)
			{
				itemEquipList[i] = itemToAdd;
				GameObject itemObj = Instantiate(item);
				itemObj.AddComponent<EquipItem>();
				itemObj.transform.SetParent(slotEquipList[i].transform);
				itemObj.transform.localPosition = Vector2.zero;
				itemObj.name = itemEquipList[i].Title;
				itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
				itemObj.GetComponentInChildren<Text>().text = "1";
				itemObj.GetComponent<EquipItem>().itemData = itemToAdd;
				itemObj.GetComponent<EquipItem>().slotIndex = i;
				break;
			}
		}
	}

	public virtual string GetDescribe(ItemData itemData)
	{
		string describe = string.Format("<color={0}>{1}</color>\n<size=20><color=green>购买价格：{2} 出售价格：{3}</color></size>\n<color=yellow><size=20>{4}</size></color>", Color.white, itemData.Title, itemData.Value, itemData.Value, itemData.Desp);
		return describe;
	}
	public void ShowToolTilePanel(string str = " ")
	{
		toolTilePanel.GetComponent<DescripPanel>().ShowPanel(str);
	}
	public void HideToolTilePanel()
	{
		toolTilePanel.GetComponent<DescripPanel>().HidePanel();
	}
}
