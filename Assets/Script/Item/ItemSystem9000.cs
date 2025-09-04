using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem9000 
{
}
#region//��ҩ
/// <summary>
/// ����ľ��
/// </summary>
public class Item_9000 : ItemBase_Consumables
{
    #region//�޸�����
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 5.ToString());
        desc = desc.Replace("/Recycle/", "40%");
        desc = desc.Replace("/Speed/", 20.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// ����ľ��
/// </summary>
public class Item_9001 : ItemBase_Consumables
{
    #region//�޸�����
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 5.ToString());
        desc = desc.Replace("/Recycle/", "75%");
        desc = desc.Replace("/Speed/", 20.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// ����ľ��
/// </summary>
public class Item_9002 : ItemBase_Consumables
{
    #region//�޸�����
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 7.ToString());
        desc = desc.Replace("/Recycle/", "30%");
        desc = desc.Replace("/Speed/", 20.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// �ź�ľ��
/// </summary>
public class Item_9003 : ItemBase_Consumables
{
    #region//�޸�����
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 2.ToString());
        desc = desc.Replace("/Recycle/", "0%");
        desc = desc.Replace("/Speed/", 20.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// ����
/// </summary>
public class Item_9010 : ItemBase_Consumables
{
    #region//�޸�����
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", 10.ToString());
        desc = desc.Replace("/Force/", 2.ToString());
        desc = desc.Replace("/Speed/", 25.ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
}
/// <summary>
/// ��Ԫ��ϻ
/// </summary>
public class Item_9020 : ItemBase_Consumables
{
}

#endregion
#region//��
/// <summary>
/// �����ǵ�������
/// </summary>
public class Item_9100 : ItemBase
{

}

/// <summary>
/// ��Լ
/// </summary>
public class Item_9900 : ItemBase
{

}
/// <summary>
/// Կ��
/// </summary>
public class Item_9901 : ItemBase
{

}

#endregion
