using DG.Tweening;
using Fusion;
using System;
using System.IO.Ports;
using System.Text;
using UniRx;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using WebSocketSharp;
using static UnityEngine.UI.GridLayoutGroup;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// 基类
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
    /// 物品品质
    /// </summary>
    public ItemQuality itemQuality;
    /// <summary>
    /// 物品配置
    /// </summary>
    public ItemConfig itemConfig;
    /// <summary>
    /// 物品地址
    /// </summary>
    public ItemPath itemPath;
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void BindPath(ItemPath path)
    {
        itemPath = path;
    }
    /// <summary>
    /// 更新数据(网络更新)
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateDataFromNet(ItemData data)
    {
        itemData = data;
        itemConfig = ItemConfigData.GetItemConfig(data.I);
        CalculateQuality();
    }
    /// <summary>
    /// 更新数据(本地模拟)
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateDataFromLocal(ItemData data)
    {
        itemData = data;
        itemConfig = ItemConfigData.GetItemConfig(data.I);
        CalculateQuality();
    }
    /// <summary>
    /// 计算物品品质
    /// </summary>
    /// <returns></returns>
    public virtual void CalculateQuality()
    {
        UnityEngine.Random.InitState(itemData.V);
        int seed = UnityEngine.Random.Range(0, 10000);
        itemQuality = seed switch
        {
            < 7000 => ItemQuality.Gray,
            < 9000 => ItemQuality.Green,
            < 9700 => ItemQuality.Blue,
            < 9900 => ItemQuality.Purple,
            < 9970 => ItemQuality.Gold,
            < 9990 => ItemQuality.Red,
            _ => ItemQuality.Rainbow
        };
    }
    #endregion
    #region//UI相关
    /// <summary>
    /// 绘制格子
    /// </summary>
    public virtual void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", $"Item_{itemConfig.Item_ID}").Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");

        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        string stringInfo = $"{stringName}\n{stringDesc}";
        gridCell.DrawCell($"Item_{itemData.I}", $"ItemBG_{(int)itemConfig.Item_Rarity}", itemData.C.ToString());
        gridCell.SetCell(stringInfo);
    }
    /// <summary>
    /// 修改描述
    /// </summary>
    /// <param name="desc"></param>
    /// <returns></returns>
    public virtual string GridCell_UpdateDesc(string desc)
    {
        return desc;
    }
    /// <summary>
    /// 左击格子
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void GridCell_LeftClick(UI_GridCell gridCell, ItemData itemData)
    {
        
    }
    /// <summary>
    /// 右击格子
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        switch (gridCell.itemPath_Bind.itemFrom)
        {
            case ItemFrom.Bag:
                {
                    InBag_Use();
                }
                break;
            case ItemFrom.Hand:
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_PutAway() { });
                }
                break;
            case ItemFrom.Head:
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHead_PutAway() { });
                }
                break;
            case ItemFrom.Body:
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBody_PutAway() { });
                }
                break;
            case ItemFrom.Accessory:
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemAccessory_PutAway() { });
                }
                break;
            case ItemFrom.Consumables:
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_PutAway() { });
                }
                break;
        }
    }
    #endregion
    #region//NetObj相关
    /// <summary>
    /// 绘制网络物体
    /// </summary>
    /// <param name="itemNetObj"></param>
    /// <param name="data"></param>
    public virtual void NetObj_Draw(ItemNetObj itemNetObj, ItemData data)
    {
        itemNetObj.spriteRenderer_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.I);
        itemNetObj.textMesh_Count.text = data.C.ToString();
    }
    /// <summary>
    /// 播放掉落动画
    /// </summary>
    /// <param name="obj"></param>
    public virtual void NetObj_PlayDrop(ItemNetObj itemNetObj)
    {
        itemNetObj.transform_Root.transform.DOKill();
        itemNetObj.transform_Root.transform.localScale = Vector3.zero;
        itemNetObj.transform_Root.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        itemNetObj.transform_Root.transform.DOLocalJump(Vector3.zero, 1, 1, 0.5f).OnComplete(() =>
        {
            itemNetObj.transform_Root.transform.DOPunchScale(new Vector3(0.2f, -0.2f, 0), 0.1f).SetEase(Ease.OutBack);
        }).SetEase(Ease.InOutQuad);
    }
    #endregion
    #region//在背包
    public virtual void InBag_Use()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Switch()
        {
            index = itemPath.itemIndex
        });
    }
    #endregion
    #region//在手上
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
    public virtual bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return true;
    }
    /// <summary>
    /// 左键释放
    /// </summary>
    /// <param name="state"></param>
    /// <param name="input"></param>
    public virtual void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {

    }
    /// <summary>
    /// 右键按压
    /// </summary>
    /// <param name="pressTimer"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <returns>最大值</returns>
    public virtual bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return true;
    }
    /// <summary>
    /// 右键释放
    /// </summary>
    /// <param name="state"></param>
    /// <param name="input"></param>
    public virtual void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {

    }
    /// <summary>
    /// 更新鼠标位置
    /// </summary>
    public virtual void OnHand_UpdateMousePos(Vector3 mouse)
    {

    }
    /// <summary>
    /// 开始持握
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="item"></param>
    public virtual void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.ShowRightHandItem(itemData.I);
    }
    /// <summary>
    /// 结束持握
    /// </summary>
    /// <param name="owner"></param>
    public virtual void OnHand_Over(ActorManager owner, BodyController_Human body)
    {

    }
    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual void OnHand_UpdateTime(int second)
    {

    }
    /// <summary>
    /// 更新外观
    /// </summary>
    public virtual void OnHand_UpdateLook()
    {

    }
    #endregion
    #region//在头上
    /// <summary>
    /// 开始穿戴
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.ShowHeadItem(itemData.I);
    }
    /// <summary>
    /// 结束穿戴
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnHead_Over(ActorManager owner, BodyController_Human body)
    {

    }
    public virtual int OnHead_CalculateArmor(int armor)
    {
        return armor;
    }
    public virtual int OnHead_CalculateResistance(int resistance)
    {
        return resistance;
    }
    public virtual void OnHead_UpdateTime(int second)
    {

    }

    #endregion
    #region//在身上
    /// <summary>
    /// 开始穿戴
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.ShowBodyItem(itemData.I);
    }
    /// <summary>
    /// 结束穿戴
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnBody_Over(ActorManager owner, BodyController_Human body)
    {

    }
    public virtual int OnBody_CalculateArmor(int armor)
    {
        return armor;
    }
    public virtual int OnBody_CalculateResistance(int resistance)
    {
        return resistance;
    }
    public virtual void OnBody_UpdateTime(int second)
    {

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
    /// 合并
    /// </summary>
    /// <param name="itemData_CombineA">合并物体A</param>
    /// <param name="itemData_CombineB">合并物体B</param>
    /// <param name="maxCombineCount">合并最大量</param>
    /// <param name="itemData_Res">合并物体剩余</param> 
    public virtual void StaticAction_Combine(ItemData itemData_CombineA, ItemData itemData_CombineB, short maxCombineCount, out ItemData itemData_Combine, out ItemData itemData_Res)
    {
        itemData_Combine = itemData_CombineA;
        itemData_Res = itemData_CombineB;
        if(itemData_CombineA.C + itemData_CombineB.C <= maxCombineCount)
        {
            itemData_Combine.C += itemData_CombineB.C;
            itemData_Res.C = 0;
        }
        else
        {
            itemData_Combine.C = maxCombineCount;
            itemData_Res.C = ((short)(itemData_CombineA.C + itemData_CombineB.C - maxCombineCount));
        }
    }
    /// <summary>
    /// 填充
    /// </summary>
    /// <param name="oldContent">旧容器</param>
    /// <param name="addItem">添加物</param>
    /// <param name="newContent">新容器</param>
    /// <param name="resItem">剩余物</param>
    public virtual void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
    }
    #endregion
}
/// <summary>
/// -----基本材料-----
/// </summary>
public class ItemBase_Materials : ItemBase
{

}
/// <summary>
/// -----基本书-----
/// </summary>
public class ItemBase_Book : ItemBase 
{
    #region//持有
    protected ItemLocalObj_Book itemLocalObj_Book;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Book = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_Book").GetComponent<ItemLocalObj_Book>();
        itemLocalObj_Book.InitData(itemData);
        itemLocalObj_Book.HoldingStart(owner, body);
    }
    #endregion
}
/// <summary>
/// -----基本食物-----
/// </summary>
public class ItemBase_Food : ItemBase
{
    #region//数据
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.V = 100;
        initData.D = 100;
        WorldManager.Instance.GetTime_Detail(out int day, out int hour, out _);
        initData.S = (short)(day * 10 + hour);
    }
    public override void UpdateDataFromNet(ItemData itemData)
    {
        base.UpdateDataFromNet(itemData);
        CalculateDurability();
    }
    /// <summary>
    /// 腐烂基本数值
    /// </summary>
    private const int int_rotBase = -5;
    /// <summary>
    /// 计算新鲜度
    /// </summary>
    /// <param name="nowTime"></param>
    public virtual void CalculateDurability()
    {
        /*当前时间*/
        WorldManager.Instance.GetTime_Detail(out int day, out int hour, out _);
        int nowTime = day * 10 + hour;
        int lastTime = itemData.S;
        /*腐败速率*/
        float rotSpeed = itemData.V * 0.01f;
        int offset = (int)((nowTime - lastTime) * rotSpeed * int_rotBase);
        if (offset <= -1)
        {
            /*腐烂大于1*/
            itemData.D = (itemData.D + offset >= 0) ? (sbyte)(itemData.D + offset) : (sbyte)0;
            itemData.S = (short)nowTime;
        }
        else
        {
            /*腐烂小于1*/
        }
    }
    public override void StaticAction_Combine(ItemData mainItem, ItemData addItem, short maxCap, out ItemData newItem, out ItemData resItem)
    {
        newItem = mainItem;
        resItem = mainItem;
        int totalCount = mainItem.C + addItem.C;
        short avgS = (short)((float)(mainItem.S * mainItem.C + addItem.S * addItem.C) / totalCount);
        sbyte avgD = (sbyte)((float)(mainItem.D * mainItem.C + addItem.D * addItem.C) / totalCount);
        // 设置平均值
        newItem.S = avgS;
        newItem.D = avgD;
        resItem.S = avgS;
        resItem.D = avgD;
        // 处理数量
        if (totalCount <= maxCap)
        {
            newItem.C = (short)totalCount;
            resItem.C = 0;
        }
        else
        {
            newItem.C = maxCap;
            resItem.C = (short)(totalCount - maxCap);
        }
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + itemConfig.Item_ID).Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");

        string stringInfo;
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        if (itemData.D <= 0)
        {
            string stringRotten = LocalizationManager.Instance.GetLocalization("Item_String", "Rotten");
            stringInfo = $"{stringName}({stringRotten})\n{stringDesc}";
        }
        else
        {
            stringInfo = $"{stringName}\n{stringDesc}";
        }
        gridCell.DrawCell($"Item_{itemData.I}", $"ItemBG_{(int)itemConfig.Item_Rarity}", itemData.C.ToString());
        gridCell.SetCell(stringInfo);
        gridCell.SetSliderVal(itemData.D / 100f);
        bool isFrozen = itemData.V == 0;
        gridCell.FreezeCell(isFrozen);
        if (isFrozen) gridCell.SetSliderColor(new Color(0.5f, 1, 0, 1));
        else
        {
            float t = itemData.D / 100f;
            gridCell.SetSliderColor(new Color(1 - t, t, 0, 1));
        }
    }

    #endregion
    #region//持有
    protected ItemLocalObj_Food itemLocalObj_Food;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Food = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_Food").GetComponent<ItemLocalObj_Food>();
        itemLocalObj_Food.InitData(itemData);
        itemLocalObj_Food.HoldingStart(owner, body);
        itemLocalObj_Food.BindFoodAction(OnHand_EatAction);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        itemLocalObj_Food.PressLeftMouse(pressTimer, owner.actorAuthority);
        return base.OnHand_UpdateLeftPress(pressTimer, state, input, player);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Food.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public virtual void OnHand_EatAction(ActorManager actor)
    {
        OnHnad_Expend(1);
    }
    public void OnHnad_Expend(int val)
    {
        if (itemData.C > val)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            _newItem.C = (short)(_newItem.C - val);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
            {
                item = itemData,
            });
        }
    }
    #endregion
}
/// <summary>
/// -----基本药剂-----
/// </summary>
public class ItemBase_Potion : ItemBase
{
    #region//持有
    private ItemLocalObj_Potion itemLocalObj_Potion;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Potion = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_Potion").GetComponent<ItemLocalObj_Potion>();
        itemLocalObj_Potion.InitData(itemData);
        itemLocalObj_Potion.HoldingStart(owner, body);
        itemLocalObj_Potion.BindPotionAction(OnHand_DrinkAction);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        itemLocalObj_Potion.PressLeftMouse(pressTimer, owner.actorAuthority);
        return base.OnHand_UpdateLeftPress(pressTimer, state, input, player);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Potion.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public virtual void OnHand_DrinkAction(ActorManager actor)
    {
        OnHnad_Expend(1);
    }
    public void OnHnad_Expend(int val)
    {
        if (itemData.C > val)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            _newItem.C = (short)(_newItem.C - val);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
            {
                item = itemData,
            });
        }
    }

    #endregion

}
/// <summary>
/// -----基本武器-----
/// </summary>
public class ItemBase_Weapon : ItemBase
{
    public override void StaticAction_InitData(short id,out ItemData initData)
    {
        initData = new ItemData(id);
        initData.V = (short)new System.Random().Next(0, short.MaxValue);
        initData.D = (sbyte)new System.Random().Next(80, 101);
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", $"Item_{itemConfig.Item_ID}").Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);
        gridCell.DrawCell($"Item_{itemData.I}", $"ItemBG_{(int)itemConfig.Item_Rarity}", $"{itemData.D}%");
        gridCell.SetCell($"{stringName}({stringQuality})\n{stringDesc}");
    }
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal && owner.actorAuthority.isPlayer)
        {
            CursorManager.Instance.AddCursor(CursorManager.CursorType.Weapon);
        }
    }
    public override void OnHand_Over(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal && owner.actorAuthority.isPlayer)
        {
            CursorManager.Instance.SubCursor(CursorManager.CursorType.Weapon);
        }
    }
}
/// <summary>
/// -----基本工具-----
/// </summary>
public class ItemBase_Tool : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.V = (short)new System.Random().Next(0, short.MaxValue);
        initData.D = (sbyte)new System.Random().Next(80, 101);
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", $"Item_{itemConfig.Item_ID}").Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);
        gridCell.DrawCell($"Item_{itemData.I}", $"ItemBG_{(int)itemConfig.Item_Rarity}", $"{itemData.D}%");
        gridCell.SetCell($"{stringName}({stringQuality})\n{stringDesc}");
    }
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal && owner.actorAuthority.isPlayer)
        {
            CursorManager.Instance.AddCursor(CursorManager.CursorType.Tool);
        }
    }
    public override void OnHand_Over(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal && owner.actorAuthority.isPlayer)
        {
            CursorManager.Instance.SubCursor(CursorManager.CursorType.Tool);
        }
    }
}
/// <summary>
/// -----基本枪-----
/// </summary>
public class ItemBase_Gun : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.V = (short)new System.Random().Next(short.MinValue, short.MaxValue);
        initData.D = (sbyte)new System.Random().Next(sbyte.MinValue, sbyte.MaxValue);
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", $"Item_{itemConfig.Item_ID}").Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);

        gridCell.DrawCell($"Item_{itemData.I}", $"ItemBG_{(int)itemConfig.Item_Rarity}", "");
        gridCell.SetCell($"{stringName}({stringQuality})\n{stringDesc}");
    }
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal && owner.actorAuthority.isPlayer)
        {
            CursorManager.Instance.AddCursor(CursorManager.CursorType.Aim);
        }
    }
    public override void OnHand_Over(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal && owner.actorAuthority.isPlayer)
        {
            CursorManager.Instance.SubCursor(CursorManager.CursorType.Aim);
        }
    }
}
/// <summary>
/// -----基本投掷物-----
/// </summary>
public class ItemBase_Throwable : ItemBase
{
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal && owner.actorAuthority.isPlayer)
        {
            CursorManager.Instance.AddCursor(CursorManager.CursorType.Aim);
        }
    }
    public override void OnHand_Over(ActorManager owner, BodyController_Human body)
    {
        if (owner.actorAuthority.isLocal && owner.actorAuthority.isPlayer)
        {
            CursorManager.Instance.SubCursor(CursorManager.CursorType.Aim);
        }
    }
}
/// <summary>
/// -----基本服装-----
/// </summary>
public class ItemBase_Clothes : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.V = (short)new System.Random().Next(0, short.MaxValue);
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", $"Item_{itemConfig.Item_ID}").Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);
        gridCell.DrawCell($"Item_{itemData.I}", $"ItemBG_{(int)itemConfig.Item_Rarity}", itemData.C.ToString());
        gridCell.SetCell($"{stringName}({stringQuality})\n{stringDesc}");
    }
    public override void InBag_Use()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBody_Switch()
        {
            index = itemPath.itemIndex
        });
    }
    public override void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        GameObject itemLocalObj = PoolManager.Instance.GetObject($"ItemObj/ItemLocalObj_Clothes");
        body.AddItemOnBody(itemLocalObj, Vector3.zero, Quaternion.identity, Vector3.one);  
        if (itemLocalObj.TryGetComponent(out ItemLocalObj localObj)) localObj.InitData(itemData);
    }
}
/// <summary>
/// -----基本帽子-----
/// </summary>
public class ItemBase_Hat : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.V = (short)new System.Random().Next(0, short.MaxValue);
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", $"Item_{itemConfig.Item_ID}").Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);
        gridCell.DrawCell($"Item_{itemData.I}", $"ItemBG_{(int)itemConfig.Item_Rarity}", itemData.C.ToString());
        gridCell.SetCell($"{stringName}({stringQuality})\n{stringDesc}");
    }
    public override void InBag_Use()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHead_Switch()
        {
            index = itemPath.itemIndex
        });
    }
    public override void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        GameObject itemLocalObj = PoolManager.Instance.GetObject($"ItemObj/ItemLocalObj_Hat");
        body.AddItemOnHead(itemLocalObj, Vector3.zero, Quaternion.identity, Vector3.one);
        if (itemLocalObj.TryGetComponent(out ItemLocalObj localObj)) localObj.InitData(itemData);
    }
}
/// <summary>
/// -----基本饰品-----
/// </summary>
public class Itembase_Accessory : ItemBase
{
    public override void InBag_Use()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemAccessory_Switch()
        {
            index = itemPath.itemIndex
        });
    }
}
/// <summary>
/// -----基本耗材-----
/// </summary>
public class ItemBase_Consumables : ItemBase
{
    public override void InBag_Use()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_Switch()
        {
            index = itemPath.itemIndex
        });
    }
}
[Serializable]
public struct ItemData : INetworkStruct, IEquatable<ItemData>
{
    /// <summary>
    /// id
    /// </summary>
    public short I;
    /// <summary>
    /// count
    /// </summary>
    public short C;
    /// <summary>
    /// value
    /// </summary>
    public short V;
    /// <summary>
    /// durability
    /// </summary>
    public sbyte D;
    /// <summary>
    /// signTime
    /// </summary>
    public short S;
    public bool Equals(ItemData other)
    {
        if (I == other.I && 
            V == other.V && 
            C == other.C)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool FullyEqual(ItemData other)
    {
        if (I == other.I && V == other.V && C == other.C && D == other.D && S == other.S)
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
        I = id;
        if (I == 0)
        {
            C = 0;
        }
        else
        {
            C = 1;
        }
        V = 0;
        D = 0;
        S = 0;
    }
}
/// <summary>
/// 物品地址
/// </summary>
public struct ItemPath
{
    public ItemFrom itemFrom;
    public int itemIndex;
    public ItemPath(ItemFrom from, int index)
    {
        itemFrom = from; 
        itemIndex = index;
    }
}
/// <summary>
/// 物品所属
/// </summary>
public enum ItemFrom
{
    Default,
    OutSide,//野外
    Hand,//手部
    Body,//身体
    Head,//头部
    Accessory,//饰品
    Consumables,//耗材
    Bag,//背包
}
public static class ItemDataJsonHelper
{
    public static string ToCompactJson(ItemData item)
    {
        return $"[{item.I},{item.C},{item.V},{item.D},{item.S}]";
    }
    public static ItemData FromCompactJson(string json)
    {
        if (string.IsNullOrEmpty(json) || json.Length < 3)
            return new ItemData(0);

        // 去除方括号
        string trimmed = json.TrimStart('[').TrimEnd(']');
        string[] parts = trimmed.Split(',');

        if (parts.Length >= 5)
        {
            return new ItemData
            {
                I = short.Parse(parts[0]),
                C = short.Parse(parts[1]),
                V = short.Parse(parts[2]),
                D = sbyte.Parse(parts[3]),
                S = short.Parse(parts[4])
            };
        }
        return new ItemData(0);
    }
    public static string SerializeArray(ItemData[] items)
    {
        if (items == null || items.Length == 0)
            return "[]";

        var sb = new StringBuilder();
        sb.Append('[');

        for (int i = 0; i < items.Length; i++)
        {
            if (i > 0) sb.Append(',');
            sb.Append(ToCompactJson(items[i]));
        }

        sb.Append(']');
        return sb.ToString();
    }
    public static ItemData[] DeserializeArray(string json)
    {
        if (string.IsNullOrEmpty(json) || json.Length < 3)
            return new ItemData[0];

        // 去除最外层方括号
        string inner = json.Substring(1, json.Length - 2);
        if (string.IsNullOrEmpty(inner))
            return new ItemData[0];

        // 按 "],[" 分割
        string[] itemStrings = inner.Split(new[] { "],[" }, System.StringSplitOptions.None);
        var result = new ItemData[itemStrings.Length];

        for (int i = 0; i < itemStrings.Length; i++)
        {
            result[i] = FromCompactJson("[" + itemStrings[i] + "]");
        }

        return result;
    }
    public static string SerializeList(System.Collections.Generic.List<ItemData> items)
    {
        if (items == null || items.Count == 0)
            return "[]";

        var sb = new StringBuilder();
        sb.Append('[');

        for (int i = 0; i < items.Count; i++)
        {
            if (i > 0) sb.Append(',');
            sb.Append(ToCompactJson(items[i]));
        }

        sb.Append(']');
        return sb.ToString();
    }
    public static System.Collections.Generic.List<ItemData> DeserializeList(string json)
    {
        var array = DeserializeArray(json);
        return new System.Collections.Generic.List<ItemData>(array);
    }
}
public class InputData
{
    public float rightPressTimer = 0;
    public float leftPressTimer = 0;
    public Vector3 mousePosition = Vector3.zero;
}
