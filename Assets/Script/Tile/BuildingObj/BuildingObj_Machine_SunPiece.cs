using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;
using WebSocketSharp;

public class BuildingObj_SunPiece : BuildingObj_Manmade
{
    public GameObject prefab_UI;
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;
    protected TileUI_SunPiece tileUI_Bind;
    public BuildingData_SunPiece buildingData_SunPiece = new BuildingData_SunPiece();
    #region//信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_SunPiece.Deserialize(local_ByteData);
        All_UpdateSun(buildingData_SunPiece.ReadItemData().C);
        tileUI_Bind?.DrawCell();
        tileUI_Bind?.CheckCell();

        base.All_OnRawDataUpdate();
    }
    public void TryToPush()
    {
        All_PushData(buildingData_SunPiece.Serialize());
    }
    #endregion
    #region//太阳祭坛
    public void All_UpdateSun(int count)
    {
        int int_SunLevel;
        short short_SunRange;
        if (count == 0)
        {
            int_SunLevel = 0;
            short_SunRange = 10;
        }
        else if (count < 10)
        {
            int_SunLevel = 1;
            short_SunRange = 200;
        }
        else if (count < 50)
        {
            int_SunLevel = 2;
            short_SunRange = 400;
        }
        else if (count < 100)
        {
            int_SunLevel = 3;
            short_SunRange = 600;
        }
        else if (count < 500)
        {
            int_SunLevel = 4;
            short_SunRange = 800;
        }
        else if (count < 1000)
        {
            int_SunLevel = 5;
            short_SunRange = 1000;
        }
        else if (count < 5000)
        {
            int_SunLevel = 6;
            short_SunRange = 1200;
        }
        else if (count < 10000)
        {
            int_SunLevel = 7;
            short_SunRange = 1500;
        }
        else
        {
            int_SunLevel = 8;
            short_SunRange = 200;
        }
        MessageBroker.Default.Publish(new MapEvent.MapEvent_Local_ChangeSunLight() 
        {
            range = short_SunRange
        });
        buildingData_SunPiece.WriteSunLevel(int_SunLevel);
        buildingData_SunPiece.WriteSunRange(short_SunRange);
    }
    public void All_CheckSun(out short sunLevel,out short sunRange)
    {
        short count = buildingData_SunPiece.ReadItemData().C;
        if (count == 0)
        {
            sunLevel = 0;
            sunRange = 10;
        }
        else if (count < 10)
        {
            sunLevel = 1;
            sunRange = 200;
        }
        else if (count < 50)
        {
            sunLevel = 2;
            sunRange = 400;
        }
        else if (count < 100)
        {
            sunLevel = 3;
            sunRange = 600;
        }
        else if (count < 500)
        {
            sunLevel = 4;
            sunRange = 800;
        }
        else if (count < 1000)
        {
            sunLevel = 5;
            sunRange = 1000;
        }
        else if (count < 5000)
        {
            sunLevel = 6;
            sunRange = 1200;
        }
        else if (count < 10000)
        {
            sunLevel = 7;
            sunRange = 1500;
        }
        else
        {
            sunLevel = 8;
            sunRange = 200;
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
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_F", obj_SingalUI_F);
            obj_SingalUI_F = null;
        }
        if (open)
        {
            obj_HighlightUI = obj_HighlightUI ? obj_HighlightUI : PoolManager.Instance.GetObject("UI/TileUI/" + All_GetTileSize());
            obj_HighlightUI.transform.position = transform.position;
            obj_HighlightUI.transform.localScale = Vector3.one;
            obj_HighlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
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
            tileUI_Bind = tileUI.GetComponent<TileUI_SunPiece>();
            tileUI_Bind.BindBuilding(this);
            tileUI_Bind.DrawCell();
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
public class BuildingData_SunPiece
{
    public ItemData itemData_SunPiece;
    public int int_SunLevel = 0;
    public short short_SunRange = 0;
    public ItemData ReadItemData() { return itemData_SunPiece; }
    public void WriteItemData(ItemData val) { itemData_SunPiece = val; }

    public int ReadSunLevel() { return int_SunLevel; }
    public void WriteSunLevel(int val) { int_SunLevel = val; }
    public short ReadSunRange() { return short_SunRange; }
    public void WriteSunRange(short val) { short_SunRange = val; }
    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(int_SunLevel);
            writer.Write(short_SunRange);
            writer.Write(itemData_SunPiece.I);
            writer.Write(itemData_SunPiece.C);
            writer.Write(itemData_SunPiece.V);
            writer.Write(itemData_SunPiece.D);
            writer.Write(itemData_SunPiece.S);
            return ms.ToArray();
        }
    }
    public void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return;
        }
        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
            int_SunLevel = reader.ReadInt32();
            short_SunRange = reader.ReadInt16();
            itemData_SunPiece = new ItemData
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