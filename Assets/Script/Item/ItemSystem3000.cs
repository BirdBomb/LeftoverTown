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
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// »Æ½ðÄñ¹û
/// </summary>
public class Item_3001 : ItemBase_Food
{
    private int config_Food = 5;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// Ë®½Û
/// </summary>
public class Item_3002 : ItemBase_Food
{
    private int config_Food = 5;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// Êí¹û
/// </summary>
public class Item_3003 : ItemBase_Food
{
    private int config_Food = 8;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// À±½·
/// </summary>
public class Item_3004 : ItemBase_Food
{
    private int config_Food = 2;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}

#endregion
#region//Èâµ°
/// <summary>
/// ´øÆ¤Èâ
/// </summary>
public class Item_3100 : ItemBase_Food
{
    private int config_Food = 2;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ´ø¹ÇÈâ
/// </summary>
public class Item_3101 : ItemBase_Food
{
    private int config_Food = 2;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ÇÝÍÈÈâ
/// </summary>
public class Item_3102 : ItemBase_Food
{
    private int config_Food = 2;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ÄÚÔà
/// </summary>
public class Item_3103 : ItemBase_Food
{
    private int config_Food = 1;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// ÎÛÈ¾Èâ
/// </summary>
public class Item_3104 : ItemBase_Food
{
    private int config_Food = 1;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
    }
}
/// <summary>
/// öêÓã
/// </summary>
public class Item_3110 : ItemBase_Food
{
    private int config_Food = 5;
    public override void Eat()
    {
        owner.hungryManager.AddFood(config_Food);
        base.Eat();
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
/// ¸¯ÀÃÊ³Îï
/// </summary>
public class Item_3999 : ItemBase
{
}
#endregion
