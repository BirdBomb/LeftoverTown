using DG.Tweening;
using Fusion;
using System;
using UniRx;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using WebSocketSharp;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// ����
/// </summary>
public class ItemBase
{
    public ActorManager owner;

    #region//��ʼ��
    public void ItemData()
    {

    }
    #endregion
    #region//�������
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public ItemData itemData;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public ItemConfig itemConfig;
    /// <summary>
    /// ��������(�������)
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateDataFromNet(ItemData data)
    {
        itemData = data;
        itemConfig = ItemConfigData.GetItemConfig(data.Item_ID);
    }
    /// <summary>
    /// ��������(����ģ��)
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateDataFromLocal(ItemData data)
    {
        itemData = data;
    }
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual void Holding_UpdateTime(int second)
    {

    }

    #endregion
    #region//UI���
    /// <summary>
    /// ���Ƹ���
    /// </summary>
    public virtual void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Count.ToString());
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {

    }
    /// <summary>
    /// �һ�����
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void RightClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {

    }
    #endregion
    #region//���ղ���
    #region//�������
    /// <summary>
    /// ��������
    /// </summary>
    public InputData inputData = new InputData();
    /// <summary>
    /// �����ѹ
    /// </summary>
    /// <param name="pressTimer"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <returns>���ֵ</returns>
    public virtual bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return true;
    }
    /// <summary>
    /// ����ͷ�
    /// </summary>
    /// <param name="state"></param>
    /// <param name="input"></param>
    public virtual void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
    }
    /// <summary>
    /// �Ҽ���ѹ
    /// </summary>
    /// <param name="pressTimer"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <returns>���ֵ</returns>
    public virtual bool Holding_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return true;
    }
    /// <summary>
    /// �Ҽ��ͷ�
    /// </summary>
    /// <param name="state"></param>
    /// <param name="input"></param>
    public virtual void Holding_ReleaseRightPress(bool state, bool input, bool player)
    {

    }
    /// <summary>
    /// �������λ��
    /// </summary>
    public virtual void Holding_UpdateMousePos(Vector3 mouse)
    {

    }
    #endregion
    #region//���շ���
    /// <summary>
    /// ��ʼ����
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="item"></param>
    public virtual void Holding_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.transform_ItemInRightHand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="who"></param>
    public virtual void Holding_Over(ActorManager who)
    {

    }
    /// <summary>
    /// �������
    /// </summary>
    public virtual void Holding_UpdateLook()
    {

    }
    /// <summary>
    /// �����;ö�
    /// </summary>
    /// <param name="val"></param>
    public virtual void Holding_ChangeDurability(sbyte val)
    {
        if (val != 0 && owner.actorAuthority.isPlayer) 
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            if (_newItem.Item_Durability + val <= 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHand()
                {
                    item = itemData,
                });
            }
            else
            {
                _newItem.Item_Durability += val;
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                {
                    oldItem = _oldItem,
                    newItem = _newItem,
                });
            }
        }
    }
    #endregion
    #endregion
    #region//��������
    /// <summary>
    /// ����(ͷ��)
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void BeWearingOnHead(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.transform_ItemOnHead.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
    }
    /// <summary>
    /// ����(����)
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void BeWearingOnBody(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.transform_ItemOnBody.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
    }

    #endregion
    #region//���÷���
    /// <summary>
    /// ���ݳ�ʼ��
    /// </summary>
    /// <returns></returns>
    public virtual void StaticAction_InitData(short id,out ItemData data)
    {
        data = new ItemData(id);
    }
    /// <summary>
    /// �ѵ�
    /// </summary>
    /// <param name="baseItem"></param>
    /// <param name="addItem"></param>
    /// <param name="newItem"></param>
    public virtual void StaticAction_PileUp(ItemData baseItem, ItemData addItem, short maxCap, out ItemData newItem,out ItemData resItem)
    {
        newItem = baseItem;
        resItem = baseItem;
        if(baseItem.Item_Count + addItem.Item_Count <= maxCap)
        {
            newItem.Item_Count += addItem.Item_Count;
            resItem.Item_Count = 0;
        }
        else
        {
            newItem.Item_Count = maxCap;
            resItem.Item_Count = ((short)(baseItem.Item_Count + addItem.Item_Count - maxCap));
        }
    }
    /// <summary>
    /// ���
    /// </summary>
    /// <param name="oldContent">������</param>
    /// <param name="addItem">�����</param>
    /// <param name="maxCap">�����������</param>
    /// <param name="newContent">������</param>
    /// <param name="resItem">ʣ����</param>
    public virtual void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
    }
    /// <summary>
    /// ���÷���(����NetObj)
    /// </summary>
    public virtual void StaticAction_DrawItemObj(ItemNetObj obj,ItemData data)
    {
        obj.spriteIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
        obj.textMeshPro.text = data.Item_Count.ToString();
    }
    /// <summary>
    /// ���÷���(���ŵ��䶯��)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void StaticAction_PlayDropAnim(ItemNetObj obj)
    {
        obj.spriteIcon.transform.DOKill();
        obj.spriteIcon.transform.localScale = Vector3.zero;
        UnityEngine.Random.InitState(itemData.Item_Seed);
        Vector3 point = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(0.2f, 0.5f);
        obj.spriteIcon.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        obj.spriteIcon.transform.DOLocalJump(point, 1, 1, 0.5f).OnComplete(() =>
        {
            obj.spriteIcon.transform.DOPunchScale(new Vector3(0.2f, -0.2f, 0), 0.1f).SetEase(Ease.OutBack);
        }).SetEase(Ease.InOutQuad);
    }
    #endregion

}
/// <summary>
/// ��������
/// </summary>
public class ItemBase_Materials : ItemBase
{
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Count.ToString());
    }
}
/// <summary>
/// ����ʳ��
/// </summary>
public class ItemBase_Food : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.Item_Info = 100;
        initData.Item_Durability = 100;
        initData.Item_DurabilityPoint = (short)(MapManager.Instance.mapNetManager.Date * 10 + MapManager.Instance.mapNetManager.Hour);
    }
    public override void UpdateDataFromNet(ItemData itemData)
    {
        base.UpdateDataFromNet(itemData);
        CalculateDurability();
    }
    public override void StaticAction_PileUp(ItemData mainItem, ItemData addItem, short maxCap, out ItemData newItem, out ItemData resItem)
    {
        newItem = mainItem;
        resItem = mainItem;

        int wa_Dp = mainItem.Item_DurabilityPoint * mainItem.Item_Count + addItem.Item_DurabilityPoint * addItem.Item_Count;
        int wa_Dv = mainItem.Item_Durability * mainItem.Item_Count + addItem.Item_Durability * addItem.Item_Count;

        newItem.Item_DurabilityPoint = (short)((float)wa_Dp / (float)(addItem.Item_Count + mainItem.Item_Count));
        newItem.Item_Durability = (sbyte)((float)wa_Dv / (float)(addItem.Item_Count + mainItem.Item_Count));
        resItem.Item_DurabilityPoint = (short)((float)wa_Dp / (float)(addItem.Item_Count + mainItem.Item_Count));
        resItem.Item_Durability = (sbyte)((float)wa_Dv / (float)(addItem.Item_Count + mainItem.Item_Count));

        if (mainItem.Item_Count + addItem.Item_Count <= maxCap)
        {
            newItem.Item_Count += addItem.Item_Count;
            resItem.Item_Count = 0;
        }
        else
        {
            newItem.Item_Count = maxCap;
            resItem.Item_Count = ((short)(mainItem.Item_Count + addItem.Item_Count - maxCap));
        }
    }
    /// <summary>
    /// �������ʶ�
    /// </summary>
    /// <param name="nowTime"></param>
    public virtual void CalculateDurability()
    {
        int nowTime = MapManager.Instance.mapNetManager.Date * 10 + MapManager.Instance.mapNetManager.Hour;
        int offset = (int)((nowTime - itemData.Item_DurabilityPoint) * -5 * itemData.Item_Info / 100f);
        if (itemData.Item_Durability + offset >= 0)
        {
            itemData.Item_Durability += (sbyte)offset;
            itemData.Item_DurabilityPoint = (short)nowTime;
        }
        else
        {
            itemData.Item_Durability = 0;
            itemData.Item_DurabilityPoint = (short)nowTime;
            itemData.Item_ID = 3999;
        }
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Count.ToString());
        gridCell.SetSliderVal(itemData.Item_Durability / 100f);
        if (itemData.Item_Info == 0)
        {
            gridCell.FreezeCell(true);
        }
        else
        {
            gridCell.FreezeCell(false);
            gridCell.SetSliderColor(new Color(Mathf.Lerp(1, 0, itemData.Item_Durability / 100f), Mathf.Lerp(0f, 1, itemData.Item_Durability / 100f), 0, 1));
        }
    }

    public override bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                if (input)
                {
                    owner.bodyController.SetAnimatorAction(BodyPart.Head, (string str) =>
                    {
                        if (str.Equals("Eat"))
                        {
                            Eat();
                        }
                    });
                }
            }
        }
        inputData.leftPressTimer = pressTimer;
        return base.Holding_UpdateLeftPress(pressTimer, state, input, player);
    }
    public override void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.Holding_ReleaseLeftPress(state, input, player);
    }
    public virtual void Eat()
    {
        Expend(1);
        owner.hungryManager.AddFood(5);
    }
    public virtual void Expend(int val)
    {
        if (itemData.Item_Count > val)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            _newItem.Item_Count -= 1;
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });

        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHand()
            {
                item = itemData,
            });
        }
    }

}
/// <summary>
/// ��������
/// </summary>
public class ItemBase_Weapon : ItemBase
{
    public override void StaticAction_InitData(short id,out ItemData initData)
    {
        initData = new ItemData(id);
        initData.Item_Seed = new System.Random().Next(0, 100000);
        initData.Item_Durability = (sbyte)new System.Random().Next(50, 101);
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(),"ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Durability.ToString() + "%");
    }
}
/// <summary>
/// ��������
/// </summary>
public class ItemBase_Tool : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.Item_Seed = new System.Random().Next(0, 100000);
        initData.Item_Durability = (sbyte)new System.Random().Next(50, 101);
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Durability.ToString() + "%");
    }
}
/// <summary>
/// ����ǹ
/// </summary>
public class ItemBase_Gun : ItemBase
{
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Content.Item_Count.ToString());
    }
}
/// <summary>
/// ������װ
/// </summary>
public class ItemBase_Clothes:ItemBase
{

}
[Serializable]
public struct ItemData : INetworkStruct, IEquatable<ItemData>
{
    public short Item_ID;
    public int Item_Seed;
    public short Item_Count;
    public short Item_Info;
    public sbyte Item_Durability;
    public short Item_DurabilityPoint;
    public ContentData Item_Content;
    public bool Equals(ItemData other)
    {
        if (Item_ID == other.Item_ID && 
            Item_Seed == other.Item_Seed && 
            Item_Info == other.Item_Info && 
            Item_Count == other.Item_Count &&
            Item_Content.Equals(other.Item_Content))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public ItemData(short id)
    {
        Item_ID = id;
        Item_Count = 1;
        Item_Seed = 0;
        Item_Info = 0;
        Item_Durability = 0;
        Item_DurabilityPoint = 0;

        Item_Content = new ContentData();
    }
    public ItemData(ContentData contentData)
    {
        Item_ID = contentData.Item_ID;
        Item_Seed = contentData.Item_Seed;
        Item_Count = contentData.Item_Count;
        Item_Info = contentData.Item_Info;
        Item_Durability = contentData.Item_Durability;
        Item_DurabilityPoint = contentData.Item_DurabilityPoint;

        Item_Content = new ContentData();
    }
}
[Serializable]
public struct ContentData : INetworkStruct
{
    public short Item_ID;
    public int Item_Seed;
    public short Item_Count;
    public short Item_Info;
    public sbyte Item_Durability;
    public short Item_DurabilityPoint;
    public ContentData(ItemData itemData)
    {
        Item_ID = itemData.Item_ID;
        Item_Seed = itemData.Item_Seed;
        Item_Count = itemData.Item_Count;
        Item_Info = itemData.Item_Info;
        Item_Durability = itemData.Item_Durability;
        Item_DurabilityPoint = itemData.Item_DurabilityPoint;
    }
    public bool Equals(ContentData other)
    {
        if (Item_ID == other.Item_ID && Item_Seed == other.Item_Seed && Item_Count == other.Item_Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class InputData
{
    public float rightPressTimer = 0;
    public float leftPressTimer = 0;
    public Vector3 mousePosition = Vector3.zero;
}
