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
/// ԭľ
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
/// ľ��
/// </summary>
[Serializable]
public class Item_1002 : ItemBase_Materials
{
}
/// <summary>
/// ʯͷ
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
/// ú̿
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
/// ���ʯ
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
/// ��֦
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
/// ����
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
/// ���ܿ�
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
/// ���ԭʯ
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
/// ����
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
/// ��ˮ��ԭʯ
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
/// ����ԭʯ
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
/// ����
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
/// ��
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
/// ������
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
/// ���
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
/// ��ʯ���
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
/// ��ˮ��
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
/// ����ʯ
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
/// ��еԪ��
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
/// ����оƬ
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
