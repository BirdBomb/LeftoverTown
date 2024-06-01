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
        new BuildingConfig(){ Building_ID = 1001,Building_Name ="����ľǽ",Building_FileName ="WoodWall_0",Building_SpriteName="WoodWall_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001,1001 } },
        new BuildingConfig(){ Building_ID = 1002,Building_Name ="���׹���",Building_FileName ="Cabinet_0",Building_SpriteName="Cabinet_0_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001,1001,1001,3001,3001 } },
        new BuildingConfig(){ Building_ID = 1003,Building_Name ="����ľ��",Building_FileName ="WoodWindow_0",Building_SpriteName="WoodWindow_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
        new BuildingConfig(){ Building_ID = 1004,Building_Name ="����ľ��",Building_FileName ="WoodDoor_0",Building_SpriteName="WoodDoor_0",Building_RawLevel = 1,Building_RawList=new List<int>(){ 1001 } },
    };
}
[Serializable]
public struct BuildingConfig
{
    [SerializeField]/*���*/
    public int Building_ID;
    [SerializeField]/*����*/
    public string Building_Name;
    [SerializeField]/*��Դ��*/
    public string Building_FileName;
    [SerializeField]/*ͼƬ��*/
    public string Building_SpriteName;
    [SerializeField]/*�ϳɱ�*/
    public List<int> Building_RawList;
    [SerializeField]/*�ϳɵȼ�*/
    public int Building_RawLevel;
}
