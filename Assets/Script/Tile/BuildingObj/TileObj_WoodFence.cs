using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj_WoodFence : TileObj
{
    #region//»æÖÆ
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject obj_LinkRight;
    [SerializeField]
    private GameObject obj_LinkLeft;
    [SerializeField]
    private GameObject obj_LinkDown;
    public override void Draw(int seed)
    {
        CheckAroundBuilding_FourSide(bindTile.name);
        base.Draw(seed);
    }
    public override void LinkAround(AroundState_FourSide aroundState)
    {
        obj_LinkRight.SetActive(false);
        obj_LinkLeft.SetActive(false);
        obj_LinkDown.SetActive(false);
        if (aroundState.Right)
        {
            obj_LinkRight.SetActive(true);
        }
        if (aroundState.Left)
        {
            obj_LinkLeft.SetActive(true);
        }
        if (aroundState.Down)
        {
            obj_LinkDown.SetActive(true);
        }
        base.LinkAround(aroundState);
    }
    #endregion
}
