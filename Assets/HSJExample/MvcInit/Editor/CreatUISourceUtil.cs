using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class CreatUISourceUtil {

    //编辑器自定义  MVC类的生成
    public static void CreatUISourceFile(GameObject selectGameObject)
    {
        string gameObjectName = selectGameObject.name;
        CustomModel(gameObjectName);
        CustomView(gameObjectName);
        CustomController(gameObjectName);
    }

    private static void CustomView(string gameObjectName)
    {
        string className = gameObjectName;
        StreamWriter sw = null;
        if (!Directory.Exists(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName))
        {
            Directory.CreateDirectory(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName);
        }
        sw = new StreamWriter(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName + "/" + className + ".cs");
        sw.WriteLine("#region 模块信息");
        sw.WriteLine("// **********************************************************************");
        sw.WriteLine("// Copyright (C) 2019 Blazors");
        sw.WriteLine("// Please contact me if you have any questions");
        sw.WriteLine("// Author:                幻世界");
        sw.WriteLine("// QQ:                    853394528 ");
        sw.WriteLine("// **********************************************************************");
        sw.WriteLine("#endregion");

        sw.WriteLine("using UnityEngine;\nusing System.Collections;\nusing UnityEngine.UI;\nusing System.Collections.Generic;");
        sw.WriteLine("public class " + className + " : BaseView {" + "\n");
        sw.WriteLine("\t" + "public override void InitUIData() {");
        sw.WriteLine("\t\t" + "base.InitUIData();" + "\n");
        sw.WriteLine("\t" + "}" + "\n");
        sw.WriteLine("\t" + "public override void InitUIOnAwake() {");
        sw.WriteLine("\t\t" + "base.InitUIOnAwake();" + "\n");
        sw.WriteLine("\t" + "}" + "\n");
        sw.WriteLine("\t" + "public override void InitEvent() {");
        sw.WriteLine("\t\t" + "base.InitEvent();" + "\n");
        sw.WriteLine("\t" + "}" + "\n");
        sw.WriteLine("\t" + "public override void Render() {");
        sw.WriteLine("\t\t" + "base.Render();" + "\n");
        sw.WriteLine("\t" + "}" + "\n");
        sw.WriteLine("}");
        sw.Flush();
        sw.Close();

        Debug.Log("Gen: " + Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName + "/" + className + ".cs");
    }
    private static void CustomController(string gameObjectName)
    {
        string className = gameObjectName + "Controller";
        StreamWriter sw = null;

        if (!Directory.Exists(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName))
        {
            Directory.CreateDirectory(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName);
        }
        sw = new StreamWriter(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName + "/" + className + ".cs");
        sw.WriteLine("#region 模块信息");
        sw.WriteLine("// **********************************************************************");
        sw.WriteLine("// Copyright (C) 2019 Blazors");
        sw.WriteLine("// Please contact me if you have any questions");
        sw.WriteLine("// Author:                幻世界");
        sw.WriteLine("// QQ:                    853394528 ");
        sw.WriteLine("// **********************************************************************");
        sw.WriteLine("#endregion");
        sw.WriteLine("using UnityEngine;\nusing System.Collections;\nusing UnityEngine.UI;\nusing System.Collections.Generic;");

        sw.WriteLine("public class " + className + " : BaseController {" + "\n");

        sw.WriteLine("\t" + "protected override void Awake() {");
        sw.WriteLine("\t\t" + "base.Awake();" + "\n");
        sw.WriteLine("\t" + "}" + "\n");
        sw.WriteLine("\t" + "protected override void Start() {");
        sw.WriteLine("\t\t" + "base.Start();" + "\n");
        sw.WriteLine("\t" + "}" + "\n");
        sw.WriteLine("\t" + "protected override void InitEvent() {");
        sw.WriteLine("\t\t" + "base.InitEvent();" + "\n");
        sw.WriteLine("\t" + "}" + "\n");
        sw.WriteLine("}");
        sw.Flush();
        sw.Close();

        Debug.Log("Gen: " + Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName + "/" + className + ".cs");
    }
    private static void CustomModel(string gameObjectName)
    {
        string className = gameObjectName + "Model";
        StreamWriter sw = null;
        if (!Directory.Exists(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName))
        {
            Directory.CreateDirectory(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName);
        }
        sw = new StreamWriter(Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName + "/" + className + ".cs");
        sw.WriteLine("#region 模块信息");
        sw.WriteLine("// **********************************************************************");
        sw.WriteLine("// Copyright (C) 2019 Blazors");
        sw.WriteLine("// Please contact me if you have any questions");
        sw.WriteLine("// Author:                幻世界");
        sw.WriteLine("// QQ:                    853394528 ");
        sw.WriteLine("// **********************************************************************");
        sw.WriteLine("#endregion");

        sw.WriteLine("using UnityEngine;\nusing System.Collections;\nusing UnityEngine.UI;\nusing System.Collections.Generic;");

        sw.WriteLine("public class " + className + "{"+ "\n");
        sw.WriteLine("}");
        sw.Flush();
        sw.Close();

        Debug.Log("Gen: " + Application.dataPath + "/Example/MvcInit/UI/" + gameObjectName + "/" + className + ".cs");
    }
}
