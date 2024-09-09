using DG.Tweening;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem2000 
{
    
}
/// <summary>
/// 木斧头
/// </summary>
[Serializable]
public class Item_2001 : ItemBase_Tool
{
    private short attackDamage = 5;
    private bool attackState = false;
    private const float attackMaxRange = 120;
    private const float attackReadySpeed = 1;
    private const float attackReadyTime = 0.5f;
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= attackReadyTime)
            {
                attackState = true;
                if (input)
                {
                    owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, Attack);
                }
                else
                {
                    owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, null);
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !attackState)
        {
            if (!inputData.rightPressState)
            {
                inputData.rightPressState = true;
                owner.BodyController.SetHandTrigger("Slash_Vertical_Ready", 1 / attackReadyTime, null);
                owner.BodyController.SetHandBool("Slash_Vertical_Release", false, 1 / attackReadyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Slash_Vertical_Play");
            }
            if (inputData.rightPressTimer < attackReadyTime)
            {
                inputData.rightPressTimer += dt * attackReadySpeed;
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / attackReadyTime), attackMaxRange, 1);
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, 1), attackMaxRange, 1);
                }
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            attackState = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Slash_Vertical_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 1);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public void Attack(string name)
    {
        if (name == "Slash_Vertical")
        {
            sbyte temp = 0;
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / attackReadyTime), attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 1);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                        temp = -4;
                    }
                }
                if (targetTile[i].TryGetComponent(out TileObj tile))
                {
                    tile.TryToChangeHp(attackDamage);
                    temp = -4;
                }
            }
            Holding_ChangeDurability(temp);
        }
    }
}
/// <summary>
/// 铁斧头
/// </summary>
[Serializable]
public class Item_2002 : ItemBase_Tool
{
    private short attackDamage = 7;
    private bool attackState = false;
    private const float attackMaxRange = 120;
    private const float attackReadySpeed = 1.2f;
    private const float attackReadyTime = 0.5f;
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= attackReadyTime)
            {
                attackState = true;
                if (input)
                {
                    owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, Attack);
                }
                else
                {
                    owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, null);
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !attackState)
        {
            if (!inputData.rightPressState)
            {
                inputData.rightPressState = true;
                owner.BodyController.SetHandTrigger("Slash_Vertical_Ready", 1 / attackReadyTime, null);
                owner.BodyController.SetHandBool("Slash_Vertical_Release", false, 1 / attackReadyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Slash_Vertical_Play");
            }
            if (inputData.rightPressTimer < attackReadyTime)
            {
                inputData.rightPressTimer += dt * attackReadySpeed;
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / attackReadyTime), attackMaxRange, 1);
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, 1), attackMaxRange, 1);
                }
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            attackState = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Slash_Vertical_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 1);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public void Attack(string name)
    {
        if (name == "Slash_Vertical")
        {
            sbyte temp = 0;
            owner.SkillSector.Checkout_SIsector
                (inputData.mousePosition, Mathf.Lerp(0, itemConfig.Attack_Distance, inputData.rightPressTimer / attackReadyTime), attackMaxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 1);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attackDamage, owner.NetManager);
                        temp = -2;
                    }
                }
                if (targetTile[i].TryGetComponent(out TileObj tile))
                {
                    tile.TryToChangeHp(attackDamage);
                    temp = -2;
                }
            }
            Holding_ChangeDurability(temp);
        }
    }
}
/// <summary>
/// 粗制木弓
/// </summary>
[Serializable]
public class Item_2003 : ItemBase_Gun
{
    private bool attackState = false;
    private const float attackMaxRange = 120;
    private const float attackMinRange = 30;
    private const float attackReadySpeed = 1f;
    private const float attackReadyTime = 2;
    private const float attackAimTime = 2;
    private const float attackAimDistance = 1;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
        body.Hand_LeftItem.localRotation = Quaternion.Euler(0, 0, -45);
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }
    public override void Holding_UpdateLook()
    {
        if (owner)
        {
            if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Count > 0)
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
        base.Holding_UpdateLook();
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
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
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
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= attackReadyTime)
            {
                Shot(Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), input);
                inputData.rightPressTimer = 0;
                attackState = true;
                owner.BodyController.SetHandTrigger("Bow_Play", 1, null);
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0); ;
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !attackState)
        {
            if (!inputData.rightPressState)
            {
                inputData.rightPressState = true;
                owner.BodyController.SetHandTrigger("Bow_Ready", 1 / attackReadyTime, null);
                owner.BodyController.SetHandBool("Bow_Release", false, 1 / attackReadyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Bow_Play");
            }
            if (inputData.rightPressTimer < attackReadyTime + attackAimTime)
            {
                inputData.rightPressTimer += dt * attackReadySpeed;
                if (showSI)
                {
                    if (inputData.rightPressTimer > attackReadyTime)
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                }
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            attackState = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            attackState = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + itemData.Item_Content.Item_ID);
            obj.transform.position = owner.SkillSector.CenterPos;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 20, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                _newItem.Item_Content.Item_Count--;
                if (owner.isPlayer)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }
        }
    }
}
/// <summary>
/// 火把
/// </summary>
public class Item_2004 : ItemBase_Tool
{
    int timer = 0;
    GameObject obj;
    private short attackDamage = 2;
    private const float attackMaxRange = 120;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2004");
        obj.transform.SetParent(body.Hand_RightItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Horizontal", 1, Slash_Horizontal);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Horizontal", 1, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    private void Slash_Horizontal(string name)
    {
        if (name == "Slash_Horizontal")
        {
            sbyte temp = -1;
            RaycastHit2D[] hit2D = Physics2D.LinecastAll(obj.transform.position, obj.transform.position + inputData.mousePosition * itemConfig.Attack_Distance);
            for (int i = 0; i < hit2D.Length; i++)
            {
                if (hit2D[i].collider.CompareTag("Actor"))
                {
                    if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == owner) { continue; }
                        else
                        {
                            actor.TakeDamage(attackDamage, owner.NetManager);
                            temp = -5;
                        }
                    }
                }
            }
            Holding_ChangeDurability(temp);
        }
    }
    public override void UpdateTime(int second)
    {
        if (timer > 5)
        {
            timer = 0;
            Burn(-1);
        }
        else
        {
            timer += second;
        }
        base.UpdateTime(second);
    }
    public void Burn(sbyte offset)
    {
        Holding_ChangeDurability(offset);
    }
}
/// <summary>
/// 精制木弓
/// </summary>
public class Item_2005 : ItemBase_Gun
{
    private bool attackState = false;
    private const float attackMaxRange = 90;
    private const float attackMinRange = 15;
    private const float attackReadySpeed = 2f;
    private const float attackReadyTime = 1f;
    private const float attackAimTime = 1f;
    private const float attackAimDistance = 1.5f;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
        body.Hand_LeftItem.localRotation = Quaternion.Euler(0, 0, -45);
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }
    public override void Holding_UpdateLook()
    {
        if (owner)
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
        base.Holding_UpdateLook();
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
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
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
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= attackReadyTime)
            {
                Shot(Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), input);
                inputData.rightPressTimer = 0;
                attackState = true;
                owner.BodyController.SetHandTrigger("Bow_Play", 1, null);
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0); ;
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !attackState)
        {
            if (!inputData.rightPressState)
            {
                inputData.rightPressState = true;
                owner.BodyController.SetHandTrigger("Bow_Ready", 1 / attackReadyTime, null);
                owner.BodyController.SetHandBool("Bow_Release", false, 1 / attackReadyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Bow_Play");
            }
            if (inputData.rightPressTimer < attackReadyTime + attackAimTime)
            {
                inputData.rightPressTimer += dt * attackReadySpeed;
                if (showSI)
                {
                    if (inputData.rightPressTimer > attackReadyTime)
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                }
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            attackState = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            attackState = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + itemData.Item_Content.Item_ID);
            obj.transform.position = owner.SkillSector.CenterPos;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 20, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                _newItem.Item_Content.Item_Count--;
                if (owner.isPlayer)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }
        }
    }
}
/// <summary>
/// 木棍
/// </summary>
public class Item_2006 : ItemBase_Weapon
{
    private float speed = 10f;
    private short attackDamage = 2;
    private GameObject obj;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2006");
        obj.transform.SetParent(body.Hand_RightItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.Find("Hand").GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse.normalized;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (obj.transform.localPosition == Vector3.zero)
        {
            obj.transform.DOKill();
            obj.transform.DOLocalMoveX(0.5f, 1f / speed).SetLoops(2, LoopType.Yoyo).OnStepComplete(() =>
            {
                if (input)
                {
                    sbyte temp = 0;
                    RaycastHit2D[] hit2D = Physics2D.LinecastAll(obj.transform.position, obj.transform.position + inputData.mousePosition * itemConfig.Attack_Distance);
                    for (int i = 0; i < hit2D.Length; i++)
                    {
                        if (hit2D[i].collider.CompareTag("Actor"))
                        {
                            if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                            {
                                if (actor == owner) { continue; }
                                else
                                {
                                    actor.TakeDamage(attackDamage, owner.NetManager);
                                    temp = -2;
                                }
                            }
                        }
                    }
                    Holding_ChangeDurability(temp);
                }
            });
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
}
/// <summary>
/// 长柄刀
/// </summary>
public class Item_2007 : ItemBase_Weapon
{
    private float speed = 10f;
    private short attackDamage = 5;
    private GameObject obj;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2007");
        obj.transform.SetParent(body.Hand_RightItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.Find("Hand").GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse.normalized;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (obj.transform.localPosition == Vector3.zero)
        {
            obj.transform.DOKill();
            obj.transform.DOLocalMoveX(0.5f, 1f / speed).SetLoops(2, LoopType.Yoyo).OnStepComplete(() =>
            {
                if (input)
                {
                    sbyte temp = 0;
                    RaycastHit2D[] hit2D = Physics2D.LinecastAll(obj.transform.position, obj.transform.position + inputData.mousePosition * itemConfig.Attack_Distance);
                    for (int i = 0; i < hit2D.Length; i++)
                    {
                        if (hit2D[i].collider.CompareTag("Actor"))
                        {
                            if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                            {
                                if (actor == owner) { continue; }
                                else
                                {
                                    actor.TakeDamage(attackDamage, owner.NetManager);
                                    temp = -2;
                                }
                            }
                        }
                    }
                    Holding_ChangeDurability(temp);
                }
            });
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
}
/// <summary>
/// 短柄刀
/// </summary>
public class Item_2008 : ItemBase_Weapon
{
    GameObject obj;
    private short attackDamage = 2;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2008");
        obj.transform.SetParent(body.Hand_RightItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            owner.BodyController.SetHandTrigger("Slash_Horizontal", 1, Slash_Horizontal);
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Slash_Horizontal(string name)
    {
        if (name == "Slash_Horizontal")
        {
            sbyte temp = 0;
            RaycastHit2D[] hit2D = Physics2D.LinecastAll(obj.transform.position, obj.transform.position + inputData.mousePosition * itemConfig.Attack_Distance);
            for (int i = 0; i < hit2D.Length; i++)
            {
                if (hit2D[i].collider.CompareTag("Actor"))
                {
                    if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == owner) { continue; }
                        else
                        {
                            actor.TakeDamage(attackDamage, owner.NetManager);
                            temp = -3;
                        }
                    }
                }
            }
            Holding_ChangeDurability(temp);
        }
    }
}
/// <summary>
/// 短筒枪
/// </summary>
public class Item_2009 : ItemBase_Gun
{
    private const float attackMaxRange = 50;
    private const float attackMinRange = 20;
    private const float attackReadySpeed = 1;
    private const float attackReadyTime = 1f;
    private const float attackAimTime = 1f;
    private const float attackAimDistance = 1f;

    private ItemLocalObj_2009 itemLocalObj_2009;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2009 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2009").GetComponent<ItemLocalObj_2009>();
        itemLocalObj_2009.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2009.transform.localPosition = Vector3.zero;
        itemLocalObj_2009.transform.localScale = Vector3.one;

        itemLocalObj_2009.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;

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
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            if (putInItemConfig.Item_ID == 9004)
            {
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, 7, out newItemData, out residueItem);
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
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= attackReadyTime)
            {
                Shot(Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), input);
                inputData.rightPressTimer = 0;
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (!inputData.rightPressState)
            {
                inputData.rightPressState = true;
                owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / attackReadyTime, null);
                owner.BodyController.SetHandBool("Shoot_Release", false, 1 / attackReadyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
            }
            if (inputData.rightPressTimer < attackReadyTime + attackAimTime)
            {
                inputData.rightPressTimer += dt * attackReadySpeed;
                if (showSI)
                {
                    if (inputData.rightPressTimer > attackReadyTime)
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                }
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }

    public override void InputMousePos(Vector3 mouse, float time)
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

        base.InputMousePos(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            AddForce(2);
            itemLocalObj_2009.Shoot();
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + itemData.Item_Content.Item_ID);
            obj.transform.position = owner.SkillSector.CenterPos;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 10, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                _newItem.Item_Content.Item_Count--;
                if (owner.isPlayer)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }
        }
        else
        {
            itemLocalObj_2009.Dull();
        }
    }
    private void AddForce(float force)
    {
        owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * force;
        owner.NetManager.UpdateSeed();
    }
}
/// <summary>
/// 黄金弓
/// </summary>
public class Item_2010 : ItemBase_Gun
{
    private bool attackState = false;
    private const float attackMaxRange = 90;
    private const float attackMinRange = 15;
    private const float attackReadySpeed = 2f;
    private const float attackReadyTime = 1f;
    private const float attackAimTime = 1f;
    private const float attackAimDistance = 1.5f;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemData.Item_ID);
        body.Hand_LeftItem.localRotation = Quaternion.Euler(0, 0, -45);
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }
    public override void Holding_UpdateLook()
    {
        if (owner)
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
        base.Holding_UpdateLook();
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
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
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
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= attackReadyTime)
            {
                Shot(Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), input);
                inputData.rightPressTimer = 0;
                attackState = true;
                owner.BodyController.SetHandTrigger("Bow_Play", 1, null);
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0); ;
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !attackState)
        {
            if (!inputData.rightPressState)
            {
                inputData.rightPressState = true;
                owner.BodyController.SetHandTrigger("Bow_Ready", 1 / attackReadyTime, null);
                owner.BodyController.SetHandBool("Bow_Release", false, 1 / attackReadyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Bow_Play");
            }
            if (inputData.rightPressTimer < attackReadyTime + attackAimTime)
            {
                inputData.rightPressTimer += dt * attackReadySpeed;
                if (showSI)
                {
                    if (inputData.rightPressTimer > attackReadyTime)
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                }
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            attackState = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            attackState = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + itemData.Item_Content.Item_ID);
            obj.transform.position = owner.SkillSector.CenterPos;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 20, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                _newItem.Item_Content.Item_Count--;
                if (owner.isPlayer)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }
        }
    }

}
/// <summary>
/// 宽刃钢刀
/// </summary>
public class Item_2011 : ItemBase_Weapon
{
    GameObject obj;
    private short attackDamage = 3;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2011");
        obj.transform.SetParent(body.Hand_RightItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }
    public override void InputMousePos(Vector3 mouse, float time)
    {
        inputData.mousePosition = mouse;
        base.InputMousePos(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            owner.BodyController.SetHandTrigger("Slash_Horizontal", 1, Slash_Horizontal);
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Slash_Horizontal(string name)
    {
        if (name == "Slash_Horizontal")
        {
            sbyte temp = 0;
            RaycastHit2D[] hit2D = Physics2D.LinecastAll(obj.transform.position, obj.transform.position + inputData.mousePosition * itemConfig.Attack_Distance);
            for (int i = 0; i < hit2D.Length; i++)
            {
                if (hit2D[i].collider.CompareTag("Actor"))
                {
                    if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                    {
                        if (actor == owner) { continue; }
                        else
                        {
                            actor.TakeDamage(attackDamage, owner.NetManager);
                            temp = -2;
                        }
                    }
                }
            }
            Holding_ChangeDurability(temp);
        }
    }

}
/// <summary>
/// 速射枪
/// </summary>
public class Item_2012 : ItemBase_Gun
{
    private bool alreadyShot = false;
    private const float attackAimDistance = 1f;
    private const float attackMaxRange = 60;
    private const float attackMinRange = 10;
    private const float attackReadySpeed = 1;
    private const float attackReadyTime = 1f;
    private const float attackAimTime = 1f;
    private const float shootCD = 0.1f;
    private ItemLocalObj_2012 itemLocalObj_2012;

    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2012 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2012").GetComponent<ItemLocalObj_2012>();
        itemLocalObj_2012.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2012.transform.localPosition = Vector3.zero;
        itemLocalObj_2012.transform.localScale = Vector3.one;
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
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            if (putInItemConfig.Item_ID == 9004)
            {
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, 30, out newItemData, out residueItem);
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
    //public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    //{
    //    if (owner)
    //    {
    //        if (rightPressTimer >= readyTime)
    //        {
    //            Shot(Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), input);
    //            rightPressTimer = 0;
    //            alreadyShot = true;
    //            owner.BodyController.SetHandTrigger("Shoot_Play", 1, null);
    //            if (showSI)
    //            {
    //                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0); ;
    //            }
    //        }
    //    }
    //    base.ClickLeftClick(dt, state, input, showSI);
    //}

    public override bool PressLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= attackReadyTime)
            {
                inputData.leftPressTimer += dt;
                if (inputData.leftPressTimer >= shootCD)
                {
                    inputData.leftPressTimer = 0;
                    Shot(Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), input);
                    if (inputData.rightPressTimer >= attackReadyTime + 0.2f)
                    {
                        inputData.rightPressTimer -= 0.2f;
                    }
                }
            }
        }
        return base.PressLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !alreadyShot)
        {
            if (!inputData.rightPressState)
            {
                inputData.rightPressState = true;
                owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / attackReadyTime, null);
                owner.BodyController.SetHandBool("Shoot_Release", false, 1 / attackReadyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
            }
            if (inputData.rightPressTimer < attackReadyTime + attackAimTime)
            {
                inputData.rightPressTimer += dt * attackReadySpeed;
                if (showSI)
                {
                    if (inputData.rightPressTimer > attackReadyTime)
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                }
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            alreadyShot = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }



    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            alreadyShot = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }

    public override void InputMousePos(Vector3 mouse, float time)
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

        base.InputMousePos(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * 2;
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            itemLocalObj_2012.Shoot();
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + itemData.Item_Content.Item_ID);
            obj.transform.position = itemLocalObj_2012.muzzle.position;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 10, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                _newItem.Item_Content.Item_Count--;
                if (owner.isPlayer)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }
        }
        else
        {
            itemLocalObj_2012.Dull();
        }
    }

}
/// <summary>
/// 双手步枪
/// </summary>
public class Item_2013 : ItemBase_Gun
{
    private bool alreadyShot = false;
    private const float attackAimDistance = 2f;
    private const float attackMaxRange = 60;
    private const float attackMinRange = 7.5f;
    private const float attackReadySpeed = 1f;
    private const float attackReadyTime = 0.25f;
    private const float attackAimTime = 1f;
    private const float shootCD = 0.15f;
    private ItemLocalObj_2013 itemLocalObj_2013;
    public override void Holding_Start(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2013 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2013").GetComponent<ItemLocalObj_2013>();
        itemLocalObj_2013.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2013.transform.localPosition = Vector3.zero;
        itemLocalObj_2013.transform.localScale = Vector3.one;
        itemLocalObj_2013.rightHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Right.GetComponent<SpriteRenderer>().sprite;
        itemLocalObj_2013.leftHand.GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
        body.Hand_Right.GetComponent<SpriteRenderer>().enabled = false;
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
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            if (putInItemConfig.Item_ID == 9004)
            {
                Type type = Type.GetType("Item_" + putInItemData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_FillUp(itemData, putInItemData, 30, out newItemData, out residueItem);
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

    public override bool PressLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (inputData.rightPressTimer >= attackReadyTime)
            {
                inputData.leftPressTimer += dt;
                if (inputData.leftPressTimer >= shootCD)
                {
                    inputData.leftPressTimer = 0;
                    Shot(Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), input);
                    if (inputData.rightPressTimer >= attackReadyTime + 0.2f)
                    {
                        inputData.rightPressTimer -= 0.2f;
                    }
                }
            }
        }
        return base.PressLeftClick(dt, state, input, showSI);
    }

    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !alreadyShot)
        {
            if (!inputData.rightPressState)
            {
                inputData.rightPressState = true;
                owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / attackReadyTime, null);
                owner.BodyController.SetHandBool("Shoot_Release", false, 1 / attackReadyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
            }
            if (inputData.rightPressTimer < attackReadyTime + attackAimTime)
            {
                inputData.rightPressTimer += dt * attackReadySpeed;
                if (showSI)
                {
                    if (inputData.rightPressTimer > attackReadyTime)
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(inputData.mousePosition, attackAimDistance, Mathf.Lerp(attackMaxRange, attackMinRange, (inputData.rightPressTimer - attackReadyTime) / attackAimTime), 1);
                }
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            alreadyShot = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }



    public override void Holding_Over(ActorManager who)
    {
        if (owner)
        {
            inputData.rightPressTimer = 0;
            alreadyShot = false;
            if (inputData.rightPressState)
            {
                inputData.rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, inputData.rightPressTimer / attackReadyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(inputData.mousePosition, 0, 0, 0);
            }
        }
        base.Holding_Over(who);
    }

    public override void InputMousePos(Vector3 mouse, float time)
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

        base.InputMousePos(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.networkRigidbody.Rigidbody.velocity = -inputData.mousePosition.normalized * 2;
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (inputData.mousePosition.normalized);
        if (itemData.Item_Content.Item_ID != 0 && itemData.Item_Content.Item_Count > 0)
        {
            itemLocalObj_2013.Shoot();
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + itemData.Item_Content.Item_ID);
            //obj.transform.position = owner.SkillSector.CenterPos;
            obj.transform.position = itemLocalObj_2013.muzzle.position;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 10, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = itemData;
                ItemData _newItem = itemData;
                _newItem.Item_Content.Item_Count--;
                if (owner.isPlayer)
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                    {
                        oldItem = _oldItem,
                        newItem = _newItem,
                    });
                }
            }
        }
        else
        {
            itemLocalObj_2013.Dull();
        }
    }

}
