using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConfigData : MonoBehaviour
{
    /*
     * 1000 - 1999 ����
     * 2000 - 2999 ����
     * 
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Name = "ԭ��ľ��",Item_Desc = "",Item_Count = 0,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2001,Item_Name = "ľ��ͷ",Item_Desc = "",Item_Count = 0,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2002,Item_Name = "����ͷ",Item_Desc = "",Item_Count = 0,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
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
    [SerializeField]/*����*/
    public int Item_Count;
    [SerializeField]/*���*/
    public ItemType Item_Type;
    [SerializeField]/*����*/
    public float Average_Weight;
    [SerializeField]/*��ֵ*/
    public float Average_Value;
    [SerializeField]/*��Ϣ*/
    public string Item_Info;
}
[Serializable]
public enum ItemType 
{
    Materials,//����
    Ingredient,//ʳ��
    Food,//ʳ��
    Weapon,//����
}