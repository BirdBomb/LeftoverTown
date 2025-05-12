using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class BuildingObj_ZombieHole : BuildingObj
{
    [SerializeField]
    private GameObject obj_Singal;
    [SerializeField]
    private GameObject prefab_UI;
    [Header("献祭物ID")]
    public int itemData_ID;
    [HideInInspector]
    public int gameTime_UseableTime;
    [HideInInspector]
    public int gameTime_CurTimeSign;

    private TileUI_ZombieHole tileUI_Bind;
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
    public void All_UpdateTime(int time)
    {
        gameTime_CurTimeSign = time;
        if (tileUI_Bind) { tileUI_Bind.DrawInfo(); }
    }
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(bool_OpenUI);
            OpenOrCloseCabinetUI(!bool_OpenUI);
        }
        base.All_ActorInputKeycode(actor, code);
    }
    public override bool All_PlayerHolding(PlayerCoreLocal player)
    {
        /*靠近是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool All_PlayerRelease(PlayerCoreLocal player)
    {
        /*离开是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCabinetUI(false);
            return true;
        }
        return false;
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_Singal.transform.DOKill();
        if (open)
        {
            obj_Singal.SetActive(true);
            obj_Singal.transform.localScale = Vector3.one;
            obj_Singal.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_Singal.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_Singal.SetActive(false);
            });
        }
    }
    private void OpenOrCloseCabinetUI(bool open)
    {
        if (open)
        {
            bool_OpenUI = true;
            UIManager.Instance.ShowTileUI(prefab_UI, out TileUI tileUI);
            tileUI_Bind = tileUI.GetComponent<TileUI_ZombieHole>();
            tileUI_Bind.BindBuilding(this);
            ReadInfo(info);
        }
        else
        {
            bool_OpenUI = false;
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
            if (tileUI_Bind) tileUI_Bind = null;
        }
    }

}
