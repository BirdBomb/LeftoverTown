using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using static UnityEditor.PlayerSettings;
using System.Threading.Tasks;
using System;

public class MapNetManager : NetworkBehaviour
{
    [SerializeField]
    public MapManager mapManager;
    public MapData mapData = null;
    private bool mapDataAlready = false;
    void Start()
    {
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_RequestMapData>().Subscribe(_ =>
        {
            TryRequestMapData(_.pos, _.player);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_TakeDamage>().Subscribe(_ =>
        {
            RPC_LocalInput_TileTakeDamage(_.tileObj.bindTile.posInCell, _.damage, Runner.LocalPlayer);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_UpdateInfo>().Subscribe(_ =>
        {
            RPC_LocalInput_TileUpdateInfo(_.tileObj.bindTile.posInCell, _.tileInfo);
        }).AddTo(this);
    }
    #region//请求与返回地图数据
    private void TryRequestMapData(Vector3Int pos,PlayerRef player)
    {
        Debug.Log("尝试请求服务器地图信息/位置坐标" + pos);
        RPC_RequestMapData(pos, player);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private async void RPC_RequestMapData(Vector3Int pos, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            Debug.Log("服务器尝试发送地图数据");
            if (!mapDataAlready)
            {
                Debug.Log("服务器地图未初始化,先初始化");
                mapDataAlready = true;
                await LoadMap();
                await TryInitMapForSomeone(pos, 19, player);
            }
            else
            {
                Debug.Log("服务器地图已经初始化");
                await TryInitMapForSomeone(pos, 19, player);
            }
        }
    }
    private async Task LoadMap()
    {
        mapData = GameDataManager.Instance.LoadMap();
        await Task.Delay(100);
        Debug.Log("服务器地图初始化成功");
    }
    /// <summary>
    /// 为某人初始化地图
    /// </summary>
    /// <param name="center">中心</param>
    /// <param name="center">尺寸</param>
    /// <param name="player">目标玩家</param>
    /// <returns></returns>
    private async Task TryInitMapForSomeone(Vector3Int center,int size, PlayerRef player)
    {
        for(int i = 0; i < 9; i++)
        {
            Vector3Int tempPos = new Vector3Int();
            if (i == 0) { tempPos = center; }
            if (i == 1) { tempPos = center + new Vector3Int(size, 0, 0); }
            if (i == 2) { tempPos = center + new Vector3Int(-size, 0, 0); }
            if (i == 3) { tempPos = center + new Vector3Int(0, size, 0); }
            if (i == 4) { tempPos = center + new Vector3Int(0, -size, 0); }
            if (i == 5) { tempPos = center + new Vector3Int(size, size, 0); }
            if (i == 6) { tempPos = center + new Vector3Int(-size, size, 0); }
            if (i == 7) { tempPos = center + new Vector3Int(-size, -size, 0); }
            if (i == 8) { tempPos = center + new Vector3Int(size, -size, 0); }
            TrySendMapData(tempPos, size, size, player);
            await Task.Delay(1000);
        }
    }
    /// <summary>
    /// 发送区域信息数据
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="width">区域宽</param>
    /// <param name="height">区域高</param>
    /// <param name="player">目标客户端</param>
    private void TrySendMapData(Vector3Int center, int width, int height, PlayerRef player)
    {
        string[] tempArray = new string[width * height];
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                string info = mapData.tiles[(center.x + x).ToString() + "," + (center.y + y).ToString()];
                tempArray[index] = info;
                index++;
            }
        }
        RPC_SendMapData(player, tempArray, center, width, height);
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SendMapData([RpcTarget] PlayerRef target, string[] tileList, Vector3Int center, int width, int height)
    {
        Debug.Log("获取服务器地图数据:中心" + center + "尺寸" + width + "/" + height);
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                mapManager.CreateTile(new Vector3Int(center.x + x, center.y + y, 0), tileList[index]);
                index++;
            }
        }
    }
    #endregion

    /// <summary>
    /// 客户端输入对地块的攻击
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="damage"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_TileTakeDamage(Vector3Int pos, int damage, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            mapManager.GetTileObj(pos, out TileObj obj);
            int Hp = obj.CurHp - damage;
            RPC_StateCall_TileUpdateHp(pos, Hp, player);
            if (Hp <= 0)
            {
                RPC_StateCall_TileBreak(pos);
            }
        }
    }
    /// <summary>
    /// 客户端输入对地块的更新
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_TileUpdateInfo(Vector3Int pos, string info)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_TileUpdateInfo(pos, info);
        }
    }


    /// <summary>
    /// 服务器更新地块生命值
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="hp"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_TileUpdateHp(Vector3Int pos, int hp, PlayerRef player)
    {
        mapManager.GetTileObj(pos, out TileObj obj);
        obj.TryToUpdateHp(hp);
    }
    /// <summary>
    /// 服务器摧毁地块
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_TileBreak(Vector3Int pos)
    {
        mapManager.GetTileObj(pos, out TileObj obj);
        obj.TryBreak();
        mapManager.CreateTile(pos, "Default");
    }
    /// <summary>
    /// 服务器更新地块信息
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_TileUpdateInfo(Vector3Int pos, string info)
    {
        mapManager.GetTileObj(pos, out TileObj obj);
        obj.TryToUpdateInfo(info);
    }
}
