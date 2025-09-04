using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Counter : BuildingObj_Manmade
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite sprite_L;
    [SerializeField]
    private Sprite sprite_M;
    [SerializeField]
    private Sprite sprite_R;
    [SerializeField]
    private Sprite sprite_S;
    public override void All_Draw()
    {
        Around around = MapManager.Instance.CheckBuilding_FourSide(buildingTile.tileID, buildingTile.tilePos);
        if (around.R && around.L)
        {
            spriteRenderer.sprite = sprite_M;
        }
        else if (around.R)
        {
            spriteRenderer.sprite = sprite_L;
        }
        else if (around.L)
        {
            spriteRenderer.sprite = sprite_R;
        }
        else
        {
            spriteRenderer.sprite = sprite_S;
        }
        base.All_Draw();
    }

}
