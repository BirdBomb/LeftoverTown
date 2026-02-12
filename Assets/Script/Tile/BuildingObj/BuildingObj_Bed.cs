using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingObj_Bed : BuildingObj_Manmade
{
    public GameObject obj_SingalUI;
    public GameObject obj_HightlightUI;
    public Transform tran_SleepPos;
    public BedFace bedFace;
    /// <summary>
    /// 占用者ID(新)
    /// </summary>
    public Fusion.NetworkId networkId_SleeperNew;
    /// <summary>
    /// 占用者ID(旧)
    /// </summary>
    public Fusion.NetworkId networkId_SleeperLast;
    /// <summary>
    /// 是否占用
    /// </summary>
    public bool bool_SleeperOn = false;
    private List<ActorManager> actors_Nearby = new List<ActorManager>();
    #region//信息上传与同步
    public override void All_UpdateInfo(string info)
    {
        ReadInfo(info);
        base.All_UpdateInfo(info);
    }
    public void ReadInfo(string info)
    {
        networkId_SleeperLast = networkId_SleeperNew;
        networkId_SleeperNew.Raw = uint.Parse(info);
        bool_SleeperOn = false;
        if(networkId_SleeperNew == new Fusion.NetworkId())
        {
            //床位空置
            foreach (ActorManager actorManager in actors_Nearby)
            {
                if (actorManager != null && actorManager.actorNetManager.Object.Id.Equals(networkId_SleeperLast))
                {
                    if(actorManager.actorAuthority.isLocal && actorManager.actorAuthority.isPlayer)
                    {
                        OpenOrCloseHighlightUI(true);
                    }
                }
            }
        }
        else
        {
            //床位占用
            foreach (ActorManager actorManager in actors_Nearby)
            {
                if (actorManager != null && actorManager.actorNetManager.Object.Id.Equals(networkId_SleeperNew))
                {
                    bool_SleeperOn = true;
                }
                
            }

        }
        Debug.Log("占用状态:"+ bool_SleeperOn);
    }
    public void WriteInfo(Fusion.NetworkId id)
    {
        Debug.Log(id);

        networkId_SleeperNew = id;
        Local_ChangeInfo(networkId_SleeperNew.Raw.ToString());
    }

    #endregion
    #region//瓦片交互
    public override bool All_ActorNearby(ActorManager actor)
    {
        if (!actors_Nearby.Contains(actor))
        {
            actors_Nearby.Add(actor);
        }
        return base.All_ActorNearby(actor);
    }
    public override bool All_ActorFaraway(ActorManager actor)
    {
        if (actors_Nearby.Contains(actor))
        {
            actors_Nearby.Remove(actor);
        }
        return base.All_ActorFaraway(actor);
    }
    public override void Local_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            Local_StartingSleep(actor);
        }
        base.Local_ActorInputKeycode(actor, code);
    }
    public override void Local_PlayerHighlight(bool on)
    {
        OpenOrCloseHighlightUI(on);
        base.Local_PlayerHighlight(on);
    }
    public override void Local_PlayerFaraway()
    {
        base.Local_PlayerFaraway();
    }
    public override void OpenOrCloseHighlightUI(bool open)
    {
        obj_SingalUI.transform.DOKill();
        if (open && !bool_SleeperOn)
        {
            obj_SingalUI.SetActive(true);
            obj_SingalUI.transform.localScale = Vector3.one;
            obj_SingalUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalUI.SetActive(false);
            });
        }
        obj_HightlightUI.transform.DOKill();
        if (open && !bool_SleeperOn)
        {
            obj_HightlightUI.SetActive(true);
            obj_HightlightUI.transform.localScale = Vector3.one;
            obj_HightlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_HightlightUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_HightlightUI.SetActive(false);
            });
        }
    }
    public override bool CanHighlight()
    {
        return !bool_SleeperOn;
    }
    #endregion
    #region//睡眠
    public void Local_StartingSleep(ActorManager who)
    {
        if (!bool_SleeperOn)
        {
            WriteInfo(who.actorNetManager.Object.Id);
            who.actorNetManager.RPC_Local_SetNetworkTransform(tran_SleepPos.position);
            who.actorNetManager.Local_AddBuff(1001, 0, buildingTile.tilePos);
            if (bedFace == BedFace.Left)
            {
                who.actorNetManager.RPC_Local_SetBodyAction((short)BodyAction.LayToLeft);
            }
            else if (bedFace == BedFace.Right)
            {
                who.actorNetManager.RPC_Local_SetBodyAction((short)BodyAction.LayToRight);
            }
            else if (bedFace == BedFace.Up)
            {
                who.actorNetManager.RPC_Local_SetBodyAction((short)BodyAction.LayToUp);
            }
        }
    }
    public void Local_EndingSleep(ActorManager who)
    {
        Debug.Log(who);
        if (networkId_SleeperNew.Equals(who.actorNetManager.Object.Id))
        {
            WriteInfo(new Fusion.NetworkId());
        }
    }
    #endregion
}
public enum BedFace
{
    Left, Right, Up
}