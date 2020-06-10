using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagSlot : MonoBehaviour ,IDropHandler,IPointerEnterHandler, IPointerExitHandler
{
	public int slotID;//槽位id
	KnapsackManager inv;
	// Use this for initialization
	void Start()
	{
		inv = KnapsackManager.GetInstance();
	}

	public void OnDrop(PointerEventData eventData){
		BagItem droppenItem = eventData.pointerDrag.GetComponent<BagItem> ();
		if (droppenItem==null)
		{
			Debug.Log("当前拖拽无效");
			return;
		}
		if (transform.childCount == 0 || inv.itemBagList[slotID].ID == -1)
		{
			//把拖拽的item对应的槽位赋值一个新的item
			inv.itemBagList[droppenItem.slotIndex] = new ItemData();
			droppenItem.slotIndex = slotID;
			//把拖拽的item赋值给当前落下的槽位
			inv.itemBagList[slotID] = droppenItem.itemData;
		}
		//交换对象，位置
		else if (droppenItem.slotIndex != slotID)
		{
			Transform item = this.transform.GetChild(0);
			item.GetComponent<GoodItem>().slotIndex = droppenItem.slotIndex;
			item.transform.SetParent(inv.slotBagList[droppenItem.slotIndex].transform);
			item.transform.position = item.transform.parent.position;
			inv.itemBagList[droppenItem.slotIndex] = item.GetComponent<BagItem>().itemData;
			droppenItem.slotIndex = slotID;
			inv.itemBagList[slotID] = droppenItem.itemData;
		}
	}
	float temp =0.5f;
	public  bool isEnter;
	private void Update()
	{
		if (isEnter)
		{
			//鼠标悬停0.5f秒钟显示描述界面
			temp -= Time.deltaTime;
			if (temp <= 0)
			{
				string text = inv.GetDescribe(inv.itemBagList[slotID]);
				KnapsackManager.GetInstance().ShowToolTilePanel(text);
				temp = 0.5f;
			}
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("鼠标进入物品槽:"+ slotID);
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
