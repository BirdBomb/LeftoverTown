using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class TileObj_Billboard : TileObj
{
    [SerializeField, Header("Singal")]
    private GameObject obj_singal;
    public override void Invoke(PlayerController player)
    {
        Debug.Log(bindTile._posInWorld);
        MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowTextUI()
        {
            text = info
        });
        obj_singal.SetActive(false);

        base.Invoke(player);
    }

    public override bool PlayerNearby(PlayerController player)
    {
        /*靠近是我自己*/
        if (player.thisPlayerIsMe)
        {
            Debug.Log("Open");
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
        return true;
    }

}
