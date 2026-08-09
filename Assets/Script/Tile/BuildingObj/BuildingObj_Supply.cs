using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniRx;
using System.Linq;
using System.IO;
using System;
/// <summary>
/// 藏匿点
/// </summary>
public class BuildingObj_Supply : BuildingObj_Box
{
    [Header("藏匿点刷新周期(小时)")]
    public int int_ResetTime = 9999;

    public override void Start()
    {
        SubscribeToEvents();
        All_CompareTime();
    }
    #region 初始化
    public virtual void SubscribeToEvents()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(All_OnHourUpdate).AddTo(this);
    }
    #endregion
    #region 藏匿点刷新
    public virtual void All_OnHourUpdate(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        All_CompareTime();
    }
    public virtual void All_CompareTime()
    {
        WorldManager.Instance.GetTime_NowHour(out int now);
        if (now>= buildingData_Box.ReadSignTime())
        {
            buildingData_Box.WriteSignTime(now + int_ResetTime);
            buildingData_Box.WriteItemDataList(Tool_GetRandomItemList(LootItemConfigData.GetLootRandomConfig(buildingTile.tileID).Loot_List, new System.Random().Next(3, 6)));
            All_TryToPush();
        }
    }
    #endregion
}
