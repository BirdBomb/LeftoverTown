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
    #region 随机数
    private static readonly System.Random random = new System.Random();
    private static readonly object randomLock = new object(); // 线程安全（虽然Unity单线程）
    #endregion
    #region 雨滴效果缓存
    private readonly string[] waterDropPaths = new string[]
    {
        "Effect/Effect_WaterDrop_0",
        "Effect/Effect_WaterDrop_1"
    };
    #endregion
    public void Init()
    {
    }
    private void FixedUpdate()
    {
        if (weather_Now == Weather.Rain)
        {
            Ruin(Time.fixedDeltaTime);
        }
        else
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
                    //particleSystem_Fierce.Stop();
                    //particleSystem_Rain.Stop();
                    //particleSystem_Breeze.Play();
                    particleSystem_Fierce.Stop();
                    particleSystem_Breeze.Stop();
                    particleSystem_Rain.Play();
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
        float x = (NextRandom(-100, 100) * 0.01f * ruin_RangeX) + transform.position.x;
        float y = (NextRandom(-100, 100) * 0.01f * ruin_RangeY) + transform.position.y;
        Vector2 position = new Vector2(x, y);
        LiquidManager.Instance?.AddWave(position);
        GameObject waterDrop = PoolManager.Instance?.GetEffectObj(waterDropPaths[NextRandom(0, waterDropPaths.Length)]);
        if (waterDrop != null)
        {
            // 随机翻转
            int flip = NextRandom(0, 2);
            waterDrop.transform.localScale = new Vector3(1 - (2 * flip), 1, 1);
            waterDrop.transform.position = position;
            waterDrop.transform.localRotation = Quaternion.identity;
        }
    }
    #endregion
    private int NextRandom(int min, int max)
    {
        lock (randomLock)
        {
            return random.Next(min, max);
        }
    }
}
public enum Weather
{
    Default,
    Rain,
    Fierce,
}