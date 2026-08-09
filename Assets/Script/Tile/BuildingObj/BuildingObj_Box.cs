using DG.Tweening;
using Fusion;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class BuildingObj_Box : BuildingObj_Manmade
{
    public GameObject prefab_UI;
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;
    protected TileUI_BoxBase tileUI_Bind;
    public enum BoxState { Close, Open }
    protected BoxState boxState = BoxState.Close;

    public BuildingData_Box buildingData_Box = new BuildingData_Box();

    #region 信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_Box.Deserialize(local_ByteData);
        tileUI_Bind?.DrawEveryCell();
        base.All_OnRawDataUpdate();
    }
    public virtual void All_TryToPush()
    {
        All_PushData(buildingData_Box.Serialize());
    }
    #endregion
    #region 箱子
    public virtual void All_ChangeBoxState(BoxState state)
    {
        
    }
    #endregion
    #region 瓦片交互
    public override void Local_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        switch (code)
        {
            case KeyCode.F: { OpenOrCloseUI(tileUI_Bind == null); } break;
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
            tileUI_Bind = tileUI.GetComponent<TileUI_BoxBase>();
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
public class BuildingData_Box
{
    private const int MAX_ITEMS = 30;
    public List<ItemData> itemDatas_List = new List<ItemData>();
    public bool bool_Lock = true;
    public int int_SignTime = int.MinValue;
    public void WriteItemDataList(List<ItemData> itemDatas)
    {
        itemDatas_List.Clear();
        foreach(ItemData itemData in itemDatas)
        {
            itemDatas_List.Add(itemData);
        }
    }
    public void ReadItemDataList(out List<ItemData> itemDatas)
    {
        itemDatas = new List<ItemData>(itemDatas_List);
    }
    public bool ReadLock() { return bool_Lock; }
    public void WriteLock(bool val) { bool_Lock = val; }
    public int ReadSignTime() { return int_SignTime; }
    public void WriteSignTime(int val) { int_SignTime = val; }
    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
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
            writer.Write(bool_Lock);
            writer.Write(int_SignTime);

            return ms.ToArray();
        }
    }
    public void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            itemDatas_List = new List<ItemData>();
            return;
        }

        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
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
            bool_Lock = reader.ReadBoolean();
            int_SignTime = reader.ReadInt32();
        }
    }
}