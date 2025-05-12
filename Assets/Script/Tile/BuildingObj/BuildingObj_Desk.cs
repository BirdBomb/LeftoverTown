using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Desk : BuildingObj
{
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
    public override void All_Draw()
    {
        Around around = MapManager.Instance.CheckBuilding_TwoSide(buildingTile.tileID, buildingTile.tilePos);
        if (around.L && around.R)
        {
            spriteRenderer.sprite = Desk_Middle;
        }
        else if (around.L)
        {
            spriteRenderer.sprite = Desk_Left;
        }
        else if (around.R)
        {
            spriteRenderer.sprite = Desk_Right;
        }
        else
        {
            spriteRenderer.sprite = Desk_Single;
        }
        base.All_Draw();
    }
}
