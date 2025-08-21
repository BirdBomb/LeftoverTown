using DG.Tweening;
using Fusion;
using System;
using System.Collections.Generic;
using System.Drawing;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Sockets.NetBitBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class ItemSystem2000 
{
    
}
#region//纯工具

/// <summary>
/// 火把
/// </summary>
public class Item_2000 : ItemBase_Tool
{
    #region//基础数值
    /// <summary>
    /// 光照范围
    /// </summary>
    private readonly float LightRange_Base = 5;
    /// <summary>
    /// 光照范围修正
    /// </summary>
    private float LightRange_Add;
    /// <summary>
    /// 损耗速度
    /// </summary>
    private readonly float LightSpeed_Base = 0.2f;
    /// <summary>
    /// 损耗速度修正
    /// </summary>
    private float LightSpeed_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/LightRange/", (LightRange_Base + LightRange_Add).ToString());
        desc = desc.Replace("/LightSpeed/",(LightSpeed_Base + LightSpeed_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        LightRange_Add = 0;
        LightSpeed_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            LightSpeed_Add -= (float)Math.Round(LightSpeed_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            LightRange_Add += 1f;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            LightSpeed_Add -= (float)Math.Round(LightSpeed_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            LightRange_Add += 1f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            LightRange_Add += 1f;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            LightSpeed_Add = -LightSpeed_Base;
        }

        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    LightRange_Add = 0;
                    LightSpeed_Add = 0.2f;
                    break;
                }
            case ItemQuality.Green:
                {
                    LightRange_Add = 1;
                    LightSpeed_Add = 0.2f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    LightRange_Add = 1;
                    LightSpeed_Add = 0.1f;
                    break;
                }
            case ItemQuality.Purple:
                {
                    LightRange_Add = 2;
                    LightSpeed_Add = 0.1f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    LightRange_Add = 2;
                    LightSpeed_Add = 0.05f;
                    break;
                }
            case ItemQuality.Red:
                {
                    LightRange_Add = 3;
                    LightSpeed_Add = 0f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    LightRange_Add = 5;
                    LightSpeed_Add = 0f;
                    break;
                }
        }
        if (itemLocalObj_Torch) itemLocalObj_Torch.UpdateTorchData(LightRange_Base + LightRange_Add, LightSpeed_Base + LightSpeed_Add, itemQuality);
    }

