using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonZombieBehaviorController : BaseBehaviorController
{
    [Header("领地范围")]
    public float LocalScope = 4;
    [Header("警惕目标")]
    public List<BaseBehaviorController> LookAt = new List<BaseBehaviorController>();

    private Vector2 tempPosMyPos;
    private Vector2 tempPosTargetPos;

    public override void FixedUpdate()
    {
        if (tempPath == null)
        {
            RunTo();
        }
        base.FixedUpdate();
    }
    #region//监听
    public override void ListenRoleMove(BaseBehaviorController who, MyTile where)
    {
        /*移动的人不是自己*/
        if (who != this)
        {
            if (Data.Temp_Alert > 100 && !LookAt.Contains(who))
            {
                tempPosMyPos = GetMyPos();
                tempPosTargetPos = where.pos;
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) < LocalScope)
                {
                    LookAt.Add(who);
                }
            }
        }
        base.ListenRoleMove(who, where);
    }
    #endregion
    #region//行为
    public override void TurnRight()
    {
        bodyController.Head.localScale = new Vector3(-1, 1, 1);
        bodyController.Body.localScale = new Vector3(-1, 1, 1);
        bodyController.Hand.localScale = new Vector3(-1, 1, 1);
        bodyController.Leg.localScale = new Vector3(-1, 1, 1);
    }
    public override void TurnLeft()
    {
        bodyController.Head.localScale = new Vector3(1, 1, 1);
        bodyController.Body.localScale = new Vector3(1, 1, 1);
        bodyController.Hand.localScale = new Vector3(1, 1, 1);
        bodyController.Leg.localScale = new Vector3(1, 1, 1);
    }
    public override void RunTo()
    {
        for (int i = 0; i < LookAt.Count; i++)
        {
            if (LookAt[i] != null)
            {
                tempPosMyPos = GetMyPos();
                tempPosTargetPos = LookAt[i].GetMyPos();
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) < LocalScope)
                {
                    TryToFindPathByRPC(tempPosTargetPos, tempPosMyPos);
                }
                else
                {
                    LookAt.RemoveAt(i);
                    i--;
                }
            }
        }
        base.RunAway();
    }
    #endregion
}
