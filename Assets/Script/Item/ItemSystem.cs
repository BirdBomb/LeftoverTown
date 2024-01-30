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
    public override void ClickRightBtn()
    {
        base.ClickRightBtn();
    }
    public override void ClickLeftBtn()
    {
        if (owner)
        {
            owner.bodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
        }
        base.ClickLeftBtn();
    }
}
/// <summary>
/// Ä¾¸«Í·
/// </summary>
[Serializable]
public class Item_2001 : ItemBase
{
    public override void BeHolding(BaseBehaviorController owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_2001");
        base.BeHolding(owner, hand);
    }
    public override void ClickLeftBtn()
    {
        if (owner)
        {
            owner.bodyController.PlayHandAction(HandAction.Slash_Vertical_Play, 0.2f, null);
        }
        base.ClickLeftBtn();
    }
    private bool press = false;
    public override void PressRightBtn()
    {
        if (owner && !press)
        {
            press = true;
            owner.bodyController.PlayHandAction(HandAction.Slash_Vertical_Ready, 0.2f, null);
        }
        base.PressRightBtn();
    }
    public override void ReleaseRightBtn()
    {
        if (owner && press)
        {
            press = false;
            owner.bodyController.PlayHandAction(HandAction.Slash_Vertical_Release, 1, null);
        }
        base.ReleaseRightBtn();
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
    public override void ClickLeftBtn()
    {
        if (owner)
        {
            owner.bodyController.PlayHandAction(HandAction.Slash_Horizontal, 0.2f, null);
        }
        base.ClickLeftBtn();
    }

}
