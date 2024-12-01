using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_WoodTable : TileObj
{
    #region//»æÖÆ
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite Table_Single;
    [SerializeField]
    private Sprite Table_Middle;
    [SerializeField]
    private Sprite Table_Left;
    [SerializeField]
    private Sprite Table_Right;

    public override void Draw(int seed)
    {
        CheckAroundBuilding_FourSide("WoodTable");
        base.Draw(seed);
    }
    public override void LinkAround(AroundState_FourSide aroundState)
    {
        if (aroundState.Left && aroundState.Right)
        {
            spriteRenderer.sprite = Table_Middle;
        }
        else if (aroundState.Left)
        {
            spriteRenderer.sprite = Table_Left;
        }
        else if (aroundState.Right)
        {
            spriteRenderer.sprite = Table_Right;
        }
        else
        {
            spriteRenderer.sprite = Table_Single;
        }
        base.LinkAround(aroundState);
    }
    #endregion
}
