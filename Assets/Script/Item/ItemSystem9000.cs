using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem9000 
{
}
#region//µ¯Ò©
/// <summary>
/// ´ÖÖÆÄ¾¼ý
/// </summary>
public class Item_9000 : ItemBase_Consumables
{
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 5.ToString());
        desc = desc.Replace("/Recycle/", "40%");
        desc = desc.Replace("/Speed/", 20.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// ¾«ÖÆÄ¾¼ý
/// </summary>
public class Item_9001 : ItemBase_Consumables
{
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 5.ToString());
        desc = desc.Replace("/Recycle/", "75%");
        desc = desc.Replace("/Speed/", 20.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// ÖÂÉËÄ¾¼ý
/// </summary>
public class Item_9002 : ItemBase_Consumables
{
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 7.ToString());
        desc = desc.Replace("/Recycle/", "30%");
        desc = desc.Replace("/Speed/", 20.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// ÐÅºÅÄ¾¼ý
/// </summary>
public class Item_9003 : ItemBase_Consumables
{
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 2.ToString());
        desc = desc.Replace("/Recycle/", "0%");
        desc = desc.Replace("/Speed/", 20.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// µ¯Íè
/// </summary>
public class Item_9010 : ItemBase_Consumables
{
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 10.ToString());
        desc = desc.Replace("/Force/", 2.ToString());
        desc = desc.Replace("/Speed/", 25.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// ´©Ç½µ¯Íè
/// </summary>
public class Item_9011 : ItemBase_Consumables
{
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 10.ToString());
        desc = desc.Replace("/Force/", 2.ToString());
        desc = desc.Replace("/Speed/", 25.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
public class Item_9900 : ItemBase_Materials
{
}
public class Item_9901 : ItemBase_Materials
{
}
public class Item_9902 : ItemBase_Materials
{
}
public class Item_9910 : ItemBase_Materials
{
}
public class Item_9911 : ItemBase_Materials
{
    public override void StaticAction_InitData(short id, out ItemData data)
    {
        base.StaticAction_InitData(id, out data);
        data.V = (short)new System.Random().Next(0, short.MaxValue);
    }
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Seed/", itemData.V.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
}
public class Item_9912 : ItemBase_Materials
{
}

#endregion
#region//Êé
public class Item_9700 : ItemBase_Book { }
public class Item_9701 : ItemBase_Book 
{
    private const int config_ExpAdd = 2;
    private const int config_ReadingLevel = 0;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/ExpAdd/", config_ExpAdd.ToString());
        desc = desc.Replace("/ReadingLevel/", config_ReadingLevel.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        base.OnHand_Start(owner, body);
        itemLocalObj_Book.UpdateBookData(config_ExpAdd, config_ReadingLevel);
    }
}
public class Item_9702 : ItemBase_Book 
{
    private const int config_ExpAdd = 2;
    private const int config_ReadingLevel = 0;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/ExpAdd/", config_ExpAdd.ToString());
        desc = desc.Replace("/ReadingLevel/", config_ReadingLevel.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        base.OnHand_Start(owner, body);
        itemLocalObj_Book.UpdateBookData(config_ExpAdd, config_ReadingLevel);
    }
}
public class Item_9703 : ItemBase_Book 
{
    private const int config_ExpAdd = 2;
    private const int config_ReadingLevel = 0;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/ExpAdd/", config_ExpAdd.ToString());
        desc = desc.Replace("/ReadingLevel/", config_ReadingLevel.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        base.OnHand_Start(owner, body);
        itemLocalObj_Book.UpdateBookData(config_ExpAdd, config_ReadingLevel);
    }
}

#endregion
#region//Â¼Ïñ´ø
public class Item_9800 : ItemBase_Materials { }
public class Item_9801 : ItemBase_Materials { }
public class Item_9802 : ItemBase_Materials { }
public class Item_9803 : ItemBase_Materials { }
public class Item_9804 : ItemBase_Materials { }
#endregion
