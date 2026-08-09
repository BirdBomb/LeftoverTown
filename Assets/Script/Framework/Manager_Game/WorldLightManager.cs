using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System;

public class WorldLightManager : SingleTon<WorldLightManager>, ISingleTon
{
    public void Init()
    {
        if (volume)
        {
            volume.profile.TryGet(out _whiteBalance);
            volume.profile.TryGet(out _colorAdjustments);
            volume.profile.TryGet(out _vignette);
        }
    }
    [Header("后处理")]
    public Volume volume;
    [Header("太阳光")]
    public Light2D light2D_Sun;
    [Header("全局光")]
    public Light2D light2D_Global;
    [Header("玩家地面光")]
    public Light2D light2D_Ground;
    [Header("玩家地上光")]
    public Light2D light2D_OnGround;
    [Range(0f, 1f), Header("太阳光强度")]
    public float float_SunIntensity = 1;
    [Header("--光(早上)--")]
    public LightData lightData_Morning;
    [Header("--光(上午)--")]
    public LightData lightData_Forenoon;
    [Header("--光(正午)--")]
    public LightData lightData_Highnoon;
    [Header("--光(下午)--")]
    public LightData lightData_Afternoon;
    [Header("--光(黄昏)--")]
    public LightData lightData_Dusk;
    [Header("--光(晚上)--")]
    public LightData lightData_Evening;
    /// <summary>
    /// 平白横
    /// </summary>
    private WhiteBalance _whiteBalance;
    /// <summary>
    /// 饱和度
    /// </summary>
    private ColorAdjustments _colorAdjustments;
    /// <summary>
    /// 暗角
    /// </summary>
    private Vignette _vignette;
    private float float_temperature;
    private float float_tint;

