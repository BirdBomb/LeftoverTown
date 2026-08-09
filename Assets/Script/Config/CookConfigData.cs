
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookConfigData 
{
    public readonly static List<MasterpieceConfig> masterpieceConfigs = new List<MasterpieceConfig>()
    {
        /*薯果肉煲*/
        new MasterpieceConfig(){Item_ID = 4101,Item_Count = 1,
            Raw_0=new List<short>(){ 3003},
            Raw_1=new List<short>(){ 3100, 3101, 3102, 3103},
            },
        /*苹果水桔煲*/
        new MasterpieceConfig(){Item_ID = 4102,Item_Count = 1,
            Raw_0=new List<short>(){ 3000 },
            Raw_1=new List<short>(){ 3002 },
            },
        /*抓肉*/
        new MasterpieceConfig(){Item_ID = 4103,Item_Count = 1,
            Raw_0=new List<short>(){ 3100},
            Raw_1=new List<short>(){ 3101},
            },
        /*果渍肉*/
        new MasterpieceConfig(){Item_ID = 4104,Item_Count = 1,
            Raw_0=new List<short>(){ 3100, 3101},
            Raw_AllNeedType=new List<FoodType>(){ FoodType.Fruit,FoodType.Meat },
            },
        /*鲜辣鲫鱼汤*/
        new MasterpieceConfig(){Item_ID = 4105,Item_Count = 1,
            Raw_0=new List<short>(){ 3110},
            Raw_1=new List<short>(){ 3004},
            },
    };
    public readonly static List<StandardConfig> standardConfigs = new List<StandardConfig>()
    {
        new StandardConfig(){Item_ID = 4400,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Meat },
            },
        new StandardConfig(){Item_ID = 4401,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Meat,FoodType.Vegetable },
            },
        new StandardConfig(){Item_ID = 4402,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Meat, FoodType.Fruit },
            },
        new StandardConfig(){Item_ID = 4403,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Meat, FoodType.Fish },
            },

        new StandardConfig(){Item_ID = 4410,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Fish },
            },
        new StandardConfig(){Item_ID = 4411,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Fish, FoodType.Vegetable },
            },
        new StandardConfig(){Item_ID = 4412,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Fish, FoodType.Fruit },
            },

        new StandardConfig(){Item_ID = 4420,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Fruit },
            },
        new StandardConfig(){Item_ID = 4421,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Fruit, FoodType.Vegetable },
            },

        new StandardConfig(){Item_ID = 4430,Item_Count = 1,
            Raw_Type=new List<FoodType>(){ FoodType.Vegetable },
            },

    };
    public static readonly Dictionary<short, FoodType> foodTypeMap = new Dictionary<short, FoodType>()
    {
        { 3000, FoodType.Fruit },  // 蛇果
        { 3001, FoodType.Fruit },  // 金蛇果
        { 3002, FoodType.Fruit },  // 桔子
        { 3003, FoodType.Vegetable },  // 薯果/蔬菜
        { 3004, FoodType.Vegetable },  // 辣椒/蔬菜
        { 3005, FoodType.Vegetable },  // 沼泽菇/蔬菜
        { 3006, FoodType.Fruit },  // 莓果
    
        { 3100, FoodType.Meat },//带皮肉
        { 3101, FoodType.Meat },//带骨肉
        { 3102, FoodType.Meat },//鸡腿
        { 3103, FoodType.Meat },//内脏
        { 3104, FoodType.Other },//臭肉
        { 3105, FoodType.Other },//鸡蛋

        { 3200, FoodType.Other },//面粉
        { 3201, FoodType.Other },//糖粉
    
        { 3110, FoodType.Fish },//鲫鱼
};
    public static short Cook(short itemID_Raw0, short itemID_Raw1, short itemID_Raw2)
    {
        short foodID = 4499;
        // 将输入的食材ID放入列表
        List<short> inputRaws = new List<short>() { itemID_Raw0, itemID_Raw1, itemID_Raw2 };

        // 第一步：尝试匹配精致菜肴
        foreach (var masterpiece in masterpieceConfigs)
        {
            if (IsMasterpieceMatch(masterpiece, inputRaws))
            {
                return masterpiece.Item_ID;
            }
        }

        // 第二步：没有精致菜肴，根据食材类型匹配次级菜肴
        // 通过外部方法获取食材的食物类型
        List<FoodType> inputFoodTypes = new List<FoodType>();
        foreach (var rawId in inputRaws)
        {
            // 假设外部方法叫 GetFoodType，需要你去实现
            FoodType foodType = GetFoodType(rawId);
            if (!inputFoodTypes.Contains(foodType))
            {
                inputFoodTypes.Add(foodType);
            }
        }

        // 对食物类型排序，确保匹配的一致性
        inputFoodTypes.Sort();

        // 在标准配置中查找匹配
        foreach (var standard in standardConfigs)
        {
            if (IsStandardMatch(standard, inputRaws))
            {
                return standard.Item_ID;
            }
        }

        return foodID; // 没有匹配的返回0
    }
    //private static bool IsMasterpieceMatch(MasterpieceConfig config, List<short> rawMaterials)
    //{
    //    // 检查材料数量是否匹配（特殊菜通常是3种，但也有可能是2种）
    //    int requiredSlots = 0;
    //    if (config.Raw_0 != null && config.Raw_0.Count > 0) requiredSlots++;
    //    if (config.Raw_1 != null && config.Raw_1.Count > 0) requiredSlots++;
    //    if (config.Raw_2 != null && config.Raw_2.Count > 0) requiredSlots++;

    //    if (rawMaterials.Count != requiredSlots) return false;

    //    // 检查每个槽位的匹配
    //    List<List<short>> slotRequirements = new List<List<short>>();
    //    if (config.Raw_0 != null) slotRequirements.Add(config.Raw_0);
    //    if (config.Raw_1 != null) slotRequirements.Add(config.Raw_1);
    //    if (config.Raw_2 != null) slotRequirements.Add(config.Raw_2);

    //    // 这里需要一个排列组合匹配，因为玩家放入的顺序可能不同
    //    return IsPermutationMatch(slotRequirements, rawMaterials);
    //}
    // 辅助方法：检查精致菜肴是否匹配
    private static bool IsMasterpieceMatch(MasterpieceConfig config, List<short> inputRaws)
    {
        // 用于标记已分类的食材
        List<bool> classified = new List<bool>(new bool[inputRaws.Count]);
        // 第一步：标记所有属于 Raw_0 的食材
        if (config.Raw_0 != null && config.Raw_0.Count > 0)
        {
            bool hasRaw0 = false;
            for (int i = 0; i < inputRaws.Count; i++)
            {
                if (!classified[i] && config.Raw_0.Contains(inputRaws[i]))
                {
                    classified[i] = true;
                    hasRaw0 = true;
                }
            }
            if (!hasRaw0) return false; // 必须至少有一个 Raw_0 食材
        }
        // 第二步：标记所有属于 Raw_1 的食材
        if (config.Raw_1 != null && config.Raw_1.Count > 0)
        {
            bool hasRaw1 = false;
            for (int i = 0; i < inputRaws.Count; i++)
            {
                if (!classified[i] && config.Raw_1.Contains(inputRaws[i]))
                {
                    classified[i] = true;
                    hasRaw1 = true;
                }
            }
            if (!hasRaw1) return false; // 必须至少有一个 Raw_1 食材
        }
        // 第三步：标记所有属于 Raw_3 的食材
        if (config.Raw_2 != null && config.Raw_2.Count > 0)
        {
            bool hasRaw2 = false;
            for (int i = 0; i < inputRaws.Count; i++)
            {
                if (!classified[i] && config.Raw_2.Contains(inputRaws[i]))
                {
                    classified[i] = true;
                    hasRaw2 = true;
                }
            }
            if (!hasRaw2) return false; // 必须至少有一个 Raw_2 食材
        }
        // Raw_Type 检查所有食材的整体类型
        if (config.Raw_AllNeedType != null && config.Raw_AllNeedType.Count > 0)
        {
            // 获取所有食材的类型
            List<FoodType> allTypes = new List<FoodType>();
            for (int i = 0; i < inputRaws.Count; i++)
            {
                FoodType foodType = GetFoodType(inputRaws[i]);
                if (!allTypes.Contains(foodType))
                {
                    allTypes.Add(foodType);
                }
            }

            // 排序后比较，确保类型集合完全一致
            allTypes.Sort();
            List<FoodType> requiredTypes = new List<FoodType>(config.Raw_AllNeedType);
            requiredTypes.Sort();

            if (allTypes.Count != requiredTypes.Count)
            {
                return false;
            }

            for (int i = 0; i < allTypes.Count; i++)
            {
                if (allTypes[i] != requiredTypes[i])
                {
                    Debug.Log(allTypes[i] + "/" + requiredTypes[i]);
                    return false;
                }
            }
            // 类型检查通过，标记所有食材为已分类
            for (int i = 0; i < inputRaws.Count; i++)
            {
                classified[i] = true;
            }
        }


        // 所有食材都被正确分类，没有剩余的
        return !classified.Contains(false);
    }
    private static bool IsStandardMatch(StandardConfig config, List<short> inputRaws)
    {
        if (config.Raw_Type == null || config.Raw_Type.Count == 0)
            return false;

        List<FoodType> rawsTypes = new List<FoodType>();
        foreach (short inputRaw in inputRaws)
        {
            FoodType foodType = GetFoodType(inputRaw);
            if (!rawsTypes.Contains(foodType))
            {
                rawsTypes.Add(foodType);
            }
        }

        // 排序
        List<FoodType> configTypes = new List<FoodType>(config.Raw_Type);
        configTypes.Sort();
        rawsTypes.Sort();

        // 比较数量和内容
        if (rawsTypes.Count != configTypes.Count)
            return false;

        for (int i = 0; i < configTypes.Count; i++)
        {
            if (rawsTypes[i] != configTypes[i])
                return false;
        }

        return true;
    }
    public static FoodType GetFoodType(short id)
    {
        if (foodTypeMap.ContainsKey(id)) { return foodTypeMap[id]; }
        return FoodType.Other;
    }
}
public enum FoodType
{
    All,
    Meat,
    Vegetable,
    Fruit,
    Fish,
    Other,
}
/// <summary>
/// 杰作菜
/// </summary>
public struct MasterpieceConfig
{
    [SerializeField]
    public short Item_ID;
    [SerializeField]
    public short Item_Count;
    /// <summary>
    /// 必须原材料1
    /// </summary>
    public List<short> Raw_0;
    /// <summary>
    /// 必须原材料2
    /// </summary>
    public List<short> Raw_1;
    /// <summary>
    /// 必须原材料3
    /// </summary>
    public List<short> Raw_2;
    /// <summary>
    /// 必须完全满足的种类
    /// </summary>
    public List<FoodType> Raw_AllNeedType;
}
/// <summary>
/// 次级菜
/// </summary>
public struct StandardConfig
{
    [SerializeField]
    public short Item_ID;
    [SerializeField]
    public short Item_Count;
    /// <summary>
    /// 必须种类
    /// </summary>
    public List<FoodType> Raw_Type;

}
public struct FoodTypeConfig
{
    [SerializeField]
    public short Item_ID;
    [SerializeField]
    public FoodType Food_Type;
}
