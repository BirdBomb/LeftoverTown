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
    /// <summary>
    /// 绑定
    /// </summary>
    public virtual void Bind(BuildingTile tile, out BuildingObj obj)
    {
        buildingTile = tile;
        obj = this;
    }
    public virtual void Start()
    {
        
    }
    #region//交互
    /// <summary>
    /// 输入
    /// </summary>
    public virtual void ActorInputKeycode(ActorManager actor, KeyCode code)
    {

    }
    /// <summary>
    /// 持有
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual bool PlayerHolding(PlayerCoreLocal player)
    {
        return false;
    }
    /// <summary>
    /// 释放
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public virtual bool PlayerRelease(PlayerCoreLocal player)
    {
        return false;
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
    #endregion
    #region//逻辑
    /// <summary>
    /// 绘制
    /// </summary>
    public virtual void Draw()
    {

    }
    /// <summary>
    /// 受伤
    /// </summary>
    public virtual void TakeDamage(int val)
    {

    }
    /// <summary>
    /// 破坏
    /// </summary>
    public virtual void Broken()
    {

    }
    /// <summary>
    /// 掉落
    /// </summary>
    public virtual void Loot()
    {

    }
    /// <summary>
    /// 更改信息
    /// </summary>
    public virtual void ChangeInfo(string info)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_UpdateBuildingInfo
        {
            pos = buildingTile.tilePos,
            info = info
        });
    }
    /// <summary>
    /// 更改HP
    /// </summary>
    public virtual void ChangeHp()
    {

    }
    /// <summary>
    /// 更新信息
    /// </summary>
    public virtual void UpdateInfo(string info)
    {
        this.info = info;
    }
    /// <summary>
    /// 更新HP
    /// </summary>
    public virtual void UpdateHP(int hp)
    {

    }
    #endregion
}
