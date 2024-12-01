using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_WoodShore : TileObj
{
    #region//»æÖÆ
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite WoodShore_Single;
    [SerializeField]
    private Sprite WoodShore_Middle;
    [SerializeField]
    private Sprite WoodShore_Left;
    [SerializeField]
    private Sprite WoodShore_Right;

    public override void Draw(int seed)
    {
        CheckAroundBuilding_FourSide("WoodShore");
        base.Draw(seed);
    }
    public override void LinkAround(AroundState_FourSide aroundState)
    {
        if (aroundState.Left && aroundState.Right)
        {
            spriteRenderer.sprite = WoodShore_Middle;
        }
        else if (aroundState.Left)
        {
            spriteRenderer.sprite = WoodShore_Left;
        }
        else if (aroundState.Right)
        {
            spriteRenderer.sprite = WoodShore_Right;
        }
        else
        {
            spriteRenderer.sprite = WoodShore_Single;
        }
        base.LinkAround(aroundState);
    }
    #endregion
}
