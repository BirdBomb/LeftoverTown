using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Vector3 = UnityEngine.Vector3;

public class TileObj_Cabinet : TileObj
{
    private GameObject obj_singal;
    private GameObject obj_cabinet;

    public void Start()
    {
    }
    public override void Invoke()
    {
        if (!obj_cabinet)
        {
            obj_cabinet = UIManager.Instance.ShowUI("UI/UI_Grid_0", new Vector3(0, 2, 0));
            obj_cabinet.transform.localScale = Vector3.zero;
            obj_cabinet.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            obj_cabinet.GetComponent<UI_Grid>().InitGrid(info, this);
            transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        if (obj_singal)
        {
            UIManager.Instance.HideUI("UI/UI_Signal", obj_singal);
            obj_singal = null;
        }

        base.Invoke();
    }
    public override bool PlayerNearby(PlayerController player)
    {
        /*靠近的不是我自己*/
        if (!player.thisPlayerIsMe) { return false; }
        if (!obj_singal)
        {
            obj_singal = UIManager.Instance.ShowUI("UI/UI_Signal", new Vector3(0, 1, 0));
            obj_singal.transform.localScale = Vector3.zero;
            obj_singal.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutBack);
            transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        return true;
    }
    public override bool PlayerFaraway(PlayerController player)
    {
        /*靠近的不是我自己*/
        if (!player.thisPlayerIsMe) { return false; }
        if (obj_singal)
        {
            UIManager.Instance.HideUI("UI/UI_Signal", obj_singal);
            obj_singal = null;
        }
        if (obj_cabinet)
        {
            UIManager.Instance.HideUI("UI/UI_Grid_0", obj_cabinet);
            obj_cabinet = null;
        }
        return true;
    }
}
