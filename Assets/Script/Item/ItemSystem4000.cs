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
/// ʧ�ܲ���
/// </summary>
public class Item_4000 : ItemBase_Ingredient
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
/// �����
/// </summary>
public class Item_4001 : ItemBase_Ingredient
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
/// ������
/// </summary>
public class Item_4002 : ItemBase_Ingredient
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
/// ������
/// </summary>
public class Item_4003 : ItemBase_Ingredient
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
/// ������
/// </summary>
public class Item_4004 : ItemBase_Ingredient
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
/// ������
/// </summary>
public class Item_4005 : ItemBase_Ingredient
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
/// ���߹�
/// </summary>
public class Item_4006 : ItemBase_Ingredient
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
/// ��ˮ��
/// </summary>
public class Item_4007 : ItemBase_Ingredient
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
/// �����
/// </summary>
public class Item_4008 : ItemBase_Ingredient
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
/// �������
/// </summary>
public class Item_4009 : ItemBase_Ingredient
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
/// ���ӹ�
/// </summary>
public class Item_4010 : ItemBase_Ingredient
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
/// ץ��
/// </summary>
public class Item_4011 : ItemBase_Ingredient
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
/// ���շ�ζ��
/// </summary>
public class Item_4012 : ItemBase_Ingredient
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
