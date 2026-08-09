using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using WebSocketSharp;

public class BuildingObj_Machine_Cook : BuildingObj_Manmade
{
    public GameObject obj_Fire;
    public GameObject obj_PotTop;
    public GameObject prefab_UI;
    public SpriteRenderer spriteRenderer_Inside;
    public SpriteAtlas spriteAtlas_Item;
    [Header("烹饪用时")]
    public int config_CookDuration;
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;
    private TileUI_Cook tileUI_Bind;
    public BuildingData_Machine_Cook buildingData_Machine_Cook = new BuildingData_Machine_Cook();
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateSecond>().Subscribe(All_OnUpdateSecond).AddTo(this);
    }
    private void All_OnUpdateSecond(GameEvent.GameEvent_All_UpdateSecond eventData)
    {
        buildingData_Machine_Cook.ReadCookFinishSign(out int gameTime_CookFinishTime);
        buildingData_Machine_Cook.ReadItemFood(out ItemData itemData);
        All_Fire(bool_RawReady && eventData.gameTime < gameTime_CookFinishTime);
        if (bool_RawReady)
        {
            if (eventData.gameTime > gameTime_CookFinishTime)
            {
                if (WorldManager.Instance.gameNetManager.Object.HasStateAuthority && itemData.I == 0)
                {
                    State_Cook();

                }
            }
            else
            {
                if (tileUI_Bind != null)
                {
                    tileUI_Bind.PlayBarAnima(1 - (gameTime_CookFinishTime - eventData.gameTime) / (float)config_CookDuration);
                }
            }
        }
    }
    #region//信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_Machine_Cook.Deserialize(local_ByteData);
        buildingData_Machine_Cook.ReadItemRaw0(out ItemData itemData_Raw0);
        buildingData_Machine_Cook.ReadItemRaw1(out ItemData itemData_Raw1);
        buildingData_Machine_Cook.ReadItemRaw2(out ItemData itemData_Raw2);
        buildingData_Machine_Cook.ReadItemFood(out ItemData itemData_Food);
        bool_RawReady = (itemData_Raw0.I != 0 && itemData_Raw1.I != 0 && itemData_Raw2.I != 0);
        All_DrawInside(itemData_Food.I);
        tileUI_Bind?.DrawAllCell();
        base.All_OnRawDataUpdate();
    }
    public void TryToPush()
    {
        WorldManager.Instance.GetTime_NowSecond(out int second);
        buildingData_Machine_Cook.WriteCookFinishSign(config_CookDuration + second);
        All_PushData(buildingData_Machine_Cook.Serialize());
    }
    #endregion
    #region//烹任
    private List<short> list_Raw = new List<short>() { };
    private bool bool_RawReady = false;

    private void State_Cook()
    {
        buildingData_Machine_Cook.ReadItemRaw0(out ItemData itemData_Raw0);
        buildingData_Machine_Cook.ReadItemRaw1(out ItemData itemData_Raw1);
        buildingData_Machine_Cook.ReadItemRaw2(out ItemData itemData_Raw2);
        list_Raw.Clear();
        list_Raw.Add(itemData_Raw0.I);
        list_Raw.Add(itemData_Raw1.I);
        list_Raw.Add(itemData_Raw2.I);

        int foodID = CookConfigData.Cook(itemData_Raw0.I, itemData_Raw1.I, itemData_Raw2.I);
        itemData_Raw0 = new ItemData();
        itemData_Raw1 = new ItemData();
        itemData_Raw2 = new ItemData();

        Type type = Type.GetType("Item_" + foodID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData((short)foodID, out ItemData itemData_Food);
        buildingData_Machine_Cook.WriteItemRaw0(itemData_Raw0);
        buildingData_Machine_Cook.WriteItemRaw1(itemData_Raw1);
        buildingData_Machine_Cook.WriteItemRaw2(itemData_Raw2);
        buildingData_Machine_Cook.WriteItemFood(itemData_Food);
        TryToPush();
    }
    private void All_Fire(bool on)
    {
        if (obj_Fire.activeSelf != on)
        {
            obj_Fire.SetActive(on);
        }
    }
    private void All_DrawInside(short id)
    {
        if (id > 0)
        {
            obj_PotTop.SetActive(false);
            spriteRenderer_Inside.gameObject.SetActive(true);
            Sprite sprite = spriteAtlas_Item.GetSprite($"Item_{id}");
            if (spriteRenderer_Inside.sprite != sprite)
            {
                spriteRenderer_Inside.sprite = spriteAtlas_Item.GetSprite($"Item_{id}");
                spriteRenderer_Inside.transform.DOPunchScale(new Vector2(-0.1f, 0.1f), 0.2f);
            }
        }
        else
        {
            spriteRenderer_Inside.gameObject.SetActive(false);
        }
    }
    public void All_Open(bool show)
    {
        if (spriteRenderer_Inside.gameObject.activeSelf == false)
        {
            obj_PotTop.SetActive(show);
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
            tileUI_Bind = tileUI.GetComponent<TileUI_Cook>();
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
public class BuildingData_Machine_Cook 
{
    private ItemData itemData_Raw0;
    private ItemData itemData_Raw1;
    private ItemData itemData_Raw2;
    private ItemData itemData_Food;
    public int gameTime_CookFinishTime;

    public void WriteItemRaw0(ItemData itemData)
    {
        itemData_Raw0 = itemData;
    }
    public void ReadItemRaw0(out ItemData itemData)
    {
        itemData = itemData_Raw0;
    }
    public void WriteItemRaw1(ItemData itemData)
    {
        itemData_Raw1 = itemData;
    }
    public void ReadItemRaw1(out ItemData itemData)
    {
        itemData = itemData_Raw1;
    }
    public void WriteItemRaw2(ItemData itemData)
    {
        itemData_Raw2 = itemData;
    }
    public void ReadItemRaw2(out ItemData itemData)
    {
        itemData = itemData_Raw2;
    }
    public void WriteItemFood(ItemData itemData)
    {
        itemData_Food = itemData;
    }
    public void ReadItemFood(out ItemData itemData)
    {
        itemData = itemData_Food;
    }
    public void WriteCookFinishSign(int gamtTime)
    {
        gameTime_CookFinishTime = gamtTime;
    }
    public void ReadCookFinishSign(out int gamtTime)
    {
        gamtTime = gameTime_CookFinishTime;
    }
    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {

            writer.Write(itemData_Raw0.I);
            writer.Write(itemData_Raw0.C);
            writer.Write(itemData_Raw0.V);
            writer.Write(itemData_Raw0.D);
            writer.Write(itemData_Raw0.S);

            writer.Write(itemData_Raw1.I);
            writer.Write(itemData_Raw1.C);
            writer.Write(itemData_Raw1.V);
            writer.Write(itemData_Raw1.D);
            writer.Write(itemData_Raw1.S);

            writer.Write(itemData_Raw2.I);
            writer.Write(itemData_Raw2.C);
            writer.Write(itemData_Raw2.V);
            writer.Write(itemData_Raw2.D);
            writer.Write(itemData_Raw2.S);

            writer.Write(itemData_Food.I);
            writer.Write(itemData_Food.C);
            writer.Write(itemData_Food.V);
            writer.Write(itemData_Food.D);
            writer.Write(itemData_Food.S);


            writer.Write(gameTime_CookFinishTime);
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
            itemData_Raw0 = new ItemData
            {
                I = reader.ReadInt16(),
                C = reader.ReadInt16(),
                V = reader.ReadInt16(),
                D = reader.ReadSByte(),
                S = reader.ReadInt16()
            };
            itemData_Raw1 = new ItemData
            {
                I = reader.ReadInt16(),
                C = reader.ReadInt16(),
                V = reader.ReadInt16(),
                D = reader.ReadSByte(),
                S = reader.ReadInt16()
            };
            itemData_Raw2 = new ItemData
            {
                I = reader.ReadInt16(),
                C = reader.ReadInt16(),
                V = reader.ReadInt16(),
                D = reader.ReadSByte(),
                S = reader.ReadInt16()
            };
            itemData_Food = new ItemData
            {
                I = reader.ReadInt16(),
                C = reader.ReadInt16(),
                V = reader.ReadInt16(),
                D = reader.ReadSByte(),
                S = reader.ReadInt16()
            };

            gameTime_CookFinishTime = reader.ReadInt32();
        }
    }
}