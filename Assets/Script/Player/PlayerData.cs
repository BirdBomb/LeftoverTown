using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    public PlayerData()
    {
        Name = "";
        Hp_Cur = 100;
        Hp_Max = 100;
        Armor_Cur = 0;
        Resistance_Cur = 0;
        Food_Cur = 100;
        Food_Max = 100;
        Water_Cur = 5;
        San_Cur = 100;
        San_Max = 100;
        Happy_Cur = 5;
        Coin_Cur = 100;
        Speed_Common = 60;
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
    /// 当前生命值
    /// </summary>
    [SerializeField]
    public short Hp_Cur;
    /// <summary>
    /// 最大生命值
    /// </summary>
    [SerializeField]
    public short Hp_Max;
    /// <summary>
    /// 护甲值
    /// </summary>
    [SerializeField]
    public short Armor_Cur;
    /// <summary>
    /// 魔抗值
    /// </summary>
    [SerializeField]
    public short Resistance_Cur;
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
    public int Coin_Cur;
    /// <summary>
    /// 赏金
    /// </summary>
    [SerializeField]
    public int Fine_Cur;
    /// <summary>
    /// BUFF
    /// </summary>
    public List<BuffData> BuffList = new List<BuffData>();
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
    /// 物体饰品
    /// </summary>
    [SerializeField]
    public ItemData ItemAccessory;
    /// <summary>
    /// 物体耗材
    /// </summary>
    [SerializeField]
    public ItemData ItemConsumables;
    /// <summary>
    /// 眼睛ID
    /// </summary>
    [SerializeField]
    public short Eye_ID;
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
}
