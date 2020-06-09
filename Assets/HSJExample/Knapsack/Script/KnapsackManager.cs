using LitJson;
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
	public List<GameObject> slotList = new List<GameObject>();
	//当前背包内的所有物品Item,包括空的槽内的
	public List<ItemData> itemList = new List<ItemData>();
	//配置文件读取到的数据
	public List<ItemData> itemJsonDataList = new List<ItemData>();
	//背包父节点
	GameObject slotParent;
	private DescripPanel toolTilePanel;

	void Start()
	{
		instance = this;
		slotParent = GameObject.Find("SlotParent");
		toolTilePanel = GameObject.Find("DescripPanel").GetComponent<DescripPanel>();
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

	    //初始化槽位
		for (int i = 0; i < 20; i++)
		{
			slotList.Add(Instantiate(slot));
			slotList[i].transform.SetParent(slotParent.transform);
			slotList[i].GetComponent<Slot>().slotID = i;
			itemList.Add(new ItemData());
		}
		//初始化物品
		for (int i = 0; i < 5; i++)
		{
			GetGoods();
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
			tempItem = itemList.FirstOrDefault(t => t.ID == itemId);
			bool isExist = tempItem == null ? false : true;
			//当前背包内是否已经存在同类型
			if (isExist)
			{
				for (int i = 0; i < itemList.Count; i++)
				{
					if (itemList[i].ID == itemId)
					{
						KnapsackItem data = slotList[i].transform.GetChild(0).GetComponent<KnapsackItem>();
						if (data.amount < itemToAdd.StackMax)
						{
							data.amount++;
							data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
						}
						else
						{
							CreatNewItem(itemToAdd);
							break;
						}
					}
				}
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
		Debug.Log("获取物品："+ itemToAdd.Title);
		for (int i = 0; i < itemList.Count; i++)
		{
			if (itemList[i].ID == -1)
			{
				itemList[i] = itemToAdd;
				GameObject itemObj = Instantiate(item);
				itemObj.transform.SetParent(slotList[i].transform);
				itemObj.transform.localPosition = Vector2.zero;
				itemObj.name = itemList[i].Title;
				itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
				itemObj.GetComponentInChildren<Text>().text = "1";
				itemObj.GetComponent<KnapsackItem>().itemData = itemToAdd;
				itemObj.GetComponent<KnapsackItem>().slotIndex = i;
				break;
			}
		}
	}
	public virtual string GetDescribe(ItemData itemData)
	{
		string describe = string.Format("<color={0}>{1}</color>\n<size=10><color=green>购买价格：{2} 出售价格：{3}</color></size>\n<color=yellow><size=10>{4}</size></color>", Color.white, itemData.Title, itemData.Value, itemData.Value, itemData.Desp);
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
