using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

public class BuildingObj_GoodShelf : BuildingObj_Manmade
{
    [Header("供货周期(小时)")]
    public int int_ResetTime = 10;
    public List<Sprite> list_Sprites = new List<Sprite>();
    public SpriteRenderer spriteRenderer_Main;
    public GameObject prefab_UI;
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;
    protected TileUI_GoodShelf tileUI_Bind;
    private System.Random random = new System.Random();
    public BuildingData_GoodShelf buildingData_GoodShelf = new BuildingData_GoodShelf();
    public override void Start()
    {
        SubscribeToEvents();
        LoadInitialState();
    }
    #region//初始化
    public virtual void SubscribeToEvents()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_UpdateHour>().Subscribe(State_OnHourUpdated).AddTo(this);
    }

    public virtual void LoadInitialState()
    {
        if (WorldManager.Instance.gameNetManager.HasStateAuthority)
        {
            State_CompareTime();
        }
    }

    #endregion
    #region 时间更新
    public virtual void State_OnHourUpdated(GameEvent.GameEvent_State_UpdateHour eventData)
    {
        State_CompareTime();
    }
    public virtual void State_CompareTime()
    {
        WorldManager.Instance.GetTime_NowHour(out int now);
        if (now >= buildingData_GoodShelf.ReadSignTime())
        {
            buildingData_GoodShelf.WriteSignTime(now + int_ResetTime);
            buildingData_GoodShelf.WriteItemDataList(Tool_GetRandomItemList(LootItemConfigData.GetLootRandomConfig(buildingTile.tileID).Loot_List, random.Next(3, 6)));
            ForState_PushData(buildingData_GoodShelf.Serialize());
        }
    }
    public override void All_OnCreate()
    {
        base.All_OnCreate();
    }
    #endregion

    #region 信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_GoodShelf.Deserialize(local_ByteData);
        buildingData_GoodShelf.ReadItemDataList(out var itemDatas);
        tileUI_Bind?.DrawEveryCell();
        UpdateLook(itemDatas);
        base.All_OnRawDataUpdate();
    }
    public virtual void All_TryToPush()
    {
        All_PushData(buildingData_GoodShelf.Serialize());
    }
    public override void All_ReceiveData(byte[] data)
    {
        base.All_ReceiveData(data); 
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
            tileUI_Bind = tileUI.GetComponent<TileUI_GoodShelf>();
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
    #region//视效果
    public virtual void UpdateLook(List<ItemData> itemDatas)
    {
        if (list_Sprites != null && itemDatas != null && list_Sprites.Count > 0)
        {
            spriteRenderer_Main.sprite = list_Sprites[Mathf.Min(itemDatas.Count, list_Sprites.Count - 1)];
        }
    }
    #endregion
}
public class BuildingData_GoodShelf
{
    private const int MAX_ITEMS = 30;
    public List<ItemData> itemDatas_List = new List<ItemData>();
    public int int_SignTime = int.MinValue;
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
    public int ReadSignTime() { return int_SignTime; }
    public void WriteSignTime(int val) { int_SignTime = val; }

    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(int_SignTime);
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
            int_SignTime = reader.ReadInt32();
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