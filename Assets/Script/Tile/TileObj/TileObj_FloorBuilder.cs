using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_FloorBuilder : TileObj
{
    [SerializeField, Header("Singal")]
    private GameObject obj_singal;
    [SerializeField, Header("Build")]
    private GameObject obj_build;

    public override void Invoke()
    {
        obj_build.SetActive(true);
        obj_build.GetComponent<UI_FloorBuilder>().Open(this);
        obj_build.GetComponent<UI_FloorBuilder>().UpdateInfoFromTile(info);
        obj_singal.SetActive(false);

        base.Invoke();
    }
    public override void TryToUpdateInfo(string info)
    {
        obj_build.GetComponent<UI_FloorBuilder>().UpdateInfoFromTile(info);
        base.TryToUpdateInfo(info);
    }

    public override bool PlayerNearby(PlayerController player)
    {
        /*靠近是我自己*/
        if (player.thisPlayerIsMe)
        {
            obj_singal.SetActive(true);
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

            return true;
        }
        return false;
    }
    public override bool PlayerFaraway(PlayerController player)
    {
        /*靠近的不是我自己*/
        if (!player.thisPlayerIsMe) { return false; }
        obj_singal.SetActive(false);
        obj_build.SetActive(false);
        return true;
    }
}
