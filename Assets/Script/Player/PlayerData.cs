using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    public PlayerData()
    {
        Name = "无名氏";
        CurHp = 100;
        MaxHp = 100;
        Armor = 0;
        CurFood = 100;
        MaxFood = 100;
        Water = 5;
        CurSan = 100;
        MaxSan = 100;
        Happy = 5;
        Coin = 100;
        En = 3000;
        CommonSpeed = 20;
        MaxSpeed = 40;

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
    /// 位置
    /// </summary>
    [SerializeField]
    public Vector3Int Pos;
    /// <summary>
    /// 常态速度
    /// </summary>
    [SerializeField]
    public short CommonSpeed;
    /// <summary>
    /// 最大速度
    /// </summary>
    [SerializeField]
    public short MaxSpeed;
    /// <summary>
    /// 耐力
    /// </summary>
    [SerializeField]
    public int En;
    /// <summary>
    /// 当前生命值
    /// </summary>
    [SerializeField]
    public short CurHp;
    /// <summary>
    /// 最大生命值
    /// </summary>
    [SerializeField]
    public int MaxHp;
    /// <summary>
    /// 护甲值
    /// </summary>
    [SerializeField]
    public short Armor;
    /// <summary>
    /// 当前食物值
    /// </summary>
    [SerializeField]
    public short CurFood;
    /// <summary>
    /// 最大食物值
    /// </summary>
    [SerializeField]
    public short MaxFood;
    /// <summary>
    /// 缺水值
    /// </summary>
    [SerializeField]
    public short Water;
    /// <summary>
    /// 当前精神值
    /// </summary>
    [SerializeField]
    public short CurSan;
    /// <summary>
    /// 最大精神值
    /// </summary>
    [SerializeField]
    public short MaxSan;
    /// <summary>
    /// 心情值
    /// </summary>
    [SerializeField]
    public short Happy;
    /// <summary>
    /// 金币
    /// </summary>
    [SerializeField]
    public short Coin;
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
    public int HairID;
    /// <summary>
    /// 头发颜色
    /// </summary>
    [SerializeField]
    public Color32 HairColor;
    /// <summary>
    /// 眼镜ID
    /// </summary>
    [SerializeField]
    public int EyeID;
}
