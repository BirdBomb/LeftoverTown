using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_Logger : BuildingObj_Manmade
{
    private ActorManager actor_Bind = new ActorManager();
    public GameObject obj_SingalFUI;
    public GameObject obj_SingalAwakeUI;
    public GameObject obj_HightlightUI;
    [SerializeField]
    private GameObject prefab_UI;
    [Header("奖励刷新周期(小时)")]
    public int int_ResetTime = 9999;
    [Header("基本掉落物")]
    public List<BaseLootInfo> baseLootInfos = new List<BaseLootInfo>();
    [Header("额外掉落物")]
    public List<ExtraLootInfo> extraLootInfos = new List<ExtraLootInfo>();
    public List<ItemData> itemDatas_List = new List<ItemData>();
    private int gameTime_Sign = -10000;
    private int gameTime_Now;
    private SupplyState supplyState = SupplyState.UnInit;
    private TileUI_LoggerBox tileUI_Bind;
    public override void Start()
    {
        CreateActor();
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            All_UpdateTime(_.hour + _.day * 10);
            All_UpdateHour(_.hour);
        }).AddTo(this);
        All_UpdateTime(MapManager.Instance.mapNetManager.Day * 10 + MapManager.Instance.mapNetManager.Hour);
    }
    #region//信息更新与上传
    public override void All_UpdateInfo(string info)
    {
        gameTime_Sign = gameTime_Now;
        if (tileUI_Bind)
        {
            ReadInfo(info);
        }
        base.All_UpdateInfo(info);
    }
    public void ReadInfo(string info)
    {
        itemDatas_List.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDatas_List.Add(data);
            }
        }
        if (tileUI_Bind)
        {
            tileUI_Bind.DrawEveryCell();
        }
    }
    public void WriteInfo()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < itemDatas_List.Count; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemDatas_List[i]));
            }
            else
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemDatas_List[i]));
            }
        }
        Local_ChangeInfo(builder.ToString());
    }
    #endregion
    #region//瓦片交互
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseUI(tileUI_Bind == null);
        }
        base.All_ActorInputKeycode(actor, code);
    }
    public override void All_PlayerHighlight(bool on)
    {
        OpenOrCloseHighlightUI(on);
        base.All_PlayerHighlight(on);
    }
    public override void All_PlayerFaraway()
    {
        OpenOrCloseUI(false);
        base.All_PlayerFaraway();
    }
    public override void OpenOrCloseHighlightUI(bool open)
    {
        obj_SingalFUI.transform.DOKill();
        if (open)
        {
            obj_SingalFUI.SetActive(true);
            obj_SingalFUI.transform.localScale = Vector3.one;
            obj_SingalFUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalFUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalFUI.SetActive(false);
            });
        }
        obj_HightlightUI.transform.DOKill();
        if (open)
        {
            obj_HightlightUI.SetActive(true);
            obj_HightlightUI.transform.localScale = Vector3.one;
            obj_HightlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_HightlightUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_HightlightUI.SetActive(false);
            });
        }
    }
    public override void OpenOrCloseAwakeUI(bool open)
    {
        obj_SingalAwakeUI.transform.DOKill();
        if (open)
        {
            obj_SingalAwakeUI.SetActive(true);
            obj_SingalAwakeUI.transform.localScale = Vector3.one;
            obj_SingalAwakeUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalAwakeUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalAwakeUI.SetActive(false);
            });
        }
    }
    public override void OpenOrCloseUI(bool open)
    {
        if (open)
        {
            UIManager.Instance.ShowTileUI(prefab_UI, out TileUI tileUI);
            tileUI_Bind = tileUI.GetComponent<TileUI_LoggerBox>();
            tileUI_Bind.BindBuilding(this);
            ReadInfo(info);
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
    #region//资源点刷新
    /// <summary>
    /// 更新时间
    /// </summary>
    /// <param name="hour"></param>
    /// <param name="date"></param>
    public void All_UpdateTime(int time)
    {
        gameTime_Now = time;
        All_CompareTime();
    }
    /// <summary>
    /// 对比时间
    /// </summary>
    public void All_CompareTime()
    {
        if (gameTime_Now - gameTime_Sign > int_ResetTime)
        {
            if (supplyState == SupplyState.UnInit)
            {
                All_UpdateSupplyState(SupplyState.Init);
            }
        }
    }
    public void All_UpdateSupplyState(SupplyState state)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.1f);
        supplyState = state;

        switch (state)
        {
            case SupplyState.Init:
                if (info == "")
                {
                    All_InitStuff();
                }
                break;
        }
    }
    /// <summary>
    /// 初始化补给内容
    /// </summary>
    public void All_InitStuff()
    {
        List<ItemData> itemDatas_List = State_GetLootItem(baseLootInfos, extraLootInfos);
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < itemDatas_List.Count; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemDatas_List[i]));
            }
            else
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemDatas_List[i]));
            }
        }
        info = builder.ToString();
        gameTime_Sign = gameTime_Now;
    }

    #endregion
    #region//召唤
    public void All_UpdateHour(int hour)
    {
        if (actor_Bind == null && hour == 0) { CreateActor(); }
    }
    private void CreateActor()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_Logger",
            pos = transform.position,
            callBack = ((actor) =>
            {
                actor_Bind = actor.GetComponent<ActorManager>();
                actor.brainManager.SetHome(buildingTile.tilePos);
            })
        });
    }
    #endregion

}
