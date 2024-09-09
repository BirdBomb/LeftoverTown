using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class TileObj_Wall : TileObj
{
    [SerializeField]
    private GameObject NoneSide;
    [SerializeField]
    private GameObject FourSide;
    [SerializeField]
    private GameObject OneSide_Up;
    [SerializeField]
    private GameObject OneSide_Down;
    [SerializeField]
    private GameObject OneSide_Left;
    [SerializeField]
    private GameObject OneSide_Right;
    [SerializeField]
    private GameObject TwoSide_UpDown;
    [SerializeField]
    private GameObject TwoSide_LeftRight;
    [SerializeField]
    private GameObject TwoSide_UpLeft;
    [SerializeField]
    private GameObject TwoSide_UpRight;
    [SerializeField]
    private GameObject TwoSide_DownLeft;
    [SerializeField]
    private GameObject TwoSide_DownRight;
    [SerializeField]
    private GameObject ThreeSide_LeftMissing;
    [SerializeField]
    private GameObject ThreeSide_RightMissing;
    [SerializeField]
    private GameObject ThreeSide_UpMissing;
    [SerializeField]
    private GameObject ThreeSide_DownMissing;
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
        NoneSide.SetActive(false);
        FourSide.SetActive(false);
        OneSide_Up.SetActive(false);
        OneSide_Down.SetActive(false);
        OneSide_Left.SetActive(false);
        OneSide_Right.SetActive(false);
        TwoSide_UpDown.SetActive(false);
        TwoSide_LeftRight.SetActive(false);
        TwoSide_DownLeft.SetActive(false);
        TwoSide_DownRight.SetActive(false);
        TwoSide_UpLeft.SetActive(false);
        TwoSide_UpRight.SetActive(false);
        ThreeSide_LeftMissing.SetActive(false);
        ThreeSide_RightMissing.SetActive(false);
        ThreeSide_UpMissing.SetActive(false);
        ThreeSide_DownMissing.SetActive(false);
        switch (linkState)
        {
            case LinkState.FourSide:
                FourSide.SetActive(true);
                break;
            case LinkState.NoneSide:
                NoneSide.SetActive(true);
                break;
            case LinkState.OneSide_Up:
                OneSide_Up.SetActive(true);
                break;
            case LinkState.OneSide_Down:
                OneSide_Down.SetActive(true);
                break;
            case LinkState.OneSide_Left:
                OneSide_Left.SetActive(true);
                break;
            case LinkState.OneSide_Right:
                OneSide_Right.SetActive(true);
                break;
            case LinkState.TwoSide_UpDown:
                TwoSide_UpDown.SetActive(true);
                break;
            case LinkState.TwoSide_LeftRight:
                TwoSide_LeftRight.SetActive(true);
                break;
            case LinkState.TwoSide_DownLeft:
                TwoSide_DownLeft.SetActive(true);
                break;
            case LinkState.TwoSide_DownRight:
                TwoSide_DownRight.SetActive(true);
                break;
            case LinkState.TwoSide_UpRight:
                TwoSide_UpRight.SetActive(true);
                break;
            case LinkState.TwoSide_UpLeft:
                TwoSide_UpLeft.SetActive(true);
                break;
            case LinkState.ThreeSide_UpMissing:
                ThreeSide_UpMissing.SetActive(true);
                break;
            case LinkState.ThreeSide_DownMissing:
                ThreeSide_DownMissing.SetActive(true);
                break;
            case LinkState.ThreeSide_LeftMissing:
                ThreeSide_LeftMissing.SetActive(true);
                break;
            case LinkState.ThreeSide_RightMissing:
                ThreeSide_RightMissing.SetActive(true);
                break;
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }
}
