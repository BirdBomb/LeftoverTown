using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using static UnityEngine.GraphicsBuffer;

public class ActorManager_Zombie : ActorManager
{
    [SerializeField, Header("��Ұ��Χ")]
    private float VisionDistance;
    [SerializeField, Header("��ǰĿ��")]
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
    private void TryToSetTarget(ActorManager target)
    {
        /*��������Ұ��Χ��*/
        if (Vector2.Distance(target.transform.position, transform.position) < VisionDistance)
        {
            Debug.Log("��ʬ(" + NetController.Id + "),����Ŀ��(" + target.NetController.Id + ")");
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
                Debug.Log("��ʬ(" + NetController.Id + "),Ŀ�궪ʧ");
                targetActor = null;
            }
        }
    }
}
