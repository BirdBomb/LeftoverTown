using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Random = UnityEngine.Random;
using UnityEditor;
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
    [SerializeField, Header("地块信息")]
    public string info;
    #region//瓦片生命周期
    public virtual void Start()
    {
        Init();
    }
    public virtual void OnDestroy()
    {

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
        if (damage > BuildingConfigData.GetBuildingConfig(bindTile.config_tileID).Building_Armor)
        {
            damage -= BuildingConfigData.GetBuildingConfig(bindTile.config_tileID).Building_Armor;
            Vector2 offset = 0.01f * new Vector2(new System.Random().Next(-10, 10), new System.Random().Next(-10, 10));
            GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + Vector2.up + offset);
            obj_num.GetComponent<UI_DamageNum>().Play((-damage).ToString(), Color.white, 48);

            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_TakeDamage()
            {
                damage = damage
            });
        }
        else
        {
            Vector2 offset = 0.01f * new Vector2(new System.Random().Next(-10, 10), new System.Random().Next(-10, 10));
            GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + Vector2.up + offset);
            obj_num.GetComponent<UI_DamageNum>().Play("伤害过低", Color.gray, 24);
        }
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
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuildingArea()
        {
            buildingID = 0,
            buildingPos = bindTile._posInCell,
            areaSize = BuildingConfigData.GetBuildingConfig(bindTile.config_tileID).Building_Size
        });
    }
    /// <summary>
    /// 尝试改变地块信息
    /// </summary>
    public virtual void TryToChangeInfo(string info)
    {
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
    public virtual void ActorInputKeycode(ActorManager actor, KeyCode code)
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
    public virtual bool PlayerHolding(PlayerCoreLocal player)
    {
        return false;
    }
    /// <summary>
    /// 玩家释放
    /// </summary>
    /// <param name="player"></param>
    /// <returns>被释放</returns>
    public virtual bool PlayerRelease(PlayerCoreLocal player)
    {
        return false;
    }

    #endregion
    #region//瓦片连接
    [HideInInspector]
    public bool linkAlready = false;

    public virtual void CheckAroundBuilding(int id)
    {

    }
    /// <summary>
    /// 连接周围
    /// </summary>
    /// <param name="aroundState">周围状态</param>
    public virtual void LinkAround(AroundState_EightSide aroundState)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public virtual void UpdateAround(int code)
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
    [SerializeField, Header("基本掉落列表")]
    public List<BaseLootInfo> baseLoots = new List<BaseLootInfo>();
    [SerializeField, Header("额外掉落列表")]
    public List<ExtraLootInfo> extraLoots = new List<ExtraLootInfo>();

    /// <summary>
    /// 掉落
    /// </summary>
    public virtual void Loot()
    {
        for (int i = 0; i < baseLoots.Count; i++)
        {
            Random.InitState(new System.Random().Next(0, 500));
            Vector3 offset = Random.insideUnitCircle.normalized * 0.25f;
            int count = new System.Random().Next(baseLoots[i].CountMin, baseLoots[i].CountMax + 1);
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = GetItemData(baseLoots[i].ID, (short)count),
                pos = transform.position - new Vector3(0, 0.1f, 0) + offset
            });
        }
        for (int i = 0; i < extraLoots.Count; i++)
        {
            Random.InitState(new System.Random().Next(0, 500));
            Vector3 offset = Random.insideUnitCircle.normalized * 0.25f;
            int random = new System.Random().Next(0, 1000);
            if (random <= extraLoots[i].Weight)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = GetItemData(extraLoots[i].ID, extraLoots[i].Count),
                    pos = transform.position - new Vector3(0, 0.1f, 0)+ offset
                });
            }
        }
    }
    private ItemData GetItemData(short ID,short Count)
    {
        Type type = Type.GetType("Item_" + ID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(ID, out ItemData initData);
        initData.Item_Count = Count;
        return initData;
    }
    #endregion
}
[Serializable]
public struct BaseLootInfo
{
    [SerializeField, Header("基本掉落物编号")]
    public short ID;
    [SerializeField, Header("最小掉落物数量")]
    public short CountMin;
    [SerializeField, Header("最大掉落物数量")]
    public short CountMax;
}
[Serializable]
public struct ExtraLootInfo
{
    [SerializeField, Header("额外掉落物编号")]
    public short ID;
    [SerializeField, Header("额外掉落物数量")]
    public short Count;
    [SerializeField, Header("额外掉落物权重(1/1000)")]
    public short Weight;
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