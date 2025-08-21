using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using DG.Tweening;
using UnityEngine.Rendering;
using UniRx;

/// <summary>
/// 世界管理器
/// 用于管理所有生物/时间/光照/事件
/// </summary>
public class WorldManager : SingleTon<WorldManager>, ISingleTon
{
    private GlobalTime globalTimeNow;
    public GlobalTime GlobalTimeNow
    {
        get { return globalTimeNow; }
        set { globalTimeNow = value; }
    }

    public void Init()
    {
        volume.profile.TryGet(out _whiteBalance);
        volume.profile.TryGet(out _colorAdjustments);
        AudioManager.Instance.PlayMusic(9000, 0, true);
    }
    #region//世界角色
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

        if (globalTime == GlobalTime.Morning)
        {
            PlayMorningLight();
        }
        else if (globalTime == GlobalTime.Forenoon)
        {
            PlayForenoonLight();
        }
        else if (globalTime == GlobalTime.Highnoon)
        {
            PlayHighnoonLight();
        }
        else if (globalTime == GlobalTime.Afternoon)
        {
            PlayAfternoonLight();
        }
        else if (globalTime == GlobalTime.Dusk)
        {
            PlayDuskLight();
        }
        else if (globalTime == GlobalTime.Evening)
        {
            PlayEveningLight();
        }
        GlobalTimeNow = globalTime;
    }

    #endregion
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
    private void PlayMorningLight()
    {
        DOTween.To(() => temperature, x => temperature = x, 25, 5f).OnUpdate(() =>
        {
            _whiteBalance.temperature.Override(temperature);
        });
        DOTween.To(() => temperature, x => tint = x, 0, 5f).OnUpdate(() =>
        {
            _whiteBalance.tint.Override(tint);
        });
        DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.9f, 5f);
        DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(0.75f, 1, 1, 1), 5f);
    }
    private void PlayForenoonLight()
    {
        DOTween.To(() => temperature, x => temperature = x, 0, 5f).OnUpdate(() =>
        {
            _whiteBalance.temperature.Override(temperature);
        });
        DOTween.To(() => temperature, x => tint = x, 0, 5f).OnUpdate(() =>
        {
            _whiteBalance.tint.Override(tint);
        });
        DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.9f, 5f);
        DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(1, 1, 0.75f, 1), 5f);

    }
    private void PlayHighnoonLight()
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
    private void PlayAfternoonLight()
    {
        DOTween.To(() => temperature, x => temperature = x, 25, 5f).OnUpdate(() =>
        {
            _whiteBalance.temperature.Override(temperature);
        });
        DOTween.To(() => temperature, x => tint = x, 25, 5f).OnUpdate(() =>
        {
            _whiteBalance.tint.Override(tint);
        });
        DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.9f, 5f);
        DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(1, 1, 0.75f, 1), 5f);

    }
    private void PlayDuskLight()
    {
        DOTween.To(() => temperature, x => temperature = x, 75, 5f).OnUpdate(() =>
        {
            _whiteBalance.temperature.Override(temperature);
        });
        DOTween.To(() => temperature, x => tint = x, 25, 5f).OnUpdate(() =>
        {
            _whiteBalance.tint.Override(tint);
        });
        DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.8f, 5f);
        DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(1, 0.75f, 0.5f, 1), 5f);

    }
    private void PlayEveningLight()
    {
        DOTween.To(() => temperature, x => temperature = x, 50, 5f).OnUpdate(() =>
        {
            _whiteBalance.temperature.Override(temperature);
        });
        DOTween.To(() => temperature, x => tint = x, 50, 5f).OnUpdate(() =>
        {
            _whiteBalance.tint.Override(tint);
        });
        DOTween.To(() => light2D_Spot.intensity, x => light2D_Spot.intensity = x, 0.6f, 5f);
        DOTween.To(() => light2D_Spot.color, x => light2D_Spot.color = x, new Color(0.75f, 0.5f, 1, 1), 5f);

    }
    public void ChangeSaturability(float val)
    {
        _colorAdjustments.saturation.Override(val);
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
        if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
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
                                actor.GetComponent<ActorManager>().brainManager.SetActivity(actors[i].pathManager.vector3Int_CurPos);
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
}
public enum GlobalTime
{
    Morning,
    Forenoon,
    Highnoon,
    Afternoon,
    Dusk,
    Evening,
}