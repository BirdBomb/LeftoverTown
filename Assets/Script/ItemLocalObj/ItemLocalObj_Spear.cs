using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Spear : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform transform_Root;
    [SerializeField]
    private SpriteRenderer spriteRenderer_LeftHand;
    [SerializeField]
    private SpriteRenderer spriteRenderer_RightHand;
    [SerializeField]
    private SI_Sector sector;

    [SerializeField, Header("¥¡¥Ã…À∫¶")]
    private short config_StabDamage;
    [SerializeField, Header("∫·…®…À∫¶")]
    private short config_HackDamage;
    [SerializeField, Header("π•ª˜æ‡¿Î")]
    private short config_AttackDistance;
    private InputData inputData = new InputData();
    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        actorManager = owner;
        itemData = data;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        spriteRenderer_LeftHand.sprite = body.transform_LeftHand.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer_RightHand.sprite = body.transform_LeftHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
    }
    public void FaceTo(Vector3 dir)
    {
        if (dir.x >= 0)
        {
            transform_Root.right = dir;
        }
        if (dir.x < 0)
        {
            transform_Root.right = -dir;
        }

    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        FaceTo(mouse);
        base.UpdateMousePos(mouse);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer == 0)
        {
            animator.SetTrigger("Stab");
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.rightPressTimer == 0)
        {
            animator.SetTrigger("Hack");
        }
        inputData.rightPressTimer = time;
        return base.PressRightMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftMouse();
    }
    public override void ReleaseRightMouse()
    {
        inputData.rightPressTimer = 0;
        base.ReleaseRightMouse();
    }
    public void Stab()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            sbyte temp = 0;
            sector.Checkout_SIsector
                (inputData.mousePosition, config_AttackDistance, 5, out Transform[] targetTile);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != actorManager)
                    {
                        actor.AllClient_Listen_TakeDamage(config_StabDamage, actorManager.actorNetManager);
                        temp = -2;
                    }
                }
            }
            ChangeDurability(temp);
        }
    }
    public void Hack()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            sbyte temp = 0;
            sector.Checkout_SIsector
                (inputData.mousePosition, config_AttackDistance, 60, out Transform[] targetTile);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != actorManager)
                    {
                        actor.AllClient_Listen_TakeDamage(config_HackDamage, actorManager.actorNetManager);
                        temp = -2;
                    }
                }
            }
            ChangeDurability(temp);
        }
    }
    private void ChangeDurability(sbyte val)
    {
        if (val != 0 && actorManager.actorAuthority.isPlayer)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            if (_newItem.Item_Durability + val <= 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemOnHand()
                {
                    item = itemData,
                });
            }
            else
            {
                _newItem.Item_Durability += val;
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
                {
                    oldItem = _oldItem,
                    newItem = _newItem,
                });
            }
        }
    }
}
