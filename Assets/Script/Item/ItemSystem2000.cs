using DG.Tweening;
using Fusion;
using System;
using System.Collections.Generic;
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
    #region//持有
    private ItemLocalObj_Torch itemLocalObj_Torch;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Torch = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2000").GetComponent<ItemLocalObj_Torch>();
        itemLocalObj_Torch.HoldingByHand(owner, body, itemData);
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
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2010").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.HoldingByHand(owner, body, itemData);
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
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2011").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.HoldingByHand(owner, body, itemData);
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
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2020").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.HoldingByHand(owner, body, itemData);
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
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2021").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.HoldingByHand(owner, body, itemData);
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
    public override void StaticAction_InitData(short id, out ItemData data)
    {
        base.StaticAction_InitData(id, out data);
        data.Item_Info = (short)new System.Random().Next(0, short.MaxValue);
    }
    #region//使用逻辑 
    private ItemLocalObj_FishRod itemLocalObj_FishRod;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_FishRod = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2030").GetComponent<ItemLocalObj_FishRod>();
        itemLocalObj_FishRod.HoldingByHand(owner, body, itemData);
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
#endregion
#region//近战武器
/// <summary>
/// 木棍
/// </summary>
public class Item_2100 : ItemBase_Weapon
{
    #region//使用逻辑
    private ItemLocalObj_Spear itemLocalObj_Spear;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Spear = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2100").GetComponent<ItemLocalObj_Spear>();
        itemLocalObj_Spear.HoldingByHand(owner, body, itemData);
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
    #region//使用逻辑
    private ItemLocalObj_Spear itemLocalObj_Spear;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Spear = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2101").GetComponent<ItemLocalObj_Spear>();
        itemLocalObj_Spear.HoldingByHand(owner, body, itemData);
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
    #region//使用逻辑
    private ItemLocalObj_Dagger itemLocalObj_Dagger;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Dagger = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2102").GetComponent<ItemLocalObj_Dagger>();
        itemLocalObj_Dagger.HoldingByHand(owner, body, itemData);
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
/// 宽刃钢刀
/// </summary>
public class Item_2103 : ItemBase_Weapon
{
    #region//使用逻辑
    private ItemLocalObj_Broadsword itemLocalObj_Broadsword;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Broadsword = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2103").GetComponent<ItemLocalObj_Broadsword>();
        itemLocalObj_Broadsword.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 10;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Bow.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 10;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Bow.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 10;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Bow.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 15;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Pistol.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 25;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Pistol.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 30;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Rifle.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 15;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Pistol.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 5;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_ScatterGun.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 50;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Rifle.HoldingByHand(owner, body, itemData);
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
    #region//UI交互
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenOrCloseGrid(itemData);
        base.GridCell_RightClick(gridCell, itemData);
    }
    #endregion
    #region//装填
    short config_bulletCapacity = 25;
    public override void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
        short cap = config_bulletCapacity;
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
        itemLocalObj_Rifle.HoldingByHand(owner, body, itemData);
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

