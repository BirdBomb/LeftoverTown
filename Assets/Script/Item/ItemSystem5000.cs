using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem5000 
{
}
/// <summary>
/// �����Ա��
/// </summary>
public class Item_5001 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnBody(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5001");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnBody);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }
}
/// <summary>
/// �����Աñ
/// </summary>
public class Item_5002 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnHead(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5002");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnHead);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }

}
/// <summary>
/// ���·�(��ɫ)
/// </summary>
public class Item_5003 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnBody(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5003");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnBody);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }
}
/// <summary>
/// ��ñ
/// </summary>
public class Item_5004 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnHead(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5004");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnHead);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }

}
/// <summary>
/// ץ�ۿ�
/// </summary>
public class Item_5005 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnHead(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5005");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnHead);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }
}
/// <summary>
/// ��ʽ������
/// </summary>
public class Item_5006 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnBody(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5006");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnBody);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }

}
/// <summary>
/// �󹤿�
/// </summary>
public class Item_5007 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnHead(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5007");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnHead);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }

}
/// <summary>
/// ���·�(��ɫ)
/// </summary>
public class Item_5008 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnBody(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5008");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnBody);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }

}
/// <summary>
/// ����
/// </summary>
public class Item_5009 : ItemBase_Clothes
{
    private GameObject itemLocalObj;
    public override void BeWearingOnHead(ActorManager owner, BaseBodyController body)
    {
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5009");
        itemLocalObj.transform.SetParent(body.Tran_ItemOnHead);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
    }
}
