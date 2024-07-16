using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UniRx;
using static UnityEngine.UI.GridLayoutGroup;
using Random = UnityEngine.Random;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public partial class ItemSystem
{
}
/// <summary>
/// 原初树枝
/// </summary>
[Serializable]
public class Item_0 : ItemBase
{
}
/// <summary>
/// 原木
/// </summary>
[Serializable]
public class Item_1001 : ItemBase
{
    private int attack = 5;
    private const float maxDistance = 1;
    private const float maxRange = 120;

    private Vector3 rightPosition = Vector3.zero;
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.text_Info.text = data.Item_Count.ToString();
        base.DrawGridCell(gridCell);
    }
    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        base.FaceTo(mouse, time);
    }

    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Slash_Vertical);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Slash_Vertical(string name)
    {
        if (name == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (rightPosition, maxDistance, maxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attack, owner.NetManager);
                    }
                }
                if (targetTile[i].TryGetComponent(out TileObj tile))
                {
                    tile.TryToChangeHp(attack);
                }
            }
        }
    }
}
/// <summary>
/// 木斧头
/// </summary>
[Serializable]
public class Item_2001 : ItemBase
{
    /// <summary>
    /// 右键按压状态
    /// </summary>
    private bool rightPressState = false;
    /// <summary>
    /// 右键当前位置
    /// </summary>
    private Vector3 rightPosition = Vector3.zero;
    /// <summary>
    /// 右键按压时长
    /// </summary>
    private float rightPressTimer = 0;
    private bool alreadyAttack = false;
    private const float maxRange = 120;
    private const float readySpeed = 1;
    private const float readyTime = 0.5f;
    private int attack = 5;
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.text_Info.text = data.Item_Val.ToString() + "%";
        base.DrawGridCell(gridCell);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if(rightPressTimer >= readyTime)
            {
                alreadyAttack = true;
                if (input)
                {
                    owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, Slash_Vertical);
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
        if (owner && !alreadyAttack)
        {
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.SetHandTrigger("Slash_Vertical_Ready", 1 / readyTime, null);
                owner.BodyController.SetHandBool("Slash_Vertical_Release", false, 1 / readyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Slash_Vertical_Play");
            }
            if (rightPressTimer < readyTime)
            {
                rightPressTimer += dt * readySpeed;
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, Mathf.Lerp(0, config.Attack_Distance, rightPressTimer / readyTime), maxRange, 1);
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyAttack = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Slash_Vertical_Release", true, rightPressTimer / readyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 1);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        base.FaceTo(mouse, time);
    }
    private void Slash_Vertical(string name)
    {
        if (name == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (rightPosition, Mathf.Lerp(0, config.Attack_Distance, rightPressTimer / readyTime), maxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 1);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attack, owner.NetManager);
                    }
                }
                if (targetTile[i].TryGetComponent(out TileObj tile))
                {
                    tile.TryToChangeHp(attack);
                }       
            }
        }
    }
}
/// <summary>
/// 铁斧头
/// </summary>
[Serializable]
public class Item_2002 : ItemBase
{
    /// <summary>
    /// 右键按压状态
    /// </summary>
    private bool rightPressState = false;
    /// <summary>
    /// 右键当前位置
    /// </summary>
    private Vector3 rightPosition = Vector3.zero;
    /// <summary>
    /// 右键按压时长
    /// </summary>
    private float rightPressTimer = 0;
    private bool alreadyAttack = false;
    private const float maxRange = 120;
    private const float readySpeed = 1;
    private const float readyTime = 0.5f;
    private int attack = 10;
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.text_Info.text = data.Item_Val.ToString() + "%";
        base.DrawGridCell(gridCell);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (rightPressTimer >= readyTime)
            {
                alreadyAttack = true;
                if (input)
                {
                    owner.BodyController.SetHandTrigger("Slash_Vertical_Play", 1, Slash_Vertical);
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
        if (owner && !alreadyAttack)
        {
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.SetHandTrigger("Slash_Vertical_Ready", 1 / readyTime, null);
                owner.BodyController.SetHandBool("Slash_Vertical_Release", false, 1 / readyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Slash_Vertical_Play");
            }
            if (rightPressTimer < readyTime)
            {
                rightPressTimer += dt * readySpeed;
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, Mathf.Lerp(0, config.Attack_Distance, rightPressTimer / readyTime), maxRange, 1);
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;
    }
    public override void ReleaseRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyAttack = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Slash_Vertical_Release", true, rightPressTimer / readyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        base.FaceTo(mouse, time);
    }
    private void Slash_Vertical(string name)
    {
        if (name == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (rightPosition, Mathf.Lerp(0, config.Attack_Distance, rightPressTimer / readyTime), maxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attack, owner.NetManager);
                    }
                }
                if (targetTile[i].TryGetComponent(out TileObj tile))
                {
                    tile.TryToChangeHp(attack);
                }
            }
        }
    }
}
/// <summary>
/// 粗制木弓
/// </summary>
[Serializable]
public class Item_2003 : ItemBase
{
    #region//操作变量
    /// <summary>
    /// 右键按压状态
    /// </summary>
    private bool rightPressState = false;
    /// <summary>
    /// 右键当前位置
    /// </summary>
    private Vector3 rightPosition = Vector3.zero;
    /// <summary>
    /// 右键按压时长
    /// </summary>
    private float rightPressTimer = 0;
    /// <summary>
    /// 已经攻击
    /// </summary>
    private bool alreadyShot = false;
    /// <summary>
    /// 瞄准距离
    /// </summary>
    private const float aimDistance = 1;
    /// <summary>
    /// 最大角度
    /// </summary>
    private const float maxAngleRange = 120;
    /// <summary>
    /// 最小角度
    /// </summary>
    private const float minAngleRange = 30;
    /// <summary>
    /// 拉弓速度
    /// </summary>
    private const float readySpeed = 1;
    /// <summary>
    /// 拉弓时长
    /// </summary>
    private const float readyTime = 2;
    /// <summary>
    /// 瞄准时长
    /// </summary>
    private const float aimTime = 2;
    #endregion
    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
        body.Hand_LeftItem.localRotation = Quaternion.Euler(0, 0, -45);
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 3;
        if (data.Item_Val != 0 && data.Item_Count > 0)
        {
            body.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
            body.Hand_RightItem.localRotation = Quaternion.Euler(0, 0, -45);
            body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_Val);
        }
        else
        {
            body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        }
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + config.Item_ID.ToString());
        if (data.Item_Val != 0)
        {
            gridCell.text_Info.text = data.Item_Count.ToString();
        }
        else
        {
            gridCell.text_Info.text = "";
        }
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid.Open(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        if (data.Item_Val == 0 || data.Item_Val == putInItemData.Item_ID)
        {
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            ItemData cannotPutInItemData = new ItemData();
            cannotPutInItemData.Item_ID = putInItemData.Item_ID;
            cannotPutInItemData.Item_Val = putInItemData.Item_Val;
            /*只能放入木箭*/
            if (putInItemConfig.Item_ID == 9002 || putInItemConfig.Item_ID == 9003)
            {
                /*当容器为空时，数量重新计算*/
                if (data.Item_Val == 0)
                {
                    if (data.Item_Count + putInItemData.Item_Count - 1 > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - 1 - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }

                }
                /*当容器不为空时，数量叠加*/
                else
                {
                    if (data.Item_Count + putInItemData.Item_Count > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }
                }
                return cannotPutInItemData;
            }
            else
            {
                return putInItemData;
            }
        }
        else
        {
            Debug.Log("放入失败");
            return putInItemData;
        }
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (rightPressTimer >= readyTime)
            {
                Shot(Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), input);
                rightPressTimer = 0;
                alreadyShot = true;
                owner.BodyController.SetHandTrigger("Bow_Play", 1, null);
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0); ;
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !alreadyShot)
        {
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.SetHandTrigger("Bow_Ready", 1 / readyTime, null);
                owner.BodyController.SetHandBool("Bow_Release", false, 1 / readyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Bow_Play");
            }
            if (rightPressTimer < readyTime + aimTime)
            {
                rightPressTimer += dt * readySpeed;
                if (showSI)
                {
                    if (rightPressTimer > readyTime)
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
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
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, rightPressTimer / readyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void BeRelease(ActorManager who)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, rightPressTimer / readyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.BeRelease(who);
    }
    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        base.FaceTo(mouse, time);
    }
    private void Shot(float offset,bool inputState)
    {
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (rightPosition.normalized);
        if (data.Item_Val != 0 && data.Item_Count > 0)
        {
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + data.Item_Val);
            obj.transform.position = owner.SkillSector.CenterPos;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 10, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = data;
                ItemData _newItem = data;
                _newItem.Item_Count--;
                if(_newItem.Item_Count == 0)
                {
                    _newItem.Item_Val = 0;
                    _newItem.Item_Count = 1;
                }
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
[SerializeField]
public class Item_2004 : ItemBase
{
    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        GameObject obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2004");
        obj.transform.SetParent(body.Hand_LeftItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + config.Item_ID.ToString());
    }

}
/// <summary>
/// 精制木弓
/// </summary>
[Serializable]
public class Item_2005 : ItemBase
{
    #region//操作变量
    /// <summary>
    /// 右键按压状态
    /// </summary>
    private bool rightPressState = false;
    /// <summary>
    /// 右键当前位置
    /// </summary>
    private Vector3 rightPosition = Vector3.zero;
    /// <summary>
    /// 右键按压时长
    /// </summary>
    private float rightPressTimer = 0;
    /// <summary>
    /// 已经攻击
    /// </summary>
    private bool alreadyShot = false;
    /// <summary>
    /// 瞄准距离
    /// </summary>
    private const float aimDistance = 1.5f;
    /// <summary>
    /// 最大角度
    /// </summary>
    private const float maxAngleRange = 90;
    /// <summary>
    /// 最小角度
    /// </summary>
    private const float minAngleRange = 15;
    /// <summary>
    /// 拉弓速度
    /// </summary>
    private const float readySpeed = 1;
    /// <summary>
    /// 拉弓时长
    /// </summary>
    private const float readyTime = 0.5f;
    /// <summary>
    /// 瞄准时长
    /// </summary>
    private const float aimTime = 1;
    #endregion
    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID);
        body.Hand_LeftItem.localRotation = Quaternion.Euler(0, 0, -45);
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sortingOrder = 3;
        if (data.Item_Val != 0 && data.Item_Count > 0)
        {
            body.Hand_RightItem.localPosition = new Vector3(0.5f, 0, 0);
            body.Hand_RightItem.localRotation = Quaternion.Euler(0, 0, -45);
            body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_Val);
        }
        else
        {
            body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
                = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
        }
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + config.Item_ID.ToString());
        if (data.Item_Val != 0)
        {
            gridCell.text_Info.text = data.Item_Count.ToString();
        }
        else
        {
            gridCell.text_Info.text = "";
        }
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid.Open(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        if (data.Item_Val == 0 || data.Item_Val == putInItemData.Item_ID)
        {
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            ItemData cannotPutInItemData = new ItemData();
            cannotPutInItemData.Item_ID = putInItemData.Item_ID;
            cannotPutInItemData.Item_Val = putInItemData.Item_Val;
            /*只能放入木箭*/
            if (putInItemConfig.Item_ID == 9002 || putInItemConfig.Item_ID == 9003)
            {
                /*当容器为空时，数量重新计算*/
                if (data.Item_Val == 0)
                {
                    if (data.Item_Count + putInItemData.Item_Count - 1 > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - 1 - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }

                }
                /*当容器不为空时，数量叠加*/
                else
                {
                    if (data.Item_Count + putInItemData.Item_Count > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }
                }
                return cannotPutInItemData;
            }
            else
            {
                return putInItemData;
            }
        }
        else
        {
            Debug.Log("放入失败");
            return putInItemData;
        }
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (rightPressTimer >= readyTime)
            {
                Shot(Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), input);
                rightPressTimer = 0;
                alreadyShot = true;
                owner.BodyController.SetHandTrigger("Bow_Play", 1, null);
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0); ;
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !alreadyShot)
        {
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.SetHandTrigger("Bow_Ready", 1 / readyTime, null);
                owner.BodyController.SetHandBool("Bow_Release", false, 1 / readyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Bow_Play");
            }
            if (rightPressTimer < readyTime + aimTime)
            {
                rightPressTimer += dt * readySpeed;
                if (showSI)
                {
                    if (rightPressTimer > readyTime)
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
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
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, rightPressTimer / readyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void BeRelease(ActorManager who)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Bow_Release", true, rightPressTimer / readyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.BeRelease(who);
    }

    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        base.FaceTo(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (rightPosition.normalized);
        if (data.Item_Val != 0 && data.Item_Count > 0)
        {
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + data.Item_Val);
            obj.transform.position = owner.SkillSector.CenterPos;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 10, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = data;
                ItemData _newItem = data;
                _newItem.Item_Count--;
                if (_newItem.Item_Count == 0)
                {
                    _newItem.Item_Val = 0;
                    _newItem.Item_Count = 1;
                }
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
[Serializable]
public class Item_2006 : ItemBase
{
    Vector3 dir;
    GameObject obj;
    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2006");
        obj.transform.SetParent(body.Hand_RightItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.Find("Hand").GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void FaceTo(Vector3 mouse, float time)
    {
        dir = mouse.normalized;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }
        base.FaceTo(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (obj.transform.localPosition == Vector3.zero)
        {
            obj.transform.DOKill();
            obj.transform.DOLocalMoveX(0.5f, 0.5f).SetLoops(2, LoopType.Yoyo).OnStepComplete(() =>
            {
                RaycastHit2D[] hit2D = Physics2D.LinecastAll(obj.transform.position, obj.transform.position + dir * config.Attack_Distance);
                for (int i = 0; i < hit2D.Length; i++)
                {
                    if (hit2D[i].collider.CompareTag("Actor"))
                    {
                        if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                        {
                            if (actor == owner) { continue; }
                            else
                            {
                                actor.TakeDamage(1, owner.NetManager);
                            }
                        }
                    }
                    else
                    {
                    }
                }
            });
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }

}
/// <summary>
/// 长柄刀
/// </summary>
[Serializable]
public class Item_2007 : ItemBase
{
    Vector3 dir;
    GameObject obj;
    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2007");
        obj.transform.SetParent(body.Hand_RightItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.Find("Hand").GetComponent<SpriteRenderer>().sprite = body.Hand_Left.GetComponent<SpriteRenderer>().sprite;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void FaceTo(Vector3 mouse, float time)
    {
        dir = mouse.normalized;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }
        base.FaceTo(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if(obj.transform.localPosition == Vector3.zero)
        {
            obj.transform.DOKill();
            obj.transform.DOLocalMoveX(0.5f, 0.2f).SetLoops(2,LoopType.Yoyo).OnStepComplete(() =>
            {
                RaycastHit2D[] hit2D = Physics2D.LinecastAll(obj.transform.position, obj.transform.position + dir * config.Attack_Distance);
                for (int i = 0; i < hit2D.Length; i++)
                {
                    if (hit2D[i].collider.CompareTag("Actor"))
                    {
                        if (hit2D[i].transform.TryGetComponent(out ActorManager actor))
                        {
                            if (actor == owner) { continue; }
                            else
                            {
                                actor.TakeDamage(2, owner.NetManager);
                            }
                        }
                    }
                    else
                    {
                    }
                }
            });
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
}
/// <summary>
/// 短柄刀
/// </summary>
[Serializable]
public class Item_2008 : ItemBase
{
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            owner.BodyController.SetHandTrigger("Slash_Horizontal", 1, Slash_Vertical);
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Slash_Vertical(string name)
    {

    }
}
/// <summary>
/// 短筒枪
/// </summary>
[Serializable]
public class Item_2009 : ItemBase
{
    #region//操作变量
    /// <summary>
    /// 右键按压状态
    /// </summary>
    private bool rightPressState = false;
    /// <summary>
    /// 右键当前位置
    /// </summary>
    private Vector3 rightPosition = Vector3.zero;
    /// <summary>
    /// 右键按压时长
    /// </summary>
    private float rightPressTimer = 0;
    /// <summary>
    /// 已经攻击
    /// </summary>
    private bool alreadyShot = false;
    /// <summary>
    /// 瞄准距离
    /// </summary>
    private const float aimDistance = 1f;
    /// <summary>
    /// 最大角度
    /// </summary>
    private const float maxAngleRange = 50;
    /// <summary>
    /// 最小角度
    /// </summary>
    private const float minAngleRange = 20;
    /// <summary>
    /// 准备速度
    /// </summary>
    private const float readySpeed = 1;
    /// <summary>
    /// 准备时长
    /// </summary>
    private const float readyTime = 1f;
    /// <summary>
    /// 瞄准时长
    /// </summary>
    private const float aimTime = 1f;
    #endregion

    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        GameObject obj = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2009");
        obj.transform.SetParent(body.Hand_RightItem);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }

    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + config.Item_ID.ToString());
        if (data.Item_Val != 0)
        {
            gridCell.text_Info.text = data.Item_Count.ToString();
        }
        else
        {
            gridCell.text_Info.text = "";
        }
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid.Open(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        if (data.Item_Val == 0 || data.Item_Val == putInItemData.Item_ID)
        {
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            ItemData cannotPutInItemData = new ItemData();
            cannotPutInItemData.Item_ID = putInItemData.Item_ID;
            cannotPutInItemData.Item_Val = putInItemData.Item_Val;
            /*只能放入木箭*/
            if (putInItemConfig.Item_ID == 9004)
            {
                /*当容器为空时，数量重新计算*/
                if (data.Item_Val == 0)
                {
                    if (data.Item_Count + putInItemData.Item_Count - 1 > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - 1 - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }

                }
                /*当容器不为空时，数量叠加*/
                else
                {
                    if (data.Item_Count + putInItemData.Item_Count > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }
                }
                return cannotPutInItemData;
            }
            else
            {
                return putInItemData;
            }
        }
        else
        {
            Debug.Log("放入失败");
            return putInItemData;
        }
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (rightPressTimer >= readyTime)
            {
                Shot(Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), input);
                rightPressTimer = 0;
                alreadyShot = true;
                owner.BodyController.SetHandTrigger("Shoot_Play", 1, null);
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0); ;
                }
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    public override bool PressRightClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner && !alreadyShot)
        {
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / readyTime, null);
                owner.BodyController.SetHandBool("Shoot_Release", false, 1 / readyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
            }
            if (rightPressTimer < readyTime + aimTime)
            {
                rightPressTimer += dt * readySpeed;
                if (showSI)
                {
                    if (rightPressTimer > readyTime)
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
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
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, rightPressTimer / readyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }
    public override void BeRelease(ActorManager who)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, rightPressTimer / readyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.BeRelease(who);
    }

    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.FaceTo(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.networkRigidbody.Rigidbody.velocity = -rightPosition.normalized * 2;
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (rightPosition.normalized);
        if (data.Item_Val != 0 && data.Item_Count > 0)
        {
            Debug.Log(data.Item_Val);
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + data.Item_Val);
            obj.transform.position = owner.SkillSector.CenterPos;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 10, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = data;
                ItemData _newItem = data;
                _newItem.Item_Count--;
                if (_newItem.Item_Count == 0)
                {
                    _newItem.Item_Val = 0;
                    _newItem.Item_Count = 1;
                }
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
/// 钢弓
/// </summary>
public class Item_2010 : ItemBase
{

}
/// <summary>
/// 宽刃钢刀
/// </summary>
public class Item_2011 : ItemBase
{

}
/// <summary>
/// 速射枪
/// </summary>
public class Item_2012 : ItemBase
{
    #region//操作变量
    /// <summary>
    /// 右键按压状态
    /// </summary>
    private bool rightPressState = false;
    /// <summary>
    /// 右键当前位置
    /// </summary>
    private Vector3 rightPosition = Vector3.zero;
    /// <summary>
    /// 右键按压时长
    /// </summary>
    private float rightPressTimer = 0;
    /// <summary>
    /// 左键按压时长
    /// </summary>
    private float leftPressTimer = 0;

    /// <summary>
    /// 已经攻击
    /// </summary>
    private bool alreadyShot = false;
    /// <summary>
    /// 瞄准距离
    /// </summary>
    private const float aimDistance = 1f;
    /// <summary>
    /// 最大角度
    /// </summary>
    private const float maxAngleRange = 60;
    /// <summary>
    /// 最小角度
    /// </summary>
    private const float minAngleRange = 10;
    /// <summary>
    /// 准备速度
    /// </summary>
    private const float readySpeed = 1;
    /// <summary>
    /// 准备时长
    /// </summary>
    private const float readyTime = 1f;
    /// <summary>
    /// 瞄准时长
    /// </summary>
    private const float aimTime = 1f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float shootCD = 0.1f;
    private ItemLocalObj_2012 itemLocalObj_2012;
    #endregion

    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2012 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2012").GetComponent<ItemLocalObj_2012>();
        itemLocalObj_2012.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2012.transform.localPosition = Vector3.zero;
        itemLocalObj_2012.transform.localScale = Vector3.one;
    }

    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + config.Item_ID.ToString());
        if (data.Item_Val != 0)
        {
            gridCell.text_Info.text = data.Item_Count.ToString();
        }
        else
        {
            gridCell.text_Info.text = "";
        }
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid.Open(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        if (data.Item_Val == 0 || data.Item_Val == putInItemData.Item_ID)
        {
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            ItemData cannotPutInItemData = new ItemData();
            cannotPutInItemData.Item_ID = putInItemData.Item_ID;
            cannotPutInItemData.Item_Val = putInItemData.Item_Val;
            /*只能放入木箭*/
            if (putInItemConfig.Item_ID == 9004)
            {
                /*当容器为空时，数量重新计算*/
                if (data.Item_Val == 0)
                {
                    if (data.Item_Count + putInItemData.Item_Count - 1 > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - 1 - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }

                }
                /*当容器不为空时，数量叠加*/
                else
                {
                    if (data.Item_Count + putInItemData.Item_Count > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }
                }
                return cannotPutInItemData;
            }
            else
            {
                return putInItemData;
            }
        }
        else
        {
            Debug.Log("放入失败");
            return putInItemData;
        }
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
            if (rightPressTimer >= readyTime)
            {
                leftPressTimer += dt;
                if (leftPressTimer >= shootCD)
                {
                    leftPressTimer = 0;
                    itemLocalObj_2012.Shoot();
                    Shot(Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), input);
                    if (rightPressTimer >= readyTime + 0.2f)
                    {
                        rightPressTimer -= 0.2f;
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
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / readyTime, null);
                owner.BodyController.SetHandBool("Shoot_Release", false, 1 / readyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
            }
            if (rightPressTimer < readyTime + aimTime)
            {
                rightPressTimer += dt * readySpeed;
                if (showSI)
                {
                    if (rightPressTimer > readyTime)
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
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
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, rightPressTimer / readyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }



    public override void BeRelease(ActorManager who)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, rightPressTimer / readyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.BeRelease(who);
    }

    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.FaceTo(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.networkRigidbody.Rigidbody.velocity = -rightPosition.normalized * 2;
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (rightPosition.normalized);
        if (data.Item_Val != 0 && data.Item_Count > 0)
        {
            Debug.Log(data.Item_Val);
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + data.Item_Val);
            //obj.transform.position = owner.SkillSector.CenterPos;
            obj.transform.position = itemLocalObj_2012.muzzle.position;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 10, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = data;
                ItemData _newItem = data;
                _newItem.Item_Count--;
                if (_newItem.Item_Count == 0)
                {
                    _newItem.Item_Val = 0;
                    _newItem.Item_Count = 1;
                }
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
/// 双手步枪
/// </summary>
public class Item_2013 : ItemBase
{
    #region//操作变量
    /// <summary>
    /// 右键按压状态
    /// </summary>
    private bool rightPressState = false;
    /// <summary>
    /// 右键当前位置
    /// </summary>
    private Vector3 rightPosition = Vector3.zero;
    /// <summary>
    /// 右键按压时长
    /// </summary>
    private float rightPressTimer = 0;
    /// <summary>
    /// 左键按压时长
    /// </summary>
    private float leftPressTimer = 0;

    /// <summary>
    /// 已经攻击
    /// </summary>
    private bool alreadyShot = false;
    /// <summary>
    /// 瞄准距离
    /// </summary>
    private const float aimDistance = 2f;
    /// <summary>
    /// 最大角度
    /// </summary>
    private const float maxAngleRange = 60;
    /// <summary>
    /// 最小角度
    /// </summary>
    private const float minAngleRange = 7.5f;
    /// <summary>
    /// 准备速度
    /// </summary>
    private const float readySpeed = 1f;
    /// <summary>
    /// 准备时长
    /// </summary>
    private const float readyTime = 0.25f;
    /// <summary>
    /// 瞄准时长
    /// </summary>
    private const float aimTime = 1f;
    /// <summary>
    /// 射击间隔
    /// </summary>
    private const float shootCD = 0.15f;
    private ItemLocalObj_2013 itemLocalObj_2013;
    #endregion

    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        itemLocalObj_2013 = PoolManager.Instance.GetObject("ItemObj/ItemLocalObj_2013").GetComponent<ItemLocalObj_2013>();
        itemLocalObj_2013.transform.SetParent(body.Hand_RightItem);
        itemLocalObj_2013.transform.localPosition = Vector3.zero;
        itemLocalObj_2013.transform.localScale = Vector3.one;
        body.Hand_Left.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + config.Item_ID.ToString());
        if (data.Item_Val != 0)
        {
            gridCell.text_Info.text = data.Item_Count.ToString();
        }
        else
        {
            gridCell.text_Info.text = "";
        }
    }
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid.Open(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        if (data.Item_Val == 0 || data.Item_Val == putInItemData.Item_ID)
        {
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            ItemData cannotPutInItemData = new ItemData();
            cannotPutInItemData.Item_ID = putInItemData.Item_ID;
            cannotPutInItemData.Item_Val = putInItemData.Item_Val;
            /*只能放入木箭*/
            if (putInItemConfig.Item_ID == 9004)
            {
                /*当容器为空时，数量重新计算*/
                if (data.Item_Val == 0)
                {
                    if (data.Item_Count + putInItemData.Item_Count - 1 > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - 1 - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }

                }
                /*当容器不为空时，数量叠加*/
                else
                {
                    if (data.Item_Count + putInItemData.Item_Count > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }
                }
                return cannotPutInItemData;
            }
            else
            {
                return putInItemData;
            }
        }
        else
        {
            Debug.Log("放入失败");
            return putInItemData;
        }
    }

    public override bool PressLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (rightPressTimer >= readyTime)
            {
                leftPressTimer += dt;
                if (leftPressTimer >= shootCD)
                {
                    leftPressTimer = 0;
                    itemLocalObj_2013.Shoot();
                    Shot(Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), input);
                    if (rightPressTimer >= readyTime + 0.2f)
                    {
                        rightPressTimer -= 0.2f;
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
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.SetHandTrigger("Shoot_Ready", 1 / readyTime, null);
                owner.BodyController.SetHandBool("Shoot_Release", false, 1 / readyTime, null);
                owner.BodyController.Animator_Hand.ResetTrigger("Shoot_Play");
            }
            if (rightPressTimer < readyTime + aimTime)
            {
                rightPressTimer += dt * readySpeed;
                if (showSI)
                {
                    if (rightPressTimer > readyTime)
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
                    }
                    else
                    {
                        owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 0.2f);
                    }
                }
                return false;
            }
            else
            {
                if (showSI)
                {
                    owner.SkillSector.Update_SIsector(rightPosition, aimDistance, Mathf.Lerp(maxAngleRange, minAngleRange, (rightPressTimer - readyTime) / aimTime), 1);
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
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, rightPressTimer / readyTime, null);
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.ReleaseRightClick(dt, state, input, showSI);
    }



    public override void BeRelease(ActorManager who)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyShot = false;
            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.SetHandBool("Shoot_Release", true, rightPressTimer / readyTime, null);
            }
            if (owner.isPlayer)
            {
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            }
        }
        base.BeRelease(who);
    }

    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        if (mouse.x >= 0)
        {
            owner.BodyController.Hand_RightItem.right = mouse;
        }
        if (mouse.x < 0)
        {
            owner.BodyController.Hand_RightItem.right = -mouse;
        }

        base.FaceTo(mouse, time);
    }
    private void Shot(float offset, bool inputState)
    {
        owner.NetManager.networkRigidbody.Rigidbody.velocity = -rightPosition.normalized * 2;
        owner.NetManager.UpdateSeed();
        float randomAngle = Mathf.Lerp(-offset * 0.5f, offset * 0.5f, owner.NetManager.RandomInRange * 0.01f);
        // 将角度转换为Quaternion
        Quaternion randomRotation = Quaternion.Euler(0f, 0f, randomAngle);
        // 将旋转应用到原始向量上
        Vector3 offsetVector = randomRotation * (rightPosition.normalized);
        if (data.Item_Val != 0 && data.Item_Count > 0)
        {
            GameObject obj = PoolManager.Instance.GetObject("Bullet/Bullet_" + data.Item_Val);
            //obj.transform.position = owner.SkillSector.CenterPos;
            obj.transform.position = itemLocalObj_2013.muzzle.position;
            obj.GetComponent<BulletBase>().InitBullet(offsetVector, 10, owner.NetManager);
            if (inputState)
            {
                ItemData _oldItem = data;
                ItemData _newItem = data;
                _newItem.Item_Count--;
                if (_newItem.Item_Count == 0)
                {
                    _newItem.Item_Val = 0;
                    _newItem.Item_Count = 1;
                }
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
/// 水
/// </summary>
[Serializable]
public class Item_3000 : ItemBase
{
}
/// <summary>
/// 污染肉
/// </summary>
[Serializable]
public class Item_3001 : ItemBase
{
    private int attack = 5;
    private const float maxDistance = 1;
    private const float maxRange = 120;

    private Vector3 rightPosition = Vector3.zero;
    public override void FaceTo(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        base.FaceTo(mouse, time);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, Slash_Vertical);
            }
            else
            {
                owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void Slash_Vertical(string name)
    {
        if (name == "Slash_Vertical")
        {
            owner.SkillSector.Checkout_SIsector
                (rightPosition, maxDistance, maxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(rightPosition, 0, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attack, owner.NetManager);
                    }
                }
                if (targetTile[i].TryGetComponent(out TileObj tile))
                {
                    tile.TryToChangeHp(attack);
                }
            }
        }
    }
}
/// <summary>
/// 木碗
/// </summary>
public class Item_4001 :ItemBase
{
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid.Open(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData putInItemData)
    {
        if (data.Item_Val == 0 || data.Item_Val == putInItemData.Item_ID)
        {
            ItemConfig putInItemConfig = ItemConfigData.GetItemConfig(putInItemData.Item_ID);
            ItemData cannotPutInItemData = new ItemData();
            cannotPutInItemData.Item_ID = putInItemData.Item_ID;
            cannotPutInItemData.Item_Val = putInItemData.Item_Val;
            /*只能放入食材与食物*/
            if (putInItemConfig.Item_Type == ItemType.Ingredient || putInItemConfig.Item_Type == ItemType.Food)
            {
                /*当容器为空时，数量重新计算*/
                if (data.Item_Val == 0)
                {
                    if (data.Item_Count + putInItemData.Item_Count - 1 > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - 1 - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }

                }
                /*当容器不为空时，数量叠加*/
                else
                {
                    if (data.Item_Count + putInItemData.Item_Count > putInItemConfig.Item_MaxCount)
                    {
                        cannotPutInItemData.Item_Count = (data.Item_Count + putInItemData.Item_Count - putInItemConfig.Item_MaxCount);
                    }
                    else
                    {
                        cannotPutInItemData.Item_Count = 0;
                    }
                }
                return cannotPutInItemData;
            }
            else
            {
                return putInItemData;
            }
        }
        else
        {
            Debug.Log("放入失败");
            return putInItemData;
        }
    }

    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        body.Hand_LeftItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString() + "_" + data.Item_Val.ToString());
        this.owner = owner;
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        if (data.Item_Val != 0)
        {
            gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString() + "_" + data.Item_Val.ToString());
            gridCell.text_Info.text = data.Item_Count.ToString();
        }
        else
        {
            gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString());
            gridCell.text_Info.text = data.Item_Count.ToString();
        }
    }
    public override void DrawItemObj(ItemNetObj obj)
    {
        if (data.Item_Val != 0)
        {
            obj.icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString() + "_" + data.Item_Val.ToString());
        }
        else
        {
            obj.icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString());
        }
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {

                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }

        base.ClickLeftClick(dt, state, input, showSI);
    }
}
/// <summary>
/// 玻璃瓶
/// </summary>
public class Item_4002 : ItemBase 
{
    public override void LeftClickGridCell(UI_GridCell gridCell, ItemData itemData)
    {
        gridCell.grid.Open(itemData, TryInPut);
        base.LeftClickGridCell(gridCell, itemData);
    }
    public ItemData TryInPut(ItemData inPutData)
    {
        if (data.Item_Val == 0 || data.Item_Val == inPutData.Item_ID)
        {
            Debug.Log("尝试放入");
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(inPutData.Item_ID);
            ItemData returnData = new ItemData();
            returnData.Item_ID = inPutData.Item_ID;
            returnData.Item_Val = inPutData.Item_Val;
            /*只能放入食材与食物*/
            if (itemConfig.Item_ID == 3000)
            {
                if (data.Item_Count + inPutData.Item_Count > itemConfig.Item_MaxCount)
                {
                    returnData.Item_Count = (data.Item_Count + inPutData.Item_Count - itemConfig.Item_MaxCount);
                }
                else
                {
                    returnData.Item_Count = 0;
                }
                return returnData;
            }
            else
            {
                return inPutData;
            }
        }
        else
        {
            Debug.Log("放入失败");
            return inPutData;
        }
    }

    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString() + "_" + data.Item_Val.ToString());
        this.owner = owner;
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.image_Icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString() + "_" + data.Item_Val.ToString());
        gridCell.text_Info.text = data.Item_Count.ToString();
    }
    public override void DrawItemObj(ItemNetObj obj)
    {
        obj.icon.sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString() + "_" + data.Item_Val.ToString());
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {

                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }

        base.ClickLeftClick(dt, state, input, showSI);
    }
}
/// <summary>
/// 陶瓷瓶
/// </summary>
public class Item_4003 : ItemBase
{
    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        body.GetComponent<SpriteRenderer>().sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + data.Item_ID.ToString() + "_" + data.Item_Val.ToString());
        this.owner = owner;
    }
    public override void DrawGridCell(UI_GridCell gridCell)
    {
        gridCell.text_Info.text = data.Item_Count.ToString();
        base.DrawGridCell(gridCell);
    }
    public override void PlayDropAnim(ItemNetObj obj)
    {

    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, (string str) =>
                {
                    if (str == "HeadEat")
                    {

                    }
                });
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
            else
            {
                owner.BodyController.SetHeadTrigger("Eat", 1, null);
                owner.BodyController.SetHandTrigger("Eat", 1, null);
            }
        }

        base.ClickLeftClick(dt, state, input, showSI);
    }

}
/// <summary>
/// 新镇队员服
/// </summary>
public class Item_5001 : ItemBase
{

}
/// <summary>
/// 新镇队员帽
/// </summary>
public class Item_5002 : ItemBase
{

}

