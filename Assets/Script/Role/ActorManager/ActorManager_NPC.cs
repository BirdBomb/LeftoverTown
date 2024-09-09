using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static ActorConfigData;
using static UnityEditor.Progress;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.InputManagerEntry;
/// <summary>
/// NPC����ű�
/// </summary>
public class ActorManager_NPC : ActorManager
{
    public ActorManager_NPC_Config config;
    /// <summary>
    /// ��������
    /// </summary>
    protected float data_AttackCD;
    protected float onlyState_attackCDTimer;

    public virtual void FixedUpdate()
    {

    }
    #region//��ʼ��
    public override void InitByNetManager(bool hasStateAuthority, bool hasInputAuthority)
    {
        base.InitByNetManager(hasStateAuthority, hasInputAuthority);
        InitNPCData();
    }
    /// <summary>
    /// ��ʼ��NPC����
    /// </summary>
    public virtual void InitNPCData()
    {
        if (NetManager.Object.HasStateAuthority)
        {
            UnityEngine.Random.InitState((int)(transform.position.x + transform.position.y));
            NetManager.Data_EyeID = EyeConfigData.eyeConfigs[UnityEngine.Random.Range(0, EyeConfigData.eyeConfigs.Count)].Eye_ID;
            NetManager.Data_HairID = HairConfigData.hairConfigs[UnityEngine.Random.Range(0, HairConfigData.hairConfigs.Count)].Hair_ID;
            NetManager.Data_HairColor = UnityEngine.Random.ColorHSV();

            NetManager.Data_CurHp = config.Config_Hp;
            NetManager.Data_MaxHp = config.Config_Hp;
            NetManager.Data_Armor = config.Config_Armor;
            NetManager.Data_CommonSpeed = config.Config_Speed;
            NetManager.Data_MaxSpeed = config.Config_Speed;
            NetManager.Data_CurEn = config.Config_Endurance;
            NetManager.Data_MaxEn = config.Config_Endurance;

            NetManager.Data_ItemOnHead = CreateItemData(config.Config_Hat);
            NetManager.Data_ItemOnBody = CreateItemData(config.Config_Clothes);
            for (int i = 0; i < config.Config_BagList.Count; i++)
            {
                ItemData item = CreateItemData(config.Config_BagList[i]);
                if (item.Item_ID != 0)
                {
                    item.Item_Seed = item.Item_Seed + i;
                    NetManager.Data_ItemInBag.Add(item);
                }
            }
            for (int i = 0; i < config.Config_LootCount; i++)
            {
                ItemData item = GetRandomLootItem();
                if (item.Item_ID != 0)
                {
                    item.Item_Seed = item.Item_Seed + i;
                    NetManager.Data_ItemInBag.Add(item);
                }
            }
            NetManager.Data_BagCapacity = NetManager.Data_ItemInBag.Capacity;

            data_AttackCD = config.Config_AttackCD;
        }
    }
    /// <summary>
    /// ������Ȩ�ػ�������ƷID
    /// </summary>
    /// <returns></returns>
    private ItemData GetRandomLootItem()
    {
        int weight_Main = 0;
        int weight_temp = 0;
        int random;
        for (int j = 0; j < config.Config_LootList.Count; j++)
        {
            weight_Main += config.Config_LootList[j].Weight;
        }
        UnityEngine.Random.InitState(System.DateTime.Now.Second);
        random = UnityEngine.Random.Range(0, weight_Main);
        for (int j = 0; j < config.Config_LootList.Count; j++)
        {
            weight_temp += config.Config_LootList[j].Weight;
            if (weight_temp > random)
            {
                return CreateItemData(config.Config_LootList[j].ID);
            }
        }
        return new ItemData();
    }
    /// <summary>
    /// ����ID�����Ʒ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private ItemData CreateItemData(short id)
    {
        Type type = Type.GetType("Item_" + id.ToString());
        ItemData itemData = new ItemData
            (id,
             MapManager.Instance.mapSeed + (int)(System.DateTime.Now.Ticks * 1000),
             1,
             0,
             0,
             0,
             new ContentData());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(itemData, out ItemData initData);
        return initData;
    }
    #endregion
    
