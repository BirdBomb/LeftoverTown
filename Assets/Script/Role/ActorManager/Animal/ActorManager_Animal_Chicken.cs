using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;

public class ActorManager_Animal_Chicken : ActorManager
{
    [SerializeField, Header("思考间隔"), Range(1, 10)]
    public float float_ThinkCD;
    private float float_ThinkTimer;
    [Header("鸡配置")]
    public ActorConfig_Chicken config;
    /// <summary>
    /// 出生点
    /// </summary>
    private Vector3Int vector3_HomePos;
    private GlobalTime globalTime_Cur;
    public override void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            AllClient_Listen_UpdateTime(_.hour, _.day, _.now);
        }).AddTo(this);
        base.AllClient_AddListener();
    }
    #region//初始化
    public override void Client_Init()
    {
        statusManager.statusType = config.status_Type;
        base.Client_Init();
    }
    public override void State_Init()
    {
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
        actorNetManager.Local_SetBagItem(itemDatas);
    }
    /// <summary>
    /// 绑定出生点
    /// </summary>
    /// <param name="pos"></param>
    public void State_BindHome(Vector3Int pos)
    {
        vector3_HomePos = pos;
    }
    #endregion
    public override void AllClient_Listen_UpdateTime(int hour, int date, GlobalTime globalTime)
    {
        globalTime_Cur = globalTime;
        base.AllClient_Listen_UpdateTime(hour, date, globalTime);
    }
    #region//对外界的反应
    public override void State_Listen_MyselfHpChange(int parameter, NetworkId id)
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
        base.State_Listen_MyselfHpChange(parameter, id);
    }
    #endregion
    #region//闲置状态
    public override void State_SecondUpdate()
    {
        float_ThinkTimer += 1;
        if (float_ThinkTimer > float_ThinkCD)
        {
            float_ThinkTimer = 0;
            State_Think();
        }
        base.State_SecondUpdate();
    }
    /// <summary>
    /// 思考
    /// </summary>
    public void State_Think()
    {
        if (globalTime_Cur != GlobalTime.Evening)
        {
            if (brainManager.actorManagers_ThreatenedTarget.Count == 0)
            {
                State_Think_Stroll();
            }
        }
        else
        {
            if (brainManager.actorManagers_ThreatenedTarget.Count == 0)
            {
                State_Think_Stroll();
            }
        }
    }
    /// <summary>
    /// 闲逛
    /// </summary>
    public void State_Think_Stroll()
    {
        if (vector3_HomePos.x - pathManager.vector3Int_CurPos.x > 10)
        {
            State_MoveSameStep(Vector3Int.right);
        }
        else if (vector3_HomePos.x - pathManager.vector3Int_CurPos.x < -10)
        {
            State_MoveSameStep(Vector3Int.left);
        }
        else if (vector3_HomePos.y - pathManager.vector3Int_CurPos.y > 10)
        {
            State_MoveSameStep(Vector3Int.up);
        }
        else if (vector3_HomePos.y - pathManager.vector3Int_CurPos.y < -10)
        {
            State_MoveSameStep(Vector3Int.down);
        }
        else
        {
            int random = new System.Random().Next(1, 4);
            if (random == 1) State_MoveSameStep(Vector3Int.down, config.short_MoveStep);
            if (random == 2) State_MoveSameStep(Vector3Int.up, config.short_MoveStep);
            if (random == 3) State_MoveSameStep(Vector3Int.right, config.short_MoveStep);
            if (random == 4) State_MoveSameStep(Vector3Int.left, config.short_MoveStep);
        }
    }
    /// <summary>
    /// 朝目标方向移动
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public void State_MoveSameStep(Vector3Int dir, short step = 1)
    {
        Vector3Int pos_F = pathManager.vector3Int_CurPos + dir * step;
        Vector3Int pos_R = pathManager.vector3Int_CurPos - dir * step;
        if (!pathManager.State_MoveTo(pos_F)) pathManager.State_MoveTo(pos_R);
    }
    #endregion
    #region//警戒状态
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
        State_Threatened_Check();
        base.State_CustomUpdate();
    }
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
        Vector3Int dirX;
        Vector3Int dirY;
        if (from.transform.position.x < transform.position.x)
        {
            dirX = Vector3Int.right;
        }
        else
        {
            dirX = Vector3Int.left;
        }
        if (from.pathManager.vector3Int_CurPos.y < pathManager.vector3Int_CurPos.y)
        {
            dirY = Vector3Int.up;
        }
        else if (from.pathManager.vector3Int_CurPos.y > pathManager.vector3Int_CurPos.y)
        {
            dirY = Vector3Int.down;
        }
        else
        {
            dirY = Vector3Int.zero;
        }
        if (pathManager.State_MoveTo(pathManager.vector3Int_CurPos + dirX + dirY)) return;
        if (pathManager.State_MoveTo(pathManager.vector3Int_CurPos + dirX)) return;
        if (pathManager.State_MoveTo(pathManager.vector3Int_CurPos + dirY)) return;
    }
    #endregion

}
[Serializable]
public struct ActorConfig_Chicken
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
