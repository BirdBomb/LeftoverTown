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

    public override void Draw(int seed)
    {
        CheckAroundBuilding_FourSide(new List<string>() { "Desk", "CashDesk" });
        base.Draw(seed);
    }
    public override void LinkAround(AroundState_FourSide aroundState)
    {
        if (aroundState.Left && aroundState.Right)
        {
            spriteRenderer.sprite = Desk_Middle;
        }
        else if (aroundState.Left)
        {
            spriteRenderer.sprite = Desk_Left;
        }
        else if (aroundState.Right)
        {
            spriteRenderer.sprite = Desk_Right;
        }
        else
        {
            spriteRenderer.sprite = Desk_Single;
        }
        base.LinkAround(aroundState);
    }
    #endregion
}
