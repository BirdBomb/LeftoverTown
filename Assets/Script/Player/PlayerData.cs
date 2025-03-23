using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    public PlayerData()
    {
        Name = "无名氏";
        Hp_Cur = 100;
        Hp_Max = 100;
        Armor_Cur = 0;
        Food_Cur = 100;
        Food_Max = 100;
        Water_Cur = 5;
        San_Cur = 100;
        San_Max = 100;
        Happy_Cur = 5;
        Coin_Cur = 100;
        En_Cur = 3000;
        Speed_Common = 60;
        Speed_Max = 80;

        Point_Strength = 1;
        Point_Intelligence = 1;
        Point_SPower = 1;
        Point_Focus = 1;
        Point_Agility = 1;
        Point_Make = 1;
        Point_Build = 1;
        Point_Cook = 1;
    }
    /// <summary>
    /// 名字
    /// </summary>
    [SerializeField]
    public string Name;
    /// <summary>
    /// 常态速度
    /// </summary>
    [SerializeField]
    public short Speed_Common;
    /// <summary>
    /// 最大速度
    /// </summary>
    [SerializeField]
    public short Speed_Max;
    /// <summary>
    /// 耐力
    /// </summary>
    [SerializeField]
    public int En_Cur;
    /// <summary>
    /// 当前生命值
    /// </summary>
    [SerializeField]
    public short Hp_Cur;
    /// <summary>
    /// 最大生命值
    /// </summary>
    [SerializeField]
    public int Hp_Max;
    /// <summary>
    /// 护甲值
    /// </summary>
    [SerializeField]
    public short Armor_Cur;
    /// <summary>
    /// 当前食物值
    /// </summary>
    [SerializeField]
    public short Food_Cur;
    /// <summary>
    /// 最大食物值
    /// </summary>
    [SerializeField]
    public short Food_Max;
    /// <summary>
    /// 缺水值
    /// </summary>
    [SerializeField]
    public short Water_Cur;
    /// <summary>
    /// 当前精神值
    /// </summary>
    [SerializeField]
    public short San_Cur;
    /// <summary>
    /// 最大精神值
    /// </summary>
    [SerializeField]
    public short San_Max;
    /// <summary>
    /// 心情值
    /// </summary>
    [SerializeField]
    public short Happy_Cur;
    /// <summary>
    /// 金币
    /// </summary>
    [SerializeField]
    public short Coin_Cur;
    /// <summary>
    /// 体力
    /// </summary>
    [SerializeField]
    public short Point_Strength;
    /// <summary>
    /// 智力
    /// </summary>
    [SerializeField]
    public short Point_Intelligence;
    /// <summary>
    /// 法力
    /// </summary>
    [SerializeField]
    public short Point_SPower;
    /// <summary>
    /// 专注
    /// </summary>
    [SerializeField]
    public short Point_Focus;
    /// <summary>
    /// 敏捷
    /// </summary>
    [SerializeField]
    public short Point_Agility;
    /// <summary>
    /// 制造
    /// </summary>
    [SerializeField]
    public short Point_Make;
    /// <summary>
    /// 建造
    /// </summary>
    [SerializeField]
    public short Point_Build;
    /// <summary>
    /// 烹饪
    /// </summary>
    [SerializeField]
    public short Point_Cook;

    /// <summary>
    /// 背包物体
    /// </summary>
    [SerializeField]
    public List<ItemData> BagItems = new List<ItemData>();
    /// <summary>
    /// 手部物体
    /// </summary>
    [SerializeField]
    public ItemData HandItem;
    /// <summary>
    /// 头部物体
    /// </summary>
    [SerializeField]
    public ItemData HeadItem;
    /// <summary>
    /// 身体物体
    /// </summary>
    [SerializeField] 
    public ItemData BodyItem;

    /// <summary>
    /// 玩家学会的技能
    /// </summary>
    [SerializeField]
    public List<short> SkillKnowList = new List<short>();
    /// <summary>
    /// 玩家使用的技能
    /// </summary>
    [SerializeField]
    public List<short> SkillUseList = new List<short>();
    /// <summary>
    /// 玩家Buff列表
    /// </summary>
    [SerializeField]
    public List<short> BuffList = new List<short>();
    /// <summary>
    /// 玩家Buff点数
    /// </summary>
    [SerializeField]
    public int BuffPoint;
    /// <summary>
    /// 头发ID
    /// </summary>
    [SerializeField]
    public short Hair_ID;
    /// <summary>
    /// 头发颜色
    /// </summary>
    [SerializeField]
    public Color32 Hair_Color;
    /// <summary>
    /// 眼镜ID
    /// </summary>
    [SerializeField]
    public short Eye_ID;
}
