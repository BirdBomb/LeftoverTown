using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Random = UnityEngine.Random;
using UnityEditor;
using static UnityEditor.Progress;
using DG.Tweening;
using System.Linq;
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
    #region//��Ƭ��������
    public virtual void Start()
    {
        Init();
        Draw();
    }
    /// <summary>
    /// ��
    /// </summary>
    /// <param name="json"></param>
    public void Bind(MyTile tile)
    {
        bindTile = tile;
    }
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void Init()
    {

    }
    /// <summary>
    /// ����
    /// </summary>
    public virtual void Draw()
    {

    }

    #endregion
    #region//��Ƭ�߼�
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
    #endregion
    #region//��Ƭ����
    /// <summary>
    /// �������
    /// </summary>
    public virtual void PlayerInput(PlayerController player, KeyCode code)
    {

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
    /// �����Χ
    /// </summary>
    /// <param name="targetTileNames">Ŀ���������</param>
    /// <param name="updateTargetTile">�Ƿ������Χ����</param>
    public virtual void CheckAround(List<string> targetTileNames, bool updateTargetTile)
    {
        int linkState = 0;
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (targetTileNames.Contains(tileObjUp.bindTile.name))
            {
                up = true;
                if (updateTargetTile)
                {
                    tileObjUp.CheckAround(targetTileNames, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (targetTileNames.Contains(tileObjDown.bindTile.name))
            {
                down = true;
                if (updateTargetTile)
                {
                    tileObjDown.CheckAround(targetTileNames, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (targetTileNames.Contains(tileObjLeft.bindTile.name))
            {
                left = true;
                if (updateTargetTile)
                {
                    tileObjLeft.CheckAround(targetTileNames, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (targetTileNames.Contains(tileObjRight.bindTile.name))
            {
                right = true;
                if (updateTargetTile)
                {
                    tileObjRight.CheckAround(targetTileNames, false);
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
    /// �����Χ
    /// </summary>
    /// <param name="targetTileName"></param>
    /// <param name="updateTargetTile"></param>
    public virtual void CheckAround(string targetTileName, bool updateTargetTile)
    {
        int linkState = 0;
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (tileObjUp.bindTile.name.Equals(targetTileName))
            {
                up = true;
                if (updateTargetTile)
                {
                    tileObjUp.CheckAround(targetTileName, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (tileObjDown.bindTile.name.Equals(targetTileName))
            {
                down = true;
                if (updateTargetTile)
                {
                    tileObjDown.CheckAround(targetTileName, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (tileObjLeft.bindTile.name.Equals(targetTileName))
            {
                left = true;
                if (updateTargetTile)
                {
                    tileObjLeft.CheckAround(targetTileName, false);
                }
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (tileObjRight.bindTile.name.Equals(targetTileName))
            {
                right = true;
                if (updateTargetTile)
                {
                    tileObjRight.CheckAround(targetTileName, false);
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
    /// ������Χ
    /// </summary>
    /// <param name="linkState">����״̬</param>
    /// <param name="UpTile">����Ƭ</param>
    /// <param name="DownTile">����Ƭ</param>
    /// <param name="LeftTile">����Ƭ</param>
    /// <param name="RightTile">����Ƭ</param>
    public virtual void LinkAround(LinkState linkState, TileObj UpTile, TileObj DownTile, TileObj LeftTile, TileObj RightTile)
    {

    }
    /// <summary>
    /// ������
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    public virtual bool LinkFrom(Vector2Int from)
    {
        return false;
    }
    #endregion
    #region//��Ƭ����
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
    #region//��Ƭ����
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