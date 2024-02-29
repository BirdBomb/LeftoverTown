using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public class ItemSystem
{
}
/// <summary>
/// Ô­³õÊ÷Ö¦
/// </summary>
[Serializable]
public class Item_0 : ItemBase
{
    public override void BeHolding(BaseBehaviorController owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_0");
        base.BeHolding(owner, hand);
    }
    public override void ClickRightClick(float time, bool hasInputAuthority = false)
    {
        base.ClickRightClick(time);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority = false)
    {
        if (owner)
        {
            owner.bodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
        }
        base.ClickLeftClick(time);
    }
}
/// <summary>
/// Ô­Ä¾
/// </summary>
[Serializable]
public class Item_1001 : ItemBase
{
    public override void BeHolding(BaseBehaviorController owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_1001");
        base.BeHolding(owner, hand);
    }
    public override void ClickRightClick(float time, bool hasInputAuthority = false)
    {
        base.ClickRightClick(time);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority = false)
    {
        if (owner)
        {
            owner.bodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
        }
        base.ClickLeftClick(time);
    }
}

/// <summary>
/// Ä¾¸«Í·
/// </summary>
[Serializable]
public class Item_2001 : ItemBase
{
    /// <summary>
    /// ÓÒ¼ü°´Ñ¹×´Ì¬
    /// </summary>
    private bool rightPressState = false;
    /// <summary>
    /// ÓÒ¼üµ±Ç°Î»ÖÃ
    /// </summary>
    private Vector3 rightPosition = Vector3.zero;
    /// <summary>
    /// ÓÒ¼ü°´Ñ¹Ê±³¤
    /// </summary>
    private float rightPressTimer = 0;
    private bool alreadyAttack = false;
    private const float maxRange = 1;
    private const float readySpeed = 1;
    private const float readyTime = 1;
    private int attack = 5; 
    public override void BeHolding(BaseBehaviorController owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_2001");
        base.BeHolding(owner, hand);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority = false)
    {
        if (owner)
        {
            if(rightPressTimer >= readyTime)
            {
                alreadyAttack = true;
                if (hasInputAuthority)
                {
                    owner.bodyController.PlayHandAction(HandAction.Slash_Vertical_Play, 0.2f, Slash_Vertical);
                }
                else
                {
                    owner.bodyController.PlayHandAction(HandAction.Slash_Vertical_Play, 0.2f, null);
                }
            }
        }
        base.ClickLeftClick(time);
    }
    public override void PressRightClick(float time, bool hasInputAuthority = false)
    {
        if (owner && !alreadyAttack)
        {
            if (rightPressTimer < readyTime)
            {
                rightPressTimer += time * readySpeed;
            }
            owner.skillSector.Update_SIsector(rightPosition, rightPressTimer * maxRange, 120);
            if (!rightPressState)
            {
                rightPressState = true;
                owner.bodyController.PlayHandAction(HandAction.Slash_Vertical_Ready, 0.2f, null);
            }
        }
        base.PressRightClick(time);
    }
    public override void ReleaseRightClick(float time, bool hasInputAuthority = false)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyAttack = false;
            owner.skillSector.Update_SIsector(rightPosition, rightPressTimer, 180f);
            if (rightPressState)
            {
                rightPressState = false;
                owner.bodyController.PlayHandAction(HandAction.Slash_Vertical_Release, 1, null);
            }
        }
        base.ReleaseRightClick(time);
    }
    public override void MousePosition(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        base.MousePosition(mouse, time);
    }
    private void Slash_Vertical(string name)
    {
        if (name == "Slash_Vertical")
        {
            owner.skillSector.Checkout_SIsector(out Transform[] targetTile);
            owner.skillSector.Update_SIsector(rightPosition, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out TileObj furniture))
                {
                    furniture.Damaged(attack);
                }
                if (targetTile[i].TryGetComponent(out BaseBehaviorController actor))
                {
                    if(actor != owner)
                    {
                        actor.TryToTakeDamage(attack);
                    }
                }
            }
        }
    }
}
/// <summary>
/// Ìú¸«Í·
/// </summary>
[Serializable]
public class Item_2002 : ItemBase
{
    public override void BeHolding(BaseBehaviorController owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_2002");
        base.BeHolding(owner, hand);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority = false)
    {
        if (owner)
        {
            owner.bodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
        }
        base.ClickLeftClick(time);
    }

}
