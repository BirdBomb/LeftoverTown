using Fusion;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

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
                actorManager.actorNetManager.RPC_LocalInput_PickItemManual(obj.Object.Id);
                break;
            }
        }
    }
    /// <summary>
    /// 快速持握
    /// </summary>
    public void ItemHand_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldHandItem = actorManager.actorNetManager.Net_ItemHand;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldHandItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
        {
            oldItem = oldHandItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速穿戴
    /// </summary>
    /// <param name="index"></param>
    public void ItemHead_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldHeadItem = actorManager.actorNetManager.Net_ItemHead;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldHeadItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHead_Change()
        {
            oldItem = oldHeadItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速穿戴
    /// </summary>
    /// <param name="index"></param>
    public void ItemBody_Switch(int index) 
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldBodyItem = actorManager.actorNetManager.Net_ItemBody;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldBodyItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBody_Change()
        {
            oldItem = oldBodyItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速替换饰品
    /// </summary>
    public void ItemAccessory_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldConsumablesItem = actorManager.actorNetManager.Net_ItemConsumables;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldConsumablesItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemAccessory_Change()
        {
            oldItem = oldConsumablesItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速替换耗材
    /// </summary>
    public void ItemConsumables_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldConsumablesItem = actorManager.actorNetManager.Net_ItemConsumables;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldConsumablesItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_Change()
        {
            oldItem = oldConsumablesItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速丢弃
    /// </summary>
    /// <param name="index"></param>
    public void Drop(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData dropItem = items[index];
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
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
        if (bodyController.faceDir.x > 0.1f) bodyController.FaceRight();
        if (bodyController.faceDir.x < -0.1f) bodyController.FaceLeft();
    }
    public void TurnTo(Vector2 dir)
    {
        bodyController.turnDir = dir.normalized;
        if (bodyController.turnDir.x > 0.1f) bodyController.TurnRight();
        if (bodyController.turnDir.x < -0.1f) bodyController.TurnLeft();
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
                DeadPlayer();
            }
            else
            {
                DeadNPC();
            }
        }
    }
    private void DeadPlayer()
    {
        if (actorManager.actorAuthority.isState)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_KillPlayer()
            {
                playerRef = actorManager.actorPlayerRef
            });
        }
    }
    private void DeadNPC()
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
        List<ItemData> dropItem = actorManager.actorNetManager.Local_GetLootItems();
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
                itemOwner = new NetworkId(),
                pos = position + actorManager.transform.position,
            });
        }
    }
    public void Client_HealHP(int val)
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            actorManager.actorNetManager.RPC_AllClient_HpChange(val, (int)HpChangeReason.Healing, new NetworkId());
        }
    }
    public void Client_IncreaseHP(int val)  
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            actorManager.actorNetManager.RPC_AllClient_MaxHpChange((short)val, new NetworkId());
        }
    }
    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="val"></param>
    /// <param name="damageState"></param>
    /// <param name="from"></param>
    public void TakeDamage(int val, DamageState damageState, ActorNetManager from)
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            NetworkId networkId = new NetworkId();
            if (from) { networkId = from.Object.Id; }
            if (damageState == DamageState.AttackPiercingDamage)
            {
                val -= actorManager.actorNetManager.Net_Armor;
                if (val > 0)
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                else
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(0, (int)HpChangeReason.AttackDamage, networkId);
                }
            }
            else if (damageState == DamageState.AttackSlashingDamage)
            {
                val -= actorManager.actorNetManager.Net_Armor;
                if (val > 0)
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                else
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(0, (int)HpChangeReason.AttackDamage, networkId);
                }
            }
            else if (damageState == DamageState.AttackBludgeoningDamage)
            {
                val -= actorManager.actorNetManager.Net_Armor;
                if (val > 0)
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                else
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(0, (int)HpChangeReason.AttackDamage, networkId);
                }
            }
            else if (damageState == DamageState.MagicDamage)
            {
                val -= actorManager.actorNetManager.Net_Resistance;
                if (val > 0)
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.MagicDamage, networkId);
                }
                else
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(0, (int)HpChangeReason.MagicDamage, networkId);
                }
            }
            bodyController.Flash();
            bodyController.Shake();
        }
    }
    /// <summary>
    /// 受力
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="force"></param>
    public void TakeForce(Vector2 dir, short force)
    {
        actorManager.actorNetManager.RPC_AllClient_AddForce(dir, force);
    }
    public bool PayCoin(int coin)
    {
        if (actorManager.actorNetManager.Local_Coin >= coin)
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
        actorManager.actorNetManager.RPC_Local_EarnCoin(coin);
        return actorManager.actorNetManager.Local_Coin;
    }
    public void Commit(short fine)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneCommit
        {
            actor = actorManager,
            fine = fine
        });
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
    public void PlayDead(float speed, Func<string,bool> func)
    {
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Dead");
        bodyController.SetAnimatorFunc(BodyPart.Body, func);
    }
    public void PlayTakeDamage(float speed)
    {
        
    }
    public void PlayPickUp(float speed, Func<string,bool> func)
    {
        bodyController.SetAnimatorTrigger(BodyPart.Hand, "Pick");
        bodyController.SetAnimatorTrigger(BodyPart.Head, "Pick");
        bodyController.SetAnimatorFunc(BodyPart.Hand, func);
    }
    #endregion

}
