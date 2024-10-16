using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class TileObj_WoodWall : TileObj
{
    #region//»æÖÆ
    public SpriteRenderer WallSprite;
    public Sprite[] FourSide;
    public Sprite[] NoneSide;
    public Sprite[] OneSide_Down;
    public Sprite[] OneSide_Left;
    public Sprite[] OneSide_Right;
    public Sprite[] OneSide_Up;
    public Sprite[] ThreeSide_DownMissing;
    public Sprite[] ThreeSide_LeftMissing;
    public Sprite[] ThreeSide_RightMissing;
    public Sprite[] ThreeSide_UpMissing;
    public Sprite[] TwoSide_DownLeft;
    public Sprite[] TwoSide_DownRight;
    public Sprite[] TwoSide_LeftRight;
    public Sprite[] TwoSide_UpDown;
    public Sprite[] TwoSide_UpLeft;
    public Sprite[] TwoSide_UpRight;
    public override void Draw()
    {
        CheckAround(new List<string>() { "WoodWall", "WoodWindow" }, true);
        base.Draw();
    }
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        int m = new System.Random().Next(0, 2);
        switch (linkState)
        {
            case LinkState.FourSide:
                WallSprite.sprite = FourSide[new System.Random().Next(0, FourSide.Length)];
                break;
            case LinkState.NoneSide:
                WallSprite.sprite = NoneSide[new System.Random().Next(0, NoneSide.Length)];
                break;
            case LinkState.OneSide_Down:
                WallSprite.sprite = OneSide_Down[new System.Random().Next(0, OneSide_Down.Length)];
                break;
            case LinkState.OneSide_Left:
                WallSprite.sprite = OneSide_Left[new System.Random().Next(0, OneSide_Left.Length)];
                break;
            case LinkState.OneSide_Right:
                WallSprite.sprite = OneSide_Right[new System.Random().Next(0, OneSide_Right.Length)];
                break;
            case LinkState.OneSide_Up:
                WallSprite.sprite = OneSide_Up[new System.Random().Next(0, OneSide_Up.Length)];
                break;
            case LinkState.ThreeSide_DownMissing:
                WallSprite.sprite = ThreeSide_DownMissing[new System.Random().Next(0, ThreeSide_DownMissing.Length)];
                break;
            case LinkState.ThreeSide_LeftMissing:
                WallSprite.sprite = ThreeSide_LeftMissing[new System.Random().Next(0, ThreeSide_LeftMissing.Length)];
                break;
            case LinkState.ThreeSide_RightMissing:
                WallSprite.sprite = ThreeSide_RightMissing[new System.Random().Next(0, ThreeSide_RightMissing.Length)];
                break;
            case LinkState.ThreeSide_UpMissing:
                WallSprite.sprite = ThreeSide_UpMissing[new System.Random().Next(0, ThreeSide_UpMissing.Length)];
                break;
            case LinkState.TwoSide_DownLeft:
                WallSprite.sprite = TwoSide_DownLeft[new System.Random().Next(0, TwoSide_DownLeft.Length)];
                break;
            case LinkState.TwoSide_DownRight:
                WallSprite.sprite = TwoSide_DownRight[new System.Random().Next(0, TwoSide_DownRight.Length)];
                break;
            case LinkState.TwoSide_LeftRight:
                WallSprite.sprite = TwoSide_LeftRight[new System.Random().Next(0, TwoSide_LeftRight.Length)];
                break;
            case LinkState.TwoSide_UpDown:
                WallSprite.sprite = TwoSide_UpDown[new System.Random().Next(0, TwoSide_UpDown.Length)];
                break;
            case LinkState.TwoSide_UpLeft:
                WallSprite.sprite = TwoSide_UpLeft[new System.Random().Next(0, TwoSide_UpLeft.Length)];
                break;
            case LinkState.TwoSide_UpRight:
                WallSprite.sprite = TwoSide_UpRight[new System.Random().Next(0, TwoSide_UpRight.Length)];
                break;
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }
    #endregion
}
