using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;

public class ActorManager_NPC_Hunter : ActorManager_NPC
{
    #region//监听
    public override void State_Listen_MyselfHpChange(int parameter, HpChangeReason reason, NetworkId id)
    {
        NetworkObject networkObject = actorNetManager.Runner.FindObject(id);
        if (networkObject != null && parameter < 0)
        {
            ActorManager who = networkObject.GetComponent<ActorManager>();
            if (who.actorAuthority.isPlayer)
            {
                State_InAttack(who);

                State_TryToSendEmoji(0, Emoji.Menace);
            }
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
    /// 检查附近
    /// </summary>
    public override void State_CheckNearby()
    {
        if (brainManager.allClient_actorManager_AttackTarget != null)
        {
            //存在攻击目标
            if (!actionManager.LookAt(brainManager.allClient_actorManager_AttackTarget, config.short_View))
            {
                State_TryToSendEmoji(0, Emoji.Search);
                State_Search(5);
                State_OutAttack();
            }
            else
            {
                brainManager.State_SetSearchPos(brainManager.allClient_actorManager_AttackTarget.pathManager.vector3Int_CurPos);
            }
            return;
        }
        else
        {
            if (brainManager.allClient_actorManager_AttackTargetID != new Fusion.NetworkId())
            {
                State_Search(5);
                State_OutAttack();
            }
            State_CheckNearbyActor();
        }
        if (brainManager.allClient_actorManager_ThreatenedTarget != null)
        {
            //存在威胁目标
            if (actionManager.LookAt(brainManager.allClient_actorManager_ThreatenedTarget, State_CalculateView()))
            {
                State_StartRunAway(brainManager.allClient_actorManager_ThreatenedTarget.transform);
            }
            else
            {
                State_OutThreatened();
            }
        }
        else
        {
            if (brainManager.allClient_actorManager_ThreatenedTargetID != new Fusion.NetworkId())
            {
                State_OutThreatened();
            }
            State_CheckNearbyActor();
        }

    }
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
                if (brainManager.actorManagers_Nearby[i].actorNetManager.Local_Fine > 100)
                {
                    /*发现罪犯*/
                    State_TryToSendEmoji(0, Emoji.Attack);
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Monster_Common)
                {
                    /*发现怪物*/
                    State_TryToSendEmoji(0, Emoji.Attack);
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType == StatusType.Animal_Common)
                {
                    /*发现猎物*/
                    State_TryToSendEmoji(0, Emoji.Happy);
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
            if (!State_Think_GoToWork())
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
                break;
            case GlobalTime.Highnoon:
                State_Think_FindFood();
                break;
            case GlobalTime.Afternoon:
                break;
            case GlobalTime.Dusk:
                State_Think_FindWork();
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
                    if (buildingTile.tileID == 2003)
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
    /// <summary>
    /// 开始谈话
    /// </summary>
    /// <param name="player"></param>
    public override void Local_StartDialog(ActorManager player)
    {
        bool_Dialog = true;
        if (player != null && player.actorNetManager.Object != null)
        {
            actorNetManager.RPC_LocalInput_StandDown(66);
            UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_Dialog"), out TileUI tileUI);
            tileUI_Dialog = tileUI.GetComponent<TileUI_Dialog>();
            actorNetManager.RPC_LocalInput_TurnTo((player.transform.position.x > transform.position.x));
            if (brainManager.globalTime_Now != GlobalTime.Evening)
            {
                Local_SetDialog(player, 0);
            }
            else
            {
                Local_SetDialog(player, 2);
            }
        }
    }
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
                dialogOption_0.optionEntry = "Hunter_Dialog0_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });
                dialogOption_1.optionTable = "Role_String";
                dialogOption_1.optionEntry = "Hunter_Dialog0_Option1";
                dialogOption_1.optionAction = (() =>
                {
                    Local_SetDialog(player, 1);
                });
                dialogOptions.Add(dialogOption_0);
                dialogOptions.Add(dialogOption_1);
                tileUI_Dialog.InitDialog("Role_String", "Hunter_Name", "Role_String", "Hunter_Dialog0");
                tileUI_Dialog.InitOption(dialogOptions);
                break;
            case 1:
                dialogOption_0.optionTable = "Role_String";
                dialogOption_0.optionEntry = "Hunter_Dialog1_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });
                dialogOptions.Add(dialogOption_0);
                tileUI_Dialog.InitDialog("Role_String", "Hunter_Name", "Role_String", "Hunter_Dialog1");
                tileUI_Dialog.InitOption(dialogOptions);
                break;
            case 2:
                dialogOption_0.optionTable = "Role_String";
                dialogOption_0.optionEntry = "Hunter_Dialog2_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });
                dialogOption_1.optionTable = "Role_String";
                dialogOption_1.optionEntry = "Hunter_Dialog2_Option1";
                dialogOption_1.optionAction = (() =>
                {
                    Local_StartDeal(player);
                });
                dialogOptions.Add(dialogOption_0);
                dialogOptions.Add(dialogOption_1);
                tileUI_Dialog.InitDialog("Role_String", "Hunter_Name", "Role_String", "Hunter_Dialog2");
                tileUI_Dialog.InitOption(dialogOptions);
                break;
            case 3:
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
