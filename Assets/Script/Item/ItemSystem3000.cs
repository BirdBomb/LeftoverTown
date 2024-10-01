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
/// ˮ
/// </summary>
public class Item_3000 : ItemBase_Ingredient
{
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        base.Holding_Start(owner, body);
    }
    /// <summary>
    /// �������ʶ�
    /// </summary>
    /// <param name="nowTime"></param>
    public override void CalculateDurability()
    {
        itemData.Item_Durability = 100;
    }
}
/// <summary>
/// ��Ⱦ��
/// </summary>
public class Item_3001 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ���
/// </summary>
public class Item_3002 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ˮ��
/// </summary>
public class Item_3003 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// �ƽ����
/// </summary>
public class Item_3004 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ��Ƥ��
/// </summary>
public class Item_3005 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ������
/// </summary>
public class Item_3006 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ������
/// </summary>
public class Item_3007 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ����
/// </summary>
public class Item_3008 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ���
/// </summary>
public class Item_3009 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ���
/// </summary>
public class Item_3010 : ItemBase_Ingredient
{
    #region//ʳ���߼�
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
/// ����ʳ��
/// </summary>
public class Item_3999 : ItemBase
{
}
