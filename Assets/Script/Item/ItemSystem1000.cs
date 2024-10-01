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
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
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
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 煤炭
/// </summary>
[Serializable]
public class Item_1004 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 金矿石
/// </summary>
[Serializable]
public class Item_1005 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 树枝
/// </summary>
[Serializable]
public class Item_1006 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 铁矿
/// </summary>
public class Item_1007 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 高能矿
/// </summary>
public class Item_1008 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 翡翠原石
/// </summary>
public class Item_1009 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 硝矿
/// </summary>
public class Item_1010 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 红水晶原石
/// </summary>
public class Item_1011 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 蛋白原石
/// </summary>
public class Item_1012 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 铁锭
/// </summary>
public class Item_1013: ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 金锭
/// </summary>
public class Item_1014: ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 高能钻
/// </summary>
public class Item_1015 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 翡翠
/// </summary>
public class Item_1016 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 硝石碎块
/// </summary>
public class Item_1017 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 红水晶
/// </summary>
public class Item_1018 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 蛋白石
/// </summary>
public class Item_1019 : ItemBase_Materials
{
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 0.2f;
    private const float config_attackMaxRange = 120;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
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
    private void OnlyInput_Attack()
    {
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, itemConfig.Attack_Distance, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                }
            }
        }
    }
    #endregion
}
/// <summary>
/// 机械元件
/// </summary>
public class Item_1020 : ItemBase_Materials
{
}
/// <summary>
/// 高能芯片
/// </summary>
public class Item_1021 : ItemBase_Materials
{
}
