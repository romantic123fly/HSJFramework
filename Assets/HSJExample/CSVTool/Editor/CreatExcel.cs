using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreatExcel : EditorWindow
{
    private static string path = "/HSJExample/CSVTool/Scripts/GameConfig/";
    private static Object selectObject;
    [MenuItem("HSJ/EditorTools/CreatCSV_C#")]
    public static void Create()
    {
        CreatExcel window = GetWindow<CreatExcel>();
    }

    private void OnGUI()
    {
        GUILayout.Label("选择保存配置文件的路径");
        path = GUILayout.TextField(path);
        GUILayout.Label("选择.csv文件");
        if (GUILayout.Button("生成C#协议文件"))
        {
            if (selectObject != null)
            {
                CreatConfigUtil.CreatConfigFile(selectObject,path);
            }
        }
        if (Selection.activeObject!= null)
        {
            string paths = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (paths.ToLower().Substring(paths.Length - 4, 4) == ".csv")
            {
                selectObject = Selection.activeObject;
                GUILayout.Label(paths);
            }
            else
            {
                Debug.Log("请选择一个csv文件");
            }
        }
       
    }
    public void OnSelectionChange()
    {
        Repaint();
    }
}
