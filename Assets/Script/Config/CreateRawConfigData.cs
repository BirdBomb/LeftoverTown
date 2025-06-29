using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �ϳ��б�
/// </summary>
public class CreateRawConfigData 
{
    public static CreateRawConfig GetCreateRawConfig(int ID)
    {
        return createRawConfigs.Find((x) => { return x.Create_TargetID == ID; });
    }
    public readonly static List<CreateRawConfig> createRawConfigs = new List<CreateRawConfig>()
    {
        /*ľ��*/new CreateRawConfig()
        {
            Create_TargetID = 1100, Create_TargetCount = 4,
            /*ԭľ1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1000,1) }
        },
        /*��еԪ��*/new CreateRawConfig()
        {
            Create_TargetID = 1200, Create_TargetCount = 2,
            /*����1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1110,1) }
        },
        /*���*/new CreateRawConfig()
        {
            Create_TargetID = 2000, Create_TargetCount = 1,
            /*��֦1,��2*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,1),new ItemRaw(1002,2) }
        },
        /*ľ����*/new CreateRawConfig()
        {
            Create_TargetID = 2010, Create_TargetCount = 1,
            /*��֦1,����4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1100, 4) }
        },
        /*������*/new CreateRawConfig()
        {
            Create_TargetID = 2011, Create_TargetCount = 1,
            /*��֦1,����4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1110, 4) }
        },
        /*ľ����*/new CreateRawConfig()
        {
            Create_TargetID = 2020, Create_TargetCount = 1,
            /*��֦1,ľ��4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1100, 4) }
        },
        /*������*/new CreateRawConfig()
        {
            Create_TargetID = 2021, Create_TargetCount = 1,
            /*��֦1,����4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1110, 4) }
        },
        /*ľ���*/new CreateRawConfig()
        {
            Create_TargetID = 2030, Create_TargetCount = 1,
            /*��֦4,ľ��4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,4), new ItemRaw(1100, 4) }
        },
        /*ľ��ͷ*/new CreateRawConfig()
        {
            Create_TargetID = 2040, Create_TargetCount = 1,
            /*��֦4,ľ��4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,4), new ItemRaw(1100, 4) }
        },
        /*ľ��*/new CreateRawConfig()
        {
            Create_TargetID = 2100, Create_TargetCount = 1,
            /*��֦4,ľ��4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,4), new ItemRaw(1100, 4) }
        },
        /*��ì*/new CreateRawConfig()
        {
            Create_TargetID = 2101, Create_TargetCount = 1,
            /*��֦2,����4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1110,2) }
        },
        /*��ذ��*/new CreateRawConfig()
        {
            Create_TargetID = 2102, Create_TargetCount = 1,
            /*��֦4,ľ��4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1110,2) }
        },
        /*����*/new CreateRawConfig()
        {
            Create_TargetID = 2103, Create_TargetCount = 1,
            /*ľ��2,����4*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,2),new ItemRaw(1110,4) }
        },
        /*ľ��*/new CreateRawConfig()
        {
            Create_TargetID = 2200, Create_TargetCount = 1,
            /*��֦3,ľ��3*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,3),new ItemRaw(1100,3) }
        },
        /*����ľ��*/new CreateRawConfig()
        {
            Create_TargetID = 2201, Create_TargetCount = 1,
            /*��֦3,ľ��1,��еԪ��1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,3),new ItemRaw(1110,1), new ItemRaw(1200, 1) }
        },
        /*��*/new CreateRawConfig()
        {
            Create_TargetID = 2202, Create_TargetCount = 1,
            /*��֦3,ľ��1,��2*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001,3),new ItemRaw(1110,1), new ItemRaw(1111, 2) }
        },
        /*������ǹ*/new CreateRawConfig()
        {
            Create_TargetID = 2300, Create_TargetCount = 1,
            /*ľ��1 ����2 ��еԪ��2 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100,1),new ItemRaw(1110,2), new ItemRaw(1200, 2) }
        },
        /*���ͳ��ǹ*/new CreateRawConfig()
        {
            Create_TargetID = 2301, Create_TargetCount = 1,
            /*ľ��1 ����2 ��еԪ��2 ǹеԪ��1 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1110, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 1) }
        },
        /*��ʽ�Զ���ǹ*/new CreateRawConfig()
        {
            Create_TargetID = 2302, Create_TargetCount = 1,
            /*ľ��1 ����2 ��еԪ��2  ǹеԪ��2 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1110, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 2) }
        },
        /*��ʽ��ǹ*/new CreateRawConfig()
        {
            Create_TargetID = 2303, Create_TargetCount = 1,
            /*ľ��1 ����2 ��еԪ��2  ǹеԪ��1 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1110, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 1) }
        },
        /*�ö�����ǹ*/new CreateRawConfig()
        {
            Create_TargetID = 2304, Create_TargetCount = 1,
            /*ľ��1 ����2 ��еԪ��2 ǹеԪ��2 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1110, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 2) }
        },
        /*���ɭ��ǹ*/new CreateRawConfig()
        {
            Create_TargetID = 2305, Create_TargetCount = 1,
            /*ľ��1 ����2 ��еԪ��2  ǹеԪ��3 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1110, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 3) }
        },
        /*��׼�Զ���ǹ*/new CreateRawConfig()
        {
            Create_TargetID = 2306, Create_TargetCount = 1,
            /*ľ��1 ����2 ��еԪ��2 ǹеԪ��5 */
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 1), new ItemRaw(1110, 2), new ItemRaw(1200, 2), new ItemRaw(1202, 5) }
        },
        /*��ñ*/new CreateRawConfig()
        {
            Create_TargetID = 5001, Create_TargetCount = 1,
            /*��6*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1002,6) }
        },
        /*����*/new CreateRawConfig()
        {
            Create_TargetID = 5005, Create_TargetCount = 1,
            /*����3*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1110,3) }
        },
        /*ľ��*/new CreateRawConfig()
        {
            Create_TargetID = 5007, Create_TargetCount = 1,
            /*ľ��8*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100,8) }
        },
        /*����*/new CreateRawConfig()
        {
            Create_TargetID = 5104, Create_TargetCount = 1,
            /*����3*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1110, 3) }
        },
        /*ľ��*/new CreateRawConfig()
        {
            Create_TargetID = 5105, Create_TargetCount = 1,
            /*ľ��12*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1100, 12) }
        },
        /*ľ��*/new CreateRawConfig()
        {
            Create_TargetID = 9000, Create_TargetCount = 9,
            /*��֦1ľ��1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001, 1),new ItemRaw(1100, 1) }
        },
        /*����*/new CreateRawConfig()
        {
            Create_TargetID = 9001, Create_TargetCount = 9,
            /*��֦1����1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1001, 1),new ItemRaw(1110, 1) }
        },
        /*���Ƶ���*/new CreateRawConfig()
        {
            Create_TargetID = 9010, Create_TargetCount = 10,
            /*����1��ʯ��1*/
            Create_RawList = new List<ItemRaw>(){ new ItemRaw(1110, 1),new ItemRaw(1114, 1) }
        },
    };
}
public struct CreateRawConfig
{
    public int Create_TargetID;
    public int Create_TargetCount;
    public List<ItemRaw> Create_RawList;
}