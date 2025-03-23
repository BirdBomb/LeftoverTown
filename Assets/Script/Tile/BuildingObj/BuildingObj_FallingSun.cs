using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_FallingSun : BuildingObj
{
    [SerializeField]
    private GameObject obj_SingalF;
    [SerializeField]
    private GameObject obj_FallingSunRock;
    [SerializeField]
    private UI_Grid_FallingSunRock uI_Grid_FallingSunRock;

    public override void ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_FallingSunRock.activeSelf);
            OpenOrCloseCabinetUI(!obj_FallingSunRock.activeSelf);
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
    private void OpenOrCloseCabinetUI(bool open)
    {
        if (open)
        {
            obj_FallingSunRock.transform.localScale = Vector3.one;
            obj_FallingSunRock.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_FallingSunRock.SetActive(true);
            uI_Grid_FallingSunRock.Open();
            uI_Grid_FallingSunRock.BindAction_ChangeInfo(ChangeInfo);
            uI_Grid_FallingSunRock.UpdateInfo(info);
        }
        else
        {
            obj_FallingSunRock.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_FallingSunRock.SetActive(false);
            });
            uI_Grid_FallingSunRock.Close();
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
    public override void UpdateInfo(string info)
    {
        uI_Grid_FallingSunRock.UpdateInfo(info);
        base.UpdateInfo(info);
    }
}
