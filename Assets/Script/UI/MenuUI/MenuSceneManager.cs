using UniRx;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;
using DG.Tweening;
using System;
/// <summary>
/// 菜单场景管理
/// </summary>
public class MenuSceneManager : MonoBehaviour
{
    public MapManager mapManager;
    private void Start()
    {
        StartCoroutine(Init());
        InvokeRepeating("TimeLoop", 3,6);
    }
    public IEnumerator Init()
    {
        yield return new WaitForSeconds(1);
        CreateScene();
        ChangeWeather(Weather.Rain);
    }
    private void TimeLoop()
    {
        Array values = Enum.GetValues(typeof(GlobalTime));
        // 随机选择一个
        GlobalTime randomState = (GlobalTime)values.GetValue(new System.Random().Next(values.Length));
        UpdateGlobalTime(randomState);
    }
    private void FixedUpdate()
    {
        if (weather_Now == Weather.Rain)
        {
            Ruin(Time.fixedDeltaTime);
        }
    }
    #region//生成地形
    private void CreateScene()
    {
        IslandAreaConfig config;
        config = new IslandAreaConfig()
        {
            island_Center = Vector2.zero,
            island_WholeSize = 20,
            island_FillSize = 10,
            noise_Offset = new Vector2(UnityEngine.Random.Range(0f, 0.5f), UnityEngine.Random.Range(0f, 0.5f)),
            noise_Sacle = 5,
        };
        CreateRandomArea(config, (pos, val) =>
        {
            if (val < 0.5f)
            {
                mapManager.CreateGround(1001, pos);
            }
            else if (val < 0.7f)
            {
                mapManager.CreateGround(1000, pos);
            }
            else
            {
                mapManager.CreateGround(9000, pos);
            }
            mapManager.DrawGround(pos, 2, 2);
        });
        CreateMapMod(Vector2Int.zero, MapModConfigData.GetMapModConfig(0));

    }
    public struct IslandAreaConfig
    {
        /// <summary>
        /// 岛屿整体尺寸
        /// </summary>
        public float island_WholeSize;
        /// <summary>
        /// 岛屿完全填充尺寸
        /// </summary>
        public float island_FillSize;
        /// <summary>
        /// 岛屿中心
        /// </summary>
        public Vector2 island_Center;
        /// <summary>
        /// 噪声尺寸 1-20(边缘毛躁半径)
        /// </summary>
        public float noise_Sacle;
        /// <summary>
        /// 噪声偏移(0,1)
        /// </summary>
        public Vector2 noise_Offset;
    }
    private void CreateRandomArea(IslandAreaConfig islandArea, Action<Vector3Int, float> action)
    {
        float reciprocal_size = 1f / islandArea.island_WholeSize;
        float size_Empty = islandArea.island_WholeSize - islandArea.island_FillSize;
        int from_x = (int)(islandArea.island_Center.x - islandArea.island_WholeSize);
        int to_x = (int)(islandArea.island_Center.x + islandArea.island_WholeSize);
        int from_y = (int)(islandArea.island_Center.y - islandArea.island_WholeSize);
        int to_y = (int)(islandArea.island_Center.y + islandArea.island_WholeSize);
        Debug.Log(from_x + "/" + to_x);
        Debug.Log(from_y + "/" + to_y);

        /*噪声图采样中心*/
        Vector2 noise_Center = islandArea.noise_Offset;
        /*噪声图采样尺寸(1-0)*/
        float noise_Scale = (1f / islandArea.noise_Sacle);
        /*到中心的距离*/
        float distance;
        /*柏林噪声(0-1)*/
        float perlinNoise;
        /*距离值(0-1)*/
        float distanceValue;
        /*实际噪声(0-1)*/
        float realNoise;
        for (int i = from_x; i < to_x; i++)
        {
            for (int j = from_y; j < to_y; j++)
            {
                int x = i;
                int y = j;
                distance = Vector2.Distance(new Vector2(x, y), islandArea.island_Center);
                if (distance > islandArea.island_FillSize)
                {
                    /*0-1*/
                    distanceValue = Mathf.Lerp(0, 1, (islandArea.island_WholeSize - distance) / size_Empty);
                    /*0-1*/
                    perlinNoise = Mathf.PerlinNoise(noise_Center.x + (x + from_x) * noise_Scale, noise_Center.y + (y + from_y) * noise_Scale);
                    realNoise = Mathf.Lerp(perlinNoise, distanceValue, Mathf.Abs(distanceValue - 0.5f) * 2);
                }
                else
                {
                    realNoise = 1;
                }
                action.Invoke(new Vector3Int(x, y, 0), realNoise);
            }
        }
    }
    private void CreateMapMod(Vector2Int center, MapModConfig mapModConfig)
    {
        MapModData data_Map = Resources.Load<MapModData>($"MapModData/MapModData{mapModConfig.MapMod_ID}");
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapFloor)
        {
            mapManager.CreateGround(pair.value, (Vector3Int)(pair.key + center));
            mapManager.DrawGround((Vector3Int)(pair.key + center), 2, 2);
        }
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapBuilding)
        {
            mapManager.CreateBuilding(pair.value, (Vector3Int)(pair.key + center),out _);
            mapManager.DrawBuilding((Vector3Int)(pair.key + center), 2, 2);
        }
    }

    #endregion
    #region//光效
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
    /// <summary>
    /// 饱和度
    /// </summary>
    private ColorAdjustments _colorAdjustments;
    private float temperature;
    private float tint;

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
    public void UpdateGlobalTime(GlobalTime globalTime)
    {
        GlobalTimeNow = globalTime;
        ChangeWhiteBalance();
        ChangeLight();
    }
    public void UpdateWeather(Weather weather)
    {
        WeatherNow = weather;
        ChangeWhiteBalance();
        ChangeLight();
    }

    /// <summary>
    /// 更改平白横
    /// </summary>
    private void ChangeWhiteBalance()
    {
        /*蓝-橙*/
        float temp_temperature = 0;
        /*绿-紫*/
        float temp_tint = 0;
        switch (GlobalTimeNow)
        {
            case GlobalTime.Morning:
                {
                    temp_temperature = 25;
                    temp_tint = -50;
                }
                break;
            case GlobalTime.Forenoon:
                {
                    temp_temperature = -50;
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
                    temp_tint = -25;
                }
                break;
            case GlobalTime.Dusk:
                {
                    temp_temperature = 100;
                    temp_tint = 25;
                }
                break;
            case GlobalTime.Evening:
                {
                    temp_temperature = 50;
                    temp_tint = 75;
                }
                break;
        }
        switch (WeatherNow)
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

        DOTween.To(() => temperature, x => temperature = x, temp_temperature, 5f).OnUpdate(() =>
        {
            _whiteBalance.temperature.Override(temperature);
        });
        DOTween.To(() => temperature, x => tint = x, temp_tint, 5f).OnUpdate(() =>
        {
            _whiteBalance.tint.Override(tint);
        });

    }
    /// <summary>
    /// 更改光照
    /// </summary>
    private void ChangeLight()
    {
        Color temp_lightColor = new Color(1, 1, 1, 1);
        float temp_lightIntensity = 0;
        switch (GlobalTimeNow)
        {
            case GlobalTime.Morning:
                {
                    temp_lightIntensity = 0.9f;
                    temp_lightColor = new Color(0.75f, 1, 1, 1);
                }
                break;
            case GlobalTime.Forenoon:
                {
                    temp_lightIntensity = 0.9f;
                    temp_lightColor = new Color(1, 1, 0.75f, 1);
                }
                break;
            case GlobalTime.Highnoon:
                {
                    temp_lightIntensity = 1f;
                    temp_lightColor = new Color(1, 1, 0.7f, 1);
                }
                break;
            case GlobalTime.Afternoon:
                {
                    temp_lightIntensity = 0.9f;
                    temp_lightColor = new Color(1, 1, 0.75f, 1);
                }
                break;
            case GlobalTime.Dusk:
                {
                    temp_lightIntensity = 0.8f;
                    temp_lightColor = new Color(1, 0.75f, 0.5f, 1);
                }
                break;
            case GlobalTime.Evening:
                {
                    temp_lightIntensity = 0.6f;
                    temp_lightColor = new Color(0.75f, 0.5f, 1, 1);
                }
                break;
        }
        switch (WeatherNow)
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
        DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, temp_lightIntensity, 5f);
        DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, temp_lightColor, 5f);
    }

    #endregion
    #region//环境
    [SerializeField, Header("灰尘粒子")]
    private ParticleSystem particleSystem_Dust;
    [SerializeField, Header("微风粒子")]
    private ParticleSystem particleSystem_Breeze;
    [SerializeField, Header("狂风粒子")]
    private ParticleSystem particleSystem_Fierce;
    [SerializeField, Header("下雨粒子")]
    private ParticleSystem particleSystem_Rain;
    private Weather weather_Now;
    public void ChangeWeather(Weather weather)
    {
        weather_Now = weather;
        Debug.Log("天气_" + weather_Now);
        switch (weather)
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
            for (int i = 0; i < ruin_Count; i++)
            {
                CreateRaindrop();
            }
        }
    }
    private void CreateRaindrop()
    {
        float x = new System.Random().Next(-100, 100) * 0.01f * ruin_RangeX + transform.position.x;
        float y = new System.Random().Next(-100, 100) * 0.01f * ruin_RangeY + transform.position.y;
        LiquidManager.Instance.AddWave(new Vector2(x, y), null, Vector2.one * 5);
        //GameObject muzzleFire101 = PoolManager.Instance.GetEffectObj("Effect/Effect_WaterDrop_" + new System.Random().Next(0, 2));
        //muzzleFire101.transform.localScale = new Vector3(1 - (2 * new System.Random().Next(0, 2)), 1, 1);
        //muzzleFire101.transform.position = new Vector2(x, y);
        //muzzleFire101.transform.localRotation = Quaternion.identity;
    }
    #endregion

    #endregion
}