/// <summary>
/// 契约
/// </summary>
public class Item_9001 : ItemBase
{
    public override void BeHolding(ActorManager owner, BaseBodyController body)
    {
        this.owner = owner;
        body.Hand_RightItem.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_9001");
        base.BeHolding(owner, body);
    }
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            if (input)
            {
                owner.BodyController.SetHandTrigger("PutUp", 0.2f, PutUp);
            }
            else
            {
                owner.BodyController.SetHandTrigger("PutUp", 0.2f, null);
            }
        }
        base.ClickLeftClick(dt, state, input, showSI);
    }
    private void PutUp(string name)
    {
        if (name == "PutUp")
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySpawnActor()
            {
                name = "Actor/Zombie_Spray"
            }); 
        }
    }

}
/// <summary>
/// 粗制木箭
/// </summary>
public class Item_9002:ItemBase
{

}
/// <summary>
/// 精制木箭
/// </summary>
public class Item_9003 : ItemBase
{

}
/// <summary>
/// 弹丸
/// </summary>
public class Item_9004 : ItemBase
{

}
/// <summary>
/// 钥匙
/// </summary>
public class Item_9005 : ItemBase
{

}

///// <summary>
///// 铁斧头
///// </summary>
//[Serializable]
//public class Item_2002 : ItemBase
//{
//    public override void BeHolding(ActorManager owner, Transform hand)
//    {
//        this.owner = owner;
//        hand.GetComponent<SpriteRenderer>().sprite
//            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_2002");
//        base.BeHolding(owner, hand);
//    }
//    public override void ClickLeftClick(float time, bool hasInputAuthority)
//    {
//        if (owner)
//        {
//            owner.BodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
//        }
//        base.ClickLeftClick(time, hasInputAuthority);
//    }

