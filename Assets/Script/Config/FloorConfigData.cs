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
        new FloorConfig(){ Floor_ID = 1001,Floor_Name ="�ݵ�",Floor_SpriteName ="Grass",Floor_FileName ="Grass",Floor_RawLevel = 1,Floor_RawList=new List<int>(){ 1001 } },
        new FloorConfig(){ Floor_ID = 1002,Floor_Name ="ľ�ʵذ�",Floor_SpriteName ="WoodFloor",Floor_FileName ="WoodFloor",Floor_RawLevel = 1,Floor_RawList=new List<int>(){ 1001 } },
    };

}
[Serializable]
public struct FloorConfig
{
    [SerializeField]/*���*/
    public int Floor_ID;
    [SerializeField]/*����*/
    public string Floor_Name;
    [SerializeField]/*ͼƬ��*/
    public string Floor_SpriteName;
    [SerializeField]/*��Դ��*/
    public string Floor_FileName;
    [SerializeField]/*�ϳɱ�*/
    public List<int> Floor_RawList;
    [SerializeField]/*�ϳɵȼ�*/
    public int Floor_RawLevel;
}
