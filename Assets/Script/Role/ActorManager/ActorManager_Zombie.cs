using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static UnityEngine.GraphicsBuffer;

public class ActorManager_Zombie : ActorManager
{
    [SerializeField, Header("视野范围")]
    private float VisionDistance;
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
}
