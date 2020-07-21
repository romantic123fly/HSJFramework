#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             TouchManager
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum TouchMoveDir
{
    idle, left, right, up, down
}
public class TouchManager : MonoBehaviour
{
    public GameObject target;
    TouchMoveDir moveDir;

    public static Vector3 currentScale;

    private Touch oldTouch1;
    private Touch oldTouch2;

    // Update is called once per frame
    void Update()
    {
        Dblclick();
        LongPress();
        Move();
        Rotating();
        ObjectScale();
    }
    //多次点击
    public void Dblclick()
    {
        //判断几个点击位置而且是最开始点击的屏幕，而不是滑动屏幕
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("第一次");
            if (Input.GetTouch(0).tapCount == 2)//tapcount是点击次数
            {
                Debug.Log("第二次");
            }
            else if (Input.GetTouch(0).tapCount == 3)
            {
                Debug.Log("第三次");
            }
            else
            {
                Debug.Log("第" + Input.GetTouch(0).tapCount + "次");
            }
        }
    }
    //长按
    float touchTime = 0;
    public void LongPress()
    {
      
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Stationary)//点击没有滑动的时候会触发Stationary
            {
                if (Time.time - touchTime > 5f)
                {
                    Debug.Log("长按" + 5+ "秒");
                }
            }
        }
    }
    //滑动
    public void Move()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (Input.GetTouch(0).deltaPosition.sqrMagnitude > 1)
            {
                Vector2 deltaDir = Input.GetTouch(0).deltaPosition;
                if (Mathf.Abs(deltaDir.x) > Mathf.Abs(deltaDir.y))
                {
                    moveDir = deltaDir.x > 0 ? TouchMoveDir.right : TouchMoveDir.left;
                }
                if (Mathf.Abs(deltaDir.y) > Mathf.Abs(deltaDir.x))
                {
                    moveDir = deltaDir.y > 0 ? TouchMoveDir.up : TouchMoveDir.down;
                }
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            moveDir = TouchMoveDir.idle;
        }
    
        if (moveDir == TouchMoveDir.right)
        {
            Debug.Log("right");
            target.transform.position += transform.right * 0.2f;
        }
        if (moveDir == TouchMoveDir.left)
        {
            Debug.Log("left");
            target.transform.position -= transform.right * 0.2f;
        }
        if (moveDir == TouchMoveDir.up)
        {
            target.transform.position += transform.up * 0.2f;
        }
        if (moveDir == TouchMoveDir.down)
        {
            target.transform.position -= transform.up * 0.2f;
        }
    }
    //单指旋转
    public void Rotating()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.deltaPosition.x!=0)
            {
                Vector2 deltaPos = touch.deltaPosition;
                target.transform.Rotate(Vector3.down * deltaPos.x, Space.Self);
                //target.transform.Rotate(Vector3.up * deltaPos.x, Space.World);
                Debug.Log("旋转");
            }
        }
    }
    //双指缩放
    public void ObjectScale()
    {
        if (Input.touchCount == 2)
        {
            //缩放
            Touch newTouch1 = Input.GetTouch(0);
            Touch newTouch2 = Input.GetTouch(1);

            if (newTouch2.phase == TouchPhase.Began)
            {
                oldTouch2 = newTouch2;
                oldTouch1 = newTouch1;
                return;
            }
            float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
            float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
            float offset = newDistance - oldDistance;
            //通过相机的fieldOfView参数实现远近缩放
            //float field = Camera.main.fieldOfView;
            //field -= offset / 10f;
            //field = Mathf.Clamp(field, 40, 80);
            //Camera.main.fieldOfView = field;
            //oldTouch1 = newTouch1;
            //oldTouch2 = newTouch2;
            //通过localScale参数实现远近缩放
            float scaleFactor = offset / 200f;
            Vector3 localScale = target.transform.localScale;
            Vector3 scale = new Vector3(localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor);
            if (scale.x >= 0.5f && scale.x <= 3)
            {
                target.transform.localScale = scale;
                currentScale = scale;
                Debug.Log("缩放");
            }
        }
    }
}
