using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem1000 
{
}
/// <summary>
/// 原木
/// </summary>
[Serializable]
public class Item_1001 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 木材
/// </summary>
[Serializable]
public class Item_1002 : ItemBase_Materials
{
}
/// <summary>
/// 石头
/// </summary>
[Serializable]
public class Item_1003 : ItemBase_Materials
{
    private short attackDamage = 3;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 煤炭
/// </summary>
[Serializable]
public class Item_1004 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 金矿石
/// </summary>
[Serializable]
public class Item_1005 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 树枝
/// </summary>
[Serializable]
public class Item_1006 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 铁矿
/// </summary>
public class Item_1007 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 高能矿
/// </summary>
public class Item_1008 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 翡翠原石
/// </summary>
public class Item_1009 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 硝矿
/// </summary>
public class Item_1010 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 红水晶原石
/// </summary>
public class Item_1011 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 蛋白原石
/// </summary>
public class Item_1012 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 铁锭
/// </summary>
public class Item_1013: ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 金锭
/// </summary>
public class Item_1014: ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 高能钻
/// </summary>
public class Item_1015 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 翡翠
/// </summary>
public class Item_1016 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 硝石碎块
/// </summary>
public class Item_1017 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 红水晶
/// </summary>
public class Item_1018 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 蛋白石
/// </summary>
public class Item_1019 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 机械元件
/// </summary>
public class Item_1020 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
/// <summary>
/// 高能芯片
/// </summary>
public class Item_1021 : ItemBase_Materials
{
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Attack);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public void Attack(string trigger)
    {
        if (trigger == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, itemConfig.Attack_Distance, attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                    }
                }
            }
        }
    }
}
