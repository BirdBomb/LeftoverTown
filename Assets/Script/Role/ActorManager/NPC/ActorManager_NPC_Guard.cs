using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Fusion;
using System;
/// <summary>
/// 护卫
/// </summary>
public class ActorManager_NPC_Guard : ActorManager_NPC
{
    #region//监听
    public override void State_Listen_RoleCommit(ActorManager who, CommitState commit, short val)
    {
        if (actionManager.LookAt(who, State_CalculateView()))
        {
            who.actionManager.AllClient_SetFine(commit, val);
            State_TryToSendEmoji(0, Emoji.Yell);
        }
    }
    public override void State_Listen_RoleSendEmoji(ActorManager actor, Emoji emoji, float distance)
    {
        if (brainManager.allClient_actorManager_AttackTarget != null || brainManager.allClient_actorManager_ThreatenedTarget != null)
        {
            return;
        }
        if (actionManager.HearTo(actor, distance))
        {
            if (emoji == Emoji.Yell)
            {
                State_TryToSendEmoji(0.5f, Emoji.Puzzled);
                State_Follow(actor.pathManager.vector3Int_CurPos);
            }
        }
        base.State_Listen_RoleSendEmoji(actor, emoji, distance);
    }
    #endregion
    #region//检查
    /// <summary>
    /// 检查附近角色
    /// </summary>
    /// <returns>终止思考</returns>
    public override bool State_CheckNearbyActor()
    {
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], config.short_View))
            {
                if (brainManager.actorManagers_Nearby[i].actorNetManager.Local_Fine > 0)
                {
                    State_TryToSendEmoji(0, Emoji.Attack);
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Monster_Common)
                {
                    State_TryToSendEmoji(0, Emoji.Attack);
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
            }
        }
        return false;
    }
    #endregion
    #region//行为逻辑
    /// <summary>
    /// 根据时间决定动作(经常触发)
    /// </summary>
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        if (time == GlobalTime.Forenoon)
        {
            if (!State_Think_GoToSleep())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        if (time == GlobalTime.Highnoon)
        {
            if (!State_Think_GoForFood())
            {
                State_Think_GoToStroll_Long(10, 5);
            }
            return;
        }
        if (time == GlobalTime.Dusk || time == GlobalTime.Evening)
        {
            if (!State_Think_GoToWork())
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
        switch (globalTime)
        {
            case GlobalTime.Morning:
                State_PutDownHand();
                break;
            case GlobalTime.Forenoon:
                State_Think_FindBed();
                break;
            case GlobalTime.Highnoon:
                State_Think_FindFood();
                break;
            case GlobalTime.Dusk:
                State_PutOnHand((itemConfig) =>
                {
                    if (itemConfig.Item_Type == ItemType.Weapon) return true;
                    return false;
                });
                break;
            case GlobalTime.Evening:
                State_PutOnHand((itemConfig) =>
                {
                    if (itemConfig.Item_Type == ItemType.Weapon) return true;
                    return false;
                });
                break;
        }
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
    public override void State_OutAttack()
    {
        if (brainManager.globalTime_Now != GlobalTime.Evening)
        {
            State_PutDownHand();
        }
        base.State_OutAttack();
    }
    #endregion
    #region//威胁逻辑
    public override void State_InThreatened(ActorManager actor)
    {
        
    }
    #endregion
    #region//交互
    public override bool Local_CanDialog()
    {
        return false;
    }
    #endregion
}
