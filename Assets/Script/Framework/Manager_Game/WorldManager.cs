using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering;
using UniRx;

public class WorldManager : SingleTon<WorldManager>, ISingleTon
{
    private GlobalTime globalTimeNow;
    private GlobalTime GlobalTimeNow
    {
        get { return globalTimeNow; }
        set { globalTimeNow = value; }
    }

    public void Init()
    {
        volume.profile.TryGet(out _whiteBalance);
        AudioManager.Instance.PlayMusic(9000, 0.5f, true);
    }
    public void UpdateSecond(int second, int hour, int day)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_All_UpdateSecond()
        {
            second = second,
            hour = hour,
            day = day,
            now = GlobalTimeNow
        });
    }
    public void UpdateHour(int hour,int day)
    {
        switch (hour)
        {
            case 0:
                {
                    UpdateGlobalTime(GlobalTime.Morning);
                }
                break;
            case 1:
                {
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
                    UpdateGlobalTime(GlobalTime.HighNoon);
                }
                break;
            case 4:
                {
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
                    UpdateGlobalTime(GlobalTime.Dusk);
                }
                break;
            case 7:
                {
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
            day = day,
            now = GlobalTimeNow
        });
    }
    public void UpdateGlobalTime(GlobalTime globalTime)
    {
        if (globalTime == GlobalTime.Morning)
        {
            DOTween.To(() => temperature, x => temperature = x, 25, 5f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => temperature, x => tint = x, 0, 5f).OnUpdate(() =>
            {
                _whiteBalance.tint.Override(tint);
            });
            DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.75f, 5f);
            DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(0.75f, 1, 1, 1), 5f);
        }
        else if (globalTime == GlobalTime.Forenoon)
        {
            DOTween.To(() => temperature, x => temperature = x, 0, 5f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => temperature, x => tint = x, 0, 5f).OnUpdate(() =>
            {
                _whiteBalance.tint.Override(tint);
            });
            DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.75f, 5f);
            DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(1, 1, 0.75f, 1), 5f);
        }
        else if (globalTime == GlobalTime.HighNoon)
        {
            DOTween.To(() => temperature, x => temperature = x, 25, 5f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => temperature, x => tint = x, 25, 5f).OnUpdate(() =>
            {
                _whiteBalance.tint.Override(tint);
            });
            DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 1f, 5f);
            DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(1, 1, 0.7f, 1), 5f);
        }
        else if (globalTime == GlobalTime.Afternoon)
        {
            DOTween.To(() => temperature, x => temperature = x, 25, 5f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => temperature, x => tint = x, 25, 5f).OnUpdate(() =>
            {
                _whiteBalance.tint.Override(tint);
            });
            DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.75f, 5f);
            DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(1, 1, 0.75f, 1), 5f);
        }
        else if (globalTime == GlobalTime.Dusk)
        {
            DOTween.To(() => temperature, x => temperature = x, 75, 5f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => temperature, x => tint = x, 25, 5f).OnUpdate(() =>
            {
                _whiteBalance.tint.Override(tint);
            });
            DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.75f, 5f);
            DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(1, 0.75f, 0.5f, 1), 5f);
        }
        else if (globalTime == GlobalTime.Evening)
        {
            DOTween.To(() => temperature, x => temperature = x, 50, 5f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => temperature, x => tint = x, 50, 5f).OnUpdate(() =>
            {
                _whiteBalance.tint.Override(tint);
            });
            DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.5f, 5f);
            DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(0.75f, 0.5f, 1, 1), 5f);
        }
        GlobalTimeNow = globalTime;
    }
    #region//世界光源
    [SerializeField, Header("后处理")]
    private Volume volume;
    [SerializeField, Header("太阳光")]
    private Light2D light2D_Spot;
    [SerializeField, Header("全局光")]
    private Light2D light2D_Global;
    /// <summary>
    /// 平白横
    /// </summary>
    private WhiteBalance _whiteBalance;
    private float temperature;
    private float tint;

    public int int_Distance = 10;
    public void UpdateDistance(int distance)
    {
        light2D_Spot.pointLightOuterRadius = distance;
        light2D_Spot.pointLightInnerRadius = distance - 5;
        int_Distance = distance;
    }
    #endregion
}
public enum GlobalTime
{
    Morning,
    Forenoon,
    HighNoon,
    Afternoon,
    Dusk,
    Evening,
}