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
    /// ����Ŀ��
    /// </summary>
    public ActorManager allClient_actorManager_AttackTarget;
    /// <summary>
    /// ��вĿ��
    /// </summary>
    public List<ActorManager> actorManagers_ThreatenedTarget = new List<ActorManager>();
    /// <summary>
    /// ������вĿ��
    /// </summary>
    public ActorManager actorManager_ThreatenedTarget;
    /// <summary>
    /// ��ΧĿ��
    /// </summary>
    public List<ActorManager> actorManagers_Nearby = new List<ActorManager>();
    public bool bool_AttackState = false;

}
