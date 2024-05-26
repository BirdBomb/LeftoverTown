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
     * 9000 - 9999 其他
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Name = "无",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1001,Item_Name = "原木",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Materials,Average_Value = 1 },
        //new ItemConfig(){ Item_ID = 1002,Item_Name = "原木板",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2001,Item_Name = "木斧头",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2002,Item_Name = "铁斧头",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2003,Item_Name = "木弓",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2004,Item_Name = "火把",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3000,Item_Name = "水",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3001,Item_Name = "污染肉",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4001,Item_Name = "木碗",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4002,Item_Name = "玻璃瓶",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4003,Item_Name = "陶瓷瓶",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9001,Item_Name = "召唤契约",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9002,Item_Name = "木箭",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Weapon,Average_Value = 1 },
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
    [SerializeField]/*最大数量*/
    public int Item_MaxCount;
    [SerializeField]/*类别*/
    public ItemType Item_Type;
    [SerializeField]/*价值*/
    public float Average_Value;
}
[Serializable]
public enum ItemType 
{
    /// <summary>
    /// 材料
    /// </summary>
    Materials,
    /// <summary>
    /// 食材
    /// </summary>
    Ingredient,
    /// <summary>
    /// 食物
    /// </summary>
    Food,
    /// <summary>
    /// 武器
    /// </summary>
    Weapon,
    /// <summary>
    /// 工具
    /// </summary>
    Tool,
    /// <summary>
    /// 容器
    /// </summary>
    Container,

}