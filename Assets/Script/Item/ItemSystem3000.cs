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
#region//�߹�
/// <summary>
/// ���
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
/// �ƽ����
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
/// ˮ��
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
/// ���
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
/// ����
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
#region//�⵰
/// <summary>
/// ��Ƥ��
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
/// ������
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
/// ������
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
/// ����
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
/// ��Ⱦ��
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
/// ����
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
#region//����
/// <summary>
/// ���
/// </summary>
public class Item_3200 : ItemBase_Materials
{
    
}
/// <summary>
/// ����ʳ��
/// </summary>
public class Item_3999 : ItemBase
{
}
#endregion
