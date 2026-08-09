using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using WebSocketSharp;

public class BuildingObj_Machine_TvByFuel : BuildingObj_Machine_ByFuel
{
    public GameObject prefab_UI;
    public SpriteRenderer spriteRenderer_Screen;
    public TextMesh textMesh_Sound;
    public GameObject gameObject_Fire;
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;
    protected List<string> list_String = new List<string>();

    private TileUI_TV tileUI_Bind;
    private int temp_SoundTimer;
    private int temp_SoundDuraction = 2;
    private System.Random random = new System.Random();
    public BuildingData_Machine_TvByFuel buildingData_Machine_TvByFuel = new BuildingData_Machine_TvByFuel();

    public override void AllClinet_OnSecondUpdate(GameEvent.GameEvent_All_UpdateSecond eventData)
    {
        All_CheckFuel(eventData.gameTime, buildingData_Machine_TvByFuel);
        buildingData_Machine_TvByFuel.WriteLastTimeSign(eventData.gameTime);
        tileUI_Bind?.DrawEveryCell();
        tileUI_Bind?.DrawBar();
    }

    protected override void All_UpdateFuelState(bool on)
    {
        gameObject_Fire.SetActive(on);
        spriteRenderer_Screen.gameObject.SetActive(on);
        if (on)
        {
            buildingData_Machine_TvByFuel.ReadItemDataVHS(out ItemData itemData_VHS);
            if(ItemConfigData.GetItemConfig(itemData_VHS.I).Item_Type == ItemType.VHS)
            {
                if (temp_SoundTimer > temp_SoundDuraction) 
                {
                    textMesh_Sound.transform.DOPunchScale(new Vector3(-0.01f, 0.01f, 0), 0.2f).SetEase(Ease.InOutBack);
                    list_String = LocalizationManager.Instance.GetLocalization("Vhs_String", itemData_VHS.I.ToString()).Split("/").ToList();
                    if (list_String.Count > 0) textMesh_Sound.text = list_String[random.Next(0, list_String.Count)];
                    temp_SoundTimer = 0;
                }
                else
                {
                    temp_SoundTimer++;
                }
                spriteRenderer_Screen.color = (Color)(VhsConfigData.GetVhsConfig(itemData_VHS.I).Vhs_Color);
            }
            else
            {
                textMesh_Sound.text = "<电流声>";
                spriteRenderer_Screen.color = Color.black;
            }
        }
        base.All_UpdateFuelState(on);
    }
    #region 信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_Machine_TvByFuel.Deserialize(local_ByteData);
        base.All_OnRawDataUpdate();
    }
    public override void All_TryToPush()
    {
        All_PushData(buildingData_Machine_TvByFuel.Serialize());
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
            tileUI_Bind = tileUI.GetComponent<TileUI_TV>();
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
public class BuildingData_Machine_TvByFuel : BuildingData_Machine_ByFuel
{
    public ItemData itemData_VHS;
    public void WriteItemDataVHS(ItemData itemData)
    {
        itemData_VHS = itemData;
    }
    public void ReadItemDataVHS(out ItemData itemData)
    {
        itemData = itemData_VHS;
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

            writer.Write(itemData_VHS.I);
            writer.Write(itemData_VHS.C);
            writer.Write(itemData_VHS.V);
            writer.Write(itemData_VHS.D);
            writer.Write(itemData_VHS.S);

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
            itemData_VHS = new ItemData
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