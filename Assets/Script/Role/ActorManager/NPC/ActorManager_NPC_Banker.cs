using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;
/// <summary>
/// 银行家
/// </summary>
public class ActorManager_NPC_Banker : ActorManager_NPC
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
        if (time == GlobalTime.Forenoon)
        {
            if (!State_Think_GoToWork())
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
        if (time == GlobalTime.Afternoon)
        {
            if (!State_Think_GoToWork())
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
        switch (globalTime)
        {
            case GlobalTime.Morning:
                break;
            case GlobalTime.Forenoon:
                State_Think_FindWork();
                break;
            case GlobalTime.Highnoon:
                State_Think_FindFood();
                break;
            case GlobalTime.Afternoon:
                State_Think_FindWork();
                break;
            case GlobalTime.Dusk:
                State_Think_FindFood();
                break;
            case GlobalTime.Evening:
                State_Think_FindBed();
                break;
        }
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
    /// <summary>
    /// 开始谈话
    /// </summary>
    /// <param name="who"></param>
    public override void State_Think_TalkStart(ActorManager who)
    {
        State_RsetThinkTime(5);
        State_TryToSendEmoji(0.2f, Emoji.Talking);
    }
    /// <summary>
    /// 结束谈话
    /// </summary>
    public override void State_Think_TalkEnd(ActorManager who)
    {

    }
    #endregion
    #region//交互
    /// <summary>
    /// 更新谈话
    /// </summary>
    public override void Local_SetDialog(ActorManager player, int index)
    {
        List<DialogOption> dialogOptions = new List<DialogOption>();
        DialogOption dialogOption_0 = new DialogOption();
        DialogOption dialogOption_1 = new DialogOption();
        switch (index)
        {
            case 0:
                dialogOption_0.optionTable = "Role_String";
                dialogOption_0.optionEntry = "Banker_Dialog0_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });
                dialogOption_1.optionTable = "Role_String";
                dialogOption_1.optionEntry = "Banker_Dialog0_Option1";
                dialogOption_1.optionAction = (() =>
                {
                    Local_StartDeal(player);
                });
                dialogOptions.Add(dialogOption_0);
                dialogOptions.Add(dialogOption_1);
                tileUI_Dialog.InitDialog("Role_String", "Banker_Name", "Role_String", "Banker_Dialog0");
                tileUI_Dialog.InitOption(dialogOptions);
                break;
        }
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
