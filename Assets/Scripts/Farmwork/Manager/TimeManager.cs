#region 模块信息
// **********************************************************************
// Copyright (C) 2017 Blazors
// Please contact me if you have any questions
// File Name:             Player
// Author:                romantic123fly
// WeChat||QQ:            at853394528 || 853394528 
// **********************************************************************
#endregion

using UnityEngine;
public struct GameTimeInfo
{
    public uint Year;
    public uint Day;
    public uint Hour;
    public uint Minute;
    public uint Weather;
    public uint Status;
}

public enum GameTimeStatus
{
    GametimeStatusStart,
    GametimeStatusPause,
    GametimeStatusSleep,
}

/// <summary>
/// 时间管理局
/// </summary>
public class TimeManager : BaseManager<TimeManager>
{
    //游戏世界时间
    private float _gameTime;
    //游戏时间状态
    private int _status;
    //服务端游戏时间信息
    private GameTimeInfo _gameTimeInfo =new GameTimeInfo ();

    //是否暂停
    private bool _isPause;

    //服务器当前秒
    private uint _serverTime = 0;
    private uint _serverTimeMsec=0;

    private float _sendTime;
    private float _recvTime =0 ;
    private uint _latency;


    //心跳间隔
    private const float SEND_INTVAL = 30.0f;

    protected override void Awake()
    {
        base.Awake();

        initStatus();
    }

    protected override void initEvent()
    {
        base.initEvent();

        //NetworkManager.instance.addEventListener(Protocol.CHECK_TIME_RESPONSE, checkTimeResponse);
        //NetworkManager.instance.addEventListener(Protocol.GAME_TIME_RESPONSE, gameTimeResponse);
        //NetworkManager.instance.addEventListener(Protocol.SET_GAME_TIME_STATUS_RESPONSE, setGameTimeStatusResponse);
    }

    public override void setup()
    {
        base.setup();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //服务器时间同步
        if((getClientTime() - _sendTime) > SEND_INTVAL)
        {
            syncTime();
        }
    }

    private void initStatus()
    {
        _gameTime = 6 * 3600;
        _status = (int)GameTimeStatus.GametimeStatusSleep;
    }

    private void syncTime()
    {
        _sendTime = getClientTime();

        //var r = new CheckTimeRequest();
        //r.sec = getServerTime();
        //r.msec = getMsec();
        //NetworkManager.instance.sendMessage(r);

        //Debug.Log("发送心跳同步:" + r.sec + "." + r.msec);
    }
    public uint getServerTime()
    {
        if (_serverTime == 0)
        {
            return 0;
        }
        uint now = _serverTime + (uint)(getClientTime() - _recvTime);
        return now;
    }

    private uint getMsec()
    {
        if (_serverTime == 0)
        {
            return 0;
        }
        uint now = _serverTimeMsec + (uint)(getClientTime() - _recvTime) * 1000;
        now %= 1000;
        return now;
    }

    public ulong getServerTimeMsec()
    {
        ulong now = (ulong)_serverTimeMsec + (ulong)_serverTime * 1000 + (ulong)((getClientTime() - _recvTime) * 1000.0f);
        return now;
    }

    public float getClientTime()
    {
        return Time.realtimeSinceStartup;
    }

    /// <summary>
    /// 现在是第几天
    /// </summary>
    public int day
    {
        get
        {
            return (int)_gameTimeInfo.Day;
        }
    }

    public int year
    {
        get
        {
            return (int)_gameTimeInfo.Year;
        }
    }

    /// <summary>
    /// 今天的时间
    /// </summary>
    public float todayTime
    {
        get
        {
            return _gameTime;
        }
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void pause()
    {
        //var req = new SetGameTimeStatusRequest();
        //req.status = 1;
        //NetworkManager.instance.sendMessage(req);
    }

    /// <summary>
    /// 睡觉
    /// </summary>
    public void sleep()
    {
        //var req = new SetGameTimeStatusRequest();
        //req.status = 2;
        //NetworkManager.instance.sendMessage(req);
    }

    /// <summary>
    /// 起床或者继续游戏
    /// </summary>
    public void cross()
    {
        //var req = new SetGameTimeStatusRequest();
        //req.status = 0;
        //NetworkManager.instance.sendMessage(req);
    }

    public bool isPause
    {
        get
        {
            return _isPause;
        }

        set
        {
            _isPause = value;
        }
    }

    public uint Weather
    {
        get
        {
            return _gameTimeInfo.Weather;
        }
    }
}
