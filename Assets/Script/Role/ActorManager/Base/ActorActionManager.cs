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
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
        bodyController = actorManager.bodyController;
        layerMask_ItemObj = LayerMask.GetMask("ItemObj");
        layerMask_Wall = LayerMask.GetMask("TileObj_Wall");
    }
    #region//物体相关
    /// <summary>
    /// 捡起
    /// </summary>
    /// <param name="radiu"></param>
    public void State_PickUp(float radiu)
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
        });
        if (dropItem.I != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = dropItem
            });
        }
    }

    #endregion
    #region//身体相关
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
    #endregion
    #region//死亡相关
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
            Debug.Log(dropItem[i].I + "/" + dropItem[i].C);
            Vector3 position = new Vector3(x, y, 0);
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = dropItem[i],
                itemOwner = new NetworkId(),
                pos = position + actorManager.transform.position,
            });
        }
    }
    #endregion
    #region//速度相关
    /// <summary>
    /// 地块速度影响参数
    /// </summary>
    private float client_SpeedOffset_Floor;
    public float Client_GetSpeed()
    {
        float temp = actorManager.actorNetManager.Net_SpeedCommon * 0.1f;
        if (actorManager.actorAuthority.isPlayer)
        {
            float sanRatio = actorManager.sanManager.GetSanRatio();
            float sanOffset = (sanRatio < 0.3f) ? Mathf.Lerp(0.5f, 1.0f, sanRatio / 0.3f) : 1f;
            temp = temp * sanOffset;
        }
        return temp;
    }

    #endregion
    #region//其他相关
    /// <summary>
    /// 受力
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="force"></param>
    public void Client_TakeForce(Vector2 dir, short force)
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
    /// <summary>
    /// 犯罪
    /// </summary>
    /// <param name="fine"></param>
    public void AllClient_Commit(CommitState commit, short fine)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneCommit
        {
            actor = actorManager,
            commit = commit,
            fine = fine
        });
    }
    /// <summary>
    /// 悬赏
    /// </summary>
    /// <param name="val"></param>
    public void AllClient_SetFine(CommitState commit, short val)
    {
        switch (commit)
        {
            case CommitState.Steal:
                break;
            case CommitState.Attacking:
                break;
            case CommitState.Murder:
                break;
        }
        if (actorManager.actorNetManager.Local_Fine < val)
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
    public void AllClient_SendEmoji(short emojiID,short distance)
    {
       actorManager.actorNetManager.RPC_LocalInput_SendEmoji(emojiID, distance);
    }
    public void AllClient_SendText(string text,int id)
    {
       actorManager.actorNetManager.RPC_LocalInput_SendText(text,id);
    }
    #endregion
    #region//Play
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