//}
///// <summary>
///// 木弓
///// </summary>
//public class Item_2003 : ItemBase
//{
//    /// <summary>
//    /// 右键按压状态
//    /// </summary>
//    private bool rightPressState = false;
//    /// <summary>
//    /// 右键当前位置
//    /// </summary>
//    private Vector3 rightPosition = Vector3.zero;
//    /// <summary>
//    /// 右键按压时长
//    /// </summary>
//    private float rightPressTimer = 0;
//    private bool alreadyAttack = false;
//    private const float maxRange = 180;
//    private const float minRange = 60;
//    private const float readySpeed = 1;
//    private const float readyTime = 2;
//    private int attack = 5;
//    public override void BeHolding(ActorManager owner, Transform hand)
//    {
//        this.owner = owner;
//        hand.GetComponent<SpriteRenderer>().sprite
//            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_2003");
//        base.BeHolding(owner, hand);
//    }
//    public override void ClickLeftClick(float time, bool hasInputAuthority)
//    {
//        if (owner)
//        {
//            if (rightPressTimer >= readyTime)
//            {
//                alreadyAttack = true;
//                if (hasInputAuthority)
//                {
//                    owner.BodyController.PlayHandAction(HandAction.Bow_Play, 1, Bow);
//                }
//                else
//                {
//                    owner.BodyController.PlayHandAction(HandAction.Bow_Play, 1, null);
//                }
//            }
//        }
//        base.ClickLeftClick(time, hasInputAuthority);
//    }
//    public override void PressRightClick(float time, bool hasInputAuthority)
//    {
//        if (owner && !alreadyAttack)
//        {
//            if (rightPressTimer < readyTime)
//            {
//                rightPressTimer += time * readySpeed;
//            }
//            if (!rightPressState)
//            {
//                rightPressState = true;
//                owner.BodyController.PlayHandAction(HandAction.Bow_Ready, 1f / readyTime, null);
//            }
//            owner.SkillSector.Update_SIsector(rightPosition, 0.5f, Mathf.Lerp(maxRange, minRange, rightPressTimer / readyTime));
//        }
//        base.PressRightClick(time, hasInputAuthority);
//    }
//    public override void ReleaseRightClick(float time, bool hasInputAuthority)
//    {
//        if (owner)
//        {
//            rightPressTimer = 0;
//            alreadyAttack = false;
//            owner.SkillSector.Update_SIsector(rightPosition, 0, 0);

