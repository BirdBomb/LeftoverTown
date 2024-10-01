using DG.Tweening;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Sockets.NetBitBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class ItemSystem2000 
{
    
}
/// <summary>
/// 木斧头
/// </summary>
public class Item_2001 : ItemBase_Tool
{
    private GameObject itemLocalObj;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2001");
        itemLocalObj.transform.SetParent(body.Hand_RightItem);
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localScale = Vector3.one;
    }
    #endregion
    #region//使用逻辑
    private bool temp_attacking = false;
    private float temp_attackRadiu;

    private const short config_attackDamage = 5;
    private const float config_attackMaxRange = 120;
    private const float config_attackReadyTime = 0.5f;

    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= config_attackReadyTime)
            {
                temp_attacking = true;
                temp_attackRadiu = GetAttackRadiu();
                owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public override bool UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            temp_attacking = false;
            owner.BodyController.SetHandTrigger("Slash_Vertical_Ready", 1 / config_attackReadyTime, null);
            owner.BodyController.SetHandBool("Slash_Vertical_Release", false, 1 / config_attackReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Slash_Vertical_Play");
        }
        inputData.rightPressTimer = pressTimer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return inputData.rightPressTimer > config_attackReadyTime;
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (owner)
        {
            if (inputData.rightPressTimer > 0)
            {
                inputData.rightPressTimer = 0;
                if (!temp_attacking)
                {
                    owner.BodyController.SetHandBool("Slash_Vertical_Release", true, inputData.rightPressTimer / config_attackReadyTime, null);
                }
            }
            if (player && input)
            {
                HideSkillSector();
            }
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    private float GetAttackRadiu()
    {
        return Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / config_attackReadyTime);
    }
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer < config_attackReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / config_attackReadyTime), config_attackMaxRange, 1);
        }
        else
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, 1), config_attackMaxRange, 1);
        }
    }
    private void HideSkillSector()
    {
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
    }
    public void OnlyInput_Attack()
    {
        sbyte temp = 0;
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, temp_attackRadiu, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 1);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                    temp = -2;
                }
            }
            if (targetTile[i].TryGetComponent(out TileObj tile))
            {
                tile.TryToTakeDamage(config_attackDamage);
                temp = -2;
            }
        }
        Holding_ChangeDurability(temp);
    }
    #endregion
}
/// <summary>
/// 铁斧头
/// </summary>
public class Item_2002 : ItemBase_Tool
{
    private GameObject itemLocalObj;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2002");
        itemLocalObj.transform.SetParent(body.Hand_RightItem);
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localScale = Vector3.one;
    }
    #endregion
    #region//使用逻辑
    private bool temp_attacking = false;
    private float temp_attackRadiu;

    private const short config_attackDamage = 7;
    private const float config_attackMaxRange = 120;
    private const float config_attackReadyTime = 0.5f;

    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= config_attackReadyTime)
            {
                temp_attacking = true;
                temp_attackRadiu = GetAttackRadiu();
                owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
                return true;    
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public override bool UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            temp_attacking = false;
            owner.BodyController.SetHandTrigger("Slash_Vertical_Ready", 1 / config_attackReadyTime, null);
            owner.BodyController.SetHandBool("Slash_Vertical_Release", false, 1 / config_attackReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Slash_Vertical_Play");
        }
        inputData.rightPressTimer = pressTimer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return inputData.rightPressTimer > config_attackReadyTime;
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (owner)
        {
            if (inputData.rightPressTimer > 0)
            {
                inputData.rightPressTimer = 0;
                if (!temp_attacking)
                {
                    owner.BodyController.SetHandBool("Slash_Vertical_Release", true, inputData.rightPressTimer / config_attackReadyTime, null);
                }
            }
            if (player && input)
            {
                HideSkillSector();
            }
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    private float GetAttackRadiu()
    {
        return Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / config_attackReadyTime);
    }
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer < config_attackReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / config_attackReadyTime), config_attackMaxRange, 1);
        }
        else
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, 1), config_attackMaxRange, 1);
        }
    }
    private void HideSkillSector()
    {
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
    }
    public void OnlyInput_Attack()
    {
        sbyte temp = 0;
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, temp_attackRadiu, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 1);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                    temp = -2;
                }
            }
            if (targetTile[i].TryGetComponent(out TileObj tile))
            {
                tile.TryToTakeDamage(config_attackDamage);
                temp = -2;
            }
        }
        Holding_ChangeDurability(temp);
    }
    #endregion
}
/// <summary>
/// 粗制木弓
/// </summary>
public class Item_2003 : ItemBase_Gun
{
    private ItemLocalObj_Bow itemLocalObj_Bow;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2003").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.transform.SetParent(body.Hand_LeftItem);
        itemLocalObj_Bow.transform.localRotation = Quaternion.identity;
        itemLocalObj_Bow.transform.localPosition = new Vector3(0.1f, 0, 0);
        itemLocalObj_Bow.transform.localScale = Vector3.one;

