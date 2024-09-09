using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_Base : TileObj
{
    [SerializeField, Header("Singal")]
    private GameObject obj_singal;

    public void Start()
    {
    }
    public override void Invoke(PlayerController player, KeyCode code)
    {
        base.Invoke(player, code);
    }
    public override void TryToUpdateInfo(string info)
    {
        base.TryToUpdateInfo(info);
    }
    public override bool PlayerNearby(PlayerController player)
    {
        /*靠近是我自己*/
        if (player.thisPlayerIsMe)
        {
            obj_singal.SetActive(true);
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
        return true;
    }

}
