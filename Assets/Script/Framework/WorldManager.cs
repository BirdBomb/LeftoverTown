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
    [SerializeField]
    private Light2D globalLight;
    [SerializeField]
    private Volume volume;
    /// <summary>
    /// Æ½°×ºá
    /// </summary>
    private WhiteBalance _whiteBalance;
    private float temperature;
    private float tint;
    private GlobalTime globalTimeNow;
    private GlobalTime GlobalTimeNow
    {
        get { return globalTimeNow; }
        set { globalTimeNow = value; }
    }

    public void Init()
    {
        volume.profile.TryGet(out _whiteBalance);
    }
    public void UpdateTime(int hour,int date)
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
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_TimeChange()
        {
            hour = hour,
            date = date,
            now = GlobalTimeNow
        });
    }
    public void UpdateGlobalTime(GlobalTime globalTime)
    {
        if (globalTime == GlobalTime.Morning)
        {
            //DOTween.To(() => temperature, x => temperature = x, -10, 10f).OnUpdate(() =>
            //{
            //    _whiteBalance.temperature.Override(temperature);
            //});
            //DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.75f, 10f);
            //DOTween.To(() => globalLight.color, x => globalLight.color = x, new Color(1, 225f / 255f, 200f / 255f, 1), 10f);
        }
        if (globalTime == GlobalTime.HighNoon)
        {
            DOTween.To(() => temperature, x => temperature = x, 10, 10f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 1f, 10f);
            DOTween.To(() => globalLight.color, x => globalLight.color = x, new Color(1, 1, 1, 1), 10f);
        }
        if (globalTime == GlobalTime.Dusk)
        {
            //DOTween.To(() => temperature, x => temperature = x, 20, 10f).OnUpdate(() =>
            //{
            //    _whiteBalance.temperature.Override(temperature);
            //});
            //DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.75f, 10f);
            //DOTween.To(() => globalLight.color, x => globalLight.color = x, new Color(1, 160f / 255f, 70f / 255f, 1), 10f);
        }
        if (globalTime == GlobalTime.Evening)
        {
            //DOTween.To(() => temperature, x => temperature = x, -20f, 10f).OnUpdate(() =>
            //{
            //    _whiteBalance.temperature.Override(temperature);
            //});
            //DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.5f, 10f);
            //DOTween.To(() => globalLight.color, x => globalLight.color = x, new Color(140f / 255f, 100f / 255f, 170f / 255f, 1), 10f);
        }
        GlobalTimeNow = globalTime;
    }
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