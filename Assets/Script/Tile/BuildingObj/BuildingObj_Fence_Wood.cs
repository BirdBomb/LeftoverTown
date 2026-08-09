using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Fence_Wood : BuildingObj_Manmade
{
    [SerializeField]
    private GameObject obj_LinkRight;
    [SerializeField]
    private GameObject obj_LinkLeft;
    [SerializeField]
    private GameObject obj_LinkDown;
    public override void All_OnDraw()
    {
        Around around = MapManager.Instance.CheckAround_Building(buildingTile.tilePos, CheckLink, DirectionType.Four);
        obj_LinkRight.SetActive(around.R);
        obj_LinkLeft.SetActive(around.L);
        obj_LinkDown.SetActive(around.D);
        base.All_OnDraw();
    }
    private bool CheckLink(int id)
    {
        BuildingConfig buildingConfig = BuildingConfigData.GetBuildingConfig(id);
        if(buildingConfig.Building_Type == BuildingType.Structure)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
