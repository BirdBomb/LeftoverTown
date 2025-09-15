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
/// ����
/// </summary>
public class ItemBase
{
    public ActorManager owner;
    #region//�������
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public ItemData itemData;
    /// <summary>
    /// ��ƷƷ��
    /// </summary>
    public ItemQuality itemQuality;
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    public ItemConfig itemConfig;
    /// <summary>
    /// ��Ʒ��ַ
    /// </summary>
    public ItemPath itemPath;
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public virtual void BindPath(ItemPath path)
    {
        itemPath = path;
    }
    /// <summary>
    /// ��������(�������)
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateDataFromNet(ItemData data)
    {
        itemData = data;
        itemConfig = ItemConfigData.GetItemConfig(data.Item_ID);
        CalculateQuality();
    }
    /// <summary>
    /// ��������(����ģ��)
    /// </summary>
    /// <param name="data"></param>
    public virtual void UpdateDataFromLocal(ItemData data)
    {
        itemData = data;
        itemConfig = ItemConfigData.GetItemConfig(data.Item_ID);
        CalculateQuality();
    }
    /// <summary>
    /// ������ƷƷ��
    /// </summary>
    /// <returns></returns>
    public virtual void CalculateQuality()
    {
        UnityEngine.Random.InitState(itemData.Item_Info);
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
    #region//UI���
    /// <summary>
    /// ���Ƹ���
    /// </summary>
    public virtual void GridCell_Draw(UI_GridCell gridCell)
    {
        string stringName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
        string stringDesc = GridCell_UpdateDesc(LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc"));
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        string stringInfo = stringName + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, itemData.Item_Count.ToString());
        gridCell.SetCell(stringInfo);
    }
    /// <summary>
    /// �޸�����
    /// </summary>
    /// <param name="desc"></param>
    /// <returns></returns>
    public virtual string GridCell_UpdateDesc(string desc)
    {
        return desc;
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public virtual void GridCell_LeftClick(UI_GridCell gridCell, ItemData itemData)
    {
        
    }
    /// <summary>
    /// �һ�����
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
    /// ���Ӽ�����
    /// </summary>
    /// <returns></returns>
    public virtual bool GridCell_OpenChildCrid()
    {
        return false;
    }
    /// <summary>
    /// ������������
    /// </summary>
    /// <param name="itemNetObj"></param>
    /// <param name="data"></param>
    public virtual void NetObj_Draw(ItemNetObj itemNetObj, ItemData data)
    {
        itemNetObj.spriteRenderer_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
        itemNetObj.textMesh_Count.text = data.Item_Count.ToString();
    }
    /// <summary>
    /// ���ŵ��䶯��
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
    #region//�ڱ���
    public virtual void InBag_Use()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Switch()
        {
            index = itemPath.itemIndex
        });
    }
    #endregion
    #region//������
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
    public virtual bool OnHand_UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        return true;
    }
    /// <summary>
    /// ����ͷ�
    /// </summary>
    /// <param name="state"></param>
    /// <param name="input"></param>
    public virtual void OnHand_ReleaseLeftPress(bool state, bool input, bool player)
    {

    }
    /// <summary>
    /// �Ҽ���ѹ
    /// </summary>
    /// <param name="pressTimer"></param>
    /// <param name="state"></param>
    /// <param name="input"></param>
    /// <returns>���ֵ</returns>
    public virtual bool OnHand_UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        return true;
    }
    /// <summary>
    /// �Ҽ��ͷ�
    /// </summary>
    /// <param name="state"></param>
    /// <param name="input"></param>
    public virtual void OnHand_ReleaseRightPress(bool state, bool input, bool player)
    {

    }
    /// <summary>
    /// �������λ��
    /// </summary>
    public virtual void OnHand_UpdateMousePos(Vector3 mouse)
    {

    }
    /// <summary>
    /// ��ʼ����
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="item"></param>
    public virtual void OnHand_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.transform_ItemInRightHand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="owner"></param>
    public virtual void OnHand_Over(ActorManager owner, BodyController_Human body)
    {

    }
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual void OnHand_UpdateTime(int second)
    {

    }
    /// <summary>
    /// �������
    /// </summary>
    public virtual void OnHand_UpdateLook()
    {

    }
    #endregion
    #region//��ͷ��
    /// <summary>
    /// ��ʼ����
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnHead_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.transform_ItemOnHead.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnHead_Over(ActorManager owner, BodyController_Human body)
    {

    }
    #endregion
    #region//������
    /// <summary>
    /// ��ʼ����
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnBody_Start(ActorManager owner, BodyController_Human body)
    {
        this.owner = owner;
        body.transform_ItemOnBody.GetComponent<SpriteRenderer>().sprite
           = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="body"></param>
    public virtual void OnBody_Over(ActorManager owner, BodyController_Human body)
    {

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
    /// �ϲ�
    /// </summary>
    /// <param name="itemData_CombineA">�ϲ�����A</param>
    /// <param name="itemData_CombineB">�ϲ�����B</param>
    /// <param name="maxCombineCount">�ϲ������</param>
    /// <param name="itemData_Res">�ϲ�����ʣ��</param> 
    public virtual void StaticAction_Combine(ItemData itemData_CombineA, ItemData itemData_CombineB, short maxCombineCount, out ItemData itemData_Combine, out ItemData itemData_Res)
    {
        itemData_Combine = itemData_CombineA;
        itemData_Res = itemData_CombineB;
        if(itemData_CombineA.Item_Count + itemData_CombineB.Item_Count <= maxCombineCount)
        {
            itemData_Combine.Item_Count += itemData_CombineB.Item_Count;
            itemData_Res.Item_Count = 0;
        }
        else
        {
            itemData_Combine.Item_Count = maxCombineCount;
            itemData_Res.Item_Count = ((short)(itemData_CombineA.Item_Count + itemData_CombineB.Item_Count - maxCombineCount));
        }
    }
    /// <summary>
    /// ���
    /// </summary>
    /// <param name="oldContent">������</param>
    /// <param name="addItem">�����</param>
    /// <param name="newContent">������</param>
    /// <param name="resItem">ʣ����</param>
    public virtual void StaticAction_FillUp(ItemData oldContent, ItemData addItem, out ItemData newContent, out ItemData resItem)
    {
        newContent = oldContent;
        resItem = addItem;
    }
    #endregion
}
/// <summary>
/// -----��������-----
/// </summary>
public class ItemBase_Materials : ItemBase
{

}
/// <summary>
/// -----����ʳ��-----
/// </summary>
public class ItemBase_Food : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.Item_Info = 100;
        initData.Item_Durability = 100;
        initData.Item_SignTime = (short)(MapManager.Instance.mapNetManager.Day * 10 + MapManager.Instance.mapNetManager.Hour);
    }
    public override void UpdateDataFromNet(ItemData itemData)
    {
        base.UpdateDataFromNet(itemData);
        CalculateDurability(int_rotBase);
    }
    /// <summary>
    /// ���û�����ֵ
    /// </summary>
    private int int_rotBase = -5;
    /// <summary>
    /// �������ʶ�
    /// </summary>
    /// <param name="nowTime"></param>
    public virtual void CalculateDurability(float ratBase)
    {
        /*��ǰʱ��*/
        int nowTime = MapManager.Instance.mapNetManager.Day * 10 + MapManager.Instance.mapNetManager.Hour;
        /*��¼ʱ��*/
        int lastTime = itemData.Item_SignTime;
        /*��������*/
        float rotSpeed = itemData.Item_Info * 0.01f;
        int offset = (int)((nowTime - lastTime) * rotSpeed * ratBase);
        if (offset <= -1)
        {
            /*���ô���1*/
            if (itemData.Item_Durability + offset >= 0)
            {
                itemData.Item_Durability += (sbyte)offset;
                if(itemData.Item_Durability <= 0) itemData.Item_Durability = 0;
                itemData.Item_SignTime = (short)nowTime;
            }
            else
            {
                itemData.Item_Durability = 0;
                itemData.Item_SignTime = (short)nowTime;
            }

        }
        else
        {
            /*����С��1*/
        }
    }
    public override void StaticAction_Combine(ItemData mainItem, ItemData addItem, short maxCap, out ItemData newItem, out ItemData resItem)
    {
        newItem = mainItem;
        resItem = mainItem;

        int wa_Dp = mainItem.Item_SignTime * mainItem.Item_Count + addItem.Item_SignTime * addItem.Item_Count;
        int wa_Dv = mainItem.Item_Durability * mainItem.Item_Count + addItem.Item_Durability * addItem.Item_Count;

        newItem.Item_SignTime = (short)((float)wa_Dp / (float)(addItem.Item_Count + mainItem.Item_Count));
        newItem.Item_Durability = (sbyte)((float)wa_Dv / (float)(addItem.Item_Count + mainItem.Item_Count));
        resItem.Item_SignTime = (short)((float)wa_Dp / (float)(addItem.Item_Count + mainItem.Item_Count));
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
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string stringName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
        string stringRotten = LocalizationManager.Instance.GetLocalization("Item_String","Rotten");
        string stringDesc = GridCell_UpdateDesc(LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc"));
        string stringInfo;
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        if(itemData.Item_Durability <= 0)
        {
            stringInfo = stringName + "("+ stringRotten + ")" + "\n" + stringDesc;
        }
        else
        {
            stringInfo = stringName + "\n" + stringDesc;
        }
        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, itemData.Item_Count.ToString());
        gridCell.SetCell(stringInfo);
        gridCell.SetSliderVal(itemData.Item_Durability / 100f);
        if (itemData.Item_Info == 0)
        {
            gridCell.FreezeCell(true);
            gridCell.SetSliderColor(new Color(0.5f, 1, 0, 1));
        }
        else
        {
            gridCell.FreezeCell(false);
            gridCell.SetSliderColor(new Color(Mathf.Lerp(1, 0, itemData.Item_Durability / 100f), Mathf.Lerp(0f, 1, itemData.Item_Durability / 100f), 0, 1));
        }
    }
    #region//����
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
        if (itemData.Item_Durability <= 0)
        {
            Posion();
        }
        Expend(1);
    }
    public virtual void Posion()
    {

    }
    public virtual void Expend(int val)
    {
        if (itemData.Item_Count > val)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            _newItem.Item_Count = (short)(_newItem.Item_Count - val);
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
/// -----����ҩ��-----
/// </summary>
public class ItemBase_Potion : ItemBase
{
    #region//����
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
        if (itemData.Item_Count > val)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            _newItem.Item_Count = (short)(_newItem.Item_Count - val);
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
/// -----��������-----
/// </summary>
public class ItemBase_Weapon : ItemBase
{
    public override void StaticAction_InitData(short id,out ItemData initData)
    {
        initData = new ItemData(id);
        initData.Item_Info = (short)new System.Random().Next(0, short.MaxValue);
        initData.Item_Durability = (sbyte)new System.Random().Next(80, 101);
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string stringName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
        string stringDesc = GridCell_UpdateDesc(LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc"));
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);

        string stringInfo = stringName + "(" + stringQuality + ")" + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, itemData.Item_Durability.ToString() + "%");
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
/// -----��������-----
/// </summary>
public class ItemBase_Tool : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.Item_Info = (short)new System.Random().Next(0, short.MaxValue);
        initData.Item_Durability = (sbyte)new System.Random().Next(80, 101);
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string stringName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
        string stringDesc = GridCell_UpdateDesc(LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc"));
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);

        string stringInfo = stringName + "(" + stringQuality + ")" + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity, itemData.Item_Durability.ToString() + "%");
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
/// -----����ǹ-----
/// </summary>
public class ItemBase_Gun : ItemBase
{
    public override void StaticAction_InitData(short id, out ItemData initData)
    {
        initData = new ItemData(id);
        initData.Item_Info = (short)new System.Random().Next(short.MinValue, short.MaxValue);
        initData.Item_Durability = (sbyte)new System.Random().Next(sbyte.MinValue, sbyte.MaxValue);
    }
    public override void GridCell_Draw(UI_GridCell gridCell)
    {
        string stringName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
        string stringDesc = GridCell_UpdateDesc(LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc"));
        string stringQuality = LocalizationManager.Instance.GetLocalization("Item_String", "_ItemQuality_" + (int)itemQuality);
        stringName = ItemConfigData.Colour(stringName, itemConfig.Item_Rarity);
        stringQuality = ItemConfigData.Colour(stringQuality, itemQuality);

        string stringInfo = stringName + "(" + stringQuality + ")" + "\n" + stringDesc;

        gridCell.DrawCell("Item_" + itemData.Item_ID.ToString(), "ItemBG_" + (int)itemConfig.Item_Rarity,"");
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
/// ��Ʒ_��װ
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
/// ��Ʒ_ñ��
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
/// ��Ʒ_�Ĳ�
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
    public short Item_ID;
    public short Item_Count;
    public short Item_Info;
    public sbyte Item_Durability;
    public short Item_SignTime;
    public bool Equals(ItemData other)
    {
        if (Item_ID == other.Item_ID && 
            Item_Info == other.Item_Info && 
            Item_Count == other.Item_Count)
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
        if (Item_ID == 0)
        {
            Item_Count = 0;
        }
        else
        {
            Item_Count = 1;
        }
        Item_Info = 0;
        Item_Durability = 0;
        Item_SignTime = 0;
    }
}
/// <summary>
/// ��Ʒ��ַ
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
/// ��Ʒ����
/// </summary>
public enum ItemFrom
{
    Default,
    /// <summary>
    /// Ұ��
    /// </summary>
    OutSide,
    /// <summary>
    /// �ֲ�
    /// </summary>
    Hand,
    /// <summary>
    /// ����
    /// </summary>
    Body,
    /// <summary>
    /// ͷ��
    /// </summary>
    Head,
    /// <summary>
    /// ��Ʒ
    /// </summary>
    Accessory,
    /// <summary>
    /// �Ĳ�
    /// </summary>
    Consumables,
    /// <summary>
    /// ����
    /// </summary>
    Bag,
}
public class InputData
{
    public float rightPressTimer = 0;
    public float leftPressTimer = 0;
    public Vector3 mousePosition = Vector3.zero;
}
