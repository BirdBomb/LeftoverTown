using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class TileObj_Billboard : TileObj
{
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    public override void Invoke(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowTextUI()
            {
                text = info
            });
        }
        obj_singalF.SetActive(false);
        base.Invoke(player, code);
    }
    public override bool PlayerHolding(PlayerController player)
    {
        /*靠近是我自己*/
        if (player.thisPlayerIsMe)
        {
            obj_singalF.SetActive(true);
            obj_singalF.transform.DOKill();
            obj_singalF.transform.localScale = Vector3.one;
            obj_singalF.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            return true;
        }
        return false;
    }
    public override bool PlayerRelease(PlayerController player)
    {
        /*离开是我自己*/
        if (player.thisPlayerIsMe) 
        {
            obj_singalF.transform.DOKill();
            obj_singalF.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                obj_singalF.SetActive(false);
            });
            return true; 
        }
        return true;
    }

}
