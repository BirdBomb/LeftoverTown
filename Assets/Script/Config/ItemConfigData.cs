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
     * 4000 - 4999 容器
     * 5000 - 5999 衣物
     * 9000 - 9999 其他
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Name = "无",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1001,Item_Name = "原木",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Materials,Average_Value = 1 },
        //new ItemConfig(){ Item_ID = 1002,Item_Name = "原木板",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2001,Item_Name = "木斧头",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2002,Item_Name = "铁斧头",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2003,Item_Name = "粗制木弓",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2004,Item_Name = "火把",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2005,Item_Name = "精致木弓",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2006,Item_Name = "木棍",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2007,Item_Name = "长柄刀",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2008,Item_Name = "短柄刀",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2009,Item_Name = "短筒枪",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3000,Item_Name = "水",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 3001,Item_Name = "污染肉",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4001,Item_Name = "木碗",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4002,Item_Name = "玻璃瓶",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4003,Item_Name = "陶瓷瓶",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Container,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 5001,Item_Name = "新镇夜巡队队员衣",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Clothes,Average_Value = 1,HowDoILook = "新镇夜巡队队员" },
        new ItemConfig(){ Item_ID = 5002,Item_Name = "新镇夜巡队队员帽",Item_Desc = "",Item_MaxCount = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,HowDoILook = "新镇夜巡队队员"},
        new ItemConfig(){ Item_ID = 9001,Item_Name = "召唤契约",Item_Desc = "",Item_MaxCount = 9,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9002,Item_Name = "粗制木箭",Item_Desc = "",Item_MaxCount = 99,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9003,Item_Name = "精致木箭",Item_Desc = "",Item_MaxCount = 99,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9004,Item_Name = "弹丸",Item_Desc = "",Item_MaxCount = 99,Item_Type = ItemType.Weapon,Average_Value = 1 },
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
    [SerializeField]/*我看上去怎么样*/
    public string HowDoILook;
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
    /// <summary>
    /// 箭
    /// </summary>
    Arrow,
    /// <summary>
    /// 子弹
    /// </summary>
    Bullet,
    /// <summary>
    /// 帽子
    /// </summary>
    Hat,
    /// <summary>
    /// 衣服
    /// </summary>
    Clothes,
}