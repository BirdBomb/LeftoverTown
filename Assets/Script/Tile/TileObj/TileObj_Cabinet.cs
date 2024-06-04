using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TileObj_Cabinet : TileObj
{
    [SerializeField,Header("Singal")]
    private GameObject obj_singal;
    [SerializeField, Header("Cabinet")]
    private GameObject obj_cabinet;

    public override void Invoke()
    {
        obj_cabinet.SetActive(true);
        obj_cabinet.GetComponent<UI_Grid_Cabinet>().Open(this);
        obj_cabinet.GetComponent<UI_Grid_Cabinet>().UpdateInfoFromTile(info);
        obj_singal.SetActive(false);

        base.Invoke();
    }
    public override void TryToUpdateInfo(string info)
    {
        obj_cabinet.GetComponent<UI_Grid_Cabinet>().UpdateInfoFromTile(info);
        base.TryToUpdateInfo(info);
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
        obj_cabinet.SetActive(false);
        obj_cabinet.GetComponent<UI_Grid_Cabinet>().Close(this);
        return true;
    }

    public override void TryToUpdateHp(int newHp)
    {
        if (newHp <= CurHp)
        {
            PlayDamagedAnim();
        }
        base.TryToUpdateHp(newHp);
    }

    public override void ListenDestroyMyObj()
    {
        PlayBreakAnim();
        Invoke("DestroyMyObj", 0.3f);
    }
    public override void PlayDamagedAnim()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
        base.PlayDamagedAnim();
    }
    public override void PlayBreakAnim()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            transform.DOScaleX(0, 0.05f);
        });
        base.PlayBreakAnim();
    }

}
