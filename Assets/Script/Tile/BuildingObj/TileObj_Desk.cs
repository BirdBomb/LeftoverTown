using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_Desk : TileObj
{
    #region//»æÖÆ
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite Desk_Single;
    [SerializeField]
    private Sprite Desk_Middle;
    [SerializeField]
    private Sprite Desk_Left;
    [SerializeField]
    private Sprite Desk_Right;

    public override void Draw()
    {
        CheckAround(new List<string>() { "Desk", "CashDesk" }, true);
        base.Draw();
    }
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        if (linkState == LinkState.NoneSide || linkState == LinkState.OneSide_Down || linkState == LinkState.OneSide_Up || linkState == LinkState.TwoSide_UpDown)
        {
            spriteRenderer.sprite = Desk_Middle;
        }
        else if (linkState == LinkState.ThreeSide_LeftMissing || linkState == LinkState.OneSide_Right || linkState == LinkState.TwoSide_DownRight || linkState == LinkState.TwoSide_UpRight)
        {
            spriteRenderer.sprite = Desk_Right;
        }
        else if (linkState == LinkState.ThreeSide_RightMissing || linkState == LinkState.OneSide_Left || linkState == LinkState.TwoSide_DownLeft || linkState == LinkState.TwoSide_UpLeft)
        {
            spriteRenderer.sprite = Desk_Left;
        }
        else
        {
            spriteRenderer.sprite = Desk_Single;
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }
    #endregion
}
