using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TileObj_Rock : TileObj
{
    #region//»æÖÆ
    public SpriteRenderer mainRockSprite;
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
        CheckAround(bindTile.name, true); 
        base.Draw();
    }
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        int m = new System.Random().Next(0, 2);
        switch (linkState)
        {
            case LinkState.FourSide:
                mainRockSprite.sprite = FourSide[new System.Random().Next(0, FourSide.Length)];
                break;
            case LinkState.NoneSide:
                mainRockSprite.sprite = NoneSide[new System.Random().Next(0, NoneSide.Length)];
                break;
            case LinkState.OneSide_Down:
                mainRockSprite.sprite = OneSide_Down[new System.Random().Next(0, OneSide_Down.Length)];
                break;
            case LinkState.OneSide_Left:
                mainRockSprite.sprite = OneSide_Left[new System.Random().Next(0, OneSide_Left.Length)];
                break;
            case LinkState.OneSide_Right:
                mainRockSprite.sprite = OneSide_Right[new System.Random().Next(0, OneSide_Right.Length)];
                break;
            case LinkState.OneSide_Up:
                mainRockSprite.sprite = OneSide_Up[new System.Random().Next(0, OneSide_Up.Length)];
                break;
            case LinkState.ThreeSide_DownMissing:
                mainRockSprite.sprite = ThreeSide_DownMissing[new System.Random().Next(0, ThreeSide_DownMissing.Length)];
                break;
            case LinkState.ThreeSide_LeftMissing:
                mainRockSprite.sprite = ThreeSide_LeftMissing[new System.Random().Next(0, ThreeSide_LeftMissing.Length)];
                break;
            case LinkState.ThreeSide_RightMissing:
                mainRockSprite.sprite = ThreeSide_RightMissing[new System.Random().Next(0, ThreeSide_RightMissing.Length)];
                break;
            case LinkState.ThreeSide_UpMissing:
                mainRockSprite.sprite = ThreeSide_UpMissing[new System.Random().Next(0, ThreeSide_UpMissing.Length)];
                break;
            case LinkState.TwoSide_DownLeft:
                mainRockSprite.sprite = TwoSide_DownLeft[new System.Random().Next(0, TwoSide_DownLeft.Length)];
                break;
            case LinkState.TwoSide_DownRight:
                mainRockSprite.sprite = TwoSide_DownRight[new System.Random().Next(0, TwoSide_DownRight.Length)];
                break;
            case LinkState.TwoSide_LeftRight:
                mainRockSprite.sprite = TwoSide_LeftRight[new System.Random().Next(0, TwoSide_LeftRight.Length)];
                break;
            case LinkState.TwoSide_UpDown:
                mainRockSprite.sprite = TwoSide_UpDown[new System.Random().Next(0, TwoSide_UpDown.Length)];
                break;
            case LinkState.TwoSide_UpLeft:
                mainRockSprite.sprite = TwoSide_UpLeft[new System.Random().Next(0, TwoSide_UpLeft.Length)];
                break;
            case LinkState.TwoSide_UpRight:
                mainRockSprite.sprite = TwoSide_UpRight[new System.Random().Next(0, TwoSide_UpRight.Length)];
                break;
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }
    #endregion
    #region//Á£×Ó
    public ParticleSystem bitsParticle;
    private void PlayBitsParticle()
    {
        bitsParticle.Play();
    }
    #endregion
    #region//ÍßÆ¬¶¯»­
    public override void PlayDamaged()
    {
        PlayBitsParticle();
        AudioManager.Instance.PlayEffect(1004, transform.position);
        base.PlayDamaged();
    }
    public override void PlayBroken()
    {
        AudioManager.Instance.PlayEffect(1005, transform.position);
        base.PlayBroken();
    }
    #endregion
}
