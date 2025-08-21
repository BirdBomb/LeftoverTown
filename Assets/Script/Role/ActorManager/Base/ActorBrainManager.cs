using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBrainManager 
{
    private ActorManager actorManager;

    public HomePostion homePostion;
    public void SetHome(Vector3Int pos)
    {
        homePostion.isValue = true; 
        homePostion.postion = pos;
    }
    public ActivityPostion activityPostion;
    public void SetActivity(Vector3Int pos)
    {
        activityPostion.isValue = true;
        activityPostion.postion = pos;
    }
    public WorkPostion workPostion;
    public void SetWork(Vector3Int pos)
    {
        workPostion.isValue = true;
        workPostion.postion = pos;
    }
    public void ResetWork()
    {
        workPostion.isValue = false;
        workPostion.postion = Vector3Int.zero;
    }
    public SearchPostion searchPostion;
    public void SetSearch(Vector3Int pos)
    {
        searchPostion.isValue = true;
        searchPostion.postion = pos;
    }
    public void ResetSearch()
    {
        searchPostion.isValue = false;
    }
    public GlobalTime globalTime_Now;
    public void SetTime(GlobalTime globalTime)
    {
        globalTime_Now = globalTime;
    }

    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager; 
    }
    /// <summary>
    /// 周围角色
    /// </summary>
    public List<ActorManager> actorManagers_Nearby = new List<ActorManager>();
    /// <summary>
    /// 攻击角色
    /// </summary>
    public ActorManager allClient_actorManager_AttackTarget;
    /// <summary>
    /// 攻击角色网络id
    /// </summary>
    public Fusion.NetworkId allClient_actorManager_AttackTargetID;
    /// <summary>
    /// 威胁角色
    /// </summary>
    public List<ActorManager> actorManagers_ThreatenedTarget = new List<ActorManager>();
    /// <summary>
    /// 威胁角色(优先)
    /// </summary>
    public ActorManager actorManager_ThreatenedTarget;
    /// <summary>
    /// 周围物品
    /// </summary>
    public List<ItemNetObj> ItemNetObj_Nearby = new List<ItemNetObj>();
    /// <summary>
    /// 目标物品
    /// </summary>
    public ItemNetObj ItemNetObj_Target = null;
    public bool bool_AttackState = false;

}
/// <summary>
/// 角色出生点
/// </summary>
public struct HomePostion
{
    public bool isValue;
    public Vector3Int postion;
}
/// <summary>
/// 角色活动点
/// </summary>
public struct ActivityPostion
{
    public bool isValue;
    public Vector3Int postion;
}
/// <summary>
/// 角色工作点
/// </summary>
public struct WorkPostion 
{
    public bool isValue;
    public Vector3Int postion;
}
/// <summary>
/// 角色搜查点
/// </summary>
public struct SearchPostion
{
    public bool isValue;
    public Vector3Int postion;
}