//            if (rightPressState)
//            {
//                rightPressState = false;
//                owner.BodyController.PlayHandAction(HandAction.Bow_Release, rightPressTimer / readyTime, null);
//            }
//        }
//        base.ReleaseRightClick(time, hasInputAuthority);
//    }
//    public override void FaceTo(Vector3 mouse, float time)
//    {
//        rightPosition = mouse;
//        base.FaceTo(mouse, time);
//    }
//    private void Bow(string name)
//    {
//        if (name == "Bow")
//        {
//        }
//    }

//}
///// <summary>
///// 污染肉
///// </summary>
//[Serializable]
//public class Item_3001 : ItemBase
//{
//    public override void BeHolding(ActorManager owner, Transform hand)
//    {
//        this.owner = owner;
//        hand.GetComponent<SpriteRenderer>().sprite
//            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_3001");
//        base.BeHolding(owner, hand);
//    }
//    public override void ClickLeftClick(float time, bool hasInputAuthority)
//    {
//        owner.BodyController.PlayHeadAction(HeadAction.Eat, 1, (string name) =>
//        {
//            if(name == "HeadEat")
//            {
//            }
//        });
//        owner.BodyController.PlayHandAction(HandAction.Eat, 1, null);
//        base.ClickLeftClick(time, hasInputAuthority);
//    }
//}

