using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Networking;
using System.IO;

public class MyTool
{
    //一系列批量build的操作
    [MenuItem("HSJ/EditorTools/Package/BuildAndroid")]
    static void PerformAndroidPadBuild()
    {
        BulidTarget("HSJFramework", "Android");
    }
    [MenuItem("HSJ/EditorTools/Package/BuildIOS")]
    static void PerformAndroidVRBuild()
    {
        BulidTarget("HSJFramework", "IOS");
    }
    //这里封装了一个简单的通用方法。
    static void BulidTarget(string appName, string target)
    {
        string targetPath = "";
        string target_name = appName + ".apk";
        BuildTargetGroup targetGroup = BuildTargetGroup.Android;
        BuildTarget buildTarget = BuildTarget.Android;

        PlayerSettings.applicationIdentifier = "com.hsj.game";
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, appName);
        if (target == "Android")
        {
            targetPath = Application.dataPath.Replace("/Assets", "/Build");
            target_name = appName + ".apk";
            targetGroup = BuildTargetGroup.Android;
        }
        if (target == "IOS")
        {
            targetPath = Application.dataPath.Replace("/Assets", "/Build");
            target_name = appName;
            targetGroup = BuildTargetGroup.iOS;
            buildTarget = BuildTarget.iOS;
        }

        //每次build删除之前的残留
        if (Directory.Exists(targetPath))
        {
            if (File.Exists(target_name))
                File.Delete(target_name);
        }
        else
            Directory.CreateDirectory(targetPath);

        GenericBuild(targetPath + "/" + target_name, buildTarget, targetGroup);
    }

    static void GenericBuild(string buildPath, BuildTarget buildTarget, BuildTargetGroup targetGroup)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, buildTarget);
        string[] sceneName = FindEnabledEditorScenes();
        BuildPipeline.BuildPlayer(sceneName, buildPath, buildTarget, BuildOptions.None);

    }
    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

}
