using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;

public class ActorManager_NPC_Hunter : ActorManager_NPC
{
    private float float_ViewDistance = 10;
    #region//监听
    public override void ForState_Listen_MyselfInjured(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && networkObject.TryGetComponent(out ActorManager who) && who.actorAuthority.isPlayer)
        {
            State_InAttack(who);
        }
        base.ForState_Listen_MyselfInjured(parameter, reason, id);
    }
    #endregion
    #region//检查
    public override bool State_CheckNearbyActor()
    {
        var temp = brainManager.State_GetNearbyActors();
        foreach (var actor in temp)
        {
            if (!actionManager.LookAt(actor, State_CalculateView())) continue;
            if (actor.actorNetManager.Local_Fine > 100)
            {
                State_InAttack(actor);
                return true;
            }
            if (actor.statusManager.statusType == StatusType.Monster_Common)
            {
                State_InAttack(actor);
                return true;
            }
            if (actor.statusManager.statusType == StatusType.Animal_Common)
            {
                State_InAttack(actor);
                return true;
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
        switch (time)
        {
            case GlobalTime.Highnoon:
                {
                    if (State_Think_GoForFood()) return;
                    break;
                }
            case GlobalTime.Dusk:
                {
                    if (State_Think_GoToWork()) return;
                    break;
                }
            case GlobalTime.Evening:
                {
                    if (State_Think_GoToSleep()) return;
                    break;
                }
        }
        base.State_ThinkByTimeUpdate(date, hour, time);
    }
    /// <summary>
    /// 根据时间变化决定动作(关键时间触发)
    /// </summary>
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        switch (globalTime)
        {
            case GlobalTime.Morning:
                break;
            case GlobalTime.Forenoon:
                break;
            case GlobalTime.Highnoon:
                StartCoroutine(State_Think_FindFoodPos());
                break;
            case GlobalTime.Afternoon:
                break;
            case GlobalTime.Dusk:
                StartCoroutine(State_Think_FindWorkPos());
                break;
            case GlobalTime.Evening:
                StartCoroutine(State_Think_FindSleepPos());
                break;
        }
    }
    public override bool State_Think_CheckWorkPlace(BuildingTile buildingTile)
    {
        return buildingTile.tileID == 2003;
    }
    /// <summary>
    /// 离开攻击状态
    /// </summary>
    public override void State_OutAttack()
    {
        State_PutDownHand();
        base.State_OutAttack();
    }

    #endregion
    #region//交互
    public override void Local_StartDialog()
    {
        base.Local_StartDialog();
        if (brainManager.globalTime_Now == GlobalTime.Evening) ChooseDialog(0);
        else ChooseDialog(1);
    }
    public override void ForAll_InitDialog()
    {
        dialogMap = new Dictionary<int, Action>();
        dialogMap[0] = Dialog_CanDeal;
        dialogMap[1] = Dialog_CannotDeal;
    }
    public void Dialog_CanDeal()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Hunter_Dialog0_Option0", Local_StartDeal),
            new DialogOption("Hunter_Dialog0_Option1", Local_OverDialog)
        };
        ShowDialog("Hunter_Dialog0", options);
    }
    public void Dialog_CannotDeal()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Hunter_Dialog1_Option0", Local_OverDialog)
        };
        ShowDialog("Hunter_Dialog1", options);
    }

    /// <summary>
    /// 收购
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public override int Local_Offer(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
        int offer = itemConfig.Item_Value * itemData.C / 2 + 1;
        return offer;
    }

    #endregion
}
