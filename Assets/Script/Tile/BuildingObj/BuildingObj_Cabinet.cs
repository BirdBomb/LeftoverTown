using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Cabinet : BuildingObj
{
    [SerializeField]
    private GameObject obj_Singal;
    [SerializeField]
    private GameObject obj_Cabinet;
    [SerializeField]
    private UI_Grid_Cabinet uI_Grid_Cabinet;
    public override void ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_Cabinet.activeSelf);
            OpenOrCloseCabinetUI(!obj_Cabinet.activeSelf);
        }
        base.ActorInputKeycode(actor, code);
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
            obj_Cabinet.transform.localScale = Vector3.one;
            obj_Cabinet.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_Cabinet.SetActive(true);
            uI_Grid_Cabinet.Open();
            uI_Grid_Cabinet.BindAction_ChangeInfo(ChangeInfo);
            uI_Grid_Cabinet.UpdateInfo(info);
        }
        else
        {
            obj_Cabinet.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_Cabinet.SetActive(false);
            });
            uI_Grid_Cabinet.Close();
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
            OpenOrCloseCabinetUI(false);
            return true;
        }
        return false;
    }

}
