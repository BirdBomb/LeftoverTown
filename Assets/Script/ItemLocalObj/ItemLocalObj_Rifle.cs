using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Rifle : ItemLocalObj_Gun
{
    [SerializeField]
    private SpriteRenderer spriteRenderer_RightHand;
    [SerializeField]
    private SpriteRenderer spriteRenderer_LeftHand;

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

        spriteRenderer_RightHand.color = body.transform_RightHand.GetComponent<SpriteRenderer>().color;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;

        spriteRenderer_LeftHand.color = body.transform_LeftHand.GetComponent<SpriteRenderer>().color;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
    }
}
