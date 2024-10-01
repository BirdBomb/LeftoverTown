using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileObj_Door : TileObj
{
    [SerializeField, Header("UnlockUI")]
    private GameObject ui_Unlock;
    [SerializeField, Header("UnlockText")]
    private Text text_Unlock;
    [SerializeField, Header("LockUI")]
    private GameObject ui_Lock;
    [SerializeField, Header("Bar")]
    private Image ui_Bar;
    [SerializeField, Header("Open")]
    private GameObject obj_door_Open;
    [SerializeField, Header("Close")]
    private GameObject obj_door_Close;
    [SerializeField, Header("Lock")]
    private GameObject obj_door_Lock;
    [SerializeField, Header("开锁几率"), Range(0, 100)]
    private int lockPro;
    private bool _open = false;
    private bool _lock = false;

    #region//玩家交互
    public override void Invoke(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            if (_lock)
            {
                UnlockDoor(player);
            }
            else
            {
                if (player.thisPlayerIsMe && player.actorManager.NetManager.Data_ItemInHand.Item_ID == 9005)
                {
                    LockDoor(player.actorManager.NetManager.Data_ItemInHand.Item_Seed.ToString());
                    OpenOrCloseLockUI(false);
                    OpenOrCloseUnLockUI(false);
                }
            }
        }
        base.Invoke(player, code);
    }
    public override bool PlayerNearby(PlayerController player)
    {
        _lock = IsDoorLock();
        if (_lock)
        {
            UpdateUnLockUI(player);
            OpenOrCloseUnLockUI(true);
        }
        else
        {
            if (player.thisPlayerIsMe && player.actorManager.NetManager.Data_ItemInHand.Item_ID == 9005)
            {
                OpenOrCloseLockUI(true);
            }
            if (!_open)
            {
                ChangeDoorState(DoorState.Open);
            }

        }
        return true;
    }
    public override bool PlayerFaraway(PlayerController player)
    {
        _lock = IsDoorLock();
        OpenOrCloseLockUI(false);
        OpenOrCloseUnLockUI(false);
        if (_open)
        {
            ChangeDoorState(DoorState.Close);
        }
        return true;
    }
    private void OpenOrCloseLockUI(bool open)
    {
        ui_Lock.transform.DOKill();
        if (open)
        {
            ui_Lock.SetActive(true);
            ui_Lock.transform.DOKill();
            ui_Lock.transform.localScale = Vector3.one;
            ui_Lock.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            ui_Lock.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                ui_Lock.SetActive(false);
            });
        }
    }
    private void OpenOrCloseUnLockUI(bool open)
    {
        ui_Unlock.transform.DOKill();
        if (open)
        {
            ui_Unlock.SetActive(true);
            ui_Unlock.transform.DOKill();
            ui_Unlock.transform.localScale = Vector3.one;
            ui_Unlock.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            ui_Bar.DOKill();
            ui_Bar.transform.localScale = new Vector3(0, 1, 1);
        }
        else
        {
            ui_Unlock.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                ui_Unlock.SetActive(false);
            });
        }
    }
    private void UpdateUnLockUI(PlayerController player)
    {
        CalculateUnLockProAndTime(player, out int pro, out float time);
        text_Unlock.text = pro + "%";
    }
    #endregion
    #region//门
    private bool IsDoorLock()
    {
        if (info == "")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void ChangeDoorState(DoorState doorState)
    {
        obj_door_Open.SetActive(false);
        obj_door_Close.SetActive(false);
        obj_door_Lock.SetActive(false);
        if (doorState == DoorState.Open)
        {
            _open = true;
            obj_door_Open.transform.DOKill();
            obj_door_Open.SetActive(true);
            obj_door_Open.transform.localScale = Vector3.one;
            obj_door_Open.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        if (doorState == DoorState.Close)
        {
            _open = false;
            _lock = IsDoorLock();
            if (_lock)
            {
                obj_door_Lock.transform.DOKill();
                obj_door_Lock.SetActive(true);
                obj_door_Lock.transform.localScale = Vector3.one;
                obj_door_Lock.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
            else
            {
                obj_door_Close.transform.DOKill();
                obj_door_Close.SetActive(true);
                obj_door_Close.transform.localScale = Vector3.one;
                obj_door_Close.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
        }
    }
    private void UnlockDoor(PlayerController player)
    {
        UpdateUnLockUI(player);
        CalculateUnLockProAndTime(player, out int pro, out float time);
        bool succes = false;
        UnityEngine.Random.InitState(System.DateTime.Now.Second);
        if(UnityEngine.Random.Range(0, 100) < pro)
        {
            succes = true;
        }
        ui_Bar.DOKill();
        ui_Bar.transform.localScale = new Vector3(0, 1, 1);
        ui_Bar.transform.DOScaleX(1, time).OnComplete(() =>
        {
            if (succes)
            {
                LockDoor("");
                OpenOrCloseLockUI(true);
                OpenOrCloseUnLockUI(false);
            }
            else
            {
                OpenOrCloseLockUI(false);
                OpenOrCloseUnLockUI(true);
            }
        });

    }
    private void LockDoor(string password)
    {
        TryToChangeInfo(password);
    }
    private void CalculateUnLockProAndTime(PlayerController player,out int pro,out float time)
    {
        if (player.actorManager.NetManager.Data_ItemInHand.Item_ID == 9005 && player.actorManager.NetManager.Data_ItemInHand.Item_Seed.ToString() == info)
        {
            pro = 100;
            time = 0.2f;
        }
        else
        {
            pro = lockPro;
            if (pro > 70) { time = 4; }
            else if (pro > 40) { time = 4.5f; }
            else
            {
                time = 5f;
            }
        }
    }

    #endregion
    #region//信息更新与上传
    public override void TryToUpdateInfo(string info)
    {
        base.TryToUpdateInfo(info);
        if (info == "")
        {
            ChangeDoorState(DoorState.Open);
        }
        else
        {
            ChangeDoorState(DoorState.Close);
        }
    }
    #endregion
}
public enum DoorState
{
    Open,
    Close,
}

