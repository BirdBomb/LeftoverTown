using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
public class ActorActionManager 
{
    private ActorManager actorManager;
    private BodyController_Base bodyController;
    private LayerMask layerMask_ItemObj;
    private LayerMask layerMask_Wall;
    private Vector2 vector2_Last;
    private Vector2 vector2_Cur;
    public Action<int, ActorNetManager> action_TakeDamage;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
        bodyController = actorManager.bodyController;
        vector2_Last = actorManager.transform.position;
        vector2_Cur = actorManager.transform.position;
        layerMask_ItemObj = LayerMask.GetMask("ItemObj");
        layerMask_Wall = LayerMask.GetMask("TileObj_Wall");
    }
    public void Listen_UpdateCustom(float dt)
    {
        vector2_Cur = actorManager.transform.position;
        float distance = Vector2.Distance(vector2_Last, vector2_Cur);
        float speed = distance * 10;
        if (speed > 0.1f)
        {
            PlayMove(speed);
            TurnTo(vector2_Cur - vector2_Last);
        }
        else
        {
            PlayStop();
        }
        vector2_Last = vector2_Cur;
    }
    public void AddForce(Vector3 dir, float force)
    {
        actorManager.actorNetManager.networkRigidbody.Rigidbody.velocity = dir * force;
    }
    public void PickUp()
    {
        var items = Physics2D.OverlapCircleAll(actorManager.transform.position, 0.5f, layerMask_ItemObj);
        foreach (Collider2D item in items)
        {
            if (item.gameObject.transform.parent.TryGetComponent(out ItemNetObj obj))
            {
                actorManager.actorNetManager.RPC_LocalInput_PickItem(obj.Object.Id);
                break;
            }
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
    public void DropDown()
    {
        for (int i = 0; i < actorManager.actorNetManager.Net_ItemsInBag.Count; i++)
        {
            ItemData item = actorManager.actorNetManager.Net_ItemsInBag[i];
            if (item.Item_ID != 0)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
                {
                    itemData = item,
                    pos = actorManager.transform.position - new Vector3(0, 0.1f, 0)
                });
            }
        }
        ItemData itemOnHead = actorManager.actorNetManager.Net_ItemOnHead;
        if (itemOnHead.Item_ID != 0)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = itemOnHead,
                pos = actorManager.transform.position - new Vector3(0, 0.1f, 0)
            });
        }
        ItemData itemOnBody = actorManager.actorNetManager.Net_ItemOnBody;
        if (itemOnBody.Item_ID != 0)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = itemOnBody,
                pos = actorManager.transform.position - new Vector3(0, 0.1f, 0)
            });
        }
        ItemData itemOnHand = actorManager.actorNetManager.Net_ItemInHand;
        if (itemOnHand.Item_ID != 0)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = itemOnHand,
                pos = actorManager.transform.position - new Vector3(0, 0.1f, 0)
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
    public void TakeDamage(int val, ActorNetManager from)
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
    public bool PayCoin(int coin)
    {
        if (actorManager.actorNetManager.Net_Coin >= coin && actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_LocalInput_PayCoin(coin);
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
            actorManager.actorNetManager.RPC_LocalInput_EarnCoin(coin);
        }
        return actorManager.actorNetManager.Net_Coin;
    }
    public void SetFine(short val)
    {
        if (actorManager.actorNetManager.Net_Fine < val && actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_LocalInput_ChangeFine(val);
        }
    }
    public void ClearFine()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_LocalInput_ChangeFine(0);
        }
    }
    public bool EnSub(int offset)
    {
        if (actorManager.actorNetManager.Net_EnCur > 50)
        {
            actorManager.actorNetManager.Net_EnCur -= offset;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EnAdd(int offset)
    {
        if (actorManager.actorNetManager.Net_EnCur < actorManager.actorNetManager.Net_EnMax)
        {
            actorManager.actorNetManager.Net_EnCur += offset;
            return false;
        }
        else
        {
            return true;
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
        bodyController.speed = speed;
        bodyController.SetAnimatorFloat(BodyPart.Body, "Speed", speed);
        bodyController.SetAnimatorBool(BodyPart.Body, "Walk", true);
        bodyController.SetAnimatorFloat(BodyPart.Hand, "Speed", speed);
        bodyController.SetAnimatorBool(BodyPart.Hand, "Walk", true);
    }
    public void PlayStop()
    {
        bodyController.speed = 0;
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
