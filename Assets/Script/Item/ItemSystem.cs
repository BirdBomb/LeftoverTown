using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UniRx;
using static UnityEngine.UI.GridLayoutGroup;

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
    public override void ClickLeftClick(float dt, bool state, bool input, bool showSI)
    {
        if (owner)
        {
            owner.BodyController.SetHandTrigger("Slash_Vertical", 0.2f, null);
        }
        base.ClickLeftClick(dt, state, input, showSI);
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
    public override void PressRightClick(float dt, bool state, bool input, bool showSI)
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
            }
            if (showSI)
            {
                owner.SkillSector.Update_SIsector(rightPosition, Mathf.Lerp(0, maxDistance, rightPressTimer / readyTime), maxRange);
            }
        }
        base.PressRightClick(dt, state, input, showSI);
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
                owner.SkillSector.Update_SIsector(rightPosition, 0, 0);
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
                (rightPosition, Mathf.Lerp(0, maxDistance, rightPressTimer / readyTime), maxRange, out Transform[] targetTile);
            owner.SkillSector.Update_SIsector(rightPosition, 0, 0);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != owner)
                    {
                        actor.TakeDamage(attack, owner.NetController.Object.Id);
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
/// ÆõÔ¼
/// </summary>
[Serializable]
public class Item_9001 : ItemBase
{
    public override void BeHolding(ActorManager owner, Transform hand)
    {
        this.owner = owner;
        hand.GetComponent<SpriteRenderer>().sprite
            = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_9001");
        base.BeHolding(owner, hand);
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

///// <summary>
///// Ìú¸«Í·
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
///// Ä¾¹­
///// </summary>
//public class Item_2003 : ItemBase
//{
//    /// <summary>
//    /// ÓÒ¼ü°´Ñ¹×´Ì¬
//    /// </summary>
//    private bool rightPressState = false;
//    /// <summary>
//    /// ÓÒ¼üµ±Ç°Î»ÖÃ
//    /// </summary>
//    private Vector3 rightPosition = Vector3.zero;
//    /// <summary>
//    /// ÓÒ¼ü°´Ñ¹Ê±³¤
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
///// ÎÛÈ¾Èâ
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

