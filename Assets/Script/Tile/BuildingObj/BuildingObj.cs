using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingObj : MonoBehaviour
{
    [HideInInspector]
    public BuildingTile buildingTile;
    [HideInInspector]
    public string info;
    [HideInInspector]
    public int local_Hp = 0;
    [HideInInspector]
    public int local_Armor = 0;
    /// <summary>
    /// ��
    /// </summary>
    public virtual void Bind(BuildingTile tile, out BuildingObj obj)
    {
        buildingTile = tile;
        Init(buildingTile.tileID);
        obj = this;
    }
    /// <summary>
    /// ��ʼ
    /// </summary>
    public virtual void Init(int id)
    {
        BuildingConfig config = BuildingConfigData.GetBuildingConfig(id);
        Local_SetHp(config.Building_Hp);
        Local_SetArmor(config.Building_Armor);
    }
    public virtual void Start()
    {
        
    }
    #region//����
    /// <summary>
    /// ��ɫ����
    /// </summary>
    public virtual void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {

    }
    /// <summary>
    /// ��Ҹ���
    /// </summary>
    /// <param name="on"></param>
    public virtual void All_PlayerHighlight(bool on)
    {

    }
    /// <summary>
    /// ��ҿ���
    /// </summary>
    public virtual void All_PlayerNearby()
    {

    }
    /// <summary>
    /// ���Զ��
    /// </summary>
    public virtual void All_PlayerFaraway()
    {

    }
    /// <summary>
    /// ��ɫ����
    /// </summary>
    /// <returns></returns>
    public virtual bool All_ActorNearby(ActorManager actor)
    {
        return false;
    }
    /// <summary>
    /// ��ɫվ��
    /// </summary>
    public virtual void All_ActorStandOn(ActorManager actor)
    {

    }
    /// <summary>
    /// ��ɫԶ��
    /// </summary>
    /// <returns></returns>
    public virtual bool All_ActorFaraway(ActorManager actor)
    {
        return false;
    }
    public virtual void OpenOrCloseHighlightUI(bool open)
    {
    }
    public virtual void OpenOrCloseAwakeUI(bool open)
    {
    }
    public virtual void OpenOrCloseUI(bool open)
    {
    }
    /// <summary>
    /// �Ƿ���Ը���
    /// </summary>
    /// <returns></returns>
    public virtual bool CanHighlight()
    {
        return false;
    }
    #endregion
    #region//��Ϣ
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public virtual void Local_ChangeInfo(string info)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeBuildingInfo
        {
            pos = buildingTile.tilePos,
            info = info
        });
    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public virtual void All_UpdateInfo(string info)
    {
        this.info = info;
    }
    #endregion
    #region//����
    /// <summary>
    /// ����
    /// </summary>
    public virtual void All_Draw()
    {

    }
    /// <summary>
    /// �ƻ�
    /// </summary>
    public virtual void All_Broken()
    {
        All_PlayBroken();
        if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
        {
            MessageBroker.Default.Publish(new MapEvent.MapEvent_State_ChangeBuildingArea()
            {
                buildingID = 0,
                buildingPos = buildingTile.tilePos,
                areaSize = BuildingConfigData.GetBuildingConfig(buildingTile.tileID).Building_Size
            });
        }
    }
    #endregion
    #region//����
    /// <summary>
    /// ���������
    /// </summary>
    public virtual List<ItemData> State_GetLootItem(List<BaseLootInfo> baseLootInfos, List<ExtraLootInfo> extraLootInfos)
    {
        List<ItemData> lootItemDatas = new List<ItemData>();
        if (baseLootInfos != null)
        {
            for (int i = 0; i < baseLootInfos.Count; i++)
            {
                lootItemDatas.Add(Local_GetItemData(baseLootInfos[i].ID, (short)new System.Random().Next(baseLootInfos[i].CountMin, baseLootInfos[i].CountMax + 1)));
            }
        }
        if (extraLootInfos != null)
        {
            for (int i = 0; i < extraLootInfos.Count; i++)
            {
                int random = new System.Random().Next(0, 1000);
                if (random <= extraLootInfos[i].Weight)
                {
                    lootItemDatas.Add(Local_GetItemData(extraLootInfos[i].ID, extraLootInfos[i].Count));
                }
            }
        }
        return lootItemDatas;
    }
    /// <summary>
    /// ���ɵ�����
    /// </summary>
    /// <param name="datas"></param>
    public void State_CreateLootItem(List<ItemData> datas)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            float angle = i * (360 / datas.Count);
            float angleRad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(angleRad) * 0.5f;
            float y = Mathf.Sin(angleRad) * 0.5f;

            Vector3 position = new Vector3(x, y, 0);
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = datas[i],
                itemOwner = new NetworkId(),
                pos = position + transform.position,
            });
        }
    }
    public ItemData Local_GetItemData(short ID, short Count)
    {
        Type type = Type.GetType("Item_" + ID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(ID, out ItemData initData);
        initData.Item_Count = Count;
        return initData;
    }
    #endregion
    #region//����ֵ
    /// <summary>
    /// ���ض�����˺�
    /// </summary>
    /// <param name="val"></param>
    public virtual void Local_TakeDamage(int val, DamageState damageState,ActorNetManager from)
    {
        if (val > local_Armor)
        {
            Local_ChangeHp(-val);
        }
        else
        {
            val = 0;
        }

        Vector2 offset = 0.025f * new Vector2(new System.Random().Next(-10, 10), new System.Random().Next(-5, 5));
        Effect_NumUI damageUI = PoolManager.Instance.GetObject("Effect/Effect_NumUI").GetComponent<Effect_NumUI>();
        damageUI.transform.position = (Vector2)transform.position + Vector2.up + offset;
        damageUI.PlayShow((-val).ToString(), Color.white, offset);
    }
    /// <summary>
    /// ���ض���Ч�˺�
    /// </summary>
    public virtual void Local_IneffectiveDamage(DamageState damageState, ActorNetManager from)
    {
        if (from.actorManager_Local.actorAuthority.isPlayer)
        {
            //MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_SendText()
            //{
            //    text = "��Ӧ�û�������",
            //});
        }
        Local_ChangeHp(0);
    }
    /// <summary>
    /// ���ض��޸�����ֵ
    /// </summary>
    /// <param name="offset"></param>
    public virtual void Local_ChangeHp(int offset)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeBuildingHp()
        {
            pos = buildingTile.tilePos,
            offset = offset
        });
    }
    public virtual void Local_SetHp(int hp)
    {
        local_Hp = hp;
    }
    public virtual void Local_SetArmor(int armor)
    {
        local_Armor = armor;
    }
    public virtual void All_UpdateHP(int newHp)
    {
        if (newHp <= 0) { All_Broken(); }
        else
        {
            if (newHp <= local_Hp)
            {
                All_HpDown(newHp - local_Hp);
            }
            else
            {
                All_HpUp(newHp - local_Hp);
            }
        }
        Local_SetHp(newHp);
    }
    public virtual void All_HpDown(int offset)
    {
        All_PlayHpDown(offset);
    }
    public virtual void All_HpUp(int offset)
    {

    }
    #endregion
    #region//��Ч
    public virtual void All_PlayHpDown(int offset)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    public virtual void All_PlayBroken()
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = transform.position;
    }
    #endregion
}
