using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorViewManager : MonoBehaviour
{
    private ActorManager actorManager;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        actorManager.AllClient_Listen_RoleInView(collision.GetComponent<ActorManager>());
        if(actorManager.actorAuthority.isState) actorManager.State_Listen_RoleInView(collision.GetComponent<ActorManager>());
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        actorManager.AllClient_Listen_RoleOutView(collision.GetComponent<ActorManager>());
        if (actorManager.actorAuthority.isState) actorManager.State_Listen_RoleOutView(collision.GetComponent<ActorManager>());
    }
}
