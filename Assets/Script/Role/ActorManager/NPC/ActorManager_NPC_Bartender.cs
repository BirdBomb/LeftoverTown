using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static GameEvent;
/// <summary>
/// 酒保
/// </summary>
public class ActorManager_NPC_Bartender : ActorManager_NPC
{
    #region//行为逻辑
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        switch (time)
        {
            case GlobalTime.Morning:
                {
                    if (State_Think_GoForFood()) return;
                    break;
                }
            case GlobalTime.Evening:
                {
                    if (State_Think_GoToSleep()) return;
                    break;
                }
        }
        if (State_Think_GoToWork()) return;
        base.State_ThinkByTimeUpdate(date, hour, time); 
    }
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        switch (globalTime)
        {
            case GlobalTime.Morning:
                StartCoroutine(State_Think_FindFoodPos());
                return;
            case GlobalTime.Evening:
                StartCoroutine(State_Think_FindSleepPos());
                return;
        }
        StartCoroutine(State_Think_FindWorkPos());
    }
    public override bool State_Think_CheckWorkPlace(BuildingTile buildingTile)
    {
        return buildingTile.tileID == 2102;
    }
    #endregion
    #region//交互
    public override void ForAll_InitDialog()
    {
        dialogMap = new Dictionary<int, Action>();
        dialogMap[0] = Dialog_Start;
    }
    public override void Local_StartDialog()
    {
        base.Local_StartDialog();
        ChooseDialog(0);
    }
    public void Dialog_Start()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Bartender_Dialog0_Option0", Local_StartDeal),
            new DialogOption("Bartender_Dialog0_Option1", Local_OverDialog)
        };
        ShowDialog("Bartender_Dialog0", options);
    }
    #endregion
    #region//交易
    public override int Local_Offer(ItemData itemData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemData.I);
        int offer = itemConfig.Item_Value * itemData.C / 2 + 1;
        return offer;
    }
    #endregion
}
