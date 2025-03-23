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
    }
    /// <summary>
    /// 攻击目标
    /// </summary>
    public ActorManager allClient_actorManager_AttackTarget;
    /// <summary>
    /// 威胁目标
    /// </summary>
    public List<ActorManager> actorManagers_ThreatenedTarget = new List<ActorManager>();
    /// <summary>
    /// 优先威胁目标
    /// </summary>
    public ActorManager actorManager_ThreatenedTarget;
    /// <summary>
    /// 周围目标
    /// </summary>
    public List<ActorManager> actorManagers_Nearby = new List<ActorManager>();
    public bool bool_AttackState = false;

}
