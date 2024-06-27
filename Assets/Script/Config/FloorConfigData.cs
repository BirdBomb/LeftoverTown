using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorConfigData : MonoBehaviour
{
    public static FloorConfig GetItemConfig(int ID)
    {
        return floorConfigs.Find((x) => { return x.Floor_ID == ID; });
    }
    public readonly static List<FloorConfig> floorConfigs = new List<FloorConfig>()
    {
        new FloorConfig(){ Floor_ID = 1001,Floor_Name ="草地",Floor_SpriteName ="Grass",Floor_FileName ="Grass",Floor_RawLevel = 1,Floor_RawList=new List<int>(){ 1001 } },
        new FloorConfig(){ Floor_ID = 1002,Floor_Name ="木质地板",Floor_SpriteName ="WoodFloor",Floor_FileName ="WoodFloor",Floor_RawLevel = 1,Floor_RawList=new List<int>(){ 1001 } },
    };

}
[Serializable]
public struct FloorConfig
{
    [SerializeField]/*编号*/
    public int Floor_ID;
    [SerializeField]/*名字*/
    public string Floor_Name;
    [SerializeField]/*图片名*/
    public string Floor_SpriteName;
    [SerializeField]/*资源名*/
    public string Floor_FileName;
    [SerializeField]/*合成表*/
    public List<int> Floor_RawList;
    [SerializeField]/*合成等级*/
    public int Floor_RawLevel;
}