    #endregion
    #region//持有
    private ItemLocalObj_Torch itemLocalObj_Torch;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Torch = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2000").GetComponent<ItemLocalObj_Torch>();
        itemLocalObj_Torch.InitData(itemData);
        itemLocalObj_Torch.HoldingStart(owner, body);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Torch.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Torch.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateTime(int second)
    {
        itemLocalObj_Torch.UpdateTime(second);
        base.OnHand_UpdateTime(second);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Torch) itemLocalObj_Torch.UpdateDataByNet(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Torch) itemLocalObj_Torch.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
/// <summary>
/// 木斧头
/// </summary>
public class Item_2010 : ItemBase_Tool
{
    #region//基础数值
    /// <summary>
    /// 劈砍伤害
    /// </summary>
    private readonly int HackDamage_Base = 5;
    /// <summary>
    /// 劈砍伤害修正
    /// </summary>
    private int HackDamage_Add = 0;
    /// <summary>
    /// 劈砍速度
    /// </summary>
    private readonly float HackSpeed_Base = 1;
    /// <summary>
    /// 劈砍速度修正
    /// </summary>
    private float HackSpeed_Add = 0;
    /// <summary>
    /// 劈砍距离
    /// </summary>
    private readonly float HackDistance_Base = 1;
    /// <summary>
    /// 劈砍距离修正
    /// </summary>
    private float HackDistance_Add = 0;
    /// <summary>
    /// 劈砍范围
    /// </summary>
    private readonly float HackRange_Base = 60;
    /// <summary>
    /// 劈砍范围修正
    /// </summary>
    private float HackRange_Add = 0;
    /// <summary>
    /// 劈砍损耗
    /// </summary>
    private readonly float HackExpend_Base = 0;
    /// <summary>
    /// 劈砍损耗修正
    /// </summary>
    private float HackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/HackDamage/", (HackDamage_Base + HackDamage_Add).ToString());
        desc = desc.Replace("/HackSpeed/", (HackSpeed_Base + HackSpeed_Add).ToString());
        desc = desc.Replace("/HackRange/", (HackRange_Base + HackRange_Add).ToString());
        desc = desc.Replace("/HackDistance/", (HackDistance_Base + HackDistance_Add).ToString());
        desc = desc.Replace("/HackExpend/", (HackExpend_Base + HackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        HackDamage_Add = 0;
        HackSpeed_Add = 0;
        HackDistance_Add = 0;
        HackRange_Add = 0;
        HackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            HackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            HackExpend_Add -= (float)Math.Round(HackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            HackDamage_Add += 1;
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            HackDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            HackExpend_Add = -HackExpend_Base;
        }
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2010").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 铁斧头
/// </summary>
public class Item_2011 : ItemBase_Tool
{
    #region//基础数值
    /// <summary>
    /// 劈砍伤害
    /// </summary>
    private readonly int HackDamage_Base = 7;
    /// <summary>
    /// 劈砍伤害修正
    /// </summary>
    private int HackDamage_Add = 0;
    /// <summary>
    /// 劈砍速度
    /// </summary>
    private readonly float HackSpeed_Base = 1.2f;
    /// <summary>
    /// 劈砍速度修正
    /// </summary>
    private float HackSpeed_Add = 0;
    /// <summary>
    /// 劈砍距离
    /// </summary>
    private readonly float HackDistance_Base = 1;
    /// <summary>
    /// 劈砍距离修正
    /// </summary>
    private float HackDistance_Add = 0;
    /// <summary>
    /// 劈砍范围
    /// </summary>
    private readonly float HackRange_Base = 60;
    /// <summary>
    /// 劈砍范围修正
    /// </summary>
    private float HackRange_Add = 0;
    /// <summary>
    /// 劈砍损耗
    /// </summary>
    private readonly float HackExpend_Base = 0;
    /// <summary>
    /// 劈砍损耗修正
    /// </summary>
    private float HackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/HackDamage/", (HackDamage_Base + HackDamage_Add).ToString());
        desc = desc.Replace("/HackSpeed/", (HackSpeed_Base + HackSpeed_Add).ToString());
        desc = desc.Replace("/HackRange/", (HackRange_Base + HackRange_Add).ToString());
        desc = desc.Replace("/HackDistance/", (HackDistance_Base + HackDistance_Add).ToString());
        desc = desc.Replace("/HackExpend/", (HackExpend_Base + HackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        HackDamage_Add = 0;
        HackSpeed_Add = 0;
        HackDistance_Add = 0;
        HackRange_Add = 0;
        HackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            HackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            HackExpend_Add -= (float)Math.Round(HackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            HackDamage_Add += 1;
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            HackDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            HackExpend_Add = -HackExpend_Base;
        }
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2011").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality); 
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 伐木斧头
/// </summary>
public class Item_2012 : ItemBase_Tool
{
    #region//基础数值
    /// <summary>
    /// 劈砍伤害
    /// </summary>
    private readonly int HackDamage_Base = 7;
    /// <summary>
    /// 劈砍伤害修正
    /// </summary>
    private int HackDamage_Add = 0;
    /// <summary>
    /// 劈砍速度
    /// </summary>
    private readonly float HackSpeed_Base = 1.2f;
    /// <summary>
    /// 劈砍速度修正
    /// </summary>
    private float HackSpeed_Add = 0;
    /// <summary>
    /// 劈砍距离
    /// </summary>
    private readonly float HackDistance_Base = 1;
    /// <summary>
    /// 劈砍距离修正
    /// </summary>
    private float HackDistance_Add = 0;
    /// <summary>
    /// 劈砍范围
    /// </summary>
    private readonly float HackRange_Base = 60;
    /// <summary>
    /// 劈砍范围修正
    /// </summary>
    private float HackRange_Add = 0;
    /// <summary>
    /// 劈砍损耗
    /// </summary>
    private readonly float HackExpend_Base = 0;
    /// <summary>
    /// 劈砍损耗修正
    /// </summary>
    private float HackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/HackDamage/", (HackDamage_Base + HackDamage_Add).ToString());
        desc = desc.Replace("/HackSpeed/", (HackSpeed_Base + HackSpeed_Add).ToString());
        desc = desc.Replace("/HackRange/", (HackRange_Base + HackRange_Add).ToString());
        desc = desc.Replace("/HackDistance/", (HackDistance_Base + HackDistance_Add).ToString());
        desc = desc.Replace("/HackExpend/", (HackExpend_Base + HackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        HackDamage_Add = 0;
        HackSpeed_Add = 0;
        HackDistance_Add = 0;
        HackRange_Add = 0;
        HackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            HackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            HackExpend_Add -= (float)Math.Round(HackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            HackDamage_Add += 1;
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            HackDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            HackExpend_Add = -HackExpend_Base;
        }
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2012").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
        base.OnHand_Start(owner, body);
    }

    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 木镐
/// </summary>
public class Item_2020 : ItemBase_Tool
{
    #region//基础数值
    /// <summary>
    /// 劈砍伤害
    /// </summary>
    private readonly int HackDamage_Base = 10;
    /// <summary>
    /// 劈砍伤害修正
    /// </summary>
    private int HackDamage_Add = 0;
    /// <summary>
    /// 劈砍速度
    /// </summary>
    private readonly float HackSpeed_Base = 0.5f;
    /// <summary>
    /// 劈砍速度修正
    /// </summary>
    private float HackSpeed_Add = 0;
    /// <summary>
    /// 劈砍距离
    /// </summary>
    private readonly float HackDistance_Base = 1;
    /// <summary>
    /// 劈砍距离修正
    /// </summary>
    private float HackDistance_Add = 0;
    /// <summary>
    /// 劈砍范围
    /// </summary>
    private readonly float HackRange_Base = 60;
    /// <summary>
    /// 劈砍范围修正
    /// </summary>
    private float HackRange_Add = 0;
    /// <summary>
    /// 劈砍损耗
    /// </summary>
    private readonly float HackExpend_Base = 0;
    /// <summary>
    /// 劈砍损耗修正
    /// </summary>
    private float HackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/HackDamage/", (HackDamage_Base + HackDamage_Add).ToString());
        desc = desc.Replace("/HackSpeed/", (HackSpeed_Base + HackSpeed_Add).ToString());
        desc = desc.Replace("/HackRange/", (HackRange_Base + HackRange_Add).ToString());
        desc = desc.Replace("/HackDistance/", (HackDistance_Base + HackDistance_Add).ToString());
        desc = desc.Replace("/HackExpend/", (HackExpend_Base + HackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        HackDamage_Add = 0;
        HackSpeed_Add = 0;
        HackDistance_Add = 0;
        HackRange_Add = 0;
        HackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            HackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            HackExpend_Add -= (float)Math.Round(HackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            HackDamage_Add += 1;
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            HackDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            HackExpend_Add = -HackExpend_Base;
        }
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2020").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 铁镐
/// </summary>
public class Item_2021 : ItemBase_Tool
{
    #region//基础数值
    /// <summary>
    /// 劈砍伤害
    /// </summary>
    private readonly int HackDamage_Base = 12;
    /// <summary>
    /// 劈砍伤害修正
    /// </summary>
    private int HackDamage_Add = 0;
    /// <summary>
    /// 劈砍速度
    /// </summary>
    private readonly float HackSpeed_Base = 0.7f;
    /// <summary>
    /// 劈砍速度修正
    /// </summary>
    private float HackSpeed_Add = 0;
    /// <summary>
    /// 劈砍距离
    /// </summary>
    private readonly float HackDistance_Base = 1;
    /// <summary>
    /// 劈砍距离修正
    /// </summary>
    private float HackDistance_Add = 0;
    /// <summary>
    /// 劈砍范围
    /// </summary>
    private readonly float HackRange_Base = 60;
    /// <summary>
    /// 劈砍范围修正
    /// </summary>
    private float HackRange_Add = 0;
    /// <summary>
    /// 劈砍损耗
    /// </summary>
    private readonly float HackExpend_Base = 0.8f;
    /// <summary>
    /// 劈砍损耗修正
    /// </summary>
    private float HackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/HackDamage/", (HackDamage_Base + HackDamage_Add).ToString());
        desc = desc.Replace("/HackSpeed/", (HackSpeed_Base + HackSpeed_Add).ToString());
        desc = desc.Replace("/HackRange/", (HackRange_Base + HackRange_Add).ToString());
        desc = desc.Replace("/HackDistance/", (HackDistance_Base + HackDistance_Add).ToString());
        desc = desc.Replace("/HackExpend/", (HackExpend_Base + HackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        HackDamage_Add = 0;
        HackSpeed_Add = 0;
        HackDistance_Add = 0;
        HackRange_Add = 0;
        HackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            HackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            HackExpend_Add -= (float)Math.Round(HackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            HackDamage_Add += 1; 
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            HackDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            HackSpeed_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            HackExpend_Add = -HackExpend_Base;
        }
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2021").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdateAexData(HackDamage_Base + HackDamage_Add, HackSpeed_Base + HackSpeed_Add, HackDistance_Base + HackDistance_Add, HackExpend_Base + HackExpend_Add, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Axe) itemLocalObj_Axe.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 木头鱼竿
/// </summary>
public class Item_2030 : ItemBase
{
    #region//数据初始化
    public override void StaticAction_InitData(short id, out ItemData data)
    {
        base.StaticAction_InitData(id, out data);
        data.Item_Info = (short)new System.Random().Next(short.MinValue, short.MaxValue);
    }

    #endregion
    #region//基础数值
    /// <summary>
    /// 渔力
    /// </summary>
    private readonly float FishPower_Base = 2;
    /// <summary>
    /// 渔力修正
    /// </summary>
    private float FishPower_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/FishPower/", (FishPower_Base + FishPower_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        FishPower_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            FishPower_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            FishPower_Add += 0.3f;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            FishPower_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            FishPower_Add += 0.7f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            FishPower_Add += 1;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            FishPower_Add += 5;
        }
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    FishPower_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    FishPower_Add = 0.2f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    FishPower_Add = 0.5f;
                    break;
                }
            case ItemQuality.Purple:
                {
                    FishPower_Add = 1f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    FishPower_Add = 1.5f;
                    break;
                }
            case ItemQuality.Red:
                {
                    FishPower_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    FishPower_Add = 10f;
                    break;
                }
        }
        if (itemLocalObj_FishRod) itemLocalObj_FishRod.UpdateFishRodData(FishPower_Base + FishPower_Add, itemQuality);
    }

    #endregion
    #region//修改UI
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string stringName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
        string stringDesc = GridCell_UpdateDesc(LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc"));
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);

        string stringInfo = stringName + "(" + stringQuality + ")" + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, stringName, itemData.Item_Count.ToString());
        gridCell.SetCell(stringInfo);
    }
    #endregion
    #region//使用逻辑 
    private ItemLocalObj_FishRod itemLocalObj_FishRod;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_FishRod = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2030").GetComponent<ItemLocalObj_FishRod>();
        itemLocalObj_FishRod.InitData(itemData);
        itemLocalObj_FishRod.HoldingStart(owner, body);
        itemLocalObj_FishRod.UpdateFishRodData(FishPower_Base + FishPower_Add, itemQuality);
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        if (owner)
        {
            itemLocalObj_FishRod.PressRightMouse(pressTimer, owner.actorAuthority);
        }
        return true;
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        if (owner)
        {
            itemLocalObj_FishRod.ReleaseRightMouse();
        }
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if(owner)
        {
            itemLocalObj_FishRod.PressLeftMouse(pressTimer, owner.actorAuthority);
        }
        return base.OnHand_UpdateLeftPress(pressTimer, state, input, player);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        if(owner)
        {
            itemLocalObj_FishRod.ReleaseLeftMouse();
        }
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_FishRod.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    #endregion
}
/// <summary>
/// 木锄
/// </summary>
public class Item_2040 : ItemBase_Tool
{
    #region//基础数值
    /// <summary>
    /// 使用损耗
    /// </summary>
    private readonly float HoeExpend_Base = 2f;
    /// <summary>
    /// 使用损耗修正
    /// </summary>
    private float HoeExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/HoeExpend/", (HoeExpend_Base + HoeExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        HoeExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            HoeExpend_Add -= (float)Math.Round(HoeExpend_Base * 0.1f, 2);
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            HoeExpend_Add -= (float)Math.Round(HoeExpend_Base * 0.1f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            HoeExpend_Add -= (float)Math.Round(HoeExpend_Base * 0.1f, 2);
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            HoeExpend_Add -= (float)Math.Round(HoeExpend_Base * 0.1f, 2);
        }
        if (itemQuality >= ItemQuality.Red)
        {
            HoeExpend_Add -= (float)Math.Round(HoeExpend_Base * 0.1f, 2);
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            HoeExpend_Add = -HoeExpend_Base;
        }
        if (itemLocalObj_Hoe) itemLocalObj_Hoe.UpdateHoeData(HoeExpend_Base + HoeExpend_Add, itemQuality);
    }

    #endregion

    #region//使用逻辑
    private ItemLocalObj_Hoe itemLocalObj_Hoe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Hoe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2040").GetComponent<ItemLocalObj_Hoe>();
        itemLocalObj_Hoe.InitData(itemData);
        itemLocalObj_Hoe.HoldingStart(owner, body);
        itemLocalObj_Hoe.UpdateHoeData(HoeExpend_Base + HoeExpend_Add, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Hoe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Hoe.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Hoe.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Hoe) itemLocalObj_Hoe.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Hoe) itemLocalObj_Hoe.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
#endregion
#region//近战武器
/// <summary>
/// 木棍
/// </summary>
public class Item_2100 : ItemBase_Weapon
{
    #region//基础数值
    /// <summary>
    /// 戳刺伤害
    /// </summary>
    private readonly int StabDamage_Base = 6;
    /// <summary>
    /// 戳刺伤害修正
    /// </summary>
    private int StabDamage_Add = 0;
    /// <summary>
    /// 横扫伤害
    /// </summary>
    private readonly int HackDamage_Base = 4;
    /// <summary>
    /// 横扫伤害修正
    /// </summary>
    private int HackDamage_Add = 0;
    /// <summary>
    /// 攻击速度
    /// </summary>
    private readonly float AttackSpeed_Base = 1f;
    /// <summary>
    /// 攻击速度修正
    /// </summary>
    private float AttackSpeed_Add = 0;
    /// <summary>
    /// 攻击距离
    /// </summary>
    private readonly float AttackDistance_Base = 2;
    /// <summary>
    /// 攻击距离修正
    /// </summary>
    private float AttackDistance_Add = 0;
    /// <summary>
    /// 攻击损耗
    /// </summary>
    private readonly float AttackExpend_Base = 0.8f;
    /// <summary>
    /// 攻击损耗修正
    /// </summary>
    private float AttackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/StabDamage/", (StabDamage_Base + StabDamage_Add).ToString());
        desc = desc.Replace("/HackDamage/", (HackDamage_Base + HackDamage_Add).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/AttackExpend/", (AttackExpend_Base + AttackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        HackDamage_Add = 0;
        StabDamage_Add = 0;
        AttackDistance_Add = 0;
        AttackSpeed_Add = 0;
        AttackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            AttackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            AttackSpeed_Add += 0.2f;
            AttackExpend_Add -= (float)Math.Round(AttackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            HackDamage_Add += 1;
            StabDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AttackSpeed_Add += 0.2f;
            AttackExpend_Add -= (float)Math.Round(AttackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Red)
        {
            HackDamage_Add += 1;
            StabDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            AttackExpend_Add = -AttackExpend_Base;
        }
        if (itemLocalObj_Spear)
        {
            itemLocalObj_Spear.UpdateSpearData(
                StabDamage_Base + StabDamage_Add,
                HackDamage_Base + HackDamage_Add,
                AttackSpeed_Base + AttackSpeed_Add,
                AttackDistance_Base + AttackDistance_Add,
                AttackExpend_Base + AttackExpend_Add,
                itemQuality);
        }
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Spear itemLocalObj_Spear;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Spear = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2100").GetComponent<ItemLocalObj_Spear>();
        itemLocalObj_Spear.InitData(itemData);
        itemLocalObj_Spear.HoldingStart(owner, body);
        itemLocalObj_Spear.UpdateSpearData(
            StabDamage_Base + StabDamage_Add,
            HackDamage_Base + HackDamage_Add,
            AttackSpeed_Base + AttackSpeed_Add,
            AttackDistance_Base + AttackDistance_Add,
            AttackExpend_Base + AttackExpend_Add, 
            itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Spear.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Spear.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Spear.ReleaseLeftMouse();
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Spear.PressRightMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Spear.ReleaseRightMouse();
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Spear) itemLocalObj_Spear.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Spear) itemLocalObj_Spear.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 长柄刀
/// </summary>
public class Item_2101 : ItemBase_Weapon
{
    #region//基础数值
    /// <summary>
    /// 戳刺伤害
    /// </summary>
    private readonly int StabDamage_Base = 8;
    /// <summary>
    /// 戳刺伤害修正
    /// </summary>
    private int StabDamage_Add = 0;
    /// <summary>
    /// 横扫伤害
    /// </summary>
    private readonly int HackDamage_Base = 6;
    /// <summary>
    /// 横扫伤害修正
    /// </summary>
    private int HackDamage_Add = 0;
    /// <summary>
    /// 攻击速度
    /// </summary>
    private readonly float AttackSpeed_Base = 1.2f;
    /// <summary>
    /// 攻击速度修正
    /// </summary>
    private float AttackSpeed_Add = 0;
    /// <summary>
    /// 攻击距离
    /// </summary>
    private readonly float AttackDistance_Base = 2;
    /// <summary>
    /// 攻击距离修正
    /// </summary>
    private float AttackDistance_Add = 0;
    /// <summary>
    /// 攻击损耗
    /// </summary>
    private readonly float AttackExpend_Base = 0.8f;
    /// <summary>
    /// 攻击损耗修正
    /// </summary>
    private float AttackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/StabDamage/", (StabDamage_Base + StabDamage_Add).ToString());
        desc = desc.Replace("/HackDamage/", (HackDamage_Base + HackDamage_Add).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/AttackExpend/", (AttackExpend_Base + AttackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        HackDamage_Add = 0;
        StabDamage_Add = 0;
        AttackDistance_Add = 0;
        AttackSpeed_Add = 0;
        AttackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            AttackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            AttackSpeed_Add += 0.2f;
            AttackExpend_Add -= (float)Math.Round(AttackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            HackDamage_Add += 1;
            StabDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AttackSpeed_Add += 0.2f;
            AttackExpend_Add -= (float)Math.Round(AttackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Red)
        {
            HackDamage_Add += 1;
            StabDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            AttackExpend_Add = -AttackExpend_Base;
        }
        if (itemLocalObj_Spear)
        {
            itemLocalObj_Spear.UpdateSpearData(
                StabDamage_Base + StabDamage_Add, 
                HackDamage_Base + HackDamage_Add, 
                AttackSpeed_Base + AttackSpeed_Add, 
                AttackDistance_Base + AttackDistance_Add, 
                AttackExpend_Base + AttackExpend_Add, 
                itemQuality);
        }
    }
    #endregion

    #region//使用逻辑
    private ItemLocalObj_Spear itemLocalObj_Spear;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Spear = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2101").GetComponent<ItemLocalObj_Spear>();
        itemLocalObj_Spear.InitData(itemData);
        itemLocalObj_Spear.HoldingStart(owner, body);
        itemLocalObj_Spear.UpdateSpearData(
            StabDamage_Base + StabDamage_Add,
            HackDamage_Base + HackDamage_Add,
            AttackSpeed_Base + AttackSpeed_Add,
            AttackDistance_Base + AttackDistance_Add,
            AttackExpend_Base + AttackExpend_Add,
            itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Spear.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Spear.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Spear.ReleaseLeftMouse();
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Spear.PressRightMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Spear.ReleaseRightMouse();
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Spear) itemLocalObj_Spear.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Spear) itemLocalObj_Spear.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 精钢匕首
/// </summary>
public class Item_2102 : ItemBase_Weapon
{
    #region//基础数值
    /// <summary>
    /// 攻击伤害
    /// </summary>
    private readonly int AttackDamage_Base = 8;
    /// <summary>
    /// 攻击伤害修正
    /// </summary>
    private int AttackDamage_Add = 0;
    /// <summary>
    /// 攻击速度
    /// </summary>
    private readonly float AttackSpeed_Base = 1f;
    /// <summary>
    /// 攻击速度修正
    /// </summary>
    private float AttackSpeed_Add = 0;
    /// <summary>
    /// 攻击损耗
    /// </summary>
    private readonly float AttackExpend_Base = 0.8f;
    /// <summary>
    /// 攻击损耗修正
    /// </summary>
    private float AttackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackExpend/", (AttackExpend_Base + AttackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        AttackSpeed_Add = 0;
        AttackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            AttackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            AttackSpeed_Add += 0.2f;
            AttackExpend_Add -= (float)Math.Round(AttackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AttackDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AttackSpeed_Add += 0.2f;
            AttackExpend_Add -= (float)Math.Round(AttackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 4;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            AttackExpend_Add = -AttackExpend_Base;
        }
        if (itemLocalObj_Dagger)
        {
            itemLocalObj_Dagger.UpdateDaggerData(AttackDamage_Base + AttackDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackExpend_Base + AttackExpend_Add, itemQuality);
        }
    }
    #endregion

    #region//使用逻辑
    private ItemLocalObj_Dagger itemLocalObj_Dagger;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Dagger = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2102").GetComponent<ItemLocalObj_Dagger>();
        itemLocalObj_Dagger.InitData(itemData);
        itemLocalObj_Dagger.HoldingStart(owner, body);
        itemLocalObj_Dagger.UpdateDaggerData(AttackDamage_Base + AttackDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackExpend_Base + AttackExpend_Add, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Dagger.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Dagger.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Dagger.ReleaseLeftMouse();
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Dagger.PressRightMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Dagger.ReleaseRightMouse();
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Dagger) itemLocalObj_Dagger.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Dagger) itemLocalObj_Dagger.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 铁剑
/// </summary>
public class Item_2103 : ItemBase_Weapon
{
    #region//基础数值
    /// <summary>
    /// 攻击伤害
    /// </summary>
    private readonly int AttackDamage_Base = 6;
    /// <summary>
    /// 攻击伤害修正
    /// </summary>
    private int AttackDamage_Add = 0;
    /// <summary>
    /// 攻击速度
    /// </summary>
    private readonly float AttackSpeed_Base = 1.5f;
    /// <summary>
    /// 攻击速度修正
    /// </summary>
    private float AttackSpeed_Add = 0;
    /// <summary>
    /// 攻击损耗
    /// </summary>
    private readonly float AttackExpend_Base = 0.5f;
    /// <summary>
    /// 攻击损耗修正
    /// </summary>
    private float AttackExpend_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackExpend/", (AttackExpend_Base + AttackExpend_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        AttackSpeed_Add = 0;
        AttackExpend_Add = 0;
        if (itemQuality >= ItemQuality.Green)
        {
            AttackSpeed_Add += 0.2f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            AttackSpeed_Add += 0.2f;
            AttackExpend_Add -= (float)Math.Round(AttackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AttackDamage_Add += 1;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AttackSpeed_Add += 0.2f;
            AttackExpend_Add -= (float)Math.Round(AttackExpend_Base * 0.3f, 2);
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 4;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            AttackExpend_Add = -AttackExpend_Base;
        }
        if (itemLocalObj_Broadsword)
        {
            itemLocalObj_Broadsword.UpdateBroadswordData(AttackDamage_Base + AttackDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackExpend_Base + AttackExpend_Add, itemQuality);
        }
    }
    #endregion

    #region//使用逻辑
    private ItemLocalObj_Broadsword itemLocalObj_Broadsword;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Broadsword = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2103").GetComponent<ItemLocalObj_Broadsword>();
        itemLocalObj_Broadsword.InitData(itemData);
        itemLocalObj_Broadsword.HoldingStart(owner, body);
        itemLocalObj_Broadsword.UpdateBroadswordData(AttackDamage_Base + AttackDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackExpend_Base + AttackExpend_Add, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Broadsword.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Broadsword.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Broadsword.ReleaseLeftMouse();
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Broadsword.PressRightMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Broadsword.ReleaseRightMouse();
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Broadsword) itemLocalObj_Broadsword.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Broadsword) itemLocalObj_Broadsword.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
#endregion
#region//远程武器
/// <summary>
/// 粗制木弓
/// </summary>
public class Item_2200 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 10;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 60;
    private int ShotSpeed_Add;
    /// <summary>
    /// 准备速度
    /// </summary>
    private readonly int ReadySpeed_Base = 1;
    private int ReadySpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 1;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 90;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 20;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.2f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/ReadySpeed/", (ReadySpeed_Base + ReadySpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        ReadySpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Bow)
        {
            itemLocalObj_Bow.UpdateBowData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap =(short)(BulletCapacity_Base);
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Arrow)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Bow itemLocalObj_Bow;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2200").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.InitData(itemData);
        itemLocalObj_Bow.HoldingStart(owner, body);
        itemLocalObj_Bow.UpdateBowData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow.ReleaseRightMouse();
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Bow.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Bow) itemLocalObj_Bow.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Bow) itemLocalObj_Bow.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
/// <summary>
/// 精制木弓
/// </summary>
public class Item_2201 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 15;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 90;
    private int ShotSpeed_Add;
    /// <summary>
    /// 准备速度
    /// </summary>
    private readonly int ReadySpeed_Base = 1;
    private int ReadySpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 2;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 60;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 10;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.2f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/ReadySpeed/", (ReadySpeed_Base + ReadySpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        ReadySpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Bow)
        {
            itemLocalObj_Bow.UpdateBowData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 10;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)(BulletCapacity_Base);
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Arrow)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Bow itemLocalObj_Bow;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2201").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.InitData(itemData);
        itemLocalObj_Bow.HoldingStart(owner, body);
        itemLocalObj_Bow.UpdateBowData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow.ReleaseRightMouse();
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Bow.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Bow) itemLocalObj_Bow.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Bow) itemLocalObj_Bow.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
/// <summary>
/// 黄金弓
/// </summary>
public class Item_2202 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 15;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 120;
    private int ShotSpeed_Add;
    /// <summary>
    /// 准备速度
    /// </summary>
    private readonly int ReadySpeed_Base = 2;
    private int ReadySpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 5;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 30;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 5;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.1f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/ReadySpeed/", (ReadySpeed_Base + ReadySpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        ReadySpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Bow)
        {
            itemLocalObj_Bow.UpdateBowData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion

    #region//装填
    short config_bulletCapacity = 10;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)(BulletCapacity_Base);
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Arrow)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Bow itemLocalObj_Bow;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2202").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.InitData(itemData);
        itemLocalObj_Bow.HoldingStart(owner, body);
        itemLocalObj_Bow.UpdateBowData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow.ReleaseRightMouse();
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Bow.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Bow) itemLocalObj_Bow.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Bow) itemLocalObj_Bow.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
#endregion
#region//热武器
/// <summary>
/// 土质手枪
/// </summary>
public class Item_2300 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 10;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 60;
    private int ShotSpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 2;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 120;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 45;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.5f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Pistol)
        {
            itemLocalObj_Pistol.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)(BulletCapacity_Base);
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Bullet)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Pistol itemLocalObj_Pistol;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Pistol = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2300").GetComponent<ItemLocalObj_Pistol>();
        itemLocalObj_Pistol.InitData(itemData);
        itemLocalObj_Pistol.HoldingStart(owner, body);
        itemLocalObj_Pistol.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Pistol.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Pistol) itemLocalObj_Pistol.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Pistol) itemLocalObj_Pistol.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 短型冲锋枪
