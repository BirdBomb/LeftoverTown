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
    [Header("NPC����")]
    public ActorManager_NPC_Config config;
    #region//��ʼ��NPC
    public override void AllClient_InitByNetManager(bool hasStateAuthority, bool hasInputAuthority)
    {
        base.AllClient_InitByNetManager(hasStateAuthority, hasInputAuthority);
        AllClient_InitNPCData();
    }
    /// <summary>
    /// ��ʼ��NPC����
    /// </summary>
    public virtual void AllClient_InitNPCData()
    {
        onlyState_AttackCD = config.Config_AttackCD;
        if (isState)
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

            NetManager.Data_ItemOnHead = OnlyState_CreateItemData(config.Config_Hat);
            NetManager.Data_ItemOnBody = OnlyState_CreateItemData(config.Config_Clothes);
            for (int i = 0; i < config.Config_BagList.Count; i++)
            {
                ItemData item = OnlyState_CreateItemData(config.Config_BagList[i]);
                if (item.Item_ID != 0)
                {
                    item.Item_Seed = item.Item_Seed + i;
                    NetManager.Data_ItemInBag.Add(item);
                }
            }
            for (int i = 0; i < config.Config_LootCount; i++)
            {
                ItemData item = OnlyState_GetRandomLootItem();
                if (item.Item_ID != 0)
                {
                    item.Item_Seed = item.Item_Seed + i;
                    NetManager.Data_ItemInBag.Add(item);
                }
            }
            NetManager.Data_BagCapacity = NetManager.Data_ItemInBag.Capacity;
        }
    }
    /// <summary>
    /// ������Ȩ�ػ�������ƷID
    /// </summary>
    /// <returns></returns>
    private ItemData OnlyState_GetRandomLootItem()
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
                return OnlyState_CreateItemData(config.Config_LootList[j].ID);
            }
        }
        return new ItemData();
    }
    /// <summary>
    /// ����ID�����Ʒ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private ItemData OnlyState_CreateItemData(short id)
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
    public override void OnlyStateListen_RoleSendEmoji(ActorManager actor, int id, float distance)
    {
        if (Tool_TryHearAt(actor, distance))
        {
            OnlyState_TryUnderstand(actor, id, Tool_TryLookAt(actor), true);
        }
        base.OnlyStateListen_RoleSendEmoji(actor, id, distance);
    }
    #endregion
    #region//����
    /// <summary>
    /// ����Ŀ��(������)
    /// </summary>
    /// <param name="followType">���ٷ�ʽ</param>
    public virtual void OnlyState_Follow(FollowType followType)
    {
        if (allClient_AttackTarget)
        {
            Vector2 moveTo;
            if (followType == FollowType.RunAway)
            {
                Vector3 attackTargetDir = allClient_AttackTarget.transform.position - transform.position;
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
                moveTo = Tool_GetMyTileWithOffset(new Vector3Int(toX, toY, 0))._posInWorld;
            }
            else if (followType == FollowType.RunTo)
            {
                moveTo = allClient_AttackTarget.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInWorld;
            }
            else if (followType == FollowType.Attack)
            {
                float targetDistance = allClient_AttackTarget == null ? 0 : Vector3.Distance(allClient_AttackTarget.transform.position, transform.position);
                float attackDistance = itemOnHand == null ? 0 : itemOnHand.itemConfig.Attack_Distance;
                if (targetDistance < attackDistance && attackDistance > 2)
                {
                    /*Ŀ�����Լ��Ĺ�����Χ��,Զ��Ŀ��*/
                    Vector3 attackTargetDir = allClient_AttackTarget.transform.position - transform.position;
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
                    moveTo = Tool_GetMyTileWithOffset(new Vector3Int(toX, toY, 0))._posInWorld;
                }
                else
                {
                    /*Ŀ�겻���Լ��Ĺ�����Χ��,����Ŀ��*/
                    moveTo = allClient_AttackTarget.Tool_GetMyTileWithOffset(Vector3Int.zero)._posInWorld;
                }
            }
            else
            {
                moveTo = Tool_GetMyTileWithOffset(Vector3Int.zero)._posInWorld;
            }
            OnlyState_FindWayToTarget(moveTo);
        }
    }
    /// <summary>
    /// ��������ó�(������)
    /// </summary>
    /// <param name="itemType">�ó��������</param>
    public virtual void OnlyState_PutOut(ItemType itemType)
    {
        if (itemOnHand != null && itemOnHand.itemConfig.Item_Type == itemType)
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
    /// ����ID�ó�(������)
    /// </summary>
    /// <param name="ID">�ó�����ID</param>
    public virtual void OnlyState_PutOut(short ID)
    {
        if (itemOnHand != null && itemOnHand.itemConfig.Item_ID == ID)
        {
            return;
        }
        else
        {
            for (int i = 0; i < NetManager.Data_ItemInBag.Count; i++)
            {
                if (ItemConfigData.GetItemConfig(NetManager.Data_ItemInBag[i].Item_ID).Item_ID == ID)
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
    /// ����(������)
    /// </summary>
    public virtual void OnlyState_PutDown()
    {
        if (itemOnHand != null)
        {
            NetManager.OnlyState_AddItemInBag(NetManager.Data_ItemInHand);
            NetManager.OnlyState_RemoveItemOnHand();
        }
    }
    /// <summary>
    /// ����Emoji(�ͻ���)
    /// </summary>
    /// <param name="wait"></param>
    /// <param name="id"></param>
    public void AllClient_TryToSendEmoji(float wait, int id)
    {
        if (id != -1)
        {
            StartCoroutine(AllClient_SendEmoji(wait, id));
        }
    }
    /// <summary>
    /// ����Emoji(������)
    /// </summary>
    /// <param name="wait"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerator AllClient_SendEmoji(float wait, int id)
    {
        yield return new WaitForSeconds(wait);
        if (isInput)
        {
            NetManager.RPC_LocalInput_SendEmoji(id);
        }
    }
    /// <summary>
    /// �������emoji(������)
    /// </summary>
    /// <param name="who">Ŀ��</param>
    /// <param name="id">emojiID</param>
    public virtual void OnlyState_TryUnderstand(ActorManager who, int id, bool look, bool hear)
    {

    }
    /// <summary>
    /// ����(������)
    /// </summary>
    public override void OnlyState_AlreadyDead()
    {
        for (int i = 0; i < NetManager.Data_ItemInBag.Count; i++)
        {
            ItemData item = NetManager.Data_ItemInBag[i];
            if (item.Item_ID != 0)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = item,
                    pos = transform.position - new Vector3(0, 0.1f, 0)
                });
            }
        }
        ItemData itemOnHead = NetManager.Data_ItemOnHead;
        if (itemOnHead.Item_ID != 0)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = itemOnHead,
                pos = transform.position - new Vector3(0, 0.1f, 0)
            });
        }
        ItemData itemOnBody = NetManager.Data_ItemOnBody;
        if (itemOnBody.Item_ID != 0)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = itemOnBody,
                pos = transform.position - new Vector3(0, 0.1f, 0)
            });
        }
        base.OnlyState_AlreadyDead();
    }
    /// <summary>
    /// (ͨ�÷���)���Կ���ĳ��
    /// </summary>
    /// <param name="who">Ŀ��</param>
    /// <returns>�ܷ񿴵�</returns>
    public bool Tool_TryLookAt(ActorManager who)
    {
        if (who != this)//����˲�����
        {
            if (Vector3.Distance(transform.position, who.transform.position) >= config.Config_View)
            {
                return false;
            }
            if (Physics2D.LinecastAll(who.transform.position, transform.position, layerMask_Wall).Length > 0)
            {
                return false;
            }
            return true;
        }
        return false;
    }
    /// <summary>
    /// (ͨ�÷���)��������ĳ��
    /// </summary>
    /// <param name="who"></param>
    /// <returns></returns>
    public bool Tool_TryHearAt(ActorManager who, float distance)
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
    #endregion


}
[Serializable]
public struct ActorManager_NPC_Config
{
    [Header("����")]
    public short Config_Hp;
    [Header("����")]
    public short Config_Armor;
    [Header("��Ұ")]
    public short Config_View;
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
