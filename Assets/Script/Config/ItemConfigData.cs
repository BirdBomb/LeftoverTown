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
     * 1000 - 1999 材料 1100一级材料1200二级材料
     * 2000 - 2999 工具
     * 3000 - 3999 食材
     * 4000 - 4999 食物
     * 5000 - 5999 衣物
     * 9000 - 9999 其他
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 0 ,},
        #region//1000-1999材料
        #region//1000-1099一级材料
        /*原木*/new ItemConfig(){ Item_ID = 1000,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*树枝*/new ItemConfig(){ Item_ID = 1001,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*干草*/new ItemConfig(){ Item_ID = 1002,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*石头*/new ItemConfig(){ Item_ID = 1010,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*煤炭*/new ItemConfig(){ Item_ID = 1011,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*金矿*/new ItemConfig(){ Item_ID = 1012,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gold },
        /*铁矿*/new ItemConfig(){ Item_ID = 1013,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*蓝晶*/new ItemConfig(){ Item_ID = 1014,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Blue },
        /*玉矿*/new ItemConfig(){ Item_ID = 1015,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Blue },
        /*硝矿*/new ItemConfig(){ Item_ID = 1016,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*红晶*/new ItemConfig(){ Item_ID = 1017,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Blue },
        /*白晶*/new ItemConfig(){ Item_ID = 1018,Item_Size = ItemSize.Gro,Item_Max = 20,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Blue },
        #endregion
        #region//1100-1199二级材料
        /*木材*/new ItemConfig(){ Item_ID = 1100,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*铁锭*/new ItemConfig(){ Item_ID = 1110,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*金锭*/new ItemConfig(){ Item_ID = 1111,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gold },
        /*能源石*/new ItemConfig(){ Item_ID = 1112,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Purple },
        /*绿宝石*/new ItemConfig(){ Item_ID = 1113,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 245,Item_Rarity = ItemRarity.Purple },
        /*硝石碎*/new ItemConfig(){ Item_ID = 1114,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*红宝石*/new ItemConfig(){ Item_ID = 1115,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 300,Item_Rarity = ItemRarity.Purple },
        /*蛋白石*/new ItemConfig(){ Item_ID = 1116,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Purple },
        #endregion
        #region//1200-1299三级材料
        /*机械元件*/new ItemConfig(){ Item_ID = 1200,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*电子元件*/new ItemConfig(){ Item_ID = 1201,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*枪械元件*/new ItemConfig(){ Item_ID = 1202,Item_Size = ItemSize.Gro,Item_Max = 40,Item_Type = ItemType.Mat,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #endregion
        #region//2000-2999工具
        #region//2000-2099纯工具
        /*火把*/new ItemConfig(){ Item_ID = 2000,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*木斧*/new ItemConfig(){ Item_ID = 2010,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*铁斧*/new ItemConfig(){ Item_ID = 2011,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*木镐*/new ItemConfig(){ Item_ID = 2020,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*铁镐*/new ItemConfig(){ Item_ID = 2021,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*木竿*/new ItemConfig(){ Item_ID = 2030,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2100-2199近战武器
        /*木棍*/new ItemConfig(){ Item_ID = 2100,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*长矛*/new ItemConfig(){ Item_ID = 2101,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*匕首*/new ItemConfig(){ Item_ID = 2102,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*铁剑*/new ItemConfig(){ Item_ID = 2103,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2200-2299远程武器
        /*粗木弓*/new ItemConfig(){ Item_ID = 2200,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*精木弓*/new ItemConfig(){ Item_ID = 2201,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*金质弓*/new ItemConfig(){ Item_ID = 2202,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2300-2399热武器
        /*土质手枪*/new ItemConfig(){ Item_ID = 2300,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*短冲锋枪*/new ItemConfig(){ Item_ID = 2301,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*木柄步枪*/new ItemConfig(){ Item_ID = 2302,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*精准手枪*/new ItemConfig(){ Item_ID = 2303,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*泵霰弹枪*/new ItemConfig(){ Item_ID = 2304,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*重型机枪*/new ItemConfig(){ Item_ID = 2305,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*精准步枪*/new ItemConfig(){ Item_ID = 2306,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2400-2499灵能武器
        #endregion
        #endregion
        #region//3000-3999食材
        #region//3000-3099果蔬
        /*鸟果*/new ItemConfig(){ Item_ID = 3000,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*黄金鸟果*/new ItemConfig(){ Item_ID = 3001,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 300,Item_Rarity = ItemRarity.Gray },
        /*水桔*/new ItemConfig(){ Item_ID = 3002,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 8,Item_Rarity = ItemRarity.Gray },
        /*薯果*/new ItemConfig(){ Item_ID = 3003,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*辣椒*/new ItemConfig(){ Item_ID = 3004,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//3100-3199肉蛋
        /*带皮肉*/new ItemConfig(){ Item_ID = 3100,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 20,Item_Rarity = ItemRarity.Gray },
        /*带骨肉*/new ItemConfig(){ Item_ID = 3101,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 22,Item_Rarity = ItemRarity.Gray },
        /*禽腿肉*/new ItemConfig(){ Item_ID = 3102,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 28,Item_Rarity = ItemRarity.Gray },
        /*内脏肉*/new ItemConfig(){ Item_ID = 3103,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        /*污染肉*/new ItemConfig(){ Item_ID = 3104,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*鲫鱼*/new ItemConfig(){ Item_ID = 3110,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//3200-3299其他
        /*面粉*/new ItemConfig(){ Item_ID = 3200,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        /*腐烂食物*/new ItemConfig(){ Item_ID = 3999,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//4000-4999食物
        #region//4000-4099烤制
        /*熟肉块*/new ItemConfig(){ Item_ID = 4001,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*熟肉排*/new ItemConfig(){ Item_ID = 4002,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*熟禽腿*/new ItemConfig(){ Item_ID = 4003,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*熟内脏*/new ItemConfig(){ Item_ID = 4004,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*臭熟肉*/new ItemConfig(){ Item_ID = 4005,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*熟蛇果*/new ItemConfig(){ Item_ID = 4006,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*熟水桔*/new ItemConfig(){ Item_ID = 4007,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*熟薯果*/new ItemConfig(){ Item_ID = 4008,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//4100-4199烹饪
        /*失败菜*/new ItemConfig(){ Item_ID = 4100,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*薯肉煲*/new ItemConfig(){ Item_ID = 4101,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*炖杂果*/new ItemConfig(){ Item_ID = 4102,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*手抓肉*/new ItemConfig(){ Item_ID = 4103,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*果渍烧*/new ItemConfig(){ Item_ID = 4104,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*水煮鱼*/new ItemConfig(){ Item_ID = 4105,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//药剂
        /*鸟果汁*/new ItemConfig(){ Item_ID = 4200,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        #endregion
        #endregion
        #region//5000-5999衣物
        #region//5000-5099帽子
        /*巡逻帽*/new ItemConfig(){ Item_ID = 5000,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*草帽*/new ItemConfig(){ Item_ID = 5001,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*防护盔*/new ItemConfig(){ Item_ID = 5002,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*矿工帽*/new ItemConfig(){ Item_ID = 5003,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*面罩*/new ItemConfig(){ Item_ID = 5004,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*铁盔*/new ItemConfig(){ Item_ID = 5005,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*熊皮帽*/new ItemConfig(){ Item_ID = 5006,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*木盔*/new ItemConfig(){ Item_ID = 5007,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        #endregion
        #region//5100-5199衣服
        /*巡逻衣*/new ItemConfig(){ Item_ID = 5100,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*灰布衣*/new ItemConfig(){ Item_ID = 5101,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*褐布衣*/new ItemConfig(){ Item_ID = 5102,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*防护衣*/new ItemConfig(){ Item_ID = 5103,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*铁甲*/new ItemConfig(){ Item_ID = 5104,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        /*木甲*/new ItemConfig(){ Item_ID = 5105,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 1,Item_Rarity = ItemRarity.Gray},
        #endregion
        #endregion
        #region//9000-9999其他
        #region//9000-9099弹药
        /*粗制木箭*/new ItemConfig(){ Item_ID = 9000,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Arrow,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*精致木箭*/new ItemConfig(){ Item_ID = 9001,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Arrow,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*铁制弹丸*/new ItemConfig(){ Item_ID = 9010,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Bullet,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*次元弹匣*/new ItemConfig(){ Item_ID = 9020,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Bullet,Item_Value = 6,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//9900-9999测试
        /*书页*/new ItemConfig(){ Item_ID = 9900,Item_Size = ItemSize.Sin,Item_Max = 9,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
        /*钥匙*/new ItemConfig(){ Item_ID = 9901,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Weapon,Item_Value = 1,Item_Rarity = ItemRarity.Gray },
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
    /// 编号
    /// </summary>
    [SerializeField]
    public short Item_ID;
    /// <summary>
    /// 最大值
    /// </summary>
    [SerializeField]
    public short Item_Max; 
    public ItemRarity Item_Rarity;
    [SerializeField]/*基础类*/
    public ItemType Item_Type;
    [SerializeField]/*尺寸类*/
    public ItemSize Item_Size;
    [SerializeField]/*价值*/
    public int Item_Value;
}
public enum ItemType 
{
    /// <summary>
    /// 默认
    /// </summary>
    Def,
    /// <summary>
    /// 材料
    /// </summary>
    Mat, 
    /// <summary>
    /// 食材
    /// </summary>
    Food,
    /// <summary>
    /// 食物
    /// </summary>
    Dishes,
    /// <summary>
    /// 武器
    /// </summary>
    Weapon,
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
public enum ItemSize
{
    /// <summary>
    /// 单体Single
    /// </summary>
    Sin,
    /// <summary>
    /// 堆叠Group
    /// </summary>
    Gro,
    /// <summary>
    /// 容器Container
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
    [SerializeField, Header("基本掉落物编号")]
    public short ID;
    [SerializeField, Header("最小掉落物数量")]
    public short CountMin;
    [SerializeField, Header("最大掉落物数量")]
    public short CountMax;
}
[Serializable]
public struct ExtraLootInfo
{
    [SerializeField, Header("额外掉落物编号")]
    public short ID;
    [SerializeField, Header("额外掉落物数量")]
    public short Count;
    [SerializeField, Header("额外掉落物权重(1/1000)")]
    public short Weight;
}