/// </summary>
public class Item_2301 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 25;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 600;
    private int ShotSpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 5;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 90;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 45;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.1f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Pistol)
        {
            itemLocalObj_Pistol.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)BulletCapacity_Base;
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Bullet)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Pistol itemLocalObj_Pistol;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Pistol = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2301").GetComponent<ItemLocalObj_Pistol>();
        itemLocalObj_Pistol.InitData(itemData);
        itemLocalObj_Pistol.HoldingStart(owner, body);
        itemLocalObj_Pistol.UpdateGunData(
            AttackDamage_Base + AttackDamage_Add,
            MagicDamage_Base + MagicDamage_Add,
            BulletCapacity_Base,
            ShotSpeed_Base + ShotSpeed_Add,
            AimSpeed_Base + AimSpeed_Add,
            AimRangeMin_Base + AimRangeMin_Add,
            AimRangeMax_Base + AimRangeMax_Add,
            Recoil_Base + Recoil_Add,
            itemQuality);

        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Pistol.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Pistol) itemLocalObj_Pistol.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Pistol) itemLocalObj_Pistol.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 制式自动步枪
/// </summary>
public class Item_2302 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 35;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 300;
    private int ShotSpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 4;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 45;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 10;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.25f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Rifle)
        {
            itemLocalObj_Rifle.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)BulletCapacity_Base;
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Bullet)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Rifle itemLocalObj_Rifle;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Rifle = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2302").GetComponent<ItemLocalObj_Rifle>();
        itemLocalObj_Rifle.InitData(itemData);
        itemLocalObj_Rifle.HoldingStart(owner, body);
        itemLocalObj_Rifle.UpdateGunData(
            AttackDamage_Base + AttackDamage_Add,
            MagicDamage_Base + MagicDamage_Add,
            BulletCapacity_Base,
            ShotSpeed_Base + ShotSpeed_Add,
            AimSpeed_Base + AimSpeed_Add,
            AimRangeMin_Base + AimRangeMin_Add,
            AimRangeMax_Base + AimRangeMax_Add,
            Recoil_Base + Recoil_Add,
            itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Rifle.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Rifle) itemLocalObj_Rifle.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Rifle) itemLocalObj_Rifle.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 制式手枪
