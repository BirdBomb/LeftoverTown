using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;
using WebSocketSharp;

public class BuildingObj_Machine_Smelter : BuildingObj_Machine_ByFuel
{
    public GameObject obj_Fire;
    public GameObject prefab_UI;
    private TileUI_Smelter tileUI_Bind;
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;
    public BuildingData_Machine_Smelter buildingData_Machine_Smelter = new BuildingData_Machine_Smelter();
    #region 精炼计算
    protected override void All_UpdateFuelState(bool on)
    {
        obj_Fire.SetActive(on); base.All_UpdateFuelState(on);
    }
    public override void AllClinet_OnSecondUpdate(GameEvent.GameEvent_All_UpdateSecond eventData)
    {
        All_CheckRefining(eventData.gameTime, All_CheckFuel(eventData.gameTime, buildingData_Machine_Smelter));
        buildingData_Machine_Smelter.WriteLastTimeSign(eventData.gameTime);
        tileUI_Bind?.DrawEveryCell();
        tileUI_Bind?.DrawBar();
        base.AllClinet_OnSecondUpdate(eventData);
    }
    protected void All_CheckRefining(int curTimeSign, int energy)
    {
        bool pushData = false;
        buildingData_Machine_Smelter.ReadLastTimeSign(out int lastTimeSign);
        buildingData_Machine_Smelter.ReadItemRefiningBefore(out ItemData itemRefiningBefore);
        buildingData_Machine_Smelter.ReadItemRefiningAfter(out ItemData itemRefiningAfter);
        buildingData_Machine_Smelter.ReadRefiningCompeletSign(out int refiningCompeletSign);
        RefiningConfig config = RefiningConfigData.GetRefiningConfig(itemRefiningBefore.I);
        /*有原料*/
        if (itemRefiningBefore.I != 0 && itemRefiningBefore.C != 0)
        {
            /*火炉在燃烧*/
            if (energy > 0 )
            {
                /*炼制完成*/
                if(curTimeSign > refiningCompeletSign)
                {
                    /*合成路径通顺*/
                    if (itemRefiningAfter.I == 0)
                    {
                        refiningCompeletSign = All_Smelter(energy, itemRefiningBefore, Tool_CreateItemData(config.RefiningAfterID, 0), config.RefiningSecond, refiningCompeletSign);
                        pushData = true;
                    }
                    else if (itemRefiningAfter.I == config.RefiningAfterID)
                    {
                        refiningCompeletSign = All_Smelter(energy, itemRefiningBefore, itemRefiningAfter, config.RefiningSecond, refiningCompeletSign);
                        pushData = true;
                    }
                    /*合成路径阻塞*/
                    else
                    {
                        refiningCompeletSign += (curTimeSign - lastTimeSign);
                    }
                }
                /*炼制未完成*/
                else
                {

                }
            }
            else
            {
                refiningCompeletSign += (curTimeSign - lastTimeSign);
            }
        }
        /*无原料*/
        else
        {
            refiningCompeletSign = int.MaxValue;
        }
        buildingData_Machine_Smelter.WriteRefiningCompeletSign(refiningCompeletSign);
        buildingData_Machine_Smelter.WriteRefiningMax(config.RefiningSecond);
        if (pushData) All_TryToPush();
    }
    protected int All_Smelter(int energy, ItemData before, ItemData after, int expend,int refiningCompeletSign)
    {
        ItemData itemData_Expend = before;
        itemData_Expend.C = 0;
        ItemData itemData_Create = Tool_CreateItemData(after.I, 0);
        itemData_Create.C = 0;
        while (energy > 0 && itemData_Expend.C < before.C)
        {
            energy = Mathf.Max(0, energy - expend);
            refiningCompeletSign += expend;
            itemData_Expend.C += 1;
            itemData_Create.C += 1;
        }
        buildingData_Machine_Smelter.WriteItemRefiningBefore(GameToolManager.Instance.SplitItem(before, itemData_Expend));
        buildingData_Machine_Smelter.WriteItemRefiningAfter(GameToolManager.Instance.CombineItem(after, itemData_Create, out _));
        return refiningCompeletSign;
    }
    #endregion
    #region 信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_Machine_Smelter.Deserialize(local_ByteData);
        tileUI_Bind?.DrawEveryCell();
        tileUI_Bind?.DrawBar();
        base.All_OnRawDataUpdate();
    }
    public override void All_TryToPush()
    {
        All_PushData(buildingData_Machine_Smelter.Serialize());
        base.All_TryToPush();
    }
    #endregion
    #region 瓦片交互
    public override void Local_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        switch (code)
        {
            case KeyCode.F:
                OpenOrCloseUI(tileUI_Bind == null); break;
        }
        base.Local_ActorInputKeycode(actor, code);
    }
    public override void Local_PlayerHighlight(bool on)
    {
        OpenOrCloseHighlightUI(on);
        base.Local_PlayerHighlight(on);
    }
    public override void Local_PlayerFaraway()
    {
        OpenOrCloseUI(false);
        base.Local_PlayerFaraway();
    }
    public override void OpenOrCloseHighlightUI(bool open)
    {
        if (open)
        {
            obj_SingalUI_F = obj_SingalUI_F ? obj_SingalUI_F : PoolManager.Instance.GetObject("UI/TileUI/SignalUI_F");
            obj_SingalUI_F.transform.position = transform.position + All_GetTileGenter();
            obj_SingalUI_F.transform.localScale = Vector3.one;
            obj_SingalUI_F.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_HighlightUI = obj_HighlightUI ? obj_HighlightUI : PoolManager.Instance.GetObject("UI/TileUI/" + All_GetTileSize());
            obj_HighlightUI.transform.position = transform.position;
            obj_HighlightUI.transform.localScale = Vector3.one;
            obj_HighlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_F", obj_SingalUI_F);
            obj_SingalUI_F = null;
            PoolManager.Instance.ReleaseObject("UI/TileUI/" + All_GetTileSize(), obj_HighlightUI);
            obj_HighlightUI = null;
        }
    }
    public override void OpenOrCloseAwakeUI(bool open)
    {
        if (open)
        {
            if (obj_SingalUI_F)
            {
                PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_F", obj_SingalUI_F);
                obj_SingalUI_F = null;
            }
            obj_SingalUI_Awake = obj_SingalUI_Awake ? obj_SingalUI_Awake : PoolManager.Instance.GetObject("UI/TileUI/SignalUI_Awake");
            obj_SingalUI_Awake.transform.position = transform.position + All_GetTileGenter();
            obj_SingalUI_Awake.transform.localScale = Vector3.one;
            obj_SingalUI_Awake.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_Awake", obj_SingalUI_Awake);
            obj_SingalUI_Awake = null;
        }
    }
    public override void OpenOrCloseUI(bool open)
    {
        if (open)
        {
            UIManager.Instance.ShowTileUI(prefab_UI, out TileUI tileUI);
            tileUI_Bind = tileUI.GetComponent<TileUI_Smelter>();
            tileUI_Bind.BindBuilding(this);
            tileUI_Bind?.DrawEveryCell();
            tileUI_Bind?.DrawBar();
        }
        else
        {
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
            if (tileUI_Bind) tileUI_Bind = null;
        }

    }
    public override bool CanHighlight()
    {
        return true;
    }

    #endregion
}
public class BuildingData_Machine_Smelter : BuildingData_Machine_ByFuel
{
    public ItemData itemData_RefiningBefore;
    public ItemData itemData_RefiningAfter;
    public int gameTime_RefiningCompeletSign;
    public int gameTime_RefiningMax;
    public void WriteItemRefiningBefore(ItemData itemData)
    {
        itemData_RefiningBefore = itemData;
    }
    public void ReadItemRefiningBefore(out ItemData itemData)
    {
        itemData = itemData_RefiningBefore;
    }
    public void WriteItemRefiningAfter(ItemData itemData)
    {

        itemData_RefiningAfter = itemData;
    }
    public void ReadItemRefiningAfter(out ItemData itemData)
    {
        itemData = itemData_RefiningAfter;
    }
    public void WriteRefiningCompeletSign(int gamtTime)
    {
        gameTime_RefiningCompeletSign = gamtTime;
    }
    public void ReadRefiningCompeletSign(out int gamtTime)
    {
        gamtTime = gameTime_RefiningCompeletSign;
    }
    public void WriteRefiningMax(int gamtTime)
    {
        gameTime_RefiningMax = gamtTime;
    }
    public void ReadRefiningMax(out int gamtTime)
    {
        gamtTime = gameTime_RefiningMax;
    }
    public override byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(gameTime_LastTimeSign);
            writer.Write(gameTime_FuelDepletedTimeSign);
            writer.Write(gameTime_FuelMax);

