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
     * 6000 - 6999 消耗品
     * 9000 - 9999 其他
     */
    public readonly static List<ItemConfig> itemConfigs = new List<ItemConfig>()
    {
        new ItemConfig(){ Item_ID = 0,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 0 ,},
        #region//1000-1999材料
        #region//1000-1099一级材料
        /*原木*/new ItemConfig(){ Item_ID = 1000,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*树枝*/new ItemConfig(){ Item_ID = 1001,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*干草*/new ItemConfig(){ Item_ID = 1002,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*花瓣*/new ItemConfig(){ Item_ID = 1003,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*竹蔗*/new ItemConfig(){ Item_ID = 1004,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 2,Item_Rarity = ItemRarity.Green },
        /*石头*/new ItemConfig(){ Item_ID = 1010,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*煤炭*/new ItemConfig(){ Item_ID = 1011,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*硝石*/new ItemConfig(){ Item_ID = 1012,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 150,Item_Rarity = ItemRarity.Gray },
        /*铜矿*/new ItemConfig(){ Item_ID = 1013,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 12,Item_Rarity = ItemRarity.Green },
        /*铁矿*/new ItemConfig(){ Item_ID = 1014,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 152,Item_Rarity = ItemRarity.Green },
        /*金矿*/new ItemConfig(){ Item_ID = 1015,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 154,Item_Rarity = ItemRarity.Gold },
        
        /*太阳碎片*/new ItemConfig(){ Item_ID = 1016,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 154,Item_Rarity = ItemRarity.Gold },
        
        /*骨头*/new ItemConfig(){ Item_ID = 1021,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*毛皮*/new ItemConfig(){ Item_ID = 1022,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*粪便*/new ItemConfig(){ Item_ID = 1023,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        
        /*太阳碎片*/new ItemConfig(){ Item_ID = 1030,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 100,Item_Rarity = ItemRarity.Gold },
        #endregion
        #region//1100-1199二级材料
        /*木材*/new ItemConfig(){ Item_ID = 1100,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*草纸*/new ItemConfig(){ Item_ID = 1101,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*煤粉*/new ItemConfig(){ Item_ID = 1111,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 20,Item_Rarity = ItemRarity.Green },
        /*硝粉*/new ItemConfig(){ Item_ID = 1112,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 20,Item_Rarity = ItemRarity.Green },
        /*铜锭*/new ItemConfig(){ Item_ID = 1113,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 20,Item_Rarity = ItemRarity.Blue },
        /*铁锭*/new ItemConfig(){ Item_ID = 1114,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 20,Item_Rarity = ItemRarity.Blue },
        /*金锭*/new ItemConfig(){ Item_ID = 1115,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 170,Item_Rarity = ItemRarity.Gold },
        #endregion
        #region//1200-1299三级材料
        /*机械元件*/new ItemConfig(){ Item_ID = 1200,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 24,Item_Rarity = ItemRarity.Gray },
        /*电子元件*/new ItemConfig(){ Item_ID = 1201,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 150,Item_Rarity = ItemRarity.Gray },
        /*枪械元件*/new ItemConfig(){ Item_ID = 1202,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Mat,Item_Value = 80,Item_Rarity = ItemRarity.Gray },
        #endregion
        #endregion
        #region//2000-2999工具
        #region//2000-2099纯工具
        /*火把*/new ItemConfig(){ Item_ID = 2000,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 8,Item_Rarity = ItemRarity.Gray },
        /*木斧*/new ItemConfig(){ Item_ID = 2010,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        /*铁斧*/new ItemConfig(){ Item_ID = 2011,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 18,Item_Rarity = ItemRarity.Green },
        /*伐斧*/new ItemConfig(){ Item_ID = 2012,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 22,Item_Rarity = ItemRarity.Blue },
        /*木镐*/new ItemConfig(){ Item_ID = 2020,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 120,Item_Rarity = ItemRarity.Gray },
        /*铁镐*/new ItemConfig(){ Item_ID = 2021,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 18,Item_Rarity = ItemRarity.Green },
        /*木竿*/new ItemConfig(){ Item_ID = 2030,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        /*木锄*/new ItemConfig(){ Item_ID = 2040,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 12,Item_Rarity = ItemRarity.Gray },
        /*木镰刀*/new ItemConfig(){ Item_ID = 2050,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 12,Item_Rarity = ItemRarity.Gray },
        /*木锤子*/new ItemConfig(){ Item_ID = 2060,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Tool,Item_Value = 12,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//2100-2199近战武器
        /*木棍*/new ItemConfig(){ Item_ID = 2100,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 15,Item_Rarity = ItemRarity.Gray },
        /*长矛*/new ItemConfig(){ Item_ID = 2101,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 24,Item_Rarity = ItemRarity.Green },
        /*匕首*/new ItemConfig(){ Item_ID = 2102,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 26,Item_Rarity = ItemRarity.Green },
        /*铁剑*/new ItemConfig(){ Item_ID = 2103,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 28,Item_Rarity = ItemRarity.Green },
        #endregion
        #region//2200-2299远程武器
        /*粗木弓*/new ItemConfig(){ Item_ID = 2200,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 10,Item_Rarity = ItemRarity.Green },
        /*精木弓*/new ItemConfig(){ Item_ID = 2201,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 20,Item_Rarity = ItemRarity.Green },
        /*金质弓*/new ItemConfig(){ Item_ID = 2202,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 160,Item_Rarity = ItemRarity.Gold },
        #endregion
        #region//2300-2399热武器
        /*土质手枪*/new ItemConfig(){ Item_ID = 2300,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 55,Item_Rarity = ItemRarity.Green },
        /*短冲锋枪*/new ItemConfig(){ Item_ID = 2301,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 80,Item_Rarity = ItemRarity.Blue },
        /*木柄步枪*/new ItemConfig(){ Item_ID = 2302,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 84,Item_Rarity = ItemRarity.Blue },
        /*精准手枪*/new ItemConfig(){ Item_ID = 2303,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 76,Item_Rarity = ItemRarity.Blue },
        /*泵霰弹枪*/new ItemConfig(){ Item_ID = 2304,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 88,Item_Rarity = ItemRarity.Blue },
        /*重型机枪*/new ItemConfig(){ Item_ID = 2305,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 106,Item_Rarity = ItemRarity.Blue },
        /*精准步枪*/new ItemConfig(){ Item_ID = 2306,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 142,Item_Rarity = ItemRarity.Purple },
        #endregion
        #region//2400-2499灵能武器
        /*火焰喷射器*/new ItemConfig(){ Item_ID = 2400,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 55,Item_Rarity = ItemRarity.Green },
        #endregion
        #region//2900-2999其他工具
        /*绳索*/new ItemConfig(){ Item_ID = 2900,Item_Size = ItemSize.Con,Item_Max = 1,Item_Type = ItemType.Weapon,Item_Value = 55,Item_Rarity = ItemRarity.Green },
        #endregion
        #endregion
        #region//3000-3999食材
        #region//3000-3099果蔬
        /*鸟果*/new ItemConfig(){ Item_ID = 3000,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Green },
        /*黄金鸟果*/new ItemConfig(){ Item_ID = 3001,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 300,Item_Rarity = ItemRarity.Gold },
        /*水桔*/new ItemConfig(){ Item_ID = 3002,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 8,Item_Rarity = ItemRarity.Green },
        /*薯果*/new ItemConfig(){ Item_ID = 3003,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 2,Item_Rarity = ItemRarity.Green },
        /*辣椒*/new ItemConfig(){ Item_ID = 3004,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Green },
        /*蘑菇*/new ItemConfig(){ Item_ID = 3005,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Green },
        /*莓果*/new ItemConfig(){ Item_ID = 3006,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Green },
        #endregion
        #region//3100-3199肉蛋
        /*带皮肉*/new ItemConfig(){ Item_ID = 3100,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        /*带骨肉*/new ItemConfig(){ Item_ID = 3101,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 12,Item_Rarity = ItemRarity.Gray },
        /*禽腿肉*/new ItemConfig(){ Item_ID = 3102,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 18,Item_Rarity = ItemRarity.Gray },
        /*内脏肉*/new ItemConfig(){ Item_ID = 3103,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 6,Item_Rarity = ItemRarity.Gray },
        /*污染肉*/new ItemConfig(){ Item_ID = 3104,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*鸡蛋*/new ItemConfig(){ Item_ID = 3105,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*鲫鱼*/new ItemConfig(){ Item_ID = 3110,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 12,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//3200-3299其他
        /*面粉*/new ItemConfig(){ Item_ID = 3200,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 8,Item_Rarity = ItemRarity.Gray },
        /*糖粉*/new ItemConfig(){ Item_ID = 3201,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        #endregion
        /*腐烂食物*/new ItemConfig(){ Item_ID = 3999,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//4000-4999食物
        #region//烤制
        /*熟肉块*/new ItemConfig(){ Item_ID = 4001,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 15,Item_Rarity = ItemRarity.Gray },
        /*熟肉排*/new ItemConfig(){ Item_ID = 4002,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 20,Item_Rarity = ItemRarity.Gray },
        /*熟禽腿*/new ItemConfig(){ Item_ID = 4003,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 20,Item_Rarity = ItemRarity.Gray },
        /*熟内脏*/new ItemConfig(){ Item_ID = 4004,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 12,Item_Rarity = ItemRarity.Gray },
        /*臭熟肉*/new ItemConfig(){ Item_ID = 4005,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 8,Item_Rarity = ItemRarity.Gray },
        /*熟蛇果*/new ItemConfig(){ Item_ID = 4006,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 12,Item_Rarity = ItemRarity.Gray },
        /*熟水桔*/new ItemConfig(){ Item_ID = 4007,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 12,Item_Rarity = ItemRarity.Gray },
        /*熟薯果*/new ItemConfig(){ Item_ID = 4008,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//佳肴
        /*失败菜*/new ItemConfig(){ Item_ID = 4100,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*薯果肉煲*/new ItemConfig(){ Item_ID = 4101,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 22,Item_Rarity = ItemRarity.Blue },
        /*苹果水桔煲*/new ItemConfig(){ Item_ID = 4102,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 20,Item_Rarity = ItemRarity.Blue },
        /*抓肉*/new ItemConfig(){ Item_ID = 4103,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 38,Item_Rarity = ItemRarity.Blue },
        /*果渍肉*/new ItemConfig(){ Item_ID = 4104,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 42,Item_Rarity = ItemRarity.Blue },
        /*鲜辣鲫鱼汤*/new ItemConfig(){ Item_ID = 4105,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Dishes,Item_Value = 36,Item_Rarity = ItemRarity.Blue },
        #endregion
        #region//药剂
        /*干净水*/new ItemConfig(){ Item_ID = 4200,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Gray },
        /*鸟果汁*/new ItemConfig(){ Item_ID = 4201,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//饮料
        /*小麦酒*/new ItemConfig(){ Item_ID = 4300,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//乱炖
        /*乱炖肉锅（大份）*/new ItemConfig(){ Item_ID = 4400,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        /*乱炖肉锅（鲜蔬）*/new ItemConfig(){ Item_ID = 4401,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        /*乱炖肉锅（果味）*/new ItemConfig(){ Item_ID = 4402,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        /*乱炖肉锅（海陆）*/new ItemConfig(){ Item_ID = 4403,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        
        /*乱炖海鲜（大份）*/new ItemConfig(){ Item_ID = 4410,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        /*乱炖海鲜（鲜蔬）*/new ItemConfig(){ Item_ID = 4411,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        /*乱炖海鲜（果味）*/new ItemConfig(){ Item_ID = 4412,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        
        /*乱拌水果（大份）*/new ItemConfig(){ Item_ID = 4420,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        /*乱拌水果（鲜蔬）*/new ItemConfig(){ Item_ID = 4421,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
    
        /*乱拌蔬菜（大份）*/new ItemConfig(){ Item_ID = 4430,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        
        /*乱炖（大份）*/new ItemConfig(){ Item_ID = 4499,Item_Size = ItemSize.Gro,Item_Max = 9,Item_Type = ItemType.Food,Item_Value = 16,Item_Rarity = ItemRarity.Green },
        #endregion

        #endregion
        #region//5000-5999衣物
        #region//5000-5499帽子 5000-5099物理战斗用 5100-5199魔法战斗用 5200-5299特殊用途 5300-5399装饰
        //------------//
        /*木草绳盔*/
        new ItemConfig(){ Item_ID = 5000,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*铜草绳盔*/new ItemConfig(){ Item_ID = 5001,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Green },
        /*铁草绳盔*/new ItemConfig(){ Item_ID = 5002,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*金皮革盔*/new ItemConfig(){ Item_ID = 5003,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Gold },
        /*防爆面罩*/new ItemConfig(){ Item_ID = 5004,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Purple },
        //------------//
        /*硝石草绳帽*/new ItemConfig(){ Item_ID = 5100,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Green },
        //------------//
        /*草帽*/new ItemConfig(){ Item_ID = 5200,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*矿灯头盔*/new ItemConfig(){ Item_ID = 5201,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Green },
        /*歹徒面罩*/new ItemConfig(){ Item_ID = 5202,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*治安官帽*/new ItemConfig(){ Item_ID = 5203,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Green },
        /*廉价耳机*/new ItemConfig(){ Item_ID = 5204,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*精品耳机*/new ItemConfig(){ Item_ID = 5205,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Purple },
        /*厨师帽*/new ItemConfig(){ Item_ID = 5206,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Green },
        /*皮革帽*/new ItemConfig(){ Item_ID = 5207,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Green },
        /*潜水面罩*/new ItemConfig(){ Item_ID = 5208,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Hat,Item_Value = 4,Item_Rarity = ItemRarity.Green },
        //------------//
        #endregion
        #region//5400-5799衣服 5400-5499物理战斗用 5500-5599魔法战斗用 5600-5699特殊用途 5700-5799装饰
        //------------//
        /*木草绳甲*/new ItemConfig(){ Item_ID = 5400,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*铜草绳甲*/new ItemConfig(){ Item_ID = 5401,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*铁草绳甲*/new ItemConfig(){ Item_ID = 5402,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*金皮革甲*/new ItemConfig(){ Item_ID = 5403,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        //------------//
        /*石灰护心甲*/new ItemConfig(){ Item_ID = 5500,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*金制护心甲*/new ItemConfig(){ Item_ID = 5501,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        //------------//
        /*治安官服*/new ItemConfig(){ Item_ID = 5600,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        //------------//
        /*布衣*/new ItemConfig(){ Item_ID = 5700,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Gray },
        /*高档西装(黑)*/new ItemConfig(){ Item_ID = 5701,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*高档西装(白)*/new ItemConfig(){ Item_ID = 5702,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*高档西装(紫)*/new ItemConfig(){ Item_ID = 5703,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*高档西装(褐)*/new ItemConfig(){ Item_ID = 5704,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*高档西装(灰)*/new ItemConfig(){ Item_ID = 5705,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*皮革背心*/new ItemConfig(){ Item_ID = 5706,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*白色布衫*/new ItemConfig(){ Item_ID = 5707,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        /*格背带衫*/new ItemConfig(){ Item_ID = 5708,Item_Size = ItemSize.Sin,Item_Max = 1,Item_Type = ItemType.Clothes,Item_Value = 4,Item_Rarity = ItemRarity.Blue },
        #endregion
        #region//5800-5999饰品
        #endregion

        #endregion
        #region//6000-6999消耗品
        /*生命水晶*/new ItemConfig(){ Item_ID = 6000,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 500,Item_Rarity = ItemRarity.Gray },
        /*薯果种子*/new ItemConfig(){ Item_ID = 6100,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*辣椒种子*/new ItemConfig(){ Item_ID = 6101,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*小麦种子*/new ItemConfig(){ Item_ID = 6102,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Food,Item_Value = 2,Item_Rarity = ItemRarity.Gray },
        /*土质手雷*/new ItemConfig(){ Item_ID = 6200,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Bullet,Item_Value = 20,Item_Rarity = ItemRarity.Gray },
        #endregion
        #region//9000-9999其他
        #region//9000-9099弹药
        /*粗制木箭*/
        new ItemConfig(){ Item_ID = 9000,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Arrow,Item_Value = 3,Item_Rarity = ItemRarity.Gray },
        /*精致木箭*/new ItemConfig(){ Item_ID = 9001,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Arrow,Item_Value = 5,Item_Rarity = ItemRarity.Gray },
        /*致伤木箭*/new ItemConfig(){ Item_ID = 9002,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Arrow,Item_Value = 8,Item_Rarity = ItemRarity.Green },
        /*信号木箭*/new ItemConfig(){ Item_ID = 9003,Item_Size = ItemSize.Gro,Item_Max = 99,Item_Type = ItemType.Arrow,Item_Value = 200,Item_Rarity = ItemRarity.Green },
        /*铁制弹丸*/new ItemConfig(){ Item_ID = 9010,Item_Size = ItemSize.Gro,Item_Max = 999,Item_Type = ItemType.Bullet,Item_Value = 10,Item_Rarity = ItemRarity.Gray },
        /*穿墙弹丸*/new ItemConfig(){ Item_ID = 9011,Item_Size = ItemSize.Gro,Item_Max = 999,Item_Type = ItemType.Bullet,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        #endregion
        #region//9700-9799书籍
        /*空白书本*/new ItemConfig(){ Item_ID = 9700,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Book,Item_Value = 10,Item_Rarity = ItemRarity.Blue },
        /*冒险故事集一*/new ItemConfig(){ Item_ID = 9701,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Book,Item_Value = 10,Item_Rarity = ItemRarity.Blue },
        /*冒险故事集二*/new ItemConfig(){ Item_ID = 9702,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Book,Item_Value = 10,Item_Rarity = ItemRarity.Blue },
        /*冒险故事集三*/new ItemConfig(){ Item_ID = 9703,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Book,Item_Value = 10,Item_Rarity = ItemRarity.Blue },
        #endregion
        #region//9800-9899录像带
        /*某人的家庭录像*/new ItemConfig(){ Item_ID = 9800,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.VHS,Item_Value = 10,Item_Rarity = ItemRarity.Blue },
        /*有点滑稽的视频*/new ItemConfig(){ Item_ID = 9801,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.VHS,Item_Value = 10,Item_Rarity = ItemRarity.Blue },
        /*有点恐怖的视频*/new ItemConfig(){ Item_ID = 9802,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.VHS,Item_Value = 10,Item_Rarity = ItemRarity.Blue },
        /*有点知识的视频*/new ItemConfig(){ Item_ID = 9803,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.VHS,Item_Value = 10,Item_Rarity = ItemRarity.Blue },
        /*有点感人的视频*/new ItemConfig(){ Item_ID = 9804,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.VHS,Item_Value = 10,Item_Rarity = ItemRarity.Blue},
        ///*十则寓言*/new ItemConfig(){ Item_ID = 9810,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        ///*午港奇闻*/new ItemConfig(){ Item_ID = 9811,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        ///*家庭煮夫*/new ItemConfig(){ Item_ID = 9812,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        ///*残暴树獭*/new ItemConfig(){ Item_ID = 9813,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        ///*某个会议纪要*/new ItemConfig(){ Item_ID = 9820,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        ///*未发行的断片*/new ItemConfig(){ Item_ID = 9821,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        ///*噤声*/new ItemConfig(){ Item_ID = 9822,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        ///*某人的自白*/new ItemConfig(){ Item_ID = 9823,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        ///*令人不适的录像带*/new ItemConfig(){ Item_ID = 9824,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        #endregion
        #region//9900-9999金融
        /*货币(正常)*/new ItemConfig(){ Item_ID = 9900,Item_Size = ItemSize.Gro,Item_Max = 999,Item_Type = ItemType.Def,Item_Value = 10,Item_Rarity = ItemRarity.Purple },
        /*货币(肮脏)*/new ItemConfig(){ Item_ID = 9901,Item_Size = ItemSize.Gro,Item_Max = 999,Item_Type = ItemType.Def,Item_Value = 7,Item_Rarity = ItemRarity.Purple },
        /*货币(标记)*/new ItemConfig(){ Item_ID = 9902,Item_Size = ItemSize.Gro,Item_Max = 999,Item_Type = ItemType.Def,Item_Value = 0,Item_Rarity = ItemRarity.Purple },
        /*回购合同*/new ItemConfig(){ Item_ID = 9910,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 1000,Item_Rarity = ItemRarity.Gold },
        /*天意彩票*/new ItemConfig(){ Item_ID = 9911,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 20,Item_Rarity = ItemRarity.Gray },
        /*雇佣合同*/new ItemConfig(){ Item_ID = 9912,Item_Size = ItemSize.Gro,Item_Max = 1,Item_Type = ItemType.Def,Item_Value = 2000,Item_Rarity = ItemRarity.Gold },
        #endregion
        #endregion
    };
    public static string Colour(string str, ItemRarity rarity)
    {
        switch (rarity) 
        {
            case ItemRarity.Gray:
                {
                    str = $"<color=#ffffff>{str}</color>";
                    break;
                }
            case ItemRarity.Green:
                {
                    str = $"<color=#43C743>{str}</color>";
                    break;
                }
            case ItemRarity.Blue:
                {
                    str = $"<color=#4487C7>{str}</color>";
                    break;
                }
            case ItemRarity.Purple:
                {
                    str = $"<color=#d507c6>{str}</color>";
                    break;
                }
            case ItemRarity.Gold:
                {
                    str = $"<color=#FF9D09>{str}</color>";
                    break;
                }
            case ItemRarity.Red:
                {
                    str = $"<color=#FF090E>{str}</color>";
                    break;
                }
            case ItemRarity.Rainbow:
                {
                    str = $"<color=#D59DD6>{str}</color>";
                    break;
                }
        }
        return str;
    }
    public static string Colour(string str, ItemQuality quality)
    {
        switch (quality)
        {
            case ItemQuality.Gray:
                {
                    str = $"<color=#ffffff>{str}</color>";
                    break;
                }
            case ItemQuality.Green:
                {
                    str = $"<color=#43C743>{str}</color>";
                    break;
                }
            case ItemQuality.Blue:
                {
                    str = $"<color=#4487C7>{str}</color>";
                    break;
                }
            case ItemQuality.Purple:
                {
                    str = $"<color=#d507c6>{str}</color>";
                    break;
                }
            case ItemQuality.Gold:
                {
                    str = $"<color=#FF9D09>{str}</color>";
                    break;
                }
            case ItemQuality.Red:
                {
                    str = $"<color=#FF090E>{str}</color>";
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    str = $"<color=#D59DD6>{str}</color>";
                    break;
                }
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
    /// <summary>
    /// 录像带
    /// </summary>
    VHS,
    /// <summary>
    /// 书籍
    /// </summary>
    Book,
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
/// <summary>
/// 物品稀有度
/// </summary>
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
/// <summary>
/// 物品品质
/// </summary>
public enum ItemQuality
{
    Gray,//平庸
    Green,//精良
    Blue,//出色
    Purple,//稀有
    Gold,//完美
    Red,//大师
    Rainbow,//英雄
}
public struct ItemRaw
{
    public short ID;
    public int Count;
    public ItemRaw(short id, int count)
    {
        ID = id;
        Count = count;
    }
}
public struct LootRandomInfo
{
    [SerializeField, Header("掉落物编号")]
    public short ID;
    [SerializeField, Header("最小掉落物数量")]
    public short CountMin;
    [SerializeField, Header("最大掉落物数量")]
    public short CountMax;
    [SerializeField, Header("掉落物权重(1/1000)")]
    public short Weight;

    public LootRandomInfo(short id, short min, short max, short weight)
    {
        ID = id;
        CountMin = min;
        CountMax = max;
        Weight = weight;
    }
}
public struct LootFixedInfo
{
    [SerializeField, Header("掉落物编号")]
    public short ID;
    [SerializeField, Header("最小掉落物数量")]
    public short CountMin;
    [SerializeField, Header("最大掉落物数量")]
    public short CountMax;
    public LootFixedInfo(short id, short min, short max)
    {
        ID = id;
        CountMin = min;
        CountMax = max;
    }
}
public struct LootInfo
{
    [SerializeField, Header("掉落物编号")]
    public short ID;
    [SerializeField, Header("最小掉落物数量")]
    public short CountMin;
    [SerializeField, Header("最大掉落物数量")]
    public short CountMax;
    [SerializeField, Header("掉落物权重(1/1000)")]
    public short Weight;
    public LootInfo(short id, short min, short max, short weight)
    {
        ID = id;
        CountMin = min;
        CountMax = max;
        Weight = weight;
    }
}
