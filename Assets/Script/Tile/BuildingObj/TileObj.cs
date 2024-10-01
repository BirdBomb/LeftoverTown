using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Random = UnityEngine.Random;
using UnityEditor;
using static UnityEditor.Progress;
using DG.Tweening;
/// <summary>
/// ������Ʒ����
/// </summary>
public class TileObj : MonoBehaviour
{
    [HideInInspector]
    public MyTile bindTile;
    [SerializeField, Header("�������ֵ")]
    public int CurHp;
    [SerializeField, Header("��ǰ����ֵ")]
    public int MaxHp;
    [SerializeField, Header("����")]
    public int Armor;
    [SerializeField, Header("�ؿ���Ϣ")]
    public string info;
    private bool breaking = false;
    #region//�����߼�
    private void Start()
    {
        Draw();
    }
    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="json"></param>
    public virtual void Init(MyTile tile)
    {
        bindTile = tile;
    }
    /// <summary>
    /// ��ȡ
    /// </summary>
    /// <param name="json"></param>
    public virtual void Load(string json)
    {
        info = json;
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="json"></param>
    public virtual void Save(out string json)
    {
        json = info;
    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Invoke(PlayerController player,KeyCode code)
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Draw()
    {

    }
    /// <summary>
    /// ���ԶԵؿ�����˺�
    /// </summary>
    /// <param name="damage"></param>
    public void TryToTakeDamage(int damage)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_TakeDamage()
        {
            tileObj = this,
            damage = damage
        });
    }
    /// <summary>
    /// ���Ը�������ֵ
    /// </summary>
    /// <param name="newHp"></param>
    public void TryToUpdateHp(int newHp)
    {
        if (newHp <= 0) { Broken(); }
        if (newHp < CurHp)
        {
            HpDown(CurHp - newHp);
        }
        else
        {
            HpUp(newHp - CurHp);
        }
        CurHp = newHp;
    }
    /// <summary>
    /// ����ֵ����
    /// </summary>
    /// <param name="val"></param>
    public virtual void HpUp(int val)
    {

    }
    /// <summary>
    /// ����ֵ�½�
    /// </summary>
    /// <param name="val"></param>
    public virtual void HpDown(int val)
    {
        PlayDamaged();
    }
    /// <summary>
    /// ��
    /// </summary>
    public virtual void Broken()
    {
        Loot();
        PlayBroken();
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
        {
            buildingID = 0,
            buildingPos = bindTile._posInCell
        });
    }
    /// <summary>
    /// ���Ըı�ؿ���Ϣ
    /// </summary>
    public virtual void TryToChangeInfo(string info)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_UpdateBuildingInfo
        {
            tileObj = this,
            tileInfo = info
        });
    }
    /// <summary>
    /// ���Ը��µؿ���Ϣ
    /// </summary>
    public virtual void TryToUpdateInfo(string info)
    {
        this.info = info;
    }

    /// <summary>
    /// ��ҿ���
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerNearby(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// ���Զ��
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerFaraway(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// ��ҳ���
    /// </summary>
    /// <param name="player"></param>
    /// <returns>������</returns>
    public virtual bool PlayerHolding(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// ����ͷ�
    /// </summary>
    /// <param name="player"></param>
    /// <returns>���ͷ�</returns>
    public virtual bool PlayerRelease(PlayerController player)
    {
        return false;
    }
    #endregion
    #region//��Ƭ����
    [HideInInspector]
    public bool linkAlready = false;
    /// <summary>
    /// �������
    /// </summary>
    public virtual void CheckAround(string targetBuilding,bool updateTargetBuilding)
    {
        int linkState = 0;
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (tileObjUp.bindTile.name.Equals(targetBuilding))
            {
                up = true;
                if (updateTargetBuilding)
                {
                    tileObjUp.CheckAround(targetBuilding, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (tileObjDown.bindTile.name.Equals(targetBuilding))
            {
                down = true;
                if (updateTargetBuilding)
                {
                    tileObjDown.CheckAround(targetBuilding, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (tileObjLeft.bindTile.name.Equals(targetBuilding))
            {
                left = true;
                if (updateTargetBuilding)
                {
                    tileObjLeft.CheckAround(targetBuilding, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (tileObjRight.bindTile.name.Equals(targetBuilding))
            {
                right = true;
                if (updateTargetBuilding)
                {
                    tileObjRight.CheckAround(targetBuilding, false);
                }
            }
        }
        if (up) { linkState += 8; }
        else { linkState += 0; }
        if (down) { linkState += 4; }
        else { linkState += 0; }
        if (left) { linkState += 2; }
        else { linkState += 0; }
        if (right) { linkState += 1; }
        else { linkState += 0; }
        LinkAround((LinkState)linkState, tileObjUp, tileObjDown, tileObjLeft, tileObjRight);
    }
    /// <summary>
    /// ��������״̬
    /// </summary>
    /// <param name="linkState"></param>
    public virtual void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {

    }
    /// <summary>
    /// ������
    /// </summary>
    public virtual bool BeLink(Vector2Int from)
    {
        return false;
    }
    #endregion
    #region//���Ŷ���
    /// <summary>
    /// ���˶���
    /// </summary>
    public virtual void PlayDamaged()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    /// <summary>
    /// ���ٶ���
    /// </summary>
    public virtual void PlayBroken()
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = transform.position;
    }
    #endregion
    #region//����
    [SerializeField, Header("�����б�")]
    public List<LootInfo> LootList = new List<LootInfo>();
    [SerializeField, Header("��������")]
    public int LootCount;

    /// <summary>
    /// ����
    /// </summary>
    public virtual void Loot()
    {
        for (int i = 0; i < LootCount; i++)
        {
            ItemData item = GetNextLootItem();
            if (item.Item_ID != 0)
            {
                item.Item_Seed = item.Item_Seed + i;
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = item,
                    pos = transform.position - new Vector3(0, 0.1f, 0)
                });
            }
        }
    }
    /// <summary>
    /// ����Ȩ�ػ��һ��������id
    /// </summary>
    /// <returns></returns>
    private ItemData GetNextLootItem()
    {
        int weight_Main = 0;
        int weight_temp = 0;
        int random;
        for (int j = 0; j < LootList.Count; j++)
        {
            weight_Main += LootList[j].Weight;
        }
        Random.InitState(System.DateTime.Now.Second);
        random = Random.Range(0, weight_Main);
        for (int j = 0; j < LootList.Count; j++)
        {
            weight_temp += LootList[j].Weight;
            if (weight_temp > random)
            {
                Type type = Type.GetType("Item_" + LootList[j].ID.ToString());
                ItemData itemData = new ItemData
                    (LootList[j].ID,
                     MapManager.Instance.mapSeed + (int)(System.DateTime.Now.Ticks * 1000),
                     1,
                     0,
                     0,
                     0,
                     new ContentData());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(itemData, out ItemData initData);
                return initData;
            }
        }
        return new ItemData();
    }

    #endregion
}
[Serializable]
public struct LootInfo
{
    [SerializeField,Header("���")]
    public short ID;
    [SerializeField,Header("Ȩ��")]
    public int Weight;
}
public enum LinkState
{
    FourSide,
    ThreeSide_RightMissing,
    ThreeSide_LeftMissing,
    TwoSide_UpDown,
    ThreeSide_DownMissing,
    TwoSide_UpLeft,
    TwoSide_UpRight,
    OneSide_Up,
    ThreeSide_UpMissing,
    TwoSide_DownLeft,
    TwoSide_DownRight,
    OneSide_Down,
    TwoSide_LeftRight,
    OneSide_Left,
    OneSide_Right,
    NoneSide,
}