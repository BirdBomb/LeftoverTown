using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateConfigData 
{
    public static CreateConfig GetItemConfig(int ID)
    {
        return createConfigs.Find((x) => { return x.Create_ID == ID; });
    }
    public readonly static List<CreateConfig> createConfigs = new List<CreateConfig>()
    {
        /*机械元件*/
        new CreateConfig(){Create_ID = 1100,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1000, 2),} },
        /*机械元件*/
        new CreateConfig(){Create_ID = 1200,Create_Level = 7,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1110, 4),} },
        /*高能芯片*/
        new CreateConfig(){Create_ID = 1201,Create_Level = 7,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1201, 4),} },
        /*火把*/
        new CreateConfig(){Create_ID = 2000,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1001, 1), new CreateRaw(1011, 1), } },
        /*木斧头*/
        new CreateConfig(){Create_ID = 2001,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 4),} },
        /*铁斧头*/
        new CreateConfig(){Create_ID = 2002,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), } },
        /*木棍*/
        new CreateConfig(){Create_ID = 2100,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2) } },
        /*长柄钢刀*/
        new CreateConfig(){Create_ID = 2101,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), } },
        /*精钢匕首*/
        new CreateConfig(){Create_ID = 2102,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), } },
        /*宽刃钢刀*/
        new CreateConfig(){Create_ID = 2103,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), } },
        /*粗制木弓*/
        new CreateConfig(){Create_ID = 2200,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2),} },
        /*精致木弓*/
        new CreateConfig(){Create_ID = 2201,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 4),} },
        /*金质弓*/
        new CreateConfig(){Create_ID = 2202,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1111, 2), } },
        /*筒手枪*/
        new CreateConfig(){Create_ID = 2300,Create_Level = 1,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), new CreateRaw(1200, 2), } },
        /*BPM*/
        new CreateConfig(){Create_ID = 2301,Create_Level = 1,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), new CreateRaw(1200, 2), } },
        /*AKM*/
        new CreateConfig(){Create_ID = 2302,Create_Level = 1,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), new CreateRaw(1200, 2), } },
    };
}
public struct CreateConfig
{
    [SerializeField]
    public short Create_ID;
    [SerializeField]
    public short Create_Level;
    [SerializeField]
    public List<CreateRaw> Create_Raw;
}
/// <summary>
/// 建筑原料
/// </summary>
public struct CreateRaw
{
    public short ID;
    public short Count;
    public CreateRaw(short id, short count)
    {
        ID = id;
        Count = count;
    }
}