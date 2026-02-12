using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemLocalObj_Torch : ItemLocalObj
{
    [SerializeField]
    private Transform transform_Fire;
    [SerializeField]
    private Transform transform_Root;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    [SerializeField]
    private Light2D light2D;
    private float config_BurnTimer = 0;
    private float temp_BurnTimer = 0;
    private InputData inputData= new InputData();
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        transform.SetParent(body.transform_ItemInRightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        spriteRenderer_Hand.color = body.transform_RightHand.GetComponent<SpriteRenderer>().color;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingStart(owner, body);
    }
    public void UpdateTorchData(float lightRange,float expendSpeed,ItemQuality itemQuality)
    {
        light2D.pointLightOuterRadius = lightRange;
        if (expendSpeed <= 0) { config_BurnTimer = int.MaxValue; }
        else { config_BurnTimer = 1f / expendSpeed; }
    }
    private void FixedUpdate()
    {
        transform_Fire.rotation = Quaternion.identity;
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer == 0)
        {
            StretchOut();
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        inputData.leftPressTimer = 0;
        StretchBack();
        base.ReleaseLeftMouse();
    }
    public void StretchOut()
    {
        transform_Root.DOKill();
        transform_Root.DOLocalMoveX(0.4f, 0.2f);
        transform_Root.DOLocalRotate(new Vector3(0, 0, -45), 0.2f);
    }
    public void StretchBack()
    {
        transform_Root.DOKill();
        transform_Root.DOLocalMoveX(0f, 0.2f);
        transform_Root.DOLocalRotate(Vector3.zero, 0.2f);
    }
    public override void UpdateTime(int second)
    {
        if (temp_BurnTimer > config_BurnTimer)
        {
            temp_BurnTimer = 0;
            ChangeDurability(-1);
        }
        else
        {
            temp_BurnTimer += second;
        }
        base.UpdateTime(second);
    }
    public void ChangeDurability(sbyte val)
    {
        if (val != 0 && actorManager.actorAuthority.isPlayer)
        {
            ItemData _oldItem = itemData;
            ItemData _newItem = itemData;
            if (_newItem.D + val <= 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Sub()
                {
                    item = itemData,
                });
            }
            else
            {
                _newItem.D += val;
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
                {
                    oldItem = _oldItem,
                    newItem = _newItem,
                });
            }
        }
    }
}
