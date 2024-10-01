using DG.Tweening;
using UnityEngine;
using UniRx;

public class TileObj_Billboard : TileObj
{
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    #region//Íæ¼Ò½»»¥
    public override void Invoke(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowTextUI()
            {
                text = info
            });
            OpenOrCloseSingal(false);
        }
        base.Invoke(player, code);
    }
    public override bool PlayerHolding(PlayerController player)
    {
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerRelease(PlayerController player)
    {
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(false);
            return true;
        }
        return false;
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
    #endregion
}
