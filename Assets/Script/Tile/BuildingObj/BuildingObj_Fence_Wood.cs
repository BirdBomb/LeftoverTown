using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Fence_Wood : BuildingObj
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject obj_LinkRight;
    [SerializeField]
    private GameObject obj_LinkLeft;
    [SerializeField]
    private GameObject obj_LinkDown;
    public override void Draw()
    {
        Around around = MapManager.Instance.CheckBuilding_FourSide(buildingTile.tileID,buildingTile.tilePos);
        obj_LinkRight.SetActive(around.R);
        obj_LinkLeft.SetActive(around.L);
        obj_LinkDown.SetActive(around.D);
        base.Draw();
    }

}
