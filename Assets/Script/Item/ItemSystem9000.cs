using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem9000 
{
}
/// <summary>
/// ÆõÔ¼
/// </summary>
public class Item_9001 : ItemBase
{
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_9001");
        base.Holding_Start(owner, body);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("PutUp", 0.2f, PutUp);
            }
            else
            {
                owner.BodyController.SetHandTrigger("PutUp", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void PutUp(string name)
    {
        if (name == "PutUp")
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySpawnActor()
            {
                name = "Actor/Zombie_Spray"
            });
        }
    }

}
/// <summary>
/// ´ÖÖÆÄ¾¼ý
/// </summary>
public class Item_9002 : ItemBase
{

}
/// <summary>
/// ¾«ÖÆÄ¾¼ý
/// </summary>
public class Item_9003 : ItemBase
{

}
/// <summary>
/// µ¯Íè
/// </summary>
public class Item_9004 : ItemBase
{

}
/// <summary>
/// Ô¿³×
/// </summary>
public class Item_9005 : ItemBase
{

}
