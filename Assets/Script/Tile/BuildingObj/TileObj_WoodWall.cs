using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class TileObj_WoodWall : TileObj
{
    #region//»æÖÆ
    public SpriteRenderer sprite_UpLeft;
    public SpriteRenderer sprite_UpRight;
    public SpriteRenderer sprite_DownLeft;
    public SpriteRenderer sprite_DownRight;

    /// <summary>
    /// OneSide_LeftDown
    /// </summary>
    public Sprite sprite_0;
    /// <summary>
    /// TwoSide_RightUp/Down
    /// </summary>
    public Sprite sprite_1;
    /// <summary>
    /// ThreeSide_WithoutRightUp
    /// </summary>
    public Sprite sprite_2;
    /// <summary>
    /// TwoSide_DownLeft/Right
    /// </summary>
    public Sprite sprite_3;
    /// <summary>
    /// TwoSide_LeftUp/RightDown
    /// </summary>
    public Sprite sprite_4;
    /// <summary>
    /// ThreeSide_WithoutLeftUp
    /// </summary>
    public Sprite sprite_5;
    /// <summary>
    /// FourSide
    /// </summary>
    public Sprite sprite_6;
    /// <summary>
    /// ThreeSide_WithoutRightDown
    /// </summary>
    public Sprite sprite_7;
    /// <summary>
    /// OneSide_LeftUp
    /// </summary>
    public Sprite sprite_8;
    /// <summary>
    /// TwoSide_UpLeft/Right
    /// </summary>
    public Sprite sprite_9;
    /// <summary>
    /// ThreeSide_WithoutLeftDown
    /// </summary>
    public Sprite sprite_10;
    /// <summary>
    /// TwoSide_LeftUp/Down
    /// </summary>
    public Sprite sprite_11;
    /// <summary>
    /// ZeroSide
    /// </summary>
    public Sprite sprite_12;
    /// <summary>
    /// OneSide_RightDown
    /// </summary>
    public Sprite sprite_13;
    /// <summary>
    /// TwoSide_LeftDown/RightUp
    /// </summary>
    public Sprite sprite_14;
    /// <summary>
    /// OneSide_LeftUp
    /// </summary>
    public Sprite sprite_15;
    public override void Draw(int seed)
    {
        CheckAroundBuilding_EightSide(bindTile.name);
        base.Draw(seed);
    }
    public override void LinkAround(AroundState_EightSide aroundState)
    {
        DrawUpLeft(aroundState);
        if (!aroundState.Up && !aroundState.Right)
        {
            sprite_UpRight.gameObject.SetActive(true);
            DrawUpRight(aroundState);
        }
        else
        {
            sprite_UpRight.gameObject.SetActive(false);
        }
        if (!aroundState.Down)
        {
            sprite_DownLeft.gameObject.SetActive(true);
            DrawDownLeft(aroundState);
        }
        else
        {
            sprite_DownLeft.gameObject.SetActive(false);
        }
        if (!aroundState.Right)
        {
            sprite_DownRight.gameObject.SetActive(true);
            DrawDownRight(aroundState);
        }
        else
        {
            sprite_DownRight.gameObject.SetActive(false);
        }
        base.LinkAround(aroundState);
    }
    private void DrawUpLeft(AroundState_EightSide aroundState)
    {
        if (aroundState.Up)
        {
            if (aroundState.Left)
            {
                if (aroundState.UpLeft)
                {
                    sprite_UpLeft.sprite = sprite_6;
                }
                else
                {
                    sprite_UpLeft.sprite = sprite_5;
                }
            }
            else
            {
                if (aroundState.UpLeft)
                {
                    sprite_UpLeft.sprite = sprite_10;
                }
                else
                {
                    sprite_UpLeft.sprite = sprite_1;
                }
            }
        }
        else
        {
            if (aroundState.Left)
            {
                if (aroundState.UpLeft)
                {
                    sprite_UpLeft.sprite = sprite_2;
                }
                else
                {
                    sprite_UpLeft.sprite = sprite_3;
                }
            }
            else
            {
                if (aroundState.UpLeft)
                {
                    sprite_UpLeft.sprite = sprite_4;
                }
                else
                {
                    sprite_UpLeft.sprite = sprite_13;
                }
            }

        }
    }
    private void DrawUpRight(AroundState_EightSide aroundState)
    {
        if (aroundState.Up)
        {
            if (aroundState.Right)
            {
                if (aroundState.UpRight)
                {
                    sprite_UpRight.sprite = sprite_6;
                }
                else
                {
                    sprite_UpRight.sprite = sprite_2;
                }
            }
            else
            {
                if (aroundState.UpRight)
                {
                    sprite_UpRight.sprite = sprite_7;
                }
                else
                {
                    sprite_UpRight.sprite = sprite_11;
                }
            }
        }
        else
        {
            if (aroundState.Right)
            {
                if (aroundState.UpRight)
                {
                    sprite_UpRight.sprite = sprite_5;
                }
                else
                {
                    sprite_UpRight.sprite = sprite_3;
                }
            }
            else
            {
                if (aroundState.UpRight)
                {
                    sprite_UpRight.sprite = sprite_14;
                }
                else
                {
                    sprite_UpRight.sprite = sprite_0;
                }
            }

        }

    }
    private void DrawDownLeft(AroundState_EightSide aroundState)
    {
        if (aroundState.Down)
        {
            if (aroundState.Left)
            {
                if (aroundState.DownLeft)
                {
                    sprite_DownLeft.sprite = sprite_6;
                }
                else
                {
                    sprite_DownLeft.sprite = sprite_10;
                }
            }
            else
            {
                if (aroundState.DownLeft)
                {
                    sprite_DownLeft.sprite = sprite_5;
                }
                else
                {
                    sprite_DownLeft.sprite = sprite_1;
                }
            }
        }
        else
        {
            if (aroundState.Left)
            {
                if (aroundState.DownLeft)
                {
                    sprite_DownLeft.sprite = sprite_7;
                }
                else
                {
                    sprite_DownLeft.sprite = sprite_9;
                }
            }
            else
            {
                if (aroundState.DownLeft)
                {
                    sprite_DownLeft.sprite = sprite_14;
                }
                else
                {
                    sprite_DownLeft.sprite = sprite_8;
                }
            }
        }

    }
    private void DrawDownRight(AroundState_EightSide aroundState)
    {
        if (aroundState.Down)
        {
            if (aroundState.Right)
            {
                if (aroundState.DownRight)
                {
                    sprite_DownRight.sprite = sprite_6;
                }
                else
                {
                    sprite_DownRight.sprite = sprite_7;
                }
            }
            else
            {
                if (aroundState.DownRight)
                {
                    sprite_DownRight.sprite = sprite_2;
                }
                else
                {
                    sprite_DownRight.sprite = sprite_11;
                }
            }
        }
        else
        {
            if (aroundState.Right)
            {
                if (aroundState.DownRight)
                {
                    sprite_DownRight.sprite = sprite_10;
                }
                else
                {
                    sprite_DownRight.sprite = sprite_9;
                }
            }
            else
            {
                if (aroundState.DownRight)
                {
                    sprite_DownRight.sprite = sprite_4;
                }
                else
                {
                    sprite_DownRight.sprite = sprite_15;
                }
            }
        }
    }
    #endregion
}
