using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBehaviorController : BaseBehaviorController
{
    [Header("领地范围")]
    public float LocalScope = 4;
    [Header("警惕目标")]
    public List<BaseBehaviorController> LookAt = new List<BaseBehaviorController>();

    private Vector2 tempPosMyPos;
    private Vector2 tempPosTargetPos;
    public override void ListenRoleMove(BaseBehaviorController who,MyTile where)
    {
        if (who == this)
        {
            /*移动的人是自己*/
            int temp = 0;
            /*遍历警惕目标*/
            for (int i = 0; i < LookAt.Count; i++)
            {
                tempPosMyPos = GetMyPos();
                tempPosTargetPos = LookAt[i].GetMyPos();

                /*警惕目标还在范围内*/
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) < LocalScope)
                {
                    temp++;
                    FindPath(tempPosMyPos + (tempPosMyPos - tempPosTargetPos).normalized, tempPosMyPos);
                }
            }
            /*警惕目标丢失*/
            if (temp == 0)
            {
                LookAt.Clear();
            }
            return; 
        }
        /*其他移动*/
        else
        {
            //牛牛警惕性高
            if (Data.Temp_Alert > 100 && !LookAt.Contains(who))
            {
                tempPosMyPos = GetMyPos();
                tempPosTargetPos = where.pos;

                //Debug.Log(tempPosMyPos);
                //Debug.Log(tempPosTargetPos);

                //Debug.Log(tempPosTargetPos);
                /*有生物进入领地范围,朝相反方向移动*/
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) < LocalScope)
                {
                    LookAt.Add(who);
                    FindPath(tempPosMyPos + (tempPosMyPos - tempPosTargetPos).normalized, tempPosMyPos);
                }
            }
        }
        base.ListenRoleMove(who,where);
    }
}
[SerializeField]
public struct CowData
{
   
}