using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KnapsackItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public ItemData itemData;
	public int slotIndex;
	public int amount = 1;

	public void OnBeginDrag(PointerEventData eventData){
		if(itemData != null){
			transform.SetParent (transform.parent.parent);
			transform.position = eventData.position;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
			//如果描述界面显示 就把他关闭
			KnapsackManager.GetInstance().HideToolTilePanel();
			KnapsackManager.GetInstance().slotList[slotIndex].GetComponent<Slot>().isEnter=false;
		}
	}
	public void OnDrag(PointerEventData eventData){
		if(itemData != null){
			transform.position = eventData.position;
		}
	}
	public void OnEndDrag(PointerEventData eventData){
		//slotIndex已经在drop逻辑内重新赋值 
		transform.SetParent (KnapsackManager.GetInstance().slotList[slotIndex].transform);
		transform.position = this.transform.parent.position;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}
