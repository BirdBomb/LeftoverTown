using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering;
using UniRx;

public class WorldManager : SingleTon<FileManager>, ISingleTon
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
        
    }

    public void Start()
    {
        volume.profile.TryGet(out _whiteBalance);
        UpdateGlobalTime(GlobalTimeNow);
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            UpdateGlobalTime(GlobalTimeNow);
        }
    }
    private void UpdateGlobalTime(GlobalTime globalTime)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_TimeChange()
        {
            now = globalTime
        });

        if (globalTime == GlobalTime.Morning)
        {
            DOTween.To(() => temperature, x => temperature = x, -25, 10f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.75f, 10f);
        }
        if (globalTime == GlobalTime.HighNoon)
        {
            DOTween.To(() => temperature, x => temperature = x, 25, 10f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 1f, 10f);
        }
        if (globalTime == GlobalTime.Dusk)
        {
            DOTween.To(() => temperature, x => temperature = x, 99f, 10f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.75f, 10f);
        }
        if (globalTime == GlobalTime.Evening)
        {
            DOTween.To(() => temperature, x => temperature = x, -99f, 10f).OnUpdate(() =>
            {
                _whiteBalance.temperature.Override(temperature);
            });
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.25f, 10f);
        }
        GlobalTimeNow = GlobalTimeNow + 1;
    }
}
public enum GlobalTime
{
    Morning,
    HighNoon,
    Dusk,
    Evening,
}