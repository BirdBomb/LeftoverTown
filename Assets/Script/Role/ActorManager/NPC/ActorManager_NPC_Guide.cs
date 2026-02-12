using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;
/// <summary>
/// 向导
/// </summary>
public class ActorManager_NPC_Guide : ActorManager_NPC
{
    #region//检查
    public override bool State_CheckNearbyActor()
    {
        if (brainManager.globalTime_Now != GlobalTime.Evening) { return true; }
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (actionManager.LookAt(brainManager.actorManagers_Nearby[i], State_CalculateView()))
            {
                if (brainManager.actorManagers_Nearby[i].statusManager.statusType != StatusType.Animal_Common &&
                    brainManager.actorManagers_Nearby[i].statusManager.statusType != StatusType.Monster_Common)
                {
                    State_InAttack(brainManager.actorManagers_Nearby[i]);
                    return true;
                }
            }
        }
        //base.State_CheckNearbyActor
        return false;
    }
    #endregion

    #region//交互
    private TileUI_Dictionary tileUI_Dictionary;
    private bool bool_Dictionay;
    public override void Local_PlayerFaraway(ActorManager actor)
    {
        Local_OverDictionary(actor);
        base.Local_PlayerFaraway(actor);
    }
    /// <summary>
    /// 更新谈话
    /// </summary>
    public override void Local_SetDialog(ActorManager player, int index)
    {
        List<DialogOption> dialogOptions = new List<DialogOption>();
        DialogOption dialogOption_0 = new DialogOption();
        DialogOption dialogOption_1 = new DialogOption();
        DialogOption dialogOption_2 = new DialogOption();
        switch (index)
        {
            case 0:
                tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog0");
                dialogOption_0.optionTable = "Role_String";
                dialogOption_0.optionEntry = "Guide_Dialog0_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_SetDialog(player, 1);
                });
                dialogOption_1.optionTable = "Role_String";
                dialogOption_1.optionEntry = "Guide_Dialog0_Option1";
                dialogOption_1.optionAction = (() =>
                {
                    Local_StartDeal(player);
                });
                dialogOption_2.optionTable = "Role_String";
                dialogOption_2.optionEntry = "Guide_Dialog0_Option2";
                dialogOption_2.optionAction = (() =>
                {
                    Local_StartDictionary(player);
                });

                dialogOptions.Add(dialogOption_0);
                dialogOptions.Add(dialogOption_1);
                dialogOptions.Add(dialogOption_2);
                tileUI_Dialog.InitOption(dialogOptions);
                break;
            case 1:
                tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog1");
                dialogOption_0.optionTable = "Role_String";
                dialogOption_0.optionEntry = "Guide_Dialog1_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });
                dialogOption_1.optionTable = "Role_String";
                dialogOption_1.optionEntry = "Guide_Dialog1_Option1";
                dialogOption_1.optionAction = (() =>
                {
                    Local_SetDialog(player, 2);
                });
                dialogOption_2.optionTable = "Role_String";
                dialogOption_2.optionEntry = "Guide_Dialog1_Option2";
                dialogOption_2.optionAction = (() =>
                {
                    Local_SetDialog(player, 3);
                });
                dialogOptions.Add(dialogOption_0);
                dialogOptions.Add(dialogOption_1);
                dialogOptions.Add(dialogOption_2);
                tileUI_Dialog.InitOption(dialogOptions);
                break;
            case 2:
                tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog2");
                dialogOption_0.optionTable = "Role_String";
                dialogOption_0.optionEntry = "Guide_Dialog2_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });

                dialogOptions.Add(dialogOption_0);
                tileUI_Dialog.InitOption(dialogOptions);
                break;
            case 3:
                tileUI_Dialog.InitDialog("Role_String", "Guide_Name", "Role_String", "Guide_Dialog3");
                dialogOption_0.optionTable = "Role_String";
                dialogOption_0.optionEntry = "Guide_Dialog3_Option0";
                dialogOption_0.optionAction = (() =>
                {
                    Local_OverDialog(player);
                });

                dialogOptions.Add(dialogOption_0);
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
        int offer = 0;
        return offer;
    }
    /// <summary>
    /// 开始字典
    /// </summary>
    /// <param name="actor"></param>
    private void Local_StartDictionary(ActorManager actor)
    {
        bool_Dictionay = true;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_Dictionary"), out TileUI tileUI);
            tileUI_Dictionary = tileUI.GetComponent<TileUI_Dictionary>();
            tileUI_Dictionary.Init();
        }
    }
    /// <summary>
    /// 结束字典
    /// </summary>
    /// <param name="actor"></param>
    private void Local_OverDictionary(ActorManager actor)
    {
        bool_Dictionay = false;
        if (actor != null && actor.actorNetManager.Object != null)
        {
            UIManager.Instance.HideTileUI(tileUI_Dictionary);
        }
    }

    #endregion
}
