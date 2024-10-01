using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem4000 
{

}
/// <summary>
/// Ê§°Ü²ËëÈ
/// </summary>
public class Item_4000 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¿¾Èâ¿é
/// </summary>
public class Item_4001 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¿¾ÈâÅÅ
/// </summary>
public class Item_4002 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¿¾ÇÝÍÈ
/// </summary>
public class Item_4003 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¿¾ÄÚÔà
/// </summary>
public class Item_4004 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¿¾³ôÈâ
/// </summary>
public class Item_4005 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¿¾Éß¹û
/// </summary>
public class Item_4006 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¿¾Ë®½Û
/// </summary>
public class Item_4007 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¿¾Êí¹û
/// </summary>
public class Item_4008 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// Êí¹ûÈâìÒ
/// </summary>
public class Item_4009 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ìÀÔÓ¹û
/// </summary>
public class Item_4010 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ×¥Èâ
/// </summary>
public class Item_4011 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
/// ¹û×Õ·çÎ¶ÉÕ
/// </summary>
public class Item_4012 : ItemBase_Ingredient
{
    #region//Ê³ÓÃÂß¼­
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
