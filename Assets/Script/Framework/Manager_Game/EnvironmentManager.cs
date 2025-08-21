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
    [SerializeField, Header("下雨粒子")]
    private ParticleSystem particleSystem_Ruin;
    private Weather weather_Now;
    public void Init()
    {
    }
    private void FixedUpdate()
    {
        if (weather_Now == Weather.Ruin)
        {
            Ruin(Time.fixedDeltaTime);
        }
    }
    public void ChangeWeather(Weather weather)
    {
        weather_Now = weather;
        Debug.Log("天气_" + weather_Now);

    }

    #region//雨
    /// <summary>
    /// 雨滴间隔
    /// </summary>
    private float ruin_Step = 0.1f;
    /// <summary>
    /// 雨滴数量
    /// </summary>
    private int ruin_Count = 2;
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
    }
    #endregion
}
public enum Weather
{
    Default,
    Ruin
}