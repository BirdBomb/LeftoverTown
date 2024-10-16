using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;

public class TileObj_GoodsShelf_Wooden : TileObj
{
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    [SerializeField, Header("SingalE")]
    private GameObject obj_singalE;
    [SerializeField, Header("CabinetSteal")]
    private GameObject obj_CabinetSteal;
    [SerializeField, Header("CabinetBuy")]
    private GameObject obj_CabinetBuy;
    [SerializeField, Header("购买UI")]
    private UI_Grid_CabinetBuy uI_CabinetBuy;
    [SerializeField, Header("偷窃UI")]
    private UI_Grid_CabinetSteal uI_CabinetSteal;
    #region//瓦片生命周期
    public override void Init()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_TimeChange>().Subscribe(_ =>
        {
            ListenTimeUpdate(_.hour);
        }).AddTo(this);
        base.Init();
    }
    #endregion
    #region//瓦片交互
    public override void PlayerInput(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_CabinetSteal.activeSelf);
            OpenOrCloseCabinetSteal(!obj_CabinetSteal.activeSelf);
            OpenOrCloseCabinetBuy(false);
        }
        if (code == KeyCode.E)
        {
            OpenOrCloseSingal(obj_CabinetBuy.activeSelf);
            OpenOrCloseCabinetBuy(!obj_CabinetBuy.activeSelf);
            OpenOrCloseCabinetSteal(false);
        }
        base.PlayerInput(player, code);
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_singalF.transform.DOKill();
        obj_singalE.transform.DOKill();
        if (open)
        {
            obj_singalF.SetActive(true);
            obj_singalF.transform.localScale = Vector3.one;
            obj_singalF.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_singalE.SetActive(true);
            obj_singalE.transform.localScale = Vector3.one;
            obj_singalE.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_singalF.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_singalF.SetActive(false);
            });
            obj_singalE.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_singalE.SetActive(false);
            });
        }
    }
    private void OpenOrCloseCabinetSteal(bool open)
    {
        if (open)
        {
            obj_CabinetSteal.transform.localScale = Vector3.one;
            obj_CabinetSteal.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_CabinetSteal.SetActive(true);
            uI_CabinetSteal.Open(this);
            uI_CabinetSteal.UpdateInfoFromTile(info);
        }
        else
        {
            obj_CabinetSteal.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_CabinetSteal.SetActive(false);
            });
            uI_CabinetSteal.Close(this);
        }
    }
    private void OpenOrCloseCabinetBuy(bool open)
    {
        if (open)
        {
            obj_CabinetBuy.transform.localScale = Vector3.one;
            obj_CabinetBuy.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_CabinetBuy.SetActive(true);
            uI_CabinetBuy.Open(this);
            uI_CabinetBuy.UpdateInfoFromTile(info);
        }
        else
        {
            obj_CabinetBuy.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_CabinetBuy.SetActive(false);
            });
            uI_CabinetBuy.Close(this);
        }
    }

    public override bool PlayerHolding(PlayerController player)
    {
        /*靠近是我自己*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerRelease(PlayerController player)
    {
        /*离开是我自己*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCabinetSteal(false);
            OpenOrCloseCabinetBuy(false);
            return true;
        }
        return true;
    }
    #endregion
    #region//绘制
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] GoodsShelf_Single_Group;
    [SerializeField]
    private Sprite GoodsShelf_Single_Empty;
    [SerializeField]
    private Sprite[] GoodsShelf_Middle_Group;
    [SerializeField]
    private Sprite GoodsShelf_Middle_Empty;
    [SerializeField]
    private Sprite[] GoodsShelf_Left_Group;
    [SerializeField]
    private Sprite GoodsShelf_Left_Empty;
    [SerializeField]
    private Sprite[] GoodsShelf_Right_Group;
    [SerializeField]
    private Sprite GoodsShelf_Right_Empty;
    public override void Draw()
    {
        CheckAround("GoodsShelf_Wooden", true);
        base.Draw();
    }
    public override void LinkAround(LinkState linkState, TileObj linkToUp, TileObj linkToDown, TileObj linkToLeft, TileObj linkToRight)
    {
        if (linkState == LinkState.NoneSide || linkState == LinkState.OneSide_Down || linkState == LinkState.OneSide_Up || linkState == LinkState.TwoSide_UpDown)
        {
            if (info == null || info == "")
            {
                spriteRenderer.sprite = GoodsShelf_Middle_Empty;
            }
            else
            {
                spriteRenderer.sprite = GoodsShelf_Middle_Group[new System.Random().Next(0, GoodsShelf_Middle_Group.Length)];
            }
        }
        else if (linkState == LinkState.ThreeSide_LeftMissing || linkState == LinkState.OneSide_Right || linkState == LinkState.TwoSide_DownRight || linkState == LinkState.TwoSide_UpRight)
        {
            if (info == null || info == "")
            {
                spriteRenderer.sprite = GoodsShelf_Right_Empty;
            }
            else
            {
                spriteRenderer.sprite = GoodsShelf_Right_Group[new System.Random().Next(0, GoodsShelf_Right_Group.Length)];
            }
        }
        else if (linkState == LinkState.ThreeSide_RightMissing || linkState == LinkState.OneSide_Left || linkState == LinkState.TwoSide_DownLeft || linkState == LinkState.TwoSide_UpLeft)
        {
            if (info == null || info == "")
            {
                spriteRenderer.sprite = GoodsShelf_Left_Empty;
            }
            else
            {
                spriteRenderer.sprite = GoodsShelf_Left_Group[new System.Random().Next(0, GoodsShelf_Left_Group.Length)];
            }
        }
        else
        {
            if (info == null || info == "")
            {
                spriteRenderer.sprite = GoodsShelf_Single_Empty;
            }
            else
            {
                spriteRenderer.sprite = GoodsShelf_Single_Group[new System.Random().Next(0, GoodsShelf_Single_Group.Length)];
            }
        }
        base.LinkAround(linkState, linkToUp, linkToDown, linkToLeft, linkToRight);
    }

    #endregion
    #region//信息更新与上传
    public override void TryToUpdateInfo(string info)
    {
        uI_CabinetSteal.UpdateInfoFromTile(info);
        uI_CabinetBuy.UpdateInfoFromTile(info);
        base.TryToUpdateInfo(info);
        Draw();
    }
    #endregion
    #region//工具货架
    public void ListenTimeUpdate(int hour)
    {

    }
    #endregion
}
