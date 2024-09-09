using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Random = UnityEngine.Random;
using UnityEditor;
/// <summary>
/// 场景物品基类
/// </summary>
public class TileObj : MonoBehaviour
{
    public MyTile bindTile;
    [SerializeField, Header("最大生命值")]
    public int CurHp;
    [SerializeField, Header("当前生命值")]
    public int MaxHp;
    [SerializeField, Header("护甲")]
    public int Armor;
    [SerializeField, Header("地块信息")]
    public string info;
    private bool breaking = false;
    #region//基本逻辑
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="json"></param>
    public virtual void Init(MyTile tile)
    {
        bindTile = tile;
    }
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="json"></param>
    public virtual void Load(string json)
    {
        Debug.Log(json);
        info = json;
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="json"></param>
    public virtual void Save(out string json)
    {
        json = info;
    }

    /// <summary>
    /// 交互
    /// </summary>
    public virtual void Invoke(PlayerController player,KeyCode code)
    {

    }
    /// <summary>
    /// 尝试改变生命值
    /// </summary>
    /// <param name="newHp"></param>
    public virtual void TryToChangeHp(int val)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_TakeDamage()
        {
            tileObj = this,
            damage = val
        });
    }
    /// <summary>
    /// 尝试更新生命值
    /// </summary>
    /// <param name="newHp"></param>
    public virtual void TryToUpdateHp(int newHp)
    {
        CurHp = newHp;
    }
    /// <summary>
    /// 尝试改变地块信息
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
    /// 尝试更新地块信息
    /// </summary>
    public virtual void TryToUpdateInfo(string info)
    {
        this.info = info;
    }

    /// <summary>
    /// 玩家靠近
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerNearby(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// 玩家远离
    /// </summary>
    /// <returns></returns>
    public virtual bool PlayerFaraway(PlayerController player)
    {
        return false;
    }

    /// <summary>
    /// 监听破坏后尝试销毁
    /// </summary>
    public virtual void TryToDestroyMyObj()
    {
        DestroyMyObj();
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void DestroyMyObj()
    {
        Destroy(gameObject);
    }
    #endregion
    #region//瓦片连接
    [HideInInspector]
    public bool linkAlready = false;
    /// <summary>
    /// 检查四周
    /// </summary>
    public virtual void CheckAround(string targetBuilding,bool updateTargetBuilding)
    {
        int linkState = 0;
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;
        if (MapManager.Instance.GetBuildObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
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
        if (MapManager.Instance.GetBuildObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
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
        if (MapManager.Instance.GetBuildObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
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
        if (MapManager.Instance.GetBuildObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
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
    /// 更新连接状态
    /// </summary>
    /// <param name="linkState"></param>
    public virtual void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {

    }
    /// <summary>
    /// 被连接
    /// </summary>
    public virtual bool BeLink(Vector2Int from)
    {
        return false;
    }
    #endregion
    #region//播放动画
    /// <summary>
    /// 受伤动画
    /// </summary>
    public virtual void PlayDamagedAnim()
    {

    }
    /// <summary>
    /// 销毁动画
    /// </summary>
    public virtual void PlayBreakAnim()
    {

    }
    #endregion
    #region//掉落
    [SerializeField, Header("掉落列表")]
    public List<LootInfo> LootList = new List<LootInfo>();
    [SerializeField, Header("掉落数量")]
    public int LootCount;

    /// <summary>
    /// 掉落
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
    /// 根据权重获得一个掉落物id
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
    [SerializeField,Header("编号")]
    public short ID;
    [SerializeField,Header("权重")]
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