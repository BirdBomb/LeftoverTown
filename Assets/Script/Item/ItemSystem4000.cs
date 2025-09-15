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
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ¿¾ÈâÅÅ
/// </summary>
public class Item_4002 : ItemBase_Food
{
    private int config_Food = 10;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ¿¾ÇÝÍÈ
/// </summary>
public class Item_4003 : ItemBase_Food
{
    private int config_Food = 10;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ¿¾ÄÚÔà
/// </summary>
public class Item_4004 : ItemBase_Food
{
    private int config_Food = 8;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ¿¾³ôÈâ
/// </summary>
public class Item_4005 : ItemBase_Food
{
    private int config_Food = 5;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ¿¾Éß¹û
/// </summary>
public class Item_4006 : ItemBase_Food
{
    private int config_Food = 7;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ¿¾Ë®½Û
/// </summary>
public class Item_4007 : ItemBase_Food
{
    private int config_Food = 7;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ¿¾Êí¹û
/// </summary>
public class Item_4008 : ItemBase_Food
{
    private int config_Food = 10;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}

#endregion
#region//Åëâ¿
/// <summary>
/// Ê§°Ü²ËëÈ
/// </summary>
public class Item_4100 : ItemBase_Food
{
    private int config_Food = 12;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// Êí¹ûÈâìÒ
/// </summary>
public class Item_4101 : ItemBase_Food
{
    private int config_Food = 25;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ìÀÔÓ¹û
/// </summary>
public class Item_4102 : ItemBase_Food
{
    private int config_Food = 25;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ×¥Èâ
/// </summary>
public class Item_4103 : ItemBase_Food
{
    private int config_Food = 25;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ¹û×Õ·çÎ¶ÉÕ
/// </summary>
public class Item_4104 : ItemBase_Food
{
    private int config_Food = 25;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// Ë®ÖóÓã
/// </summary>
public class Item_4105 : ItemBase_Food
{
    private int config_Food = 25;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}

#endregion
#region//Ò©¼Á
/// <summary>
/// Äñ¹ûÖ­
/// </summary>
public class Item_4200 : ItemBase_Potion
{
    private int config_Food = 20;
    public override void Eat()
    {
        owner.actionManager.Client_HealHP(15);
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
#endregion
