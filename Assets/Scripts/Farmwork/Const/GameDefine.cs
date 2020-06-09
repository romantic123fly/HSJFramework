#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion
using System.Collections.Generic;
using UnityEngine;



//游戏自定义的属性类
public class GameDefine
{
    public static bool isDebug = false;

    public const float UNIT_RATIO = 100;
    public const float DESIGN_WIDTH = 1280.0f;
    public const float DEGIGN_HEIGHT = 720.0f;
    //设计尺寸
    public static Vector3 DESING_SIZE = new Vector3(DESIGN_WIDTH / UNIT_RATIO, DEGIGN_HEIGHT / UNIT_RATIO);

    //世界最小单元格
    public const float TILE_SIZE = 0.32f;
    //世界最大单元格数量
    public static int MAX_TILE_SIZE = 100;

    //代码相关
    public static string DOUBLE_SLASH = "//";
    public static string DOUBLE_BACKSLASH = "\\";
    public static char SPACE = ' ';

    //Resources路径
    public const string PREFAB_PATH = "Prefabs/";
   
    public const string UI_PREFAB_PATH = "Prefabs/UI/";
  

    //季节资源名
    public static string[] SEASON_NAME = { "Spring", "Summer", "Autumn", "Winter" };
    //朝向资源名
    public static string[] DIRECTION_NAME = { "01", "02", "03", "04" };

    //语言包路径
    public const string LANG_PATH = "Language/zh_Hans_CN.po";

    public const string MAP_XML_PATH = "Maps/";
    public const string UI_PATH = "UI/";
    //特效路径
    public const string EFFECT_PREFAB_PATH = "Effect/";
    public const string DATA_PATH = "Data/";
    public const string DRAMA_PATH = "Data/Drama/";
    public const string SOUND_PATH = "Audio/Sound/";
    public const string MUSIC_PATH = "Audio/Music/";
    public const string ICON = "Icon/";
    public const string ICON_ITEM = "Icon/Item/";
    public const string ICON_ITEM_PREFAB_PATH = "Prefabs/Icon/Item/";



    //半身像
    public const string ICON_PORTRAIT = "Icon/Portrait/";
    //头像
    public const string ICON_HEAD = "Icon/Head/";
    //avatar
    public const string AVATAR_PATH = "Animation/Avatar/";
    //其他
    public const string ICON_OTHER = "Icon/Other/";

    //资源名
    public const int PLAYER_ID = 100101;
    //死亡植物
    public const int PLANT_DEATH = 10000000;

    public const string ENEMY_GROUND_NAME = "Enemy_101001";
    public const int ENEMY_GROUND_ID = 101001;

    public const string MAP_NAME = "Map";

    //真实一天时间(秒)
    public const float REAL_DAY_TIME = 86400.0f;
    //狼叫时间
    public const float DUSK_WOLF = 17640.0f;
    //关卡传送建筑id
    public const int DUNGEON_BUILDING = 11101006;
    //家ID
    public const uint HOME_SCENE_ID = 10001;
    //  背包容量
    public const int BAG_MAX = 20;
    //  快捷栏容量
    public const int SHORTCUT_MAX = 5;

    //长虫掉血
    public static int fieldBugEffect = 0;
    //GlobalConfig
   
}
