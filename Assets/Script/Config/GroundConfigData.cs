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
        /*�ĵ�*/new GroundConfig(){ Ground_ID = 1000,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(118,94,66,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*�ݵ�*/new GroundConfig(){ Ground_ID = 1001,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(27,88,33,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*ǳ��*/new GroundConfig(){ Ground_ID = 1002,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(27,88,33,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*̦ԭ*/new GroundConfig(){ Ground_ID = 1003,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(27,88,33,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*ѩԭ*/new GroundConfig(){ Ground_ID = 1004,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(27,88,33,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*ɳĮ*/new GroundConfig(){ Ground_ID = 1005,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(27,88,33,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*ľ�Ƶذ�*/new GroundConfig(){ Ground_ID = 2000,Ground_Age = AgeGroup.StoneAge,Ground_Color = new Color32(129,91,53,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*ʯש�ذ�*/new GroundConfig(){ Ground_ID = 2001,Ground_Age = AgeGroup.StoneAge,Ground_Color = new Color32(93,102,116,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*�������*/new GroundConfig(){ Ground_ID = 2002,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(57,37,31,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*ˮ*/new GroundConfig(){ Ground_ID = 9000,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(24,50,80,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
        /*��*/new GroundConfig(){ Ground_ID = 9999,Ground_Age = AgeGroup.Nature,Ground_Color = new Color32(23,12,11,255),
            Ground_Raw=new List<ItemRaw>(){ new ItemRaw(1001,2) } },
    };

}
[Serializable]
public struct GroundConfig
{
    public short Ground_ID;
    public AgeGroup Ground_Age;
    public Color32 Ground_Color;
    public List<ItemRaw> Ground_Raw;
}
