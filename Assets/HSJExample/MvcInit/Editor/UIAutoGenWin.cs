using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UIAutoGenWin : EditorWindow {
    [MenuItem("HSJ/InitUI_MVC")]
    static void Run() {
        EditorWindow.GetWindow<UIAutoGenWin>();
    }

    void OnGUI() {
        GUILayout.Label("选择一个UI 视图根节点");
        if (GUILayout.Button("生成代码")) {
            if (Selection.activeGameObject != null) {
                Debug.Log("开始生成...");
                CreatUISourceUtil.CreatUISourceFile(Selection.activeGameObject);
                Debug.Log("生成结束");
                AssetDatabase.Refresh();
            }
        }

        if (Selection.activeGameObject != null) {
            GUILayout.Label(Selection.activeGameObject.name);
        }
        else {
            GUILayout.Label("没有选中的UI节点，无法生成");
        }
    }

    void OnSelectionChange() {
        this.Repaint();
    }
}
