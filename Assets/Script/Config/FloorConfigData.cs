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
        new FloorConfig(){ Floor_ID = 1001,Floor_Name ="�ݵ�",
            Floor_SpriteName ="Grass",Floor_FileName ="Grass",Floor_RawLevel = 1,
            Floor_Raw=new List<FloorRaw>(){ new FloorRaw(1001,2) } },
        new FloorConfig(){ Floor_ID = 1002,Floor_Name ="ľ�ʵذ�",
            Floor_SpriteName ="WoodFloor",Floor_FileName ="WoodFloor",Floor_RawLevel = 1,
            Floor_Raw=new List<FloorRaw>(){ new FloorRaw(1001,2),new FloorRaw(2001,1) } },
        new FloorConfig(){ Floor_ID = 1003,Floor_Name ="ʯש·",
            Floor_SpriteName ="RockRoad",Floor_FileName ="RockRoad",Floor_RawLevel = 1,
            Floor_Raw=new List<FloorRaw>(){ new FloorRaw(1001,2),new FloorRaw(2001,1) } },
        new FloorConfig(){ Floor_ID = 1004,Floor_Name ="�ĵ�RedPlushCarpet",
            Floor_SpriteName ="Ground",Floor_FileName ="Ground",Floor_RawLevel = 1,
            Floor_Raw=new List<FloorRaw>(){ new FloorRaw(1001,2),new FloorRaw(2001,1) } },
        new FloorConfig(){ Floor_ID = 1005,Floor_Name ="���޵�̺",
            Floor_SpriteName ="RedPlushCarpet",Floor_FileName ="RedPlushCarpet",Floor_RawLevel = 1,
            Floor_Raw=new List<FloorRaw>(){ new FloorRaw(1001,2),new FloorRaw(2001,1) } },
        new FloorConfig(){ Floor_ID = 1006,Floor_Name ="ˮ��·",
            Floor_SpriteName ="Concrete",Floor_FileName ="Concrete",Floor_RawLevel = 1,
            Floor_Raw=new List<FloorRaw>(){ new FloorRaw(1001,2),new FloorRaw(2001,1) } },
    };

}
[Serializable]
public struct FloorConfig
{
    [SerializeField]/*���*/
    public int Floor_ID;
    [SerializeField]/*����*/
    public string Floor_Name;
    [SerializeField]/*��Դ��*/
    public string Floor_FileName;
    [SerializeField]/*ͼƬ��*/
    public string Floor_SpriteName;
    [SerializeField]/*�ϳɵȼ�*/
    public int Floor_RawLevel;
    /*����ԭ��*/
    public List<FloorRaw> Floor_Raw;
}
/// <summary>
/// �ذ�ԭ��
/// </summary>
public struct FloorRaw
{
    public short ID;
    public short Count;
    public FloorRaw(short id, short count)
    {
        ID = id;
        Count = count;
    }
}