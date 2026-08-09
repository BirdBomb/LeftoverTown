using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UniRx;
using UnityEngine;
using WebSocketSharp;

public class BuildingObj_Home_ZombieHook : BuildingObj_Manmade
{
    public GameObject prefab_UI;
    [Header("献祭物ID")]
    public int itemData_ID;
    [Header("献祭周期")]
    public int int_UseableWait;

    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;
    public BuildingData_Home_ZombieHook buildingData_Home_ZombieHook = new BuildingData_Home_ZombieHook();
    private TileUI_Home_ZombieHook tileUI_Bind;
    public override void Start()
    {
        SubscribeToEvents();
        base.Start();
    }
    #region 初始化
    public virtual void SubscribeToEvents()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>()
            .Subscribe(All_OnHourUpdate)
            .AddTo(this);
    }
    #endregion
    #region 信息更新与上传
    public override void All_OnRawDataUpdate()
    {
        buildingData_Home_ZombieHook.Deserialize(local_ByteData);
        tileUI_Bind?.DrawInfo();
        base.All_OnRawDataUpdate();
    }
    public void TryToPush()
    {
        All_PushData(buildingData_Home_ZombieHook.Serialize());
    }
    #endregion
    #region//瓦皮交互
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
            tileUI_Bind = tileUI.GetComponent<TileUI_Home_ZombieHook>();
            tileUI_Bind.BindBuilding(this);
            tileUI_Bind.DrawInfo();
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
    public void All_OnHourUpdate(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        tileUI_Bind?.DrawInfo();
    }
}
public class BuildingData_Home_ZombieHook
{
    public int gameTime_UseableTime = int.MinValue;
    public int ReadUseableTime() { return gameTime_UseableTime; }
    public void WriteUseableTime(int val) { gameTime_UseableTime = val; }
    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(gameTime_UseableTime);
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
            gameTime_UseableTime = reader.ReadInt32();
        }
    }
}