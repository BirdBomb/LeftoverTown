using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;

public class ActorManager_NPC_Assistant : ActorManager_NPC
{
    #region//监听
    public override void OnlyState_TryUnderstand(ActorManager who, int id, bool look, bool hear)
    {
        if (id == 16)
        {
            int emojiID;
            if (!look)
            {
                emojiID = 1;
            }
            else
            {
                emojiID = 0;
            }
            TryToSendEmoji(0.5f, emojiID);
        }
        base.OnlyState_TryUnderstand(who, id, look, hear);
    }
    public override void ListenRoleCommit(ActorManager who, short val)
    {
        if (OnlyState_TryLookAt(who))
        {
            who.AddFine(val);
        }
        TryToSendEmoji(0.2f, 16);
        base.ListenRoleCommit(who, val);
    }
    public override void ListenRoleMove_Other(ActorManager actor, MyTile where)
    {
        if (isState)
        {
            if (!rememberTarget.Contains(actor))
            {
                if (OnlyState_TryLookAt(actor))
                {
                    rememberTarget.Add(actor);
                    CheckOutSomeone(actor, out short handItemID, out short headItemID, out short bodyItemID, out short fine);
                    CheckStatus(bodyItemID, headItemID, fine, out short id);
                    MeetSomeone(id, fine);
                }
            }
        }
        base.ListenRoleMove_Other(actor, where);
    }
    GlobalTime lastGlobalTime;
    public override void ListenWorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {
        if (lastGlobalTime != globalTime)
        {
            lastGlobalTime = globalTime;
            if (globalTime == GlobalTime.Morning || globalTime == GlobalTime.Forenoon || globalTime == GlobalTime.HighNoon || globalTime == GlobalTime.Afternoon)
            {
                GoToWork();
            }
            else if (globalTime == GlobalTime.Evening)
            {
                GoToRest();
            }
        }
        base.ListenWorldGlobalTimeChange(hour, date, globalTime);
    }
    public override void Listen_HpChange(int parameter, NetworkId networkId)
    {
        ActorManager actor = NetManager.Runner.FindObject(networkId).GetComponent<ActorManager>();
        if (attackTarget != actor)
        {
            FromRPC_ChangeAttackTarget(actor.NetManager.Object.Id);
            OnlyState_Follow(FollowType.RunTo);
            TryToSendEmoji(0.1f, 16);
        }
        base.Listen_HpChange(parameter, networkId);
    }

    #endregion
    #region//店员方法
    protected TileObj workTile = null;
    protected TileObj restTile = null;
    /// <summary>
    /// 绑定工作tile
    /// </summary>
    /// <param name="tile"></param>
    public virtual void BindWorkTile(TileObj tile)
    {
        workTile = tile;
    }
    /// <summary>
    /// 绑定休息tile
    /// </summary>
    /// <param name="tile"></param>
    public virtual void BindRestTile(TileObj tile)
    {
        restTile = tile;
        FindWayToTarget(restTile.bindTile._posInWorld);
    }
    /// <summary>
    /// 去工作
    /// </summary>
    public void GoToWork()
    {
        if (workTile != null)
        {
            FindWayToTarget(workTile.bindTile._posInWorld + Vector2.up);
        }
    }
    /// <summary>
    /// 去休息
    /// </summary>
    public void GoToRest()
    {
        rememberTarget.Clear();
        lookTarget.Clear();
        attackTarget = null;
        if (restTile != null)
        {
            FindWayToTarget(restTile.bindTile._posInWorld);
        }
        else
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneFindTargetTile
            {
                actor = this,
                id = 1005,
                pos = transform.position,
                distance = 10,
                action = BindRestTile
            });
        }
    }
    /// <summary>
    /// 遇见某人
    /// </summary>
    public void MeetSomeone(int id, int fine)
    {
        int emoji;
        if(id == 1001 || id == 1002)
        {
            emoji = 0;
        }
        else
        {
            emoji = 4;
        }
        TryToSendEmoji(0.2f, emoji);
    }
    #endregion
}
