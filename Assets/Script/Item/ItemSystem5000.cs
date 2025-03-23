using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem5000 
{
}
#region//Ã±×Ó
/// <summary>
/// Ñ²ÂßÃ±
/// </summary>
public class Item_5000 : ItemBase_Clothes
{
    public override void BeWearingOnHead(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5000");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
    }
}
/// <summary>
/// ²ÝÃ±
/// </summary>
public class Item_5001 : ItemBase_Clothes
{
    public override void BeWearingOnHead(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5001");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
    }
}
/// <summary>
/// ·À»¤¿ø
/// </summary>
public class Item_5002 : ItemBase_Clothes
{
    public override void BeWearingOnHead(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5002");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
    }
}
/// <summary>
/// ¿ó¹¤Ã±
/// </summary>
public class Item_5003 : ItemBase_Clothes
{
    public override void BeWearingOnHead(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5003");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
    }
}
/// <summary>
/// ÃæÕÖ
/// </summary>
public class Item_5004 : ItemBase_Clothes
{
    public override void BeWearingOnHead(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5004");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
    }
}
/// <summary>
/// Ìú¿ø
/// </summary>
public class Item_5005 : ItemBase_Clothes
{
    public override void BeWearingOnHead(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5005");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
    }
}
/// <summary>
/// ÐÜÆ¤Ã±
/// </summary>
public class Item_5006 : ItemBase_Clothes
{
    public override void BeWearingOnHead(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5006");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
    }
}
#endregion
#region//ÒÂ·þ
/// <summary>
/// Ñ²ÂßÒÂ
/// </summary>
public class Item_5100 : ItemBase_Clothes
{
    public override void BeWearingOnBody(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5100");
        itemLocalObj.transform.SetParent(body.transform_ItemOnBody, false);
        body.gameObjects_ItemOnBody.Add(itemLocalObj);
    }
}
/// <summary>
/// ²¼ÖÆÒÂ·þ(»Ò)
/// </summary>
public class Item_5101 : ItemBase_Clothes
{
    public override void BeWearingOnBody(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5101");
        itemLocalObj.transform.SetParent(body.transform_ItemOnBody, false);
        body.gameObjects_ItemOnBody.Add(itemLocalObj);
    }

}
/// <summary>
/// ²¼ÖÆÒÂ·þ(ºÖ)
/// </summary>
public class Item_5102 : ItemBase_Clothes
{
    public override void BeWearingOnBody(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5102");
        itemLocalObj.transform.SetParent(body.transform_ItemOnBody, false);
        body.gameObjects_ItemOnBody.Add(itemLocalObj);
    }

}
/// <summary>
/// ·À»¤ÒÂ
/// </summary>
public class Item_5103 : ItemBase_Clothes
{
    public override void BeWearingOnBody(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5103");
        itemLocalObj.transform.SetParent(body.transform_ItemOnBody, false);
        body.gameObjects_ItemOnBody.Add(itemLocalObj);
    }

}
/// <summary>
/// Ìú¼×
/// </summary>
public class Item_5104 : ItemBase_Clothes
{
    public override void BeWearingOnBody(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5104");
        itemLocalObj.transform.SetParent(body.transform_ItemOnBody, false);
        body.gameObjects_ItemOnBody.Add(itemLocalObj);
    }

}

#endregion
