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
        new TileConfig(){ Tile_ID = 1001,Tile_Name ="����ľǽ",Tile_FileName ="WoodWall_0",Tile_RawLevel = 1,Tile_RawList=new List<int>(){ 1001,1001,1001 } },
        new TileConfig(){ Tile_ID = 1002,Tile_Name ="���׹���",Tile_FileName ="Cabinet_0",Tile_RawLevel = 1,Tile_RawList=new List<int>(){ 1001,1001,1001 } },
    };

}
[Serializable]
public struct TileConfig
{
    [SerializeField]/*���*/
    public int Tile_ID;
    [SerializeField]/*����*/
    public string Tile_Name;
    [SerializeField]/*��Դ��*/
    public string Tile_FileName;
    [SerializeField]/*�ϳɱ�*/
    public List<int> Tile_RawList;
    [SerializeField]/*�ϳɵȼ�*/
    public int Tile_RawLevel;
}
