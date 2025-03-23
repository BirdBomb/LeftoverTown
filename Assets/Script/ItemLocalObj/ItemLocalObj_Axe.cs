using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;
/// <summary>
/// ¸«×Ó
/// </summary>
public class ItemLocalObj_Axe : ItemLocalObj
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    [SerializeField]
    private SI_Sector sector;
    [SerializeField, Header("Åü¿³ÉËº¦")]
    private short config_hackDamage = 5;
    [SerializeField, Header("Åü¿³·¶Î§")]
    private float config_hackMaxRange = 60;
    [SerializeField, Header("Åü¿³¾àÀë")]
    private float config_hackMaxDistance = 1;
    private InputData inputData = new InputData();
    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        itemData = data;
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        spriteRenderer_Hand.sprite = body.transform_RightHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer == 0)
        {
            animator.SetTrigger("Hack");
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        sector.Update_SIsector(inputData.mousePosition, config_hackMaxDistance, config_hackMaxRange, 1);
        base.UpdateMousePos(mouse);
    }
    public void Hack()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            sbyte temp = 0;
            sector.Checkout_SIsector(inputData.mousePosition, config_hackMaxDistance, config_hackMaxRange, out Transform[] targetTile);
            for (int i = 0; i < targetTile.Length; i++)
            {
                if (targetTile[i].TryGetComponent(out ActorManager actor))
                {
                    if (actor != actorManager)
                    {
                        actor.AllClient_Listen_TakeDamage(config_hackDamage, actorManager.actorNetManager);
                        temp = -2;
                    }
                }
            }
            ChangeDurability(temp);
        }
    }
    public void ChangeDurability(sbyte val)
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