        itemLocalObj_Bow.leftHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            if (inputData.rightPressTimer > 0)
            {
                inputData.rightPressTimer = 0;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Content.Item_Count.ToString();
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_Content.Item_ID.ToString());
        }
        else
        {
            gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        }
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9002 || putInItemData.Item_ID == 9003)
            {
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, 10, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//使用逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 1f;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 120;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 25;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 1;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 2f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 1f;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 1.2f;

    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (temp_shotTime != 0) { return true; }
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return true;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            itemLocalObj_Bow.DrawBow();
            owner.BodyController.SetHandTrigger("Bow_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Bow_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Bow_Play");
            AddArrow();
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return GetAttackRange() == config_aimMinRange;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            itemLocalObj_Bow.ReleaseBow();
            owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    private void AddArrow()
    {
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
                owner.BodyController.Hand_RightItem.localRotation = Quaternion.Euler(0, 0, -45);
                owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_Content.Item_ID);
            }
            else
            {
                owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
            }
        }
        else
        {
            owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
            owner.BodyController.Hand_RightItem.localRotation = Quaternion.Euler(0, 0, -45);
            owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + 9003);
        }
    }
    private void SubArrow()
    {
        owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (temp_shotTime > 0) { owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0); }
        else
        {
            if (inputData.rightPressTimer > config_shotReadyTime)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
            }
            else
            {
                if (inputData.rightPressTimer == 0)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
                }
                else
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
                }
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            Shot(bulletID, GetRandomDir(offset), owner);
            SubArrow();
            itemLocalObj_Bow.ShotBow();
            owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
    }
    /// <summary>
    /// 射击
    /// </summary>
    private void Shot(short bulletID, Vector3 dir, ActorManager actor)
    {
        owner.BodyController.Hand_RightItem.DOKill();
        owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
        owner.BodyController.Hand_RightItem.DOPunchPosition(new Vector3(-0.2f, 0, 0), 0.1f);
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + bulletID);
        obj.transform.position = owner.SkillSector.CenterPos;
        obj.GetComponent<BulletBase>().InitBullet(dir, 10, actor.NetManager);
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9003;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        _newItem.Item_Seed--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset)
    {
        UnityEngine.Random.InitState(itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// 火把
/// </summary>
public class Item_2004 : ItemBase_Tool
{
    private GameObject itemLocalObj;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2004");
        itemLocalObj.transform.SetParent(body.Hand_RightItem);
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localScale = Vector3.one;
    }
    #endregion
    #region//使用逻辑
    private short config_attackDamage = 2;
    private float config_attackSpeed = 1.5f;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Horizontal", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Horizontal") && itemLocalObj)
                    {
                        OnlyInput_Attack();
                    }
                });
            }
        }
        return true;
    }
    private void OnlyInput_Attack()
    {
        sbyte temp = -1;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(itemLocalObj.transform.position, itemLocalObj.transform.position + inputData.mousePosition.normalized * itemConfig.Attack_Distance);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (actor == owner) { continue; }
                    else
                    {
                        actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                        temp = -5;
                    }
                }
            }
        }
        Holding_ChangeDurability(temp);
    }
    #endregion
    #region//燃烧逻辑
    private int temp_burnTimer = 0;
    private const int config_burnTimer = 5;
    public override void UpdateTime(int second)
    {
        if (temp_burnTimer > config_burnTimer)
        {
            temp_burnTimer = 0;
            Burn(-1);
        }
        else
        {
            temp_burnTimer += second;
        }
        base.UpdateTime(second);
    }
    public void Burn(sbyte offset)
    {
        Holding_ChangeDurability(offset);
    }
    #endregion
}
/// <summary>
/// 精制木弓
/// </summary>
public class Item_2005 : ItemBase_Gun
{
    private ItemLocalObj_Bow itemLocalObj_Bow;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2005").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.transform.SetParent(body.Hand_LeftItem);
        itemLocalObj_Bow.transform.localRotation = Quaternion.identity;
        itemLocalObj_Bow.transform.localPosition = new Vector3(0.1f, 0, 0);
        itemLocalObj_Bow.transform.localScale = Vector3.one;

