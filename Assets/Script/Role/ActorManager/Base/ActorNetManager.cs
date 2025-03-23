using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UnityEngine.Windows;
using Fusion.Addons.Physics;
using System;
using ExitGames.Client.Photon.StructWrapping;
/// <summary>
/// ��ɫ���������
/// </summary>
public class ActorNetManager : NetworkBehaviour
{
    [Header("����ͬ�����")]
    public NetworkRigidbody2D networkRigidbody;
    [Header("���ؽ�ɫ���")]
    public ActorManager actorManager_Local;
    public override void Spawned()
    {
        actorManager_Local.AllClient_Init();
        AllClient_UpdateNetworkData();
        if (Object.HasStateAuthority)
        {
            actorManager_Local.State_Init();
        }
        base.Spawned();
    }
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
    }
    public override void FixedUpdateNetwork()
    {
        actorManager_Local.AllClient_FixedUpdateNetwork(Runner.DeltaTime);
        if (actorManager_Local.actorAuthority.isState) actorManager_Local.State_FixedUpdateNetwork(Runner.DeltaTime);
        base.FixedUpdateNetwork();
    }
    #region//Networked
    /// <summary>
    /// ��ʼ��NetData
    /// </summary>
    public void AllClient_UpdateNetworkData()
    {
        OnNameChange();
        OnHairIDChange();
        OnHairColorChange();
        OnEyeIDChange();

        OnItemInBagChange();
        OnItemInHandChange();
        OnItemOnHeadChange();
        OnItemOnBodyChange();

        OnBuffsChange();
        OnSkillKnowChange();
        OnSkillUseChange();
    }
    #region//��ò������
    [Networked, OnChangedRender(nameof(OnNameChange)), HideInInspector]
    public string Net_Name { get; set; }
    [Networked, OnChangedRender(nameof(OnEyeIDChange)), HideInInspector]
    public short Net_EyeID { get; set; }
    [Networked, OnChangedRender(nameof(OnHairIDChange))]
    public short Net_HairID { get; set; }
    [Networked, OnChangedRender(nameof(OnHairColorChange)), HideInInspector]
    public Color32 Net_HairColor { get;set; }

    public void OnNameChange()
    {
        actorManager_Local.actorUI.ShowName(Net_Name);
    }
    public void OnEyeIDChange()
    {
        actorManager_Local.bodyController.InitFace(Net_HairID, Net_EyeID, Net_HairColor);
    }
    public void OnHairIDChange()
    {
        actorManager_Local.bodyController.InitFace(Net_HairID, Net_EyeID, Net_HairColor);
    }
    public void OnHairColorChange()
    {
        actorManager_Local.bodyController.InitFace(Net_HairID, Net_EyeID, Net_HairColor);
    }
    #endregion
    #region//��������
    /// <summary>
    /// ��ͨ�ٶ�(����/��)
    /// </summary>
    [Networked, HideInInspector]
    public short Net_SpeedCommon { get; set; }
    /// <summary>
    /// ����ٶ�(����/��)
    /// </summary>
    [Networked, HideInInspector]
    public short Net_SpeedMax { get; set; }
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurHpChange)), HideInInspector]
    public int Net_HpCur { get; set; }
    /// <summary>
    /// �������ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnMaxHpChange)), HideInInspector]
    public int Net_HpMax { get; set; }
    /// <summary>
    /// ����
    /// </summary>
    [Networked, OnChangedRender(nameof(OnArmorChange)), HideInInspector]
    public short Net_Armor { get; set; }
    /// <summary>
    /// ��ǰʳ��ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurFoodChange)), HideInInspector]
    public short Net_FoodCur { get; set; }
    /// <summary>
    /// ���ʳ��ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnMaxFoodChange)), HideInInspector]
    public short Net_FoodMax { get; set; }
    /// <summary>
    /// ȱˮֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnWaterChange)), HideInInspector]
    public short Net_Water { get; set; }
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCurSanChange)), HideInInspector]
    public short Net_SanCur { get; set; }
    /// <summary>
    /// �����ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnMaxSanChange)), HideInInspector]
    public short Net_SanMax { get; set; }
    /// <summary>
    /// ����ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnHappyChange)), HideInInspector]
    public short Net_Happy { get; set; }
    /// <summary>
    /// ���
    /// </summary>
    [Networked, OnChangedRender(nameof(OnCoinChange)), HideInInspector]
    public int Net_Coin { get; set; }
    /// <summary>
    /// ����
    /// </summary>
    [Networked, OnChangedRender(nameof(OnFineChange)), HideInInspector]
    public short Net_Fine { get; set; }
    /// <summary>
    /// �������ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnEnChange)), HideInInspector]
    public int Net_EnMax { get; set; }/*��λ����*/
    /// <summary>
    /// ��ǰ����ֵ
    /// </summary>
    [Networked, OnChangedRender(nameof(OnEnChange)), HideInInspector]
    public int Net_EnCur { get; set; }/*��λ����*/
    /// <summary>
    /// �����ָ��ٶ�
    /// </summary>
    [Networked, HideInInspector]
    public int Net_EnRelease { get; set; }/*��̬Ϊ1000*/

    public void OnCurHpChange()
    {
        if (Net_HpCur <= 0)
        {
            actorManager_Local.AllClient_UpdateHpBar(0);
        }
        else
        {
            actorManager_Local.AllClient_UpdateHpBar((float)Net_HpCur / (float)Net_HpMax);
        }
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateHPData()
            {
                HP_Cur = Net_HpCur,
                HP_Max = Net_HpMax,
            });
        }
    }
    public void OnMaxHpChange()
    {

    }
    public void OnArmorChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateArmorData()
            {
                Armor = Net_Armor,
            });
        }
    }
    public void OnCurFoodChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateFoodData()
            {
                Food_Cur = Net_FoodCur,
                Food_Max = Net_FoodMax,
            });
        }
    }
    public void OnMaxFoodChange()
    {

    }
    public void OnWaterChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateWaterData()
            {
                Water = Net_Water,
            });
        }
    }
    public void OnCurSanChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateSanData()
            {
                San_Cur = Net_SanCur,
                San_Max = Net_SanMax,
            });
        }
    }
    public void OnMaxSanChange()
    {

    }
    public void OnHappyChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateHappyData()
            {
                Happy = Net_Happy,
            });
        }
    }
    public void OnCoinChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateCoinData()
            {
                Coin = Net_Coin,
            });
        }
    }
    public void OnFineChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateFineData()
            {
                Fine = Net_Fine,
            });
        }
    }
    public void OnEnChange()
    {
        if (Net_EnCur <= 0)
        {
            actorManager_Local.AllClient_UpdateEnBar(0);
        }
        else
        {
            actorManager_Local.AllClient_UpdateEnBar((float)Net_EnCur / (float)Net_EnMax);
        }
    }

    #endregion
    #region//��������
    [Networked, HideInInspector]
    public short Net_Point_Strength { get; set; }
    [Networked, HideInInspector]
    public short Net_Point_Intelligence { get; set; }
    [Networked, HideInInspector]
    public short Net_Point_SPower { get; set; }
    [Networked, HideInInspector]
    public short Net_Point_Focus { get; set; }
    [Networked, HideInInspector]
    public short Net_Point_Agility { get; set; }
    [Networked, HideInInspector]
    public short Net_Point_Make { get; set; }
    [Networked, HideInInspector]
    public short Net_Point_Build { get; set; }
    [Networked, HideInInspector]
    public short Net_Point_Cook { get; set; }
    #endregion
    #region//������Ʒ
    /// <summary>
    /// ��������
    /// </summary>
    [Networked, Capacity(20), OnChangedRender(nameof(OnItemInBagChange))]
    public NetworkLinkedList<ItemData> Net_ItemsInBag { get; }
    /// <summary>
    /// ��������
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemInBagChange)), HideInInspector]
    public short Net_ItemsInBagCapacity { get; set; }
    /// <summary>
    /// �ֲ�����
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemInHandChange)), HideInInspector]
    public ItemData Net_ItemInHand { get; set; } 
    /// <summary>
    /// ͷ������
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemOnHeadChange)), HideInInspector]
    public ItemData Net_ItemOnHead { get; set; } 
    /// <summary>
    /// ��������
    /// </summary>
    [Networked, OnChangedRender(nameof(OnItemOnBodyChange)), HideInInspector]
    public ItemData Net_ItemOnBody { get; set; } 
    [HideInInspector]
    public StatusType statusType;
    public void OnItemInHandChange()
    {
        actorManager_Local.itemManager.UpdateItemInHand(Net_ItemInHand);
    }
    public void OnItemOnHeadChange()
    {
        actorManager_Local.statusManager.Tool_CheckStatus(Net_ItemOnBody.Item_ID, Net_ItemOnHead.Item_ID, Net_Fine, out statusType);
        actorManager_Local.itemManager.UpdateItemOnHead(Net_ItemOnHead);
    }
    public void OnItemOnBodyChange()
    {
        actorManager_Local.statusManager.Tool_CheckStatus(Net_ItemOnBody.Item_ID, Net_ItemOnHead.Item_ID, Net_Fine, out statusType);
        actorManager_Local.itemManager.UpdateItemOnBody(Net_ItemOnBody);
    }
    public void OnItemInBagChange()
    {
        if (actorManager_Local.actorAuthority.isPlayer && actorManager_Local.actorAuthority.isLocal)
        {
            List<ItemData> temp = new List<ItemData>(Net_ItemsInBag);
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
            {
                bagCapacity = Net_ItemsInBagCapacity,
                itemDatas = temp
            });
        }
    }
    #endregion

    #region//�����뼼��
    [Networked, Capacity(20), OnChangedRender(nameof(OnBuffsChange)), HideInInspector]
    public NetworkLinkedList<short> Net_Buffs { get; }
    public void OnBuffsChange()
    {
        actorManager_Local.buffManager.UpdateBuff(Net_Buffs);
    }
    public void OnSkillKnowChange()
    {

    }
    public void OnSkillUseChange()
    {

    }
    #endregion
    #endregion
    #region//RPC
    #region//��ʼ��
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_InitPlayerCommonData(PlayerNetData netData,string playerName)
    {
        Net_Name = playerName;
        Net_HairID = netData.Hair_ID;
        Net_HairColor = netData.Hair_Color;
        Net_EyeID = netData.Eye_ID;
        if (netData.Hp_Cur <= 0)
        {
            Net_HpCur = netData.Hp_Max;
        }
        else
        {
            Net_HpCur = netData.Hp_Cur;
        }
        Net_HpMax = netData.Hp_Max;
        Net_Armor = netData.Armor_Cur;
        Net_FoodCur = netData.Food_Cur;
        Net_FoodMax = netData.Food_Max;
        Net_Water = netData.Water_Cur;
        Net_SanCur = netData.San_Cur;
        Net_SanMax = netData.San_Max;
        Net_Happy = netData.Happy_Cur;
        Net_Coin = netData.Coin_Cur;

        Net_SpeedCommon = netData.Speed_Common;
        Net_SpeedMax = netData.Speed_Max;

        Net_EnMax = netData.En_Cur;
        Net_EnCur = netData.En_Cur;
        Net_EnRelease = 1000;

        Net_Point_Strength = netData.Point_Strength;
        Net_Point_Intelligence = netData.Point_Intelligence;
        Net_Point_SPower = netData.Point_SPower;
        Net_Point_Focus = netData.Point_Focus;
        Net_Point_Agility = netData.Point_Agility;
        Net_Point_Make = netData.Point_Make;
        Net_Point_Build = netData.Point_Build;
        Net_Point_Cook = netData.Point_Cook;
    }
    #endregion
    #region//��Ʒ����(����)
    /// <summary>
    /// RPC:���ı�������
    /// </summary>
    /// <param name="capacity"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeBagCapacity(short capacity)
    {
        OnlyState_ChangeBagCapacity(capacity);
    }
    public void OnlyState_ChangeBagCapacity(short capacity)
    {
        Net_ItemsInBagCapacity = capacity;
    }
    /// <summary>
    /// RPC:��ӱ�������
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemInBag(ItemData addData)
    {
        OnlyState_AddItemInBag(addData);
    }
    public void OnlyState_AddItemInBag(ItemData addData)
    {
        if (addData.Item_ID > 0)
        {
            GameToolManager.Instance.PutInItemList(Net_ItemsInBag, Net_ItemsInBagCapacity, addData, out ItemData resData);
            if (resData.Item_Count > 0)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    pos = transform.position,
                    itemData = resData
                });
            }
        }
    }
    /// <summary>
    /// RPC:���ٱ�����Ʒ
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_SubItemInBag(ItemData subData)
    {
        OnlyState_SubItemInBag(subData);
    }
    public void OnlyState_SubItemInBag(ItemData subData)
    {
        if (subData.Item_ID > 0 && subData.Item_Count > 0)
        {
            GameToolManager.Instance.PutOutItemList(Net_ItemsInBag, subData);
        }
    }
    /// <summary>
    /// RPC:�޸ı�����Ʒ
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemInBag(ItemData oldItemData, ItemData newItemData)
    {
        OnlyState_ChangeItemInBag(oldItemData, newItemData);
    }
    public void OnlyState_ChangeItemInBag(ItemData oldItemData, ItemData newItemData)
    {
        int index = Net_ItemsInBag.IndexOf(oldItemData);
        if (index >= 0)
        {
            Net_ItemsInBag.Set(index, newItemData);
        }
        else
        {
            Debug.Log("��Ʒ�޸�ʧ��");
        }
    }
    #endregion
    #region//��Ʒ����(�ֲ�)
    /// <summary>
    /// RPC:��ӳ�������
    /// </summary>
    /// <param name="addData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnHand(ItemData addData)
    {
        OnlyState_AddItemOnHand(addData);
    }
    public void OnlyState_AddItemOnHand(ItemData addData)
    {
        Net_ItemInHand = GameToolManager.Instance.PutInItemSingle(Net_ItemInHand, addData, out ItemData resData);
        if (resData.Item_ID > 0 && resData.Item_Count > 0)
        {
            OnlyState_AddItemInBag(resData);
        }
    }
    /// <summary>
    /// RPC:���ٳ�������
    /// </summary>
    /// <param name="subData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_SubItemOnHand(ItemData subData)
    {
        OnlyState_SubItemOnHand(subData);
    }
    public void OnlyState_SubItemOnHand(ItemData subData)
    {
        Net_ItemInHand = GameToolManager.Instance.PutOutItemSingle(Net_ItemInHand, subData);
    }
    /// <summary>
    /// RPC:�޸ĳ�����Ʒ
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnHand(ItemData oldItemData, ItemData newItemData)
    {
        OnlyState_ChangeItemOnHand(oldItemData,newItemData);
    }
    public void OnlyState_ChangeItemOnHand(ItemData oldItemData, ItemData newItemData)
    {
        if (Net_ItemInHand.Equals(oldItemData))
        {
            Net_ItemInHand = newItemData;
        }
    }
    #endregion
    #region//��Ʒ����(ͷ��)
    /// <summary>
    /// RPC:���ͷ������
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnHead(ItemData addData)
    {
        OnlyState_AddItemOnHead(addData);
    }
    public void OnlyState_AddItemOnHead(ItemData addData)
    {
        Net_ItemOnHead = GameToolManager.Instance.PutInItemSingle(Net_ItemOnHead, addData, out ItemData resData);
        if (resData.Item_ID > 0 && resData.Item_Count > 0)
        {
            OnlyState_AddItemInBag(resData);
        }
    }
    /// <summary>
    /// RPC:����ͷ������
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_SubItemOnHead(ItemData subData)
    {
        OnlyState_SubItemOnHead(subData);
    }
    public void OnlyState_SubItemOnHead(ItemData subData)
    {
        Net_ItemOnHead = GameToolManager.Instance.PutOutItemSingle(Net_ItemOnHead, subData);
    }
    /// <summary>
    /// RPC:����ͷ������
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnHead(ItemData oldItemData, ItemData newItemData)
    {
        OnlyState_ChangeItemOnHead(oldItemData, newItemData);
    }
    public void OnlyState_ChangeItemOnHead(ItemData oldItemData, ItemData newItemData)
    {
        if (Net_ItemOnHead.Equals(oldItemData))
        {
            Net_ItemOnHead = newItemData;
        }
    }
    #endregion
    #region//��Ʒ����(����)
    /// <summary>
    /// RPC:��Ӵ�������
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_AddItemOnBody(ItemData addData)
    {
        OnlyState_AddItemOnBody(addData);
    }
    public void OnlyState_AddItemOnBody(ItemData addData)
    {
        Net_ItemOnBody = GameToolManager.Instance.PutInItemSingle(Net_ItemOnBody, addData, out ItemData resData);
        if (resData.Item_ID > 0 && resData.Item_Count > 0)
        {
            OnlyState_AddItemInBag(resData);
        }
    }
    /// <summary>
    /// RPC:���ٴ�������
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_SubItemOnBody(ItemData subData)
    {
        OnlyState_SubItemOnBody(subData);
    }
    public void OnlyState_SubItemOnBody(ItemData subData)
    {
        Net_ItemOnBody = GameToolManager.Instance.PutOutItemSingle(Net_ItemOnBody, subData);
    }
    /// <summary>
    /// RPC:�޸Ĵ�������
    /// </summary>
    /// <param name="itemData"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeItemOnBody(ItemData oldItemData, ItemData newItemData)
    {
        OnlyState_ChangeItemOnBody(oldItemData, newItemData);
    }
    public void OnlyState_ChangeItemOnBody(ItemData oldItemData,ItemData newItemData)
    {
        if (Net_ItemOnBody.Equals(oldItemData))
        {
            Net_ItemOnBody = newItemData;
        }
    }
    #endregion
    #region//Buff����
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_AddBuff(short buffID,string param)
    {
        if (!Net_Buffs.Contains(buffID))
        {
            Net_Buffs.Add(buffID);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SubBuff(short buffID)
    {
        if (Net_Buffs.Contains(buffID))
        {
            Net_Buffs.Remove(buffID);
        }
    }

    #endregion
    #region//��ɫ���ܲ���
    /// <summary>
    /// ���Ĺ���״̬
    /// </summary>
    /// <param name="attacking"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcChangeAttackState(bool attacking)
    {
        actorManager_Local.AllClient_Listen_ChangeAttackState(attacking);
        if (actorManager_Local.actorAuthority.isState)
        {
            actorManager_Local.State_Listen_ChangeAttackState(attacking);
        }
    }
    /// <summary>
    /// ���Ĺ���Ŀ��
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcChangeAttackTarget(NetworkId id)
    {
        actorManager_Local.AllClient_Listen_ChangeAttackTarget(id);
        if (actorManager_Local.actorAuthority.isState)
        {
            actorManager_Local.State_Listen_ChangeAttackTarget(id);
        }
    }
    /// <summary>
    /// ʹ�ü���
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_State_NpcUseSkill(int id, Vector3 vector3, NetworkId networkId)
    {
        actorManager_Local.AllClient_Listen_NpcAction(id, vector3, networkId);
    }

    #endregion
    #region//��ɫλ��
    /// <summary>
    /// ���ض˸������λ��
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_UpdateNetworkTransform(Vector3 pos, float speed)
    {
        OnlyState_UpdateNetworkRigidbody(pos, speed);
    }
    public void OnlyState_UpdateNetworkRigidbody(Vector3 pos, float speed)
    {
        if (networkRigidbody && actorManager_Local.actorAuthority.isState)
        {
            if (networkRigidbody.Rigidbody.velocity.magnitude <= speed)
            {
                //networkRigidbody.Rigidbody.velocity = Vector2.zero;
                networkRigidbody.Rigidbody.position = (pos);
            }
        }
    }
    public void OnlyState_UpdateNetworkTran(Vector3 pos, float speed)
    {
        Debug.Log($"{speed}{pos}");
        transform.position = pos;
    }
    #endregion
    #region//��������
    /// <summary>
    /// ���ض˷��ͱ���
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendEmoji(int id)
    {
        actorManager_Local.actionManager.SendEmoji(id);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_SendText(string text, int id)
    {
        actorManager_Local.actionManager.SendText(text,id);
    }
    /// <summary>
    /// RPC:���ض�ʰ����Ʒ
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_LocalInput_PickItem(NetworkId id)
    {
        void OnlyState_PickItem(string val)
        {
            if (Object.HasStateAuthority && val.Equals("Pick"))
            {
                NetworkObject networkPlayerObject = Runner.FindObject(id);
                networkPlayerObject.GetComponent<ItemNetObj>().PickUp(out ItemData itemData);
                Runner.Despawn(networkPlayerObject);

                OnlyState_AddItemInBag(itemData);
            }
        }
        actorManager_Local.actionManager.PlayPickUp(1, OnlyState_PickItem);
    }
    /// <summary>
    /// RPC:����ֵ�ı�
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="networkId"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AllClient_HpChange(int parameter, NetworkId networkId)
    {
        actorManager_Local.AllClient_Listen_MyselfHpChange(parameter, networkId);
        if (actorManager_Local.actorAuthority.isState)
        {
            OnlyState_HpChange(parameter, networkId);
        }
    }
    private void OnlyState_HpChange(int parameter, NetworkId networkId)
    {
        if (Net_HpCur + parameter <= 0)
        {
            Net_HpCur = 0;
            Net_EnCur = 0;
            Net_SpeedCommon = 0;
            Net_SpeedMax = 0;
            RPC_AllClient_Dead();
        }
        else if (Net_HpCur + parameter > Net_HpMax)
        {
            Net_HpCur = Net_HpMax;
        }
        else
        {
            Net_HpCur += parameter;
            actorManager_Local.State_Listen_MyselfHpChange(parameter, networkId);
        }
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_AllClient_Dead()
    {
        actorManager_Local.bodyController.Dead();
        actorManager_Local.actionManager.Dead();
    }
    /// <summary>
    /// RPC:����ֵ�ı�
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_FoodChange(short parameter)
    {
        OnlyState_FoodChange(parameter);
    }
    private void OnlyState_FoodChange(short val)
    {
        Net_FoodCur = val;
    }

    /// <summary>
    /// ���ض�֧��
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_PayCoin(int val)
    {
        OnlyState_PayCoin(val);
    }
    private void OnlyState_PayCoin(int val)
    {
        Net_Coin -= val;
    }
    /// <summary>
    /// ���ض�׬Ǯ
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_EarnCoin(int val)
    {
        OnlyState_EarnCoin(val);
    }
    private void OnlyState_EarnCoin(int val)
    {
        Net_Coin += val;
    }
    /// <summary>
    /// ���ض������ͽ�
    /// </summary>
    /// <param name="val"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_LocalInput_ChangeFine(short val)
    {
        OnlyState_ChangeFine(val);
    }
    private void OnlyState_ChangeFine(short val)
    {
        Net_Fine = val;
    }
    #endregion

    #endregion

}
public struct ActorNetData : INetworkStruct
{
    public int Speed;
    public int MaxSpeed;
    public int Endurance;
    public int Point_Strength;
    public int Point_Intelligence;
    public int Point_Focus;
    public int Point_Agility;
}
