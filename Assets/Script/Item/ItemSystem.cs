using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UniRx;
public class ItemSystem
{
}
/// <summary>
/// Ô­³õÊ÷Ö¦
/// </summary>
[Serializable]
public class Item_0 : ItemBase
{
    public override void BeHolding(ActorManager owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_0");
        base.BeHolding(owner, hand);
    }
    public override void ClickRightClick(float time, bool hasInputAuthority)
    {
        base.ClickRightClick(time, hasInputAuthority);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority)
    {
        if (owner)
        {
            owner.BodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
        }
        base.ClickLeftClick(time, hasInputAuthority);
    }
}
/// <summary>
/// Ô­Ä¾
/// </summary>
[Serializable]
public class Item_1001 : ItemBase
{
    public override void BeHolding(ActorManager owner, Transform item)
    {
        this.owner = owner;
        item.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_1001");
        base.BeHolding(owner, item);
    }
    public override void ClickRightClick(float time, bool hasInputAuthority)
    {
        base.ClickRightClick(time, hasInputAuthority);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority)
    {
        if (owner)
        {
            owner.BodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
        }
        base.ClickLeftClick(time, hasInputAuthority);
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
    private const float maxDistance = 1;
    private const float maxRange = 120;
    private const float readySpeed = 1;
    private const float readyTime = 0.5f;
    private int attack = 5; 
    public override void BeHolding(ActorManager owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_2001");
        base.BeHolding(owner, hand);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority)
    {
        if (owner)
        {
            if(rightPressTimer >= readyTime)
            {
                alreadyAttack = true;
                if (hasInputAuthority)
                {
                    owner.BodyController.PlayHandAction(HandAction.Slash_Vertical_Play, 1, Slash_Vertical);
                }
                else
                {
                    owner.BodyController.PlayHandAction(HandAction.Slash_Vertical_Play, 1, null);
                }
            }
        }
        base.ClickLeftClick(time, hasInputAuthority);
    }
    public override void PressRightClick(float time, bool hasInputAuthority)
    {
        if (owner && !alreadyAttack)
        {
            if (rightPressTimer < readyTime)
            {
                rightPressTimer += time * readySpeed;
            }
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.PlayHandAction(HandAction.Slash_Vertical_Ready,1f / readyTime, null);
            }
            owner.SkillSector.Update_SIsector(rightPosition, Mathf.Lerp(0, maxDistance, rightPressTimer / readyTime), maxRange);
        }
        base.PressRightClick(time, hasInputAuthority);
    }
    public override void ReleaseRightClick(float time, bool hasInputAuthority)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyAttack = false;
            owner.SkillSector.Update_SIsector(rightPosition, 0, 0);

            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.PlayHandAction(HandAction.Slash_Vertical_Release, rightPressTimer / readyTime, null);
            }
        }
        base.ReleaseRightClick(time, hasInputAuthority);
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
            owner.SkillSector.Checkout_SIsector
                (rightPosition, Mathf.Lerp(0, maxDistance, rightPressTimer / readyTime), maxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(rightPosition, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out TileObj furniture))
                {
                    MessageBroker.Default.Publish(new MapEvent.MapEvent_DamageTile_Upload()
                    {
                        tilePos = new Vector3Int( furniture.bindTile.x,furniture.bindTile.y,0),
                        damageValue = attack
                    });
                }
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if(actor != owner)
                    {
                        actor.NetController.Data_Hp -= attack;
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
    public override void BeHolding(ActorManager owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_2002");
        base.BeHolding(owner, hand);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority)
    {
        if (owner)
        {
            owner.BodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
        }
        base.ClickLeftClick(time, hasInputAuthority);
    }

}
/// <summary>
/// Ä¾¹­
/// </summary>
public class Item_2003 : ItemBase
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
    private const float maxRange = 180;
    private const float minRange = 60;
    private const float readySpeed = 1;
    private const float readyTime = 2;
    private int attack = 5;
    public override void BeHolding(ActorManager owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_2003");
        base.BeHolding(owner, hand);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority)
    {
        if (owner)
        {
            if (rightPressTimer >= readyTime)
            {
                alreadyAttack = true;
                if (hasInputAuthority)
                {
                    owner.BodyController.PlayHandAction(HandAction.Bow_Play, 1, Bow);
                }
                else
                {
                    owner.BodyController.PlayHandAction(HandAction.Bow_Play, 1, null);
                }
            }
        }
        base.ClickLeftClick(time, hasInputAuthority);
    }
    public override void PressRightClick(float time, bool hasInputAuthority)
    {
        if (owner && !alreadyAttack)
        {
            if (rightPressTimer < readyTime)
            {
                rightPressTimer += time * readySpeed;
            }
            if (!rightPressState)
            {
                rightPressState = true;
                owner.BodyController.PlayHandAction(HandAction.Bow_Ready, 1f / readyTime, null);
            }
            owner.SkillSector.Update_SIsector(rightPosition, 0.5f, Mathf.Lerp(maxRange, minRange, rightPressTimer / readyTime));
        }
        base.PressRightClick(time, hasInputAuthority);
    }
    public override void ReleaseRightClick(float time, bool hasInputAuthority)
    {
        if (owner)
        {
            rightPressTimer = 0;
            alreadyAttack = false;
            owner.SkillSector.Update_SIsector(rightPosition, 0, 0);

            if (rightPressState)
            {
                rightPressState = false;
                owner.BodyController.PlayHandAction(HandAction.Bow_Release, rightPressTimer / readyTime, null);
            }
        }
        base.ReleaseRightClick(time, hasInputAuthority);
    }
    public override void MousePosition(Vector3 mouse, float time)
    {
        rightPosition = mouse;
        base.MousePosition(mouse, time);
    }
    private void Bow(string name)
    {
        if (name == "Bow")
        {
        }
    }

}
/// <summary>
/// ÎÛÈ¾Èâ
/// </summary>
[Serializable]
public class Item_3001 : ItemBase
{
    public override void BeHolding(ActorManager owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_3001");
        base.BeHolding(owner, hand);
    }
    public override void ClickLeftClick(float time, bool hasInputAuthority)
    {
        owner.BodyController.PlayHeadAction(HeadAction.Eat, 1, (string name) =>
        {
            if(name == "HeadEat")
            {
                if (networkItem.Item_CurCount > 0)
                {
                    networkItem.Item_CurCount = 1;
                    owner.NetController.Data_ItemInBag.Remove(networkItem);
                }
            }
        });
        owner.BodyController.PlayHandAction(HandAction.Eat, 1, null);
        base.ClickLeftClick(time, hasInputAuthority);
    }
}