    public int int_Distance = 10;
    public void UpdateSunLight(int distance)
    {
        //light2D_Sun.pointLightOuterRadius = Math.Max(0, distance);
        //light2D_Sun.pointLightInnerRadius = Math.Max(0, distance - 1);

        DOTween.To(() => light2D_Sun.pointLightOuterRadius, x => light2D_Sun.pointLightOuterRadius = x, Math.Max(0, distance), 10f);
        DOTween.To(() => light2D_Sun.pointLightInnerRadius, x => light2D_Sun.pointLightInnerRadius = x, Math.Max(0, distance - 5), 30f);

    }
    /// <summary>
    /// 更改饱和度
    /// </summary>
    public void ChangeSaturability(float val)
    {
        _colorAdjustments.saturation.Override(val);
    }
    /// <summary>
    /// 更改平白横
    /// </summary>
    public void ChangeWhiteBalance(GlobalTime globalTimeNow,Weather weatherNow)
    {
        /*蓝-橙*/
        float temp_temperature = 0;
        /*绿-紫*/
        float temp_tint = 0;
        switch (globalTimeNow)
        {
            case GlobalTime.Morning:
                {
                    temp_temperature = 25;
                    temp_tint = 0;
                }
                break;
            case GlobalTime.Forenoon:
                {
                    temp_temperature = 0;
                    temp_tint = 0;
                }
                break;
            case GlobalTime.Highnoon:
                {
                    temp_temperature = 25;
                    temp_tint = 25;
                }
                break;
            case GlobalTime.Afternoon:
                {
                    temp_temperature = 25;
                    temp_tint = 25;
                }
                break;
            case GlobalTime.Dusk:
                {
                    temp_temperature = 75;
                    temp_tint = 25;
                }
                break;
            case GlobalTime.Evening:
                {
                    temp_temperature = 50;
                    temp_tint = 50;
                }
                break;
        }
        switch (weatherNow)
        {
            case Weather.Default:
                {

                }
                break;
            case Weather.Rain:
                {
                    temp_temperature = Mathf.Lerp(-100, temp_temperature, 0.5f);
                    temp_tint = Mathf.Lerp(50, temp_tint, 0.5f);
                }
                break;
            case Weather.Fierce:
                {
                    temp_temperature = Mathf.Lerp(-50, temp_temperature, 0.5f);
                }
                break;
        }

        DOTween.To(() => float_temperature, x => float_temperature = x, temp_temperature, 5f).OnUpdate(() =>
        {
            _whiteBalance.temperature.Override(float_temperature);
        });
        DOTween.To(() => float_temperature, x => float_tint = x, temp_tint, 5f).OnUpdate(() =>
        {
            _whiteBalance.tint.Override(float_tint);
        });

    }
    /// <summary>
    /// 更改光照
    /// </summary>
    public void ChangeLight(GlobalTime globalTimeNow, Weather weatherNow)
    {
        Color temp_lightColor = new Color(1, 1, 1, 1);
        float temp_lightIntensity = 0;
        switch (globalTimeNow)
        {
            case GlobalTime.Morning:
                {
                    temp_lightIntensity = lightData_Morning.lightIntensity;
                    temp_lightColor = lightData_Morning.lightColor;
                }
                break;
            case GlobalTime.Forenoon:
                {
                    temp_lightIntensity = lightData_Forenoon.lightIntensity;
                    temp_lightColor = lightData_Forenoon.lightColor;
                }
                break;
            case GlobalTime.Highnoon:
                {
                    temp_lightIntensity = lightData_Highnoon.lightIntensity;
                    temp_lightColor = lightData_Highnoon.lightColor;
                }
                break;
            case GlobalTime.Afternoon:
                {
                    temp_lightIntensity = lightData_Afternoon.lightIntensity;
                    temp_lightColor = lightData_Afternoon.lightColor;
                }
                break;
            case GlobalTime.Dusk:
                {
                    temp_lightIntensity = lightData_Dusk.lightIntensity;
                    temp_lightColor = lightData_Dusk.lightColor;
                }
                break;
            case GlobalTime.Evening:
                {
                    temp_lightIntensity = lightData_Evening.lightIntensity;
                    temp_lightColor = lightData_Evening.lightColor;
                }
                break;
        }
        switch (weatherNow)
        {
            case Weather.Default:
                {

                }
                break;
            case Weather.Rain:
                {
                    temp_lightIntensity = Mathf.Lerp(0.5f, temp_lightIntensity, 0.5f);
                    temp_lightColor = new Color(Mathf.Lerp(0.5f, temp_lightColor.r, 0.5f), Mathf.Lerp(0.5f, temp_lightColor.g, 0.5f), Mathf.Lerp(1, temp_lightColor.b, 0.5f), temp_lightColor.a);
                }
                break;
            case Weather.Fierce:
                {
                    temp_lightIntensity = Mathf.Lerp(0.5f, temp_lightIntensity, 0.5f);
                    temp_lightColor = new Color(Mathf.Lerp(0.5f, temp_lightColor.r, 0.5f), Mathf.Lerp(0.5f, temp_lightColor.g, 0.5f), Mathf.Lerp(1, temp_lightColor.b, 0.5f), temp_lightColor.a);
                }
                break;
        }
        temp_lightIntensity = Mathf.Lerp(0, temp_lightIntensity, float_SunIntensity);
        DOTween.To(() => light2D_Sun.intensity, x => light2D_Sun.intensity = x, temp_lightIntensity, 5f);
        DOTween.To(() => light2D_Sun.color, x => light2D_Sun.color = x, temp_lightColor, 5f);
        DOTween.To(() => light2D_Ground.color, x => light2D_Ground.color = x, temp_lightColor, 5f);
        DOTween.To(() => light2D_OnGround.color, x => light2D_OnGround.color = x, temp_lightColor, 5f);
    }
    bool bool_ShowVignette = false;
    /// <summary>
    /// 更改暗角
    /// </summary>
    /// <param name="show"></param>
    public void ChangeVignette(bool show)
    {
        if (bool_ShowVignette == show) return;
        bool_ShowVignette = show;
        float targetIntensity;
        if (show)
        {
            targetIntensity = 1;
        }
        else
        {
            targetIntensity = 0;
        }

        // 获取当前强度
        float startIntensity = _vignette.intensity.value;

        // DOTween.To: 在每帧中更新一个值
        DOTween.To(() => startIntensity, x => { _vignette.intensity.value = x; startIntensity = x; }, targetIntensity, 0.5f).SetEase(Ease.InOutQuad);
    }
}
[Serializable]
public struct LightData
{
    [Range(0, 1f)]
    public float lightIntensity;
    public Color lightColor;
}