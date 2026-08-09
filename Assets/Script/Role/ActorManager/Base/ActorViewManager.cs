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
            actorManager.ForAll_Listen_ItemInView(collision.transform.parent.GetComponent<ItemNetObj>());
            return;
        }
        if (collision.gameObject.tag.Equals("Actor") && collision.TryGetComponent(out ActorManager actor))
        {
            actorManager.AllClient_Listen_RoleInView(actor);
            if (actorManager.actorAuthority.isState)
            {
                actorManager.State_Listen_RoleInView(actor);
                return;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Item"))
        {
            actorManager.ForAll_Listen_ItemOutView(collision.transform.parent.GetComponent<ItemNetObj>());
            return;
        }
        if (collision.gameObject.tag.Equals("Actor") && collision.TryGetComponent(out ActorManager actor))
        {
            actorManager.AllClient_Listen_RoleOutView(actor);
            if (actorManager.actorAuthority.isState)
            {
                actorManager.State_Listen_RoleOutView(actor);
                return;
            }
        }
    }
}
