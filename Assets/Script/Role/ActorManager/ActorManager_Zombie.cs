using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static UnityEngine.GraphicsBuffer;

public class ActorManager_Zombie : ActorManager
{
    [SerializeField, Header("��Ұ��Χ")]
    private float VisionDistance;
    [SerializeField, Header("��������")]
    private int AttackDamage;
    [SerializeField, Header("������Χ")]
    private float AttackDistance;
    [SerializeField, Header("�������")]
    private float AttackCD;
    private float AttackTimer;
    [Header("��ǰĿ��")]
    private ActorManager targetActor;
    public override void ListenRoleMove(ActorManager who, MyTile where)
    {
        if (isState)
        {
            /*ֻ�е�ǰĿ��Ϊ�ղŻ�׷����Ŀ��*/
            if (targetActor == null && who != this)
            {
                TryToSetTarget(who);
            }
            /*Ŀ���ƶ�*/
            if (targetActor == who)
            {
                TryToFollowTarget();
            }
        }
        base.ListenRoleMove(who, where);
    }
    public override void CustomUpdate()
    {
        if (isState)
        {
            TryToAttack(customUpdateTime);
        }
        base.CustomUpdate();
    }
    /*����������*/
    #region
    private void TryToSetTarget(ActorManager target)
    {
        /*��������Ұ��Χ��*/
        if (Vector2.Distance(target.transform.position, transform.position) < VisionDistance)
        {
            Debug.Log("��ʬ(" + NetManager.Id + "),����Ŀ��(" + target.NetManager.Id + ")");
            targetActor = target;
            TryToFollowTarget();
        }
    }
    private void TryToFollowTarget()
    {
        if (targetActor)
        {
            if (Vector2.Distance(targetActor.transform.position, transform.position) < 1.5f * VisionDistance)
            {
                FindWayToTarget(targetActor.GetMyTile()._posInWorld);
            }
            else
            {
                Debug.Log("��ʬ(" + NetManager.Id + "),Ŀ�궪ʧ");
                targetActor = null;
            }
        }
    }
    private void TryToAttack(float dt)
    {
        if (targetActor != null && AttackTimer <= 0)
        {
            if (Vector2.Distance(targetActor.transform.position, transform.position) < AttackDistance)
            {
                AttackTimer = AttackCD;
                NetManager.RPC_State_NpcUseSkill(1, targetActor.NetManager.Object.Id);
            }
        }
        else
        {
            AttackTimer -= dt;
        }
    }
    #endregion
    /*�ͻ��˷���*/
    #region
    public override void FromRPC_NpcUseSkill(int parameter, Fusion.NetworkId id)
    {
        if (parameter == 1)
        {
            BodyController.SetHeadTrigger("Bite", 1, (str) =>
            {
                if (isState)
                {
                    var cols = Physics2D.OverlapCircleAll(transform.position, 2);
                    if (cols != null)
                    {
                        foreach (var col in cols)
                        {
                            if (col.TryGetComponent(out ActorManager actor))
                            {
                                if (actor != this)
                                {
                                    actor.TakeDamage(AttackDamage, NetManager);
                                }
                            }
                        }
                    }
                }
            });
        }
        base.FromRPC_NpcUseSkill(parameter, id);
    }

    #endregion
}
