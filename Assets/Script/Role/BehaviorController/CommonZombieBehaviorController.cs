using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CommonZombieBehaviorController : BaseBehaviorController
{
    [Header("领地范围")]
    public float LocalScope = 4;
    [Header("警惕目标")]
    public List<BaseBehaviorController> LookAt = new List<BaseBehaviorController>();

    private Vector2 tempPosMyPos;
    private Vector2 tempPosTargetPos;

    public override void Init()
    {
        InvokeRepeating("TryToBite", 2, 2);
        base.Init();
    }
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
    /*网络同步行为*/
    #region
    public void TryToBite()
    {
        if (Object.HasStateAuthority)
        {
            for (int i = 0; i < LookAt.Count; i++)
            {
                if (LookAt[i] != null && Vector2.Distance(transform.position, LookAt[i].transform.position) < 1.5f)
                {
                    RPC_Bite();
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 寻找路径
    /// </summary>
    /// <param name="to"></param>
    /// <param name="from"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_Bite()
    {
        if (Object.HasStateAuthority)
        {
            bodyController.PlayHeadAction(HeadAction.Bite, 1, (string name) => 
            {
                if(name == "HeadBite")
                {
                    var cols = Physics2D.OverlapCircleAll(transform.position, 2);
                    if (cols != null)
                    {
                        foreach (var col in cols)
                        {
                            if(col.TryGetComponent(out BaseBehaviorController actor))
                            {
                                if (actor != this)
                                {
                                    actor.TryToTakeDamage(2);
                                }
                            }
                        }
                    }
                }
            });
        }
        else
        {
            bodyController.PlayHeadAction(HeadAction.Bite, 1, null);
        }
    }
    #endregion

}
