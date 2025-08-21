using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Windows;

public class ItemSystem1000 
{

}
/// <summary>
/// 空气
/// </summary>
[Serializable]
public class Item_0 : ItemBase
{
    #region//持有
    private ItemLocalObj_Punch itemLocalObj_Punch;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Punch = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_0").GetComponent<ItemLocalObj_Punch>();
        itemLocalObj_Punch.HoldingStart(owner, body);
    }
    #endregion
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        
    }

    #region//使用逻辑
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Punch.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Punch.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Punch.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    #endregion
}

#region//一级材料
/// <summary>
/// 原木
/// </summary>
public class Item_1000 : ItemBase_Materials
{
}
/// <summary>
/// 树枝
/// </summary>
public class Item_1001 : ItemBase_Materials
{
}
/// <summary>
/// 草
/// </summary>
public class Item_1002 : ItemBase_Materials
{
}
/// <summary>
/// 石头
/// </summary>
public class Item_1010 : ItemBase_Materials
{
}
/// <summary>
/// 煤炭
/// </summary>
public class Item_1011 : ItemBase_Materials
{
}
/// <summary>
/// 金矿石
/// </summary>
public class Item_1012 : ItemBase_Materials
{
}
/// <summary>
/// 铁矿石
/// </summary>
public class Item_1013 : ItemBase_Materials
{
}
/// <summary>
/// 高能矿
/// </summary>
public class Item_1014 : ItemBase_Materials
{
}
/// <summary>
/// 翡翠原石
/// </summary>
public class Item_1015 : ItemBase_Materials
{
}
/// <summary>
/// 硝矿
/// </summary>
public class Item_1016 : ItemBase_Materials
{
}
/// <summary>
/// 红水晶原石
/// </summary>
public class Item_1017 : ItemBase_Materials
{
}
/// <summary>
/// 蛋白原石
/// </summary>
public class Item_1018 : ItemBase_Materials
{
}
#endregion
#region//二级材料
/// <summary>
/// 木板
/// </summary>
public class Item_1100 : ItemBase_Materials
{
}
/// <summary>
/// 草纸
/// </summary>
public class Item_1101 : ItemBase_Materials
{
}
/// <summary>
/// 铁锭
/// </summary>
public class Item_1110 : ItemBase_Materials
{
}
/// <summary>
/// 金锭
/// </summary>
public class Item_1111 : ItemBase_Materials
{
}
/// <summary>
/// 高能水晶
/// </summary>
public class Item_1112 : ItemBase_Materials
{
}
/// <summary>
/// 翡翠
/// </summary>
public class Item_1113 : ItemBase_Materials
{
}
/// <summary>
/// 硝石碎块
/// </summary>
public class Item_1114 : ItemBase_Materials
{
}
/// <summary>
/// 红水晶
/// </summary>
public class Item_1115 : ItemBase_Materials
{
}
/// <summary>
/// 蛋白石
/// </summary>
public class Item_1116 : ItemBase_Materials
{
}
#endregion
#region//三级材料
/// <summary>
/// 机械元件
/// </summary>
public class Item_1200 : ItemBase_Materials
{
}
/// <summary>
/// 高能芯片
/// </summary>
public class Item_1201 : ItemBase_Materials
{
}
/// <summary>
/// 枪械元件
/// </summary>
public class Item_1202: ItemBase_Materials
{
}
#endregion
