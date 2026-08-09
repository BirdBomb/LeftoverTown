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
    private int config_Food = 5;
    public override void OnHand_EatAction(ActorManager actor)
    {
        actor.hungryManager.AddFood(config_Food);
        if (actor.actorAuthority.isLocal && actor.actorAuthority.isPlayer)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = 22
            });
        }
        base.OnHand_EatAction(actor);
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
        itemLocalObj_Seed?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Seed?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Seed?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Seed?.UpdateDataByLocal(data);
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
        itemLocalObj_Seed?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Seed?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Seed?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Seed?.UpdateDataByLocal(data);
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
        itemLocalObj_Seed?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Seed?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Seed?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Seed?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 土质手雷
/// </summary>
public class Item_6200 : ItemBase_Throwable
{
    #region//基础数值
    private readonly int BludgeoningDamage_Base = 200;
    private int BludgeoningDamage_Add;
    private readonly float ExplodeRange_Base = 1.5f;
    private float ExplodeRange_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/BludgeoningDamage/", ((BludgeoningDamage_Base + BludgeoningDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ExplodeRange/", (ExplodeRange_Base + ExplodeRange_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    break;
                }
            case ItemQuality.Green:
                {
                    break;
                }
            case ItemQuality.Blue:
                {
                    break;
                }
            case ItemQuality.Purple:
                {
                    break;
                }
            case ItemQuality.Gold:
                {
                    break;
                }
            case ItemQuality.Red:
                {
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    break;
                }
        }
        itemLocalObj_Bomb?.UpdateBombData(BludgeoningDamage_Base + BludgeoningDamage_Add, ExplodeRange_Base + ExplodeRange_Add, itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Bomb itemLocalObj_Bomb;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Bomb = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_6200").GetComponent<ItemLocalObj_Bomb>();
        itemLocalObj_Bomb.InitData(itemData);
        itemLocalObj_Bomb.HoldingStart(owner, body);
        itemLocalObj_Bomb?.UpdateBombData(BludgeoningDamage_Base + BludgeoningDamage_Add, ExplodeRange_Base + ExplodeRange_Add, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bomb.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bomb.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bomb.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bomb.ReleaseRightMouse();
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Bomb.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Bomb) itemLocalObj_Bomb.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Bomb) itemLocalObj_Bomb.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}

