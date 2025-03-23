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
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        
    }
    #region//使用逻辑
    private const short config_attackDamage = 1;
    public override void Holding_UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.Holding_UpdateMousePos(mouse);
    }
    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                if(new System.Random().Next(0, 2) == 0) 
                {
                    owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "PunchRight");
                }
                else
                {
                    owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "PunchRight");
                }
                if (input) Attack();
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }

    private void Attack()
    {
        owner.bodyController.SetAnimatorAction(BodyPart.Hand, (string str) =>
        {
            if (str.Equals("PunchRight")|| str.Equals("PunchLeft"))
            {
                RaycastHit2D[] hit2D = Physics2D.LinecastAll(owner.transform.position, owner.transform.position + inputData.mousePosition.normalized);
                for (int i = 0; i < hit2D.Length; i++)
                {
                    if (hit2D[i].collider.transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == owner) { continue; }
                        else
                        {
                            actor.AllClient_Listen_TakeDamage(config_attackDamage, owner.actorNetManager);
                        }
                    }
                }
            }
        });

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
#endregion
