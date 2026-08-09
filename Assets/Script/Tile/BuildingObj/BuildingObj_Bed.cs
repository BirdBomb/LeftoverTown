using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingObj_Bed : BuildingObj_Manmade
{
    #region 序列化字段
    public BedFace bedFace;
    #endregion
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_HighlightUI;
    private BedState bedState;
    private HashSet<ActorManager> actors_Nearby = new HashSet<ActorManager>();
    private float float_Timer;

    public BuildingData_Bed buildingData_Bed = new BuildingData_Bed();
    private void Update()
    {
        float_Timer += Time.deltaTime;
        if (float_Timer > 0.2) { float_Timer = 0; CheckBedAround(buildingData_Bed.ReadSleeper()); }
    }
    #region 信息上传与同步
    public override void All_OnRawDataUpdate()
    {
        buildingData_Bed.Deserialize(local_ByteData);
        CheckBedAround(buildingData_Bed.ReadSleeper());
        base.All_OnRawDataUpdate();
    }
    public void TryToPush()
    {
        All_PushData(buildingData_Bed.Serialize());
    }
    #endregion
    #region 瓦片交互
    public override bool All_ActorNearby(ActorManager actor)
    {
        actors_Nearby.Add(actor);
        return true;
    }
    public override bool All_ActorFaraway(ActorManager actor)
    {
        actors_Nearby.Remove(actor);
        return true;
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
    public override void OpenOrCloseHighlightUI(bool open)
    {
        if (open)
        {
            if (obj_SingalUI_F == null)
            {
                obj_SingalUI_F = PoolManager.Instance.GetObject("UI/TileUI/SignalUI_F");
                obj_SingalUI_F.transform.position = transform.position + All_GetTileGenter();
                obj_SingalUI_F.transform.localScale = Vector3.one;
                obj_SingalUI_F.transform.DOKill();
                obj_SingalUI_F.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
            if (obj_HighlightUI == null)
            {
                obj_HighlightUI = PoolManager.Instance.GetObject("UI/TileUI/" + All_GetTileSize());
                obj_HighlightUI.transform.position = transform.position;
                obj_HighlightUI.transform.localScale = Vector3.one;
                obj_HighlightUI.transform.DOKill();
                obj_HighlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_F", obj_SingalUI_F);
            obj_SingalUI_F = null;
            PoolManager.Instance.ReleaseObject("UI/TileUI/" + All_GetTileSize(), obj_HighlightUI);
            obj_HighlightUI = null;
        }
    }
    public override bool CanHighlight()
    {
        return bedState == BedState.Empty;
    }
    #endregion
    #region 床铺操作
    public bool EqualsSleeper(Fusion.NetworkId? id)
    {
        if(id == null) { return false; }
        else
        {
            return ((Fusion.NetworkId)id).Raw == buildingData_Bed.ReadSleeper().Raw;
        }
    }
    public BedState GetBedState()
    {
        return bedState;
    }
    public void CheckBedAround(Fusion.NetworkId sleeper)
    {
        bool findSleeper = false;
        foreach (ActorManager actorManager in actors_Nearby)
        {
            if (actorManager != null)
            {
                if (actorManager.actorNetManager.Object.Id.Equals(sleeper))
                {
                    findSleeper = true;
                }
            }
        }
        if (sleeper != new Fusion.NetworkId())
        {
            if (!findSleeper) Local_EndingSleep(buildingData_Bed.ReadSleeper());
        }
        ChangeBedState(findSleeper ? BedState.Occupied : BedState.Empty);
        if (bedState != BedState.Empty) OpenOrCloseHighlightUI(false);
    }
    private void ChangeBedState(BedState state)
    {
        bedState = state;
    }
    #endregion
    #region 睡眠
    public void Local_StartingSleep(ActorManager who)
    {
        if (bedState == BedState.Empty)
        {
            buildingData_Bed.WriteSleeper(who.actorNetManager.Object.Id);
            TryToPush();
            who.actorNetManager.Local_AddBuff(1001, 0, buildingTile.tilePos);
            switch (bedFace) 
            {
                case BedFace.Left:
                    {
                        who.actorNetManager.RPC_Local_SetNetworkTransform(transform.position + new Vector3(0.5f, 0, 0));
                        who.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.LayToLeft, 1, transform.position + new Vector3(0.5f, 0, 0));
                        break;
                    }
                case BedFace.Right:
                    {
                        who.actorNetManager.RPC_Local_SetNetworkTransform(transform.position + new Vector3(0.5f, 0, 0));
                        who.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.LayToRight, 1, transform.position + new Vector3(0.5f, 0, 0));
                        break;
                    }
                case BedFace.Up:
                    {
                        who.actorNetManager.RPC_Local_SetNetworkTransform(transform.position + new Vector3(0, 0.5f, 0));
                        who.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.LayToUp, 1, transform.position + new Vector3(0.5f, 0, 0));
                        break;
                    }
            }
        }
    }
    public void Local_EndingSleep(Fusion.NetworkId? networkId)
    {
        if (EqualsSleeper(networkId))
        {
            buildingData_Bed.WriteSleeper(new Fusion.NetworkId());
            TryToPush();
        }
    }
    #endregion
}
public enum BedFace
{
    Left, Right, Up
}
public enum BedState
{
    Empty, Occupied
}
public class BuildingData_Bed
{
    public Fusion.NetworkId networkId_SleeperCur;
    public Fusion.NetworkId ReadSleeper() { return networkId_SleeperCur; }
    public void WriteSleeper(Fusion.NetworkId val) { networkId_SleeperCur = val; }
    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(networkId_SleeperCur.Raw);
            return ms.ToArray();
        }
    }
    public void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            networkId_SleeperCur = new Fusion.NetworkId();
            return;
        }

        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
            networkId_SleeperCur.Raw = reader.ReadUInt32();
        }
    }
}