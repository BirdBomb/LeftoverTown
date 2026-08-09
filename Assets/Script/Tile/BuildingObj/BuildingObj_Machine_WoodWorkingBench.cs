using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Machine_WoodWorkingBench : BuildingObj_Manmade
{
    public GameObject prefab_UI;
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_SingalUI_Awake;
    protected GameObject obj_HighlightUI;
    private TileUI tileUI_Bind = null;
    #region//ÍßÆ¬½»»¥
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
            UIManager.Instance.ShowTileUI(prefab_UI, out tileUI_Bind);
            CreateListConfig createListConfig = CreateListConfigData.GetCreateListConfig(3003);
            List<CreateRawConfig> createRawConfigs = new List<CreateRawConfig>();
            for (int i = 0; i < createListConfig.List.Count; i++)
            {
                createRawConfigs.Add(CreateRawConfigData.GetCreateRawConfig(createListConfig.List[i]));
            }
            tileUI_Bind.GetComponent<TileUI_CreateItem>().InitPool(createRawConfigs, createListConfig.Name);
            tileUI_Bind.GetComponent<TileUI_CreateItem>().BindBuilding(this);
        }
        else
        {
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
        }
    }
    public override bool CanHighlight()
    {
        return true;
    }

    #endregion
}
