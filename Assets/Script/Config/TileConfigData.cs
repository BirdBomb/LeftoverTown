using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConfigData : MonoBehaviour
{
    public static TileConfig GetItemConfig(int ID)
    {
        return tileConfigs.Find((x) => { return x.Tile_ID == ID; });
    }
    public readonly static List<TileConfig> tileConfigs = new List<TileConfig>()
    {
        new TileConfig(){ Tile_ID = 1001,Tile_Name ="简易木墙",Tile_FileName ="WoodWall_0",Tile_RawLevel = 1,Tile_RawList=new List<int>(){ 1001,1001,1001 } },
        new TileConfig(){ Tile_ID = 1002,Tile_Name ="简易柜子",Tile_FileName ="Cabinet_0",Tile_RawLevel = 1,Tile_RawList=new List<int>(){ 1001,1001,1001 } },
    };

}
[Serializable]
public struct TileConfig
{
    [SerializeField]/*编号*/
    public int Tile_ID;
    [SerializeField]/*名字*/
    public string Tile_Name;
    [SerializeField]/*资源名*/
    public string Tile_FileName;
    [SerializeField]/*合成表*/
    public List<int> Tile_RawList;
    [SerializeField]/*合成等级*/
    public int Tile_RawLevel;
}
