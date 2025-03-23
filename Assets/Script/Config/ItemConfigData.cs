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
        new ItemConfig(){ Item_ID = 0,Item_Name = "����",ItemRarity = 0,Item_Desc = "����",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Default,Average_Value = 0 ,},
        #region//1000-1999����
        #region//1000-1099һ������
        new ItemConfig(){ Item_ID = 1000,Item_Name = "ԭľ",ItemRarity = 0,Item_Desc = "������ľ�õ���ԭľ���ӹ���ľ��֮����������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1001,Item_Name = "��֦",ItemRarity = 0,Item_Desc = "�洦�ɼ�����֦���ӹ�һ�¿��ܻ�����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1010,Item_Name = "ʯͷ",ItemRarity = 0,Item_Desc = "��Ӳ��ʯͷ,�����������컹�����������Ϲ���",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1011,Item_Name = "ú̿",ItemRarity = 0,Item_Desc = "ƽ�������ּ�����Ҫ����Դ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1012,Item_Name = "���ʯ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ������ұ����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1013,Item_Name = "����ʯ",ItemRarity = 0,Item_Desc = "����ұ������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1014,Item_Name = "���ܿ�",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ������ұ������ˮ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1015,Item_Name = "���ԭʯ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɱ�ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1016,Item_Name = "����ʯ",ItemRarity = 0,Item_Desc = "����ұ����ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1017,Item_Name = "��ˮ��ԭʯ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɱ�ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 300 },
        new ItemConfig(){ Item_ID = 1018,Item_Name = "����ԭʯ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        #endregion
        #region//1100-1199��������
        new ItemConfig(){ Item_ID = 1100,Item_Name = "ľ��",ItemRarity = 0,Item_Desc = "�ӹ�ԭľ�õ���ľ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1110,Item_Name = "����",ItemRarity = 0,Item_Desc = "���쿪ʼ�ĵ�һ�����Լ�֮���ÿһ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1111,Item_Name = "��",ItemRarity = 0,Item_Desc = "��ֵ���ߵĽ𶧣�ó���е�Ӳͨ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1112,Item_Name = "����ˮ��",ItemRarity = 0,Item_Desc = "�̺��޴�������ˮ����������Ϊ��Ч����Դ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1113,Item_Name = "���",ItemRarity = 0,Item_Desc = "��ֵ���ߵı�ʯ��ó���е�Ӳͨ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 245 },
        new ItemConfig(){ Item_ID = 1114,Item_Name = "��ʯ��",ItemRarity = 0,Item_Desc = "����ʯ�ӹ���������������������䲻�ȶ�",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1115,Item_Name = "��ˮ��",ItemRarity = 0,Item_Desc = "��ֵ���ߵı�ʯ��ó���е�Ӳͨ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 300 },
        new ItemConfig(){ Item_ID = 1116,Item_Name = "����ʯ",ItemRarity = 0,Item_Desc = "��ֵ���ߵı�ʯ��ó���е�Ӳͨ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        #endregion
        #region//1200-1299��������
        new ItemConfig(){ Item_ID = 1200,Item_Name = "��еԪ��",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1201,Item_Name = "����оƬ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        #endregion
        #endregion
        #region//2000-2999����
        #region//2000-2099������
        new ItemConfig(){ Item_ID = 2000,Item_Name = "���",ItemRarity = 0,Item_Desc = "��������������Ϩ�������޷��ӻ�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2010,Item_Name = "ľ��ͷ",ItemRarity = 0,Item_Desc = "ľ�Ƶĸ�ͷ����̫���õ�����������",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2011,Item_Name = "����ͷ",ItemRarity = 0,Item_Desc = "���Ƶĸ�ͷ�������Լ۱�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2020,Item_Name = "ľ��",ItemRarity = 0,Item_Desc = "�������ɿ���Ĺ���",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2021,Item_Name = "����",ItemRarity = 0,Item_Desc = "�������ɿ���Ĺ���",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2030,Item_Name = "ľͷ���",ItemRarity = 0,Item_Desc = "�������ǰ,������һ�����ץ��������",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        #endregion
        #region//2100-2199��ս����
        new ItemConfig(){ Item_ID = 2100,Item_Name = "ľ��",ItemRarity = 0,Item_Desc = "ľͷ�Ƴɵĳ�����ɱ�������޾�������ѵ��ʹ��",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2101,Item_Name = "�����ֵ�",ItemRarity = 0,Item_Desc = "ľ��������ļ򵥽�ϣ������Լ۱�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2102,Item_Name = "����ذ��",ItemRarity = 0,Item_Desc = "С�ɵ�ذ�ף���ɱ�����������������ڽ�����͵Ϯ",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2103,Item_Name = "���иֵ�",ItemRarity = 0,Item_Desc = "������������ذ�ף��ó�����ʱ�����ζ����������׼��",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        #endregion
        #region//2200-2299Զ������
        new ItemConfig(){ Item_ID = 2200,Item_Name = "����ľ��",ItemRarity = 0,Item_Desc = "��ǿ����ʹ�õ�ľ��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 10,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2201,Item_Name = "����ľ��",ItemRarity = 0,Item_Desc = "ľ���Ľ��װ棬�ȴ���ľ������׼��˳��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 10,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2202,Item_Name = "���ʹ�",ItemRarity = 1,Item_Desc = "�����뾫��ľ��������ͬ�����������ʹ��۸�߰�",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 12,Item_Type = ItemType.Weapon,Average_Value = 1 },
        #endregion
        #region//2300-2399������
        new ItemConfig(){ Item_ID = 2300,Item_Name = "������ǹ",ItemRarity = 1,Item_Desc = "�Ƿ��������������������ٻ��Ǿ�׼�ȶ���������",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 12,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2301,Item_Name = "���ͳ��ǹ",ItemRarity = 2,Item_Desc = "ӵ�м��ߵ����ٺͼ��ߵĵ�����ɢ�����������̾���ѹ��ʹ��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2302,Item_Name = "��ʽ�Զ���ǹ",ItemRarity = 2,Item_Desc = "ͬʱ��߾�׼�Ⱥ�ѹ������ǹе",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 15,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2303,Item_Name = "��ʽ��ǹ",ItemRarity = 2,Item_Desc = "С�ɵĵ���ǹе,ӵ������ľ�׼�Ⱥ����ٵ�������ƫС����ͻ����ǰ����󷽰�",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2304,Item_Name = "�ö�����ǹ",ItemRarity = 2,Item_Desc = "�������ֻ��Ҫ�����������������Ȼ���ڻ���������ʤ��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2305,Item_Name = "���ɭ��ǹ",ItemRarity = 2,Item_Desc = "�ϲ������Ķ��ع�����ʽ��֤�˸ߵ����������������ڵ���Ұ������ǹе�ľ�׼�ȡ������ڴ��ģ��ͻ������ѹ��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 20,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2306,Item_Name = "��׼�Զ���ǹ",ItemRarity = 3,Item_Desc = "����������ͻ����ǹ�����⹹����հѱ�֤�����������ǹе���ȶ���",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 20,Item_Type = ItemType.Weapon,Average_Value = 1 },
        #endregion
        #region//2400-2499��������
        #endregion
        #endregion
        #region//3000-3999ʳ��
        #region//3000-3099����
        new ItemConfig(){ Item_ID = 3000,Item_Name = "���",ItemRarity = 0,Item_Desc = "��ɫ�Ĺ�ʵ������ɬ��û������ζ����ֻ�����ϲ���ԣ����Ե������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 5 },
        new ItemConfig(){ Item_ID = 3001,Item_Name = "�ƽ����",ItemRarity = 3,Item_Desc = "ʮ��ϡ�е������ֻ�г����ض��ط��Ĺ������м��Ϳ����Բ����ƽ����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 300 },
        new ItemConfig(){ Item_ID = 3002,Item_Name = "ˮ��",ItemRarity = 0,Item_Desc = "��֭�Ĺ��ӣ���ɬ��΢��һ����ζ��ʮ�ֽ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 8 },
        new ItemConfig(){ Item_ID = 3003,Item_Name = "���",ItemRarity = 0,Item_Desc = "���зḻ��۵Ĺ�ʵ��ûʲôζ������ʮ�ֹܱ���������������ʳ��ֲ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 2 },
        new ItemConfig(){ Item_ID = 3004,Item_Name = "����",ItemRarity = 0,Item_Desc = "һ������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 5 },
        #endregion
        #region//3100-3199�⵰
        new ItemConfig(){ Item_ID = 3100,Item_Name = "��Ƥ��",ItemRarity = 0,Item_Desc = "����Ƥ���⣬��֬���ӷḻ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 20 },
        new ItemConfig(){ Item_ID = 3101,Item_Name = "������",ItemRarity = 0,Item_Desc = "���йǵ��⣬�ڸи��ӽ���",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 22 },
        new ItemConfig(){ Item_ID = 3102,Item_Name = "������",ItemRarity = 0,Item_Desc = "������������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 28 },
        new ItemConfig(){ Item_ID = 3103,Item_Name = "����",ItemRarity = 0,Item_Desc = "һЩ��������࣬��˵�����൫��ζ���Ϳڸж������������������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 10 },
        new ItemConfig(){ Item_ID = 3104,Item_Name = "��Ⱦ��",ItemRarity = 0,Item_Desc = "����Ⱦ���⣬ʳ�û�������غ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 5 },
        new ItemConfig(){ Item_ID = 3110,Item_Name = "����",ItemRarity = 0,Item_Desc = "һ��ʮ�ֳ�������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 10 },
        #endregion
        #region//3200-3299����
        new ItemConfig(){ Item_ID = 3200,Item_Name = "���",ItemRarity = 0,Item_Desc = "����С����Ƴɵ���ۣ�������ʳ��ԭ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        new ItemConfig(){ Item_ID = 3999,Item_Name = "����ʳ��",ItemRarity = 0,Item_Desc = "���õ���ʳ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #region//4000-4999ʳ��
        #region//4000-4099����
        new ItemConfig(){ Item_ID = 4001,Item_Name = "�����",ItemRarity = 1,Item_Desc = "��������ð�͵Ĵ�Ƥ�⣬��Ϊ������֬�ḻ���Գ���ȥ������֭",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4002,Item_Name = "������",ItemRarity = 1,Item_Desc = "���ƵĴ����⣬���Ź�ͷҧ��һ��ں�����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4003,Item_Name = "������",ItemRarity = 1,Item_Desc = "��������ð�͵Ĵ�Ƥ�⣬��Ϊ������֬�ḻ���Գ���ȥ������֭",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4004,Item_Name = "������",ItemRarity = 1,Item_Desc = "���ƵĴ����⣬���Ź�ͷҧ��һ��ں�����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4005,Item_Name = "��ζ����",ItemRarity = 1,Item_Desc = "��������ð�͵Ĵ�Ƥ�⣬��Ϊ������֬�ḻ���Գ���ȥ������֭",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4006,Item_Name = "���߹�",ItemRarity = 1,Item_Desc = "���ƵĴ����⣬���Ź�ͷҧ��һ��ں�����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4007,Item_Name = "��ˮ��",ItemRarity = 1,Item_Desc = "��������ð�͵Ĵ�Ƥ�⣬��Ϊ������֬�ḻ���Գ���ȥ������֭",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4008,Item_Name = "�����",ItemRarity = 1,Item_Desc = "���ƵĴ����⣬���Ź�ͷҧ��һ��ں�����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #region//4100-4199���
        new ItemConfig(){ Item_ID = 4100,Item_Name = "ʧ�ܲ���",ItemRarity = 0,Item_Desc = "��Ȼ���ʧ�ܣ��������ǲ��ܳ�",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4101,Item_Name = "�������",ItemRarity = 2,Item_Desc = "����������Ƴɵ��������ڵĲ��ȣ����õ����ʱ��������������������ԭ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4102,Item_Name = "���ӹ�",ItemRarity = 2,Item_Desc = "ˮ���ƳɵĲ��ȣ��������ȵ�ˮ������Щ�˲�̫ϰ�ߣ�������Ϊ�˴������ʵ�ˮ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4103,Item_Name = "ץ��",ItemRarity = 2,Item_Desc = "�ɴ�Ƥ��ʹ������Ƴɵļ���ʳ��������κ��������ϵĴ����ʳ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4104,Item_Name = "���շ�ζ��",ItemRarity = 3,Item_Desc = "��ˮ����ζ�ı�Ƭ���⣬��Ϊ����������Ũ��Ĺ�֭���Կ��⿴��ȥ��Ө��͸����������ˮ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4105,Item_Name = "ˮ����",ItemRarity = 3,Item_Desc = "���������������Ƭ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #region//ҩ��
        new ItemConfig(){ Item_ID = 4200,Item_Name = "���֭",ItemRarity = 0,Item_Desc = "���ե�����Ĺ�֭����֪Ϊ�α�ñȹ�ʵ������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #endregion
        #region//5000-5999����
        #region//5000-5099ñ��
        new ItemConfig(){ Item_ID = 5000,Item_Name = "Ѳ��ñ",ItemRarity = 2,Item_Desc = "�ΰ�����װ֮һ�����ṩ����ֵ",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5001,Item_Name = "��ñ",ItemRarity = 0,Item_Desc = "�ṩЩ����������",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5002,Item_Name = "������",ItemRarity = 3,Item_Desc = "ͷ������ĺ�ɫץ��������ߵ���ɥʬ��֤��",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5003,Item_Name = "��ñ",ItemRarity = 0,Item_Desc = "ûʲô������������",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5004,Item_Name = "����",ItemRarity = 1,Item_Desc = "���к��˳�����Ÿ����ְ�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5005,Item_Name = "����",ItemRarity = 2,Item_Desc = "���Ƶ�ͷ�������Ե����˺�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5006,Item_Name = "��Ƥñ",ItemRarity = 2,Item_Desc = "�͸�����Ƶ�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        #endregion
        #region//5100-5199�·�
        new ItemConfig(){ Item_ID = 5100,Item_Name = "Ѳ����",ItemRarity = 2,Item_Desc = "�ΰ�����װ֮һ�����ṩ����ֵ",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5101,Item_Name = "�����·�(��)",ItemRarity = 0,Item_Desc = "��ɫ�Ĳ����·�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5102,Item_Name = "�����·�(��)",ItemRarity = 0,Item_Desc = "��ɫ�Ĳ����·�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5103,Item_Name = "������",ItemRarity = 3,Item_Desc = "���Ե���һ���̶ȵĹ���",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5104,Item_Name = "����",ItemRarity = 2,Item_Desc = "���Ե���һ���̶ȵĹ���",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        #endregion
        #endregion
        #region//9000-9999����
        #region//9000-9099��ҩ
        new ItemConfig(){ Item_ID = 9000,Item_Name = "����ľ��",ItemRarity = 0,Item_Desc = "������֦�ƳɵĹ�����ɱ��������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Arrow,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9001,Item_Name = "����ľ��",ItemRarity = 0,Item_Desc = "����֦�ƳɵĹ�������װ������ͷ����ٶ���ɱ����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Arrow,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9010,Item_Name = "����",ItemRarity = 0,Item_Desc = "�ܹܿصĹ�ҵ�ӵ���ʡ�ŵ���",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Bullet,Average_Value = 1 },
        #endregion
        #region//9900-9999����
        new ItemConfig(){ Item_ID = 9900,Item_Name = "������ҳ",ItemRarity = 6,Item_Desc = "��ĳ������˺�µ�����ֽ�ţ�������������ٻ�����",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9901,Item_Name = "Կ��",ItemRarity = 0,Item_Desc = "�����Կ��ȥ����һ��û�б����ϵ���",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        #endregion
#endregion
    };
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
    /// ����
    /// </summary>
    [SerializeField]
    public string Item_Name;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    public string Item_Desc;

    /// <summary>
    /// ���ֵ
    /// </summary>
    [SerializeField]
    public short Item_MaxCount;
    /// <summary>
    /// ��Ʒϡ�ж�
    /// </summary>
    public int ItemRarity;
    [SerializeField]/*������*/
    public ItemType Item_Type;
    [SerializeField]/*�ߴ���*/
    public ItemSize Item_Size;
    [SerializeField]/*����������*/
    public float Attack_Distance;
    [SerializeField]/*��ֵ*/
    public float Average_Value;
}
[Serializable]
public enum ItemType 
{
    /// <summary>
    /// Ĭ��
    /// </summary>
    Default,
    /// <summary>
    /// ����
    /// </summary>
    Materials,
    /// <summary>
    /// ʳ��
    /// </summary>
    Ingredient,
    /// <summary>
    /// ʳ��
    /// </summary>
    Food,
    /// <summary>
    /// ����
    /// </summary>
    Weapon,
    /// <summary>
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
[Serializable]
public enum ItemSize
{
    /// <summary>
    /// ������
    /// </summary>
    AsOne,
    /// <summary>
    /// �ɶѵ�
    /// </summary>
    AsGroup,
    /// <summary>
    /// ������
    /// </summary>
    AsContainer,
}
