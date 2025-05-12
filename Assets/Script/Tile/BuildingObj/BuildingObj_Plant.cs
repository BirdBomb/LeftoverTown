using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Plant : BuildingObj
{
    [SerializeField]
    private GameObject obj_Singal;
    [SerializeField]
    private short short_PlantID;
    [SerializeField]
    private SpriteRenderer spriteRenderer_Plant;
    [Header("幼苗")]
    public Sprite sprite_State0;
    [Header("生长中")]
    public Sprite sprite_State1;
    [Header("成熟")]
    public Sprite sprite_State2;
    private PlantState plantState;
    public override void All_Draw()
    {
        if (info == "0")
        {
            spriteRenderer_Plant.sprite = sprite_State0;
            plantState = PlantState.State0;
        }
        else if (info == "1")
        {
            spriteRenderer_Plant.sprite = sprite_State1;
            plantState = PlantState.State1;
        }
        else if (info == "" || info == "2")
        {
            spriteRenderer_Plant.sprite = sprite_State2;
            plantState = PlantState.State2;
        }
        base.All_Draw();
    }
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            if (plantState == PlantState.State2)
            {
                Harvest();
            }
        }
        base.All_ActorInputKeycode(actor, code);
    }
    public override bool All_PlayerHolding(PlayerCoreLocal player)
    {
        /*靠近是我自己*/
        if (player.bool_Local && plantState == PlantState.State2)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool All_PlayerRelease(PlayerCoreLocal player)
    {
        /*离开是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(false);
            return true;
        }
        return true;
    }

    private void OpenOrCloseSingal(bool open)
    {
        obj_Singal.transform.DOKill();
        if (open)
        {
            obj_Singal.SetActive(true);
            obj_Singal.transform.localScale = Vector3.one;
            obj_Singal.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_Singal.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_Singal.SetActive(false);
            });
        }
    }
    /// <summary>
    /// 收获
    /// </summary>
    public void Harvest()
    {
        Type type = Type.GetType("Item_" + short_PlantID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(short_PlantID, out ItemData initData);
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            itemData = initData
        });
        spriteRenderer_Plant.transform.localScale = Vector3.one;
        spriteRenderer_Plant.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        Local_ChangeInfo("0");
    }
    public override void All_UpdateInfo(string info)
    {
        base.All_UpdateInfo(info);
        All_Draw();
    }
    public enum PlantState
    {
        State0,
        State1,
        State2,
    }

}
