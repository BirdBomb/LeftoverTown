using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Fusion.Sockets.NetBitBuffer;
/// <summary>
/// 村民
/// </summary>
public class ActorManager_NPC_Villager : ActorManager_NPC
{
    #region//监听
    public override void State_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {
        if (brainManager.allClient_actorManager_AttackTarget != null || brainManager.allClient_actorManager_ThreatenedTarget != null)
        {
            return;
        }
        if (actionManager.HearTo(actor, distance))
        {
            if (emoji == Emoji.Greeting)
            {
                //有人向我问候
                if (brainManager.globalTime_Now != GlobalTime.Evening)
                {
                    State_Think_TalkStart(actor);
                }
            }
            else if (emoji == Emoji.Talking)
            {
                //谈话中
                if (brainManager.globalTime_Now != GlobalTime.Evening)
                {
                    State_Think_Talking(actor);
                }
            }
            else if (emoji == Emoji.TalkEnd)
            {
                //谈话中
                if (brainManager.globalTime_Now != GlobalTime.Evening)
                {
                    State_Think_TalkEnd(actor);
                }
            }
            else if (emoji == Emoji.Yell)
            {
                //谈话中
                State_TryToSendEmoji(0.5f, Emoji.Puzzled);
                State_Follow(actor.pathManager.vector3Int_CurPos);
            }
        }
    }
    #endregion
    #region//行为逻辑
    /// <summary>
    /// 根据时间决定动作(经常触发)
    /// </summary>
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        if (time == GlobalTime.Highnoon)
        {
            if (!State_Think_GoForFood())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        if (time == GlobalTime.Dusk)
        {
            if (!State_Think_GoForFood())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }

        if (time == GlobalTime.Evening)
        {
            if (!State_Think_GoToSleep())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        State_Think_GoToStroll_Long(10, 5);
    }
    /// <summary>
    /// 根据时间变化决定动作(关键时间触发)
    /// </summary>
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        //if (globalTime == GlobalTime.Forenoon) { State_Think_FindWork(); }
        //if (globalTime == GlobalTime.Afternoon) { State_Think_FindWork(); }
        if (globalTime == GlobalTime.Highnoon) { State_Think_FindFood(); }
        if (globalTime == GlobalTime.Dusk) { State_Think_FindFood(); }
        if (globalTime == GlobalTime.Evening) { State_Think_FindBed(); }
    }

    public override bool State_Think_FindWork()
    {
        brainManager.State_ResetWorkPos();
        for (int i = -5; i < 5; i++)
        {
            for (int j = -5; j < 5; j++)
            {
                if (MapManager.Instance.GetBuilding(brainManager.state_homePostion.position + new Vector3Int(i, j, 0), out BuildingTile buildingTile))
                {
                    if (buildingTile.tileID == 2013)
                    {
                        brainManager.State_SetWorkPos(brainManager.state_homePostion.position + new Vector3Int(i, j, 0));
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public override void State_Think_BetweenStroll()
    {
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], 5))
            {
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Human_Common)
                {
                    State_TryToSendEmoji(0.5f, Emoji.Greeting, 5);
                }
                else if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Human_Bigwigs)
                {
                    State_TryToSendEmoji(0.5f, Emoji.Greeting, 5);
                }
            }
        }
        base.State_Think_BetweenStroll();
    }
    #endregion
    #region//交互
    public override bool Local_CanDialog()
    {
        return false;
    }
    #endregion

}
