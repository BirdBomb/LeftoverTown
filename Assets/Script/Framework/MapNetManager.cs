using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using System.Threading.Tasks;

public class MapNetManager : NetworkBehaviour
{
    [SerializeField]
    public MapManager mapManager;
    public MapTileTypeData mapTileTypeData = null;
    public MapTileInfoData mapTileInfoData = null;
    private bool mapDataAlready = false;
    void Start()
    {
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_RequestMapData>().Subscribe(_ =>
        {
            TryRequestMapData(_.pos, _.player);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_SaveMapData>().Subscribe(_ =>
        {
            if (Object.HasStateAuthority)
            {
                SaveMap();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_TakeDamage>().Subscribe(_ =>
        {
            RPC_LocalInput_TileTakeDamage(_.tileObj.bindTile.posInCell, _.damage, Runner.LocalPlayer);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_UpdateInfo>().Subscribe(_ =>
        {
            RPC_LocalInput_TileUpdateInfo(_.tileObj.bindTile.posInCell, _.tileInfo);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_ChangeTile>().Subscribe(_ =>
        {
            RPC_LocalInput_TileChange(_.tilePos, _.tileName);
        }).AddTo(this);
    }
    #region//请求与返回地图数据
    private void TryRequestMapData(Vector3Int pos,PlayerRef player)
    {
        Debug.Log("尝试请求服务器地图信息/位置坐标" + pos);
        RPC_RequestMapData(pos, player);
    }
    /// <summary>
    /// 客户端请求地图数据
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="player"></param>
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
        GameDataManager.Instance.LoadMap(out MapTileTypeData tileTypeData, out MapTileInfoData tileInfoData);
        mapTileTypeData = tileTypeData;
        mapTileInfoData = tileInfoData;
        await Task.Delay(100);
        Debug.Log("服务器地图初始化成功");
    }
    private void SaveMap()
    {
        if (mapTileTypeData != null && mapTileInfoData != null)
        {
            GameDataManager.Instance.SaveMap(mapTileTypeData, mapTileInfoData);
            Debug.Log("服务器地图保存成功");
        }
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
            TrySendMapTileTypeData(tempPos, size, size, player);
            await Task.Delay(1000);
            TrySendMapTileInfoData(tempPos, size, size, player);
        }
    }
    /// <summary>
    /// 发送地块类型数据
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="width">区域宽</param>
    /// <param name="height">区域高</param>
    /// <param name="player">目标客户端</param>
    private void TrySendMapTileTypeData(Vector3Int center, int width, int height, PlayerRef player)
    {
        string[] tempArray = new string[width * height];
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                string info = mapTileTypeData.tiles[(center.x + x).ToString() + "," + (center.y + y).ToString()];
                tempArray[index] = info;
                index++;
            }
        }
        RPC_SendMapTileTypeData(player, tempArray, center, width, height);
    }
    /// <summary>
    /// 发送地块信息数据
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="player"></param>
    private void TrySendMapTileInfoData(Vector3Int center, int width, int height, PlayerRef player)
    {
        string[] tempArray = new string[width * height];
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                if(mapTileInfoData.tiles.ContainsKey((center.x + x).ToString() + "," + (center.y + y).ToString()))
                {
                    string info = mapTileInfoData.tiles[(center.x + x).ToString() + "," + (center.y + y).ToString()];
                    tempArray[index] = info;
                }
                else
                {
                    string info = "";
                    tempArray[index] = info;
                }
                index++;
            }
        }
        RPC_SendMapTileInfoData(player, tempArray, center, width, height);
    }
    /// <summary>
    /// 服务端发送地图数据
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SendMapTileTypeData([RpcTarget] PlayerRef target, string[] tileList, Vector3Int center, int width, int height)
    {
        //Debug.Log("获取服务器地图数据:中心" + center + "尺寸" + width + "/" + height);
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
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SendMapTileInfoData([RpcTarget] PlayerRef target, string[] tileList, Vector3Int center, int width, int height)
    {
        //Debug.Log("获取服务器地图数据:中心" + center + "尺寸" + width + "/" + height);
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                if (tileList[index].Length > 0)
                {
                    mapManager.GetTileObj(new Vector3Int(center.x + x, center.y + y, 0), out TileObj obj);
                    if (obj != null)
                    {
                        obj.TryToUpdateInfo(tileList[index]);
                    }
                }
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
                RPC_StateCall_TileChange(pos, "Default");
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
            mapTileInfoData.tiles[(pos.x).ToString() + "," + (pos.y).ToString()] = info;
            Debug.Log(info);
        }
    }
    /// <summary>
    /// 客户端输入对地块的改变
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_TileChange(Vector3Int pos, string name)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_TileChange(pos, name);
            mapTileTypeData.tiles[(pos.x).ToString() + "," + (pos.y).ToString()] = name;
            Debug.Log(name);
        }
    }

    /// <summary>
    /// 服务器通知更新地块生命值
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
    /// 服务器通知改变地块
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_TileChange(Vector3Int pos, string name)
    {
        mapManager.GetTileObj(pos, out TileObj obj);
        if (obj != null)
        {
            obj.TryBreak();
        }
        mapManager.CreateTile(pos, name);
    }
    /// <summary>
    /// 服务器通知更新地块信息
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_TileUpdateInfo(Vector3Int pos, string info)
    {
        mapManager.GetTileObj(pos, out TileObj obj);
        obj.TryToUpdateInfo(info);
    }
}
