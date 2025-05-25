using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConfigData 
{
    public static ItemConfig GetItemConfig(int ID)
    {
        return itemConfigs.Find((x) => { return x.Item_ID == ID; });
    }
    /*
     * 1000 - 1999 ���� 1100һ������1200��������
     * 2000 - 2999 ����
     * 3000 - 3999 ʳ��
     * 4000 - 4999 ʳ��
     * 5000 - 5999 ����
     * 9000 - 9999 ����
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 0 ,},
        #region//1000-1999����
        #region//1000-1099һ������
        /*ԭľ*/new ItemConfig(){ Item_ID = 1000,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��֦*/new ItemConfig(){ Item_ID = 1001,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*�ɲ�*/new ItemConfig(){ Item_ID = 1002,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ʯͷ*/new ItemConfig(){ Item_ID = 1010,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ú̿*/new ItemConfig(){ Item_ID = 1011,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*���*/new ItemConfig(){ Item_ID = 1012,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gold },
        /*����*/new ItemConfig(){ Item_ID = 1013,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*����*/new ItemConfig(){ Item_ID = 1014,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Blue },
        /*���*/new ItemConfig(){ Item_ID = 1015,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Blue },
        /*����*/new ItemConfig(){ Item_ID = 1016,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*�쾧*/new ItemConfig(){ Item_ID = 1017,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Blue },
        /*�׾�*/new ItemConfig(){ Item_ID = 1018,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Blue },
        #endregion
        #region//1100-1199��������
        /*ľ��*/new ItemConfig(){ Item_ID = 1100,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*����*/new ItemConfig(){ Item_ID = 1110,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��*/new ItemConfig(){ Item_ID = 1111,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gold },
        /*��Դʯ*/new ItemConfig(){ Item_ID = 1112,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Purple },
        /*�̱�ʯ*/new ItemConfig(){ Item_ID = 1113,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 245,Item_Rarity = ItemRarity.Purple },
        /*��ʯ��*/new ItemConfig(){ Item_ID = 1114,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*�챦ʯ*/new ItemConfig(){ Item_ID = 1115,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 300,Item_Rarity = ItemRarity.Purple },
        /*����ʯ*/new ItemConfig(){ Item_ID = 1116,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Purple },
        #endregion
        #region//1200-1299��������
        /*��еԪ��*/new ItemConfig(){ Item_ID = 1200,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*����Ԫ��*/new ItemConfig(){ Item_ID = 1201,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ǹеԪ��*/new ItemConfig(){ Item_ID = 1202,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #endregion
        #region//2000-2999����
        #region//2000-2099������
        /*���*/new ItemConfig(){ Item_ID = 2000,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ľ��*/new ItemConfig(){ Item_ID = 2010,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*����*/new ItemConfig(){ Item_ID = 2011,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ľ��*/new ItemConfig(){ Item_ID = 2020,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*����*/new ItemConfig(){ Item_ID = 2021,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ľ��*/new ItemConfig(){ Item_ID = 2030,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2100-2199��ս����
        /*ľ��*/new ItemConfig(){ Item_ID = 2100,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��ì*/new ItemConfig(){ Item_ID = 2101,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ذ��*/new ItemConfig(){ Item_ID = 2102,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*����*/new ItemConfig(){ Item_ID = 2103,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2200-2299Զ������
        /*��ľ��*/new ItemConfig(){ Item_ID = 2200,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��ľ��*/new ItemConfig(){ Item_ID = 2201,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*���ʹ�*/new ItemConfig(){ Item_ID = 2202,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2300-2399������
        /*������ǹ*/new ItemConfig(){ Item_ID = 2300,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*�̳��ǹ*/new ItemConfig(){ Item_ID = 2301,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ľ����ǹ*/new ItemConfig(){ Item_ID = 2302,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��׼��ǹ*/new ItemConfig(){ Item_ID = 2303,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*������ǹ*/new ItemConfig(){ Item_ID = 2304,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*���ͻ�ǹ*/new ItemConfig(){ Item_ID = 2305,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��׼��ǹ*/new ItemConfig(){ Item_ID = 2306,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2400-2499��������
        #endregion
        #endregion
        #region//3000-3999ʳ��
        #region//3000-3099����
        /*���*/new ItemConfig(){ Item_ID = 3000,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*�ƽ����*/new ItemConfig(){ Item_ID = 3001,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 300,Item_Rarity = ItemRarity.Gray },
        /*ˮ��*/new ItemConfig(){ Item_ID = 3002,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 8,Item_Rarity = ItemRarity.Gray },
        /*���*/new ItemConfig(){ Item_ID = 3003,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*����*/new ItemConfig(){ Item_ID = 3004,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//3100-3199�⵰
        /*��Ƥ��*/new ItemConfig(){ Item_ID = 3100,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 20,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 3101,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 22,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 3102,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 28,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 3103,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        /*��Ⱦ��*/new ItemConfig(){ Item_ID = 3104,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*����*/new ItemConfig(){ Item_ID = 3110,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//3200-3299����
        /*���*/new ItemConfig(){ Item_ID = 3200,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        /*����ʳ��*/new ItemConfig(){ Item_ID = 3999,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//4000-4999ʳ��
        #region//4000-4099����
        /*�����*/new ItemConfig(){ Item_ID = 4001,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 4002,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 4003,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 4004,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 4005,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*���߹�*/new ItemConfig(){ Item_ID = 4006,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��ˮ��*/new ItemConfig(){ Item_ID = 4007,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*�����*/new ItemConfig(){ Item_ID = 4008,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//4100-4199���
        /*ʧ�ܲ�*/new ItemConfig(){ Item_ID = 4100,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 4101,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*���ӹ�*/new ItemConfig(){ Item_ID = 4102,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��ץ��*/new ItemConfig(){ Item_ID = 4103,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*������*/new ItemConfig(){ Item_ID = 4104,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*ˮ����*/new ItemConfig(){ Item_ID = 4105,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//ҩ��
        /*���֭*/new ItemConfig(){ Item_ID = 4200,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #endregion
        #region//5000-5999����
        #region//5000-5099ñ��
        /*Ѳ��ñ*/new ItemConfig(){ Item_ID = 5000,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*��ñ*/new ItemConfig(){ Item_ID = 5001,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*������*/new ItemConfig(){ Item_ID = 5002,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*��ñ*/new ItemConfig(){ Item_ID = 5003,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*����*/new ItemConfig(){ Item_ID = 5004,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*����*/new ItemConfig(){ Item_ID = 5005,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*��Ƥñ*/new ItemConfig(){ Item_ID = 5006,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*ľ��*/new ItemConfig(){ Item_ID = 5007,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        #endregion
        #region//5100-5199�·�
        /*Ѳ����*/new ItemConfig(){ Item_ID = 5100,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*�Ҳ���*/new ItemConfig(){ Item_ID = 5101,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*�ֲ���*/new ItemConfig(){ Item_ID = 5102,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*������*/new ItemConfig(){ Item_ID = 5103,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*����*/new ItemConfig(){ Item_ID = 5104,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*ľ��*/new ItemConfig(){ Item_ID = 5105,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        #endregion
        #endregion
        #region//9000-9999����
        #region//9000-9099��ҩ
        /*����ľ��*/new ItemConfig(){ Item_ID = 9000,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Arrow,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*����ľ��*/new ItemConfig(){ Item_ID = 9001,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Arrow,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*���Ƶ���*/new ItemConfig(){ Item_ID = 9010,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Bullet,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*��Ԫ��ϻ*/new ItemConfig(){ Item_ID = 9020,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Bullet,Item_Value = 6,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//9900-9999����
        /*��ҳ*/new ItemConfig(){ Item_ID = 9900,Item_Size = ItemSize.Sin,Item_Max = 9,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*Կ��*/new ItemConfig(){ Item_ID = 9901,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
#endregion
    };
    public static string Colour(string str,ItemRarity rarity)
    {
        if (rarity == ItemRarity.Gray)
        {
            str = "<color=#9A9A9A>" + str + "</color>";
        }
        else if (rarity == ItemRarity.Green)
        {
            str = "<color=#43C743>" + str + "</color>";
        }
        else if (rarity == ItemRarity.Blue)
        {
            str = "<color=#4487C7>" + str + "</color>";
        }
        else if (rarity == ItemRarity.Purple)
        {
            str = "<color=#d507c6>" + str + "</color>";
        }
        else if (rarity == ItemRarity.Gold)
        {
            str = "<color=#FF9D09>" + str + "</color>";
        }
        else if (rarity == ItemRarity.Red)
        {
            str = "<color=#FF090E>" + str + "</color>";
        }
        else if (rarity == ItemRarity.Rainbow)
        {
            str = "<color=#D59DD6>" + str + "</color>";
        }
        return str;
    }
}
[Serializable]
public struct ItemConfig 
{
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    public short Item_ID;
    /// <summary>
    /// ���ֵ
    /// </summary>
    [SerializeField]
    public short Item_Max; 
    public ItemRarity Item_Rarity;
    [SerializeField]/*������*/
    public ItemType Item_Type;
    [SerializeField]/*�ߴ���*/
    public ItemSize Item_Size;
    [SerializeField]/*��ֵ*/
    public int Item_Value;
}
public enum ItemType 
{
    /// <summary>
    /// Ĭ��
    /// </summary>
    Def,
    /// <summary>
    /// ����
    /// </summary>
    Mat, 
    /// <summary>
    /// ʳ��
    /// </summary>
    Food,
    /// <summary>
    /// ʳ��
    /// </summary>
    Dishes,
    /// <summary>
    /// ����
    /// </summary>
    Weapon,
    /// ����
    /// </summary>
    Tool,
    /// <summary>
    /// ����
    /// </summary>
    Container,
    /// <summary>
    /// ��
    /// </summary>
    Arrow,
    /// <summary>
    /// �ӵ�
    /// </summary>
    Bullet,
    /// <summary>
    /// ñ��
    /// </summary>
    Hat,
    /// <summary>
    /// �·�
    /// </summary>
    Clothes,
}
public enum ItemSize
{
    /// <summary>
    /// ����Single
    /// </summary>
    Sin,
    /// <summary>
    /// �ѵ�Group
    /// </summary>
    Gro,
    /// <summary>
    /// ����Container
    /// </summary>
    Con,
}
public enum ItemRarity
{
    Gray,
    Green,
    Blue,
    Purple,
    Gold,
    Red,
    Rainbow,
}
public struct ItemRaw
{
    public short ID;
    public short Count;
    public ItemRaw(short id, short count)
    {
        ID = id;
        Count = count;
    }
}
[Serializable]
public struct BaseLootInfo
{
    [SerializeField, Header("������������")]
    public short ID;
    [SerializeField, Header("��С����������")]
    public short CountMin;
    [SerializeField, Header("������������")]
    public short CountMax;
}
[Serializable]
public struct ExtraLootInfo
{
    [SerializeField, Header("�����������")]
    public short ID;
    [SerializeField, Header("�������������")]
    public short Count;
    [SerializeField, Header("���������Ȩ��(1/1000)")]
    public short Weight;
}
