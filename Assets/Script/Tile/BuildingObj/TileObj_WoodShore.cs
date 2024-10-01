using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_WoodShore : TileObj
{
    [SerializeField]
    private GameObject Single;
    [SerializeField]
    private GameObject Left;
    [SerializeField]
    private GameObject Right;
    [SerializeField]
    private GameObject Middle;
    #region//»æÖÆ
    public override void Draw()
    {
        CheckAround(bindTile.name, true);
        base.Draw();
    }
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        Single.SetActive(false);
        Left.SetActive(false);
        Right.SetActive(false);
        Middle.SetActive(false);
        if (linkState == LinkState.NoneSide || linkState == LinkState.OneSide_Down || linkState == LinkState.OneSide_Up || linkState == LinkState.TwoSide_UpDown)
        {
            Middle.SetActive(true);
        }
        else if (linkState == LinkState.ThreeSide_LeftMissing)
        {
            Right.SetActive(true);
        }
        else if (linkState == LinkState.ThreeSide_RightMissing)
        {
            Left.SetActive(true);
        }
        else
        {
            Single.SetActive(true);
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }

    #endregion
}
