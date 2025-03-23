using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_Wall_Brick : MonoBehaviour
{
    ////#region//绘制
    ////public SpriteRenderer WallSprite;
    ////public Sprite[] FourSide;
    ////public Sprite[] NoneSide;
    ////public Sprite[] OneSide_Down;
    ////public Sprite[] OneSide_Left;
    ////public Sprite[] OneSide_Right;
    ////public Sprite[] OneSide_Up;
    ////public Sprite[] ThreeSide_DownMissing;
    ////public Sprite[] ThreeSide_LeftMissing;
    ////public Sprite[] ThreeSide_RightMissing;
    ////public Sprite[] ThreeSide_UpMissing;
    ////public Sprite[] TwoSide_DownLeft;
    ////public Sprite[] TwoSide_DownRight;
    ////public Sprite[] TwoSide_LeftRight;
    ////public Sprite[] TwoSide_UpDown;
    ////public Sprite[] TwoSide_UpLeft;
    ////public Sprite[] TwoSide_UpRight;
    ////public override void Draw(int seed)
    ////{
    ////    CheckAroundBuilding_FourSide(new List<string>() { "BrickWall", "BrickWindow" });
    ////    base.Draw(seed);
    ////}
    ////public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    ////{
    ////    int m = new System.Random().Next(0, 2);
    ////    switch (linkState)
    ////    {
    ////        case LinkState.FourSide:
    ////            WallSprite.sprite = FourSide[new System.Random().Next(0, FourSide.Length)];
    ////            break;
    ////        case LinkState.NoneSide:
    ////            WallSprite.sprite = NoneSide[new System.Random().Next(0, NoneSide.Length)];
    ////            break;
    ////        case LinkState.OneSide_Down:
    ////            WallSprite.sprite = OneSide_Down[new System.Random().Next(0, OneSide_Down.Length)];
    ////            break;
    ////        case LinkState.OneSide_Left:
    ////            WallSprite.sprite = OneSide_Left[new System.Random().Next(0, OneSide_Left.Length)];
    ////            break;
    ////        case LinkState.OneSide_Right:
    ////            WallSprite.sprite = OneSide_Right[new System.Random().Next(0, OneSide_Right.Length)];
    ////            break;
    ////        case LinkState.OneSide_Up:
    ////            WallSprite.sprite = OneSide_Up[new System.Random().Next(0, OneSide_Up.Length)];
    ////            break;
    ////        case LinkState.ThreeSide_DownMissing:
    ////            WallSprite.sprite = ThreeSide_DownMissing[new System.Random().Next(0, ThreeSide_DownMissing.Length)];
    ////            break;
    ////        case LinkState.ThreeSide_LeftMissing:
    ////            WallSprite.sprite = ThreeSide_LeftMissing[new System.Random().Next(0, ThreeSide_LeftMissing.Length)];
    ////            break;
    ////        case LinkState.ThreeSide_RightMissing:
    ////            WallSprite.sprite = ThreeSide_RightMissing[new System.Random().Next(0, ThreeSide_RightMissing.Length)];
    ////            break;
    ////        case LinkState.ThreeSide_UpMissing:
    ////            WallSprite.sprite = ThreeSide_UpMissing[new System.Random().Next(0, ThreeSide_UpMissing.Length)];
    ////            break;
    ////        case LinkState.TwoSide_DownLeft:
    ////            WallSprite.sprite = TwoSide_DownLeft[new System.Random().Next(0, TwoSide_DownLeft.Length)];
    ////            break;
    ////        case LinkState.TwoSide_DownRight:
    ////            WallSprite.sprite = TwoSide_DownRight[new System.Random().Next(0, TwoSide_DownRight.Length)];
    ////            break;
    ////        case LinkState.TwoSide_LeftRight:
    ////            WallSprite.sprite = TwoSide_LeftRight[new System.Random().Next(0, TwoSide_LeftRight.Length)];
    ////            break;
    ////        case LinkState.TwoSide_UpDown:
    ////            WallSprite.sprite = TwoSide_UpDown[new System.Random().Next(0, TwoSide_UpDown.Length)];
    ////            break;
    ////        case LinkState.TwoSide_UpLeft:
    ////            WallSprite.sprite = TwoSide_UpLeft[new System.Random().Next(0, TwoSide_UpLeft.Length)];
    ////            break;
    ////        case LinkState.TwoSide_UpRight:
    ////            WallSprite.sprite = TwoSide_UpRight[new System.Random().Next(0, TwoSide_UpRight.Length)];
    ////            break;
    ////    }
    ////    base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    ////}
    ////#endregion
    //#region//绘制
    //public SpriteRenderer sprite_UpLeft;
    //public SpriteRenderer sprite_UpRight;
    //public SpriteRenderer sprite_DownLeft;
    //public SpriteRenderer sprite_DownRight;

    ///// <summary>
    ///// OneSide_LeftDown
    ///// </summary>
    //public Sprite sprite_0;
    ///// <summary>
    ///// TwoSide_RightUp/Down
    ///// </summary>
    //public Sprite sprite_1;
    ///// <summary>
    ///// ThreeSide_WithoutRightUp
    ///// </summary>
    //public Sprite sprite_2;
    ///// <summary>
    ///// TwoSide_DownLeft/Right
    ///// </summary>
    //public Sprite sprite_3;
    ///// <summary>
    ///// TwoSide_LeftUp/RightDown
    ///// </summary>
    //public Sprite sprite_4;
    ///// <summary>
    ///// ThreeSide_WithoutLeftUp
    ///// </summary>
    //public Sprite sprite_5;
    ///// <summary>
    ///// FourSide
    ///// </summary>
    //public Sprite sprite_6;
    ///// <summary>
    ///// ThreeSide_WithoutRightDown
    ///// </summary>
    //public Sprite sprite_7;
    ///// <summary>
    ///// OneSide_LeftUp
    ///// </summary>
    //public Sprite sprite_8;
    ///// <summary>
    ///// TwoSide_UpLeft/Right
    ///// </summary>
    //public Sprite sprite_9;
    ///// <summary>
    ///// ThreeSide_WithoutLeftDown
    ///// </summary>
    //public Sprite sprite_10;
    ///// <summary>
    ///// TwoSide_LeftUp/Down
    ///// </summary>
    //public Sprite sprite_11;
    ///// <summary>
    ///// ZeroSide
    ///// </summary>
    //public Sprite sprite_12;
    ///// <summary>
    ///// OneSide_RightDown
    ///// </summary>
    //public Sprite sprite_13;
    ///// <summary>
    ///// TwoSide_LeftDown/RightUp
    ///// </summary>
    //public Sprite sprite_14;
    ///// <summary>
    ///// OneSide_LeftUp
    ///// </summary>
    //public Sprite sprite_15;
    //public override void Draw(int seed)
    //{
    //    CheckAroundBuilding_EightSide(bindTile.name);
    //    base.Draw(seed);
    //}
    //public override void LinkAround(AroundState_EightSide aroundState)
    //{
    //    DrawUpLeft(aroundState);
    //    if (!aroundState.Up && !aroundState.Right)
    //    {
    //        sprite_UpRight.gameObject.SetActive(true);
    //        DrawUpRight(aroundState);
    //    }
    //    else
    //    {
    //        sprite_UpRight.gameObject.SetActive(false);
    //    }
    //    if (!aroundState.Down)
    //    {
    //        sprite_DownLeft.gameObject.SetActive(true);
    //        DrawDownLeft(aroundState);
    //    }
    //    else
    //    {
    //        sprite_DownLeft.gameObject.SetActive(false);
    //    }
    //    if (!aroundState.Right)
    //    {
    //        sprite_DownRight.gameObject.SetActive(true);
    //        DrawDownRight(aroundState);
    //    }
    //    else
    //    {
    //        sprite_DownRight.gameObject.SetActive(false);
    //    }
    //    base.LinkAround(aroundState);
    //}
    //private void DrawUpLeft(AroundState_EightSide aroundState)
    //{
    //    if (aroundState.Up)
    //    {
    //        if (aroundState.Left)
    //        {
    //            if (aroundState.UpLeft)
    //            {
    //                sprite_UpLeft.sprite = sprite_6;
    //            }
    //            else
    //            {
    //                sprite_UpLeft.sprite = sprite_5;
    //            }
    //        }
    //        else
    //        {
    //            if (aroundState.UpLeft)
    //            {
    //                sprite_UpLeft.sprite = sprite_10;
    //            }
    //            else
    //            {
    //                sprite_UpLeft.sprite = sprite_1;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (aroundState.Left)
    //        {
    //            if (aroundState.UpLeft)
    //            {
    //                sprite_UpLeft.sprite = sprite_2;
    //            }
    //            else
    //            {
    //                sprite_UpLeft.sprite = sprite_3;
    //            }
    //        }
    //        else
    //        {
    //            if (aroundState.UpLeft)
    //            {
    //                sprite_UpLeft.sprite = sprite_4;
    //            }
    //            else
    //            {
    //                sprite_UpLeft.sprite = sprite_13;
    //            }
    //        }

    //    }
    //}
    //private void DrawUpRight(AroundState_EightSide aroundState)
    //{
    //    if (aroundState.Up)
    //    {
    //        if (aroundState.Right)
    //        {
    //            if (aroundState.UpRight)
    //            {
    //                sprite_UpRight.sprite = sprite_6;
    //            }
    //            else
    //            {
    //                sprite_UpRight.sprite = sprite_2;
    //            }
    //        }
    //        else
    //        {
    //            if (aroundState.UpRight)
    //            {
    //                sprite_UpRight.sprite = sprite_7;
    //            }
    //            else
    //            {
    //                sprite_UpRight.sprite = sprite_11;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (aroundState.Right)
    //        {
    //            if (aroundState.UpRight)
    //            {
    //                sprite_UpRight.sprite = sprite_5;
    //            }
    //            else
    //            {
    //                sprite_UpRight.sprite = sprite_3;
    //            }
    //        }
    //        else
    //        {
    //            if (aroundState.UpRight)
    //            {
    //                sprite_UpRight.sprite = sprite_14;
    //            }
    //            else
    //            {
    //                sprite_UpRight.sprite = sprite_0;
    //            }
    //        }

    //    }

    //}
    //private void DrawDownLeft(AroundState_EightSide aroundState)
    //{
    //    if (aroundState.Down)
    //    {
    //        if (aroundState.Left)
    //        {
    //            if (aroundState.DownLeft)
    //            {
    //                sprite_DownLeft.sprite = sprite_6;
    //            }
    //            else
    //            {
    //                sprite_DownLeft.sprite = sprite_10;
    //            }
    //        }
    //        else
    //        {
    //            if (aroundState.DownLeft)
    //            {
    //                sprite_DownLeft.sprite = sprite_5;
    //            }
    //            else
    //            {
    //                sprite_DownLeft.sprite = sprite_1;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (aroundState.Left)
    //        {
    //            if (aroundState.DownLeft)
    //            {
    //                sprite_DownLeft.sprite = sprite_7;
    //            }
    //            else
    //            {
    //                sprite_DownLeft.sprite = sprite_9;
    //            }
    //        }
    //        else
    //        {
    //            if (aroundState.DownLeft)
    //            {
    //                sprite_DownLeft.sprite = sprite_14;
    //            }
    //            else
    //            {
    //                sprite_DownLeft.sprite = sprite_8;
    //            }
    //        }
    //    }

    //}
    //private void DrawDownRight(AroundState_EightSide aroundState)
    //{
    //    if (aroundState.Down)
    //    {
    //        if (aroundState.Right)
    //        {
    //            if (aroundState.DownRight)
    //            {
    //                sprite_DownRight.sprite = sprite_6;
    //            }
    //            else
    //            {
    //                sprite_DownRight.sprite = sprite_7;
    //            }
    //        }
    //        else
    //        {
    //            if (aroundState.DownRight)
    //            {
    //                sprite_DownRight.sprite = sprite_2;
    //            }
    //            else
    //            {
    //                sprite_DownRight.sprite = sprite_11;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (aroundState.Right)
    //        {
    //            if (aroundState.DownRight)
    //            {
    //                sprite_DownRight.sprite = sprite_10;
    //            }
    //            else
    //            {
    //                sprite_DownRight.sprite = sprite_9;
    //            }
    //        }
    //        else
    //        {
    //            if (aroundState.DownRight)
    //            {
    //                sprite_DownRight.sprite = sprite_4;
    //            }
    //            else
    //            {
    //                sprite_DownRight.sprite = sprite_15;
    //            }
    //        }
    //    }
    //}
    //#endregion
    //#region//粒子
    //public ParticleSystem bitsParticle;
    //private void PlayBitsParticle()
    //{
    //    bitsParticle.Play();
    //}
    //#endregion
    //#region//瓦片动画
    //public override void PlayDamaged()
    //{
    //    PlayBitsParticle();
    //    AudioManager.Instance.PlayEffect(1004, transform.position);
    //    base.PlayDamaged();
    //}
    //public override void PlayBroken()
    //{
    //    AudioManager.Instance.PlayEffect(1005, transform.position);
    //    base.PlayBroken();
    //}
    //#endregion

}
