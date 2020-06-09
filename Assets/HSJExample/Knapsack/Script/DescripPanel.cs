using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DescripPanel : MonoBehaviour
{
	private float alpha = 0.0f;
	//做淡入淡出
	private float alphaSpeed = 1.0f;
	private Text toolTilePanel;
	private Text toolTileText;
	//此处利用CanvasGroup组件处理整个界面的显示隐藏
	private CanvasGroup cg;
	// Use this for initialization
	void Start ()
	{
		toolTilePanel = this.transform.GetComponent<Text>();
		cg = this.transform.GetComponent<CanvasGroup>();
		toolTileText = this.transform.GetChild(1).GetComponent<Text>();
	}
	
	void Update ()
	{
		if (alpha != cg.alpha)
		{
			//做淡入淡出
			//cg.alpha = Mathf.Lerp(cg.alpha,alpha, Time.deltaTime);
			//if (Mathf.Abs(alpha-cg.alpha)<=0.01)
			//{
			//	cg.alpha = alpha;
			//}
			cg.alpha = alpha;
		}
		//跟随鼠标固定偏移
		Vector2 point = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<Canvas>().transform as RectTransform, Input.mousePosition, null, out point);
		transform.localPosition = point + new Vector2(10, -50);
	}
	public void ShowPanel(string str = "未读取")
	{
		//此处利用ContentSizeFitter组件做一个 背景尺寸自适应文字内容
		toolTilePanel.text = str;
		toolTileText.text = str;
		this.alpha = 1;
	}
	public void HidePanel()
	{
		alpha = 0;
	}
}
