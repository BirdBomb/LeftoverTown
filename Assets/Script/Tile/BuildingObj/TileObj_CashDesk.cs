using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileObj_CashDesk : TileObj
{
    private ActorManager onlyState_owner;
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    [SerializeField, Header("CabinetSell")]
    private GameObject obj_CabinetSell;
    [SerializeField, Header("出售UI")]
    private UI_Grid_CabinetSell uI_CabinetSell;
    #region//瓦片生命周期
    public override void Init()
    {
        CreateNPC();
        base.Init();
    }
    private void OnDestroy()
    {
        DestroyNPC();
    }
    #endregion
    #region//瓦片交互
    public override void PlayerInput(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_CabinetSell.activeSelf);
            OpenOrCloseCabinetSell(!obj_CabinetSell.activeSelf);
        }
        base.PlayerInput(player, code);
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_singalF.transform.DOKill();
        if (open)
        {
            obj_singalF.SetActive(true);
            obj_singalF.transform.localScale = Vector3.one;
            obj_singalF.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_singalF.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_singalF.SetActive(false);
            });
        }
    }
    private void OpenOrCloseCabinetSell(bool open)
    {
        if (open)
        {
            obj_CabinetSell.transform.localScale = Vector3.one;
            obj_CabinetSell.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_CabinetSell.SetActive(true);
            uI_CabinetSell.Open(this);
            uI_CabinetSell.UpdateInfoFromTile(info);
        }
        else
        {
            obj_CabinetSell.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_CabinetSell.SetActive(false);
            });
            uI_CabinetSell.Close(this);
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
            OpenOrCloseCabinetSell(false);
            return true;
        }
        return false;
    }

    #endregion
    #region//信息更新与上传
    public override void TryToUpdateInfo(string info)
    {
        uI_CabinetSell.UpdateInfoFromTile(info);
        base.TryToUpdateInfo(info);
    }
    #endregion
    #region//收银台
    private void CreateNPC()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_TownAssistant",
            pos = transform.position + Vector3.up,
            callBack = BindOwner
        });
    }
    private void DestroyNPC()
    {
        if(onlyState_owner != null)
        {
            Destroy(onlyState_owner.gameObject);
        }
    }
    public void BindOwner(ActorManager actor)
    {
        onlyState_owner = actor;
        (actor as ActorManager_NPC_TownAssistant).BindWorkTile(this);
    }
    #endregion
    #region//绘制
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite CashDesk_Single;
    [SerializeField]
    private Sprite CashDesk_Middle;
    [SerializeField]
    private Sprite CashDesk_Left;
    [SerializeField]
    private Sprite CashDesk_Right;

    public override void Draw(int seed)
    {
        CheckAroundBuilding_FourSide(new List<string>() { "Desk", "CashDesk" });
        base.Draw(seed);
    }
    public override void LinkAround(AroundState_FourSide aroundState)
    {
        if (aroundState.Left && aroundState.Right)
        {
            spriteRenderer.sprite = CashDesk_Middle;
        }
        else if (aroundState.Left)
        {
            spriteRenderer.sprite = CashDesk_Left;
        }
        else if (aroundState.Right)
        {
            spriteRenderer.sprite = CashDesk_Right;
        }
        else
        {
            spriteRenderer.sprite = CashDesk_Single;
        }
        base.LinkAround(aroundState);
    }
    #endregion
}
