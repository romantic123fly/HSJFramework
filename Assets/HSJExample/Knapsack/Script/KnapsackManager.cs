﻿using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KnapsackManager : MonoBehaviour
{
	private static KnapsackManager instance;
	public static KnapsackManager GetInstance()
	{
	   return instance;
	}
	public GameObject slot;
	public GameObject item;

	//当前背包的所有物品槽
	public List<GameObject> slotBagList = new List<GameObject>();
	//当前背包内的所有物品Item,包括空的槽内的
	public List<ItemData> itemBagList = new List<ItemData>();

	//配置文件读取到的数据
	public List<ItemData> itemJsonDataList = new List<ItemData>();
	//背包父节点
	GameObject slotBagParent;
	private DescripPanel toolTilePanel;

	void Start()
	{
		instance = this;
		toolTilePanel = GameObject.Find("DescripPanel").GetComponent<DescripPanel>();

		slotBagParent = GameObject.Find("Canvas/KnapsackPanel/Viewport/SlotParent");

		GetItemJsonConfiguration();
		InitBag();
	}

	private void InitBag()
	{
		//初始化背包槽位
		for (int i = 0; i < 16; i++)
		{
			slotBagList.Add(Instantiate(slot));
			slotBagList[i].AddComponent<BagSlot>();
			slotBagList[i].transform.SetParent(slotBagParent.transform);
			slotBagList[i].GetComponent<BagSlot>().slotID = i;
			itemBagList.Add(new ItemData());
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
			tempItem = itemBagList.FirstOrDefault(t => t.ID == itemId);
			bool isExist = tempItem == null ? false : true;
			//当前背包内是否已经存在同类型
			if (isExist)
			{
				for (int i = 0; i < itemBagList.Count; i++)
				{
					if (itemBagList[i].ID == itemId)
					{
						GoodItem data = slotBagList[i].transform.GetChild(0).GetComponent<GoodItem>();
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
		if (itemBagList.FirstOrDefault(t => t.ID == -1) == null)
		{
			Debug.LogError("存储已满");
			return;
		}
		Debug.Log("获取物品："+ itemToAdd.Title);
		for (int i = 0; i < itemBagList.Count; i++)
		{
			if (itemBagList[i].ID == -1)
			{
				itemBagList[i] = itemToAdd;
				GameObject itemObj = Instantiate(item);
				itemObj.AddComponent<BagItem>();
				itemObj.transform.SetParent(slotBagList[i].transform);
				itemObj.transform.localPosition = Vector2.zero;
				itemObj.name = itemBagList[i].Title;
				itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
				itemObj.GetComponentInChildren<Text>().text = "1";
				itemObj.GetComponent<BagItem>().itemData = itemToAdd;
				itemObj.GetComponent<BagItem>().slotIndex = i;
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
