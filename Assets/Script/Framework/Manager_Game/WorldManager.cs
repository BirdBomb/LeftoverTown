using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using DG.Tweening;
using UnityEngine.Rendering;
using UniRx;
using System;

/// <summary>
/// 世界管理器
/// 用于管理所有生物/时间/光照/事件
/// </summary>
public class WorldManager : SingleTon<WorldManager>, ISingleTon
{
    [Header("网络组件")]
    public GameNetManager gameNetManager;
    private GlobalTime globalTimeNow;
    public GlobalTime GlobalTimeNow
    {
        get { return globalTimeNow; }
        set { globalTimeNow = value; }
    }
    private Weather weatherNow;
    public Weather WeatherNow
    {
        get { return weatherNow; }
        set { weatherNow = value; }
    }

    public void Init()
    {
        AudioManager.Instance.PlayMusic(9000, 0, true);
    }

    public void UpdateSecond(int second, int hour, int day)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_All_UpdateSecond()
        {
            second = second,
            secondPerHour = gameNetManager.int_SecondsPerHour,
            hour = hour,
            hourPerDay = gameNetManager.int_HourPerDay,
            day = day,
            gameTime = second + hour * gameNetManager.int_SecondsPerHour + day * gameNetManager.int_SecondsPerHour * gameNetManager.int_HourPerDay,
            now = GlobalTimeNow
        });
        switch (GlobalTimeNow)
        {
            case GlobalTime.Morning:
                {
                    WorldEventManager.Instance.InMorning();
                }
                break;
            case GlobalTime.Forenoon:
                {
                    WorldEventManager.Instance.InForenoon();
                }
                break;
            case GlobalTime.Highnoon:
                {
                    WorldEventManager.Instance.InHighnoon();
                }
                break;
            case GlobalTime.Afternoon:
                {
                    WorldEventManager.Instance.InAfternoon();
                }
                break;
            case GlobalTime.Dusk:
                {
                    WorldEventManager.Instance.InDusk();
                }
                break;
            case GlobalTime.Evening:
                {
                    WorldEventManager.Instance.InEvening();
                }
                break;
        }

    }
    public void UpdateHour(int hour, int day)
    {
        //Debug.Log("当前时间" + hour + "/"+ day);
        switch (hour)
        {
            case 0:
                {
                    WorldEventManager.Instance.StartMorning();
                    UpdateGlobalTime(GlobalTime.Morning);
                }
                break;
            case 1:
                {
                    WorldEventManager.Instance.StartForenoon();
                    UpdateGlobalTime(GlobalTime.Forenoon);
                }
                break;
            case 2:
                {
                    UpdateGlobalTime(GlobalTime.Forenoon);
                }
                break;
            case 3:
                {
                    WorldEventManager.Instance.StartHighnoon();
                    UpdateGlobalTime(GlobalTime.Highnoon);
                }
                break;
            case 4:
                {
                    WorldEventManager.Instance.StartAfternoon();
                    UpdateGlobalTime(GlobalTime.Afternoon);
                }
                break;
            case 5:
                {
                    UpdateGlobalTime(GlobalTime.Afternoon);
                }
                break;
            case 6:
                {
                    WorldEventManager.Instance.StartDusk();
                    UpdateGlobalTime(GlobalTime.Dusk);
                }
                break;
            case 7:
                {
                    WorldEventManager.Instance.StartEvening();
                    UpdateGlobalTime(GlobalTime.Evening);
                }
                break;
            case 8:
                {
                    UpdateGlobalTime(GlobalTime.Evening);
                }
                break;
            case 9:
                {
                    UpdateGlobalTime(GlobalTime.Evening);
                }
                break;
        }
        MessageBroker.Default.Publish(new GameEvent.GameEvent_All_UpdateHour()
        {
            hour = hour,
            hourPerDay = gameNetManager.int_HourPerDay,
            day = day,
            gameTime = hour + day * gameNetManager.int_HourPerDay,
            now = GlobalTimeNow
        });
        if (gameNetManager.Object.HasStateAuthority)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_UpdateHour()
            {
                hour = hour,
                hourPerDay = gameNetManager.int_HourPerDay,
                day = day,
                gameTime = hour + day * gameNetManager.int_HourPerDay,
                now = GlobalTimeNow
            });
        }
    }
    public void UpdateGlobalTime(GlobalTime globalTime)
    {
        //Debug.Log("当前时间" + globalTime);
        GlobalTimeNow = globalTime;
        WorldLightManager.Instance.ChangeWhiteBalance(GlobalTimeNow, WeatherNow);
        WorldLightManager.Instance.ChangeLight(GlobalTimeNow, WeatherNow);
    }
    public void UpdateWeather(Weather weather)
    {
        WeatherNow = weather;
        WorldLightManager.Instance.ChangeWhiteBalance(GlobalTimeNow, WeatherNow);
        WorldLightManager.Instance.ChangeLight(GlobalTimeNow, WeatherNow);
    }
    public void GetTime_Detail(out int day, out int hour, out GlobalTime globalTime)
    {
        day = gameNetManager ? gameNetManager.Day : 0;
        hour = gameNetManager ? gameNetManager.Hour : 0;
        globalTime = GlobalTimeNow;
    }
    public void GetTime_NowHour(out int now)
    {
        int day = gameNetManager ? gameNetManager.Day : 0;
        int hour = gameNetManager ? gameNetManager.Hour : 0;
        int hourPerDay = gameNetManager ? gameNetManager.int_HourPerDay : 10;
        now = day * hourPerDay + hour;
    }
    public void GetTime_NowSecond(out int now)
    {
        int second = gameNetManager ? gameNetManager.Second : 0;
        int day = gameNetManager ? gameNetManager.Day : 0;
        int hour = gameNetManager ? gameNetManager.Hour : 0;
        int secondsPerHour = gameNetManager ? gameNetManager.int_SecondsPerHour : 120;
        int hourPerDay = gameNetManager ? gameNetManager.int_HourPerDay : 10;
        now = day * hourPerDay * secondsPerHour + hour * secondsPerHour + second;
    }
}
/// <summary>
/// 时间
/// </summary>
public enum GlobalTime
{
    /// <summary>
    /// 0
    /// </summary>
    Morning,
    /// <summary>
    /// 1-2
    /// </summary>
    Forenoon,
    /// <summary>
    /// 3
    /// </summary>
    Highnoon,
    /// <summary>
    /// 4-5
    /// </summary>
    Afternoon,
    /// <summary>
    /// 6
    /// </summary>
    Dusk,
    /// <summary>
    /// 7-9
    /// </summary>
    Evening,
}
