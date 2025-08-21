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
    /// ��Χ��ɫ
    /// </summary>
    public List<ActorManager> actorManagers_Nearby = new List<ActorManager>();
    /// <summary>
    /// ������ɫ
    /// </summary>
    public ActorManager allClient_actorManager_AttackTarget;
    /// <summary>
    /// ������ɫ����id
    /// </summary>
    public Fusion.NetworkId allClient_actorManager_AttackTargetID;
    /// <summary>
    /// ��в��ɫ
    /// </summary>
    public List<ActorManager> actorManagers_ThreatenedTarget = new List<ActorManager>();
    /// <summary>
    /// ��в��ɫ(����)
    /// </summary>
    public ActorManager actorManager_ThreatenedTarget;
    /// <summary>
    /// ��Χ��Ʒ
    /// </summary>
    public List<ItemNetObj> ItemNetObj_Nearby = new List<ItemNetObj>();
    /// <summary>
    /// Ŀ����Ʒ
    /// </summary>
    public ItemNetObj ItemNetObj_Target = null;
    public bool bool_AttackState = false;

}
/// <summary>
/// ��ɫ������
/// </summary>
public struct HomePostion
{
    public bool isValue;
    public Vector3Int postion;
}
/// <summary>
/// ��ɫ���
/// </summary>
public struct ActivityPostion
{
    public bool isValue;
    public Vector3Int postion;
}
/// <summary>
/// ��ɫ������
/// </summary>
public struct WorkPostion 
{
    public bool isValue;
    public Vector3Int postion;
}
/// <summary>
/// ��ɫ�Ѳ��
/// </summary>
public struct SearchPostion
{
    public bool isValue;
    public Vector3Int postion;
}