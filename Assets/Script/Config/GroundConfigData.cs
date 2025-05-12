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
        new GroundConfig(){ Ground_ID = 1000,Ground_Name ="荒地",Ground_Age = AgeGroup.Nature,
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        new GroundConfig(){ Ground_ID = 1001,Ground_Name ="草地",Ground_Age = AgeGroup.Nature,
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        new GroundConfig(){ Ground_ID = 2000,Ground_Name ="木制地板",Ground_Age = AgeGroup.StoneAge,
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        new GroundConfig(){ Ground_ID = 2001,Ground_Name ="石砖地板",Ground_Age = AgeGroup.StoneAge,
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },

        new GroundConfig(){ Ground_ID = 9000,Ground_Name ="水",Ground_Age = AgeGroup.Nature,
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        new GroundConfig(){ Ground_ID = 9999,Ground_Name ="坑",Ground_Age = AgeGroup.Nature,
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
    };

}
[Serializable]
public struct GroundConfig
{
    [SerializeField]
    public short Ground_ID;
    [SerializeField]
    public string Ground_Name;
    public List<ItemRaw> Ground_Raw;
    public AgeGroup Ground_Age;
}
