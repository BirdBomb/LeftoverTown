using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_WoodWorkingBench : TileObj
{
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    [SerializeField, Header("Build")]
    private GameObject obj_build;
    [SerializeField, Header("建造UI")]
    private UI_CreateItem uI_CreateItem;
    #region//玩家交互
    public override void Invoke(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_build.activeSelf);
            OpenOrCloseCreateUI(!obj_build.activeSelf);
        }
        base.Invoke(player, code);
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_singalF.transform.DOKill();
        if (open)
        {
            obj_singalF.SetActive(true);
            obj_singalF.transform.localScale = Vector3.one;
            obj_singalF.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_singalF.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_singalF.SetActive(false);
            });
        }
    }
    private void OpenOrCloseCreateUI(bool open)
    {
        if (open)
        {
            obj_build.transform.localScale = Vector3.one;
            obj_build.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_build.SetActive(true);
            uI_CreateItem.Open(this);
            uI_CreateItem.UpdateInfoFromTile(info);
        }
        else
        {
            obj_build.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_build.SetActive(false);
            });
            uI_CreateItem.Close(this);
        }
    }
    public override bool PlayerHolding(PlayerController player)
    {
        /*靠近是我自己*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerRelease(PlayerController player)
    {
        /*离开是我自己*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCreateUI(false);
            return true;
        }
        return true;
    }
    #endregion
    #region//更新与上传
    public override void TryToUpdateInfo(string info)
    {
        uI_CreateItem.UpdateInfoFromTile(info);
        base.TryToUpdateInfo(info);
    }
    #endregion

}
