using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConfigData : MonoBehaviour
{
    public static BuildingConfig GetItemConfig(int ID)
    {
        return buildConfigs.Find((x) => { return x.Building_ID == ID; });
    }
    public readonly static List<BuildingConfig> buildConfigs = new List<BuildingConfig>()
    {
        new BuildingConfig(){ Building_ID = 1001,Building_Name ="简易木墙",Building_FileName ="WoodWall_0",Building_SpriteName="WoodWall_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001,1001 } },
        new BuildingConfig(){ Building_ID = 1002,Building_Name ="简易柜子",Building_FileName ="Cabinet_0",Building_SpriteName="Cabinet_0_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001,1001,1001,3001,3001 } },
        new BuildingConfig(){ Building_ID = 1003,Building_Name ="简易木窗",Building_FileName ="WoodWindow_0",Building_SpriteName="WoodWindow_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
        new BuildingConfig(){ Building_ID = 1004,Building_Name ="简易木门",Building_FileName ="WoodDoor_0",Building_SpriteName="WoodDoor_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*编号*/
    public int Building_ID;
    [SerializeField]/*名字*/
    public string Building_Name;
    [SerializeField]/*资源名*/
    public string Building_FileName;
    [SerializeField]/*图片名*/
    public string Building_SpriteName;
    [SerializeField]/*合成表*/
    public List<int> Building_RawList;
    [SerializeField]/*合成等级*/
    public int Building_RawLevel;
}
