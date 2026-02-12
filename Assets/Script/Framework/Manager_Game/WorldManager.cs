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
    public void Init()
    {
        if (volume)
        {
            volume.profile.TryGet(out _whiteBalance);
            volume.profile.TryGet(out _colorAdjustments);
        }
        AudioManager.Instance.PlayMusic(9000, 0, true);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_BindLocalPlayer>().Subscribe(_ =>
        {
            playerCoreLocal = _.playerCore;
        }).AddTo(this);
    }
    #region//世界角色
    public PlayerCoreLocal playerCoreLocal;
    public List<ActorManager> actors = new List<ActorManager>();
    public void AddActor(ActorManager actorManager)
    {
        actors.Add(actorManager);
    }
    public void SubActor(ActorManager actorManager)
    {
        actors.Remove(actorManager);
    }
    public bool FindPlayer(out ActorManager player)
    {
        for (int i = 0; i < actors.Count; i++)
        {
            if (actors[i] != null && actors[i].actorAuthority.isPlayer)
            {
                player = actors[i];
                return true;
            }
        }
        player = null;
        return false;
    }
    public bool FindActor(out ActorManager actor)
    {
        if (actors.Count > 0)
        {
            actor = actors[new System.Random().Next(0, actors.Count)];
            return true;
        }
        else
        {
            actor = null;
            return false;
        }
    }
    #endregion
    #region//世界时间
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
    public void UpdateHour(int hour, int day)
    {
        Debug.Log("当前时间" + hour + "/"+ day);
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
                    UpdateGlobalTime(GlobalTime.Highnoon);
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
                    Dusk();
                    UpdateGlobalTime(GlobalTime.Dusk);
                }
                break;
            case 7:
                {
                    Evening();
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
        Debug.Log("当前时间" + globalTime);
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
    public void GetTime(out int day, out int hour, out GlobalTime globalTime)
    {
        day = gameNetManager ? gameNetManager.Day : 0;
        hour = gameNetManager ? gameNetManager.Hour : 0;
        globalTime = GlobalTimeNow;
    }
    #endregion
    #region//世界光源
    [SerializeField, Header("后处理")]
    private Volume volume;
    [SerializeField, Header("太阳光")]
    private Light2D light2D_Spot;
    [Range(0f, 1f), Header("太阳光强度")]
    public float light2D_Spot_Intensity = 1;
    [SerializeField, Header("全局光")]
    private Light2D light2D_Global;
    [SerializeField, Header("玩家地面光")]
    public Light2D light2D_Ground;
    [SerializeField, Header("玩家地上光")]
    public Light2D light2D_OnGround;
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
    private float temperature;
    private float tint;

    public int int_Distance = 10;
    public void UpdateDistance(int distance)
    {
        light2D_Spot.pointLightOuterRadius = distance;
        light2D_Spot.pointLightInnerRadius = distance - 5;
        int_Distance = distance;
    }
    public void ChangeSaturability(float val)
    {
        _colorAdjustments.saturation.Override(val);
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
        switch (WeatherNow)
        {
            case Weather.Default:
                {

                }
                break;
            case Weather.Rain:
                {
                    temp_lightIntensity = Mathf.Lerp(0.5f, temp_lightIntensity, 0.5f);
                    temp_lightColor = new Color(Mathf.Lerp(0.5f, temp_lightColor.r, 0.5f),Mathf.Lerp(0.5f, temp_lightColor.g, 0.5f),Mathf.Lerp(1, temp_lightColor.b, 0.5f),temp_lightColor.a);
                }
                break;
            case Weather.Fierce:
                {
                    temp_lightIntensity = Mathf.Lerp(0.5f, temp_lightIntensity, 0.5f);
                    temp_lightColor = new Color(Mathf.Lerp(0.5f, temp_lightColor.r, 0.5f), Mathf.Lerp(0.5f, temp_lightColor.g, 0.5f), Mathf.Lerp(1, temp_lightColor.b, 0.5f), temp_lightColor.a);
                }
                break;
        }
        temp_lightIntensity = Mathf.Lerp(0, temp_lightIntensity, light2D_Spot_Intensity);
        DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, temp_lightIntensity, 5f);
        DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, temp_lightColor, 5f);
        DOTween.To(() => light2D_Ground.color, x => light2D_Ground.color = x, temp_lightColor, 5f);
        DOTween.To(() => light2D_OnGround.color, x => light2D_OnGround.color = x, temp_lightColor, 5f);
    }
    #endregion
    #region//世界事件
    /// <summary>
    /// 黄昏
    /// </summary>
    public void Dusk()
    {

    }
    /// <summary>
    /// 夜晚
    /// </summary>
    public void Evening()
    {
        ZombieComing();
    }
    /// <summary>
    /// 初级僵尸敌袭事件
    /// </summary>
    public void ZombieComing()
    {
        if (gameNetManager.Object.HasStateAuthority)
        {
            for(int i = 0; i<actors.Count; i++)
            {
                if (actors[i].actorAuthority.isPlayer)
                {
                    if (GetEnemyPos(actors[i].pathManager.vector3Int_CurPos, 15, out Vector3Int enemyPos))
                    {
                        //MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
                        //{
                        //    name = "Actor/Zombie_Spray",
                        //    pos = enemyPos,
                        //    callBack = ((actor) =>
                        //    {
                        //        actor.GetComponent<ActorManager>().brainManager.SetActivity(actors[i].pathManager.vector3Int_CurPos);
                        //    })
                        //});
                        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
                        {
                            name = "Actor/Zombie_Runner",
                            pos = enemyPos,
                            callBack = ((actor) =>
                            {
                                actor.GetComponent<ActorManager>().brainManager.State_SetActivityPos(actors[i].pathManager.vector3Int_CurPos);
                            })
                        });
                    }
                    else
                    {
                        Debug.Log("未找到合适位置");
                    }
                }
            }
        }
    }
    /// <summary>
    /// 敌人位置
    /// </summary>
    /// <param name="enemyPos"></param>
    /// <returns></returns>
    public bool GetEnemyPos(Vector3Int targetPos, int distance, out Vector3Int enemyPos)
    {
        List<Vector3Int> postions = GetExactRadiusPoints(targetPos, distance);
        for (int k = 0; k < postions.Count; k++)
        {
            if (!MapManager.Instance.GetBuilding(postions[k], out _) && MapManager.Instance.GetGround(postions[k], out GroundTile ground))
            {
                if (ground.tileID < 2000)
                {
                    enemyPos = postions[k];
                    return true;
                }
            }
        }
        enemyPos = Vector3Int.zero;
        return false;
    }
    List<Vector3Int> GetExactRadiusPoints(Vector3Int center, int radius)
    {
        List<Vector3Int> points = new List<Vector3Int>();
        int radiusSquared = radius * radius;

        // 只在可能包含解的x范围内搜索
        for (int x = -radius; x <= radius; x++)
        {
            // 计算y² = r² - x²
            int remaining = radiusSquared - x * x;

            // 只有当remaining是完全平方数时才可能有整数解
            if (remaining >= 0)
            {
                int y = (int)Mathf.Sqrt(remaining);

                // 检查是否精确匹配
                if (y * y == remaining && y != 0)
                {
                    // 添加四个象限的点
                    points.Add(new Vector3Int(center.x + x, center.y + y));
                    points.Add(new Vector3Int(center.x + x, center.y - y));

                    // 避免重复添加当x=0或y=0的点
                    if (x != 0)
                    {
                        points.Add(new Vector3Int(center.x + y, center.y + x));
                        points.Add(new Vector3Int(center.x - y, center.y + x));
                    }
                }
            }
        }

        return points;
    }
    #endregion
    #region//网络组件
    [Header("网络组件")]
    public GameNetManager gameNetManager;
    #endregion
}
/// <summary>
/// 
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
[Serializable]
public struct LightData
{
    [Range(0,1f)]
    public float lightIntensity;
    public Color lightColor;
}