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
    public virtual void Draw(int seed)
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
    /// 角色靠近
    /// </summary>
    /// <returns></returns>
    public virtual bool ActorNearby(ActorManager actor)
    {
        return false;
    }
    /// <summary>
    /// 角色站在
    /// </summary>
    public virtual void ActorStandOn(ActorManager actor)
    {

    }
    /// <summary>
    /// 角色远离
    /// </summary>
    /// <returns></returns>
    public virtual bool ActorFaraway(ActorManager actor)
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
    /// 检查周围同名建筑(八方向)
    /// </summary>
    /// <param name="targetName"></param>
    public virtual void CheckAroundBuilding_EightSide(string targetName)
    {
        AroundState_EightSide aroundState = new AroundState_EightSide();
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (tileObjUp.bindTile.name.Equals(targetName))
            {
                aroundState.Up = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (tileObjDown.bindTile.name.Equals(targetName))
            {
                aroundState.Down = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (tileObjLeft.bindTile.name.Equals(targetName))
            {
                aroundState.Left = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (tileObjRight.bindTile.name.Equals(targetName))
            {
                aroundState.Right = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up + Vector3Int.left, out TileObj tileObjUpLeft))
        {
            if (tileObjUpLeft.bindTile.name.Equals(targetName))
            {
                aroundState.UpLeft = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up + Vector3Int.right, out TileObj tileObjUpRight))
        {
            if (tileObjUpRight.bindTile.name.Equals(targetName))
            {
                aroundState.UpRight = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down + Vector3Int.left, out TileObj tileObjDownLeft))
        {
            if (tileObjDownLeft.bindTile.name.Equals(targetName))
            {
                aroundState.DownLeft = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down + Vector3Int.right, out TileObj tileObjDownRight))
        {
            if (tileObjDownRight.bindTile.name.Equals(targetName))
            {
                aroundState.DownRight = true;
            }
        }
        LinkAround(aroundState);
    }
    /// <summary>
    /// 检查周围同名建筑(八方向)
    /// </summary>
    /// <param name="targetNames"></param>
    public virtual void CheckAroundBuilding_EightSide(List<string> targetNames)
    {
        AroundState_EightSide aroundState = new AroundState_EightSide();
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (targetNames.Contains(tileObjUp.bindTile.name))
            {
                aroundState.Up = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (targetNames.Contains(tileObjDown.bindTile.name))
            {
                aroundState.Down = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (targetNames.Contains(tileObjLeft.bindTile.name))
            {
                aroundState.Left = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (targetNames.Contains(tileObjRight.bindTile.name))
            {
                aroundState.Right = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up + Vector3Int.left, out TileObj tileObjUpLeft))
        {
            if (targetNames.Contains(tileObjUpLeft.bindTile.name))
            {
                aroundState.UpLeft = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up + Vector3Int.right, out TileObj tileObjUpRight))
        {
            if (targetNames.Contains(tileObjUpRight.bindTile.name))
            {
                aroundState.UpRight = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down + Vector3Int.left, out TileObj tileObjDownLeft))
        {
            if (targetNames.Contains(tileObjDownLeft.bindTile.name))
            {
                aroundState.DownLeft = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down + Vector3Int.right, out TileObj tileObjDownRight))
        {
            if (targetNames.Contains(tileObjDownRight.bindTile.name))
            {
                aroundState.DownRight = true;
            }
        }
        LinkAround(aroundState);
    }
    /// <summary>
    /// 检查周围同名建筑(四方向)
    /// </summary>
    /// <param name="targetName"></param>
    public virtual void CheckAroundBuilding_FourSide(string targetName)
    {
        AroundState_FourSide aroundState = new AroundState_FourSide();
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (tileObjUp.bindTile.name.Equals(targetName))
            {
                aroundState.Up = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (tileObjDown.bindTile.name.Equals(targetName))
            {
                aroundState.Down = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (tileObjLeft.bindTile.name.Equals(targetName))
            {
                aroundState.Left = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (tileObjRight.bindTile.name.Equals(targetName))
            {
                aroundState.Right = true;
            }
        }
        LinkAround(aroundState);
    }
    /// <summary>
    /// 检查周围同名建筑(四方向)
    /// </summary>
    /// <param name="targetNames"></param>
    public virtual void CheckAroundBuilding_FourSide(List<string> targetNames)
    {
        AroundState_FourSide aroundState = new AroundState_FourSide();
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (targetNames.Contains(tileObjUp.bindTile.name))
            {
                aroundState.Up = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (targetNames.Contains(tileObjDown.bindTile.name))
            {
                aroundState.Down = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (targetNames.Contains(tileObjLeft.bindTile.name))
            {
                aroundState.Left = true;
            }
        }
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (targetNames.Contains(tileObjRight.bindTile.name))
            {
                aroundState.Right = true;
            }
        }
        LinkAround(aroundState);
    }
    /// <summary>
    /// 检查周围同名地板(八方向)
    /// </summary>
    /// <param name="targetName"></param>
    public virtual void CheckAroundFloor_EightSide(string targetName)
    {
        AroundState_EightSide aroundState = new AroundState_EightSide();
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (tileObjUp.bindTile.name.Equals(targetName))
            {
                aroundState.Up = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (tileObjDown.bindTile.name.Equals(targetName))
            {
                aroundState.Down = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (tileObjLeft.bindTile.name.Equals(targetName))
            {
                aroundState.Left = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (tileObjRight.bindTile.name.Equals(targetName))
            {
                aroundState.Right = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.up + Vector3Int.left, out TileObj tileObjUpLeft))
        {
            if (tileObjUpLeft.bindTile.name.Equals(targetName))
            {
                aroundState.UpLeft = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.up + Vector3Int.right, out TileObj tileObjUpRight))
        {
            if (tileObjUpRight.bindTile.name.Equals(targetName))
            {
                aroundState.UpRight = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.down + Vector3Int.left, out TileObj tileObjDownLeft))
        {
            if (tileObjDownLeft.bindTile.name.Equals(targetName))
            {
                aroundState.DownLeft = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.down + Vector3Int.right, out TileObj tileObjDownRight))
        {
            if (tileObjDownRight.bindTile.name.Equals(targetName))
            {
                aroundState.DownRight = true;
            }
        }
        LinkAround(aroundState);
    }
    /// <summary>
    /// 检查周围同名地板(八方向)
    /// </summary>
    /// <param name="targetNames"></param>
    public virtual void CheckAroundFloor_EightSide(List<string> targetNames)
    {
        AroundState_EightSide aroundState = new AroundState_EightSide();
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (targetNames.Contains(tileObjUp.bindTile.name))
            {
                aroundState.Up = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (targetNames.Contains(tileObjDown.bindTile.name))
            {
                aroundState.Down = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (targetNames.Contains(tileObjLeft.bindTile.name))
            {
                aroundState.Left = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (targetNames.Contains(tileObjRight.bindTile.name))
            {
                aroundState.Right = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.up + Vector3Int.left, out TileObj tileObjUpLeft))
        {
            if (targetNames.Contains(tileObjUpLeft.bindTile.name))
            {
                aroundState.UpLeft = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.up + Vector3Int.right, out TileObj tileObjUpRight))
        {
            if (targetNames.Contains(tileObjUpRight.bindTile.name))
            {
                aroundState.UpRight = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.down + Vector3Int.left, out TileObj tileObjDownLeft))
        {
            if (targetNames.Contains(tileObjDownLeft.bindTile.name))
            {
                aroundState.DownLeft = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.down + Vector3Int.right, out TileObj tileObjDownRight))
        {
            if (targetNames.Contains(tileObjDownRight.bindTile.name))
            {
                aroundState.DownRight = true;
            }
        }
        LinkAround(aroundState);

    }
    /// <summary>
    /// 检查周围同名地板(四方向)
    /// </summary>
    /// <param name="targetName"></param>
    public virtual void CheckAroundFloor_FourSide(string targetName)
    {
        AroundState_FourSide aroundState = new AroundState_FourSide();
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (tileObjUp.bindTile.name.Equals(targetName))
            {
                aroundState.Up = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (tileObjDown.bindTile.name.Equals(targetName))
            {
                aroundState.Down = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (tileObjLeft.bindTile.name.Equals(targetName))
            {
                aroundState.Left = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (tileObjRight.bindTile.name.Equals(targetName))
            {
                aroundState.Right = true;
            }
        }
        LinkAround(aroundState);
    }
    /// <summary>
    /// 检查周围同名地板(四方向)
    /// </summary>
    /// <param name="targetNames"></param>
    public virtual void CheckAroundFloor_FourSide(List<string> targetNames)
    {
        AroundState_FourSide aroundState = new AroundState_FourSide();
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp))
        {
            if (targetNames.Contains(tileObjUp.bindTile.name))
            {
                aroundState.Up = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            if (targetNames.Contains(tileObjDown.bindTile.name))
            {
                aroundState.Down = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObjLeft))
        {
            if (targetNames.Contains(tileObjLeft.bindTile.name))
            {
                aroundState.Left = true;
            }
        }
        if (MapManager.Instance.GetFloorObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObjRight))
        {
            if (targetNames.Contains(tileObjRight.bindTile.name))
            {
                aroundState.Right = true;
            }
        }
        LinkAround(aroundState);
    }

    /// <summary>
    /// 连接周围
    /// </summary>
    /// <param name="aroundState">周围状态</param>
    public virtual void LinkAround(AroundState_EightSide aroundState)
    {

    }
    /// <summary>
    /// 连接周围
    /// </summary>
    /// <param name="aroundState">周围状态</param>
    public virtual void LinkAround(AroundState_FourSide aroundState)
    {

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
public struct AroundState_EightSide
{
    public bool Left;
    public bool Right;
    public bool Up;
    public bool Down;
    public bool UpLeft;
    public bool UpRight;
    public bool DownLeft;
    public bool DownRight;
}
public struct AroundState_FourSide
{
    public bool Left;
    public bool Right;
    public bool Up;
    public bool Down;
}