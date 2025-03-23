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
        new ItemConfig(){ Item_ID = 0,Item_Name = "空气",ItemRarity = 0,Item_Desc = "空气",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Default,Average_Value = 0 ,},
        #region//1000-1999材料
        #region//1000-1099一级材料
        new ItemConfig(){ Item_ID = 1000,Item_Name = "原木",ItemRarity = 0,Item_Desc = "砍伐树木得到的原木，加工成木材之后用来建造",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1001,Item_Name = "树枝",ItemRarity = 0,Item_Desc = "随处可见的树枝，加工一下可能会有用",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1010,Item_Name = "石头",ItemRarity = 0,Item_Desc = "坚硬的石头,除了用来建造还可以拿在手上攻击",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1011,Item_Name = "煤炭",ItemRarity = 0,Item_Desc = "平常但是又极度重要的资源",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1012,Item_Name = "金矿石",ItemRarity = 0,Item_Desc = "稀有且珍贵的矿石，用来冶炼金锭",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1013,Item_Name = "铁矿石",ItemRarity = 0,Item_Desc = "用来冶炼铁锭",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1014,Item_Name = "高能矿",ItemRarity = 0,Item_Desc = "稀有且珍贵的矿石，用来冶炼高能水晶",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1015,Item_Name = "翡翠原石",ItemRarity = 0,Item_Desc = "稀有且珍贵的矿石，用来加工成宝石",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1016,Item_Name = "硝矿石",ItemRarity = 0,Item_Desc = "用来冶炼硝石",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1017,Item_Name = "红水晶原石",ItemRarity = 0,Item_Desc = "稀有且珍贵的矿石，用来加工成宝石",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 300 },
        new ItemConfig(){ Item_ID = 1018,Item_Name = "蛋白原石",ItemRarity = 0,Item_Desc = "稀有且珍贵的矿石，用来加工成蛋白石",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        #endregion
        #region//1100-1199二级材料
        new ItemConfig(){ Item_ID = 1100,Item_Name = "木材",ItemRarity = 0,Item_Desc = "加工原木得到的木材",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1110,Item_Name = "铁锭",ItemRarity = 0,Item_Desc = "制造开始的第一步，以及之后的每一步",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1111,Item_Name = "金锭",ItemRarity = 0,Item_Desc = "价值极高的金锭，贸易中的硬通货",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1112,Item_Name = "高能水晶",ItemRarity = 0,Item_Desc = "蕴含巨大能量的水晶，可以作为高效动力源",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1113,Item_Name = "翡翠",ItemRarity = 0,Item_Desc = "价值极高的宝石，贸易中的硬通货",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 245 },
        new ItemConfig(){ Item_ID = 1114,Item_Name = "硝石碎",ItemRarity = 0,Item_Desc = "由硝石加工而来，虽具有能量但极其不稳定",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1115,Item_Name = "红水晶",ItemRarity = 0,Item_Desc = "价值极高的宝石，贸易中的硬通货",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 300 },
        new ItemConfig(){ Item_ID = 1116,Item_Name = "蛋白石",ItemRarity = 0,Item_Desc = "价值极高的宝石，贸易中的硬通货",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        #endregion
        #region//1200-1299三级材料
        new ItemConfig(){ Item_ID = 1200,Item_Name = "机械元件",ItemRarity = 0,Item_Desc = "稀有且珍贵的矿石，用来加工成蛋白石",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 1201,Item_Name = "高能芯片",ItemRarity = 0,Item_Desc = "稀有且珍贵的矿石，用来加工成蛋白石",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Materials,Average_Value = 1 },
        #endregion
        #endregion
        #region//2000-2999工具
        #region//2000-2099纯工具
        new ItemConfig(){ Item_ID = 2000,Item_Name = "火把",ItemRarity = 0,Item_Desc = "用来照亮，容易熄灭所以无法挥击",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2010,Item_Name = "木斧头",ItemRarity = 0,Item_Desc = "木制的斧头，不太耐用但是易于制造",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2011,Item_Name = "铁斧头",ItemRarity = 0,Item_Desc = "铁制的斧头，极具性价比",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2020,Item_Name = "木镐",ItemRarity = 0,Item_Desc = "用来开采矿物的工具",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2021,Item_Name = "铁镐",ItemRarity = 0,Item_Desc = "用来开采矿物的工具",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2030,Item_Name = "木头鱼竿",ItemRarity = 0,Item_Desc = "相对于以前,现在你一天可以抓到两条鱼",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Tool,Average_Value = 1 },
        #endregion
        #region//2100-2199近战武器
        new ItemConfig(){ Item_ID = 2100,Item_Name = "木棍",ItemRarity = 0,Item_Desc = "木头制成的长棍，杀伤力有限经常用作训练使用",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2101,Item_Name = "长柄钢刀",ItemRarity = 0,Item_Desc = "木棍与金属的简单结合，极具性价比",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2102,Item_Name = "精钢匕首",ItemRarity = 0,Item_Desc = "小巧的匕首，高杀伤力和隐蔽性适用于近距离偷袭",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2103,Item_Name = "宽刃钢刀",ItemRarity = 0,Item_Desc = "更长更锋利的匕首，拿出它的时候就意味着你做好了准备",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1.5f,Item_Type = ItemType.Weapon,Average_Value = 1 },
        #endregion
        #region//2200-2299远程武器
        new ItemConfig(){ Item_ID = 2200,Item_Name = "粗制木弓",ItemRarity = 0,Item_Desc = "勉强可以使用的木弓",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 10,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2201,Item_Name = "精致木弓",ItemRarity = 0,Item_Desc = "木弓的进阶版，比粗制木弓更精准更顺手",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 10,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2202,Item_Name = "金质弓",ItemRarity = 1,Item_Desc = "属性与精致木弓基本相同，但纯金打造使其价格高昂",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 12,Item_Type = ItemType.Weapon,Average_Value = 1 },
        #endregion
        #region//2300-2399热武器
        new ItemConfig(){ Item_ID = 2300,Item_Name = "土质手枪",ItemRarity = 1,Item_Desc = "非法自制武器，无论是射速还是精准度都极其有限",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 12,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2301,Item_Name = "短型冲锋枪",ItemRarity = 2,Item_Desc = "拥有极高的射速和极高的弹道发散，往往用作短距离压制使用",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2302,Item_Name = "制式自动步枪",ItemRarity = 2,Item_Desc = "同时兼具精准度和压制力的枪械",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 15,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2303,Item_Name = "制式手枪",ItemRarity = 2,Item_Desc = "小巧的单手枪械,拥有优秀的精准度和射速但弹容量偏小。冲突升级前的最后方案",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2304,Item_Name = "泵动霰弹枪",ItemRarity = 2,Item_Desc = "解决纷争只需要三步。靠近，射击，然后在回声里享受胜利",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 7,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2305,Item_Name = "麦德森机枪",ItemRarity = 2,Item_Desc = "上部供弹的独特供弹方式保证了高弹容量，但是由于遮挡视野牺牲了枪械的精准度。往往在大规模冲突中用作压制",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 20,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 2306,Item_Name = "精准自动步枪",ItemRarity = 3,Item_Desc = "制作精良的突击步枪，特殊构造的握把保证了连续射击下枪械的稳定性",
            Item_Size = ItemSize.AsContainer,Item_MaxCount = 1,Attack_Distance = 20,Item_Type = ItemType.Weapon,Average_Value = 1 },
        #endregion
        #region//2400-2499灵能武器
        #endregion
        #endregion
        #region//3000-3999食材
        #region//3000-3099果蔬
        new ItemConfig(){ Item_ID = 3000,Item_Name = "鸟果",ItemRarity = 0,Item_Desc = "红色的果实，除了涩口没有其他味道，只有鸟儿喜欢吃，所以得名鸟果",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 5 },
        new ItemConfig(){ Item_ID = 3001,Item_Name = "黄金鸟果",ItemRarity = 3,Item_Desc = "十分稀有的鸟果，只有长在特定地方的果树才有极低可能性产出黄金鸟果",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 300 },
        new ItemConfig(){ Item_ID = 3002,Item_Name = "水桔",ItemRarity = 0,Item_Desc = "多汁的果子，酸涩略微带一点甜味，十分解渴",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 8 },
        new ItemConfig(){ Item_ID = 3003,Item_Name = "薯果",ItemRarity = 0,Item_Desc = "带有丰富淀粉的果实，没什么味道但是十分管饱，常常被当成主食种植",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 2 },
        new ItemConfig(){ Item_ID = 3004,Item_Name = "辣椒",ItemRarity = 0,Item_Desc = "一种辣椒",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 5 },
        #endregion
        #region//3100-3199肉蛋
        new ItemConfig(){ Item_ID = 3100,Item_Name = "带皮肉",ItemRarity = 0,Item_Desc = "带有皮的肉，油脂更加丰富",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 20 },
        new ItemConfig(){ Item_ID = 3101,Item_Name = "带骨肉",ItemRarity = 0,Item_Desc = "带有骨的肉，口感更加紧致",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 22 },
        new ItemConfig(){ Item_ID = 3102,Item_Name = "禽腿肉",ItemRarity = 0,Item_Desc = "禽类掉落的腿肉",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 28 },
        new ItemConfig(){ Item_ID = 3103,Item_Name = "内脏",ItemRarity = 0,Item_Desc = "一些零碎的内脏，虽说是肉类但是味道和口感都难以与正常肉类比拟",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 10 },
        new ItemConfig(){ Item_ID = 3104,Item_Name = "污染肉",ItemRarity = 0,Item_Desc = "被污染的肉，食用会带来严重后果",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 5 },
        new ItemConfig(){ Item_ID = 3110,Item_Name = "鲫鱼",ItemRarity = 0,Item_Desc = "一种十分常见的鱼",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 10 },
        #endregion
        #region//3200-3299其他
        new ItemConfig(){ Item_ID = 3200,Item_Name = "面粉",ItemRarity = 0,Item_Desc = "研制小麦而制成的面粉，各种面食的原料",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        new ItemConfig(){ Item_ID = 3999,Item_Name = "腐烂食物",ItemRarity = 0,Item_Desc = "腐烂掉的食物",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #region//4000-4999食物
        #region//4000-4099烤制
        new ItemConfig(){ Item_ID = 4001,Item_Name = "熟肉块",ItemRarity = 1,Item_Desc = "烤至滋滋冒油的带皮肉，因为本身油脂丰富所以吃上去饱满多汁",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4002,Item_Name = "熟肉排",ItemRarity = 1,Item_Desc = "烤制的带骨肉，握着骨头咬上一大口很满足",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4003,Item_Name = "熟禽腿",ItemRarity = 1,Item_Desc = "烤至滋滋冒油的带皮肉，因为本身油脂丰富所以吃上去饱满多汁",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4004,Item_Name = "熟内脏",ItemRarity = 1,Item_Desc = "烤制的带骨肉，握着骨头咬上一大口很满足",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4005,Item_Name = "异味熟肉",ItemRarity = 1,Item_Desc = "烤至滋滋冒油的带皮肉，因为本身油脂丰富所以吃上去饱满多汁",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4006,Item_Name = "熟蛇果",ItemRarity = 1,Item_Desc = "烤制的带骨肉，握着骨头咬上一大口很满足",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4007,Item_Name = "熟水桔",ItemRarity = 1,Item_Desc = "烤至滋滋冒油的带皮肉，因为本身油脂丰富所以吃上去饱满多汁",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4008,Item_Name = "熟薯果",ItemRarity = 1,Item_Desc = "烤制的带骨肉，握着骨头咬上一大口很满足",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #region//4100-4199烹饪
        new ItemConfig(){ Item_ID = 4100,Item_Name = "失败菜肴",ItemRarity = 0,Item_Desc = "虽然烹饪失败，不过不是不能吃",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4101,Item_Name = "薯果肉煲",ItemRarity = 2,Item_Desc = "由薯果和肉制成的热气腾腾的菜肴，过久的烹饪时间是肉质软烂薯果松软的原因",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4102,Item_Name = "炖杂果",ItemRarity = 2,Item_Desc = "水果制成的菜肴，甜腻温热的水果让有些人不太习惯，往往是为了处理不新鲜的水果",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4103,Item_Name = "抓肉",ItemRarity = 2,Item_Desc = "由带皮肉和带骨肉制成的简单肉食，不添加任何其他佐料的存粹肉食",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4104,Item_Name = "果渍风味烧",ItemRarity = 3,Item_Desc = "由水果调味的薄片烤肉，因为沁满了烧制浓稠的果汁所以烤肉看上去晶莹剔透，需用新鲜水果",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 4105,Item_Name = "水煮鱼",ItemRarity = 3,Item_Desc = "加入大量辣椒的鱼片汤",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #region//药剂
        new ItemConfig(){ Item_ID = 4200,Item_Name = "鸟果汁",ItemRarity = 0,Item_Desc = "鸟果榨出来的果汁，不知为何变得比果实更甜了",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 9,Attack_Distance = 0,Item_Type = ItemType.Ingredient,Average_Value = 1 },
        #endregion
        #endregion
        #region//5000-5999衣物
        #region//5000-5099帽子
        new ItemConfig(){ Item_ID = 5000,Item_Name = "巡逻帽",ItemRarity = 2,Item_Desc = "治安官套装之一，不提供护甲值",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5001,Item_Name = "草帽",ItemRarity = 0,Item_Desc = "提供些许遮阳功能",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5002,Item_Name = "防护盔",ItemRarity = 3,Item_Desc = "头盔侧面的红色抓痕是佩戴者抵御丧尸的证明",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5003,Item_Name = "矿工帽",ItemRarity = 0,Item_Desc = "没什么比它更有用了",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5004,Item_Name = "面罩",ItemRarity = 1,Item_Desc = "哪有好人成天戴着个面罩啊",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5005,Item_Name = "铁盔",ItemRarity = 2,Item_Desc = "铁制的头盔，可以抵御伤害",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5006,Item_Name = "熊皮帽",ItemRarity = 2,Item_Desc = "就跟真的似的",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Hat,Average_Value = 1 ,},
        #endregion
        #region//5100-5199衣服
        new ItemConfig(){ Item_ID = 5100,Item_Name = "巡逻衣",ItemRarity = 2,Item_Desc = "治安官套装之一，不提供护甲值",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5101,Item_Name = "布制衣服(灰)",ItemRarity = 0,Item_Desc = "灰色的布制衣服",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5102,Item_Name = "布制衣服(褐)",ItemRarity = 0,Item_Desc = "褐色的布制衣服",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5103,Item_Name = "防护衣",ItemRarity = 3,Item_Desc = "可以抵御一定程度的攻击",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        new ItemConfig(){ Item_ID = 5104,Item_Name = "铁甲",ItemRarity = 2,Item_Desc = "可以抵御一定程度的攻击",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 1,Attack_Distance = 1,Item_Type = ItemType.Clothes,Average_Value = 1 ,},
        #endregion
        #endregion
        #region//9000-9999其他
        #region//9000-9099弹药
        new ItemConfig(){ Item_ID = 9000,Item_Name = "粗制木箭",ItemRarity = 0,Item_Desc = "简单用树枝制成的弓箭，杀伤力有限",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Arrow,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9001,Item_Name = "精致木箭",ItemRarity = 0,Item_Desc = "用树枝制成的弓箭，加装金属箭头提高速度与杀伤力",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Arrow,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9010,Item_Name = "弹丸",ItemRarity = 0,Item_Desc = "受管控的工业子弹，省着点用",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Bullet,Average_Value = 1 },
        #endregion
        #region//9900-9999测试
        new ItemConfig(){ Item_ID = 9900,Item_Name = "神秘书页",ItemRarity = 6,Item_Desc = "从某本书上撕下的神秘纸张，念出上述咒语召唤造物",
            Item_Size = ItemSize.AsOne,Item_MaxCount = 9,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        new ItemConfig(){ Item_ID = 9901,Item_Name = "钥匙",ItemRarity = 0,Item_Desc = "用这把钥匙去锁上一个没有被锁上的门",
            Item_Size = ItemSize.AsGroup,Item_MaxCount = 99,Attack_Distance = 1,Item_Type = ItemType.Weapon,Average_Value = 1 },
        #endregion
#endregion
    };
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
    /// 名字
    /// </summary>
    [SerializeField]
    public string Item_Name;
    /// <summary>
    /// 描述
    /// </summary>
    [SerializeField]
    public string Item_Desc;

    /// <summary>
    /// 最大值
    /// </summary>
    [SerializeField]
    public short Item_MaxCount;
    /// <summary>
    /// 物品稀有度
    /// </summary>
    public int ItemRarity;
    [SerializeField]/*基础类*/
    public ItemType Item_Type;
    [SerializeField]/*尺寸类*/
    public ItemSize Item_Size;
    [SerializeField]/*攻击最大距离*/
    public float Attack_Distance;
    [SerializeField]/*价值*/
    public float Average_Value;
}
[Serializable]
public enum ItemType 
{
    /// <summary>
    /// 默认
    /// </summary>
    Default,
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
[Serializable]
public enum ItemSize
{
    /// <summary>
    /// 单体类
    /// </summary>
    AsOne,
    /// <summary>
    /// 可堆叠
    /// </summary>
    AsGroup,
    /// <summary>
    /// 可容纳
    /// </summary>
    AsContainer,
}
