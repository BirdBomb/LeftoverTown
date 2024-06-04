using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConfigData : MonoBehaviour
{
    public static ItemConfig GetItemConfig(int ID)
    {
        return itemConfigs.Find((x) => { return x.Item_ID == ID; });
    }
    /*
     * 1000 - 1999 ����
     * 2000 - 2999 ����
     * 3000 - 3999 ʳ��
     * 4000 - 4999 ����
     * 5000 - 5999 ����
     * 9000 - 9999 ����
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Name = "��",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1001,Item_Name = "ԭľ",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Materials,Average_Value = 1 },
        //new ItemConfig(){ Item_ID = 1002,Item_Name = "ԭľ��",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2001,Item_Name = "ľ��ͷ",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2002,Item_Name = "����ͷ",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2003,Item_Name = "����ľ��",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2004,Item_Name = "���",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2005,Item_Name = "����ľ��",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2006,Item_Name = "ľ��",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2007,Item_Name = "������",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2008,Item_Name = "�̱���",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2009,Item_Name = "��Ͳǹ",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3000,Item_Name = "ˮ",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3001,Item_Name = "��Ⱦ��",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4001,Item_Name = "ľ��",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4002,Item_Name = "����ƿ",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4003,Item_Name = "�մ�ƿ",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 5001,Item_Name = "����ҹѲ�Ӷ�Ա��",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Clothes,Average_Value = 1,HowDoILook = "����ҹѲ�Ӷ�Ա" },
        new ItemConfig(){ Item_ID = 5002,Item_Name = "����ҹѲ�Ӷ�Աñ",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,HowDoILook = "����ҹѲ�Ӷ�Ա"},
        new ItemConfig(){ Item_ID = 9001,Item_Name = "�ٻ���Լ",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9002,Item_Name = "����ľ��",Item_Desc = "",Item_MaxCount = 99,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9003,Item_Name = "����ľ��",Item_Desc = "",Item_MaxCount = 99,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9004,Item_Name = "����",Item_Desc = "",Item_MaxCount = 99,Item_Type = ItemType.Weapon,Average_Value = 1 },
    };
}
[Serializable]
public struct ItemConfig 
{
    [SerializeField]/*���*/
    public int Item_ID;
    [SerializeField]/*����*/
    public string Item_Name;
    [SerializeField]/*����*/
    public string Item_Desc;
    [SerializeField]/*�������*/
    public int Item_MaxCount;
    [SerializeField]/*���*/
    public ItemType Item_Type;
    [SerializeField]/*��ֵ*/
    public float Average_Value;
    [SerializeField]/*�ҿ���ȥ��ô��*/
    public string HowDoILook;
}
[Serializable]
public enum ItemType 
{
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