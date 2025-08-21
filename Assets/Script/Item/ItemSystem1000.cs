using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Windows;

public class ItemSystem1000 
{

}
/// <summary>
/// ����
/// </summary>
[Serializable]
public class Item_0 : ItemBase
{
    #region//����
    private ItemLocalObj_Punch itemLocalObj_Punch;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Punch = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_0").GetComponent<ItemLocalObj_Punch>();
        itemLocalObj_Punch.HoldingStart(owner, body);
    }
    #endregion
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        
    }

    #region//ʹ���߼�
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Punch.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Punch.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Punch.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    #endregion
}

#region//һ������
/// <summary>
/// ԭľ
/// </summary>
public class Item_1000 : ItemBase_Materials
{
}
/// <summary>
/// ��֦
/// </summary>
public class Item_1001 : ItemBase_Materials
{
}
/// <summary>
/// ��
/// </summary>
public class Item_1002 : ItemBase_Materials
{
}
/// <summary>
/// ʯͷ
/// </summary>
public class Item_1010 : ItemBase_Materials
{
}
/// <summary>
/// ú̿
/// </summary>
public class Item_1011 : ItemBase_Materials
{
}
/// <summary>
/// ���ʯ
/// </summary>
public class Item_1012 : ItemBase_Materials
{
}
/// <summary>
/// ����ʯ
/// </summary>
public class Item_1013 : ItemBase_Materials
{
}
/// <summary>
/// ���ܿ�
/// </summary>
public class Item_1014 : ItemBase_Materials
{
}
/// <summary>
/// ���ԭʯ
/// </summary>
public class Item_1015 : ItemBase_Materials
{
}
/// <summary>
/// ����
/// </summary>
public class Item_1016 : ItemBase_Materials
{
}
/// <summary>
/// ��ˮ��ԭʯ
/// </summary>
public class Item_1017 : ItemBase_Materials
{
}
/// <summary>
/// ����ԭʯ
/// </summary>
public class Item_1018 : ItemBase_Materials
{
}
#endregion
#region//��������
/// <summary>
/// ľ��
/// </summary>
public class Item_1100 : ItemBase_Materials
{
}
/// <summary>
/// ��ֽ
/// </summary>
public class Item_1101 : ItemBase_Materials
{
}
/// <summary>
/// ����
/// </summary>
public class Item_1110 : ItemBase_Materials
{
}
/// <summary>
/// ��
/// </summary>
public class Item_1111 : ItemBase_Materials
{
}
/// <summary>
/// ����ˮ��
/// </summary>
public class Item_1112 : ItemBase_Materials
{
}
/// <summary>
/// ���
/// </summary>
public class Item_1113 : ItemBase_Materials
{
}
/// <summary>
/// ��ʯ���
/// </summary>
public class Item_1114 : ItemBase_Materials
{
}
/// <summary>
/// ��ˮ��
/// </summary>
public class Item_1115 : ItemBase_Materials
{
}
/// <summary>
/// ����ʯ
/// </summary>
public class Item_1116 : ItemBase_Materials
{
}
#endregion
#region//��������
/// <summary>
/// ��еԪ��
/// </summary>
public class Item_1200 : ItemBase_Materials
{
}
/// <summary>
/// ����оƬ
/// </summary>
public class Item_1201 : ItemBase_Materials
{
}
/// <summary>
/// ǹеԪ��
/// </summary>
public class Item_1202: ItemBase_Materials
{
}
#endregion
