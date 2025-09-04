using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem9000 
{
}
#region//弹药
/// <summary>
/// 粗制木箭
/// </summary>
public class Item_9000 : ItemBase_Consumables
{
    #region//修改描述
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
/// 精制木箭
/// </summary>
public class Item_9001 : ItemBase_Consumables
{
    #region//修改描述
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
/// 致伤木箭
/// </summary>
public class Item_9002 : ItemBase_Consumables
{
    #region//修改描述
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
/// 信号木箭
/// </summary>
public class Item_9003 : ItemBase_Consumables
{
    #region//修改描述
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
/// 弹丸
/// </summary>
public class Item_9010 : ItemBase_Consumables
{
    #region//修改描述
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
/// 次元弹匣
/// </summary>
public class Item_9020 : ItemBase_Consumables
{
}

#endregion
#region//书
/// <summary>
/// 《我们的三季镇》
/// </summary>
public class Item_9100 : ItemBase
{

}

/// <summary>
/// 契约
/// </summary>
public class Item_9900 : ItemBase
{

}
/// <summary>
/// 钥匙
/// </summary>
public class Item_9901 : ItemBase
{

}

#endregion
