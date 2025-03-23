using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_WoodWorkingBench : BuildingObj
{
    [SerializeField]
    private GameObject obj_SingalF;
    [SerializeField]
    private GameObject obj_CreateItem;
    [SerializeField]
    private UI_Grid_CreateItem uI_CreateItem;
    #region//瓦片交互
    public override void ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_CreateItem.activeSelf);
            OpenOrCloseCreateUI(!obj_CreateItem.activeSelf);
        }
        base.ActorInputKeycode(actor, code);
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_SingalF.transform.DOKill();
        if (open)
        {
            obj_SingalF.SetActive(true);
            obj_SingalF.transform.localScale = Vector3.one;
            obj_SingalF.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalF.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalF.SetActive(false);
            });
        }
    }
    private void OpenOrCloseCreateUI(bool open)
    {
        if (open)
        {
            obj_CreateItem.transform.localScale = Vector3.one;
            obj_CreateItem.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_CreateItem.SetActive(true);
            uI_CreateItem.Open();
            uI_CreateItem.BindAction_ChangeInfo(ChangeInfo);
            uI_CreateItem.UpdateInfo(info);
        }
        else
        {
            obj_CreateItem.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_CreateItem.SetActive(false);
            });
            uI_CreateItem.Close();
        }
    }
    public override bool PlayerHolding(PlayerCoreLocal player)
    {
        /*靠近是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerRelease(PlayerCoreLocal player)
    {
        /*离开是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCreateUI(false);
            return true;
        }
        return true;
    }
    #endregion
    #region//信息更新与上传
    public override void UpdateInfo(string info)
    {
        uI_CreateItem.UpdateInfo(info);
        base.UpdateInfo(info);
    }
    #endregion
}
