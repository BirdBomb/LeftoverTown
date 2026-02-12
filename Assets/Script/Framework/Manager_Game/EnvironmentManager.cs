using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 环境管理器
/// </summary>
public class EnvironmentManager : SingleTon<EnvironmentManager>, ISingleTon
{
    [SerializeField, Header("灰尘粒子")]
    private ParticleSystem particleSystem_Dust;
    [SerializeField, Header("微风粒子")]
    private ParticleSystem particleSystem_Breeze;
    [SerializeField, Header("狂风粒子")]
    private ParticleSystem particleSystem_Fierce;
    [SerializeField, Header("下雨粒子")]
    private ParticleSystem particleSystem_Rain;
    private Weather weather_Now;
    public void Init()
    {
    }
    private void FixedUpdate()
    {
        if (weather_Now == Weather.Rain)
        {
            Ruin(Time.fixedDeltaTime);
        }
    }
    public void ChangeWeather(Weather weather)
    {
        weather_Now = weather;
        Debug.Log("天气_" + weather_Now);
        switch(weather)
        {
            case Weather.Default:
                {
                    particleSystem_Fierce.Stop();
                    particleSystem_Rain.Stop();
                    particleSystem_Breeze.Play();
                    break;
                }
            case Weather.Rain:
                {
                    particleSystem_Fierce.Stop();
                    particleSystem_Breeze.Stop();
                    particleSystem_Rain.Play();
                    break;
                }
            case Weather.Fierce:
                {
                    particleSystem_Breeze.Stop();
                    particleSystem_Rain.Stop();
                    particleSystem_Fierce.Play();
                    break;
                }
        }
    }

    #region//雨
    /// <summary>
    /// 雨滴间隔
    /// </summary>
    private float ruin_Step = 0.05f;
    /// <summary>
    /// 雨滴数量
    /// </summary>
    private int ruin_Count = 3;
    /// <summary>
    /// 下雨计时
    /// </summary>
    private float ruin_Timer;
    public float ruin_RangeX = 10;
    public float ruin_RangeY = 10;
    private void Ruin(float dt)
    {
        ruin_Timer += dt;
        if (ruin_Timer > ruin_Step)
        {
            ruin_Timer = 0;
            for(int i = 0; i < ruin_Count; i++)
            {
                CreateRaindrop();
            }
        }
    }
    private void CreateRaindrop()
    {
        float x = new System.Random().Next(-100, 100) * 0.01f * ruin_RangeX + transform.position.x;
        float y = new System.Random().Next(-100, 100) * 0.01f * ruin_RangeY + transform.position.y;
        LiquidManager.Instance.AddWave(new Vector2(x, y));
        GameObject muzzleFire101 = PoolManager.Instance.GetEffectObj("Effect/Effect_WaterDrop_" + new System.Random().Next(0, 2));
        muzzleFire101.transform.localScale = new Vector3(1 - (2 * new System.Random().Next(0, 2)), 1, 1);
        muzzleFire101.transform.position = new Vector2(x, y);
        muzzleFire101.transform.localRotation = Quaternion.identity;
    }
    #endregion
}
public enum Weather
{
    Default,
    Rain,
    Fierce,
}