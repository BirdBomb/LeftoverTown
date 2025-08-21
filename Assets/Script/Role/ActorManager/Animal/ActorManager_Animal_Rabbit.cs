using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using static Fusion.Allocator;
using static GameEvent;

public class ActorManager_Animal_Rabbit : ActorManager
{
    [Header("兔子配置")]
    public ActorConfig_Rabbit config;
    private float float_ThinkCD = 2;
    private float float_ThinkTimer;
    private GlobalTime globalTime_Cur;

    #region//初始化
    public override void AllClient_Init()
    {
        statusManager.statusType = config.status_Type;
        base.AllClient_Init();
    }
    public override void State_Init()
    {
        float_ThinkCD += new System.Random().Next(0, 100) * 0.01f;
        float_ThinkTimer = float_ThinkCD;
        State_InitNPCData();
        base.State_Init();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void State_InitNPCData()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < config.lootInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.lootInfos_Base[i].CountMin, config.lootInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.lootInfos_Base[i].ID, (short)count);
            itemDatas.Add(item);
        }
        for (int i = 0; i < config.lootInfos_Extra.Count; i++)
        {
            int random = new System.Random().Next(0, 1000);
            if (random <= config.lootInfos_Extra[i].Weight)
            {
                ItemData item = itemManager.CreateItemData(config.lootInfos_Extra[i].ID, (short)config.lootInfos_Extra[i].Count);
                itemDatas.Add(item);
            }
        }
        State_SetAbilityData(config.short_Hp, config.short_Armor, config.short_Resistance, config.short_MoveSpeed);
        actorNetManager.Local_SetLootItems(itemDatas);
    }
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }
    #endregion
    #region//时间周期
    public override void State_FixedUpdateNetwork(float dt)
    {
        float_ThinkTimer -= dt;
        if (float_ThinkTimer < 0)
        {
            float_ThinkTimer = float_ThinkCD;
            State_Think();
        }
        base.State_FixedUpdateNetwork(dt);
    }
    public override void State_CustomUpdate()
    {
        State_Threatened_Check();
        base.State_CustomUpdate();
    }
    #endregion
    #region//对外界的反应
    public override void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (actionManager.LookAt(who, config.float_ViewDistance))
            {
                State_Threatened_Run(who);
            }
        }
        base.State_Listen_MyselfHpChange(parameter, reason, id);
    }
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        globalTime_Cur = globalTime;
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    public override void State_Listen_RoleInView(ActorManager actor)
    {
        if (actor.statusManager.statusType != StatusType.Animal_Common)
        {
            brainManager.actorManagers_Nearby.Add(actor);
        }
        base.State_Listen_RoleInView(actor);
    }
    public override void State_Listen_RoleOutView(ActorManager actor)
    {
        if (actor.statusManager.statusType != StatusType.Animal_Common)
        {
            brainManager.actorManagers_Nearby.Remove(actor);
        }
        base.State_Listen_RoleInView(actor);
    }
    #endregion
    #region//闲置状态
    /// <summary>
    /// 思考
    /// </summary>
    public void State_Think()
    {
        if (globalTime_Cur != GlobalTime.Evening)
        {
            if (brainManager.actorManagers_ThreatenedTarget.Count == 0)
            {
                State_Think_Stroll(config.short_MoveStep);
            }
        }
        else
        {
            if (brainManager.actorManagers_ThreatenedTarget.Count == 0)
            {
                if (brainManager.homePostion.isValue)
                {
                    State_Think_GoHome();
                }
                else
                {
                    State_Think_Stroll(config.short_MoveStep);
                }
            }
        }
    }
    private void State_Think_Stroll(int distance, int tryTime = 0)
    {
        if (distance < 1 || tryTime > 3)
        {
            /*闲逛失败*/
        }
        else
        {
            Vector3Int random = new Vector3Int(new System.Random().Next(-distance, distance + 1), new System.Random().Next(-distance, distance + 1));
            Vector3Int offset = Vector3Int.zero;
            if (brainManager.activityPostion.isValue)
            {
                if (brainManager.activityPostion.postion.x - pathManager.vector3Int_CurPos.x > 5)
                {
                    offset += Vector3Int.right;
                }
                if (brainManager.activityPostion.postion.x - pathManager.vector3Int_CurPos.x < -5)
                {
                    offset += Vector3Int.left;
                }
                if (brainManager.activityPostion.postion.y - pathManager.vector3Int_CurPos.y > 5)
                {
                    offset += Vector3Int.up;
                }
                if (brainManager.activityPostion.postion.y - pathManager.vector3Int_CurPos.y < -5)
                {
                    offset += Vector3Int.down;
                }
            }
            if (!pathManager.State_MovePostion(pathManager.vector3Int_CurPos + random + offset))
            {
                /*无法抵达*/
                State_Think_Stroll(distance - 1, tryTime + 1);
            }
        }
    }
    /// <summary>
    /// 回家
    /// </summary>
    public void State_Think_GoHome()
    {
        if (Vector3.Distance(pathManager.vector3Int_CurPos, brainManager.homePostion.postion) < 0.5f)
        {
            actionManager.Despawn();
        }
        else
        {
            pathManager.State_MovePostion(brainManager.homePostion.postion);
        }
    }

    #endregion
    #region//警戒状态
    /// <summary>
    /// 检查威胁
    /// </summary>
    public void State_Threatened_Check()
    {
        float distance_Min = float.MaxValue;
        float distance_New = 0;
        brainManager.actorManager_ThreatenedTarget = null;
        brainManager.actorManagers_ThreatenedTarget.Clear();
        brainManager.actorManagers_Nearby.RemoveAll((x) => { return x == null; });
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], config.float_ViewDistance))
            {
                distance_New = Vector3.Distance(transform.position, (brainManager.actorManagers_Nearby[i].transform.position));
                if (distance_Min > distance_New)
                {
                    distance_Min = distance_New;
                    brainManager.actorManager_ThreatenedTarget = brainManager.actorManagers_Nearby[i];
                }
                brainManager.actorManagers_ThreatenedTarget.Add(brainManager.actorManagers_Nearby[i]);
            }
        }
        if (brainManager.actorManager_ThreatenedTarget != null)
        {
            State_Threatened_Run(brainManager.actorManager_ThreatenedTarget);
        }
    }
    /// <summary>
    /// 逃跑
    /// </summary>
    public void State_Threatened_Run(ActorManager from)
    {
        int randomX = new System.Random().Next(0, 2);
        int randomY = new System.Random().Next(0, 2);
        Vector3Int dirX = Vector3Int.zero;
        Vector3Int dirY = Vector3Int.zero;
        #region//首先尝试对角逃跑
        if (from.pathManager.vector3Int_CurPos.x > pathManager.vector3Int_CurPos.x)
        {
            dirX += Vector3Int.left;
        }
        else if (from.pathManager.vector3Int_CurPos.x < pathManager.vector3Int_CurPos.x)
        {
            dirX += Vector3Int.right;
        }
        if (from.pathManager.vector3Int_CurPos.y > pathManager.vector3Int_CurPos.y)
        {
            dirY += Vector3Int.down;
        }
        else if (from.pathManager.vector3Int_CurPos.y < pathManager.vector3Int_CurPos.y)
        {
            dirY += Vector3Int.up;
        }
        if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX + dirY)) return;
        #endregion
        #region//然后尝试四向逃跑
        int distanceX = Math.Abs(from.pathManager.vector3Int_CurPos.x - pathManager.vector3Int_CurPos.x);
        int distanceY = Math.Abs(from.pathManager.vector3Int_CurPos.y - pathManager.vector3Int_CurPos.y);
        if (distanceX >= distanceY)
        {
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
            if (randomY == 0)
            {
                dirY = Vector3Int.up;
            }
            else
            {
                dirY = Vector3Int.down;
            }
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
            dirY = -dirY;
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
        }
        else
        {
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirY)) return;
            if (randomX == 0)
            {
                dirX = Vector3Int.right;
            }
            else
            {
                dirX = Vector3Int.left;
            }
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
            dirX = -dirX;
            if (pathManager.State_MovePostion(pathManager.vector3Int_CurPos + dirX)) return;
        }
        #endregion
    }
    #endregion
}
[Serializable]
public struct ActorConfig_Rabbit
{
    [Header("初始身份")]
    public StatusType status_Type;
    [Header("生命")]
    public short short_Hp;
    [Header("护甲")]
    public short short_Armor;
    [Header("魔抗")]
    public short short_Resistance;
    [Header("移动速度")]
    public short short_MoveSpeed;
    [Header("移动距离"), Range(1, 10)]
    public short short_MoveStep;
    [Header("视野距离"), Range(1, 99)]
    public float float_ViewDistance;
    [Header("基本掉落列表")]
    public List<BaseLootInfo> lootInfos_Base;
    [Header("额外掉落列表")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
