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
