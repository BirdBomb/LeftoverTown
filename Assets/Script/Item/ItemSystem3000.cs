using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.UI.GridLayoutGroup;

public class ItemSystem3000 
{
    
}
#region//Êß¹û
/// <summary>
/// Äñ¹û
/// </summary>
public class Item_3000 : ItemBase_Food
{
    private int config_Food = 5;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion

    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// »Æ½ðÄñ¹û
/// </summary>
public class Item_3001 : ItemBase_Food
{
    private int config_Food = 5;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// Ë®½Û
/// </summary>
public class Item_3002 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 1;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddSan/", $"+{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.AddSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// Êí¹û
/// </summary>
public class Item_3003 : ItemBase_Food
{
    private int config_Food = 1;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// À±½·
/// </summary>
public class Item_3004 : ItemBase_Food
{
    private int config_San = 5;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddSan/", $"+{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.sanManager.AddSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ÕÓÔó¹½
/// </summary>
public class Item_3005 : ItemBase_Food
{
    private int config_San = 5;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.sanManager.SubSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// Ý®¹û
/// </summary>
public class Item_3006 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 1;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddSan/", $"+{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        base.OnHand_EatAction(actor);
    }
}
#endregion
#region//Èâµ°
/// <summary>
/// ´øÆ¤Èâ
/// </summary>
public class Item_3100 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 2;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.SubSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ´ø¹ÇÈâ
/// </summary>
public class Item_3101 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 2;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.SubSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ÇÝÍÈÈâ
/// </summary>
public class Item_3102 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 2;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.SubSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ÄÚÔà
/// </summary>
public class Item_3103 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 2;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.SubSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ÎÛÈ¾Èâ
/// </summary>
public class Item_3104 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 2;
    private int config_Hp = 50;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        desc = desc.Replace("/SubHp/", $"-{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.SubSan(config_San);
        actor.actionManager.TakeDamage(config_Hp, DamageState.RealDamage, null);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ¼¦µ°
/// </summary>
public class Item_3105 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 2;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.SubSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// öêÓã
/// </summary>
public class Item_3110 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 2;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.SubSan(config_San);
        base.OnHand_EatAction(actor);
    }
}
#endregion
#region//ÆäËû
/// <summary>
/// Ãæ·Û
/// </summary>
public class Item_3200 : ItemBase_Materials
{

}
/// <summary>
/// ÌÇ·Û
/// </summary>
public class Item_3201 : ItemBase_Materials
{

}
#endregion
