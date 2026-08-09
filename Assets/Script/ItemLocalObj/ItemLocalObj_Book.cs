using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemLocalObj_Book : ItemLocalObj
{
    public SpriteRenderer spriteRenderer_BookOpen;
    public SpriteRenderer spriteRenderer_BookClose;
    public SpriteRenderer spriteRenderer_LeftHand;
    public SpriteRenderer spriteRenderer_RightHand;
    public SpriteAtlas spriteAtlas_Book;
    public SpriteAtlas spriteAtlas_Item;
    private BookState bookState = BookState.Close;
    private SpriteRenderer spriteRenderer_ActorLeftHand;
    private SpriteRenderer spriteRenderer_ActorRightHand;
    private const float float_CheckDuration = 0.2f;
    private float float_stationaryTimer = 0f;      // 静止持续时间
    private const float float_requiredStationaryTime = 1f;  // 需要静止1秒才能打开书
    private float float_AddExpCD = 10;
    private float float_AddExpTimer = 0;
    private short short_AddExpVal = 0;
    private short short_ReadingLevel = 0;
    private bool isMoving = false;
    public enum BookState
    {
        Close, Open
    }

    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        body.AddItemOnLeftHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_ActorLeftHand = body.ShowLeftHand(true);
        spriteRenderer_ActorRightHand = body.ShowRightHand(true);
        spriteRenderer_LeftHand.color = spriteRenderer_ActorLeftHand.color;
        spriteRenderer_RightHand.color = spriteRenderer_ActorRightHand.color;

        spriteRenderer_BookOpen.sprite = spriteAtlas_Book.GetSprite(itemData.I.ToString());
        spriteRenderer_BookClose.sprite = spriteAtlas_Item.GetSprite("Item_" + itemData.I.ToString());

        StartCoroutine(Check());
        base.HoldingStart(owner, body);

    }
    public void UpdateBookData(short ExpVal,short ReadingLevel)
    {
        short_AddExpVal = ExpVal;
        short_ReadingLevel = ReadingLevel;
    }
    private IEnumerator Check()
    {
        while (actorManager)
        {
            float currentSpeed = actorManager.bodyController.GetSpeed();
            bool currentlyMoving = currentSpeed > 0.1f;

            if (currentlyMoving)
            {
                // 正在移动：重置静止计时器，书必须关闭
                float_stationaryTimer = 0f;
                if (bookState != BookState.Close)
                {
                    ChangeBookState(true); // 关闭书本
                }
            }
            else
            {
                // 静止状态：累加静止时间
                float_stationaryTimer += float_CheckDuration;

                // 只有静止时间达到1秒后，才打开书本
                bool shouldOpen = float_stationaryTimer >= float_requiredStationaryTime;
                bool isCurrentlyOpen = (bookState == BookState.Open);

                if (shouldOpen && !isCurrentlyOpen)
                {
                    ChangeBookState(false); // 打开书本
                }
                else if (!shouldOpen && isCurrentlyOpen)
                {
                    ChangeBookState(true); // 还没到1秒，保持关闭
                }
            }

            yield return new WaitForSeconds(float_CheckDuration);

            // 只有在书本打开状态下才增加经验
            if (bookState == BookState.Open)
            {
                float_AddExpTimer += float_CheckDuration;
                if (float_AddExpTimer >= float_AddExpCD)
                {
                    AddExp();
                    float_AddExpTimer = 0;
                }
            }
        }
    }
    private void ChangeBookState(bool close)
    {
        bookState =  close ? BookState.Close : BookState.Open;
        spriteRenderer_BookClose.gameObject.SetActive(close);
        spriteRenderer_BookOpen.gameObject.SetActive(!close);

        spriteRenderer_ActorLeftHand.enabled = close;
        spriteRenderer_ActorRightHand.enabled = close;
    }
    private void AddExp()
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal && short_AddExpVal > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddExp()
            {
                exp = short_AddExpVal
            });
        }
    }
}
