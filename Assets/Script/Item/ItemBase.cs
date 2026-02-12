using DG.Tweening;
using Fusion;
using System;
using System.IO.Ports;
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
        if (seed < 7000)
        {
            itemQuality = ItemQuality.Gray;
        }
        else if (seed < 9000)
        {
            itemQuality = ItemQuality.Green;
        }
        else if (seed < 9700)
        {
            itemQuality = ItemQuality.Blue;
        }
        else if (seed < 9900)
        {
            itemQuality = ItemQuality.Purple;
        }
        else if (seed < 9970)
        {
            itemQuality = ItemQuality.Gold;
        }
        else if (seed < 9990)
        {
            itemQuality = ItemQuality.Red;
        }
        else
        {
            itemQuality = ItemQuality.Rainbow;
        }
    }
    #endregion
    #region//UI相关
    /// <summary>
    /// 绘制格子
    /// </summary>
    public virtual void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + itemConfig.Item_ID).Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");

        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        string stringInfo = stringName + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.I.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, itemData.C.ToString());
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
        if (gridCell.itemPath_Bind.itemFrom == ItemFrom.Bag)
        {
            InBag_Use();
        }
        else if (gridCell.itemPath_Bind.itemFrom == ItemFrom.Hand)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_PutAway()
            {
                
            });
        }
        else if (gridCell.itemPath_Bind.itemFrom == ItemFrom.Head)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHead_PutAway()
            {

            });

        }
        else if (gridCell.itemPath_Bind.itemFrom == ItemFrom.Body)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBody_PutAway()
            {

            });
        }
        else if (gridCell.itemPath_Bind.itemFrom == ItemFrom.Accessory)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemAccessory_PutAway()
            {

            });
        }
        else if (gridCell.itemPath_Bind.itemFrom == ItemFrom.Consumables)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_PutAway()
            {

            });
        }
    }
    /// <summary>
    /// 打开子集格子
    /// </summary>
    /// <returns></returns>
    public virtual bool GridCell_OpenChildCrid()
    {
        return false;
    }
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
        body.transform_ItemInRightHand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.I);
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
        body.transform_ItemOnHead.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.I);
    }
    /// <summary>
    /// 结束穿戴
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnHead_Over(ActorManager owner, BodyController_Human body)
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
        body.transform_ItemOnBody.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.I);
    }
    /// <summary>
    /// 结束穿戴
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnBody_Over(ActorManager owner, BodyController_Human body)
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
/// -----基本食物-----
/// </summary>
public class ItemBase_Food : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.V = 100;
        initData.D = 100;
        WorldManager.Instance.GetTime(out int day, out int hour, out _);
        initData.S = (short)(day * 10 + hour);
    }
    public override void UpdateDataFromNet(ItemData itemData)
    {
        base.UpdateDataFromNet(itemData);
        CalculateDurability(int_rotBase);
    }
    /// <summary>
    /// 腐烂基本数值
    /// </summary>
    private int int_rotBase = -5;
    /// <summary>
    /// 计算新鲜度
    /// </summary>
    /// <param name="nowTime"></param>
    public virtual void CalculateDurability(float ratBase)
    {
        /*当前时间*/
        WorldManager.Instance.GetTime(out int day, out int hour, out _);
        int nowTime = day * 10 + hour;
        /*记录时间*/
        int lastTime = itemData.S;
        /*腐败速率*/
        float rotSpeed = itemData.V * 0.01f;
        int offset = (int)((nowTime - lastTime) * rotSpeed * ratBase);
        if (offset <= -1)
        {
            /*腐烂大于1*/
            if (itemData.D + offset >= 0)
            {
                itemData.D += (sbyte)offset;
                if(itemData.D <= 0) itemData.D = 0;
                itemData.S = (short)nowTime;
            }
            else
            {
                itemData.D = 0;
                itemData.S = (short)nowTime;
            }

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

        int wa_Dp = mainItem.S * mainItem.C + addItem.S * addItem.C;
        int wa_Dv = mainItem.D * mainItem.C + addItem.D * addItem.C;

        newItem.S = (short)((float)wa_Dp / (float)(addItem.C + mainItem.C));
        newItem.D = (sbyte)((float)wa_Dv / (float)(addItem.C + mainItem.C));
        resItem.S = (short)((float)wa_Dp / (float)(addItem.C + mainItem.C));
        resItem.D = (sbyte)((float)wa_Dv / (float)(addItem.C + mainItem.C));

        if (mainItem.C + addItem.C <= maxCap)
        {
            newItem.C += addItem.C;
            resItem.C = 0;
        }
        else
        {
            newItem.C = maxCap;
            resItem.C = ((short)(mainItem.C + addItem.C - maxCap));
        }
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + itemConfig.Item_ID).Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");

        string stringRotten = LocalizationManager.Instance.GetLocalization("Item_String","Rotten");
        string stringInfo;
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        if(itemData.D <= 0)
        {
            stringInfo = stringName + "("+ stringRotten + ")" + "\n" + stringDesc;
        }
        else
        {
            stringInfo = stringName + "\n" + stringDesc;
        }
        gridCell.DrawCell("Item_" + itemData.I.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, itemData.C.ToString());
        gridCell.SetCell(stringInfo);
        gridCell.SetSliderVal(itemData.D / 100f);
        if (itemData.V == 0)
        {
            gridCell.FreezeCell(true);
            gridCell.SetSliderColor(new Color(0.5f, 1, 0, 1));
        }
        else
        {
            gridCell.FreezeCell(false);
            gridCell.SetSliderColor(new Color(Mathf.Lerp(1, 0, itemData.D / 100f), Mathf.Lerp(0f, 1, itemData.D / 100f), 0, 1));
        }
    }
    #region//持有
    private ItemLocalObj_Food itemLocalObj_Food;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Food = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_Food").GetComponent<ItemLocalObj_Food>();
        itemLocalObj_Food.InitData(itemData);
        itemLocalObj_Food.HoldingStart(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        itemLocalObj_Food.PressLeftMouse(pressTimer, owner.actorAuthority);
        if (inputData.leftPressTimer == 0 && owner)
        {
            if (Check())
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                BodyController_Human bodyController_Human = (BodyController_Human)owner.bodyController;
                bodyController_Human.transform_ItemInRightHand.DOKill();
                bodyController_Human.transform_ItemInRightHand.localScale = Vector3.one;
                bodyController_Human.transform_ItemInRightHand.DOPunchScale(new Vector3(-0.2f, 0.2f, 1), 0.2f);
                AudioManager.Instance.Play3DEffect(2004, owner.transform.position);
                itemLocalObj_Food.PlayParticle();
                owner.bodyController.SetAnimatorFunc(BodyPart.Head, (string str) =>
                {
                    if (str.Equals("Eat"))
                    {
                        bodyController_Human.transform_ItemInRightHand.DOKill();
                        bodyController_Human.transform_ItemInRightHand.localScale = Vector3.one;
                        bodyController_Human.transform_ItemInRightHand.DOPunchScale(new Vector3(0.2f, -0.2f, 1), 0.2f);
                        AudioManager.Instance.Play3DEffect(2005, owner.transform.position);
                        itemLocalObj_Food.StopParticle();
                        if (input)
                        {
                            Eat();
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            else
            {
                owner.actionManager.AllClient_SendText("吃不下了", (int)Emoji.Yell);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return base.OnHand_UpdateLeftPress(pressTimer, state, input, player);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Food.ReleaseLeftMouse();
        inputData.leftPressTimer = 0;
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    /// <summary>
    /// 是否可食用
    /// </summary>
    /// <returns></returns>
    public virtual bool Check()
    {
        return true;
    }
    /// <summary>
    /// 食用
    /// </summary>
    public virtual void Eat()
    {
        if (itemData.D <= 0)
        {
            Posion();
        }
        Expend(1);
    }
    /// <summary>
    /// 中毒
    /// </summary>
    public virtual void Posion()
    {

    }
    /// <summary>
    /// 消耗
    /// </summary>
    /// <param name="val"></param>
    public virtual void Expend(int val)
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
    public override void InBag_Use()
    {
        base.InBag_Use();
    }
}
/// <summary>
/// -----基本药剂-----
/// </summary>
public class ItemBase_Potion : ItemBase
{
    #region//持有
    private ItemLocalObj_Food itemLocalObj_Food;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Food = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_Food").GetComponent<ItemLocalObj_Food>();
        itemLocalObj_Food.InitData(itemData);
        itemLocalObj_Food.HoldingStart(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        itemLocalObj_Food.PressLeftMouse(pressTimer, owner.actorAuthority);
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.bodyController.SetAnimatorTrigger(BodyPart.Hand, "Eat");
                owner.bodyController.SetAnimatorTrigger(BodyPart.Head, "Eat");
                BodyController_Human bodyController_Human = (BodyController_Human)owner.bodyController;
                bodyController_Human.transform_ItemInRightHand.DOKill();
                bodyController_Human.transform_ItemInRightHand.localScale = Vector3.one;
                bodyController_Human.transform_ItemInRightHand.DOPunchScale(new Vector3(-0.2f, 0.2f, 1), 0.2f);
                AudioManager.Instance.Play3DEffect(2004, owner.transform.position);
                itemLocalObj_Food.PlayParticle();
                owner.bodyController.SetAnimatorFunc(BodyPart.Head, (string str) =>
                {
                    if (str.Equals("Eat"))
                    {
                        bodyController_Human.transform_ItemInRightHand.DOKill();
                        bodyController_Human.transform_ItemInRightHand.localScale = Vector3.one;
                        bodyController_Human.transform_ItemInRightHand.DOPunchScale(new Vector3(0.2f, -0.2f, 1), 0.2f);
                        AudioManager.Instance.Play3DEffect(2005, owner.transform.position);
                        itemLocalObj_Food.StopParticle();
                        if (input)
                        {
                            Eat();
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
        inputData.leftPressTimer = pressTimer;
        return base.OnHand_UpdateLeftPress(pressTimer, state, input, player);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Food.ReleaseLeftMouse();
        inputData.leftPressTimer = 0;
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public virtual void Eat()
    {
        Expend(1);
    }
    public virtual void Expend(int val)
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
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + itemConfig.Item_ID).Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);

        string stringInfo = stringName + "(" + stringQuality + ")" + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.I.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, itemData.D.ToString() + "%");
        gridCell.SetCell(stringInfo);
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
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + itemConfig.Item_ID).Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);

        string stringInfo = stringName + "(" + stringQuality + ")" + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.I.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, itemData.D.ToString() + "%");
        gridCell.SetCell(stringInfo);
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
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + itemConfig.Item_ID).Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);

        string stringInfo = stringName + "(" + stringQuality + ")" + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.I.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity,"");
        gridCell.SetCell(stringInfo);
    }
    public override void GridCell_RightClick(UI_GridCell gridCell, ItemData itemData)
    {
        
        base.GridCell_RightClick(gridCell, itemData);
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
/// 物品_服装
/// </summary>
public class ItemBase_Clothes : ItemBase
{
    public override void InBag_Use()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBody_Switch()
        {
            index = itemPath.itemIndex
        });
    }
}
/// <summary>
/// 物品_帽子
/// </summary>
public class ItemBase_Hat : ItemBase
{
    public override void InBag_Use()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHead_Switch()
        {
            index = itemPath.itemIndex
        });
    }
}
/// <summary>
/// 物品_饰品
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
/// 物品_耗材
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
    /// SignTime
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
    /// <summary>
    /// 野外
    /// </summary>
    OutSide,
    /// <summary>
    /// 手部
    /// </summary>
    Hand,
    /// <summary>
    /// 身体
    /// </summary>
    Body,
    /// <summary>
    /// 头部
    /// </summary>
    Head,
    /// <summary>
    /// 饰品
    /// </summary>
    Accessory,
    /// <summary>
    /// 耗材
    /// </summary>
    Consumables,
    /// <summary>
    /// 背包
    /// </summary>
    Bag,
}
public class InputData
{
    public float rightPressTimer = 0;
    public float leftPressTimer = 0;
    public Vector3 mousePosition = Vector3.zero;
}