/// </summary>
public class Item_2303 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 10;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 120;
    private int ShotSpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 6;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 45;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 20;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.5f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Pistol)
        {
            itemLocalObj_Pistol.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)BulletCapacity_Base;
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Bullet)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Pistol itemLocalObj_Pistol;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Pistol = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2303").GetComponent<ItemLocalObj_Pistol>();
        itemLocalObj_Pistol.InitData(itemData);
        itemLocalObj_Pistol.HoldingStart(owner, body);
        if (itemLocalObj_Pistol)
        {
            itemLocalObj_Pistol.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }

        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Pistol.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Pistol) itemLocalObj_Pistol.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Pistol) itemLocalObj_Pistol.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 泵动霰弹枪
/// </summary>
public class Item_2304 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 5;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 90;
    private int ShotSpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 6;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 60;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 30;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.2f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_ScatterGun)
        {
            itemLocalObj_ScatterGun.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)BulletCapacity_Base;
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Bullet)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_ScatterGun itemLocalObj_ScatterGun;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_ScatterGun = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2304").GetComponent<ItemLocalObj_ScatterGun>();
        itemLocalObj_ScatterGun.InitData(itemData);
        itemLocalObj_ScatterGun.HoldingStart(owner, body);
        itemLocalObj_ScatterGun.UpdateGunData(
            AttackDamage_Base + AttackDamage_Add,
            MagicDamage_Base + MagicDamage_Add,
            BulletCapacity_Base,
            ShotSpeed_Base + ShotSpeed_Add,
            AimSpeed_Base + AimSpeed_Add,
            AimRangeMin_Base + AimRangeMin_Add,
            AimRangeMax_Base + AimRangeMax_Add,
            Recoil_Base + Recoil_Add,
            itemQuality);

        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_ScatterGun.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_ScatterGun.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_ScatterGun.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_ScatterGun.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_ScatterGun.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_ScatterGun) itemLocalObj_ScatterGun.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_ScatterGun) itemLocalObj_ScatterGun.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 麦德森机枪
