using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager_Zombie_Spray : ActorManager
{
    [SerializeField, Header("视野范围")]
    private float VisionDistance;
    [SerializeField, Header("基础攻击")]
    private int AttackDamage;
    [SerializeField, Header("近战攻击范围")]
    private float AttackDistance;
    [SerializeField, Header("喷射攻击范围")]
    private float SkillDistance;
    [SerializeField, Header("攻击间隔")]
    private float AttackCD;
    private float AttackTimer;
    [Header("当前目标")]
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
            Debug.Log("僵尸(" + NetManager.Id + "),发现目标(" + target.NetManager.Id + ")");
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
                Debug.Log("僵尸(" + NetManager.Id + "),目标丢失");
                targetActor = null;
            }
        }
    }
    private void TryToAttack(float dt)
    {
        if (targetActor != null && AttackTimer <= 0)
        {
            float distance = Vector2.Distance(targetActor.transform.position, transform.position);
            if (distance < AttackDistance)
            {
                AttackTimer = AttackCD;
                NetManager.RPC_State_Skill(1, targetActor.NetManager.Object.Id);
            }
            else if (distance < SkillDistance)
            {
                AttackTimer = AttackCD * 2;
                NetManager.RPC_State_Skill(2, targetActor.NetManager.Object.Id);
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
    public override void FromRPC_InvokeSkill(int parameter, Fusion.NetworkId id)
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
                        List<ActorManager> temp = new List<ActorManager>();
                        foreach (var col in cols)
                        {
                            if (col.TryGetComponent(out ActorManager actor))
                            {
                                if (actor != this && !temp.Contains(actor))
                                {
                                    actor.TakeDamage(AttackDamage, NetManager);
                                    temp.Add(actor);
                                }
                            }
                        }
                    }
                }
            });
        }
        else if (parameter == 2)
        {
            BodyController.SetHeadTrigger("Spray", 1, (str) =>
            {
                if(str == "HeadSpray")
                {
                    GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_100");
                    Vector3 from = BodyController.Head.position;
                    Vector3 to = NetManager.Runner.FindObject(id).transform.position;

                    obj.transform.position = from;
                    obj.GetComponent<BulletBase>().InitBullet((to - from).normalized, 3f, NetManager);
                }
            });
        }
        base.FromRPC_InvokeSkill(parameter, id);
    }
    #endregion
}
