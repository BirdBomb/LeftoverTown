using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConfigData : MonoBehaviour
{
    /*
     * 1000 - 1999 工具
     * 2000 - 2999 材料
     * 
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Name = "原初木棍",Item_Desc = "",Item_Count = 0,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2001,Item_Name = "木斧头",Item_Desc = "",Item_Count = 0,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2002,Item_Name = "铁斧头",Item_Desc = "",Item_Count = 0,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
    };
}
[Serializable]
public struct ItemConfig
{
    [SerializeField]/*编号*/
    public int Item_ID;
    [SerializeField]/*名字*/
    public string Item_Name;
    [SerializeField]/*描述*/
    public string Item_Desc;
    [SerializeField]/*数量*/
    public int Item_Count;
    [SerializeField]/*类别*/
    public ItemType Item_Type;
    [SerializeField]/*重量*/
    public float Average_Weight;
    [SerializeField]/*价值*/
    public float Average_Value;
    [SerializeField]/*信息*/
    public string Item_Info;
}
[Serializable]
public enum ItemType 
{
    Materials,//材料
    Ingredient,//食材
    Food,//食物
    Weapon,//武器
}