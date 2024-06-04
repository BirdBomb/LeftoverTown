using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TileObj_Wall : TileObj
{
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
