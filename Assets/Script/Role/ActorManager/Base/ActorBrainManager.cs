using System.Collections;
using System.Collections.Generic;

public class ActorBrainManager 
{
    private ActorManager actorManager;
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
