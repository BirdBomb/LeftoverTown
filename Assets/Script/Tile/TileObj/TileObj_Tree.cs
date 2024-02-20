using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileObj_Tree : TileObj
{
    public override void Damaged(int val)
    {
        if (CurHp > 0)
        {
            CurHp -= val;
            if (CurHp <= 0)
            {
                PlayBreakAnim();
                Invoke("Break", 0.3f);
            }
            else
            {
                PlayDamagedAnim();
            }
        }
        base.Damaged(val);
    }
    public override void Break()
    {
        Loot();
        Remove();
        base.Break();
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
