using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using static Fusion.Allocator;
using static GameEvent;
using static UnityEngine.InputManagerEntry;

public class ActorManager_Animal_Rabbit : ActorManager
{
    [Header("兔子配置")]
    public ActorConfig_Rabbit config;

    private Vector2 vector2_State_RabbitHole;
    private int int_Second;
    private GlobalTime globalTime_Cur;
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent_AllClient_SomeoneSendEmoji>().Subscribe(_ =>
        {
            AllClient_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);
            if (actorAuthority.isState) State_Listen_RoleSendEmoji(_.actor, _.emoji, _.distance);

        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent_AllClient_UpdateTime>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.date, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }

    public override void AllClient_Init()
    {
        statusManager.statusType = config.status_Type;
        base.AllClient_Init();
    }
    public override void State_Init()
    {
        State_InitNPCData();
        base.State_Init();
    }
    public override void State_SecondUpdate()
    {
        int_Second++;
        if (int_Second % config.float_Stroll == 0)
        {
            if (globalTime_Cur != GlobalTime.Evening)
            {
                if (brainManager.actorManagers_ThreatenedTarget.Count == 0)
                {
                    State_Stroll();
                }
            }
            else
            {
                if (brainManager.actorManagers_ThreatenedTarget.Count == 0)
                {
                    State_GoHome();
                }
            }
        }
        base.State_SecondUpdate();
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
    public override void State_CustomUpdate()
    {
        State_CheckThreatened();
        base.State_CustomUpdate();
    }
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void State_InitNPCData()
    {
        actorNetManager.Net_HpCur = config.short_Hp;
        actorNetManager.Net_HpMax = config.short_Hp;
        actorNetManager.Net_SpeedCommon = config.short_Speed;
        actorNetManager.Net_SpeedMax = config.short_Speed;
        for (int i = 0; i < config.lootInfos_Base.Count; i++)
        {
            int count = new System.Random().Next(config.lootInfos_Base[i].CountMin, config.lootInfos_Base[i].CountMax + 1);
            ItemData item = itemManager.CreateItemData(config.lootInfos_Base[i].ID, (short)count);
            actorNetManager.Net_ItemsInBag.Add(item);
        }
        for (int i = 0; i < config.lootInfos_Extra.Count; i++)
        {
            int random = new System.Random().Next(0, 1000);
            if (random <= config.lootInfos_Extra[i].Weight)
            {
                ItemData item = itemManager.CreateItemData(config.lootInfos_Extra[i].ID, (short)config.lootInfos_Extra[i].Count);
                actorNetManager.Net_ItemsInBag.Add(item);
            }
        }
    }

    /// <summary>
    /// 绑定兔子洞
    /// </summary>
    /// <param name="myTile"></param>
    public void State_BindRabbitHole(Vector2 pos)
    {
        vector2_State_RabbitHole = pos;
    }
    /// <summary>
    /// 检查威胁
    /// </summary>
    public void State_CheckThreatened()
    {
        float distance_Min = float.MaxValue;
        float distance_New = 0;
        brainManager.actorManager_ThreatenedTarget = null;
        brainManager.actorManagers_ThreatenedTarget.Clear();
        brainManager.actorManagers_Nearby.RemoveAll((x) => { return x == null; });
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], 99))
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
            State_Run(brainManager.actorManager_ThreatenedTarget);
        }
    }
    /// <summary>
    /// 逃跑
    /// </summary>
    public void State_Run(ActorManager from)
    {
        Vector3Int dir = Vector3Int.zero;
        if (from.transform.position.x < transform.position.x)
        {
            dir += Vector3Int.right;
        }
        else
        {
            dir += Vector3Int.left;
        }
        if (from.transform.position.y < transform.position.y)
        {
            dir += Vector3Int.up;
        }
        else
        {
            dir += Vector3Int.down;
        }
        pathManager.State_MoveTo(pathManager.vector3Int_CurPos + dir);
    }
    /// <summary>
    /// 闲逛
    /// </summary>
    public void State_Stroll()
    {
        Vector2 pos = transform.position;
        if (vector2_State_RabbitHole.x - pos.x > 10)
        {
            State_MoveOneStep(Vector3Int.right);
        }
        else if (vector2_State_RabbitHole.x - pos.x < -10)
        {
            State_MoveOneStep(Vector3Int.left);
        }
        else if (vector2_State_RabbitHole.y - pos.y > 10)
        {
            State_MoveOneStep(Vector3Int.up);
        }
        else if (vector2_State_RabbitHole.y - pos.y < -10)
        {
            State_MoveOneStep(Vector3Int.down);
        }
        else
        {
            int random = new System.Random().Next(1, 4);
            if (random == 1) State_MoveOneStep(Vector3Int.down);
            if (random == 2) State_MoveOneStep(Vector3Int.up);
            if (random == 3) State_MoveOneStep(Vector3Int.right);
            if (random == 4) State_MoveOneStep(Vector3Int.left);
        }
    }
    /// <summary>
    /// 回家
    /// </summary>
    public void State_GoHome()
    {
        if (Vector2.Distance(transform.position, vector2_State_RabbitHole) < 0.5f)
        {
            actionManager.Despawn();
        }
        else
        {
            pathManager.State_MoveTo(vector2_State_RabbitHole);
        }
    }
    /// <summary>
    /// 目标方向移动一格
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public void State_MoveOneStep(Vector3Int dir)
    {
        Vector3Int pos_F = pathManager.vector3Int_CurPos + dir;
        if (!pathManager.State_MoveTo(pos_F))
        {
            Vector3Int pos_R = pathManager.vector3Int_CurPos - dir;
            pathManager.State_MoveTo(pos_R);
        }
    }
}
[Serializable]
public struct ActorConfig_Rabbit
{
    [Header("初始身份")]
    public StatusType status_Type;
    [Header("初始生命")]
    public short short_Hp;
    [Header("初始速度")]
    public short short_Speed;
    [Header("初始闲逛间隔"), Range(1, 10)]
    public float float_Stroll;
    [Header("基本掉落列表")]
    public List<BaseLootInfo                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    > lootInfos_Base;
    [Header("额外掉落列表")]
    public List<ExtraLootInfo> lootInfos_Extra;
}
