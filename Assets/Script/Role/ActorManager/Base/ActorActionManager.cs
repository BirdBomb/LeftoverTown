using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ActorActionManager 
{
    private ActorManager actorManager;
    private BodyController_Base bodyController;
    private LayerMask layerMask_ItemObj;
    private LayerMask layerMask_Wall;
    public Action<int, ActorNetManager> action_TakeDamage;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
        bodyController = actorManager.bodyController;
        layerMask_ItemObj = LayerMask.GetMask("ItemObj");
        layerMask_Wall = LayerMask.GetMask("TileObj_Wall");
    }
    public void AddForce(Vector3 dir, float force)
    {
        actorManager.actorNetManager.networkRigidbody.Rigidbody.velocity = dir * force;
    }
    /// <summary>
    /// 捡起
    /// </summary>
    /// <param name="radiu"></param>
    public void PickUp(float radiu)
    {
        var items = Physics2D.OverlapCircleAll(actorManager.transform.position, radiu, layerMask_ItemObj);
        foreach (Collider2D item in items)
        {
            if (item.gameObject.transform.parent.TryGetComponent(out ItemNetObj obj))
            {
                actorManager.actorNetManager.RPC_LocalInput_PickItem(obj.Object.Id);
                break;
            }
        }
    }
    /// <summary>
    /// 快速切换
    /// </summary>
    public void Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_GetBagItem();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldHandItem = actorManager.actorNetManager.Net_ItemInHand;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            index = index,
            itemData = oldHandItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemOnHand()
        {
            oldItem = oldHandItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速丢弃
    /// </summary>
    /// <param name="index"></param>
    public void Drop(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_GetBagItem();
        index = index % items.Count;
        ItemData dropItem = items[index];
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            index = index,
            itemData = new ItemData(),
        }) ;
        if(dropItem.Item_ID != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = dropItem
            });
        }
    }
    public void FaceTo(Vector2 dir)
    {
        bodyController.faceDir = dir.normalized;
        if (dir.x > 0) bodyController.FaceRight();
        else bodyController.FaceLeft();
    }
    public void TurnTo(Vector2 dir)
    {
        bodyController.turnDir = dir.normalized;
        if (dir.x > 0.1f) bodyController.TurnRight();
        if (dir.x < -0.1f) bodyController.TurnLeft();
    }
    public bool LookAt(ActorManager who, float view)
    {
        if (who)
        {
            if (Vector3.Distance(actorManager.transform.position, who.transform.position) >= view)
            {
                return false;
            }
            if (Physics2D.LinecastAll(who.transform.position, actorManager.transform.position, layerMask_Wall).Length > 0)
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public bool HearTo(ActorManager who, float view)
    {
        if (who != actorManager)//这个人不是我
        {
            if (Vector3.Distance(actorManager.transform.position, who.transform.position) >= view)
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public void Dead()
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            actorManager.actorState = ActorState.Dead;
            actorManager.AllClient_UpdateHpBar(0);
            if (actorManager.actorAuthority.isPlayer)
            {
                DeadByPlayer();
            }
            else
            {
                DeadByNPC();
            }
        }
    }
    private void DeadByPlayer()
    {
        if (actorManager.actorAuthority.isState)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_RevivePlayer()
            {
                playerRef = actorManager.actorPlayerRef
            });
            Despawn();
        }
    }
    private void DeadByNPC()
    {
        if (actorManager.actorAuthority.isState)
        {
            DropDown();
            Despawn();
        }
    }
    public void Despawn()
    {
        actorManager.actorNetManager.Runner.Despawn(actorManager.actorNetManager.Object);
    }
    /// <summary>
    /// 掉落所有物体
    /// </summary>
    public void DropDown()
    {
        List<ItemData> dropItem = new List<ItemData>();
        List<ItemData> bagItems = actorManager.actorNetManager.Local_GetBagItem();
        for (int i = 0; i < bagItems.Count; i++)
        {
            ItemData item = bagItems[i];
            if (item.Item_ID != 0)
            {
                dropItem.Add(item);
            }
        }
        ItemData itemOnHead = actorManager.actorNetManager.Net_ItemOnHead;
        if (itemOnHead.Item_ID != 0)
        {
            dropItem.Add(itemOnHead);
        }
        ItemData itemOnBody = actorManager.actorNetManager.Net_ItemOnBody;
        if (itemOnBody.Item_ID != 0)
        {
            dropItem.Add(itemOnBody);
        }
        ItemData itemOnHand = actorManager.actorNetManager.Net_ItemInHand;
        if (itemOnHand.Item_ID != 0)
        {
            dropItem.Add(itemOnHand);
        }
        for (int i = 0; i < dropItem.Count; i++)
        {
            float angle = i * (360 / dropItem.Count);
            float angleRad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(angleRad) * 0.5f;
            float y = Mathf.Sin(angleRad) * 0.5f;
            Debug.Log(dropItem[i].Item_ID + "/" + dropItem[i].Item_Count);
            Vector3 position = new Vector3(x, y, 0);
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = dropItem[i],
                pos = position + actorManager.transform.position,
            });
        }
    }
    public void Heal(int val)
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            actorManager.actorNetManager.RPC_AllClient_HpChange(val, new NetworkId());
        }
    }
    /// <summary>
    /// 物理伤害
    /// </summary>
    /// <param name="val"></param>
    /// <param name="from"></param>
    public void TakeAttackDamage(int val, ActorNetManager from)
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            NetworkId networkId = new NetworkId();
            if (from) { networkId = from.Object.Id; }
            val -= actorManager.actorNetManager.Net_Armor;
            if (val > 0)
            {
                actorManager.actorNetManager.RPC_AllClient_HpChange(-val, networkId);
            }
            else
            {
                actorManager.actorNetManager.RPC_AllClient_HpChange(0, networkId);
            }
        }
    }
    /// <summary>
    /// 魔法伤害
    /// </summary>
    public void TakeMagicDamage(int val, ActorNetManager from)
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            NetworkId networkId = new NetworkId();
            if (from) { networkId = from.Object.Id; }
            val -= actorManager.actorNetManager.Net_Resistance;
            if (val > 0)
            {
                actorManager.actorNetManager.RPC_AllClient_HpChange(-val, networkId);
            }
            else
            {
                actorManager.actorNetManager.RPC_AllClient_HpChange(0, networkId);
            }
        }
    }
    public bool PayCoin(int coin)
    {
        if (actorManager.actorNetManager.Local_Coin >= coin && actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_Local_PayCoin(coin);
            return true;
        }
        else
        {
            return false;
        }
    }
    public int EarnCoin(int coin)
    {
        if (actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_Local_EarnCoin(coin);
        }
        return actorManager.actorNetManager.Local_Coin;
    }
    public void SetFine(short val)
    {
        if (actorManager.actorNetManager.Local_Fine < val && actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_Local_ChangeFine(val);
        }
    }
    public void ClearFine()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_Local_ChangeFine(0);
        }
    }
    public void SendEmoji(int emojiID)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneSendEmoji
        {
            actor = actorManager,
            emoji = (Emoji)emojiID,
            distance = 10
        });
        actorManager.actorUI.SendEmoji((Emoji)emojiID);
    }
    public void SendText(string text,int id)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneSendEmoji
        {
            actor = actorManager,
            emoji = (Emoji)id,
            distance = 10
        });
        actorManager.actorUI.SendText(text);
    }
    #region//Play
    public void PlayMove(float speed)
    {
        bodyController.float_Speed = speed;
        bodyController.SetAnimatorFloat(BodyPart.Body, "Speed", speed);
        bodyController.SetAnimatorBool(BodyPart.Body, "Walk", true);
        bodyController.SetAnimatorFloat(BodyPart.Hand, "Speed", speed);
        bodyController.SetAnimatorBool(BodyPart.Hand, "Walk", true);
    }
    public void PlayStop()
    {
        bodyController.float_Speed = 0;
        bodyController.SetAnimatorFloat(BodyPart.Body, "Speed", 1);
        bodyController.SetAnimatorBool(BodyPart.Body, "Walk", false);
        bodyController.SetAnimatorFloat(BodyPart.Hand, "Speed", 1);
        bodyController.SetAnimatorBool(BodyPart.Hand, "Walk", false);
    }
    public void PlayDead(float speed, Action<string> action)
    {
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Dead");
        bodyController.SetAnimatorAction(BodyPart.Body, action);
    }
    public void PlayTakeDamage(float speed)
    {
        
    }
    public void PlayPickUp(float speed, Action<string> action)
    {
        bodyController.SetAnimatorTrigger(BodyPart.Hand, "Pick");
        bodyController.SetAnimatorTrigger(BodyPart.Head, "Pick");
        bodyController.SetAnimatorAction(BodyPart.Hand, action);
    }
    #endregion

}
