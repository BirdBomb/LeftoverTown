using System;
using UnityEngine;

public class ItemSystem2000 
{
    
}
#region//纯工具

/// <summary>
/// 火把
/// </summary>
public class Item_2000 : ItemBase_Tool
{
    #region//基础数值
    private readonly float LightRange_Base = 5;
    private float LightRange_Add;
    private readonly float ExpendSpeed_Base = 0.2f;
    private float ExpendSpeed_Sub;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/LightRange/", (LightRange_Base + LightRange_Add).ToString());
        desc = desc.Replace("/ExpendSpeed/",(ExpendSpeed_Base - ExpendSpeed_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray: 
                {
                    LightRange_Add = 0;
                    ExpendSpeed_Sub = 0;
                    break; 
                } 
            case ItemQuality.Green: 
                { 
                    ExpendSpeed_Sub = (float)Math.Round(ExpendSpeed_Base * 0.3f, 2);
                    LightRange_Add = 0;
                    break;
                }
            case ItemQuality.Blue: 
                { 
                    ExpendSpeed_Sub = (float)Math.Round(ExpendSpeed_Base * 0.3f, 2); 
                    LightRange_Add = 1;
                    break;
                } 
            case ItemQuality.Purple: 
                { 
                    ExpendSpeed_Sub = (float)Math.Round(ExpendSpeed_Base * 0.6f, 2); 
                    LightRange_Add = 1;
                    break;
                } 
            case ItemQuality.Gold: 
                { 
                    ExpendSpeed_Sub = (float)Math.Round(ExpendSpeed_Base * 0.6f, 2); 
                    LightRange_Add = 2;
                    break;
                } 
            case ItemQuality.Red: 
                { 
                    ExpendSpeed_Sub = (float)Math.Round(ExpendSpeed_Base * 0.9f, 2); 
                    LightRange_Add = 2;
                    break;
                } 
            case ItemQuality.Rainbow: 
                { 
                    ExpendSpeed_Sub = ExpendSpeed_Base; 
                    LightRange_Add = 2;
                    break;
                } 
        }
        itemLocalObj_Torch?.UpdateTorchData(LightRange_Base + LightRange_Add, ExpendSpeed_Base - ExpendSpeed_Sub, itemQuality);
    }
    #endregion
    #region//持有
    private ItemLocalObj_Torch itemLocalObj_Torch;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Torch = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2000").GetComponent<ItemLocalObj_Torch>();
        itemLocalObj_Torch.InitData(itemData);
        itemLocalObj_Torch.HoldingStart(owner, body);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Torch.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Torch?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateTime(int second)
    {
        itemLocalObj_Torch?.UpdateTime(second);
        base.OnHand_UpdateTime(second);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Torch?.UpdateDataByNet(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Torch?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
/// <summary>
/// 木斧头
/// </summary>
public class Item_2010 : ItemBase_Tool
{
    #region//基础数值
    private readonly int SlashingDamage_Base = 30;
    private int SlashingDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1.5f;
    private float AttackSpeed_Add = 0;
    private readonly float AttackRange_Base = 60;
    private float AttackRange_Add = 0;
    private readonly float AttackDistance_Base = 1;
    private float AttackDistance_Add = 0;
    private readonly float Expend_Base = 0.5f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/SlashingDamage/", ((SlashingDamage_Base + SlashingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackRange/", (AttackRange_Base).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray: 
                {
                    SlashingDamage_Add = 0;
                    AttackSpeed_Add = 0;
                    Expend_Sub = 0;
                    break; 
                } 
            case ItemQuality.Green:
                {
                    SlashingDamage_Add = 0;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.1f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    SlashingDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    SlashingDamage_Add = 5;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    SlashingDamage_Add = 10;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    SlashingDamage_Add = 10;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    SlashingDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Axe?.UpdateAexData(SlashingDamage_Base + SlashingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2010").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdateAexData(SlashingDamage_Base + SlashingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Axe?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Axe?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 铁斧头
/// </summary>
public class Item_2011 : ItemBase_Tool
{
    #region//基础数值
    private readonly int SlashingDamage_Base = 50;
    private int SlashingDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1.5f;
    private float AttackSpeed_Add = 0;
    private readonly float AttackDistance_Base = 1;
    private float AttackDistance_Add = 0;
    private readonly float AttackRange_Base = 60;
    private float AttackRange_Add = 0;
    private readonly float Expend_Base = 0.5f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/SlashingDamage/", ((SlashingDamage_Base + SlashingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackRange/", (AttackRange_Base + AttackRange_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray: 
                {
                    SlashingDamage_Add = 0;
                    AttackSpeed_Add = 0;
                    Expend_Sub = 0;
                    break; 
                } 
            case ItemQuality.Green:
                {
                    SlashingDamage_Add = 0;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.1f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    SlashingDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    SlashingDamage_Add = 5;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    SlashingDamage_Add = 10;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    SlashingDamage_Add = 10;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    SlashingDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Axe?.UpdateAexData(SlashingDamage_Base + SlashingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2011").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdateAexData(SlashingDamage_Base + SlashingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality); 
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Axe?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Axe?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 伐木斧头
/// </summary>
public class Item_2012 : ItemBase_Tool
{
    #region//基础数值
    private readonly int SlashingDamage_Base = 70;
    private int SlashingDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1.5f;
    private float AttackSpeed_Add = 0;
    private readonly float AttackDistance_Base = 1;
    private float AttackDistance_Add = 0;
    private readonly float AttackRange_Base = 60;
    private float AttackRange_Add = 0;
    private readonly float Expend_Base = 0.2f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/SlashingDamage/", ((SlashingDamage_Base + SlashingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackRange/", (AttackRange_Base + AttackRange_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        AttackDistance_Add = 0;
        AttackRange_Add = 0;
        switch (itemQuality)
        {
            case ItemQuality.Gray: 
                {
                    SlashingDamage_Add = 0;
                    AttackSpeed_Add = 0;
                    Expend_Sub = 0;
                    break; 
                } 
            case ItemQuality.Green:
                {
                    SlashingDamage_Add = 0;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.1f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    SlashingDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    SlashingDamage_Add = 5;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    SlashingDamage_Add = 10;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    SlashingDamage_Add = 10;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    SlashingDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Axe?.UpdateAexData(SlashingDamage_Base + SlashingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Axe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2012").GetComponent<ItemLocalObj_Axe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdateAexData(SlashingDamage_Base + SlashingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }

    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Axe?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Axe?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 木镐
/// </summary>
public class Item_2020 : ItemBase_Tool
{
    #region//基础数值
    private readonly int BludgeoningDamage_Base = 50;
    private int BludgeoningDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1.5f;
    private float AttackSpeed_Add = 0;
    private readonly float AttackDistance_Base = 1;
    private float AttackDistance_Add = 0;
    private readonly float AttackRange_Base = 60;
    private float AttackRange_Add = 0;
    private readonly float Expend_Base = 0.5f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/BludgeoningDamage/", ((BludgeoningDamage_Base + BludgeoningDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackRange/", (AttackRange_Base + AttackRange_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        BludgeoningDamage_Add = 0;
        AttackSpeed_Add = 0;
        AttackDistance_Add = 0;
        AttackRange_Add = 0;
        Expend_Sub = 0;
        switch (itemQuality)
        {
            case ItemQuality.Gray: { break; } 
            case ItemQuality.Green:
                {
                    BludgeoningDamage_Add = 0;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.1f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    BludgeoningDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    BludgeoningDamage_Add = 5;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    BludgeoningDamage_Add = 10;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    BludgeoningDamage_Add = 10;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    BludgeoningDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Axe?.UpdatePickaxeData(BludgeoningDamage_Base + BludgeoningDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Pickaxe itemLocalObj_Axe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Axe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2020").GetComponent<ItemLocalObj_Pickaxe>();
        itemLocalObj_Axe.InitData(itemData);
        itemLocalObj_Axe.HoldingStart(owner, body);
        itemLocalObj_Axe.UpdatePickaxeData(BludgeoningDamage_Base + BludgeoningDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Axe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Axe?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Axe?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Axe?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Axe?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 铁镐
/// </summary>
public class Item_2021 : ItemBase_Tool
{
    #region//基础数值
    private readonly int BludgeoningDamage_Base = 70;
    private int BludgeoningDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1.5f;
    private float AttackSpeed_Add = 0;
    private readonly float AttackDistance_Base = 1;
    private float AttackDistance_Add = 0;
    private readonly float AttackRange_Base = 60;
    private float AttackRange_Add = 0;
    private readonly float Expend_Base = 0.5f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/BludgeoningDamage/", ((BludgeoningDamage_Base + BludgeoningDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackRange/", (AttackRange_Base + AttackRange_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        BludgeoningDamage_Add = 0;
        AttackSpeed_Add = 0;
        AttackDistance_Add = 0;
        AttackRange_Add = 0;
        Expend_Sub = 0;

        switch (itemQuality)
        {
            case ItemQuality.Gray: { break; } 
            case ItemQuality.Green:
                {
                    BludgeoningDamage_Add = 0;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.1f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    BludgeoningDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    BludgeoningDamage_Add = 5;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    BludgeoningDamage_Add = 10;
                    AttackSpeed_Add = 0.5f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    BludgeoningDamage_Add = 10;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    BludgeoningDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Pickaxe?.UpdatePickaxeData(BludgeoningDamage_Base + BludgeoningDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Pickaxe itemLocalObj_Pickaxe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Pickaxe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2021").GetComponent<ItemLocalObj_Pickaxe>();
        itemLocalObj_Pickaxe.InitData(itemData);
        itemLocalObj_Pickaxe.HoldingStart(owner, body);
        itemLocalObj_Pickaxe.UpdatePickaxeData(BludgeoningDamage_Base + BludgeoningDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pickaxe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pickaxe.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Pickaxe.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Pickaxe) itemLocalObj_Pickaxe.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Pickaxe) itemLocalObj_Pickaxe.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 木头鱼竿
/// </summary>
public class Item_2030 : ItemBase
{
    #region//数据初始化
    public override void StaticAction_InitData(short id, out ItemData data)
    {
        base.StaticAction_InitData(id, out data);
        data.V = (short)new System.Random().Next(short.MinValue, short.MaxValue);
    }
    #endregion
    #region//基础数值
    private readonly float FishPower_Base = 2;
    private float FishPower_Add = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/FishPower/", (FishPower_Base + FishPower_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    FishPower_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    FishPower_Add = 0.2f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    FishPower_Add = 0.5f;
                    break;
                }
            case ItemQuality.Purple:
                {
                    FishPower_Add = 1f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    FishPower_Add = 1.5f;
                    break;
                }
            case ItemQuality.Red:
                {
                    FishPower_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    FishPower_Add = 10f;
                    break;
                }
        }
        itemLocalObj_FishRod?.UpdateFishRodData(FishPower_Base + FishPower_Add, itemQuality);
    }
    #endregion
    #region//修改UI
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + itemConfig.Item_ID).Split('_');
        string stringName = parts.Length > 0 ? parts[0] : "Error";
        string stringDesc = GridCell_UpdateDesc(parts.Length > 1 ? parts[1] : "Error");
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);
        gridCell.DrawCell($"Item_{itemData.I}", $"ItemBG_{(int)itemConfig.Item_Rarity}", itemData.C.ToString());
        gridCell.SetCell($"{stringName}({stringQuality})\n{stringDesc}");
    }
    #endregion
    #region//使用逻辑 
    private ItemLocalObj_FishRod itemLocalObj_FishRod;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_FishRod = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2030").GetComponent<ItemLocalObj_FishRod>();
        itemLocalObj_FishRod.InitData(itemData);
        itemLocalObj_FishRod.HoldingStart(owner, body);
        itemLocalObj_FishRod.UpdateFishRodData(FishPower_Base + FishPower_Add, itemQuality);
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        itemLocalObj_FishRod?.PressRightMouse(pressTimer, owner.actorAuthority);
        return true;
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_FishRod?.ReleaseRightMouse();
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        itemLocalObj_FishRod?.PressLeftMouse(pressTimer, owner.actorAuthority);
        return base.OnHand_UpdateLeftPress(pressTimer, state, input, player);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_FishRod?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_FishRod?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    #endregion
}
/// <summary>
/// 木锄
/// </summary>
public class Item_2040 : ItemBase_Tool
{
    #region//基础数值
    private readonly float Expend_Base = 2f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Expend_Sub = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.1f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.8f, 2);
                    break;
                }
        }
        itemLocalObj_Hoe?.UpdateHoeData(Expend_Base - Expend_Sub, itemQuality);
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Hoe itemLocalObj_Hoe;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Hoe = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2040").GetComponent<ItemLocalObj_Hoe>();
        itemLocalObj_Hoe.InitData(itemData);
        itemLocalObj_Hoe.HoldingStart(owner, body);
        itemLocalObj_Hoe.UpdateHoeData(Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Hoe.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Hoe?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Hoe?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Hoe?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Hoe?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 木镰刀
/// </summary>
public class Item_2050 : ItemBase_Tool
{
    #region//基础数值
    private readonly int AttackDamage_Base = 50;
    private readonly float AttackSpeed_Base = 2f;
    private float AttackSpeed_Add = 0;
    private readonly float Expend_Base = 2f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    Expend_Sub = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.1f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Sickle?.UpdateSickleData(AttackDamage_Base, AttackSpeed_Base + AttackSpeed_Add, Expend_Base - Expend_Sub, itemQuality);
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Sickle itemLocalObj_Sickle;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Sickle = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2050").GetComponent<ItemLocalObj_Sickle>();
        itemLocalObj_Sickle.InitData(itemData);
        itemLocalObj_Sickle.HoldingStart(owner, body);
        itemLocalObj_Sickle.UpdateSickleData(AttackDamage_Base, AttackSpeed_Base + AttackSpeed_Add, Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Sickle.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Sickle?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Sickle?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Sickle?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Sickle?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 木锤
/// </summary>
public class Item_2060 : ItemBase_Tool
{
    #region//基础数值
    private readonly int StructureDamage_Base = 100;
    private readonly int BludgeoningDamage_Base = 100;
    private int BludgeoningDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1f;
    private float AttackSpeed_Add = 0;
    private readonly float AttackDistance_Base = 1;
    private float AttackDistance_Add = 0;
    private readonly float AttackRange_Base = 60;
    private float AttackRange_Add = 0;
    private readonly float Expend_Base = 0.5f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/BludgeoningDamage/", ((BludgeoningDamage_Base + BludgeoningDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackRange/", (AttackRange_Base + AttackRange_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    AttackSpeed_Add = 0f;
                    Expend_Sub = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    AttackSpeed_Add = 0.4f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    AttackSpeed_Add = 0.6f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    AttackSpeed_Add = 1f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.6f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Hammer?.UpdateHammerData(StructureDamage_Base, BludgeoningDamage_Base + BludgeoningDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Hammer itemLocalObj_Hammer;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Hammer = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2060").GetComponent<ItemLocalObj_Hammer>();
        itemLocalObj_Hammer.InitData(itemData);
        itemLocalObj_Hammer.HoldingStart(owner, body);
        itemLocalObj_Hammer.UpdateHammerData(StructureDamage_Base, BludgeoningDamage_Base + BludgeoningDamage_Add, AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add, Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Hammer.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Hammer?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Hammer?.UpdateMousePos(mouse);
        inputData.mousePosition = mouse;
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Hammer?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Hammer?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
#endregion
#region//近战武器
/// <summary>
/// 木棍
/// </summary>
public class Item_2100 : ItemBase_Weapon
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 60;
    private int PiercingDamage_Add = 0;
    private readonly int SlashingDamage_Base = 40;
    private int SlashingDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1f;
    private float AttackSpeed_Add = 0;
    private readonly float AttackDistance_Base = 2;
    private float AttackDistance_Add = 0;
    private readonly float Expend_Base = 0.8f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/SlashingDamage/", ((SlashingDamage_Base + SlashingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    SlashingDamage_Add = 0;
                    PiercingDamage_Add = 0;
                    AttackSpeed_Add = 0f;
                    Expend_Sub = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    SlashingDamage_Add = 5;
                    PiercingDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    SlashingDamage_Add = 10;
                    PiercingDamage_Add = 10;
                    AttackSpeed_Add = 0.4f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    SlashingDamage_Add = 10;
                    PiercingDamage_Add = 10;
                    AttackSpeed_Add = 0.6f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    SlashingDamage_Add = 15;
                    PiercingDamage_Add = 15;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    SlashingDamage_Add = 15;
                    PiercingDamage_Add = 15;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.6f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    SlashingDamage_Add = 20;
                    PiercingDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Spear?.UpdateSpearData(PiercingDamage_Base + PiercingDamage_Add,SlashingDamage_Base + SlashingDamage_Add,AttackSpeed_Base + AttackSpeed_Add, AttackDistance_Base + AttackDistance_Add,Expend_Base - Expend_Sub,itemQuality);
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Spear itemLocalObj_Spear;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Spear = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2100").GetComponent<ItemLocalObj_Spear>();
        itemLocalObj_Spear.InitData(itemData);
        itemLocalObj_Spear.HoldingStart(owner, body);
        itemLocalObj_Spear.UpdateSpearData(
            PiercingDamage_Base + PiercingDamage_Add,
            SlashingDamage_Base + SlashingDamage_Add,
            AttackSpeed_Base + AttackSpeed_Add,
            AttackDistance_Base + AttackDistance_Add,
            Expend_Base - Expend_Sub, 
            itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Spear?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Spear.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Spear?.ReleaseLeftMouse();
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Spear.PressRightMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Spear?.ReleaseRightMouse();
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Spear?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Spear?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 长柄刀
/// </summary>
public class Item_2101 : ItemBase_Weapon
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 80;
    private int PiercingDamage_Add = 0;
    private readonly int SlashingDamage_Base = 60;
    private int SlashingDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1f;
    private float AttackSpeed_Add = 0;
    private readonly float AttackDistance_Base = 2;
    private float AttackDistance_Add = 0;
    private readonly float Expend_Base = 0.8f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/SlashingDamage/", ((SlashingDamage_Base + SlashingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/AttackDistance/", (AttackDistance_Base + AttackDistance_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    SlashingDamage_Add = 0;
                    PiercingDamage_Add = 0;
                    AttackSpeed_Add = 0f;
                    Expend_Sub = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    SlashingDamage_Add = 5;
                    PiercingDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    SlashingDamage_Add = 10;
                    PiercingDamage_Add = 10;
                    AttackSpeed_Add = 0.4f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    SlashingDamage_Add = 10;
                    PiercingDamage_Add = 10;
                    AttackSpeed_Add = 0.6f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    SlashingDamage_Add = 15;
                    PiercingDamage_Add = 15;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    SlashingDamage_Add = 15;
                    PiercingDamage_Add = 15;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.6f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    SlashingDamage_Add = 20;
                    PiercingDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Spear?.UpdateSpearData(PiercingDamage_Base + PiercingDamage_Add,SlashingDamage_Base + SlashingDamage_Add,AttackSpeed_Base + AttackSpeed_Add,AttackDistance_Base + AttackDistance_Add,Expend_Base - Expend_Sub,itemQuality);
    }

    #endregion
    #region//使用逻辑
    private ItemLocalObj_Spear itemLocalObj_Spear;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Spear = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2101").GetComponent<ItemLocalObj_Spear>();
        itemLocalObj_Spear.InitData(itemData);
        itemLocalObj_Spear.HoldingStart(owner, body);
        itemLocalObj_Spear.UpdateSpearData(
            PiercingDamage_Base + PiercingDamage_Add,
            SlashingDamage_Base + SlashingDamage_Add,
            AttackSpeed_Base + AttackSpeed_Add,
            AttackDistance_Base + AttackDistance_Add,
            Expend_Base - Expend_Sub,
            itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Spear?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Spear.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Spear?.ReleaseLeftMouse();
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Spear.PressRightMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Spear?.ReleaseRightMouse();
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Spear?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Spear?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 精钢匕首
/// </summary>
public class Item_2102 : ItemBase_Weapon
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 80;
    private int PiercingDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1f;
    private float AttackSpeed_Add = 0;
    private readonly float Expend_Base = 0.8f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    AttackSpeed_Add = 0f;
                    Expend_Sub = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 10;
                    AttackSpeed_Add = 0.4f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 10;
                    AttackSpeed_Add = 0.6f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 15;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 15;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.6f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Dagger?.UpdateDaggerData(PiercingDamage_Base + PiercingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, Expend_Base - Expend_Sub, itemQuality);
    }
    #endregion

    #region//使用逻辑
    private ItemLocalObj_Dagger itemLocalObj_Dagger;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Dagger = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2102").GetComponent<ItemLocalObj_Dagger>();
        itemLocalObj_Dagger.InitData(itemData);
        itemLocalObj_Dagger.HoldingStart(owner, body);
        itemLocalObj_Dagger.UpdateDaggerData(PiercingDamage_Base + PiercingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Dagger?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Dagger.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Dagger?.ReleaseLeftMouse();
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Dagger.PressRightMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Dagger?.ReleaseRightMouse();
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Dagger?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Dagger?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
/// <summary>
/// 铁剑
/// </summary>
public class Item_2103 : ItemBase_Weapon
{
    #region//基础数值
    private readonly int SlashingDamage_Base = 60;
    private int SlashingDamage_Add = 0;
    private readonly float AttackSpeed_Base = 1.5f;
    private float AttackSpeed_Add = 0;
    private readonly float Expend_Base = 0.5f;
    private float Expend_Sub = 0;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/SlashingDamage/", ((SlashingDamage_Base + SlashingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/AttackSpeed/", (AttackSpeed_Base + AttackSpeed_Add).ToString());
        desc = desc.Replace("/Expend/", (Expend_Base - Expend_Sub).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    SlashingDamage_Add = 0;
                    AttackSpeed_Add = 0f;
                    Expend_Sub = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    SlashingDamage_Add = 5;
                    AttackSpeed_Add = 0.2f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.2f, 2);
                    break;
                }
            case ItemQuality.Blue:
                {
                    SlashingDamage_Add = 10;
                    AttackSpeed_Add = 0.4f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.3f, 2);
                    break;
                }
            case ItemQuality.Purple:
                {
                    SlashingDamage_Add = 10;
                    AttackSpeed_Add = 0.6f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.4f, 2);
                    break;
                }
            case ItemQuality.Gold:
                {
                    SlashingDamage_Add = 15;
                    AttackSpeed_Add = 0.8f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.5f, 2);
                    break;
                }
            case ItemQuality.Red:
                {
                    SlashingDamage_Add = 15;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = (float)Math.Round(Expend_Base * 0.6f, 2);
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    SlashingDamage_Add = 20;
                    AttackSpeed_Add = 1f;
                    Expend_Sub = Expend_Base;
                    break;
                }
        }
        itemLocalObj_Broadsword?.UpdateBroadswordData(SlashingDamage_Base + SlashingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, Expend_Base - Expend_Sub, itemQuality);
    }
    #endregion

    #region//使用逻辑
    private ItemLocalObj_Broadsword itemLocalObj_Broadsword;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Broadsword = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2103").GetComponent<ItemLocalObj_Broadsword>();
        itemLocalObj_Broadsword.InitData(itemData);
        itemLocalObj_Broadsword.HoldingStart(owner, body);
        itemLocalObj_Broadsword.UpdateBroadswordData(SlashingDamage_Base + SlashingDamage_Add, AttackSpeed_Base + AttackSpeed_Add, Expend_Base - Expend_Sub, itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Broadsword?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Broadsword.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Broadsword?.ReleaseLeftMouse();
    }
    public override bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Broadsword.PressRightMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Broadsword?.ReleaseRightMouse();
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Broadsword?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Broadsword?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    #endregion
}
#endregion
#region//远程武器
/// <summary>
/// 粗制木弓
/// </summary>
public class Item_2200 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 60;
    private int ShotSpeed_Add;
    private readonly int ReadySpeed_Base = 1;
    private int ReadySpeed_Add;
    private readonly float AimSpeed_Base = 1;
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 90;
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 20;
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.2f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/ReadySpeed/", (ReadySpeed_Base + ReadySpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", $"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Bow?.UpdateBowData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Bow itemLocalObj_Bow;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2200").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.InitData(itemData);
        itemLocalObj_Bow.HoldingStart(owner, body);
        itemLocalObj_Bow.UpdateBowData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow?.ReleaseRightMouse();
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Bow?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Bow?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Bow?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
/// <summary>
/// 精制木弓
/// </summary>
public class Item_2201 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 90;//射击速度
    private int ShotSpeed_Add;
    private readonly int ReadySpeed_Base = 1;//准备速度
    private int ReadySpeed_Add;
    private readonly float AimSpeed_Base = 2;//瞄准速度
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 60;//最大射击角度
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 10;//最小射击角度
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.2f;//射击角度扩散
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/ReadySpeed/", (ReadySpeed_Base + ReadySpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/",$"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Bow?.UpdateBowData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Bow itemLocalObj_Bow;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2201").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.InitData(itemData);
        itemLocalObj_Bow.HoldingStart(owner, body);
        itemLocalObj_Bow.UpdateBowData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow.ReleaseRightMouse();
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Bow.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        if (itemLocalObj_Bow) itemLocalObj_Bow.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        if (itemLocalObj_Bow) itemLocalObj_Bow.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
/// <summary>
/// 黄金弓
/// </summary>
public class Item_2202 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 120;//射击速度
    private int ShotSpeed_Add;
    private readonly int ReadySpeed_Base = 2;//准备速度
    private int ReadySpeed_Add;
    private readonly float AimSpeed_Base = 5;//瞄准速度
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 30;//最大射击角度
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 5;//最小射击角度
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.1f;//射击角度扩散
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/ReadySpeed/", (ReadySpeed_Base + ReadySpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", $"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Bow?.UpdateBowData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Bow itemLocalObj_Bow;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2202").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.InitData(itemData);
        itemLocalObj_Bow.HoldingStart(owner, body);
        itemLocalObj_Bow.UpdateBowData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                ReadySpeed_Base + ReadySpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Bow.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Bow?.ReleaseRightMouse();
        base.OnHand_ReleaseRightPress(state, input, player);
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Bow?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Bow?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Bow?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
#endregion
#region//热武器
/// <summary>
/// 土质手枪
/// </summary>
public class Item_2300 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 60;//射击速度
    private int ShotSpeed_Add;
    private readonly float AimSpeed_Base = 2;//瞄准速度
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 120;//最大射击角度
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 45;//最小射击角度
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.5f;//射击角度扩散
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/",$"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Pistol?.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Pistol itemLocalObj_Pistol;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Pistol = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2300").GetComponent<ItemLocalObj_Pistol>();
        itemLocalObj_Pistol.InitData(itemData);
        itemLocalObj_Pistol.HoldingStart(owner, body);
        itemLocalObj_Pistol.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Pistol?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Pistol?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Pistol?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 短型冲锋枪
/// </summary>
public class Item_2301 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 600;
    private int ShotSpeed_Add;
    private readonly float AimSpeed_Base = 5;
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 90;
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 45;
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.1f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", $"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Pistol?.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Pistol itemLocalObj_Pistol;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Pistol = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2301").GetComponent<ItemLocalObj_Pistol>();
        itemLocalObj_Pistol.InitData(itemData);
        itemLocalObj_Pistol.HoldingStart(owner, body);
        itemLocalObj_Pistol.UpdateGunData(
            PiercingDamage_Base + PiercingDamage_Add,
            MagicDamage_Base + MagicDamage_Add,
            ShotSpeed_Base + ShotSpeed_Add,
            AimSpeed_Base + AimSpeed_Add,
            AimRangeMin_Base + AimRangeMin_Add,
            AimRangeMax_Base + AimRangeMax_Add,
            Recoil_Base + Recoil_Add,
            itemQuality);

        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Pistol?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Pistol?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Pistol?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 制式自动步枪
/// </summary>
public class Item_2302 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 300;
    private int ShotSpeed_Add;
    private readonly float AimSpeed_Base = 4;
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 45;
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 10;
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.25f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", $"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Rifle?.UpdateGunData(
                 PiercingDamage_Base + PiercingDamage_Add,
                 MagicDamage_Base + MagicDamage_Add,
                 ShotSpeed_Base + ShotSpeed_Add,
                 AimSpeed_Base + AimSpeed_Add,
                 AimRangeMin_Base + AimRangeMin_Add,
                 AimRangeMax_Base + AimRangeMax_Add,
                 Recoil_Base + Recoil_Add,
                 itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Rifle itemLocalObj_Rifle;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Rifle = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2302").GetComponent<ItemLocalObj_Rifle>();
        itemLocalObj_Rifle.InitData(itemData);
        itemLocalObj_Rifle.HoldingStart(owner, body);
        itemLocalObj_Rifle.UpdateGunData(
            PiercingDamage_Base + PiercingDamage_Add,
            MagicDamage_Base + MagicDamage_Add,
            ShotSpeed_Base + ShotSpeed_Add,
            AimSpeed_Base + AimSpeed_Add,
            AimRangeMin_Base + AimRangeMin_Add,
            AimRangeMax_Base + AimRangeMax_Add,
            Recoil_Base + Recoil_Add,
            itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Rifle?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Rifle?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Rifle?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 制式手枪
/// </summary>
public class Item_2303 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 120;
    private int ShotSpeed_Add;
    private readonly float AimSpeed_Base = 6;
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 45;
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 20;
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.5f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", $"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Pistol?.UpdateGunData(
               PiercingDamage_Base + PiercingDamage_Add,
               MagicDamage_Base + MagicDamage_Add,
               ShotSpeed_Base + ShotSpeed_Add,
               AimSpeed_Base + AimSpeed_Add,
               AimRangeMin_Base + AimRangeMin_Add,
               AimRangeMax_Base + AimRangeMax_Add,
               Recoil_Base + Recoil_Add,
               itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Pistol itemLocalObj_Pistol;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Pistol = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2303").GetComponent<ItemLocalObj_Pistol>();
        itemLocalObj_Pistol.InitData(itemData);
        itemLocalObj_Pistol.HoldingStart(owner, body);
        itemLocalObj_Pistol.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Pistol.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Pistol?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Pistol?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Pistol?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Pistol?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 泵动霰弹枪
/// </summary>
public class Item_2304 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 90;
    private int ShotSpeed_Add;
    private readonly float AimSpeed_Base = 6;
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 60;
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 30;
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.2f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", $"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_ScatterGun?.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_ScatterGun itemLocalObj_ScatterGun;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_ScatterGun = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2304").GetComponent<ItemLocalObj_ScatterGun>();
        itemLocalObj_ScatterGun.InitData(itemData);
        itemLocalObj_ScatterGun.HoldingStart(owner, body);
        itemLocalObj_ScatterGun.UpdateGunData(
            PiercingDamage_Base + PiercingDamage_Add,
            MagicDamage_Base + MagicDamage_Add,
            ShotSpeed_Base + ShotSpeed_Add,
            AimSpeed_Base + AimSpeed_Add,
            AimRangeMin_Base + AimRangeMin_Add,
            AimRangeMax_Base + AimRangeMax_Add,
            Recoil_Base + Recoil_Add,
            itemQuality);

        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_ScatterGun.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_ScatterGun.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_ScatterGun?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_ScatterGun?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_ScatterGun?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_ScatterGun?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_ScatterGun?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion
}
/// <summary>
/// 麦德森机枪
/// </summary>
public class Item_2305 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 480;
    private int ShotSpeed_Add;
    private readonly float AimSpeed_Base = 3;
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 60;
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 30;
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.1f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", $"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Rifle?.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Rifle itemLocalObj_Rifle;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Rifle = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2305").GetComponent<ItemLocalObj_Rifle>();
        itemLocalObj_Rifle.InitData(itemData);
        itemLocalObj_Rifle.HoldingStart(owner, body);
        itemLocalObj_Rifle.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Rifle?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Rifle?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Rifle?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
/// <summary>
/// 精准自动步枪
/// </summary>
public class Item_2306 : ItemBase_Gun
{
    #region//基础数值
    private readonly int PiercingDamage_Base = 0;
    private int PiercingDamage_Add;
    private readonly int MagicDamage_Base = 0;
    private int MagicDamage_Add;
    private readonly int ShotSpeed_Base = 420;
    private int ShotSpeed_Add;
    private readonly float AimSpeed_Base = 4;
    private float AimSpeed_Add;
    private readonly int AimRangeMax_Base = 45;
    private int AimRangeMax_Add;
    private readonly int AimRangeMin_Base = 10;
    private int AimRangeMin_Add;
    private readonly float Recoil_Base = 0.05f;
    private float Recoil_Add;
    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/PiercingDamage/", ((PiercingDamage_Base + PiercingDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/ShotSpeed/", (ShotSpeed_Base + ShotSpeed_Add).ToString());
        desc = desc.Replace("/AimSpeed/", (AimSpeed_Base + AimSpeed_Add).ToString());
        desc = desc.Replace("/AimRange/", $"{(AimRangeMin_Base + AimRangeMin_Add)}-{(AimRangeMax_Base + AimRangeMax_Add)}");
        desc = desc.Replace("/Recoil/", (Recoil_Base + Recoil_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 0;
                    AimSpeed_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 0.5f;
                    break;
                }
            case ItemQuality.Blue:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 5;
                    AimSpeed_Add = 1;
                    break;
                }
            case ItemQuality.Purple:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 1.5f;
                    break;
                }
            case ItemQuality.Gold:
                {
                    PiercingDamage_Add = 0;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Red:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 0;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    PiercingDamage_Add = 50;
                    MagicDamage_Add = 50;
                    ShotSpeed_Add = 10;
                    AimSpeed_Add = 2f;
                    break;
                }
        }
        itemLocalObj_Rifle?.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Rifle itemLocalObj_Rifle;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Rifle = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2306").GetComponent<ItemLocalObj_Rifle>();
        itemLocalObj_Rifle.InitData(itemData);
        itemLocalObj_Rifle.HoldingStart(owner, body);
        itemLocalObj_Rifle.UpdateGunData(
                PiercingDamage_Base + PiercingDamage_Add,
                MagicDamage_Base + MagicDamage_Add,
                ShotSpeed_Base + ShotSpeed_Add,
                AimSpeed_Base + AimSpeed_Add,
                AimRangeMin_Base + AimRangeMin_Add,
                AimRangeMax_Base + AimRangeMax_Add,
                Recoil_Base + Recoil_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Rifle.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Rifle?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Rifle?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Rifle?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Rifle?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
#endregion
#region//魔法武器
//煤粉喷射器
public class Item_2400 : ItemBase_Gun
{
    #region//基础数值
    private readonly int MagicDamage_Base = 15;
    private int MagicDamage_Add;
    private readonly float BulletSpeed_Base = 1;
    private float BulletSpeed_Add;
    private readonly float BulletDuration_Base = 1;
    private float BulletDuration_Add;
    private readonly float JetFuelPer_Base = 0.2f;
    private float JetFuelPer_Sub;
    private readonly float JetInterval_Base = 0.2f;
    private float JetInterval_Add;

    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/BulletSpeed/", (BulletSpeed_Base + BulletSpeed_Add).ToString());
        desc = desc.Replace("/BulletDuration/", (BulletDuration_Base + BulletDuration_Add).ToString());
        desc = desc.Replace("/JetFuelPer/", (JetFuelPer_Base - JetFuelPer_Sub).ToString());
        desc = desc.Replace("/JetInterval/", (JetInterval_Base + JetInterval_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    MagicDamage_Add = 0;
                    BulletSpeed_Add = 0;
                    JetFuelPer_Sub = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    MagicDamage_Add = 0;
                    BulletSpeed_Add = 0;
                    JetFuelPer_Sub = 0;
                    break;
                }
            case ItemQuality.Blue:
                {
                    MagicDamage_Add = 0;
                    BulletSpeed_Add = 0;
                    JetFuelPer_Sub = 0;
                    break;
                }
            case ItemQuality.Purple:
                {
                    MagicDamage_Add = 0;
                    BulletSpeed_Add = 0;
                    JetFuelPer_Sub = 0;
                    break;
                }
            case ItemQuality.Gold:
                {
                    MagicDamage_Add = 0;
                    BulletSpeed_Add = 0;
                    JetFuelPer_Sub = 0;
                    break;
                }
            case ItemQuality.Red:
                {
                    MagicDamage_Add = 0;
                    BulletSpeed_Add = 0;
                    JetFuelPer_Sub = 0;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    MagicDamage_Add = 0;
                    BulletSpeed_Add = 0;
                    JetFuelPer_Sub = 0;
                    break;
                }
        }
        itemLocalObj_JetByFuel?.UpdateJetData(
                MagicDamage_Base + MagicDamage_Add,
                BulletSpeed_Base + BulletSpeed_Add,
                BulletDuration_Base + BulletDuration_Add,
                JetFuelPer_Base + JetFuelPer_Sub,
                JetInterval_Base + JetInterval_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Jet_ByFuel itemLocalObj_JetByFuel;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_JetByFuel = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2400").GetComponent<ItemLocalObj_Jet_ByFuel>();
        itemLocalObj_JetByFuel.InitData(itemData);
        itemLocalObj_JetByFuel.HoldingStart(owner, body);
        itemLocalObj_JetByFuel.UpdateJetData(
                MagicDamage_Base + MagicDamage_Add,
                BulletSpeed_Base + BulletSpeed_Add,
                BulletDuration_Base + BulletDuration_Add,
                JetFuelPer_Base + JetFuelPer_Sub,
                JetInterval_Base + JetInterval_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_JetByFuel.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_JetByFuel.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_JetByFuel?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_JetByFuel?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_JetByFuel?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_JetByFuel?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_JetByFuel?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
//油喷射器
public class Item_2401 : ItemBase_Gun
{
    #region//基础数值
    private readonly int MagicDamage_Base = 1;
    private int MagicDamage_Add;
    private readonly float JetDistance_Base = 2;
    private float JetDistance_Add;
    private readonly float JetTime_Base = 10;
    private float JetTime_Add;
    private readonly float JetInterval_Base = 0.2f;
    private float JetInterval_Add;

    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/JetDistance/", (JetDistance_Base + JetDistance_Add).ToString());
        desc = desc.Replace("/JetTime/", (JetTime_Base + JetTime_Add).ToString());
        desc = desc.Replace("/JetInterval/", (JetInterval_Base + JetInterval_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Blue:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Purple:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Gold:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Red:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
        }
        itemLocalObj_Jet?.UpdateJetData(
                MagicDamage_Base + MagicDamage_Add,
                JetDistance_Base + JetDistance_Add,
                JetTime_Base + JetTime_Add,
                JetInterval_Base + JetInterval_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Jet itemLocalObj_Jet;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Jet = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2401").GetComponent<ItemLocalObj_Jet>();
        itemLocalObj_Jet.InitData(itemData);
        itemLocalObj_Jet.HoldingStart(owner, body);
        itemLocalObj_Jet.UpdateJetData(
                MagicDamage_Base + MagicDamage_Add,
                JetDistance_Base + JetDistance_Add,
                JetTime_Base + JetTime_Add,
                JetInterval_Base + JetInterval_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Jet.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Jet.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Jet?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Jet?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Jet?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Jet?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Jet?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
//
public class Item_2402 : ItemBase_Gun
{
    #region//基础数值
    private readonly int MagicDamage_Base = 1;
    private int MagicDamage_Add;
    private readonly float JetDistance_Base = 2;
    private float JetDistance_Add;
    private readonly float JetTime_Base = 10;
    private float JetTime_Add;
    private readonly float JetInterval_Base = 0.2f;
    private float JetInterval_Add;

    #endregion
    #region//修改描述
    public override string GridCell_UpdateDesc(string desc)
    {
        desc = desc.Replace("/MagicDamage/", ((MagicDamage_Base + MagicDamage_Add) * 0.1f).ToString());
        desc = desc.Replace("/JetDistance/", (JetDistance_Base + JetDistance_Add).ToString());
        desc = desc.Replace("/JetTime/", (JetTime_Base + JetTime_Add).ToString());
        desc = desc.Replace("/JetInterval/", (JetInterval_Base + JetInterval_Add).ToString());
        return base.GridCell_UpdateDesc(desc);
    }
    #endregion
    #region//修改品质
    public override void CalculateQuality()
    {
        base.CalculateQuality();
        switch (itemQuality)
        {
            case ItemQuality.Gray:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Green:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Blue:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Purple:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Gold:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Red:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
            case ItemQuality.Rainbow:
                {
                    MagicDamage_Add = 0;
                    JetDistance_Add = 0;
                    JetTime_Add = 0;
                    break;
                }
        }
        itemLocalObj_Jet?.UpdateJetData(
                MagicDamage_Base + MagicDamage_Add,
                JetDistance_Base + JetDistance_Add,
                JetTime_Base + JetTime_Add,
                JetInterval_Base + JetInterval_Add,
                itemQuality);
    }
    #endregion
    #region//使用逻辑
    private ItemLocalObj_Jet itemLocalObj_Jet;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Jet = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2402").GetComponent<ItemLocalObj_Jet>();
        itemLocalObj_Jet.InitData(itemData);
        itemLocalObj_Jet.HoldingStart(owner, body);
        itemLocalObj_Jet.UpdateJetData(
                MagicDamage_Base + MagicDamage_Add,
                JetDistance_Base + JetDistance_Add,
                JetTime_Base + JetTime_Add,
                JetInterval_Base + JetInterval_Add,
                itemQuality);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Jet.PressLeftMouse(timer, owner.actorAuthority);
    }
    public override bool OnHand_UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        return itemLocalObj_Jet.PressRightMouse(timer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Jet?.ReleaseLeftMouse();
    }
    public override void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {
        itemLocalObj_Jet?.ReleaseRightMouse();
    }
    public override void OnHand_UpdateMousePos(Vector3 mouse)
    {
        itemLocalObj_Jet?.UpdateMousePos(mouse);
        base.OnHand_UpdateMousePos(mouse);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Jet?.UpdateDataByLocal(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Jet?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }

    #endregion
}
#endregion
#region//其他工具
/// <summary>
/// 套索
/// </summary>
public class Item_2900 : ItemBase_Tool
{
    #region//持有
    private ItemLocalObj_Lasso itemLocalObj_Lasso;
    public override void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        itemLocalObj_Lasso = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2900").GetComponent<ItemLocalObj_Lasso>();
        itemLocalObj_Lasso.InitData(itemData);
        itemLocalObj_Lasso.HoldingStart(owner, body);
        base.OnHand_Start(owner, body);
    }
    public override bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return itemLocalObj_Lasso.PressLeftMouse(pressTimer, owner.actorAuthority);
    }
    public override void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {
        itemLocalObj_Lasso?.ReleaseLeftMouse();
        base.OnHand_ReleaseLeftPress(state, input, player);
    }
    public override void OnHand_UpdateTime(int second)
    {
        itemLocalObj_Lasso?.UpdateTime(second);
        base.OnHand_UpdateTime(second);
    }
    public override void UpdateDataFromLocal(ItemData data)
    {
        itemLocalObj_Lasso?.UpdateDataByNet(data);
        base.UpdateDataFromLocal(data);
    }
    public override void UpdateDataFromNet(ItemData data)
    {
        itemLocalObj_Lasso?.UpdateDataByNet(data);
        base.UpdateDataFromNet(data);
    }
    #endregion

}
#endregion
