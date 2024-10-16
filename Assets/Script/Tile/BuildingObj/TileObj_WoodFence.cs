using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_WoodFence : TileObj
{
    #region//»æÖÆ
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    public Sprite FourSide;
    [SerializeField]
    public Sprite NoneSide;
    [SerializeField]
    public Sprite OneSide_Down;
    [SerializeField]
    public Sprite OneSide_Left;
    [SerializeField]
    public Sprite OneSide_Right;
    [SerializeField]
    public Sprite OneSide_Up;
    [SerializeField]
    public Sprite ThreeSide_DownMissing;
    [SerializeField]
    public Sprite ThreeSide_LeftMissing;
    [SerializeField]
    public Sprite ThreeSide_RightMissing;
    [SerializeField]
    public Sprite ThreeSide_UpMissing;
    [SerializeField]
    public Sprite TwoSide_DownLeft;
    [SerializeField]
    public Sprite TwoSide_DownRight;
    [SerializeField]
    public Sprite TwoSide_LeftRight;
    [SerializeField]
    public Sprite TwoSide_UpDown;
    [SerializeField]
    public Sprite TwoSide_UpLeft;
    [SerializeField]
    public Sprite TwoSide_UpRight;
    public override void Draw()
    {
        CheckAround(bindTile.name, true);
        base.Draw();
    }
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        switch (linkState)
        {
            case LinkState.FourSide:
                spriteRenderer.sprite = FourSide;
                break;
            case LinkState.NoneSide:
                spriteRenderer.sprite = NoneSide;
                break;
            case LinkState.OneSide_Up:
                spriteRenderer.sprite = OneSide_Up;
                break;
            case LinkState.OneSide_Down:
                spriteRenderer.sprite = OneSide_Down;
                break;
            case LinkState.OneSide_Left:
                spriteRenderer.sprite = OneSide_Left;
                break;
            case LinkState.OneSide_Right:
                spriteRenderer.sprite = OneSide_Right;
                break;
            case LinkState.TwoSide_UpDown:
                spriteRenderer.sprite = TwoSide_UpDown;
                break;
            case LinkState.TwoSide_LeftRight:
                spriteRenderer.sprite = TwoSide_LeftRight;
                break;
            case LinkState.TwoSide_DownLeft:
                spriteRenderer.sprite = TwoSide_DownLeft;
                break;
            case LinkState.TwoSide_DownRight:
                spriteRenderer.sprite = TwoSide_DownRight;
                break;
            case LinkState.TwoSide_UpRight:
                spriteRenderer.sprite = TwoSide_UpRight;
                break;
            case LinkState.TwoSide_UpLeft:
                spriteRenderer.sprite = TwoSide_UpLeft;
                break;
            case LinkState.ThreeSide_UpMissing:
                spriteRenderer.sprite = ThreeSide_UpMissing;
                break;
            case LinkState.ThreeSide_DownMissing:
                spriteRenderer.sprite = ThreeSide_DownMissing;
                break;
            case LinkState.ThreeSide_LeftMissing:
                spriteRenderer.sprite = ThreeSide_LeftMissing;
                break;
            case LinkState.ThreeSide_RightMissing:
                spriteRenderer.sprite = ThreeSide_RightMissing;
                break;
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }

    #endregion
}
