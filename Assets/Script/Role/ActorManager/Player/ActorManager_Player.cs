using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorManager_Player : ActorManager
{
    public void Start()
    {
        inputManager.Local_AddInputKeycodeAction(Local_Listen_Input);
    }
    public override void FixedUpdate()
    {
        if (actorAuthority.isLocal)
        {
            Local_UpdateClosestActor();
            Local_UpdateClosestItem();
        }
        base.FixedUpdate();
    }
    #region//监听
    public override void ForAll_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(ForAll_Listen_UpdateTime).AddTo(this);
        base.ForAll_AddListener();
    }
    public override void ForAll_SecondUpdate()
    {
        if (actorAuthority.isLocal)
        {
            bool outOfRange = Vector2.Distance(transform.position, WorldLightManager.Instance.light2D_Sun.transform.position) > WorldManager.Instance.gameNetManager.SunLight;
            WorldLightManager.Instance.ChangeVignette(outOfRange);
            if (outOfRange)
            {
                buffManager.Local_AddBuff(new BuffData(2000));
                MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowTips() {  });
            }
            else 
            { 
                buffManager.Local_RemoveBuff(2000);
                MessageBroker.Default.Publish(new UIEvent.UIEvent_HideTips() { });
            }
        }
        base.ForAll_SecondUpdate();
    }
    #endregion
    #region//附近人物
    /// <summary>
    /// 最近的可以对话的角色
    /// </summary>
    private ActorManager actorManager_Closest = null;
    public void Local_UpdateClosestActor()
    {
        if (brainManager.actorManagers_Nearby.Count > 0)
        {
            if (brainManager.actorManagers_Nearby.Count == 1)
            {
                if (actorManager_Closest != brainManager.actorManagers_Nearby[0])
                {
                    if (actorManager_Closest != null)
                    { 
                        actorManager_Closest.Local_PlayerFaraway(this);
                        actorManager_Closest = null;
                    }
                    actorManager_Closest = brainManager.actorManagers_Nearby[0];
                    actorManager_Closest.Local_PlayerClose(this);
                }
            }
            else
            {
                float distance_Temp = float.MaxValue;
                ActorManager actorManager_Temp = null;
                for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
                {
                    float temp = Vector3.Distance(transform.position, brainManager.actorManagers_Nearby[i].transform.position);
                    if (temp < distance_Temp)
                    {
                        distance_Temp = temp;
                        actorManager_Temp = brainManager.actorManagers_Nearby[i];
                    }
                }

                if (actorManager_Closest != actorManager_Temp)
                {
                    if (actorManager_Closest != null)
                    {
                        actorManager_Closest.Local_PlayerFaraway(this);
                        actorManager_Closest = null;
                    }
                    actorManager_Closest = actorManager_Temp;
                    actorManager_Closest.Local_PlayerClose(this);
                }
            }
        }
        else
        {
            if (actorManager_Closest != null)
            {
                actorManager_Closest.Local_PlayerFaraway(this);
                actorManager_Closest = null;
            }
        }
    }
    public override void AllClient_Listen_RoleInView(ActorManager actor)
    {
        if(actorAuthority.isPlayer && actorAuthority.isLocal && actor.Local_IsInteractable())
        {
            brainManager.State_AddNearbyActors(actor);
        }
        base.AllClient_Listen_RoleInView(actor);
    }
    public override void AllClient_Listen_RoleOutView(ActorManager actor)
    {
        if (actorAuthority.isPlayer && actorAuthority.isLocal)
        {
            brainManager.State_RemoveNearbyActors(actor);
        }
        base.AllClient_Listen_RoleOutView(actor);
    }
    public void Local_Listen_Input(ActorManager actor, KeyCode keyCode)
    {
        if (actorManager_Closest != null && brainManager.actorManagers_Nearby.Contains(actorManager_Closest))
        {
            actorManager_Closest.Local_GetPlayerInput(this, keyCode);
        }
    }

    #endregion
    #region//附近物体
    public override void ForAll_Listen_ItemInView(ItemNetObj obj)
    {
        if (actorAuthority.isLocal)
        {
            brainManager.allClient_ItemNetObj_Nearby.Add(obj);
        }
        base.ForAll_Listen_ItemInView(obj);
    }
    public override void ForAll_Listen_ItemOutView(ItemNetObj obj)
    {
        if (actorAuthority.isLocal)
        {
            brainManager.allClient_ItemNetObj_Nearby.Remove(obj);
        }
        base.ForAll_Listen_ItemOutView(obj);
    }
    public override void ForState_Listen_ItemOutView(ItemNetObj obj)
    {
        if (obj && obj.Object && actorNetManager && actorNetManager.Object && obj.Client_Owner == actorNetManager.Object.Id)
        {
            obj.State_RsetOwner();
        }
        base.ForState_Listen_ItemOutView(obj);
    }
    public void Local_UpdateClosestItem()
    {
        for (int i = 0; i < brainManager.allClient_ItemNetObj_Nearby.Count; i++)
        {
            if (brainManager.allClient_ItemNetObj_Nearby[i].Client_Owner != actorNetManager.Object.Id)
            {
                if (actorNetManager.Local_BagItemCount < actorNetManager.Local_BagCapacity)
                {
                    ItemNetObj itemNetObj = brainManager.allClient_ItemNetObj_Nearby[i];
                    brainManager.allClient_ItemNetObj_Nearby.RemoveAt(i);
                    actorNetManager.RPC_LocalInput_PickItemAuto(itemNetObj.Object.Id);
                    break;
                }
            }
        }
    }

    #endregion
}
