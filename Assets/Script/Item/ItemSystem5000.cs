using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem5000 
{
}
#region//Ã±×Ó
/// <summary>
/// ²ÝÃ±
/// </summary>
public class Item_5001 : ItemBase_Hat
{
    private short Armor = 1;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5001");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor(Armor);
        }
    }
    public override void OnHead_Over(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor((short)-Armor);
        }
        base.OnHead_Over(owner, body);
    }
}
#endregion
#region//ÒÂ·þ
/// <summary>
/// Ñ²ÂßÒÂ
/// </summary>
public class Item_5500 : ItemBase_Hat
{
    private short Armor = 1;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5100");
        itemLocalObj.transform.SetParent(body.transform_ItemOnBody, false);
        body.gameObjects_ItemOnBody.Add(itemLocalObj);
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor((short)Armor);
        }
    }
    public override void OnBody_Over(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor((short)-Armor);
        }
        base.OnHead_Over(owner, body);
    }
}
#endregion
#region//ÊÎÆ·
#endregion
