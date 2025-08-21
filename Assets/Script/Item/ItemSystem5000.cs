using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem5000 
{
}
#region//ñ��
/// <summary>
/// Ѳ��ñ
/// </summary>
public class Item_5000 : ItemBase_Hat
{
    private short Armor = 1;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5000");
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
/// <summary>
/// ��ñ
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
/// <summary>
/// ������
/// </summary>
public class Item_5002 : ItemBase_Hat
{
    private short Armor = 4;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5002");
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
/// <summary>
/// ��ñ
/// </summary>
public class Item_5003 : ItemBase_Hat
{
    private short Armor = 1;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5003");
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
/// <summary>
/// ����
/// </summary>
public class Item_5004 : ItemBase_Hat
{
    private short Resistance = 1;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5004");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeResistance(Resistance);
        }
    }
    public override void OnHead_Over(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeResistance((short)-Resistance);
        }
        base.OnHead_Over(owner, body);
    }
}
/// <summary>
/// ����
/// </summary>
public class Item_5005 : ItemBase_Hat
{
    private short Armor = 4;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5005");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor((short)Armor);
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
/// <summary>
/// ��Ƥñ
/// </summary>
public class Item_5006 : ItemBase_Hat
{
    private short Armor = 2;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5006");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor((short)Armor);
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
/// <summary>
/// ľ��
/// </summary>
public class Item_5007 : ItemBase_Hat
{
    private short Armor = 2;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5007");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor((short)Armor);
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
/// <summary>
/// ��ʦñ
/// </summary>
public class Item_5008 : ItemBase_Hat
{
    private short Armor = 2;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5008");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor((short)Armor);
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
/// <summary>
/// ����ñ��
/// </summary>
public class Item_5009 : ItemBase_Hat
{
    private short Armor = 2;
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5009");
        itemLocalObj.transform.SetParent(body.transform_ItemOnHead, false);
        body.gameObjects_ItemOnHead.Add(itemLocalObj);
        if (owner.actorAuthority.isLocal)
        {
            owner.actorNetManager.RPC_LocalInput_ChangeArmor((short)Armor);
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
#region//�·�
/// <summary>
/// Ѳ����
/// </summary>
public class Item_5100 : ItemBase_Clothes
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
/// <summary>
/// �����·�(��)
/// </summary>
public class Item_5101 : ItemBase_Clothes
{
    private short Armor = 1;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5101");
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
/// <summary>
/// �����·�(��)
/// </summary>
public class Item_5102 : ItemBase_Clothes
{
    private short Armor = 1;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5102");
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
/// <summary>
/// ������
/// </summary>
public class Item_5103 : ItemBase_Clothes
{
    private short Armor = 5;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5103");
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
/// <summary>
/// ����
/// </summary>
public class Item_5104 : ItemBase_Clothes
{
    private short Armor = 4;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5104");
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
/// <summary>
/// ľ��
/// </summary>
public class Item_5105 : ItemBase_Clothes
{
    private short Armor = 2;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5105");
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
/// <summary>
/// ������
/// </summary>
public class Item_5106 : ItemBase_Clothes
{
    private short Armor = 2;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5106");
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
/// <summary>
/// �ײ���
/// </summary>
public class Item_5107 : ItemBase_Clothes
{
    private short Armor = 2;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5107");
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
/// <summary>
/// �ײ���
/// </summary>
public class Item_5108 : ItemBase_Clothes
{
    private short Armor = 2;
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        GameObject itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_5108");
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
