using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class ActorItemManager 
{
    private ActorManager actorManager;
    private BodyController_Human bodyController;
    [HideInInspector]
    public ItemBase itemBase_OnHand = new ItemBase();
    private int lastItemID_OnHand = -1;
    [HideInInspector]
    public ItemBase itemBase_OnHead = new ItemBase();
    private int lastItemID_OnHead = -1;
    [HideInInspector]
    public ItemBase itemBase_OnBody = new ItemBase();
    private int lastItemID_OnBody = -1;
    [HideInInspector]
    public List<ItemBase> itemBases_InBag = new List<ItemBase>();
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
        bodyController = actorManager.GetComponentInChildren<BodyController_Human>();
    }
    public void Listen_UpdateSecond(int val)
    {
        itemBase_OnHand?.OnHand_UpdateTime(val);
        itemBase_OnHead?.OnHead_UpdateTime(val);
        itemBase_OnBody?.OnBody_UpdateTime(val);
    }
    #region//手
    public void UpdateItemHand(ItemData data)
    {
        if (bodyController == null)
        {
            return;
        }
        if (data.I >= 0)
        {
            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_ItemHand_Update()
                {
                    itemData = data
                });
            }
            if (lastItemID_OnHand != data.I)
            {
                /*异类物体*/
                ResetItemInHand();
                CreateItemInHand(data);
            }
            else
            {
                /*同类物体*/
                if (itemBase_OnHand == null)
                {
                    CreateItemInHand(data);
                }
                else
                {
                    itemBase_OnHand.UpdateDataFromNet(data);
                    itemBase_OnHand.OnHand_UpdateLook();
                }
            }
        }
        else
        {
            ResetItemInHand();
        }
        lastItemID_OnHand = data.I;
    }
    private void CreateItemInHand(ItemData data)
    {
        Type type = Type.GetType("Item_" + data.I.ToString());
        itemBase_OnHand = (ItemBase)Activator.CreateInstance(type);
        itemBase_OnHand.UpdateDataFromNet(data);
        itemBase_OnHand.OnHand_Start(actorManager, bodyController);
        itemBase_OnHand.OnHand_UpdateLook();
    }
    private void ResetItemInHand()
    {
        if (itemBase_OnHand != null) { itemBase_OnHand.OnHand_Over(actorManager, bodyController); }
        itemBase_OnHand = new ItemBase();
        bodyController.CleanItemInHand();
        bodyController.ShowRightHand(true);
        bodyController.ShowRightHandItem(0);
        bodyController.ShowLeftHand(true);
        bodyController.ShowLeftHandItem(0);
    }
    #endregion
    #region//头
    public void UpdateItemHead(ItemData data)
    {
        if (bodyController == null)
        {
            return;
        }
        if (data.I >= 0)
        {
            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_ItemHead_Update()
                {
                    itemData = data
                });
            }
            if (lastItemID_OnHead != data.I)
            {
                /*异类物体*/
                ResetItemOnHead();
                CreateItemOnHead(data);
            }
            else
            {
                /*同类物体*/
                if (itemBase_OnHead == null)
                {
                    CreateItemOnHead(data);
                }
                else
                {
                    itemBase_OnHead.UpdateDataFromNet(data);
                }
            }
        }
        else
        {
            ResetItemOnHead();
        }
        if (actorManager.actorAuthority.isLocal)
        {
            actorManager.actionManager.Local_ResetArmor();
            actorManager.actionManager.Local_ResetResistance();
        }
        lastItemID_OnHead = data.I;
    }
    public void CreateItemOnHead(ItemData data)
    {
        Type type = Type.GetType("Item_" + data.I.ToString());
        itemBase_OnHead = (ItemBase)Activator.CreateInstance(type);
        itemBase_OnHead.UpdateDataFromNet(data);
        itemBase_OnHead.OnHead_Start(actorManager, bodyController);

    }
    public void ResetItemOnHead()
    {
        if (itemBase_OnHead != null) { itemBase_OnHead.OnHead_Over(actorManager, bodyController); }
        itemBase_OnHead = new ItemBase();
        bodyController.CleanItemOnHead();
    }
    #endregion
    #region//身
    public void UpdateItemBody(ItemData data)
    {
        if (bodyController == null) return;
        if (data.I >= 0)
        {
            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_ItemBody_Update()
                {
                    itemData = data
                });
            }
            if (lastItemID_OnBody != data.I)
            {
                ResetItemOnBody();
                CreateItemOnBody(data);
            }
            else
            {
                if (itemBase_OnBody == null)
                {
                    CreateItemOnBody(data);
                }
                else
                {
                    itemBase_OnBody.UpdateDataFromNet(data);
                }
            }
        }
        else
        {
            ResetItemOnBody();
        }
        if (actorManager.actorAuthority.isLocal)
        {
            actorManager.actionManager.Local_ResetArmor();
            actorManager.actionManager.Local_ResetResistance();
        }
        lastItemID_OnBody = data.I;
    }
    public void CreateItemOnBody(ItemData data)
    {
        Type type = Type.GetType("Item_" + data.I.ToString());
        itemBase_OnBody = (ItemBase)Activator.CreateInstance(type);
        itemBase_OnBody.UpdateDataFromNet(data);
        itemBase_OnBody.OnBody_Start(actorManager, bodyController);

    }
    public void ResetItemOnBody()
    {
        if (itemBase_OnBody != null) { itemBase_OnBody.OnBody_Over(actorManager, bodyController); }
        itemBase_OnBody = new ItemBase();
        bodyController.CleanItemOnBody();
    }
    #endregion
    #region//饰品
    public void UpdateItemAccessory(ItemData data)
    {
        if (bodyController == null)return;
        if (data.I >= 0 && actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_ItemAccessory_Update()
            {
                itemData = data
            });
        }
    }

    #endregion
    #region//耗材
    public void UpdateItemConsumables(ItemData data)
    {
        if (bodyController == null) return;
        if (data.I >= 0 && actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_ItemConsumables_Update()
            {
                itemData = data
            });
        }
    }
    #endregion
    #region//生成
    public ItemData CreateItemData(short id, short count = 1)
    {
        Type type = Type.GetType("Item_" + id.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(id, out ItemData initData);
        initData.C = count;
        return initData;
    }
    #endregion
}
