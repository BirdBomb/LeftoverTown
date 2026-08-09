using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem5000 
{
}
#region//帽子
/// <summary>
/// 木盔
/// </summary>
public class Item_5000 : ItemBase_Hat
{
    #region//基础数值
    private readonly int Armor_Base = 7;
    private int Armor_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    #endregion
    #region//穿戴
    public override int OnHead_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
    #endregion
}
/// <summary>
/// 铜盔
/// </summary>
public class Item_5001 : ItemBase_Hat
{
    #region//基础数值
    private readonly int Armor_Base = 20;
    private int Armor_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    #endregion
    #region//穿戴
    public override int OnHead_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
    #endregion
}
/// <summary>
/// 铁盔
/// </summary>
public class Item_5002 : ItemBase_Hat
{
    #region//基础数值
    private readonly int Armor_Base = 30;
    private int Armor_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    #endregion
    #region//穿戴
    public override int OnHead_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
    #endregion
}
/// <summary>
/// 金皮革盔
/// </summary>
public class Item_5003 : ItemBase_Hat
{
    #region//基础数值
    private readonly int Armor_Base = 20;
    private int Armor_Add;
    private readonly int Resistance_Base = 20;
    private int Resistance_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        desc = desc.Replace("/Resistance/", Math.Round((Resistance_Base + Resistance_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    Resistance_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    Resistance_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    Resistance_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    Resistance_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    Resistance_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    Resistance_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    Resistance_Add = 6;
                    break;
                }
        }
    }
    #endregion
    #region//穿戴
    public override int OnHead_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
    public override int OnHead_CalculateResistance(int resistance)
    {
        return resistance + Resistance_Base + Resistance_Add;
    }
    #endregion
}
/// <summary>
/// 防爆面罩
/// </summary>
public class Item_5004 : ItemBase_Hat
{
    #region//基础数值
    private readonly int Armor_Base = 40;
    private int Armor_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    #endregion
    #region//穿戴
    public override int OnHead_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
    #endregion
}
/// <summary>
/// 硝石帽子
/// </summary>
public class Item_5100 : ItemBase_Hat
{
    #region//基础数值
    private readonly int Resistance_Base = 20;
    private int Resistance_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Resistance/", Math.Round((Resistance_Base + Resistance_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Resistance_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Resistance_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Resistance_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Resistance_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Resistance_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Resistance_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Resistance_Add = 6;
                    break;
                }
        }
    }
    #endregion
    #region//穿戴
    public override int OnHead_CalculateResistance(int resistance)
    {
        return resistance + Resistance_Base + Resistance_Add;
    }
    #endregion
}
/// <summary>
/// 草帽
/// </summary>
public class Item_5200 : ItemBase_Hat
{

}
/// <summary>
/// 矿灯头盔
/// </summary>
public class Item_5201 : ItemBase_Hat
{
    #region//基础数值
    private readonly int Armor_Base = 20;
    private int Armor_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    #endregion
    #region//穿戴
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        GameObject itemLocalObj = PoolManager.Instance.GetObject($"ItemObj/ItemLocalObj_{itemData.I}");
        body.AddItemOnHead(itemLocalObj, Vector3.zero, Quaternion.identity, Vector3.one);
    }
    public override int OnHead_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
    #endregion
}
/// <summary>
/// 歹徒面罩
/// </summary>
public class Item_5202 : ItemBase_Hat
{
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        GameObject itemLocalObj = PoolManager.Instance.GetObject($"ItemObj/ItemLocalObj_{itemData.I}");
        body.AddItemOnHead(itemLocalObj, Vector3.zero, Quaternion.identity, Vector3.one);
    }
}
/// <summary>
/// 治安官帽子
/// </summary>
public class Item_5203 : ItemBase_Hat
{

}
/// <summary>
/// 廉价耳机
/// </summary>
public class Item_5204 : ItemBase_Hat
{
    #region//基础数值
    private const float SanUp_Base = 0.1f;
    private float SanUp_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/SanUp/", (SanUp_Base + SanUp_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    SanUp_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    SanUp_Add = 0.02f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    SanUp_Add = 0.04f;
                    break;
                }
            case ItemQuality.Purple:
                {
                    SanUp_Add = 0.06f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    SanUp_Add = 0.08f;
                    break;
                }
            case ItemQuality.Red:
                {
                    SanUp_Add = 0.1f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    SanUp_Add = 0.2f;
                    break;
                }
        }
    }
    #endregion
    #region//恢复精神
    private float temp;
    public override void OnHead_UpdateTime(int second)
    {
        temp += (SanUp_Base + SanUp_Add) * second;
        if (temp > 0) { temp -= 1; owner.sanManager.AddSan(1); }
        base.OnHead_UpdateTime(second);
    }
    #endregion
    #region//穿戴
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        GameObject itemLocalObj = PoolManager.Instance.GetObject($"ItemObj/ItemLocalObj_{itemData.I}");
        body.AddItemOnHead(itemLocalObj, Vector3.zero, Quaternion.identity, Vector3.one);
    }
    #endregion
}
/// <summary>
/// 精品耳机
/// </summary>
public class Item_5205 : ItemBase_Hat
{
    #region//基础数值
    private const float SanUp_Base = 0.2f;
    private float SanUp_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/SanUp/", (SanUp_Base + SanUp_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    SanUp_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    SanUp_Add = 0.02f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    SanUp_Add = 0.04f;
                    break;
                }
            case ItemQuality.Purple:
                {
                    SanUp_Add = 0.06f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    SanUp_Add = 0.08f;
                    break;
                }
            case ItemQuality.Red:
                {
                    SanUp_Add = 0.1f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    SanUp_Add = 0.2f;
                    break;
                }
        }

    }
    #endregion
    #region//恢复精神
    private float temp;
    public override void OnHead_UpdateTime(int second)
    {
        temp += (SanUp_Base + SanUp_Add) * second;
        if(temp > 0) { temp -= 1; owner.sanManager.AddSan(1); }
        base.OnHead_UpdateTime(second);
    }
    #endregion
    #region//穿戴
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        GameObject itemLocalObj = PoolManager.Instance.GetObject($"ItemObj/ItemLocalObj_{itemData.I}");
        body.AddItemOnHead(itemLocalObj, Vector3.zero, Quaternion.identity, Vector3.one);
    }
    #endregion
}
/// <summary>
/// 厨师帽
/// </summary>
public class Item_5206 : ItemBase_Hat
{
    #region//穿戴
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        GameObject itemLocalObj = PoolManager.Instance.GetObject($"ItemObj/ItemLocalObj_{itemData.I}");
        body.AddItemOnHead(itemLocalObj, Vector3.zero, Quaternion.identity, Vector3.one);
    }
    #endregion
}
/// <summary>
/// 皮革帽
/// </summary>
public class Item_5207 : ItemBase_Hat { }
/// <summary>
/// 潜水面罩
/// </summary>
public class Item_5208 : ItemBase_Hat 
{
    #region//穿戴
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        GameObject itemLocalObj = PoolManager.Instance.GetObject($"ItemObj/ItemLocalObj_{itemData.I}");
        body.AddItemOnHead(itemLocalObj, Vector3.zero, Quaternion.identity, Vector3.one);
    }
    #endregion
}
#endregion
#region//衣服
public class Item_5400 : ItemBase_Clothes
{
    private readonly int Armor_Base = 10;
    private int Armor_Add;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    public override int OnBody_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
}
public class Item_5401 : ItemBase_Clothes
{
    private readonly int Armor_Base = 20;
    private int Armor_Add;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    public override int OnBody_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
}
public class Item_5402 : ItemBase_Clothes
{
    private readonly int Armor_Base = 30;
    private int Armor_Add;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    public override int OnBody_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
}
public class Item_5403 : ItemBase_Clothes
{
    private readonly int Armor_Base = 50;
    private int Armor_Add;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    break;
                }
        }
    }
    public override int OnBody_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
}
public class Item_5500 : ItemBase_Clothes
{
    private readonly int Armor_Base = 10;
    private int Armor_Add;
    private readonly int Resistance_Base = 10;
    private int Resistance_Add;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        desc = desc.Replace("/Resistance/", Math.Round((Resistance_Base + Resistance_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    Resistance_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    Resistance_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    Resistance_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    Resistance_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    Resistance_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    Resistance_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    Resistance_Add = 6;
                    break;
                }
        }
    }
    public override int OnBody_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
    public override int OnBody_CalculateResistance(int resistance)
    {
        return resistance + Resistance_Base + Resistance_Add;
    }
}
public class Item_5501 : ItemBase_Clothes
{
    private readonly int Armor_Base = 30;
    private int Armor_Add;
    private readonly int Resistance_Base = 30;
    private int Resistance_Add;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Armor/", Math.Round((Armor_Base + Armor_Add) * 0.1f, 1).ToString());
        desc = desc.Replace("/Resistance/", Math.Round((Resistance_Base + Resistance_Add) * 0.1f, 1).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Armor_Add = 0;
                    Resistance_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Armor_Add = 1;
                    Resistance_Add = 1;
                    break;
                }
            case ItemQuality.Blue:
                {
                    Armor_Add = 2;
                    Resistance_Add = 2;
                    break;
                }
            case ItemQuality.Purple:
                {
                    Armor_Add = 3;
                    Resistance_Add = 3;
                    break;
                }
            case ItemQuality.Gold:
                {
                    Armor_Add = 4;
                    Resistance_Add = 4;
                    break;
                }
            case ItemQuality.Red:
                {
                    Armor_Add = 5;
                    Resistance_Add = 5;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Armor_Add = 6;
                    Resistance_Add = 6;
                    break;
                }
        }
    }
    public override int OnBody_CalculateArmor(int armor)
    {
        return armor + Armor_Base + Armor_Add;
    }
    public override int OnBody_CalculateResistance(int resistance)
    {
        return resistance + Resistance_Base + Resistance_Add;
    }
}
public class Item_5600 : ItemBase_Clothes { }
public class Item_5700 : ItemBase_Clothes { }
public class Item_5701 : ItemBase_Clothes { }
public class Item_5702 : ItemBase_Clothes { }
public class Item_5703 : ItemBase_Clothes { }
public class Item_5704 : ItemBase_Clothes { }
public class Item_5705 : ItemBase_Clothes { }
public class Item_5706 : ItemBase_Clothes { }
public class Item_5707 : ItemBase_Clothes { }
public class Item_5708 : ItemBase_Clothes { }
#endregion
#region//饰品
#endregion
