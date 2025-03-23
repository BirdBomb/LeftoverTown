using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
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
        bodyController = actorManager.GetComponent<BodyController_Human>();
    }
    public void Listen_UpdateSecond(int val)
    {
        if (itemBase_OnHand != null)
        {
            itemBase_OnHand.Holding_UpdateTime(val);
        }
        if (itemBase_OnHead != null)
        {
            
        }
        if (itemBase_OnBody != null)
        {
            
        }
    }

    #region//手
    public void UpdateItemInHand(ItemData data)
    {
        if (bodyController == null)
        {
            return;
        }
        if (data.Item_ID >= 0)
        {
            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInHand()
                {
                    itemData = data
                });
            }
            if (lastItemID_OnHand != data.Item_ID)
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
                    itemBase_OnHand.Holding_UpdateLook();
                }
            }
        }
        else
        {
            ResetItemInHand();
        }
        lastItemID_OnHand = data.Item_ID;
    }
    private void CreateItemInHand(ItemData data)
    {
        Type type = Type.GetType("Item_" + data.Item_ID.ToString());
        itemBase_OnHand = (ItemBase)Activator.CreateInstance(type);
        itemBase_OnHand.UpdateDataFromNet(data);
        itemBase_OnHand.Holding_Start(actorManager, bodyController);
        itemBase_OnHand.Holding_UpdateLook();
    }
    private void ResetItemInHand()
    {
        if (itemBase_OnHand != null) { itemBase_OnHand.Holding_Over(actorManager); }
        itemBase_OnHand = new ItemBase();
        if (bodyController.gameObjects_ItemInHand.Count > 0)
        {
            for (int i = 0; i < bodyController.gameObjects_ItemInHand.Count; i++)
            {
                UnityEngine.Object.Destroy(bodyController.gameObjects_ItemInHand[i]);
            }
        }
        bodyController.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = true;
        bodyController.transform_RightHand.GetComponent<SpriteRenderer>().enabled = true;

        bodyController.transform_ItemInRightHand.localScale = Vector3.one;
        bodyController.transform_ItemInRightHand.localPosition = Vector3.zero;
        bodyController.transform_ItemInRightHand.localRotation = Quaternion.identity;
        bodyController.transform_ItemInLeftHand.localScale = Vector3.one;
        bodyController.transform_ItemInLeftHand.localPosition = Vector3.zero;
        bodyController.transform_ItemInLeftHand.localRotation = Quaternion.identity;
        bodyController.transform_ItemInLeftHand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.transform_ItemInLeftHand.GetComponent<SpriteRenderer>().sortingOrder = 1;

        bodyController.transform_ItemInRightHand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        bodyController.transform_ItemInRightHand.GetComponent<SpriteRenderer>().sortingOrder = 4;

    }
    #endregion
    #region//头
    public void UpdateItemOnHead(ItemData data)
    {
        if (bodyController == null)
        {
            return;
        }
        if (data.Item_ID >= 0)
        {
            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateStatus()
                {
                    statusType = actorManager.actorNetManager.statusType
                });
                MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemOnHead()
                {
                    itemData = data
                });
            }
            if (lastItemID_OnHead != data.Item_ID)
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
        lastItemID_OnHead = data.Item_ID;
    }
    public void CreateItemOnHead(ItemData data)
    {
        Type type = Type.GetType("Item_" + data.Item_ID.ToString());
        itemBase_OnHead = (ItemBase)Activator.CreateInstance(type);
        itemBase_OnHead.UpdateDataFromNet(data);
        itemBase_OnHead.BeWearingOnHead(actorManager, bodyController);

    }
    public void ResetItemOnHead()
    {
        itemBase_OnHead = new ItemBase();
        if (bodyController.gameObjects_ItemOnHead.Count > 0)
        {
            for (int i = 0; i < bodyController.gameObjects_ItemOnHead.Count; i++)
            {
                UnityEngine.Object.Destroy(bodyController.gameObjects_ItemOnHead[i]);
            }
        }
        bodyController.transform_ItemOnHead.GetComponent<SpriteRenderer>().sprite = null;
    }
    #endregion
    #region//身
    public void UpdateItemOnBody(ItemData data)
    {
        if (bodyController == null)
        {
            return;
        }
        if (data.Item_ID >= 0)
        {
            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateStatus()
                {
                    statusType = actorManager.actorNetManager.statusType
                });
                MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemOnBody()
                {
                    itemData = data
                });
            }
            if (lastItemID_OnBody != data.Item_ID)
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
        lastItemID_OnBody = data.Item_ID;
    }
    public void CreateItemOnBody(ItemData data)
    {
        Type type = Type.GetType("Item_" + data.Item_ID.ToString());
        itemBase_OnBody = (ItemBase)Activator.CreateInstance(type);
        itemBase_OnBody.UpdateDataFromNet(data);
        itemBase_OnBody.BeWearingOnBody(actorManager, bodyController);

    }
    public void ResetItemOnBody()
    {
        itemBase_OnBody = new ItemBase();
        if (bodyController.gameObjects_ItemOnBody.Count > 0)
        {
            for (int i = 0; i < bodyController.gameObjects_ItemOnBody.Count; i++)
            {
                UnityEngine.Object.Destroy(bodyController.gameObjects_ItemOnBody[i]);
            }
        }
        bodyController.transform_ItemOnBody.GetComponent<SpriteRenderer>().sprite = null;
    }
    #endregion
    #region//生成
    public ItemData CreateItemData(short id, short count = 1)
    {
        Type type = Type.GetType("Item_" + id.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(id, out ItemData initData);
        initData.Item_Count = count;
        return initData;
    }
    #endregion
}
