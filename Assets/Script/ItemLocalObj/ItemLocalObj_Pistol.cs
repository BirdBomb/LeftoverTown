using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;
/// <summary>
/// µ• ÷ ÷«π
/// </summary>
public class ItemLocalObj_Pistol : ItemLocalObj_Gun
{
    [SerializeField]
    private SpriteRenderer spriteRenderer_RightHand;
    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        itemData = data;
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        temp_NextAimPoint = config_ShotAimTime;

        spriteRenderer_RightHand.sprite = body.transform_RightHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
    }
}
