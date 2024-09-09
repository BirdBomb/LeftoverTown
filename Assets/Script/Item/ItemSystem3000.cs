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
/// <summary>
/// 水
/// </summary>
public class Item_3000 : ItemBase_Ingredient
{
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        base.Holding_Start(owner, body);
    }
    /// <summary>
    /// 计算新鲜度
    /// </summary>
    /// <param name="nowTime"></param>
    public override void CalculateDurability()
    {
        itemData.Item_Durability = 100;
    }
}
/// <summary>
/// 污染肉
/// </summary>
public class Item_3001 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 鸟果
/// </summary>
public class Item_3002 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 水桔
/// </summary>
public class Item_3003 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 黄金鸟果
/// </summary>
public class Item_3004 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 带皮肉
/// </summary>
public class Item_3005 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 带骨肉
/// </summary>
public class Item_3006 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 禽腿肉
/// </summary>
public class Item_3007 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 内脏
/// </summary>
public class Item_3008 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 薯果
/// </summary>
public class Item_3009 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 面粉
/// </summary>
public class Item_3010 : ItemBase_Ingredient
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {
                        Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }

        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Eat()
    {
        Expend(1);
        owner.AddFood(5);
    }
}
/// <summary>
/// 腐烂食物
/// </summary>
public class Item_3999 : ItemBase
{
}
