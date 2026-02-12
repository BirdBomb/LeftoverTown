using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ActorBrainManager 
{
    private ActorManager actorManager;


    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
        WorldManager.Instance.GetTime(out day_Now,out hour_Now,out globalTime_Now);
    }
    #region//时间感知
    /// <summary>
    /// 当前时段
    /// </summary>
    public GlobalTime globalTime_Now;
    /// <summary>
    /// 当前小时
    /// </summary>
    public int hour_Now;
    /// <summary>
    /// 当前天数
    /// </summary>
    public int day_Now;
    public void SetTime(int day, int hour, GlobalTime globalTime)
    {
        day_Now = day;
        hour_Now = hour;
        globalTime_Now = globalTime;
    }

    #endregion
    #region//周围感知
    /// <summary>
    /// 周围角色
    /// </summary>
    public List<ActorManager> actorManagers_Nearby = new List<ActorManager>();

    #endregion
    #region//攻击感知
    /// <summary>
    /// 攻击角色
    /// </summary>
    public ActorManager allClient_actorManager_AttackTarget;
    /// <summary>
    /// 攻击角色网络id
    /// </summary>
    public Fusion.NetworkId allClient_actorManager_AttackTargetID;
    /// <summary>
    /// 当前是否是攻击状态
    /// </summary>
    public bool allClient_AttackState = false;

    #endregion
    #region//威胁感知
    /// <summary>
    /// 威胁角色
    /// </summary>
    public List<ActorManager> actorManagers_ThreatenedTarget = new List<ActorManager>();
    /// <summary>
    /// 威胁角色(优先)
    /// </summary>
    public ActorManager allClient_actorManager_ThreatenedTarget;
    /// <summary>
    /// 威胁角色网络id
    /// </summary>
    public Fusion.NetworkId allClient_actorManager_ThreatenedTargetID;

    #endregion
    #region//拾取逻辑
    /// <summary>
    /// 周围物品 
    /// </summary>
    public List<ItemNetObj> allClient_ItemNetObj_Nearby = new List<ItemNetObj>();
    /// <summary>
    /// 目标物品
    /// </summary>
    public ItemNetObj allClient_ItemNetObj_Target = null;
    /// <summary>
    /// 将最近的物品设定为目标
    /// </summary>
    /// <returns>是否有最近物体</returns>
    public bool State_FocusOnNearbyItem()
    {
        ItemNetObj target = null;
        float distance = float.MaxValue;
        for (int i = 0; i < allClient_ItemNetObj_Nearby.Count; i++)
        {
            float temp = Vector2.Distance(actorManager.transform.position, allClient_ItemNetObj_Nearby[i].transform.position);
            if (temp < distance)
            {
                distance = temp;
                target = allClient_ItemNetObj_Nearby[i];
            }
        }
        if (target != null)
        {
            /*前往目标*/
            allClient_ItemNetObj_Target = target;
            return true;
        }
        else
        {
            /*没有目标*/
            allClient_ItemNetObj_Target = null;
            return false;
        }
    }
    #endregion
    #region//地点感知
    public HomePostion state_homePostion;
    /// <summary>
    /// 设置出生点
    /// </summary>
    /// <param name="pos"></param>
    public void State_SetHomePos(Vector3Int pos)
    {
        state_homePostion.isValue = true;
        state_homePostion.position = pos;
    }
    public SleepPostion state_sleepPostion;
    /// <summary>
    /// 设置睡眠点
    /// </summary>
    /// <param name="pos"></param>
    public void State_SetSleepPos(Vector3Int pos)
    {
        state_sleepPostion.isValue = true;
        state_sleepPostion.position = pos;
    }
    public void ResetSleepPos()
    {
        state_sleepPostion.isValue = false;
    }

    public ActivityPostion state_ActivityPostion;
    /// <summary>
    /// 设置活动点
    /// </summary>
    public void State_SetActivityPos(Vector3Int pos)
    {
        state_ActivityPostion.isValue = true;
        state_ActivityPostion.position = pos;
    }
    public WorkPostion state_workPostion;
    /// <summary>
    /// 设置工作点
    /// </summary>
    public void State_SetWorkPos(Vector3Int pos)
    {
        state_workPostion.isValue = true;
        state_workPostion.position = pos;
    }
    public void State_ResetWorkPos()
    {
        state_workPostion.isValue = false;
        state_workPostion.position = Vector3Int.zero;
    }
    public SearchPostion state_searchPostion;
    /// <summary>
    /// 设置搜寻点
    /// </summary>
    /// <param name="pos"></param>
    public void State_SetSearchPos(Vector3Int pos)
    {
        state_searchPostion.isValue = true;
        state_searchPostion.position = pos;
    }
    public void State_ResetSearchPos()
    {
        state_searchPostion.isValue = false;
    }
    public FoodPositon state_foodPositon;
    /// <summary>
    /// 设置觅食点
    /// </summary>
    /// <param name="pos">用餐点位置</param>
    /// <param name="foodCount">食物总数</param>
    public void State_SetFoodPos(Vector3Int pos,int foodCount = 3)
    {
        state_foodPositon.isValue = true;
        state_foodPositon.foodCount = foodCount;
        state_foodPositon.position = pos;
    }
    public void State_ResetFoodPos()
    {
        state_foodPositon.isValue = false;
    }
    #endregion
}
/// <summary>
/// 角色出生点
/// </summary>
public struct HomePostion
{
    public bool isValue;
    public Vector3Int position;
}
/// <summary>
/// 角色活动点
/// </summary>
public struct ActivityPostion
{
    public bool isValue;
    public Vector3Int position;
}
/// <summary>
/// 角色工作点
/// </summary>
public struct WorkPostion
{
    public bool isValue;
    public Vector3Int position;
}
/// <summary>
/// 角色睡眠点
/// </summary>
public struct SleepPostion
{
    public bool isValue;
    public Vector3Int position;
}
/// <summary>
/// 角色用餐点
/// </summary>
public struct FoodPositon
{
    /// <summary>
    /// 用餐点是否赋值
    /// </summary>
    public bool isValue;
    /// <summary>
    /// 用餐点食物总数
    /// </summary>
    public int foodCount;
    /// <summary>
    /// 用餐点位置
    /// </summary>
    public Vector3Int position;
}
/// <summary>
/// 角色搜查点
/// </summary>
public struct SearchPostion
{
    public bool isValue;
    public Vector3Int position;
}