using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundConfigData 
{
    public static GroundConfig GetFloorConfig(int ID)
    {
        return groundConfigs.Find((x) => { return x.Ground_ID == ID; });
    }
    public readonly static List<GroundConfig> groundConfigs = new List<GroundConfig>()
    {
        new GroundConfig(){ Ground_ID = 1000,Ground_Name ="�ĵ�",
            Ground_Raw=new List<RawItem>(){ new RawItem(1001,2) } },
        new GroundConfig(){ Ground_ID = 1001,Ground_Name ="�ݵ�",
            Ground_Raw=new List<RawItem>(){ new RawItem(1001,2) } },
        new GroundConfig(){ Ground_ID = 2000,Ground_Name ="ľ�Ƶذ�",
            Ground_Raw=new List<RawItem>(){ new RawItem(1001,2) } },

        new GroundConfig(){ Ground_ID = 9000,Ground_Name ="ˮ",
            Ground_Raw=new List<RawItem>(){ new RawItem(1001,2) } },
        new GroundConfig(){ Ground_ID = 9999,Ground_Name ="��",
            Ground_Raw=new List<RawItem>(){ new RawItem(1001,2) } },
    };

}
[Serializable]
public struct GroundConfig
{
    [SerializeField]/*���*/
    public short Ground_ID;
    [SerializeField]/*����*/
    public string Ground_Name;
    /*����ԭ��*/
    public List<RawItem> Ground_Raw;
}
