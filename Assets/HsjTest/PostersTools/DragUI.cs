#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             DragUI
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private Vector2 offsetPos;  //临时记录点击点与UI的相对位置

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - offsetPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offsetPos = eventData.position - (Vector2)transform.position;
    }
}
