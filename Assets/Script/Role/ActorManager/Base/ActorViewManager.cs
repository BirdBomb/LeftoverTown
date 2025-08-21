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
        if (collision.gameObject.tag.Equals("Item"))
        {
            actorManager.AllClient_Listen_ItemInView(collision.transform.parent.GetComponent<ItemNetObj>());
            if (actorManager.actorAuthority.isState) actorManager.State_Listen_ItemInView(collision.transform.parent.GetComponent<ItemNetObj>());
        }
        else
        {
            actorManager.AllClient_Listen_RoleInView(collision.GetComponent<ActorManager>());
            if (actorManager.actorAuthority.isState) actorManager.State_Listen_RoleInView(collision.GetComponent<ActorManager>());
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Item"))
        {
            if (actorManager.actorAuthority.isState) actorManager.State_Listen_ItemOutView(collision.transform.parent.GetComponent<ItemNetObj>());
        }
        else
        {
            actorManager.AllClient_Listen_RoleOutView(collision.GetComponent<ActorManager>());
            if (actorManager.actorAuthority.isState) actorManager.State_Listen_RoleOutView(collision.GetComponent<ActorManager>());
        }
    }
}
