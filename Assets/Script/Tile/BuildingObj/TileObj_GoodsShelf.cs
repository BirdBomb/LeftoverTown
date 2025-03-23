using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;

public class TileObj_GoodsShelf : MonoBehaviour
{
    //[SerializeField, Header("SingalF")]
    //private GameObject obj_singalF;
    //[SerializeField, Header("SingalE")]
    //private GameObject obj_singalE;
    //[SerializeField, Header("CabinetSteal")]
    //private GameObject obj_CabinetSteal;
    //[SerializeField, Header("CabinetBuy")]
    //private GameObject obj_CabinetBuy;
    //[SerializeField, Header("购买UI")]
    //private UI_Grid_Buy uI_CabinetBuy;
    //[SerializeField, Header("偷窃UI")]
    //private UI_Grid_GoodsShelf uI_CabinetSteal;
    //#region//瓦片生命周期
    //public override void Init()
    //{
    //    MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_UpdateTime>().Subscribe(_ =>
    //    {
    //        ListenTimeUpdate(_.hour);
    //    }).AddTo(this);
    //    base.Init();
    //}
    //#endregion
    //#region//瓦片交互
    //public override void ActorInputKeycode(ActorManager actor, KeyCode code)
    //{
    //    if (code == KeyCode.F)
    //    {
    //        OpenOrCloseSingal(obj_CabinetSteal.activeSelf);
    //        OpenOrCloseCabinetSteal(!obj_CabinetSteal.activeSelf);
    //        OpenOrCloseCabinetBuy(false);
    //    }
    //    if (code == KeyCode.E)
    //    {
    //        OpenOrCloseSingal(obj_CabinetBuy.activeSelf);
    //        OpenOrCloseCabinetBuy(!obj_CabinetBuy.activeSelf);
    //        OpenOrCloseCabinetSteal(false);
    //    }
    //    base.ActorInputKeycode(actor, code);
    //}
    //private void OpenOrCloseSingal(bool open)
    //{
    //    obj_singalF.transform.DOKill();
    //    obj_singalE.transform.DOKill();
    //    if (open)
    //    {
    //        obj_singalF.SetActive(true);
    //        obj_singalF.transform.localScale = Vector3.one;
    //        obj_singalF.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    //        obj_singalE.SetActive(true);
    //        obj_singalE.transform.localScale = Vector3.one;
    //        obj_singalE.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    //    }
    //    else
    //    {
    //        obj_singalF.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //        {
    //            obj_singalF.SetActive(false);
    //        });
    //        obj_singalE.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //        {
    //            obj_singalE.SetActive(false);
    //        });
    //    }
    //}
    //private void OpenOrCloseCabinetSteal(bool open)
    //{
    //    if (open)
    //    {
    //        obj_CabinetSteal.transform.localScale = Vector3.one;
    //        obj_CabinetSteal.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    //        obj_CabinetSteal.SetActive(true);
    //        uI_CabinetSteal.Open();
    //        uI_CabinetSteal.BindAction_ChangeInfo(TryToChangeInfo);
    //        uI_CabinetSteal.BindTileObj(this);
    //        uI_CabinetSteal.UpdateInfo(info);
    //    }
    //    else
    //    {
    //        obj_CabinetSteal.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //        {
    //            obj_CabinetSteal.SetActive(false);
    //        });
    //        uI_CabinetSteal.Close();
    //    }
    //}
    //private void OpenOrCloseCabinetBuy(bool open)
    //{
    //    if (open)
    //    {
    //        obj_CabinetBuy.transform.localScale = Vector3.one;
    //        obj_CabinetBuy.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    //        obj_CabinetBuy.SetActive(true);
    //        uI_CabinetBuy.Open();
    //        uI_CabinetBuy.BindAction_ChangeInfo(TryToChangeInfo);
    //        uI_CabinetBuy.BindTileObj(this);
    //        uI_CabinetBuy.UpdateInfo(info);
    //    }
    //    else
    //    {
    //        obj_CabinetBuy.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //        {
    //            obj_CabinetBuy.SetActive(false);
    //        });
    //        uI_CabinetBuy.Close();
    //    }
    //}

