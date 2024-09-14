using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_NPC_TownAssistant : ActorManager_NPC
{
    protected TileObj safeTile = null;
    protected TileObj workTile = null;
    protected TileObj restTile = null;
    GlobalTime lastGlobalTime;

    #region//小镇店主专有方法
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
            who.SetFine(val);
        }
        TryToSendEmoji(0.2f, 16);
        base.ListenRoleCommit(who, val);
    }
    public override void Listen_OtherMove(ActorManager actor, MyTile where)
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
        base.Listen_OtherMove(actor, where);
    }
    public override void Listen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
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
        base.Listen_WorldGlobalTimeChange(hour, date, globalTime);
    }

    public override void Listen_HpChange(int parameter, NetworkId networkId)
    {
        ActorManager who = NetManager.Runner.FindObject(networkId).GetComponent<ActorManager>();
        if (who.isPlayer && OnlyState_TryLookAt(who))
        {
            who.SetFine(500);
            TryToSendEmoji(0.1f, 16);
        }
        GoToSafe();
        base.Listen_HpChange(parameter, networkId);
    }

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
    }
    /// <summary>
    /// 绑定安全tile
    /// </summary>
    /// <param name="tile"></param>
    public virtual void BindSafeTile(TileObj tile)
    {
        safeTile = tile;
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
            if (restTile)
            {
                FindWayToTarget(restTile.bindTile._posInWorld);
            }
        }
    }
    /// <summary>
    /// 避险
    /// </summary>
    public void GoToSafe()
    {
        if (!safeTile)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SomeoneFindTargetTile
            {
                actor = this,
                id = 1023,
                pos = transform.position,
                distance = 20,
                action = BindSafeTile
            });
        }
        if (safeTile)
        {
            if (Vector2.Distance(safeTile.transform.position, transform.position) > 5)
            {
                FindWayToTarget(safeTile.bindTile._posInWorld);
                return;
            }
        }
        NetManager.UpdateSeed();
        UnityEngine.Random.InitState(NetManager.RandomInRange);
        Vector2 pos = UnityEngine.Random.insideUnitCircle * 4;
        FindWayToTarget(GetMyTile()._posInWorld + pos);
    }
    /// <summary>
    /// 遇见某人
    /// </summary>
    public void MeetSomeone(int id, int fine)
    {
        int emoji;
        if (id == 1001 || id == 1002)
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
