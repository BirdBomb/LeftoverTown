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
    public List<BaseBehaviorController> LookAtList = new List<BaseBehaviorController>();

    private Vector2 tempPosMyPos;
    private Vector2 tempPosTargetPos;

    private Vector2 chargeDir;
    private float chargeSpeed;
    private float chargeTime;
    public override void Init()
    {
        InvokeRepeating("TryToBite", 2, 2);

        base.Init();
    }
    public override void FixedUpdate()
    {
        if (Data.Data_Dead) return;
        //if (chargeTime > 0)
        //{
        //    if(chargeTime == 1)
        //    {
        //        PlayTurn(chargeDir);
        //        bodyController.PlayBodyAction(BodyAction.Charge, 1, null);
        //        bodyController.PlayHeadAction(HeadAction.Charge, 1, null);
        //        bodyController.PlayLegAction(LegAction.Charge, 1,null);
        //        bodyController.PlayHandAction(HandAction.Charge, 1, null);
        //        tempPath = null;
        //    }
        //    chargeTime -= Time.fixedDeltaTime;
        //    if (Object.HasStateAuthority)
        //    {
        //        StateMove(chargeDir, chargeSpeed, Time.fixedDeltaTime);
        //    }
        //    return;
        //}
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
            tempPosMyPos = GetMyPos();
            tempPosTargetPos = where.pos;

            if (LookAtList.Contains(who))
            {
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) > LocalScope)
                {
                    LookAtList.Remove(who);
                }
            }
            else
            {
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) <= LocalScope)
                {
                    LookAt(who);
                }
            }
        }
        /*移动的人是自己*/
        else
        {

        }
        base.ListenRoleMove(who, where);
    }
    #endregion
    #region//行为
    public override void TurnRight()
    {
        bodyController.Head.localScale = new Vector3(1, 1, 1);
        bodyController.Body.localScale = new Vector3(1, 1, 1);
        bodyController.Hand.localScale = new Vector3(1, 1, 1);
        bodyController.Leg.localScale = new Vector3(1, 1, 1);
    }
    public override void TurnLeft()
    {
        bodyController.Head.localScale = new Vector3(-1, 1, 1);
        bodyController.Body.localScale = new Vector3(-1, 1, 1);
        bodyController.Hand.localScale = new Vector3(-1, 1, 1);
        bodyController.Leg.localScale = new Vector3(-1, 1, 1);
    }
    public override void LookAt(BaseBehaviorController who)
    {
        LookAtList.Add(who);
        base.LookAt(who);
    }
    public override void RunTo()
    {
        for (int i = 0; i < LookAtList.Count; i++)
        {
            if (LookAtList[i] != null)
            {
                tempPosMyPos = GetMyPos();
                tempPosTargetPos = LookAtList[i].GetMyPos();
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) < LocalScope)
                {
                    TryToFindPathByRPC(tempPosTargetPos, tempPosMyPos);
                }
                else
                {
                    LookAtList.RemoveAt(i);
                    i--;
                }
            }
        }
        base.RunTo();
    }
    #endregion
    /*网络同步行为*/
    #region
    public void TryToBite()
    {
        if (Object.HasStateAuthority && !Data.Data_Dead)
        {
            for (int i = 0; i < LookAtList.Count; i++)
            {
                if (LookAtList[i] != null && Vector2.Distance(transform.position, LookAtList[i].transform.position) < 1.5f)
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

    public void TryToCharge()
    {
        if (Object.HasStateAuthority)
        {
            if (LookAtList.Count > 0)
            {
                Vector2 dir = LookAtList[0].transform.position - transform.position;
                RPC_Charge(dir, 5);
            }
        }
    }
    /// <summary>
    /// 飞扑
    /// </summary>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    public void RPC_Charge(Vector2 dir,float speed)
    {
        chargeDir = dir;
        chargeSpeed = speed;
        chargeTime = 1;
    }
    #endregion

}
