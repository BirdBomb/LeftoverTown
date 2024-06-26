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
        new BuildingConfig(){ Building_ID = 1005,Building_Name ="木床组件",Building_FileName ="WoodBed_Part",Building_SpriteName="WoodBed_Part",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
        new BuildingConfig(){ Building_ID = 1006,Building_Name ="路灯",Building_FileName ="Lamp_0",Building_SpriteName="Lamp_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
        new BuildingConfig(){ Building_ID = 1007,Building_Name ="篝火",Building_FileName ="Bonfire_0",Building_SpriteName="Bonfire_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
        new BuildingConfig(){ Building_ID = 1008,Building_Name ="巨大告示牌",Building_FileName ="Billboard",Building_SpriteName="Billboard",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
        new BuildingConfig(){ Building_ID = 1009,Building_Name ="骸骨",Building_FileName ="Bone",Building_SpriteName="Bone",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
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