            writer.Write(itemData_Fuel.I);
            writer.Write(itemData_Fuel.C);
            writer.Write(itemData_Fuel.V);
            writer.Write(itemData_Fuel.D);
            writer.Write(itemData_Fuel.S);

            writer.Write(gameTime_RefiningCompeletSign);
            writer.Write(gameTime_RefiningMax);

            writer.Write(itemData_RefiningBefore.I);
            writer.Write(itemData_RefiningBefore.C);
            writer.Write(itemData_RefiningBefore.V);
            writer.Write(itemData_RefiningBefore.D);
            writer.Write(itemData_RefiningBefore.S);

            writer.Write(itemData_RefiningAfter.I);
            writer.Write(itemData_RefiningAfter.C);
            writer.Write(itemData_RefiningAfter.V);
            writer.Write(itemData_RefiningAfter.D);
            writer.Write(itemData_RefiningAfter.S);
            return ms.ToArray();
        }
    }
    public override void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return;
        }

        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
            gameTime_LastTimeSign = reader.ReadInt32();
            gameTime_FuelDepletedTimeSign = reader.ReadInt32();
            gameTime_FuelMax = reader.ReadInt32();

            itemData_Fuel = new ItemData
            {
                I = reader.ReadInt16(),
                C = reader.ReadInt16(),
                V = reader.ReadInt16(),
                D = reader.ReadSByte(),
                S = reader.ReadInt16()
            };

            gameTime_RefiningCompeletSign = reader.ReadInt32();
            gameTime_RefiningMax = reader.ReadInt32();

            itemData_RefiningBefore = new ItemData
            {
                I = reader.ReadInt16(),
                C = reader.ReadInt16(),
                V = reader.ReadInt16(),
                D = reader.ReadSByte(),
                S = reader.ReadInt16()
            };
            itemData_RefiningAfter = new ItemData
            {
                I = reader.ReadInt16(),
                C = reader.ReadInt16(),
                V = reader.ReadInt16(),
                D = reader.ReadSByte(),
                S = reader.ReadInt16()
            };
        }
    }
}