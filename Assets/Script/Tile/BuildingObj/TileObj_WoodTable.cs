using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_WoodTable : TileObj
{
    [SerializeField]
    private GameObject Single;
    [SerializeField]
    private GameObject Left;
    [SerializeField]
    private GameObject Right;
    [SerializeField]
    private GameObject Middle;
    private void Start()
    {
        CheckAround(bindTile.name, true);
    }
    public override void TryToUpdateHp(int newHp)
    {
        if (newHp <= CurHp)
        {
            PlayDamagedAnim();
        }
        base.TryToUpdateHp(newHp);
    }
    public override void TryToDestroyMyObj()
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
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        Single.SetActive(false);
        Left.SetActive(false);
        Right.SetActive(false);
        Middle.SetActive(false);
        if (linkToLeft && linkToRight)
        {
            Middle.SetActive(true);
        }
        else if (linkToLeft != null)
        {
            Right.SetActive(true);
        }
        else if (linkToRight != null)
        {
            Left.SetActive(true);
        }
        else
        {
            Single.SetActive(true);
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }
}
