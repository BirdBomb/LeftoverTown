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

    public override void Draw()
    {
        //CheckAround(new List<string>() { "WoodShore", "Rock" }, true);
        CheckAround("WoodShore", true);
        base.Draw();
    }
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        if (linkState == LinkState.NoneSide || linkState == LinkState.OneSide_Down || linkState == LinkState.OneSide_Up || linkState == LinkState.TwoSide_UpDown)
        {
            spriteRenderer.sprite = WoodShore_Middle;
        }
        else if (linkState == LinkState.ThreeSide_LeftMissing || linkState == LinkState.OneSide_Right || linkState == LinkState.TwoSide_DownRight || linkState == LinkState.TwoSide_UpRight)
        {
            spriteRenderer.sprite = WoodShore_Right;
        }
        else if (linkState == LinkState.ThreeSide_RightMissing || linkState == LinkState.OneSide_Left || linkState == LinkState.TwoSide_DownLeft || linkState == LinkState.TwoSide_UpLeft)
        {
            spriteRenderer.sprite = WoodShore_Left;
        }
        else
        {
            spriteRenderer.sprite = WoodShore_Single;
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }

    #endregion
}
