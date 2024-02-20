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
    public override void ListenRoleMove(BaseBehaviorController who,MyTile where)
    {
        if (who == this)
        {
            /*�ƶ��������Լ�*/
            int temp = 0;
            /*��������Ŀ��*/
            for (int i = 0; i < LookAt.Count; i++)
            {
                tempPosMyPos = GetMyPos();
                tempPosTargetPos = LookAt[i].GetMyPos();

                /*����Ŀ�껹�ڷ�Χ��*/
                if (Vector2.Distance(tempPosMyPos, tempPosTargetPos) < LocalScope)
                {
                    temp++;
                    FindPath(tempPosMyPos + (tempPosMyPos - tempPosTargetPos).normalized, tempPosMyPos);
                }
            }
            /*����Ŀ�궪ʧ*/
            if (temp == 0)
            {
                LookAt.Clear();
            }
            return; 
        }
        /*�����ƶ�*/
        else
        {
            //ţţ�����Ը�
            if (Data.Temp_Alert > 100 && !LookAt.Contains(who))
            {
                tempPosMyPos = GetMyPos();
                tempPosTargetPos = where.pos;

                //Debug.Log(tempPosMyPos);
                //Debug.Log(tempPosTargetPos);

                //Debug.Log(tempPosTargetPos);
                /*�����������ط�Χ,���෴�����ƶ�*/
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