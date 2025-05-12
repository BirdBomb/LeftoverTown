using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class ActorManager_Player : ActorManager
{
    public void Start()
    {
        inputManager.Local_AddInputKeycodeAction(Local_Input);
    }
    public override void AllClient_Listen_RoleInView(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            brainManager.actorManagers_Nearby.Add(actor);
            if (actor.Local_Communication())
            {
                actor.actorUI.ShowSingal(true);
            }
        }
        base.AllClient_Listen_RoleInView(actor);
    }
    public override void AllClient_Listen_RoleOutView(ActorManager actor)
    {
        if (actorAuthority.isLocal)
        {
            brainManager.actorManagers_Nearby.Remove(actor);
            actor.actorUI.ShowSingal(false);
            if (actorManager_DealWith == actor) { Local_TryToOverDeal (); }
        }
        base.AllClient_Listen_RoleOutView(actor);
    }
    public void Local_Input(ActorManager actor,KeyCode keyCode)
    {
        if(keyCode == KeyCode.F)
        {
            Local_TryToStartDeal();
        }
    }
    #region//交易
    private ActorManager actorManager_DealWith = null;
    /// <summary>
    /// 尝试开始交易
    /// </summary>
    public void Local_TryToStartDeal()
    {
        for (int i = 0; i < brainManager.actorManagers_Nearby.Count; i++)
        {
            if (brainManager.actorManagers_Nearby[i].Local_Deal(out ActorManager dealActor, out List<ItemData> dealGoods, out Func<ItemData, int> dealOffer))
            {
                actorManager_DealWith = brainManager.actorManagers_Nearby[0];
                MessageBroker.Default.Publish(new UIEvent.UIEvent_StartDeal()
                {
                    dealActor = dealActor,
                    dealGoods = dealGoods,
                    dealOffer = dealOffer,
                });
                return;
            }
        }
    }
    /// <summary>
    /// 尝试结束交易
    /// </summary>
    public void Local_TryToOverDeal()
    {
        MessageBroker.Default.Publish(new UIEvent.UIEvent_OverDeal()
        {
            dealActor = actorManager_DealWith
        });
        actorManager_DealWith = null;
    }
    #endregion
}
