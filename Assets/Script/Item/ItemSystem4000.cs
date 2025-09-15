using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem4000 
{

}
#region//����
/// <summary>
/// �����
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
/// ������
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
/// ������
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
/// ������
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
/// ������
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
/// ���߹�
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
/// ��ˮ��
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
/// �����
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
#region//���
/// <summary>
/// ʧ�ܲ���
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
/// �������
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
/// ���ӹ�
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
/// ץ��
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
/// ���շ�ζ��
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
/// ˮ����
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
#region//ҩ��
/// <summary>
/// ���֭
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
