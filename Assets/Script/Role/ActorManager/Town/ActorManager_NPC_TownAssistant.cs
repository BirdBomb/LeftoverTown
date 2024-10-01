using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_NPC_TownAssistant : ActorManager_NPC
{
    protected TileObj onlyState_safeTile = null;
    protected TileObj onlyState_workTile = null;
    protected TileObj onlyState_restTile = null;
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
            AllClient_TryToSendEmoji(0.5f, emojiID);
        }
        base.OnlyState_TryUnderstand(who, id, look, hear);
    }
    public override void AllClientListen_RoleCommit(ActorManager who, short val)
    {
        if (Tool_TryLookAt(who))
        {
            who.AllClient_SetFine(val);
        }
        AllClient_TryToSendEmoji(0.2f, 16);
        base.AllClientListen_RoleCommit(who, val);
    }
    public override void OnlyStateListen_MoveOther(ActorManager actor, MyTile where)
    {
        if (!onlyState_RememberTarget.Contains(actor))
        {
            if (Tool_TryLookAt(actor))
            {
                onlyState_RememberTarget.Add(actor);
                Tool_CheckOutSomeone(actor, out short handItemID, out short headItemID, out short bodyItemID, out short fine);
                Tool_CheckStatus(bodyItemID, headItemID, fine, out short id);
                OnlyState_MeetSomeone(id, fine);
            }
        }
        base.OnlyStateListen_MoveOther(actor, where);
    }
    public override void AllClientListen_WorldGlobalTimeChange(int hour, int date, GlobalTime globalTime)
    {
        if (isState)
        {
            if (lastGlobalTime != globalTime)
            {
                lastGlobalTime = globalTime;
                if (globalTime == GlobalTime.Morning || globalTime == GlobalTime.Forenoon || globalTime == GlobalTime.HighNoon || globalTime == GlobalTime.Afternoon)
                {
                    OnlyState_GoToWork();
                }
                else if (globalTime == GlobalTime.Evening)
                {
                    OnlyState_GoToRest();
                }
            }
        }
        base.AllClientListen_WorldGlobalTimeChange(hour, date, globalTime);
    }
    public override void AllClientListen_MyselfHpChange(int parameter, NetworkId networkId)
    {
        if(isState)
        {
            ActorManager who = NetManager.Runner.FindObject(networkId).GetComponent<ActorManager>();
            if (who.isPlayer && Tool_TryLookAt(who))
            {
                who.AllClient_SetFine(500);
                AllClient_TryToSendEmoji(0.1f, 16);
            }
            OnlyState_GoToSafe();
        }
        base.AllClientListen_MyselfHpChange(parameter, networkId);
    }

    /// <summary>
    /// 绑定工作tile
    /// </summary>
    /// <param name="tile"></param>
    public virtual void BindWorkTile(TileObj tile)
    {
        onlyState_workTile = tile;
    }
    /// <summary>
    /// 绑定休息tile
    /// </summary>
    /// <param name="tile"></param>
    public virtual void BindRestTile(TileObj tile)
    {
        onlyState_restTile = tile;
    }
    /// <summary>
    /// 绑定安全tile
    /// </summary>
    /// <param name="tile"></param>
    public virtual void BindSafeTile(TileObj tile)
    {
        onlyState_safeTile = tile;
    }
    /// <summary>
    /// 去工作
    /// </summary>
    public void OnlyState_GoToWork()
    {
        if (onlyState_workTile != null)
        {
            OnlyState_FindWayToTarget(onlyState_workTile.bindTile._posInWorld + Vector2.up);
        }
    }
    /// <summary>
    /// 去休息
    /// </summary>
    public void OnlyState_GoToRest()
    {
        onlyState_RememberTarget.Clear();
        onlyState_LookTarget.Clear();
        allClient_AttackTarget = null;
        if (onlyState_restTile != null)
        {
            OnlyState_FindWayToTarget(onlyState_restTile.bindTile._posInWorld);
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
            if (onlyState_restTile)
            {
                OnlyState_FindWayToTarget(onlyState_restTile.bindTile._posInWorld);
            }
        }
    }
    /// <summary>
    /// 避险
    /// </summary>
    public void OnlyState_GoToSafe()
    {
        if (!onlyState_safeTile)
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
        if (onlyState_safeTile)
        {
            if (Vector2.Distance(onlyState_safeTile.transform.position, transform.position) > 5)
            {
                OnlyState_FindWayToTarget(onlyState_safeTile.bindTile._posInWorld);
                return;
            }
        }
        NetManager.UpdateSeed();
        UnityEngine.Random.InitState(NetManager.Data_RandomInRange);
        Vector2 pos = UnityEngine.Random.insideUnitCircle * 4;
        OnlyState_FindWayToTarget(Tool_GetMyTileWithOffset(Vector3Int.zero)._posInWorld + pos);
    }
    /// <summary>
    /// 遇见某人
    /// </summary>
    public void OnlyState_MeetSomeone(int id, int fine)
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
        AllClient_TryToSendEmoji(0.2f, emoji);
    }


    #endregion
}
