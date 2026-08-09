using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class BuildingObj_Chair_Toilet : BuildingObj_Chair
{
    public GameObject prefab_UI;
    public int int_FoodExpend = 20;
    protected TileUI_Toilet tileUI_Bind;
    
    public override void All_OnDraw()
    {

    }
    #region 信息更新与上传

    public override void All_OnRawDataUpdate()
    {
        base.All_OnRawDataUpdate();
        tileUI_Bind?.DrawEveryCell();
    }
    #endregion
    #region 马桶
    public void All_UseToilet()
    {
        if (WorldActorManager.Instance.GetPlayer().actorManager_Bind.actorNetManager.Net_FoodCur > int_FoodExpend)
        {
            WorldActorManager.Instance.GetPlayer().actorManager_Bind.hungryManager.SubFood(int_FoodExpend);
            Type type = Type.GetType("Item_1023");
            ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(1023, out ItemData initData);
            initData.C = (short)1;
            buildingData_Chair.ReadItemDataList(out var oldList);
            GameToolManager.Instance.PutInItemList(oldList, initData, 0, 9, out ItemData itemData_Res);
            if (itemData_Res.C > 0 && itemData_Res.I > 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    index = 0,
                    itemData = itemData_Res,
                    itemFrom = ItemFrom.OutSide
                });
            }
            buildingData_Chair.WriteItemDataList(oldList);
            TryToPush();
        }
    }
    #endregion
    #region 瓦片交互
    public override void Local_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        switch (code)
        {
            case KeyCode.F: 
                { 
                    OpenOrCloseUI(tileUI_Bind == null);
                    Local_StartingSit(actor);
                } break;
        }
    }
    public override void Local_PlayerFaraway()
    {
        OpenOrCloseUI(false);
        base.Local_PlayerFaraway();
    }
    public override void OpenOrCloseUI(bool open)
    {
        if (open)
        {
            UIManager.Instance.ShowTileUI(prefab_UI, out TileUI tileUI);
            tileUI_Bind = tileUI.GetComponent<TileUI_Toilet>();
            tileUI_Bind.BindBuilding(this);
            tileUI_Bind.DrawEveryCell();
        }
        else
        {
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
            if (tileUI_Bind) tileUI_Bind = null;
        }
    }
    #endregion

}
