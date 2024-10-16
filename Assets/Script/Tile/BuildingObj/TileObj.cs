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
/// 场景物品基类
/// </summary>
public class TileObj : MonoBehaviour
{
    [HideInInspector]
    public MyTile bindTile;
    [SerializeField, Header("最大生命值")]
    public int CurHp;
    [SerializeField, Header("当前生命值")]
    public int MaxHp;
    [SerializeField, Header("护甲")]
    public int Armor;
    [SerializeField, Header("地块信息")]
    public string info;
    #region//瓦片生命周期
    public virtual void Start()
    {
        Init();
        Draw();
    }
    /// <summary>
    /// 绑定
    /// </summary>
    /// <param name="json"></param>
    public void Bind(MyTile tile)
    {
        bindTile = tile;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {

    }
    /// <summary>
    /// 绘制
    /// </summary>
    public virtual void Draw()
    {

    }

    #endregion
    #region//瓦片逻辑
    /// <summary>
    /// 尝试对地块造成伤害
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
    /// 尝试更新生命值
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
    /// 生命值提升
    /// </summary>
    /// <param name="val"></param>
    public virtual void HpUp(int val)
    {

    }
    /// <summary>
    /// 生命值下降
    /// </summary>
    /// <param name="val"></param>
    public virtual void HpDown(int val)
    {
        PlayDamaged();
    }
    /// <summary>
    /// 损坏
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
    #endregion
    #region//瓦片交互
    /// <summary>
    /// 玩家输入
    /// </summary>
    public virtual void PlayerInput(PlayerController player, KeyCode code)
    {

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
    /// 玩家持有
    /// </summary>
    /// <param name="player"></param>
    /// <returns>被持有</returns>
    public virtual bool PlayerHolding(PlayerController player)
    {
        return false;
    }
    /// <summary>
    /// 玩家释放
    /// </summary>
    /// <param name="player"></param>
    /// <returns>被释放</returns>
    public virtual bool PlayerRelease(PlayerController player)
    {
        return false;
    }

    #endregion
    #region//瓦片连接
    [HideInInspector]
    public bool linkAlready = false;
    /// <summary>
    /// 检查周围
    /// </summary>
    /// <param name="targetTileNames">目标格子名字</param>
    /// <param name="updateTargetTile">是否更新周围格子</param>
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
    /// 检查周围
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
    /// 连接周围
    /// </summary>
    /// <param name="linkState">连接状态</param>
    /// <param name="UpTile">上瓦片</param>
    /// <param name="DownTile">下瓦片</param>
    /// <param name="LeftTile">左瓦片</param>
    /// <param name="RightTile">右瓦片</param>
    public virtual void LinkAround(LinkState linkState, TileObj UpTile, TileObj DownTile, TileObj LeftTile, TileObj RightTile)
    {

    }
    /// <summary>
    /// 被连接
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    public virtual bool LinkFrom(Vector2Int from)
    {
        return false;
    }
    #endregion
    #region//瓦片动画
    /// <summary>
    /// 受伤动画
    /// </summary>
    public virtual void PlayDamaged()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    /// <summary>
    /// 销毁动画
    /// </summary>
    public virtual void PlayBroken()
    {
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = transform.position;
    }
    #endregion
    #region//瓦片掉落
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