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
    System.Random random = new System.Random(); 
    #region//检查
    public override bool State_CheckNearbyActor()
    {
        return true;
    }
    #endregion
    #region//思考
    /// <summary>
    /// 根据时间决定动作(经常触发)
    /// </summary>
    public override void State_ThinkByTimeUpdate(int date, int hour, GlobalTime time)
    {

    }
    /// <summary>
    /// 根据时间变化决定动作(关键时间触发)
    /// </summary>
    public override void State_ThinkByTimeChange(int date, int hour, GlobalTime globalTime)
    {

    }
    #endregion
    #region//百科
    private TileUI_Dictionary tileUI_Dictionary;
    public override void Local_PlayerFaraway(ActorManager actor)
    {
        Local_OverDictionary();
        base.Local_PlayerFaraway(actor);
    }
    /// <summary>
    /// 开始字典
    /// </summary>
    /// <param name="actor"></param>
    private void Local_StartDictionary()
    {
        UIManager.Instance.ShowTileUI(Resources.Load<GameObject>("UI/TileUI/TileUI_Dictionary"), out TileUI tileUI);
        tileUI_Dictionary = tileUI.GetComponent<TileUI_Dictionary>();
        tileUI_Dictionary.Init();
    }
    /// <summary>
    /// 结束字典
    /// </summary>
    /// <param name="actor"></param>
    private void Local_OverDictionary()
    {
        UIManager.Instance.HideTileUI(tileUI_Dictionary);
    }

    #endregion
    #region//交互
    public override void Local_StartDialog()
    {
        base.Local_StartDialog();
        dialogMap = new Dictionary<int, Action>();
        dialogMap[0] = DiagLog_TheSunlightIsTooWeek;
        dialogMap[1] = DiagLog_NeedSunDebris;
        dialogMap[2] = DiagLog_PoorGuyHaveSome;
        dialogMap[3] = DiagLog_TheSunlightIsBetterNow;
        dialogMap[4] = DiagLog_WhatDoYouWantToKnow;
        dialogMap[5] = DiagLog_BeatSomeEnemy;
        dialogMap[6] = DiagLog_ISealSomething;
        dialogMap[7] = DiagLog_IDonotCare;
        
        if (WorldManager.Instance.gameNetManager.SunLight < 50) ChooseDialog(0);
        else ChooseDialog(3);

        
    }

    /// <summary>
    /// 太阳太弱了...你像我一样都被困住了
    /// </summary>
    public void DiagLog_TheSunlightIsTooWeek()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Guide_Dialog_SunTooWeek_Option_HowToLeave", ()=>ChooseDialog(1)),
        };
        ShowDialog("Guide_Dialog_SunTooWeek", options);
    }
    /// <summary>
    /// 祭坛需要碎片来提高光照等级
    /// </summary>
    public void DiagLog_NeedSunDebris()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Guide_Dialog_NeedSunDebris_Option_HowToGet", ()=>ChooseDialog(2)),
        };
        ShowDialog("Guide_Dialog_NeedSunDebris", options);
    }
    /// <summary>
    /// 倒在小屋里的那个家伙身上可能有
    /// </summary>
    public void DiagLog_PoorGuyHaveSome()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Guide_Dialog_Option_OK", ()=>Local_OverDialog()),
        };
        ShowDialog("Guide_Dialog_PoorGuyHaveSome", options);
    }
    /// <summary>
    /// 太阳亮一点了，你需要更多碎片来修复下一阶段
    /// </summary>
    public void DiagLog_TheSunlightIsBetterNow()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Guide_Dialog_Option_AskSomething", ()=>ChooseDialog(4)),
            new DialogOption("Guide_Dialog_Option_Deal", ()=>Local_StartDeal()),
            new DialogOption("Guide_Dialog_Option_Dictionary", ()=>Local_StartDictionary()),
        };
        ShowDialog("Guide_Dialog_TheSunlightIsBetterNow", options);
    }
    /// <summary>
    /// 你想知道什么
    /// </summary>
    public void DiagLog_WhatDoYouWantToKnow()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Guide_Dialog_WhatDoYouWantToKnow_Option_HowToGetMore", ()=>ChooseDialog(5)),
            new DialogOption("Guide_Dialog_WhatDoYouWantToKnow_Option_WhoAreYou", ()=>ChooseDialog(6)),
            new DialogOption("Guide_Dialog_WhatDoYouWantToKnow_Option_WhatToDo", ()=>ChooseDialog(7)),
        };
        ShowDialog("Guide_Dialog_WhatDoYouWantToKnow", options);
    }
    /// <summary>
    /// 去打倒一些敌人
    /// </summary>
    public void DiagLog_BeatSomeEnemy()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Guide_Dialog_Option_AskSomething", ()=>ChooseDialog(4)),
            new DialogOption("Guide_Dialog_Option_OK", ()=>Local_OverDialog()),
        };
        ShowDialog("Guide_Dialog_BeatSomeEnemy", options);
    }
    /// <summary>
    /// 这里既明亮又有稳定客源，我在这里卖一些东西。你如果想买其他的就去南边的镇子看看，那里卖什么的都有
    /// </summary>
    public void DiagLog_ISealSomething()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Guide_Dialog_Option_AskSomething", ()=>ChooseDialog(4)),
            new DialogOption("Guide_Dialog_Option_Deal", ()=>Local_StartDeal()),
            new DialogOption("Guide_Dialog_Option_OK", ()=>Local_OverDialog()),
        };
        ShowDialog("Guide_Dialog_ISealSomething", options);
    }
    /// <summary>
    /// 如果你想的话就帮忙收集一些太阳碎片，不想的话尽量活下去。要是你觉得野外太危险就找个镇子住进去，从这里往南边走就是
    /// </summary>
    public void DiagLog_IDonotCare()
    {
        List<DialogOption> options = new List<DialogOption>
        {
            new DialogOption("Guide_Dialog_Option_AskSomething", ()=>ChooseDialog(4)),
            new DialogOption("Guide_Dialog_Option_OK", ()=>Local_OverDialog()),
        };
        ShowDialog("Guide_Dialog_IDonotCare", options);
    }
    #endregion
    #region//交易
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
    #endregion
}
