using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ItemLocalObj_Torch : ItemLocalObj
{
    [SerializeField]
    private Transform transform_Fire;
    [SerializeField]
    private Transform transform_Root;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    private int temp_burnTimer = 0;
    private const int config_burnTimer = 5;
    private InputData inputData= new InputData();
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
        if (temp_burnTimer > config_burnTimer)
        {
            temp_burnTimer = 0;
            ChangeDurability(-1);
        }
        else
        {
            temp_burnTimer += second;
        }
        base.UpdateTime(second);
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
