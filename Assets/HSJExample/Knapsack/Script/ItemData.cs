using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemData{
	public int ID{ get; set; }
	public string Title{ get; set; }
	public int Value { get; set; }
	public string Desp { get; set; }
	public string MadeBy  { get; set; }
	public Sprite Sprite{ get; set; }
	public bool Stackable { get; set;}
	public int StackMax{ get; set;}

	public ItemData(int _id, string _title, int _value, string _des, string _mader, string _slug,bool _stackable, int _stackMax){
		this.ID = _id;
		this.Title = _title;
		this.Value = _value;
		this.Desp = _des;
		this.MadeBy = _mader;
		this.Sprite = Resources.Load<Sprite> ("Image/Knapsack/" + _slug);
		this.Stackable = _stackable;
		this.StackMax = _stackMax;
	
	}
	public ItemData(){
		this.ID = -1;
	}

}