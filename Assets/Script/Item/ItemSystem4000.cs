using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem4000 
{

}
#region//¿¾ÖÆ
/// <summary>
/// ¿¾Èâ¿é
/// </summary>
public class Item_4001 : ItemBase_Food
{
    private int config_Food = 10;
    private int config_Hp = 10;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion

    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.actorHpManager.HealHp(config_Hp);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ¿¾ÈâÅÅ
/// </summary>
public class Item_4002 : ItemBase_Food
{
    private int config_Food = 10;
    private int config_Hp = 10;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.actorHpManager.HealHp(config_Hp);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ¿¾ÇÝÍÈ
/// </summary>
public class Item_4003 : ItemBase_Food
{
    private int config_Food = 10;
    private int config_Hp = 10;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.actorHpManager.HealHp(config_Hp);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ¿¾ÄÚÔà
/// </summary>
public class Item_4004 : ItemBase_Food
{
    private int config_Food = 8;
    private int config_Hp = 10;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
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
/// ¿¾³ôÈâ
/// </summary>
public class Item_4005 : ItemBase_Food
{
    private int config_Food = 10;
    private int config_San = 10;
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
/// ¿¾Éß¹û
/// </summary>
public class Item_4006 : ItemBase_Food
{
    private int config_Food = 8;
    private int config_San = 2;
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
/// ¿¾Ë®½Û
/// </summary>
public class Item_4007 : ItemBase_Food
{
    private int config_Food = 8;
    private int config_San = 2;
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
/// ¿¾Êí¹û
/// </summary>
public class Item_4008 : ItemBase_Food
{
    private int config_Food = 6;
    private int config_San = 2;
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
#region//Åëâ¿
/// <summary>
/// Ê§°Ü²ËëÈ
/// </summary>
public class Item_4100 : ItemBase_Food
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
/// Êí¹ûÈâìÒ
/// </summary>
public class Item_4101 : ItemBase_Food
{
    private int config_Food = 20;
    private int config_San = 4;
    private int config_Hp = 50;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddSan/", $"+{config_San}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.AddSan(config_San);
        actor.actorHpManager.HealHp(config_Hp);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ìÀÔÓ¹û
/// </summary>
public class Item_4102 : ItemBase_Food
{
    private int config_Food = 5;
    private int config_San = 12;
    private int config_Hp = 50;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddSan/", $"+{config_San}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.AddSan(config_San);
        actor.actorHpManager.HealHp(config_Hp);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ×¥Èâ
/// </summary>
public class Item_4103 : ItemBase_Food
{
    private int config_Food = 25;
    private int config_San = 6;
    private int config_Hp = 100;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddSan/", $"+{config_San}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.AddSan(config_San);
        actor.actorHpManager.HealHp(config_Hp);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// ¹û×Õ·çÎ¶ÉÕ
/// </summary>
public class Item_4104 : ItemBase_Food
{
    private int config_Food = 15;
    private int config_San = 20;
    private int config_Hp = 30;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddSan/", $"+{config_San}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.AddSan(config_San);
        actor.actorHpManager.HealHp(config_Hp);
        base.OnHand_EatAction(actor);
    }
}
/// <summary>
/// Ë®ÖóÓã
/// </summary>
public class Item_4105 : ItemBase_Food
{
    private int config_Food = 10;
    private int config_San = 15;
    private int config_Hp = 30;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddSan/", $"+{config_San}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.AddSan(config_San);
        actor.actorHpManager.HealHp(config_Hp);
        base.OnHand_EatAction(actor);
    }
}

#endregion
#region//Ò©¼Á
/// <summary>
/// ¸É¾»µÄË®
/// </summary>
public class Item_4200 : ItemBase_Potion
{
}
/// <summary>
/// Äñ¹ûÖ­
/// </summary>
public class Item_4201 : ItemBase_Potion
{
    private int config_Food = 20;
    private int config_Hp = 150;
    #region//ÐÞ¸ÄÃèÊö
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/AddHp/", $"+{config_Hp * 0.1f}");
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    public override void OnHand_DrinkAction(ActorManager actor)
    {
        actor.actorHpManager.HealHp(config_Hp);
        actor.hungryManager.AddFood(config_Food);
        base.OnHand_DrinkAction(actor);
    }

}
#endregion
#region//ÒûÁÏ
/// <summary>
/// Ð¡Âó¾Æ
/// </summary>
public class Item_4300 : ItemBase_Potion
{
    private const int config_Food = 20;
    private const int config_San = 20;
    private const int config_Exp = 10;
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AddFood/", $"+{config_Food}");
        desc = desc.Replace("/SubSan/", $"-{config_San}");
        desc = desc.Replace("/AddExp/", $"+{config_Exp}");
        return base.GridCell_UpdateDesc(desc);
    }
    public override void OnHand_DrinkAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        actor.sanManager.SubSan(config_San);
        if (actor.actorAuthority.isLocal && actor.actorAuthority.isPlayer)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = config_Exp
            });
        }
        base.OnHand_DrinkAction(actor);
    }
}

#endregion
#region//ÂÒìÀ
public class Item_4400 : ItemBase_Food
{
}
public class Item_4401 : ItemBase_Food
{
}
public class Item_4402 : ItemBase_Food
{
}
public class Item_4403 : ItemBase_Food
{
}
public class Item_4410 : ItemBase_Food
{
}
public class Item_4411 : ItemBase_Food
{
}
public class Item_4412 : ItemBase_Food
{
}
public class Item_4420 : ItemBase_Food
{
}
public class Item_4421 : ItemBase_Food
{
}
public class Item_4430 : ItemBase_Food
{
}
public class Item_4499 : ItemBase_Food
{
}

#endregion
