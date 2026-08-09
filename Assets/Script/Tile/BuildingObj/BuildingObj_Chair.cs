using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingObj_Chair : BuildingObj_Manmade
{
    #region 序列化字段
    public ChairFace chairFace;
    public Transform transform_Chair;
    #endregion
    protected GameObject obj_SingalUI_F;
    protected GameObject obj_HighlightUI;
    private ChairState chairState;
    private HashSet<ActorManager> actors_Nearby = new HashSet<ActorManager>();
    public BuildingData_Chair buildingData_Chair = new BuildingData_Chair();
    public override void All_OnDraw()
    {
        Around around = MapManager.Instance.CheckAround_Building(buildingTile.tilePos, (int id) => { return id == 3101; }, DirectionType.Four);
        transform_Chair.localPosition = Vector3.zero;
        if (chairState == ChairState.Empty)
        {
            if (around.D && chairFace == ChairFace.Down)
            {
                transform_Chair.localPosition = new Vector3(0, -0.625f, 0);
                return;
            }
            if (around.L && chairFace == ChairFace.Left)
            {
                transform_Chair.localPosition = new Vector3(-0.625f, 0, 0);
                return;
            }
            if (around.R && chairFace == ChairFace.Right)
            {
                transform_Chair.localPosition = new Vector3(0.625f, 0, 0);
                return;
            }
            if (around.U && chairFace == ChairFace.Up)
            {
                transform_Chair.localPosition = new Vector3(0, 0.625f, 0);
                return;
            }
        }
        base.All_OnDraw();
    }

    #region 信息上传与同步
    public override void All_OnRawDataUpdate()
    {
        buildingData_Chair.Deserialize(local_ByteData);
        CheckChairAround(buildingData_Chair.ReadSitter());
        base.All_OnRawDataUpdate();
    }
    public void TryToPush()
    {
        All_PushData(buildingData_Chair.Serialize());
    }
    #endregion
    #region 瓦片交互
    public override bool All_ActorNearby(ActorManager actor)
    {
        actors_Nearby.Add(actor);
        CheckChairAround(buildingData_Chair.ReadSitter());
        return true;
    }
    public override bool All_ActorFaraway(ActorManager actor)
    {
        actors_Nearby.Remove(actor);
        CheckChairAround(buildingData_Chair.ReadSitter());
        return true;
    }
    public override void Local_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            Local_StartingSit(actor);
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
        return chairState == ChairState.Empty;
    }
    #endregion
    #region 床铺操作
    public bool EqualsSiter(Fusion.NetworkId? id)
    {
        if (id == null) { return false; }
        else
        {
            return ((Fusion.NetworkId)id).Raw == buildingData_Chair.ReadSitter().Raw;
        }
    }
    public ChairState GetChairState()
    {
        return chairState;
    }
    public void CheckChairAround(Fusion.NetworkId sleeper)
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
            if (!findSleeper) Local_EndingSit(buildingData_Chair.ReadSitter());
        }
        ChangeChairState(findSleeper ? ChairState.Occupied : ChairState.Empty);
        if (chairState != ChairState.Empty) OpenOrCloseHighlightUI(false);
    }
    private void ChangeChairState(ChairState state)
    {
        chairState = state;
        All_OnDraw();
    }
    #endregion
    #region 坐下
    public void Local_StartingSit(ActorManager who)
    {
        if (chairState == ChairState.Empty)
        {
            buildingData_Chair.WriteSitter(who.actorNetManager.Object.Id);
            TryToPush();
            who.actorNetManager.Local_AddBuff(1002, 0, buildingTile.tilePos);
            switch (chairFace)
            {
                case ChairFace.Left:
                    {
                        who.actorNetManager.RPC_Local_SetNetworkTransform(transform.position + new Vector3(-0.15f, -0.05f, 0));
                        who.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.SitToLeft, 1, transform.position);
                        break;
                    }
                case ChairFace.Right:
                    {
                        who.actorNetManager.RPC_Local_SetNetworkTransform(transform.position + new Vector3(0.15f, -0.05f, 0));
                        who.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.SitToRight, 1, transform.position);
                        break;
                    }
                case ChairFace.Down:
                    {
                        who.actorNetManager.RPC_Local_SetNetworkTransform(transform.position + new Vector3(0, -0.05f, 0));
                        who.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.SitToDown, 1, transform.position);
                        break;
                    }
                case ChairFace.Up:
                    {
                        who.actorNetManager.RPC_Local_SetNetworkTransform(transform.position + new Vector3(0, 0, 0));
                        who.actorNetManager.RPC_Local_SetBodyAction((short)BodyActionType.SitToDown, 1, transform.position);
                        break;
                    }
            }
        }
    }
    public void Local_EndingSit(Fusion.NetworkId? networkId)
    {
        if (EqualsSiter(networkId))
        {
            buildingData_Chair.WriteSitter(new Fusion.NetworkId());
            TryToPush();
        }
    }
    #endregion

}
public enum ChairFace
{
    Left, Right, Down, Up
}
public enum ChairState
{
    Empty, Occupied
}
public class BuildingData_Chair
{
    public Fusion.NetworkId networkId_SitterCur;
    public Fusion.NetworkId ReadSitter() { return networkId_SitterCur; }
    public void WriteSitter(Fusion.NetworkId val) { networkId_SitterCur = val; }
    private const int MAX_ITEMS = 30;
    public List<ItemData> itemDatas_List = new List<ItemData>();
    public void WriteItemDataList(List<ItemData> itemDatas)
    {
        itemDatas_List.Clear();
        foreach (ItemData itemData in itemDatas)
        {
            itemDatas_List.Add(itemData);
        }
    }
    public void ReadItemDataList(out List<ItemData> itemDatas)
    {
        itemDatas = new List<ItemData>(itemDatas_List);
    }

    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(networkId_SitterCur.Raw);
            // 实际物品数量
            int count = itemDatas_List?.Count ?? 0;
            writer.Write((short)Math.Min(count, MAX_ITEMS));
            for (int i = 0; i < count && i < MAX_ITEMS; i++)
            {
                var item = itemDatas_List[i];
                writer.Write(item.I);
                writer.Write(item.C);
                writer.Write(item.V);
                writer.Write(item.D);
                writer.Write(item.S);
            }
            return ms.ToArray();
        }
    }
    public void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            itemDatas_List = new List<ItemData>();
            networkId_SitterCur = new Fusion.NetworkId();
            return;
        }
        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
            networkId_SitterCur.Raw = reader.ReadUInt32();
            short count = reader.ReadInt16();
            itemDatas_List = new List<ItemData>(count);
            for (int i = 0; i < count; i++)
            {
                var item = new ItemData
                {
                    I = reader.ReadInt16(),
                    C = reader.ReadInt16(),
                    V = reader.ReadInt16(),
                    D = reader.ReadSByte(),
                    S = reader.ReadInt16()
                };
                itemDatas_List.Add(item);
            }
        }
    }
}