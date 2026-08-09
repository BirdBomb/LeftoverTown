using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.Accessibility;
using WebSocketSharp;

public class BuildingObj_Machine_Blender : BuildingObj_Manmade
{
    public GameObject prefab_UI;
    private TileUI_Blender tileUI_Bind = null;
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;

    public BuildingData_Blender buildingData_Blender = new BuildingData_Blender();
    #region//信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_Blender.Deserialize(local_ByteData);
        tileUI_Bind?.DrawAllCell();
        base.All_OnRawDataUpdate();
    }
    public void All_TryToPush()
    {
        All_PushData(buildingData_Blender.Serialize());
    }
    #endregion
    #region//制造
    public void Local_Blender()
    {
        buildingData_Blender.ReadItemDataFrom(out ItemData itemData_From);
        buildingData_Blender.ReadItemDataTo(out ItemData itemData_To);
        if (itemData_From.C > 0 && itemData_From.I > 0)
        {
            BlenderConfig blenderConfig = BlenderConfigData.GetBlenderConfig(itemData_From.I);
            int toID = blenderConfig.blender_ToID;
            int toCount = blenderConfig.blender_ToCount;
            if (itemData_To.I == 0 || itemData_To.I == toID)
            {
                ItemData itemData_Expend = itemData_From;
                itemData_Expend.C = 1;
                itemData_From = GameToolManager.Instance.SplitItem(itemData_From, itemData_Expend);
                Type type = Type.GetType("Item_" + toID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData((short)toID, out ItemData initData);
                initData.C = (short)toCount;
                if (itemData_To.I == 0)
                {
                    itemData_To = initData;
                }
                else if (itemData_To.I == toID)
                {
                    itemData_To = GameToolManager.Instance.CombineItem(itemData_To, initData, out ItemData itemData_Res);
                    if (itemData_Res.C > 0 && itemData_Res.I > 0)
                    {
                        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                        {
                            index = 0,
                            itemData = itemData_Res,
                            itemFrom =ItemFrom.OutSide
                        });
                    }
                }
            }
            buildingData_Blender.WriteItemDataFrom(itemData_From);
            buildingData_Blender.WriteItemDataTo(itemData_To);
            All_TryToPush();
        }
    }
    #endregion
    #region//瓦片交互
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
            tileUI_Bind = tileUI.GetComponent<TileUI_Blender>();
            tileUI_Bind.BindBuilding(this);
            tileUI_Bind.DrawAllCell();
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
public class BuildingData_Blender
{
    public ItemData itemData_From;
    public ItemData itemData_To;
    public void WriteItemDataFrom(ItemData itemData) { itemData_From = itemData; }
    public void ReadItemDataFrom(out ItemData itemData) { itemData = itemData_From; }
    public void WriteItemDataTo(ItemData itemData) { itemData_To = itemData; }
    public void ReadItemDataTo(out ItemData itemData) { itemData = itemData_To; }
    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            for (int i = 0; i < 2; i++)
            {
                switch (i) 
                {
                    case 0: 
                        {
                            writer.Write(itemData_From.I);
                            writer.Write(itemData_From.C);
                            writer.Write(itemData_From.V);
                            writer.Write(itemData_From.D);
                            writer.Write(itemData_From.S);
                        }
                        break;
                    case 1:
                        {
                            writer.Write(itemData_To.I);
                            writer.Write(itemData_To.C);
                            writer.Write(itemData_To.V);
                            writer.Write(itemData_To.D);
                            writer.Write(itemData_To.S);
                        }
                        break;
                }
            }
            return ms.ToArray();
        }
    }
    public void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            itemData_From = new ItemData();
            itemData_To = new ItemData();
            return;
        }

        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            itemData_From = new ItemData
                            {
                                I = reader.ReadInt16(),
                                C = reader.ReadInt16(),
                                V = reader.ReadInt16(),
                                D = reader.ReadSByte(),
                                S = reader.ReadInt16()
                            };
                        }
                        break;
                    case 1:
                        {
                            itemData_To = new ItemData
                            {
                                I = reader.ReadInt16(),
                                C = reader.ReadInt16(),
                                V = reader.ReadInt16(),
                                D = reader.ReadSByte(),
                                S = reader.ReadInt16()
                            };
                        }
                        break;
                }
            }
        }
    }
}