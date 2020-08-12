#region 模块信息
// **********************************************************************
// Copyright (C) 2020 
// Please contact me if you have any questions
// File Name:             HSJ_Hotfix
// Author:                幻世界
// QQ:                    853394528 
// **********************************************************************
#endregion
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using XLua.LuaDLL;

public class HSJ_Hotfix : MonoBehaviour
{
    private LuaEnv luaEnv;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        //新建注册机
        luaEnv = new LuaEnv();
        //加载lua脚本
        luaEnv.AddLoader(HsjLoadLua);
        //执行lua命令
        luaEnv.DoString("require'hsj'");
     
    }

    // Update is called once per frame
    void Update()
    {
    }
    //加载lua脚本
    private byte[] HsjLoadLua(ref string filePath)
    {
        //lua文件路径
        string absPath =Application.dataPath+ "/XLua/Resources/xlua/" + filePath + ".lua";
        return System.Text.Encoding.UTF8.GetBytes(File.ReadAllText(absPath));
    }

    private void OnDisable()
    {
        luaEnv.DoString("require'luaDispose'");
    }
    private void OnDestroy()
    {
        luaEnv.Dispose();
        Debug.Log("释放Lua注册机");
    }
}
