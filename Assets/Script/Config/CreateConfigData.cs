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
        new CreateConfig(){Create_ID = 1020,Create_Level = 7,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1013, 4),} },
        /*����оƬ*/
        new CreateConfig(){Create_ID = 1021,Create_Level = 7,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1013, 4),} },
        /*ľ��ͷ*/
        new CreateConfig(){Create_ID = 2001,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1001, 4), new CreateRaw(1006, 4), } },
        /*����ͷ*/
        new CreateConfig(){Create_ID = 2002,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1001, 4), new CreateRaw(1006, 4), } },
        /*����ľ��*/
        new CreateConfig(){Create_ID = 2003,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1006, 4),} },
        /*���*/
        new CreateConfig(){Create_ID = 2004,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1006, 4), new CreateRaw(1002, 4), } },
        /*����ľ��*/
        new CreateConfig(){Create_ID = 2005,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1006, 4),} },
        /*ľ��*/
        new CreateConfig(){Create_ID = 2006,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1002, 4) } },
        /*�����ֵ�*/
        new CreateConfig(){Create_ID = 2007,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1002, 4), new CreateRaw(1013, 1), } },
        /*����ذ��*/
        new CreateConfig(){Create_ID = 2008,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1002, 2), new CreateRaw(1013, 1), } },
        /*Ͳ��ǹ*/
        new CreateConfig(){Create_ID = 2009,Create_Level = 1,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1002, 2), new CreateRaw(1013, 1), new CreateRaw(1020, 2), } },
        /*���ʹ�*/
        new CreateConfig(){Create_ID = 2010,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1014, 4), new CreateRaw(1006, 4), } },
        /*���иֵ�*/
        new CreateConfig(){Create_ID = 2011,Create_Level = 0,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1002, 2), new CreateRaw(1013, 1), } },
        /*BPM*/
        new CreateConfig(){Create_ID = 2012,Create_Level = 1,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1002, 2), new CreateRaw(1013, 1), new CreateRaw(1020, 2), } },
        /*AKM*/
        new CreateConfig(){Create_ID = 2013,Create_Level = 1,
            Create_Raw = new List<CreateRaw>(){ new CreateRaw(1002, 2), new CreateRaw(1013, 1), new CreateRaw(1020, 2), } },
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