        itemLocalObj_Bow.leftHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            if (inputData.rightPressTimer > 0)
            {
                inputData.rightPressTimer = 0;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Content.Item_Count.ToString();
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_Content.Item_ID.ToString());
        }
        else
        {
            gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        }
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9002 || putInItemData.Item_ID == 9003)
            {
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, 10, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//使用逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 1.5f;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 90;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 15;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 0.5f;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 1f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 1f;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 1.5f;

    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (temp_shotTime != 0) { return true; }
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return true;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            itemLocalObj_Bow.DrawBow();
            owner.BodyController.SetHandTrigger("Bow_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Bow_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Bow_Play");
            AddArrow();
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return GetAttackRange() == config_aimMinRange;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            itemLocalObj_Bow.ReleaseBow();
            owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    private void AddArrow()
    {
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
                owner.BodyController.Hand_RightItem.localRotation = Quaternion.Euler(0, 0, -45);
                owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_Content.Item_ID);
            }
            else
            {
                owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
            }
        }
        else
        {
            owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
            owner.BodyController.Hand_RightItem.localRotation = Quaternion.Euler(0, 0, -45);
            owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + 9003);
        }
    }
    private void SubArrow()
    {
        owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (temp_shotTime > 0) { owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0); }
        else
        {
            if (inputData.rightPressTimer > config_shotReadyTime)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
            }
            else
            {
                if (inputData.rightPressTimer == 0)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
                }
                else
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
                }
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            Shot(bulletID, GetRandomDir(offset), owner);
            SubArrow();
            itemLocalObj_Bow.ShotBow();
            owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
    }
    /// <summary>
    /// 射击
    /// </summary>
    private void Shot(short bulletID, Vector3 dir, ActorManager actor)
    {
        owner.BodyController.Hand_RightItem.DOKill();
        owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
        owner.BodyController.Hand_RightItem.DOPunchPosition(new Vector3(-0.2f, 0, 0), 0.1f);
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + bulletID);
        obj.transform.position = owner.SkillSector.CenterPos;
        obj.GetComponent<BulletBase>().InitBullet(dir, 10, actor.NetManager);
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9003;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        _newItem.Item_Seed--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset)
    {
        UnityEngine.Random.InitState(itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// 木棍
/// </summary>
public class Item_2006 : ItemBase_Weapon
{
    private GameObject itemLocalObj;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2007");
        itemLocalObj.transform.SetParent(body.Hand_RightItem);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
        itemLocalObj.transform.Find("Hand").GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }
        base.UpdateMousePos(mouse);
    }

    #endregion
    #region//使用逻辑
    private const float config_attackSpeed = 10f;
    private const short config_attackDamage = 2;
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (itemLocalObj.transform.localPosition == Vector3.zero)
            {
                itemLocalObj.transform.DOKill();
                itemLocalObj.transform.DOLocalMoveX(0.5f, 1f / config_attackSpeed).SetLoops(2, LoopType.Yoyo).OnStepComplete(() =>
                {
                    if (input && itemLocalObj)
                    {
                        OnlyInput_Attack();
                    }
                });
            }
        }
        return true;
    }
    private void OnlyInput_Attack()
    {
        sbyte temp = 0;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(itemLocalObj.transform.position, itemLocalObj.transform.position + inputData.mousePosition.normalized * itemConfig.Attack_Distance);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (actor == owner) { continue; }
                    else
                    {
                        actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                        temp = -2;
                    }
                }
            }
        }
        Holding_ChangeDurability(temp);
    }
    #endregion
}
/// <summary>
/// 长柄刀
/// </summary>
public class Item_2007 : ItemBase_Weapon
{
    private GameObject itemLocalObj;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2007");
        itemLocalObj.transform.SetParent(body.Hand_RightItem);
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localScale = Vector3.one;
        itemLocalObj.transform.Find("Hand").GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }
        base.UpdateMousePos(mouse);
    }

    #endregion
    #region//使用逻辑
    private const float config_attackSpeed = 10f;
    private const short config_attackDamage = 5;
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (itemLocalObj.transform.localPosition == Vector3.zero)
            {
                itemLocalObj.transform.DOKill();
                itemLocalObj.transform.DOLocalMoveX(0.5f, 1f / config_attackSpeed).SetLoops(2, LoopType.Yoyo).OnStepComplete(() =>
                {
                    if (input && itemLocalObj)
                    {
                        OnlyInput_Attack();
                    }
                });
            }
        }
        return true;
    }
    private void OnlyInput_Attack()
    {
        sbyte temp = 0;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(itemLocalObj.transform.position, itemLocalObj.transform.position + inputData.mousePosition.normalized * itemConfig.Attack_Distance);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (actor == owner) { continue; }
                    else
                    {
                        actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                        temp = -2;
                    }
                }
            }
        }
        Holding_ChangeDurability(temp);
    }
    #endregion
}
/// <summary>
/// 精钢匕首
/// </summary>
public class Item_2008 : ItemBase_Weapon
{
    private GameObject itemLocalObj;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2008");
        itemLocalObj.transform.SetParent(body.Hand_RightItem);
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localScale = Vector3.one;
    }
    #endregion
    #region//使用逻辑
    private const short config_attackDamage = 2;
    private const float config_attackSpeed = 1.5f;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Horizontal", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Horizontal") && itemLocalObj)
                    {
                        OnlyInput_Attack();
                    }
                });
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Attack()
    {
        sbyte temp = 0;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(itemLocalObj.transform.position, itemLocalObj.transform.position + inputData.mousePosition.normalized * itemConfig.Attack_Distance);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (actor == owner) { continue; }
                    else
                    {
                        actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                        temp = -2;
                    }
                }
            }
        }
        Holding_ChangeDurability(temp);
    }
    #endregion
}
/// <summary>
/// 短筒枪
/// </summary>
public class Item_2009 : ItemBase_Gun
{
    private ItemLocalObj_2009 itemLocalObj_2009;
    #region//枪械交互逻辑
    /// <summary>
    /// 弹容量
    /// </summary>
    private const int config_bulletCapacity = 15;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2009 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2009").GetComponent<ItemLocalObj_2009>();
        itemLocalObj_2009.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2009.transform.localRotation = Quaternion.identity;
        itemLocalObj_2009.transform.localPosition = Vector3.zero;
        itemLocalObj_2009.transform.localScale = Vector3.one;

