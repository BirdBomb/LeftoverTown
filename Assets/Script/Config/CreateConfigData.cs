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
        /*��еԪ��*/
        new CreateConfig(){Create_ID = 1100,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1000, 2),} },
        /*��еԪ��*/
        new CreateConfig(){Create_ID = 1200,Create_Level = 7,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1110, 4),} },
        /*����оƬ*/
        new CreateConfig(){Create_ID = 1201,Create_Level = 7,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1201, 4),} },
        /*���*/
        new CreateConfig(){Create_ID = 2000,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1001, 1), new CreateRaw(1011, 1), } },
        /*ľ��ͷ*/
        new CreateConfig(){Create_ID = 2001,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 4),} },
        /*����ͷ*/
        new CreateConfig(){Create_ID = 2002,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), } },
        /*ľ��*/
        new CreateConfig(){Create_ID = 2100,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2) } },
        /*�����ֵ�*/
        new CreateConfig(){Create_ID = 2101,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), } },
        /*����ذ��*/
        new CreateConfig(){Create_ID = 2102,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), } },
        /*���иֵ�*/
        new CreateConfig(){Create_ID = 2103,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2), new CreateRaw(1110, 1), } },
        /*����ľ��*/
        new CreateConfig(){Create_ID = 2200,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 2),} },
        /*����ľ��*/
        new CreateConfig(){Create_ID = 2201,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1100, 4),} },
        /*���ʹ�*/
        new CreateConfig(){Create_ID = 2202,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1111, 2), } },
        /*Ͳ��ǹ*/
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
/// ����ԭ��
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