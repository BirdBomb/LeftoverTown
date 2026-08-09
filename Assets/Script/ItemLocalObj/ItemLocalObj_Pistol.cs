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
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_RightHand.color = body.ShowRightHand(false).color;

        temp_NextAimPoint = AimTime;
        base.HoldingStart(owner, body);
    }
}