        itemLocalObj_2009.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;

    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    /// <summary>
    /// 尝试给枪械装弹
    /// </summary>
    /// <param name="putInItemData"></param>
    /// <returns></returns>
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9004)
            {
                AudioManager.Instance.PlayEffect(1003);
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, config_bulletCapacity, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//枪械射击逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 1f;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 50;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 20;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 1;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 1;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 1;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 1.2f;
    /// <summary>
    /// 每次射击的后坐力
    /// </summary>
    private const float config_shotRecoilForce = 0.5f;
    /// <summary>
    /// NPC最大连续射击次数
    /// </summary>
    private const int config_shotMaxTime = 1;
    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return temp_shotTime > config_shotMaxTime;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Shoot_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > config_shotReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            itemLocalObj_2009.Shoot(bulletID, GetRandomDir(offset, temp_shotTime), owner);
            AddForce(config_shotRecoilForce);
        }
        else
        {
            itemLocalObj_2009.Dull(owner);
        }
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9004;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 添加后坐力
    /// </summary>
    /// <param name="force"></param>
    private void AddForce(float force)
    {
        if (owner.isPlayer)
        {
            owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * force;
        }
        owner.NetManager.UpdateSeed();
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset, int bullet)
    {
        UnityEngine.Random.InitState(bullet + itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// 黄金弓
/// </summary>
public class Item_2010 : ItemBase_Gun
{
    private ItemLocalObj_Bow itemLocalObj_Bow;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_Bow = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2010").GetComponent<ItemLocalObj_Bow>();
        itemLocalObj_Bow.transform.SetParent(body.Hand_LeftItem);
        itemLocalObj_Bow.transform.localRotation = Quaternion.identity;
        itemLocalObj_Bow.transform.localPosition = new Vector3(0.1f, 0, 0);
        itemLocalObj_Bow.transform.localScale = Vector3.one;

        itemLocalObj_Bow.leftHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            if (inputData.rightPressTimer>0)
            {
                inputData.rightPressTimer = 0;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_MainIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID.ToString());
        gridCell.text_Name.text = itemConfig.Item_Name.ToString();
        gridCell.text_Info.text = itemData.Item_Content.Item_Count.ToString();
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_Content.Item_ID.ToString());
        }
        else
        {
            gridCell.image_ChildIcon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        }
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9002 || putInItemData.Item_ID == 9003)
            {
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, 10, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//使用逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 1.5f;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 90;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 15;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 0.5f;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 1f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 1f;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 1.5f;

    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (temp_shotTime != 0) { return true; }
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return true;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            itemLocalObj_Bow.DrawBow();
            owner.BodyController.SetHandTrigger("Bow_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Bow_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Bow_Play");
            AddArrow();
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return GetAttackRange() == config_aimMinRange;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            itemLocalObj_Bow.ReleaseBow();
            owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    private void AddArrow()
    {
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
                owner.BodyController.Hand_RightItem.localRotation = Quaternion.Euler(0, 0, -45);
                owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_Content.Item_ID);
            }
            else
            {
                owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
            }
        }
        else
        {
            owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
            owner.BodyController.Hand_RightItem.localRotation = Quaternion.Euler(0, 0, -45);
            owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + 9003);
        }
    }
    private void SubArrow()
    {
        owner.BodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (temp_shotTime > 0) { owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0); }
        else
        {
            if (inputData.rightPressTimer > config_shotReadyTime)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
            }
            else
            {
                if (inputData.rightPressTimer == 0)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
                }
                else
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
                }
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            Shot(bulletID, GetRandomDir(offset), owner);
            SubArrow();
            itemLocalObj_Bow.ShotBow();
            owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
    }
    /// <summary>
    /// 射击
    /// </summary>
    private void Shot(short bulletID, Vector3 dir, ActorManager actor)
    {
        owner.BodyController.Hand_RightItem.DOKill();
        owner.BodyController.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
        owner.BodyController.Hand_RightItem.DOPunchPosition(new Vector3(-0.2f, 0, 0), 0.1f);
        GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + bulletID);
        obj.transform.position = owner.SkillSector.CenterPos;
        obj.GetComponent<BulletBase>().InitBullet(dir, 10, actor.NetManager);
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9003;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        _newItem.Item_Seed--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset)
    {
        UnityEngine.Random.InitState(itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// 宽刃钢刀
/// </summary>
public class Item_2011 : ItemBase_Weapon
{
    private GameObject itemLocalObj;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2011");
        itemLocalObj.transform.SetParent(body.Hand_RightItem);
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localScale = Vector3.one;
    }
    #endregion
    #region//使用逻辑
    private const short config_attackDamage = 3;
    private const float config_attackSpeed = 1;
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("Slash_Horizontal", config_attackSpeed, (string str) =>
                {
                    if (input && str.Equals("Slash_Horizontal") && itemLocalObj)
                    {
                        OnlyInput_Attack();
                    }
                });
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_Attack()
    {
        sbyte temp = 0;
        RaycastHit2D[] hit2D = Physics2D.LinecastAll(itemLocalObj.transform.position, itemLocalObj.transform.position + inputData.mousePosition.normalized * itemConfig.Attack_Distance);
        for (int i = 0; i < hit2D.Length; i++)
        {
            if (hit2D[i].collider.CompareTag("Actor"))
            {
                if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                {
                    if (actor == owner) { continue; }
                    else
                    {
                        actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                        temp = -2;
                    }
                }
            }
        }
        Holding_ChangeDurability(temp);
    }
    #endregion
}
/// <summary>
/// BPM
/// </summary>
public class Item_2012 : ItemBase_Gun
{
    private ItemLocalObj_2012 itemLocalObj_2012;
    #region//枪械交互逻辑
    /// <summary>
    /// 弹容量
    /// </summary>
    private const int config_bulletCapacity = 30;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2012 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2012").GetComponent<ItemLocalObj_2012>();
        itemLocalObj_2012.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2012.transform.localRotation = Quaternion.identity;
        itemLocalObj_2012.transform.localPosition = Vector3.zero;
        itemLocalObj_2012.transform.localScale = Vector3.one;

        itemLocalObj_2012.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;

    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    /// <summary>
    /// 尝试给枪械装弹
    /// </summary>
    /// <param name="putInItemData"></param>
    /// <returns></returns>
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9004)
            {
                AudioManager.Instance.PlayEffect(1003);
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, config_bulletCapacity, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//枪械射击逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 1f;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 60;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 20;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 0.2f;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 1f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 0.1f;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 0.25f;
    /// <summary>
    /// 每次射击的后坐力
    /// </summary>
    private const float config_shotRecoilForce = 0.1f;
    /// <summary>
    /// NPC最大连续射击次数
    /// </summary>
    private const int config_shotMaxTime = 5;
    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return temp_shotTime > config_shotMaxTime;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Shoot_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > config_shotReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            itemLocalObj_2012.Shoot(bulletID, GetRandomDir(offset, temp_shotTime), owner);
            AddForce(config_shotRecoilForce);
        }
        else
        {
            itemLocalObj_2012.Dull(owner);
        }
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9004;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 添加后坐力
    /// </summary>
    /// <param name="force"></param>
    private void AddForce(float force)
    {
        if (owner.isPlayer)
        {
            owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * force;
        }
        owner.NetManager.UpdateSeed();
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset, int bullet)
    {
        UnityEngine.Random.InitState(bullet + itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// AKM
/// </summary>
public class Item_2013 : ItemBase_Gun
{
    private ItemLocalObj_2013 itemLocalObj_2013;
    #region//枪械交互逻辑
    /// <summary>
    /// 弹容量
    /// </summary>
    private const int config_bulletCapacity = 30;

    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2013 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2013").GetComponent<ItemLocalObj_2013>();
        itemLocalObj_2013.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2013.transform.localPosition = Vector3.zero;
        itemLocalObj_2013.transform.localRotation = Quaternion.identity;
        itemLocalObj_2013.transform.localScale = Vector3.one;
        itemLocalObj_2013.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        itemLocalObj_2013.leftHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9004)
            {
                AudioManager.Instance.PlayEffect(1003);
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, config_bulletCapacity, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//枪械射击逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 2;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 60;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 10;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 0.25f;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 1f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 0.15f;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 0.25f;
    /// <summary>
    /// 每次射击的后坐力
    /// </summary>
    private const float config_shotRecoilForce = 1f;
    /// <summary>
    /// NPC最大连续射击次数
    /// </summary>
    private const int config_shotMaxTime = 3;

    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return temp_shotTime > config_shotMaxTime;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Shoot_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > config_shotReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            itemLocalObj_2013.Shoot(bulletID, GetRandomDir(offset, temp_shotTime), owner);
            AddForce(config_shotRecoilForce);
        }
        else
        {
            itemLocalObj_2013.Dull(owner);
        }
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9004;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 添加后坐力
    /// </summary>
    /// <param name="force"></param>
    private void AddForce(float force)
    {
        if (owner.isPlayer)
        {
            owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * force;
        }
        owner.NetManager.UpdateSeed();
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset, int bullet)
    {
        UnityEngine.Random.InitState(bullet + itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// Glock
/// </summary>
public class Item_2014 : ItemBase_Gun
{
    private ItemLocalObj_2014 itemLocalObj_2014;
    #region//枪械交互逻辑
    /// <summary>
    /// 弹容量
    /// </summary>
    private const int config_bulletCapacity = 15;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2014 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2014").GetComponent<ItemLocalObj_2014>();
        itemLocalObj_2014.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2014.transform.localRotation = Quaternion.identity;
        itemLocalObj_2014.transform.localPosition = Vector3.zero;
        itemLocalObj_2014.transform.localScale = Vector3.one;

        itemLocalObj_2014.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;

    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    /// <summary>
    /// 尝试给枪械装弹
    /// </summary>
    /// <param name="putInItemData"></param>
    /// <returns></returns>
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9004)
            {
                AudioManager.Instance.PlayEffect(1003);
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, config_bulletCapacity, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//枪械射击逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 1.5f;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 35;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 10;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 0.2f;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 0.5f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 0.5f;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 0.6f;
    /// <summary>
    /// 每次射击的后坐力
    /// </summary>
    private const float config_shotRecoilForce = 0.5f;
    /// <summary>
    /// NPC最大连续射击次数
    /// </summary>
    private const int config_shotMaxTime = 2;
    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return temp_shotTime > config_shotMaxTime;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Shoot_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > config_shotReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            itemLocalObj_2014.Shoot(bulletID, GetRandomDir(offset, temp_shotTime), owner);
            AddForce(config_shotRecoilForce);
        }
        else
        {
            itemLocalObj_2014.Dull(owner);
        }
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9004;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 添加后坐力
    /// </summary>
    /// <param name="force"></param>
    private void AddForce(float force)
    {
        if (owner.isPlayer)
        {
            owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * force;
        }
        owner.NetManager.UpdateSeed();
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset, int bullet)
    {
        UnityEngine.Random.InitState(bullet + itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// Ithaca
/// </summary>
public class Item_2015 : ItemBase_Gun
{
    private ItemLocalObj_2015 itemLocalObj_2015;
    #region//枪械交互逻辑
    /// <summary>
    /// 弹容量
    /// </summary>
    private const int config_bulletCapacity = 5;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2015 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2015").GetComponent<ItemLocalObj_2015>();
        itemLocalObj_2015.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2015.transform.localRotation = Quaternion.identity;
        itemLocalObj_2015.transform.localPosition = Vector3.zero;
        itemLocalObj_2015.transform.localScale = Vector3.one;

        itemLocalObj_2015.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        itemLocalObj_2015.leftHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9004)
            {
                AudioManager.Instance.PlayEffect(1003);
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, config_bulletCapacity, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }

    #endregion
    #region//枪械射击逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 1;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 120;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 30;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 0.5f;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 0.5f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 0.5f;
    /// <summary>
    /// 散射子弹数
    /// </summary>
    private const int config_bulletCount = 5;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 0.6f;
    /// <summary>
    /// 每次射击的后坐力
    /// </summary>
    private const float config_shotRecoilForce = 2f;
    /// <summary>
    /// NPC最大连续射击次数
    /// </summary>
    private const int config_shotMaxTime = 1;

    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return temp_shotTime > config_shotMaxTime;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Shoot_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.UpdateMousePos(mouse);
    }

    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > config_shotReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            itemLocalObj_2015.Shoot(bulletID, GetRandomDir(offset, temp_shotTime), owner);
            AddForce(config_shotRecoilForce);
        }
        else
        {
            itemLocalObj_2015.Dull(owner);
        }
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9004;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 添加后坐力
    /// </summary>
    /// <param name="force"></param>
    private void AddForce(float force)
    {
        if (owner.isPlayer)
        {
            owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * force;
        }
        owner.NetManager.UpdateSeed();
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        //Debug.Log(temp_nextAimPoint - inputData.rightPressTimer);
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private List<Vector3> GetRandomDir(float offset, int bullet)
    {
        List<Vector3> dirList = new List<Vector3>();
        for(int i = 0; i < config_bulletCount; i++)
        {
            UnityEngine.Random.InitState(bullet + itemData.Item_Seed + i * 2);
            //获得随机偏转角
            float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
            //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
            // 将角度转换为Quaternion
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
            // 将旋转应用到原始向量上
            dirList.Add(randomRotation * (inputData.mousePosition.normalized));
        }
        return dirList;
    }
    #endregion
}
/// <summary>
/// Madsen
/// </summary>
public class Item_2016 : ItemBase_Gun
{
    private ItemLocalObj_2016 itemLocalObj_2016;
    #region//枪械交互逻辑
    /// <summary>
    /// 弹容量
    /// </summary>
    private const int config_bulletCapacity = 60;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2016 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2016").GetComponent<ItemLocalObj_2016>();
        itemLocalObj_2016.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2016.transform.localPosition = Vector3.zero;
        itemLocalObj_2016.transform.localRotation = Quaternion.identity;
        itemLocalObj_2016.transform.localScale = Vector3.one;
        itemLocalObj_2016.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        itemLocalObj_2016.leftHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    /// <summary>
    /// 尝试给枪械装弹
    /// </summary>
    /// <param name="putInItemData"></param>
    /// <returns></returns>
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9004)
            {
                AudioManager.Instance.PlayEffect(1003);
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, config_bulletCapacity, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//枪械射击逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 1.5f;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 60;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 20;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 0.5f;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 0.5f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 0.1f;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 0.15f;
    /// <summary>
    /// 每次射击的后坐力
    /// </summary>
    private const float config_shotRecoilForce = 2f;
    /// <summary>
    /// NPC最大连续射击次数
    /// </summary>
    private const int config_shotMaxTime = 6;
    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return temp_shotTime > config_shotMaxTime;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Shoot_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > config_shotReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
        }
        else
        {
            if(inputData.rightPressTimer == 0)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            itemLocalObj_2016.Shoot(bulletID, GetRandomDir(offset, temp_shotTime), owner);
            AddForce(config_shotRecoilForce);
        }
        else
        {
            itemLocalObj_2016.Dull(owner);
        }
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9004;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 添加后坐力
    /// </summary>
    /// <param name="force"></param>
    private void AddForce(float force)
    {
        if (owner.isPlayer)
        {
            owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * force;
        }
        owner.NetManager.UpdateSeed();
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset, int bullet)
    {
        UnityEngine.Random.InitState(bullet + itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// AUG
/// </summary>
public class Item_2017 : ItemBase_Gun
{
    private ItemLocalObj_2017 itemLocalObj_2017;
    #region//枪械交互逻辑
    /// <summary>
    /// 弹容量
    /// </summary>
    private const int config_bulletCapacity = 25;

    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2017 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2017").GetComponent<ItemLocalObj_2017>();
        itemLocalObj_2017.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2017.transform.localPosition = Vector3.zero;
        itemLocalObj_2017.transform.localRotation = Quaternion.identity;
        itemLocalObj_2017.transform.localScale = Vector3.one;
        itemLocalObj_2017.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        itemLocalObj_2017.leftHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid_Child.OpenGrid(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        ItemData residueItem = putInItemData;
        ItemData newItemData = itemData;
        if (itemData.Item_Content.Item_Count == 0 || itemData.Item_Content.Item_ID == putInItemData.Item_ID)
        {
            if (putInItemData.Item_ID == 9004)
            {
                AudioManager.Instance.PlayEffect(1003);
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, config_bulletCapacity, out newItemData, out residueItem);
            }
        }
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            oldItem = itemData,
            newItem = newItemData,
        });
        if (residueItem.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = residueItem,
            });
        }
        return residueItem;
    }
    #endregion
    #region//枪械射击逻辑
    /// <summary>
    /// 射击次数
    /// </summary>
    private int temp_shotTime = 0;
    /// <summary>
    /// 下次射击时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextShotPoint = 0;
    /// <summary>
    /// 下次完全准备时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextReadyPoint = config_shotReadyTime;
    /// <summary>
    /// 下次完全瞄准时间点(随着射击次数提高)
    /// </summary>
    private float temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
    /// <summary>
    /// UI瞄准距离
    /// </summary>
    private const float config_aimDistance = 2;
    /// <summary>
    /// 最大瞄准角度
    /// </summary>
    private const int config_aimMaxRange = 20;
    /// <summary>
    /// 最小瞄准角度
    /// </summary>
    private const int config_aimMinRange = 5;
    /// <summary>
    /// 预备时间
    /// </summary>
    private const float config_shotReadyTime = 0.25f;
    /// <summary>
    /// 瞄准时间
    /// </summary>
    private const float config_shotAimTime = 0.75f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float config_shotCD = 0.125f;
    /// <summary>
    /// 每次射击的散射程度(必须大于等于射击间隔)
    /// </summary>
    private const float config_shotRecoilTime = 0.15f;
    /// <summary>
    /// 每次射击的后坐力
    /// </summary>
    private const float config_shotRecoilForce = 1f;
    /// <summary>
    /// NPC最大连续射击次数
    /// </summary>
    private const int config_shotMaxTime = 3;

    public override bool UpdateLeftPress(float timer, bool state, bool input, bool player)
    {
        inputData.leftPressTimer = timer;
        if (inputData.rightPressTimer > temp_nextReadyPoint)
        {
            if (inputData.leftPressTimer >= temp_nextShotPoint)
            {
                TryToShot(GetAttackRange());
                temp_nextShotPoint = inputData.leftPressTimer + config_shotCD;
                temp_nextReadyPoint = inputData.rightPressTimer + config_shotCD;
                if (temp_nextAimPoint > inputData.rightPressTimer)
                {
                    temp_nextAimPoint += config_shotRecoilTime;
                }
                else
                {
                    temp_nextAimPoint = inputData.rightPressTimer + config_shotRecoilTime;
                }
                return temp_shotTime > config_shotMaxTime;
            }
        }
        return false;
    }
    public override bool UpdateRightPress(float timer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / config_shotReadyTime, null);
            owner.BodyController.SetHandBool("Shoot_Release", false, 1 / config_shotReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
        }
        inputData.rightPressTimer = timer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        temp_nextShotPoint = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer > 0)
        {
            temp_shotTime = 0;
            temp_nextShotPoint = 0;
            temp_nextAimPoint = config_shotReadyTime + config_shotAimTime;
            temp_nextReadyPoint = config_shotReadyTime;
            inputData.rightPressTimer = 0;
            if (player && input)
            {
                UpdateSkillSector();
            }
            owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / config_shotReadyTime, null);
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// 更新技能指示器
    /// </summary>
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer > config_shotReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 1);
        }
        else
        {
            if (inputData.rightPressTimer == 0)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
            else
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, config_aimDistance, GetAttackRange(), 0.2f);
            }
        }
    }
    /// <summary>
    /// 尝试射击
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="inputState"></param>
    private void TryToShot(float offset)
    {
        if (CheckBullet(out short bulletID))
        {
            temp_shotTime++;
            itemLocalObj_2017.Shoot(bulletID, GetRandomDir(offset, temp_shotTime), owner);
            AddForce(config_shotRecoilForce);
        }
        else
        {
            itemLocalObj_2017.Dull(owner);
        }
    }
    /// <summary>
    /// 检查子弹
    /// </summary>
    /// <returns>可以射击</returns>
    private bool CheckBullet(out short bulletID)
    {
        bulletID = 9004;
        if (owner.isPlayer)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
            {
                bulletID = itemData.Item_Content.Item_ID;
                UseBullet();
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 消耗子弹
    /// </summary>
    private void UseBullet()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Content.Item_Count--;
        UpdateDataFromLocal(_newItem);
        if (owner.isPlayer && owner.isInput)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    /// <summary>
    /// 添加后坐力
    /// </summary>
    /// <param name="force"></param>
    private void AddForce(float force)
    {
        if (owner.isPlayer)
        {
            owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * force;
        }
        owner.NetManager.UpdateSeed();
    }
    /// <summary>
    /// 获得射击范围
    /// </summary>
    /// <returns></returns>
    private float GetAttackRange()
    {
        return Mathf.Lerp(config_aimMinRange, config_aimMaxRange, (temp_nextAimPoint - inputData.rightPressTimer) / config_shotAimTime);
    }
    /// <summary>
    /// 获得随机方向
    /// </summary>
    /// <param name="offset"></param>
    /// <returns></returns>
    private Vector3 GetRandomDir(float offset, int bullet)
    {
        UnityEngine.Random.InitState(bullet + itemData.Item_Seed);
        //获得随机偏转角
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, UnityEngine.Random.Range(0f, 1f));
        //Debug.Log(randomAngle);
        //Debug.Log(offset + "Left:" + inputData.leftPressTimer + "/Right:" + inputData.rightPressTimer + "/Face:" + inputData.mousePosition);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        return offsetVector;
    }
    #endregion
}
/// <summary>
/// 铁镐
/// </summary>
public class Item_2018 : ItemBase_Tool
{
    private GameObject itemLocalObj;
    #region//交互逻辑
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2018");
        itemLocalObj.transform.SetParent(body.Hand_RightItem);
        itemLocalObj.transform.localPosition = Vector3.zero;
        itemLocalObj.transform.localRotation = Quaternion.identity;
        itemLocalObj.transform.localScale = Vector3.one;
    }
    #endregion
    #region//使用逻辑
    private bool temp_attacking = false;
    private float temp_attackRadiu;

    private const short config_attackDamage = 7;
    private const float config_attackMaxRange = 30;
    private const float config_attackReadyTime = 0.5f;

    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= config_attackReadyTime)
            {
                temp_attacking = true;
                temp_attackRadiu = GetAttackRadiu();
                owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, (string str) =>
                {
                    if (input && str.Equals("Slash_Vertical"))
                    {
                        OnlyInput_Attack();
                    }
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public override bool UpdateRightPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.rightPressTimer == 0)
        {
            temp_attacking = false;
            owner.BodyController.SetHandTrigger("Slash_Vertical_Ready", 1 / config_attackReadyTime, null);
            owner.BodyController.SetHandBool("Slash_Vertical_Release", false, 1 / config_attackReadyTime, null);
            owner.BodyController.Animator_Hand.ResetTrigger("Slash_Vertical_Play");
        }
        inputData.rightPressTimer = pressTimer;
        if (player && input)
        {
            UpdateSkillSector();
        }
        return inputData.rightPressTimer > config_attackReadyTime;
    }
    public override void ReleaseRightPress(bool state, bool input, bool player)
    {
        if (owner)
        {
            if (inputData.rightPressTimer > 0)
            {
                inputData.rightPressTimer = 0;
                if (!temp_attacking)
                {
                    owner.BodyController.SetHandBool("Slash_Vertical_Release", true, inputData.rightPressTimer / config_attackReadyTime, null);
                }
            }
            if (player && input)
            {
                HideSkillSector();
            }
        }
        base.ReleaseRightPress(state, input, player);
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    private float GetAttackRadiu()
    {
        return Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / config_attackReadyTime);
    }
    private void UpdateSkillSector()
    {
        if (inputData.rightPressTimer < config_attackReadyTime)
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / config_attackReadyTime), config_attackMaxRange, 1);
        }
        else
        {
            owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, 1), config_attackMaxRange, 1);
        }
    }
    private void HideSkillSector()
    {
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
    }
    public void OnlyInput_Attack()
    {
        sbyte temp = 0;
        owner.SkillSector.Checkout_SIsector
            (inputData.mousePosition, temp_attackRadiu, config_attackMaxRange, out Transform[] targetTile);
        owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 1);
        for (int i = 0; i < targetTile.Length; i++)
        {
            if (targetTile[i].TryGetComponent(out ActorManager actor))
            {
                if (actor != owner)
                {
                    actor.AllClient_TakeDamage(config_attackDamage, owner.NetManager);
                    temp = -2;
                }
            }
            if (targetTile[i].TryGetComponent(out TileObj tile))
            {
                tile.TryToTakeDamage(config_attackDamage);
                temp = -2;
            }
        }
        Holding_ChangeDurability(temp);
    }
    #endregion
}

