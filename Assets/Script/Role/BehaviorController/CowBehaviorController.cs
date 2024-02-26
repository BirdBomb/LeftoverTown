using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBehaviorController : BaseBehaviorController
{
    [Header("��ط�Χ")]
    public float LocalScope = 4;
    [Header("����Ŀ��")]
    public List<BaseBehaviorController> LookAt = new List<BaseBehaviorController>();

    private Vector2 tempPosMyPos;
    private Vector2 tempPosTargetPos;

    public override void FixedUpdate()
    {
        if (tempPath == null)
        {
            RunAway();
        }
        base.FixedUpdate();
    }
    #region//����
    public override void ListenRoleMove(BaseBehaviorController who,MyTile where)
    {
        /*�ƶ����˲����Լ�*/
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
        base.ListenRoleMove(who,where);
    }
    #endregion
    #region//��Ϊ
    public override void TurnRight()
    {
        bodyController.transform.localScale = new Vector3(-1, 1, 1);
    }
    public override void TurnLeft()
    {
        bodyController.transform.localScale = new Vector3(1, 1, 1);
    }
    public override void RunAway()
    {
        for(int i = 0; i < LookAt.Count; i++)
        {
            if (LookAt[i] != null)
            {
                tempPosMyPos = GetMyPos();
                tempPosTargetPos = LookAt[i].GetMyPos();
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) < LocalScope)
                {
                    TryToFindPathByRPC(tempPosMyPos + (tempPosMyPos - tempPosTargetPos).normalized * 2, tempPosMyPos);
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
[SerializeField]
public struct CowData
{
   
}