/// </summary>
public class Item_2305 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 60;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 480;
    private int ShotSpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 3;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 60;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 30;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.1f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Rifle)
        {
            itemLocalObj_Rifle.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)BulletCapacity_Base;
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Bullet)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Rifle itemLocalObj_Rifle;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Rifle = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2305").GetComponent<ItemLocalObj_Rifle>();
        itemLocalObj_Rifle.InitData(itemData);
        itemLocalObj_Rifle.HoldingStart(owner, body);
        itemLocalObj_Rifle.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Rifle.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Rifle) itemLocalObj_Rifle.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Rifle) itemLocalObj_Rifle.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 精准自动步枪
/// </summary>
public class Item_2306 : ItemBase_Gun
{
    #region//基础数值
    /// <summary>
    /// 物理伤害
    /// </summary>
    private readonly int AttackDamage_Base = 0;
    private int AttackDamage_Add;
    /// <summary>
    /// 魔法伤害
    /// </summary>
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    /// <summary>
    /// 弹容量
    /// </summary>
    private readonly int BulletCapacity_Base = 30;
    /// <summary>
    /// 射击速度
    /// </summary>
    private readonly int ShotSpeed_Base = 420;
    private int ShotSpeed_Add;
    /// <summary>
    /// 瞄准速度
    /// </summary>
    private readonly float AimSpeed_Base = 4;
    private float AimSpeed_Add;
    /// <summary>
    /// 最大射击角度
    /// </summary>
    private readonly int AimRangeMax_Base = 45;
    private int AimRangeMax_Add;
    /// <summary>
    /// 最小射击角度
    /// </summary>
    private readonly int AimRangeMin_Base = 10;
    private int AimRangeMin_Add;
    /// <summary>
    /// 射击角度扩散
    /// </summary>
    private readonly float Recoil_Base = 0.05f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackDamage/", (AttackDamage_Base + AttackDamage_Add).ToString());
        desc = desc.Replace("/MagicDamage/", (MagicDamage_Base + MagicDamage_Add).ToString());
        desc = desc.Replace("/BulletCapacity/", (BulletCapacity_Base).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", (AimRangeMin_Base + AimRangeMin_Add).ToString() + "-" + (AimRangeMax_Base + AimRangeMax_Add).ToString());
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDamage_Add = 0;
        MagicDamage_Add = 0;
        ShotSpeed_Add = 0;
        AimSpeed_Add = 0;
        AimRangeMax_Add = 0;
        AimRangeMin_Add = 0;
        Recoil_Add = 0;
        if (itemQuality >= ItemQuality.Gray)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Green)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Blue)
        {
            ShotSpeed_Add += 5;
        }
        if (itemQuality >= ItemQuality.Purple)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Gold)
        {
            AimSpeed_Add += 0.5f;
        }
        if (itemQuality >= ItemQuality.Red)
        {
            AttackDamage_Add += 5;
        }
        if (itemQuality >= ItemQuality.Rainbow)
        {
            MagicDamage_Add += 5;
        }
        if (itemLocalObj_Rifle)
        {
            itemLocalObj_Rifle.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        }
    }
    #endregion
    #region//装填
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = (short)BulletCapacity_Base;
        if (oldContent.Item_Content.Item_Count == 0 || oldContent.Item_Content.Item_ID == 0 || oldContent.Item_Content.Item_ID == addItem.Item_ID)
        {
            if (ItemConfigData.GetItemConfig(addItem.Item_ID).Item_Type == ItemType.Bullet)
            {
                if (oldContent.Item_Content.Item_Count + addItem.Item_Count <= cap)
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = (short)(oldContent.Item_Content.Item_Count + addItem.Item_Count);
                    resItem.Item_Count = 0;
                }
                else
                {
                    newContent.Item_Content.Item_ID = addItem.Item_ID;
                    newContent.Item_Content.Item_Count = cap;
                    resItem.Item_Count = (short)(addItem.Item_Count + oldContent.Item_Content.Item_Count - cap);
                }
            }
        }
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Rifle itemLocalObj_Rifle;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Rifle = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2306").GetComponent<ItemLocalObj_Rifle>();
        itemLocalObj_Rifle.InitData(itemData);
        itemLocalObj_Rifle.HoldingStart(owner, body);
        itemLocalObj_Rifle.UpdateGunData(
                AttackDamage_Base + AttackDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                BulletCapacity_Base,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Rifle.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Rifle) itemLocalObj_Rifle.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Rifle) itemLocalObj_Rifle.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
#endregion

