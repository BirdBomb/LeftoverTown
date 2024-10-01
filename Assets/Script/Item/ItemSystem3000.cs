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
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 鸟果
/// </summary>
public class Item_3002 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 水桔
/// </summary>
public class Item_3003 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 黄金鸟果
/// </summary>
public class Item_3004 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 带皮肉
/// </summary>
public class Item_3005 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 带骨肉
/// </summary>
public class Item_3006 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 禽腿肉
/// </summary>
public class Item_3007 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 内脏
/// </summary>
public class Item_3008 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 薯果
/// </summary>
public class Item_3009 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 面粉
/// </summary>
public class Item_3010 : ItemBase_Ingredient
{
    #region//食用逻辑
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (input && str.Equals("HeadEat"))
                    {
                        OnlyInput_Eat();
                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Eat()
    {
        Expend(1);
        owner.AllClient_AddFood(5);
    }

    #endregion
}
/// <summary>
/// 腐烂食物
/// </summary>
public class Item_3999 : ItemBase
{
}
