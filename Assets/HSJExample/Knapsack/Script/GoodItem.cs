using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoodItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public ItemData itemData;
	public int slotIndex;
	public int amount = 1;

	public virtual void OnBeginDrag(PointerEventData eventData){
		
	}
	public virtual void OnDrag(PointerEventData eventData){
		if(itemData != null){
			transform.position = eventData.position;
		}
	}
	public virtual void OnEndDrag(PointerEventData eventData){
	
	}
}
