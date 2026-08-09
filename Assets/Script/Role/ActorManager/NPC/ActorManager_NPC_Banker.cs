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
    #region//行为逻辑
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {
        switch (time)
        {
            case GlobalTime.Forenoon:
                {
                    if (State_Think_GoToWork()) return;
                    break;
                }
            case GlobalTime.Highnoon:
                {
                    if (State_Think_GoForFood()) return;
                    break;
                }
            case GlobalTime.Afternoon:
                {
                    if (State_Think_GoToWork()) return;
                    break;
                }
            case GlobalTime.Dusk:
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
        base.State_ThinkByTimeUpdate(date, hour, time);
    }
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {
        switch (globalTime)
        {
            case GlobalTime.Forenoon:
                StartCoroutine(State_Think_FindWorkPos());
                break;
            case GlobalTime.Highnoon:
                StartCoroutine(State_Think_FindFoodPos());
                break;
            case GlobalTime.Afternoon:
                StartCoroutine(State_Think_FindWorkPos());
                break;
            case GlobalTime.Dusk:
                StartCoroutine(State_Think_FindFoodPos());
                break;
            case GlobalTime.Evening:
                StartCoroutine(State_Think_FindSleepPos());
                break;
        }
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
            new DialogOption("Banker_Dialog0_Option0", Local_StartDeal),
            new DialogOption("Banker_Dialog0_Option1", Local_OverDialog)
        };
        ShowDialog("Banker_Dialog0", options);
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
