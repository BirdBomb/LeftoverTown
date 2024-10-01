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
    #region//ʹ���߼�
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
    #region//ʹ���߼�
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
/// ú̿
/// </summary>
[Serializable]
public class Item_1004 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ���ʯ
/// </summary>
[Serializable]
public class Item_1005 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ��֦
/// </summary>
[Serializable]
public class Item_1006 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ����
/// </summary>
public class Item_1007 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ���ܿ�
/// </summary>
public class Item_1008 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ���ԭʯ
/// </summary>
public class Item_1009 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ����
/// </summary>
public class Item_1010 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ��ˮ��ԭʯ
/// </summary>
public class Item_1011 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ����ԭʯ
/// </summary>
public class Item_1012 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ����
/// </summary>
public class Item_1013: ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ��
/// </summary>
public class Item_1014: ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ������
/// </summary>
public class Item_1015 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ���
/// </summary>
public class Item_1016 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ��ʯ���
/// </summary>
public class Item_1017 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ��ˮ��
/// </summary>
public class Item_1018 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ����ʯ
/// </summary>
public class Item_1019 : ItemBase_Materials
{
    #region//ʹ���߼�
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
/// ��еԪ��
/// </summary>
public class Item_1020 : ItemBase_Materials
{
}
/// <summary>
/// ����оƬ
/// </summary>
public class Item_1021 : ItemBase_Materials
{
}
