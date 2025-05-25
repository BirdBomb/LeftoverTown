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
    [Header("��Ϣ")]
    public string info;
    [Header("����ֵ")]
    public int hp = 0;
    [Header("����")]
    public int armor = 0;
    /// <summary>
    /// ��
    /// </summary>
    public virtual void Bind(BuildingTile tile, out BuildingObj obj)
    {
        buildingTile = tile;
        obj = this;
    }
    public virtual void Start()
    {
        
    }
    #region//����
    /// <summary>
    /// ����
    /// </summary>
    public virtual void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual bool All_PlayerHolding(PlayerCoreLocal player)
    {
        return false;
    }
    /// <summary>
    /// �ͷ�
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual bool All_PlayerRelease(PlayerCoreLocal player)
    {
        return false;
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
                lootItemDatas.Add(State_GetItemData(baseLootInfos[i].ID, (short)new System.Random().Next(baseLootInfos[i].CountMin, baseLootInfos[i].CountMax + 1)));
            }
        }
        if (extraLootInfos != null)
        {
            for (int i = 0; i < extraLootInfos.Count; i++)
            {
                int random = new System.Random().Next(0, 1000);
                if (random <= extraLootInfos[i].Weight)
                {
                    lootItemDatas.Add(State_GetItemData(extraLootInfos[i].ID, extraLootInfos[i].Count));
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
                pos = position + transform.position,
            });
        }
    }
    public ItemData State_GetItemData(short ID, short Count)
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
    public virtual void Local_TakeDamage(int val)
    {
        if (val > armor)
        {
            val -= armor;
            Local_ChangeHp(-val);
        }
        else
        {
            val = 0;
        }

        Vector2 offset = 0.025f * new Vector2(new System.Random().Next(-10, 10), new System.Random().Next(-5, 5));
        Effect_DamageUI damageUI = PoolManager.Instance.GetObject("Effect/Effect_DamageUI").GetComponent<Effect_DamageUI>();
        damageUI.transform.position = (Vector2)transform.position + Vector2.up + offset;
        damageUI.PlayShow((-val).ToString(), Color.white, offset);
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
    public virtual void All_UpdateHP(int newHp)
    {
        if (newHp <= 0) { All_Broken(); }
        if (newHp < hp)
        {
            All_HpDown(hp - newHp);
        }
        else
        {
            All_HpUp(newHp - hp);
        }
        hp = newHp;
    }
    public virtual void All_HpDown(int offset)
    {
        All_PlayHpDown();
    }
    public virtual void All_HpUp(int offset)
    {

    }
    #endregion
    #region//��Ч
    public virtual void All_PlayHpDown()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    public virtual void All_PlayBroken()
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = transform.position;
    }
    #endregion
}
