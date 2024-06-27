using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using System.Threading.Tasks;

public class MapNetManager : NetworkBehaviour
{
    public MapTileTypeData buildingTileTypeData = null;
    public MapTileInfoData buildingTileInfoData = null;
    public MapTileInfoData floorTileTypeData = null;
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
            RPC_LocalInput_TileTakeDamage(_.tileObj.bindTile._posInCell, _.damage, Runner.LocalPlayer);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_UpdateBuildingInfo>().Subscribe(_ =>
        {
            RPC_LocalInput_TileUpdateInfo(_.tileObj.bindTile._posInCell, _.tileInfo);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_ChangeBuilding>().Subscribe(_ =>
        {
            RPC_LocalInput_BuildingChange(_.buildingPos, _.buildingName);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_ChangeFloor>().Subscribe(_ =>
        {
            RPC_LocalInput_FloorChange(_.floorPos, _.floorName);
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
        GameDataManager.Instance.LoadMap(out MapTileTypeData buildingTypeData, out MapTileInfoData buildingInfoData,out MapTileInfoData flootInfoData);
        buildingTileTypeData = buildingTypeData;
        buildingTileInfoData = buildingInfoData;
        floorTileTypeData = flootInfoData;
        await Task.Delay(100);
        Debug.Log("服务器地图初始化成功");
    }
    private void SaveMap()
    {
        if (buildingTileTypeData != null && buildingTileInfoData != null && floorTileTypeData != null)
        {
            GameDataManager.Instance.SaveMap(buildingTileTypeData, buildingTileInfoData, floorTileTypeData);
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
            TrySendBuildingTileTypeData(tempPos, size, size, player);
            TrySendFloorTileTypeData(tempPos, size, size, player);
            await Task.Delay(1000);
            TrySendMapTileInfoData(tempPos, size, size, player);
        }
    }
    /// <summary>
    /// 发送建筑地块类型数据
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="width">区域宽</param>
    /// <param name="height">区域高</param>
    /// <param name="player">目标客户端</param>
    private void TrySendBuildingTileTypeData(Vector3Int center, int width, int height, PlayerRef player)
    {
        string[] tempArray = new string[width * height];
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                string info = buildingTileTypeData.tiles[(center.x + x).ToString() + "," + (center.y + y).ToString()];
                tempArray[index] = info;
                index++;
            }
        }
        RPC_SendBuildingTileTypeData(player, tempArray, center, width, height);
    }
    /// <summary>
    /// 发送地板地块类型数据
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="player"></param>
    private void TrySendFloorTileTypeData(Vector3Int center, int width, int height, PlayerRef player)
    {
        string[] tempArray = new string[width * height];
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                string info;
                if (floorTileTypeData.tiles.ContainsKey((center.x + x).ToString() + "," + (center.y + y).ToString()))
                {
                    info = floorTileTypeData.tiles[(center.x + x).ToString() + "," + (center.y + y).ToString()];
                }
                else
                {
                    info ="Grass";
                }
                tempArray[index] = info;
                index++;
            }
        }
        RPC_SendFloorTileTypeData(player, tempArray, center, width, height);
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
                if(buildingTileInfoData.tiles.ContainsKey((center.x + x).ToString() + "," + (center.y + y).ToString()))
                {
                    string info = buildingTileInfoData.tiles[(center.x + x).ToString() + "," + (center.y + y).ToString()];
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
        RPC_SendBuildingTileInfoData(player, tempArray, center, width, height);
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
    private void RPC_SendBuildingTileTypeData([RpcTarget] PlayerRef target, string[] tileList, Vector3Int center, int width, int height)
    {
        //Debug.Log("获取服务器地图数据:中心" + center + "尺寸" + width + "/" + height);
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                MapManager.Instance.CreateBuilding(new Vector3Int(center.x + x, center.y + y, 0), tileList[index]);
                index++;
            }
        }
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SendBuildingTileInfoData([RpcTarget] PlayerRef target, string[] tileList, Vector3Int center, int width, int height)
    {
        //Debug.Log("获取服务器地图数据:中心" + center + "尺寸" + width + "/" + height);
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                if (tileList[index].Length > 0)
                {
                    MapManager.Instance.GetTileObj(new Vector3Int(center.x + x, center.y + y, 0), out TileObj obj);
                    if (obj != null)
                    {
                        obj.TryToUpdateInfo(tileList[index]);
                    }
                }
                index++;
            }
        }
    }
    /// <summary>
    /// 服务端发送地板数据
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SendFloorTileTypeData([RpcTarget] PlayerRef target, string[] tileList, Vector3Int center, int width, int height)
    {
        //Debug.Log("获取服务器地图数据:中心" + center + "尺寸" + width + "/" + height);
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                MapManager.Instance.CreateFloor(new Vector3Int(center.x + x, center.y + y, 0), tileList[index]);
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
            MapManager.Instance.GetTileObj(pos, out TileObj obj);
            int Hp = obj.CurHp - damage;
            RPC_StateCall_TileUpdateHp(pos, Hp, player);
            if (Hp <= 0)
            {
                RPC_StateCall_BuildingChange(pos, "Default");
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
            RPC_StateCall_BuildingUpdateInfo(pos, info);
            buildingTileInfoData.tiles[(pos.x).ToString() + "," + (pos.y).ToString()] = info;
            Debug.Log(info);
        }
    }
    /// <summary>
    /// 客户端输入对地块的改变
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_BuildingChange(Vector3Int pos, string name)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_BuildingChange(pos, name);
            buildingTileTypeData.tiles[(pos.x).ToString() + "," + (pos.y).ToString()] = name;
            Debug.Log(name);
        }
    }
    /// <summary>
    /// 客户端输入对地块的改变
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_FloorChange(Vector3Int pos, string name)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_FloorChange(pos, name);
            floorTileTypeData.tiles[(pos.x).ToString() + "," + (pos.y).ToString()] = name;
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
        MapManager.Instance.GetTileObj(pos, out TileObj obj);
        obj.TryToUpdateHp(hp);
    }
    /// <summary>
    /// 服务器通知改变建筑
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_BuildingChange(Vector3Int pos, string name)
    {
        MapManager.Instance.GetTileObj(pos, out TileObj obj);
        if (obj != null)
        {
            obj.ListenDestroyMyObj();
        }
        MapManager.Instance.CreateBuilding(pos, name);
    }
    /// <summary>
    /// 服务器通知更新建筑信息
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_BuildingUpdateInfo(Vector3Int pos, string info)
    {
        MapManager.Instance.GetTileObj(pos, out TileObj obj);
        obj.TryToUpdateInfo(info);
    }
    /// <summary>
    /// 服务器通知改变地板
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_FloorChange(Vector3Int pos, string name)
    {
        MapManager.Instance.CreateFloor(pos, name);
    }

}
