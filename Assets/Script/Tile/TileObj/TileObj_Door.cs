using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileObj_Door : TileObj
{
    [SerializeField, Header("Unlock")]
    private GameObject ui_Unlock;
    [SerializeField, Header("Lock")]
    private GameObject ui_Lock;
    [SerializeField, Header("Bar")]
    private Image ui_Bar;
    [SerializeField, Header("Open")]
    private GameObject obj_door_Open;
    [SerializeField, Header("Close")]
    private GameObject obj_door_Close;
    [SerializeField, Header("Lock")]
    private GameObject obj_door_Lock;
    private bool open = false;

    public override void Invoke(PlayerController player)
    {
        if (info != "")
        {
            ui_Bar.DOKill();
            ui_Bar.transform.localScale = new Vector3(0, 1, 1);
            ui_Bar.transform.DOScaleX(1, 1f).OnComplete(() =>
            {
                TryToChangeInfo("");
                ui_Lock.SetActive(false);
                ui_Unlock.SetActive(false);
            });
        }
        else
        {
            if (player.thisPlayerIsMe && player.actorManager.NetManager.Data_ItemInHand.Item_ID == 9005)
            {
                TryToChangeInfo("lock");
                ui_Lock.SetActive(false);
                ui_Unlock.SetActive(false);
            }
        }
        base.Invoke(player);
    }
    public override void TryToUpdateInfo(string info)
    {
        base.TryToUpdateInfo(info);
        if (info == "")
        {
            obj_door_Open.SetActive(true);
            obj_door_Close.SetActive(false);
            obj_door_Lock.SetActive(false);
        }
        else
        {
            obj_door_Open.SetActive(false);
            obj_door_Close.SetActive(false);
            obj_door_Lock.SetActive(true);
        }
    }
    public override bool PlayerNearby(PlayerController player)
    {
        if (info == "")
        {
            if (player.thisPlayerIsMe && player.actorManager.NetManager.Data_ItemInHand.Item_ID == 9005)
            {
                ui_Lock.SetActive(true);
                ui_Lock.transform.DOKill();
                ui_Lock.transform.localScale = Vector3.one;
                ui_Lock.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
            /*门没上锁*/
            if (!open)
            {
                open = true;
                obj_door_Open.SetActive(true);
                obj_door_Close.SetActive(false);
                obj_door_Lock.SetActive(false);
                ui_Bar.DOKill();
                ui_Bar.transform.localScale = new Vector3(0, 1, 1);
            }
        }
        else
        {
            /*门上锁了*/
            ui_Unlock.SetActive(true);
            ui_Unlock.transform.DOKill();
            ui_Unlock.transform.localScale = Vector3.one;
            ui_Unlock.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }


        ///*靠近是我自己*/
        //if (player.thisPlayerIsMe)
        //{
        //    Debug.Log("Open");
        //    obj_singal.SetActive(true);

        //    transform.DOKill();
        //    transform.localScale = Vector3.one;
        //    transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

        //    return true;
        //}
        return true;
    }
    public override bool PlayerFaraway(PlayerController player)
    {
        ui_Lock.SetActive(false);
        ui_Unlock.SetActive(false);
        if (info == "")
        {
            /*门没上锁*/
            if (open)
            {
                open = false;
                obj_door_Open.SetActive(false);
                obj_door_Close.SetActive(true);
                obj_door_Lock.SetActive(false);
            }
        }
        else
        {
            /*门上锁了*/

        }



        ///*靠近的不是我自己*/
        //if (!player.thisPlayerIsMe) { return false; }
        //obj_door_Open.SetActive(false);
        //obj_door_Close.SetActive(true);
        //obj_singal.SetActive(false);
        return true;
    }
    public override void TryToUpdateHp(int newHp)
    {
        if (newHp <= CurHp)
        {
            PlayDamagedAnim();
        }
        base.TryToUpdateHp(newHp);
    }
    public override void ListenDestroyMyObj()
    {
        PlayBreakAnim();
        Invoke("DestroyMyObj", 0.3f);
    }
    public override void PlayDamagedAnim()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
        base.PlayDamagedAnim();
    }
    public override void PlayBreakAnim()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            transform.DOScaleX(0, 0.05f);
        });
        base.PlayBreakAnim();
    }

}
