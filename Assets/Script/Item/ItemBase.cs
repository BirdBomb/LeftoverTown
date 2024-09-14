
using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 基本物体
/// </summary>
public class ItemBase
{
    public ActorManager owner;

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
    /// 更新数据
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateData(ItemData data)
    {
        itemData = data;
        itemConfig = ItemConfigData.GetItemConfig(data.Item_ID);
    }
    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual void UpdateTime(int second)
    {

    }

    #endregion
    #region//UI相关
    /// <summary>
    /// 绘制格子
    /// </summary>
    public virtual void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Count.ToString();
        gridCell.panel_Bar.gameObject.SetActive(false);
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
    /// 右键点击
    /// </summary>
    public virtual void ClickRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// 右键按压
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <param name="showSI"></param>
    /// <returns>达到最大值</returns>
    public virtual bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        return true;
    }
    /// <summary>
    /// 右键释放
    /// </summary>
    public virtual void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// 左键点击
    /// </summary>
    public virtual void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// 左键按压
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <param name="showSI"></param>
    /// <returns>达到最大值</returns>
    public virtual bool PressLeftClick(float dt, bool state, bool input, bool showSI)
    {
        return true;
    }
    /// <summary>
    /// 左键释放
    /// </summary>
    public virtual void ReleaseLeftClick(float dt, bool state, bool input, bool showSI)
    {

    }
    /// <summary>
    /// 鼠标位置
    /// </summary>
    public virtual void InputMousePos(Vector3 mouse, float time)
    {

    }

    #endregion
    #region//持握方法
    /// <summary>
    /// 开始持握
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="item"></param>
    public virtual void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
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
        if (val != 0 && owner.isPlayer) 
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            if (_newItem.Item_Durability + val <= 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemOnHand()
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
    public virtual void BeWearingOnHead(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Head_Item.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
    }
    /// <summary>
    /// 穿戴(身体)
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void BeWearingOnBody(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Body_Item.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
    }

    #endregion
    #region//外置方法
    /// <summary>
    /// 外置方法(数据初始化)
    /// </summary>
    /// <returns></returns>
    public virtual void StaticAction_InitData(ItemData baseData,out ItemData initData)
    {
        initData = baseData;
    }
    /// <summary>
    /// 外置方法(堆叠)
    /// </summary>
    /// <param name="mainItem"></param>
    /// <param name="addItem"></param>
    /// <param name="newItem"></param>
    public virtual void StaticAction_PileUp(ItemData mainItem, ItemData addItem, short maxCap, out ItemData newItem,out ItemData resItem)
    {
        newItem = mainItem;
        resItem = mainItem;
        if(mainItem.Item_Count + addItem.Item_Count <= maxCap)
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
    /// 外置方法(填充堆叠)
    /// </summary>
    /// <param name="mainItem"></param>
    /// <param name="addItem"></param>
    /// <param name="newItem"></param>
    /// <param name="resItem"></param>
    public virtual void StaticAction_FillUp(ItemData mainItem, ItemData contentItem, short maxCap, out ItemData newItem, out ItemData resItem)
    {
        newItem = mainItem;
        resItem = contentItem;
        newItem.Item_Content = new ContentData(contentItem);
        newItem.Item_Content.Item_Count = mainItem.Item_Content.Item_Count;
        if (mainItem.Item_Content.Item_Count + contentItem.Item_Count <= maxCap)
        {
            newItem.Item_Content.Item_Count += contentItem.Item_Count;
            resItem.Item_Count = 0;
        }
        else
        {
            newItem.Item_Content.Item_Count = maxCap;
            resItem.Item_Count = ((short)(mainItem.Item_Content.Item_Count + contentItem.Item_Count - maxCap));
        }
    }
    /// <summary>
    /// 外置方法(绘制NetObj)
    /// </summary>
    public virtual void StaticAction_DrawItemObj(ItemNetObj obj,ItemData data)
    {
        obj.icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
    }
    /// <summary>
    /// 外置方法(播放掉落动画)
    /// </summary>
    /// <param name="obj"></param>
    public virtual void StaticAction_PlayDropAnim(ItemNetObj obj)
    {
        obj.root.enabled = true;
        obj.icon.transform.DOKill();
        obj.icon.transform.localScale = Vector3.zero;
        UnityEngine.Random.InitState(itemData.Item_Seed);
        Vector3 point = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(0.2f, 0.5f);
        obj.icon.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        obj.icon.transform.DOLocalJump(point, 1, 1, 0.5f).OnComplete(() =>
        {
            obj.root.enabled = false;
            obj.icon.transform.DOPunchScale(new Vector3(0.2f, -0.2f, 0), 0.1f).SetEase(Ease.OutBack);
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
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Count.ToString();
        gridCell.panel_Bar.gameObject.SetActive(false);
    }
}
/// <summary>
/// 基本食材
/// </summary>
public class ItemBase_Ingredient: ItemBase
{
    public override void StaticAction_InitData(ItemData baseData, out ItemData initData)
    {
        baseData.Item_Info = 100;
        baseData.Item_Durability = 100;
        baseData.Item_DurabilityPoint = (short)(MapManager.Instance.mapNetManager.Date * 10 + MapManager.Instance.mapNetManager.Hour);
        initData = baseData;
    }
    public override void UpdateData(ItemData itemData)
    {
        base.UpdateData(itemData);
        CalculateDurability();
    }
    public override void StaticAction_FillUp(ItemData mainItem, ItemData contentItem, short maxCap, out ItemData newItem, out ItemData resItem)
    {
        newItem = mainItem;
        resItem = contentItem;
        newItem.Item_Content = new ContentData(contentItem);
        newItem.Item_Content.Item_Count = mainItem.Item_Content.Item_Count;

        int wa_Dp = mainItem.Item_Content.Item_DurabilityPoint * mainItem.Item_Content.Item_Count + contentItem.Item_DurabilityPoint * contentItem.Item_Count;
        int wa_Dv = mainItem.Item_Content.Item_Durability * mainItem.Item_Content.Item_Count + contentItem.Item_Durability * contentItem.Item_Count;
        newItem.Item_Content.Item_DurabilityPoint = (short)((float)wa_Dp / (float)(contentItem.Item_Count + mainItem.Item_Content.Item_Count));
        newItem.Item_Content.Item_Durability = (sbyte)((float)wa_Dv / (float)(contentItem.Item_Count + mainItem.Item_Content.Item_Count));
        resItem.Item_DurabilityPoint = (short)((float)wa_Dp / (float)(contentItem.Item_Count + mainItem.Item_Content.Item_Count));
        resItem.Item_Durability = (sbyte)((float)wa_Dv / (float)(contentItem.Item_Count + mainItem.Item_Content.Item_Count));

        if (mainItem.Item_Content.Item_Count + contentItem.Item_Count <= maxCap)
        {
            newItem.Item_Content.Item_Count += contentItem.Item_Count;
            resItem.Item_Count = 0;
        }
        else
        {
            newItem.Item_Content.Item_Count = maxCap;
            resItem.Item_Count = ((short)(mainItem.Item_Content.Item_Count + contentItem.Item_Count - maxCap));
        }
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
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Count.ToString();
        gridCell.panel_Bar.gameObject.SetActive(true);
        gridCell.image_Bar.transform.localScale = new Vector3(itemData.Item_Durability / 100f, 1, 1);
        if (itemData.Item_Info == 0)
        {
            gridCell.FreezeCell(true);
        }
        else
        {
            gridCell.FreezeCell(false);
            gridCell.image_Bar.color = new Color(Mathf.Lerp(1, 0, itemData.Item_Durability / 100f), Mathf.Lerp(0f, 1, itemData.Item_Durability / 100f), 0, 1);
        }
    }
    /// <summary>
    /// 消耗
    /// </summary>
    /// <param name="val"></param>
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
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryRemoveItemOnHand()
            {
                item = itemData,
            });
        }
    }
}
/// <summary>
/// 基本食物
/// </summary>
public class ItemBase_Food : ItemBase
{
    public override void StaticAction_InitData(ItemData baseData,out ItemData initData)
    {
        baseData.Item_Info = 100;
        baseData.Item_Durability = 100;
        baseData.Item_DurabilityPoint = (short)(MapManager.Instance.mapNetManager.Date * 10 + MapManager.Instance.mapNetManager.Hour);
        initData = baseData;
    }
    public override void UpdateData(ItemData itemData)
    {
        base.UpdateData(itemData);
        CalculateDurability();
    }
    public override void StaticAction_FillUp(ItemData mainItem, ItemData contentItem, short maxCap, out ItemData newItem, out ItemData resItem)
    {
        newItem = mainItem;
        resItem = contentItem;
        newItem.Item_Content = new ContentData(contentItem);
        newItem.Item_Content.Item_Count = mainItem.Item_Content.Item_Count;

        int wa_Dp = mainItem.Item_Content.Item_DurabilityPoint * mainItem.Item_Content.Item_Count + contentItem.Item_DurabilityPoint * contentItem.Item_Count;
        int wa_Dv = mainItem.Item_Content.Item_Durability * mainItem.Item_Content.Item_Count + contentItem.Item_Durability * contentItem.Item_Count;
        newItem.Item_Content.Item_DurabilityPoint = (short)((float)wa_Dp / (float)(contentItem.Item_Count + mainItem.Item_Content.Item_Count));
        newItem.Item_Content.Item_Durability = (sbyte)((float)wa_Dv / (float)(contentItem.Item_Count + mainItem.Item_Content.Item_Count));
        resItem.Item_DurabilityPoint = (short)((float)wa_Dp / (float)(contentItem.Item_Count + mainItem.Item_Content.Item_Count));
        resItem.Item_Durability = (sbyte)((float)wa_Dv / (float)(contentItem.Item_Count + mainItem.Item_Content.Item_Count));

        if (mainItem.Item_Content.Item_Count + contentItem.Item_Count <= maxCap)
        {
            newItem.Item_Content.Item_Count += contentItem.Item_Count;
            resItem.Item_Count = 0;
        }
        else
        {
            newItem.Item_Content.Item_Count = maxCap;
            resItem.Item_Count = ((short)(mainItem.Item_Content.Item_Count + contentItem.Item_Count - maxCap));
        }
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
        }
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Count.ToString();
        gridCell.panel_Bar.gameObject.SetActive(true);
        gridCell.image_Bar.transform.localScale = new Vector3(itemData.Item_Durability / 100f, 1, 1);
        if (itemData.Item_Info == 0)
        {
            gridCell.FreezeCell(true);
        }
        else
        {
            gridCell.image_Bar.color = new Color(Mathf.Lerp(1, 0, itemData.Item_Durability / 100f), Mathf.Lerp(0f, 1, itemData.Item_Durability / 100f), 0, 1);
            gridCell.FreezeCell(false);
        }
    }

}
/// <summary>
/// 基本武器
/// </summary>
public class ItemBase_Weapon : ItemBase
{
    public override void StaticAction_InitData(ItemData baseItem,out ItemData initData)
    {
        UnityEngine.Random.InitState(baseItem.Item_Seed);
        baseItem.Item_Durability = (sbyte)UnityEngine.Random.Range(25, 100);
        initData = baseItem;
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Durability.ToString() + "%";
        gridCell.panel_Bar.gameObject.SetActive(false);
    }
}
/// <summary>
/// 基本工具
/// </summary>
public class ItemBase_Tool : ItemBase
{
    public override void StaticAction_InitData(ItemData baseItem, out ItemData initData)
    {
        UnityEngine.Random.InitState(baseItem.Item_Seed);
        baseItem.Item_Durability = (sbyte)UnityEngine.Random.Range(25, 100);
        initData = baseItem;
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Durability.ToString() + "%";
        gridCell.panel_Bar.gameObject.SetActive(false);
    }
}
/// <summary>
/// 基本枪
/// </summary>
public class ItemBase_Gun : ItemBase
{
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Content.Item_Count.ToString();
        gridCell.panel_Bar.gameObject.SetActive(false);
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
        if (Item_ID == other.Item_ID && Item_Seed == other.Item_Seed && Item_Count == other.Item_Count && Item_Info == other.Item_Info)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public ItemData(short id, int seed, short count, short info, sbyte durability, short durabilityPoint, ContentData content)
    {
        Item_ID = id;
        Item_Seed = seed;
        Item_Count = count;
        Item_Info = info;
        Item_Durability = durability;
        Item_DurabilityPoint = durabilityPoint;

        Item_Content = content;
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

}
public class InputData
{
    public bool rightPressState = false;
    public float rightPressTimer = 0;
    public bool leftPressState = false;
    public float leftPressTimer = 0;
    public Vector3 mousePosition = Vector3.zero;
}
