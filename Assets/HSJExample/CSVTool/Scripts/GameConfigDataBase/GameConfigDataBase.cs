using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public class GameConfigDataBase
{
	protected virtual string getFilePath()
	{
		return "";
	}
	//PetConfigData
	static Dictionary<Type, Dictionary<string, GameConfigDataBase>> dataDic = new Dictionary<Type, Dictionary<string, GameConfigDataBase>>();

	public static T GetConfigData<T>(string key) where T : GameConfigDataBase
	{
		Type setT = typeof(T);
		if (!dataDic.ContainsKey(setT))
		{
			ReadConfigData<T>();
		}
		Dictionary<string, GameConfigDataBase> objDic = dataDic[setT];
		Debug.Log("test  (" + key + ")" + objDic.Count);
		if (!objDic.ContainsKey(key))
		{
			throw new Exception("no this config");
		}
		return (T)(objDic[key]);
	}

	public static List<T> GetConfigDatas<T>() where T : GameConfigDataBase
	{
		List<T> returnList = new List<T>();
		Type setT = typeof(T);
		if (!dataDic.ContainsKey(setT))
		{
			ReadConfigData<T>();
		}
		Dictionary<string, GameConfigDataBase> objDic = dataDic[setT];
		foreach (KeyValuePair<string, GameConfigDataBase> kvp in objDic)
		{
			returnList.Add((T)(kvp.Value));
		}
		return returnList;
	}

	static void ReadConfigData<T>() where T : GameConfigDataBase
	{
		T obj = Activator.CreateInstance<T>();
		string fileName = obj.getFilePath();

		string getString = Resources.Load<TextAsset>("GameConfig/" + fileName).text;

		CsvReaderByString csr = new CsvReaderByString(getString);

		Dictionary<string, GameConfigDataBase> objDic = new Dictionary<string, GameConfigDataBase>();

		FieldInfo[] fis = new FieldInfo[csr.ColCount];
		for (int colNum = 1; colNum < csr.ColCount + 1; colNum++)
		{
			fis[colNum - 1] = typeof(T).GetField(csr[1, colNum]);
		}

		for (int rowNum = 3; rowNum < csr.RowCount + 1; rowNum++)
		{
			T configObj = Activator.CreateInstance<T>();
			for (int i = 0; i < fis.Length; i++)
			{
				string fieldValue = csr[rowNum, i + 1];
				object setValue = new object();
				switch (fis[i].FieldType.ToString())
				{
					case "System.Int32":
						setValue = int.Parse(fieldValue);
						break;
					case "System.Int64":
						setValue = long.Parse(fieldValue);
						break;
					case "System.String":
						setValue = fieldValue;
						break;
					default:
						Debug.Log("error data type");
						break;
				}
				fis[i].SetValue(configObj, setValue);
				if (fis[i].Name == "key" || fis[i].Name == "id")
				{
					//只检测key和id的值，然后添加到objDic 中
					objDic.Add(setValue.ToString(), configObj);
				}
			}
		}
		dataDic.Add(typeof(T), objDic);    //可以作为参数
	}
}
