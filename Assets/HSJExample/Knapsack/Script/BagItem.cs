#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             BagItem
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagItem : GoodItem
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (itemData != null)
        {
            transform.SetParent(transform.parent.parent);
            transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            //如果描述界面显示 就把他关闭
            KnapsackManager.GetInstance().HideToolTilePanel();
            KnapsackManager.GetInstance().slotBagList[slotIndex].GetComponent<BagSlot>().isEnter = false;
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        //slotIndex已经在drop逻辑内重新赋值 
        transform.SetParent(KnapsackManager.GetInstance().slotBagList[slotIndex].transform);
        transform.position = transform.parent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
