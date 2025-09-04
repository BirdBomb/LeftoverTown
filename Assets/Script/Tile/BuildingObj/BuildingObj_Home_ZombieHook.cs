using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class BuildingObj_Home_ZombieHook : BuildingObj_Manmade
{
    public GameObject obj_SingalFUI;
    public GameObject obj_SingalAwakeUI;
    public GameObject obj_HightlightUI;
    [SerializeField]
    private GameObject prefab_UI;
    [Header("献祭物ID")]
    public int itemData_ID;
    [HideInInspector]
    public int gameTime_UseableTime;
    [HideInInspector]
    public int gameTime_CurTimeSign;

    private TileUI_Home_ZombieHook tileUI_Bind;
    private bool bool_OpenUI = false;
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            All_UpdateTime(_.hour + _.day * 10);
        }).AddTo(this);
        All_UpdateTime(MapManager.Instance.mapNetManager.Day * 10 + MapManager.Instance.mapNetManager.Hour);
        base.Start();
    }
    #region//信息更新与上传
    public override void All_UpdateInfo(string info)
    {
        if (tileUI_Bind)
        {
            ReadInfo(info);
        }
        base.All_UpdateInfo(info);
    }
    public void ReadInfo(string info)
    {
        if (info != null && info != "")
        {
            gameTime_UseableTime = int.Parse(info);
            if (tileUI_Bind) { tileUI_Bind.DrawInfo(); }
        }
    }
    public void WriteInfo()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append((gameTime_CurTimeSign + 10).ToString());    
        Local_ChangeInfo(builder.ToString());
    }
    #endregion
    #region//瓦皮交互
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
            tileUI_Bind = tileUI.GetComponent<TileUI_Home_ZombieHook>();
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
    public void All_UpdateTime(int time)
    {
        gameTime_CurTimeSign = time;
        if (tileUI_Bind) { tileUI_Bind.DrawInfo(); }
    }
}
