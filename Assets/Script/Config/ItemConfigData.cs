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
     * 1000 - 1999 材料
     * 2000 - 2999 工具
     * 3000 - 3999 食材
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Name = "原初木棍",Item_Desc = "",Item_CurCount = 1,Item_MaxCount = 9,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1001,Item_Name = "原木",Item_Desc = "",Item_CurCount = 1,Item_MaxCount = 9,Item_Type = ItemType.Materials,Average_Weight = 1,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2001,Item_Name = "木斧头",Item_Desc = "",Item_CurCount = 1,Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2002,Item_Name = "铁斧头",Item_Desc = "",Item_CurCount = 1,Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Weight = 1,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3001,Item_Name = "污染肉",Item_Desc = "",Item_CurCount = 1,Item_MaxCount = 9,Item_Type = ItemType.Ingredient,Average_Weight = 1,Average_Value = 1 },
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
    [SerializeField]/*当前数量*/
    public int Item_CurCount;
    [SerializeField]/*最大数量*/
    public int Item_MaxCount;
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