    #region//����
    public override void ListenRoleMove(ActorManager actor, MyTile where)
    {
        if (actor == this)
        {
            ListenRoleMove_Me(actor, where);
        }
        else
        {
            ListenRoleMove_Other(actor, where);
        }
        base.ListenRoleMove(actor, where);
    }
    /// <summary>
    /// �������ƶ�
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="where"></param>
    public virtual void ListenRoleMove_Other(ActorManager actor, MyTile where)
    {

    }
    /// <summary>
    /// ���Լ��ƶ�
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="where"></param>
    public virtual void ListenRoleMove_Me(ActorManager actor, MyTile where)
    {

    }
    /// <summary>
    /// ����ʱ��仯
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="date"></param>
    /// <param name="globalTime"></param>
    public override void ListenWorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {
        base.ListenWorldGlobalTimeChange(hour, date, globalTime);
    }
    /// <summary>
    /// ����ĳ�˷���Emoji
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="id"></param>
    public override void ListenRoleSendEmoji(ActorManager actor, int id, float distance)
    {
        if (isState)
        {
            if (OnlyState_TryHearAt(actor,distance))
            {
                OnlyState_TryUnderstand(actor, id, OnlyState_TryLookAt(actor), true);
            }
        }
        base.ListenRoleSendEmoji(actor, id, distance);
    }
    /// <summary>
    /// ��������ֵ�仯
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="id"></param>
    public override void Listen_HpChange(int parameter, NetworkId id)
    {
        base.Listen_HpChange(parameter, id);
    }
    #endregion
    #region//֡����
    /// <summary>
    /// NPC������
    /// </summary>
    /// <param name="dt"></param>
    public virtual void AllLocal_Update_AttackingTarget(float dt)
    {
        if (attackState && attackTarget)
        {
            if (holdingByHand != null)
            {
                if (holdingByHand.PressRightClick(dt, isState, isInput, false))
                {
                    if (holdingByHand.PressLeftClick(dt, isState, isInput, false))
                    {
                        holdingByHand.ClickLeftClick(dt, isState, isInput, false);
                        if (isState)
                        {
                            holdingByHand.ReleaseLeftClick(dt, isState, isInput, false);
                            holdingByHand.ReleaseRightClick(dt, isState, isInput, false);
                            NetManager.RPC_State_NpcChangeAttackState(false);
                        }
                        else
                        {
                            attackState = false;
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// NPCע����
    /// </summary>
    /// <param name="dt"></param>
    public virtual void AllLocal_Update_LookingTarget(float dt)
    {
        if (attackTarget)
        {
            FaceTo(attackTarget.transform.position - transform.position);
            if (holdingByHand != null)
            {
                holdingByHand.InputMousePos(attackTarget.transform.position - transform.position, dt);
            }
        }
    }
    /// <summary>
    /// ����Ƿ�ʼ��һ�ι���
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>������һ�ι���</returns>
    public virtual bool OnlyState_Update_CheckingAttackingTimer(float dt)
    {
        if (onlyState_attackCDTimer > 0)
        {
            onlyState_attackCDTimer -= dt;
            return false;
        }
        else
        {
            onlyState_attackCDTimer = data_AttackCD;
            return true;
        }

    }
    /// <summary>
    /// ����Ƿ�ﵽ��������
    /// </summary>
    /// <param name="dt"></param>
    /// <returns>�ﵽ��������</returns>
    public virtual bool OnlyState_Update_CheckingAttackingDistance(float dt)
    {
        if (Vector3.Distance(attackTarget.transform.position, transform.position) < holdingByHand.itemConfig.Attack_Distance + 0.5f)
        {
            NetManager.RPC_State_NpcChangeAttackState(true);
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    #region//����
    /// <summary>
    /// ���Ĺ���Ŀ��
    /// </summary>
    /// <param name="id">Ŀ���netID</param>
    public override void FromRPC_ChangeAttackTarget(NetworkId id)
    {
        base.FromRPC_ChangeAttackTarget(id);
    }
    /// <summary>
    /// ����ĳ��
    /// </summary>
    /// <param name="who"></param>
    /// <param name="handItemID"></param>
    /// <param name="headItemID"></param>
    /// <param name="bodyItemID"></param>
    public virtual void OnlyState_CheckOutSomeone(ActorManager who, out short handItemID, out short headItemID, out short bodyItemID,out short fine)
    {
        handItemID = who.NetManager.Data_ItemInHand.Item_ID;
        headItemID = who.NetManager.Data_ItemOnHead.Item_ID;
        bodyItemID = who.NetManager.Data_ItemOnBody.Item_ID;
        fine = who.NetManager.Data_Fine;
    }
    /// <summary>
    /// ����Ŀ��
    /// </summary>
    /// <param name="followType">���ٷ�ʽ</param>
    public virtual void OnlyState_Follow(FollowType followType)
    {
        if (attackTarget)
        {
            Vector2 moveTo = Vector2.one;
            if (followType == FollowType.RunAway)
            {
                Vector3 attackTargetDir = attackTarget.transform.position - transform.position;
                int toX = 0;
                int toY = 0;
                if (attackTargetDir.x > 0.5)
                {
                    toX = -1;
                }
                else if (attackTargetDir.x < -0.5)
                {
                    toX = 1;
                }
                if (attackTargetDir.y > 0.5)
                {
                    toY = -1;
                }
                else if (attackTargetDir.y < -0.5)
                {
                    toY = 1;
                }
                moveTo = GetMyTileWithOffset(new Vector3Int(toX, toY, 0))._posInWorld;
            }
            else if (followType == FollowType.RunTo)
            {
                moveTo = attackTarget.GetMyTile()._posInWorld;
            }
            else if (followType == FollowType.Attack)
            {
                float targetDistance = attackTarget == null ? 0 : Vector3.Distance(attackTarget.transform.position, transform.position);
                float attackDistance = holdingByHand == null ? 0 : holdingByHand.itemConfig.Attack_Distance;
                if (targetDistance < attackDistance && attackDistance > 2)
                {
                    Vector3 attackTargetDir = attackTarget.transform.position - transform.position;
                    int toX = 0;
                    int toY = 0;
                    if (attackTargetDir.x > 0.5)
                    {
                        toX = -1;
                    }
                    else if (attackTargetDir.x < -0.5)
                    {
                        toX = 1;
                    }
                    if (attackTargetDir.y > 0.5)
                    {
                        toY = -1;
                    }
                    else if (attackTargetDir.y < -0.5)
                    {
                        toY = 1;
                    }
                    moveTo = GetMyTileWithOffset(new Vector3Int(toX, toY, 0))._posInWorld;
                }
                else
                {
                    moveTo = attackTarget.GetMyTile()._posInWorld;
                }

            }
            else
            {
                moveTo = GetMyTile()._posInWorld;
            }
            FindWayToTarget(moveTo);
        }
    }
    /// <summary>
    /// �ó�
    /// </summary>
    /// <param name="itemType">�ó��������</param>
    public virtual void OnlyState_PutOut(ItemType itemType)
    {
        if (holdingByHand != null && holdingByHand.itemConfig.Item_Type == itemType)
        {
            return;
        }
        else
        {
            for (int i = 0; i < NetManager.Data_ItemInBag.Count; i++)
            {
                if (ItemConfigData.GetItemConfig(NetManager.Data_ItemInBag[i].Item_ID).Item_Type == itemType)
                {
                    ItemData item = NetManager.Data_ItemInBag[i];
                    NetManager.OnlyState_AddItemOnHand(item);
                    NetManager.OnlyState_LoseItem(item);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void OnlyState_PutDown()
    {
        if (holdingByHand != null)
        {
            NetManager.OnlyState_AddItemInBag(NetManager.Data_ItemInHand);
            NetManager.OnlyState_RemoveItemOnHand();
        }
    }
    /// <summary>
    /// ���Կ���ĳ��
    /// </summary>
    /// <param name="who">Ŀ��</param>
    /// <returns>�ܷ񿴵�</returns>
    public virtual bool OnlyState_TryLookAt(ActorManager who)
    {
        if (isState)
        {
            if (who != this)//����˲�����
            {
                if (Vector3.Distance(transform.position, who.transform.position) >= 10)
                {
                    return false;
                }
                if (Physics2D.LinecastAll(who.transform.position, transform.position, tileLayer).Length > 0)
                {
                    return false;
                }
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// ��������ĳ��
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public virtual bool OnlyState_TryHearAt(ActorManager who, float distance)
    {
        if (isState)
        {
            if (who != this)//����˲�����
            {
                if (Vector3.Distance(transform.position, who.transform.position) >= distance)
                {
                    return false;
                }
                return true;
            }
        }
        return false;

    }
    /// <summary>
    /// �������emoji
    /// </summary>
    /// <param name="who">Ŀ��</param>
    /// <param name="id">emojiID</param>
    public virtual void OnlyState_TryUnderstand(ActorManager who, int id, bool look, bool hear)
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public override void Dead()
    {
        for (int i = 0; i < NetManager.Data_ItemInBag.Count; i++)
        {
            ItemData item = NetManager.Data_ItemInBag[i];
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = item,
                pos = transform.position - new Vector3(0, 0.1f, 0)
            });
        }
        ItemData itemOnHead = NetManager.Data_ItemOnHead;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
        {
            itemData = itemOnHead,
            pos = transform.position - new Vector3(0, 0.1f, 0)
        });
        ItemData itemOnBody = NetManager.Data_ItemOnBody;
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
        {
            itemData = itemOnBody,
            pos = transform.position - new Vector3(0, 0.1f, 0)
        });
        base.Dead();
    }
    public void TryToSendEmoji(float wait, int id)
    {
        if (id != -1)
        {
            StartCoroutine(SendEmoji(wait, id));
        }
    }
    /// <summary>
    /// �ӳٷ���emoji
    /// </summary>
    /// <param name="wait"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerator SendEmoji(float wait,int id)
    {
        yield return new WaitForSeconds(wait);
        NetManager.RPC_LocalInput_SendEmoji(id);
    }
    #endregion
    /// <summary>
    /// ���ٷ�ʽ
    /// </summary>
    public enum FollowType
    {
        /// <summary>
        /// ����
        /// </summary>
        RunAway,
        /// <summary>
        /// ����
        /// </summary>
        RunTo,
        /// <summary>
        /// ����
        /// </summary>
        Attack
    }
}
public struct ActorConfig
{
}
[Serializable]
public struct ActorManager_NPC_Config
{
    [Header("����")]
    public short Config_Hp;
    [Header("����")]
    public short Config_Armor;
    [Header("�ٶ�(����ÿ��)")]
    public short Config_Speed;
    [Header("����")]
    public int Config_Endurance;
    [Header("ñ��")]
    public short Config_Hat;
    [Header("�·�")]
    public short Config_Clothes;
    [Header("��������")]
    public List<short> Config_BagList;
    [Header("��������")]
    public int Config_LootCount;
    [Header("�����б�")]
    public List<LootInfo> Config_LootList;
    [Header("�������")]
    public float Config_AttackCD;
}