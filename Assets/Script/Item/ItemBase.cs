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
/// 基类
/// </summary>
public class ItemBase
{
    public ActorManager owner;

    #region//初始化
    public void ItemData()
    {

    }
    #endregion
    #region//数据相关
    /// <summary>
    /// 物品数据
    /// </summary>
    public ItemData itemData;
    /// <summary>
    /// 物品配置
    /// </summary>
    public ItemConfig itemConfig;
    /// <summary>
    /// 更新数据(网络更新)
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateDataFromNet(ItemData data)
    {
        itemData = data;
        itemConfig = ItemConfigData.GetItemConfig(data.Item_ID);
    }
    /// <summary>
    /// 更新数据(本地模拟)
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateDataFromLocal(ItemData data)
    {
        itemData = data;
    }
    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual void Holding_UpdateTime(int second)
    {

    }

    #endregion
    #region//UI相关
    /// <summary>
    /// 绘制格子
    /// </summary>
    public virtual void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Count.ToString());
    }
    /// <summary>
    /// 左击格子
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {

    }
    /// <summary>
    /// 右击格子
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void RightClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {

    }
    #endregion
    #region//持握部分
    #region//输入操作
    /// <summary>
    /// 输入数据
    /// </summary>
    public InputData inputData = new InputData();
    /// <summary>
    /// 左键按压
    /// </summary>
    /// <param name="pressTimer"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <returns>最大值</returns>
    public virtual bool Holding_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return true;
    }
    /// <summary>
    /// 左键释放
    /// </summary>
    /// <param name="state"></param>
    /// <param name="input"></param>
    public virtual void Holding_ReleaseLeftPress(bool state, bool input, bool player)
    {
    }
    /// <summary>
    /// 右键按压
    /// </summary>
    /// <param name="pressTimer"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <returns>最大值</returns>
    public virtual bool Holding_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return true;
    }
    /// <summary>
    /// 右键释放
    /// </summary>
    /// <param name="state"></param>
    /// <param name="input"></param>
    public virtual void Holding_ReleaseRightPress(bool state, bool input, bool player)
    {

    }
    /// <summary>
    /// 更新鼠标位置
    /// </summary>
    public virtual void Holding_UpdateMousePos(Vector3 mouse)
    {

    }
    #endregion
    #region//持握方法
    /// <summary>
    /// 开始持握
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
    /// 结束持握
    /// </summary>
    /// <param name="who"></param>
    public virtual void Holding_Over(ActorManager who)
    {

    }
    /// <summary>
    /// 更新外观
    /// </summary>
    public virtual void Holding_UpdateLook()
    {

    }
    /// <summary>
    /// 更改耐久度
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
    #region//穿戴部分
    /// <summary>
    /// 穿戴(头部)
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
    /// 穿戴(身体)
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
    #region//外置方法
    /// <summary>
    /// 数据初始化
    /// </summary>
    /// <returns></returns>
    public virtual void StaticAction_InitData(short id,out ItemData data)
    {
        data = new ItemData(id);
    }
    /// <summary>
    /// 堆叠
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
    /// 填充
    /// </summary>
    /// <param name="oldContent">旧容器</param>
    /// <param name="addItem">添加物</param>
    /// <param name="maxCap">容器最大容量</param>
    /// <param name="newContent">新容器</param>
    /// <param name="resItem">剩余物</param>
    public virtual void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
    }
    /// <summary>
    /// 外置方法(绘制NetObj)
    /// </summary>
    public virtual void StaticAction_DrawItemObj(ItemNetObj obj,ItemData data)
    {
        obj.spriteIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
        obj.textMeshPro.text = data.Item_Count.ToString();
    }
    /// <summary>
    /// 外置方法(播放掉落动画)
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
/// 基本材料
/// </summary>
public class ItemBase_Materials : ItemBase
{
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Count.ToString());
    }
}
/// <summary>
/// 基本食物
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
    /// 计算新鲜度
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
/// 基本武器
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
/// 基本工具
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
/// 基本枪
/// </summary>
public class ItemBase_Gun : ItemBase
{
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + itemConfig.ItemRarity, itemConfig.Item_Name.ToString(), itemData.Item_Content.Item_Count.ToString());
    }
}
/// <summary>
/// 基本服装
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
