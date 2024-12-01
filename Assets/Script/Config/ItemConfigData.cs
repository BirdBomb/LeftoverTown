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
     * 1000 - 1999 ����
     * 2000 - 2999 ����
     * 3000 - 3999 ʳ��
     * 4000 - 4999 ʳ��
     * 5000 - 5999 ����
     * 9000 - 9999 ����
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Name = "��",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Default,Average_Value = 1 },
        #region//1000
        new ItemConfig(){ Item_ID = 1001,Item_Name = "ԭľ",ItemRarity = 0,Item_Desc = "������ľ�õ���ԭľ���ӹ���ľ��֮����������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1002,Item_Name = "ľ��",ItemRarity = 0,Item_Desc = "�ӹ�ԭľ�õ���ľ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1003,Item_Name = "ʯͷ",ItemRarity = 0,Item_Desc = "��Ӳ��ʯͷ,�����������컹�����������Ϲ���",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1004,Item_Name = "ú̿",ItemRarity = 0,Item_Desc = "ƽ�������ּ�����Ҫ����Դ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1005,Item_Name = "���",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ������ұ����ש",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1006,Item_Name = "��֦",ItemRarity = 0,Item_Desc = "�洦�ɼ�����֦���ӹ�һ�¿��ܻ�����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1007,Item_Name = "����",ItemRarity = 0,Item_Desc = "����ұ����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1008,Item_Name = "���ܿ�",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ������ұ������ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1009,Item_Name = "���ԭʯ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɱ�ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1010,Item_Name = "����",ItemRarity = 0,Item_Desc = "����ұ����ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1011,Item_Name = "��ˮ��ԭʯ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɱ�ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 300 },
        new ItemConfig(){ Item_ID = 1012,Item_Name = "����ԭʯ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1013,Item_Name = "����",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1014,Item_Name = "��",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1015,Item_Name = "����ˮ��",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1016,Item_Name = "���",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 245 },
        new ItemConfig(){ Item_ID = 1017,Item_Name = "��ʯ��",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1018,Item_Name = "��ˮ��",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 300 },
        new ItemConfig(){ Item_ID = 1019,Item_Name = "����ʯ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1020,Item_Name = "��еԪ��",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1021,Item_Name = "����оƬ",ItemRarity = 0,Item_Desc = "ϡ�������Ŀ�ʯ�������ӹ��ɵ���ʯ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        #endregion
        #region//2000
        new ItemConfig(){ Item_ID = 2001,Item_Name = "ľ��ͷ",ItemRarity = 0,Item_Desc = "ľ�Ƶĸ�ͷ����̫���õ�����������",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2002,Item_Name = "����ͷ",ItemRarity = 0,Item_Desc = "���Ƶĸ�ͷ�������Լ۱�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2003,Item_Name = "����ľ��",ItemRarity = 0,Item_Desc = "��ǿ����ʹ�õ�ľ��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 10,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2004,Item_Name = "���",ItemRarity = 0,Item_Desc = "��������������Ϩ�������޷��ӻ�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2005,Item_Name = "����ľ��",ItemRarity = 0,Item_Desc = "ľ���Ľ��װ棬�ȴ���ľ������׼��˳��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 10,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2006,Item_Name = "ľ��",ItemRarity = 0,Item_Desc = "ľͷ�Ƴɵĳ�����ɱ�������޾�������ѵ��ʹ��",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2007,Item_Name = "�����ֵ�",ItemRarity = 0,Item_Desc = "ľ��������ļ򵥽�ϣ������Լ۱�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2008,Item_Name = "����ذ��",ItemRarity = 0,Item_Desc = "С�ɵ�ذ�ף���ɱ�����������������ڽ�����͵Ϯ",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2009,Item_Name = "Ͳ��ǹ",ItemRarity = 1,Item_Desc = "�Ƿ��������������������ٻ��Ǿ�׼�ȶ���������",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 12,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2010,Item_Name = "���ʹ�",ItemRarity = 1,Item_Desc = "�����뾫��ľ��������ͬ�����������ʹ��۸�߰�",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 12,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2011,Item_Name = "���иֵ�",ItemRarity = 0,Item_Desc = "������������ذ�ף��ó�����ʱ�����ζ����������׼��",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2012,Item_Name = "BPM",ItemRarity = 2,Item_Desc = "ӵ�м��ߵ����ٺͼ��ߵĵ�����ɢ�����������̾���ѹ��ʹ��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2013,Item_Name = "AKM",ItemRarity = 2,Item_Desc = "ͬʱ��߾�׼�Ⱥ�ѹ������ǹе",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 15,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2014,Item_Name = "Glock",ItemRarity = 2,Item_Desc = "С�ɵĵ���ǹе,ӵ������ľ�׼�Ⱥ����ٵ�������ƫС����ͻ����ǰ����󷽰�",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2015,Item_Name = "Ithaca",ItemRarity = 2,Item_Desc = "Ithaca�������ֻ��Ҫ�����������������Ȼ����Ithaca�Ļ���������ʤ��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2016,Item_Name = "Madsen",ItemRarity = 2,Item_Desc = "�ϲ������Ķ��ع�����ʽ��֤�˸ߵ����������������ڵ���Ұ������ǹе�ľ�׼�ȡ������ڴ��ģ��ͻ������ѹ��",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 20,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2017,Item_Name = "AUG",ItemRarity = 3,Item_Desc = "����������ͻ����ǹ�����⹹����հѱ�֤�����������ǹе���ȶ���",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 20,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2018,Item_Name = "����",ItemRarity = 0,Item_Desc = "�������ɿ���Ĺ���",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },


        #endregion
        #region//3000
        new ItemConfig(){ Item_ID = 3000,Item_Name = "ˮ",ItemRarity = 0,Item_Desc = "ˮ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3001,Item_Name = "��Ⱦ��",ItemRarity = 0,Item_Desc = "����Ⱦ���⣬ʳ�û�������غ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3002,Item_Name = "���",ItemRarity = 0,Item_Desc = "��ɫ�Ĺ�ʵ������ɬ��û������ζ����ֻ�����ϲ���ԣ����Ե������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3003,Item_Name = "ˮ��",ItemRarity = 0,Item_Desc = "��֭�Ĺ��ӣ���ɬ��΢��һ����ζ��ʮ�ֽ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3004,Item_Name = "�ƽ����",ItemRarity = 3,Item_Desc = "ʮ��ϡ�е������ֻ�г����ض��ط��Ĺ������м��Ϳ����Բ����ƽ����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3005,Item_Name = "��Ƥ��",ItemRarity = 0,Item_Desc = "����Ƥ���⣬��֬���ӷḻ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3006,Item_Name = "������",ItemRarity = 0,Item_Desc = "���йǵ��⣬�ڸи��ӽ���",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3007,Item_Name = "������",ItemRarity = 0,Item_Desc = "������������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3008,Item_Name = "����",ItemRarity = 0,Item_Desc = "һЩ��������࣬��˵�����൫��ζ���Ϳڸж������������������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3009,Item_Name = "���",ItemRarity = 0,Item_Desc = "���зḻ��۵Ĺ�ʵ��ûʲôζ������ʮ�ֹܱ���������������ʳ��ֲ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3010,Item_Name = "���",ItemRarity = 0,Item_Desc = "����С����Ƴɵ���ۣ�������ʳ��ԭ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3999,Item_Name = "����ʳ��",ItemRarity = 0,Item_Desc = "���õ���ʳ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #region//4000
        new ItemConfig(){ Item_ID = 4000,Item_Name = "ʧ�ܲ���",ItemRarity = 0,Item_Desc = "��Ȼ���ʧ�ܣ��������ǲ��ܳ�",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
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
        new ItemConfig(){ Item_ID = 4009,Item_Name = "�������",ItemRarity = 2,Item_Desc = "����������Ƴɵ��������ڵĲ��ȣ����õ����ʱ��������������������ԭ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4010,Item_Name = "���ӹ�",ItemRarity = 2,Item_Desc = "ˮ���ƳɵĲ��ȣ��������ȵ�ˮ������Щ�˲�̫ϰ�ߣ�������Ϊ�˴������ʵ�ˮ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4011,Item_Name = "ץ��",ItemRarity = 2,Item_Desc = "�ɴ�Ƥ��ʹ������Ƴɵļ���ʳ��������κ��������ϵĴ����ʳ",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4012,Item_Name = "���շ�ζ��",ItemRarity = 3,Item_Desc = "��ˮ����ζ�ı�Ƭ���⣬��Ϊ����������Ũ��Ĺ�֭���Կ��⿴��ȥ��Ө��͸����������ˮ��",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #region//5000
        new ItemConfig(){ Item_ID = 5001,Item_Name = "ҹѲ����",ItemRarity = 2,Item_Desc = "������ҹѲ��Ա��װ֮һ�����ṩ����ֵ",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1, },
        new ItemConfig(){ Item_ID = 5002,Item_Name = "ҹѲ��ñ",ItemRarity = 2,Item_Desc = "������ҹѲ��Ա��װ֮һ�����ṩ����ֵ",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5003,Item_Name = "���·�(��)",ItemRarity = 0,Item_Desc = "��ɫ�Ĳ����·�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5004,Item_Name = "��ñ",ItemRarity = 0,Item_Desc = "ȥð�հ�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5005,Item_Name = "ץ�ۿ�",ItemRarity = 3,Item_Desc = "�������ΰ���Ա����װ֮һ��ͷ������ĺ�ɫץ��������ߵ���ɥʬ��֤��",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5006,Item_Name = "��ʽ������",ItemRarity = 3,Item_Desc = "�������ΰ���Ա����װ֮һ�����Ե���һ���̶ȵĹ���",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5007,Item_Name = "��ñ",ItemRarity = 0,Item_Desc = "�������ΰ���Ա����װ֮һ�����Ե���һ���̶ȵĹ���",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5008,Item_Name = "���·�(��)",ItemRarity = 0,Item_Desc = "��ɫ�Ĳ����·�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5009,Item_Name = "����",ItemRarity = 1,Item_Desc = "���к��˳�����Ÿ����ְ�",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        #endregion
        #region//9000
        new ItemConfig(){ Item_ID = 9001,Item_Name = "������ҳ",ItemRarity = 6,Item_Desc = "��ĳ������˺�µ�����ֽ�ţ�������������ٻ�����",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9002,Item_Name = "����ľ��",ItemRarity = 0,Item_Desc = "������֦�ƳɵĹ�����ɱ��������",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9003,Item_Name = "����ľ��",ItemRarity = 0,Item_Desc = "����֦�ƳɵĹ�������װ������ͷ����ٶ���ɱ����",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9004,Item_Name = "����",ItemRarity = 0,Item_Desc = "�ܹܿصĹ�ҵ�ӵ���ʡ�ŵ���",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9005,Item_Name = "Կ��",ItemRarity = 0,Item_Desc = "�����Կ��ȥ����һ��û�б����ϵ���",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
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
