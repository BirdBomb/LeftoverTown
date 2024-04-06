using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static UnityEngine.GraphicsBuffer;

public class ActorManager_Zombie : ActorManager
{
    [SerializeField, Header("视野范围")]
    private float VisionDistance;
    [SerializeField, Header("攻击范围")]
    private float AttackDistance;
    [SerializeField, Header("攻击间隔")]
    private float AttackCD;
    private float AttackTimer;
    [SerializeField, Header("当前目标")]
    private ActorManager targetActor;
    public override void ListenRoleMove(ActorManager who, MyTile where)
    {
        if (isState)
        {
            /*只有当前目标为空才会追逐新目标*/
            if (targetActor == null && who != this)
            {
                TryToSetTarget(who);
            }
            /*目标移动*/
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
    /*服务器方法*/
    #region
    private void TryToSetTarget(ActorManager target)
    {
        /*敌人在视野范围内*/
        if (Vector2.Distance(target.transform.position, transform.position) < VisionDistance)
        {
            Debug.Log("僵尸(" + NetController.Id + "),发现目标(" + target.NetController.Id + ")");
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
                SetTargetTile(targetActor.GetMyTile().pos);
            }
            else
            {
                Debug.Log("僵尸(" + NetController.Id + "),目标丢失");
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
                NetController.RPC_Skill(5, targetActor.NetController.Object.Id);
            }
        }
        else
        {
            AttackTimer -= dt;
        }
    }
    #endregion
    /*客户端方法*/
    #region
    public override void RPC_Skill(int parameter, Fusion.NetworkId id)
    {
        BodyController.PlayHeadAction(HeadAction.Bite, 1, (str) => 
        {
            if(str == "HeadBite")
            {
                //var cols = Physics2D.OverlapCircleAll(transform.position, 2);
                //if (cols != null)
                //{
                //    foreach (var col in cols)
                //    {
                //        if (col.TryGetComponent(out BaseBehaviorController actor))
                //        {
                //            if (actor != this)
                //            {
                //                actor.TryToTakeDamage(2);
                //            }
                //        }
                //    }
                //}

            }
        });
        base.RPC_Skill(parameter, id);
    }

    #endregion
}
