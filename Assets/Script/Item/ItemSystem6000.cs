using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemSystem6000 
{

}
/// <summary>
/// 生命水晶
/// </summary>
public class Item_6000 : ItemBase_Food
{
    private int config_MaxHp = 5;
    public override void Eat()
    {
        owner.actorHpManager.IncreaseHP(config_MaxHp);
        if (owner.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = 22
            });
        }
        base.Eat();
    }
}
/// <summary>
/// 薯果种子
/// </summary>
public class Item_6100 : ItemBase
{
    #region//使用逻辑
    private ItemLocalObj_Seed itemLocalObj_Seed;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Seed = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_6100").GetComponent<ItemLocalObj_Seed>();
        itemLocalObj_Seed.InitData(itemData);
        itemLocalObj_Seed.HoldingStart(owner, body);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Seed.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Seed.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Seed.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Seed) itemLocalObj_Seed.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Seed) itemLocalObj_Seed.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion

}
/// <summary>
/// 辣椒种子
/// </summary>
public class Item_6101 : ItemBase
{
    #region//使用逻辑
    private ItemLocalObj_Seed itemLocalObj_Seed;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Seed = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_6101").GetComponent<ItemLocalObj_Seed>();
        itemLocalObj_Seed.InitData(itemData);
        itemLocalObj_Seed.HoldingStart(owner, body);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Seed.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Seed.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Seed.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Seed) itemLocalObj_Seed.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Seed) itemLocalObj_Seed.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 小麦种子
/// </summary>
public class Item_6102 : ItemBase
{
    #region//使用逻辑
    private ItemLocalObj_Seed itemLocalObj_Seed;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Seed = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_6102").GetComponent<ItemLocalObj_Seed>();
        itemLocalObj_Seed.InitData(itemData);
        itemLocalObj_Seed.HoldingStart(owner, body);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Seed.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Seed.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Seed.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Seed) itemLocalObj_Seed.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Seed) itemLocalObj_Seed.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}

