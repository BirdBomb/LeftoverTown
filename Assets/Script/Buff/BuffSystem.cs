using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BuffSystem
{
}
/// <summary>
/// ˯��
/// </summary>
public class Buff100 : BuffBase 
{
    public override void Listen_AddOnActor(ActorManager actor)
    {
        Debug.Log("˯��");
        actor.BodyController.HideActor();
        base.Listen_AddOnActor(actor);
    }
    public override void Listen_SubFromActor(ActorManager actor)
    {
        Debug.Log("����");
        actor.BodyController.ShowActor();
        foreach(MyTile tile in actor.Tool_GetNearbyTiles(ActorManager.NearByMean.EightSide))
        {
            if (tile.bindObj)
            {
                if (tile.bindObj.TryGetComponent(out TileObj_Bed bed))
                {
                    bed.GetUpOnBed(actor);
                }
            }
            else
            {

            }
        };
        base.Listen_SubFromActor(actor);
    }
    public override void Listen_MyselfMove(ActorManager actor)
    {
        actor.NetManager.RPC_LocalInput_SubBuff(100);
        base.Listen_MyselfMove(actor);
    }
}
/// <summary>
/// ����
/// </summary>
public class Buff101 : BuffBase
{

}
/// <summary>
/// ��������
/// </summary>
public class Buff8000 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Strength -= 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Strength += 1;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// С����
/// </summary>
public class Buff8001 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Strength -= 2;
        data.Point_Agility += 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Strength += 2;
        data.Point_Agility -= 1;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// ��ɢ˼ά
/// </summary>
public class Buff8002 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Focus -= 2;
        data.Point_Intelligence += 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Focus += 2;
        data.Point_Intelligence -= 1;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// ���ֱ���
/// </summary>
public class Buff8003 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Agility -= 2;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Agility += 2;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// �ߴ�л
/// </summary>
public class Buff8004 : BuffBase
{
}
/// <summary>
/// ����֢
/// </summary>
public class Buff8005 : BuffBase
{
}
/// <summary>
/// ��ʳ
/// </summary>
public class Buff8006 : BuffBase
{
}
/// <summary>
/// ζ��ʧ��
/// </summary>
public class Buff8007 : BuffBase
{
}
/// <summary>
/// ��������
/// </summary>
public class Buff8008 : BuffBase
{
}
/// <summary>
/// ������
/// </summary>
public class Buff8009 : BuffBase
{
}
/// <summary>
/// �������
/// </summary>
public class Buff8010 : BuffBase
{
}
/// <summary>
/// ͨ����
/// </summary>
public class Buff8011 : BuffBase
{
}
/// <summary>
/// С����
/// </summary>
public class Buff8012 : BuffBase
{
}
/// <summary>
/// ��ծ����
/// </summary>
public class Buff8013 : BuffBase
{
}
/// <summary>
/// ������
/// </summary>
public class Buff8014 : BuffBase
{
}

/// <summary>
/// ��ǿ��׳
/// </summary>
public class Buff9000:BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Strength += 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Strength -= 1;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// ���ͷ
/// </summary>
public class Buff9001 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Strength += 2;
        data.Point_Agility -= 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Strength -= 2;
        data.Point_Agility += 1;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// ȫ���ע
/// </summary>
public class Buff9002 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Focus += 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Focus -= 1;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// �����
/// </summary>
public class Buff9003 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Intelligence += 2;
        data.Point_Focus -= 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Intelligence -= 2;
        data.Point_Focus += 1;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// ��������
/// </summary>
public class Buff9004 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Agility += 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
    public override void Listen_SubOnPlayerCreation(ref PlayerData data)
    {
        data.Point_Agility -= 1;
        base.Listen_SubOnPlayerCreation(ref data);
    }
}
/// <summary>
/// �ʹ�л
/// </summary>
public class Buff9005 : BuffBase
{

}
/// <summary>
/// ������
/// </summary>
public class Buff9006 : BuffBase
{

}
/// <summary>
/// Ƥ�����
/// </summary>
public class Buff9007 : BuffBase
{
    public override void Listen_AddOnPlayerCreation(ref PlayerData data)
    {
        data.Armor += 1;
        base.Listen_AddOnPlayerCreation(ref data);
    }
}
/// <summary>
/// ��ʳƷ����
/// </summary>
public class Buff9008 : BuffBase
{
}
/// <summary>
/// ����սʿ
/// </summary>
public class Buff9009 : BuffBase
{
}
/// <summary>
/// �質��
/// </summary>
public class Buff9010 : BuffBase
{
}
/// <summary>
/// ������
/// </summary>
public class Buff9011 : BuffBase
{
}
/// <summary>
/// �������
/// </summary>
public class Buff9012 : BuffBase
{
}
/// <summary>
/// �ΰ������
/// </summary>
public class Buff9013 : BuffBase
{
}
/// <summary>
/// �������
/// </summary>
public class Buff9014 : BuffBase
{
}
/// <summary>
/// ��ͽ
/// </summary>
public class Buff9015 : BuffBase
{
}
/// <summary>
/// �Ҿ���ʵ
/// </summary>
public class Buff9016 : BuffBase
{
}
/// <summary>
/// �������ʦ
/// </summary>
public class Buff9017 : BuffBase
{
}
/// <summary>
/// ̼��ˮ������
/// </summary>
public class Buff9018 : BuffBase
{
}
/// <summary>
/// �����
/// </summary>
public class Buff9019 : BuffBase
{

}


