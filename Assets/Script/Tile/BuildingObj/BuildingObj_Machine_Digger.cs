using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using WebSocketSharp;

public class BuildingObj_Machine_Digger : BuildingObj_Machine_ByFuel
{
    public GameObject prefab_UI;
    public GameObject gameObject_DiggerBox;

    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;

    /// <summary>
    /// 单次挖掘耗时
    /// </summary>
    public int int_DigTime = 10;

    private TileUI_Digger tileUI_Bind;

    public BuildingData_Machine_Digger buildingData_Machine_Digger = new BuildingData_Machine_Digger();
    private System.Random random = new System.Random();

    public override void AllClinet_OnSecondUpdate(GameEvent.GameEvent_All_UpdateSecond eventData)
    {
        All_CheckDigger(eventData.gameTime, All_CheckFuel(eventData.gameTime, buildingData_Machine_Digger));
        buildingData_Machine_Digger.WriteLastTimeSign(eventData.gameTime);
        tileUI_Bind?.DrawEveryCell();
        tileUI_Bind?.DrawBar();
    }
    protected override void All_UpdateFuelState(bool on)
    {
        gameObject_DiggerBox.transform.DOKill();
        gameObject_DiggerBox.transform.localPosition = Vector3.zero;
        if (on) gameObject_DiggerBox.transform.DOShakePosition(0.5f, new Vector3(0.1f, 0.1f, 0)).SetLoops(-1);
        base.All_UpdateFuelState(on);
    }
    #region 挖掘计算
    protected void All_CheckDigger(int curTimeSign, int energy)
    {
        bool pushData = false;
        buildingData_Machine_Digger.ReadLastTimeSign(out int lastTimeSign);
        buildingData_Machine_Digger.ReadDigCompeletSign(out int digCompeletSign);
        if (energy > 0)/*挖机在工作*/
        {
            if (digCompeletSign == 0) digCompeletSign = curTimeSign + int_DigTime;
            if (curTimeSign > digCompeletSign) /*挖掘完成*/
            {
                digCompeletSign =  All_Dig(energy, digCompeletSign);
                pushData = true;
            }
        }
        else/*火炉不燃烧*/
        {
            digCompeletSign += (curTimeSign - lastTimeSign);
        }
        buildingData_Machine_Digger.WriteDigCompeletSign(digCompeletSign);
        if (pushData) All_TryToPush();
    }
    private int All_Dig(int energy, int digCompeletSign)
    {
        buildingData_Machine_Digger.ReadItemDataList(out var itemDatas);
        while (energy > 0)
        {
            energy = Mathf.Max(0, energy - int_DigTime);
            digCompeletSign += int_DigTime;
            var itemData = Tool_GetRandomItem(LootItemConfigData.GetLootRandomConfig(buildingTile.tileID).Loot_List, random);
            GameToolManager.Instance.PutInItemList(itemDatas, itemData, 0, 5, out _);
        }
        buildingData_Machine_Digger.WriteItemDataList(itemDatas);
        return digCompeletSign;
    }
    #endregion
    #region 信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_Machine_Digger.Deserialize(local_ByteData);
        tileUI_Bind?.DrawEveryCell();
        tileUI_Bind?.DrawBar();
        base.All_OnRawDataUpdate();
    }
    public override void All_TryToPush()
    {
        All_PushData(buildingData_Machine_Digger.Serialize());
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
            tileUI_Bind = tileUI.GetComponent<TileUI_Digger>();
            tileUI_Bind.BindBuilding(this);
            tileUI_Bind.DrawEveryCell();
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
public class BuildingData_Machine_Digger : BuildingData_Machine_ByFuel
{
    private const int MAX_ITEMS = 24;
    public List<ItemData> itemDatas_List = new List<ItemData>();
    public int gameTime_DigCompeletSign;
    public void WriteDigCompeletSign(int gamtTime)
    {
        gameTime_DigCompeletSign = gamtTime;
    }
    public void ReadDigCompeletSign(out int gamtTime)
    {
        gamtTime = gameTime_DigCompeletSign;
    }

    public void WriteItemDataList(List<ItemData> itemDatas)
    {
        itemDatas_List.Clear();
        foreach (ItemData itemData in itemDatas)
        {
            itemDatas_List.Add(itemData);
        }
    }
    public void ReadItemDataList(out List<ItemData> itemDatas)
    {
        itemDatas = new List<ItemData>(itemDatas_List);
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

            writer.Write(gameTime_DigCompeletSign);

            // 实际物品数量
            int count = itemDatas_List?.Count ?? 0;
            writer.Write((short)Math.Min(count, MAX_ITEMS));
            for (int i = 0; i < count && i < MAX_ITEMS; i++)
            {
                var item = itemDatas_List[i];
                writer.Write(item.I);
                writer.Write(item.C);
                writer.Write(item.V);
                writer.Write(item.D);
                writer.Write(item.S);
            }

            return ms.ToArray();
        }
    }
    public override void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            itemDatas_List = new List<ItemData>();
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

            gameTime_DigCompeletSign = reader.ReadInt32();

            short count = reader.ReadInt16();
            itemDatas_List = new List<ItemData>(count);
            for (int i = 0; i < count; i++)
            {
                var item = new ItemData
                {
                    I = reader.ReadInt16(),
                    C = reader.ReadInt16(),
                    V = reader.ReadInt16(),
                    D = reader.ReadSByte(),
                    S = reader.ReadInt16()
                };
                itemDatas_List.Add(item);
            }
        }
    }

}