    //public override bool PlayerHolding(PlayerCoreLocal player)
    //{
    //    /*靠近是我自己*/
    //    if (player.bool_Local)
    //    {
    //        OpenOrCloseSingal(true);
    //        return true;
    //    }
    //    return false;
    //}
    //public override bool PlayerRelease(PlayerCoreLocal player)
    //{
    //    /*离开是我自己*/
    //    if (player.bool_Local)
    //    {
    //        OpenOrCloseSingal(false);
    //        OpenOrCloseCabinetSteal(false);
    //        OpenOrCloseCabinetBuy(false);
    //        return true;
    //    }
    //    return true;
    //}
    //#endregion
    //#region//绘制
    //[SerializeField]
    //private SpriteRenderer spriteRenderer;
    //[SerializeField]
    //private Sprite[] GoodsShelf_Single_Group;
    //[SerializeField]
    //private Sprite GoodsShelf_Single_Empty;
    //[SerializeField]
    //private Sprite[] GoodsShelf_Middle_Group;
    //[SerializeField]
    //private Sprite GoodsShelf_Middle_Empty;
    //[SerializeField]
    //private Sprite[] GoodsShelf_Left_Group;
    //[SerializeField]
    //private Sprite GoodsShelf_Left_Empty;
    //[SerializeField]
    //private Sprite[] GoodsShelf_Right_Group;
    //[SerializeField]
    //private Sprite GoodsShelf_Right_Empty;
    //public override void Draw(int seed)
    //{
    //    CheckAroundBuilding_FourSide(bindTile.name);
    //    base.Draw(seed);
    //}
    //public override void LinkAround(AroundState_FourSide aroundState)
    //{
    //    if (aroundState.Left && aroundState.Right)
    //    {
    //        if (info == null || info == "")
    //        {
    //            spriteRenderer.sprite = GoodsShelf_Middle_Empty;
    //        }
    //        else
    //        {
    //            spriteRenderer.sprite = GoodsShelf_Middle_Group[new System.Random().Next(0, GoodsShelf_Middle_Group.Length)];
    //        }
    //    }
    //    else if (aroundState.Right)
    //    {
    //        if (info == null || info == "")
    //        {
    //            spriteRenderer.sprite = GoodsShelf_Right_Empty;
    //        }
    //        else
    //        {
    //            spriteRenderer.sprite = GoodsShelf_Right_Group[new System.Random().Next(0, GoodsShelf_Right_Group.Length)];
    //        }
    //    }
    //    else if (aroundState.Left)
    //    {
    //        if (info == null || info == "")
    //        {
    //            spriteRenderer.sprite = GoodsShelf_Left_Empty;
    //        }
    //        else
    //        {
    //            spriteRenderer.sprite = GoodsShelf_Left_Group[new System.Random().Next(0, GoodsShelf_Left_Group.Length)];
    //        }
    //    }
    //    else
    //    {
    //        if (info == null || info == "")
    //        {
    //            spriteRenderer.sprite = GoodsShelf_Single_Empty;
    //        }
    //        else
    //        {
    //            spriteRenderer.sprite = GoodsShelf_Single_Group[new System.Random().Next(0, GoodsShelf_Single_Group.Length)];
    //        }
    //    }
    //    base.LinkAround(aroundState);
    //}
    //#endregion
    //#region//信息更新与上传
    //public override void TryToUpdateInfo(string info)
    //{
    //    uI_CabinetSteal.UpdateInfo(info);
    //    uI_CabinetBuy.UpdateInfo(info);
    //    base.TryToUpdateInfo(info);
    //    Draw(0);
    //}
    //#endregion
    //#region//工具货架
    //public void ListenTimeUpdate(int hour)
    //{

    //}
    //#endregion
}
