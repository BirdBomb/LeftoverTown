using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using System.Threading.Tasks;
using static UnityEditor.PlayerSettings;
using System.Numerics;
using System.Text;

public class MapNetManager : NetworkBehaviour
{
    public MapTileTypeData buildingTileTypeData = null;
    public MapTileInfoData buildingTileInfoData = null;
    public MapTileTypeData floorTileTypeData = null;
    private bool mapDataAlready = false;
    private int mapSeed;
    void Start()
    {
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_RequestMapData>().Subscribe(_ =>
        {
            Local_RequestMapData(_.playerPos, _.playerRef);
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
            Debug.Log(_.tileObj.bindTile._posInCell);
            if (_.tileObj.bindTile == null) { Debug.Log("ss1"); }
            if (Runner.LocalPlayer == null) { Debug.Log("aaa"); }
            RPC_LocalInput_TileTakeDamage(_.tileObj.bindTile._posInCell, _.damage, Runner.LocalPlayer);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_UpdateBuildingInfo>().Subscribe(_ =>
        {
            RPC_LocalInput_TileUpdateInfo(_.tileObj.bindTile._posInCell, _.tileInfo);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_ChangeBuilding>().Subscribe(_ =>
        {
            RPC_LocalInput_BuildingChange(_.buildingPos, _.buildingID);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_ChangeFloor>().Subscribe(_ =>
        {
            RPC_LocalInput_FloorChange(_.floorPos, _.floorID);
        }).AddTo(this);
    }
    public override void Spawned()
    {
        base.Spawned();
    }


    #region//保存和读取
    private async Task LoadMap()
    {
        GameDataManager.Instance.LoadMap(out MapTileTypeData buildingTypeData, out MapTileInfoData buildingInfoData, out MapTileTypeData floorTypeData);
        buildingTileTypeData = buildingTypeData;
        buildingTileInfoData = buildingInfoData;
        floorTileTypeData = floorTypeData;

        await Task.Delay(100);
        Debug.Log("服务器地图初始化成功");
        InitMap();
    }
    private void SaveMap()
    {
        if (buildingTileTypeData != null && buildingTileInfoData != null && floorTileTypeData != null)
        {
            GameDataManager.Instance.SaveMap(buildingTileTypeData, buildingTileInfoData, floorTileTypeData);
            Debug.Log("服务器地图保存成功");
        }
    }
    private void InitMap()
    {
        InitLoop();
        InitSeed();
    }
    private void InitLoop()
    {
        Debug.Log("开始计算世界时间");
        Hour = buildingTileInfoData.hour;
        Date = buildingTileInfoData.date;
        InvokeRepeating("UpdateTime", 5, 5);
    }
    private void InitSeed()
    {
        string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int seedInt = 0;
        StringBuilder seedStr = new StringBuilder(buildingTileInfoData.seed);
        for (int i = 0; i < seedStr.Length; i++)
        {
            char c = seedStr[i];
            int temp = str.IndexOf(c);
            seedInt += temp * (int)Mathf.Pow(10, i);
        }
        mapSeed = seedInt;
    }
    #endregion
    #region//时间周期
    public void UpdateTime()
    {
        if (Object.HasStateAuthority)
        {
            if (Hour < 10)
            {
                Hour += 1;
            }
            else
            {
                Date += 1;
                Hour = 0;
            }
            buildingTileInfoData.hour = Hour;
            buildingTileInfoData.date = Date;
        }
    }
    #endregion
    #region//本地端
    /// <summary>
    /// 本地端请求地图数据
    /// </summary>
    /// <param name="center">区域中心(只能是20的倍数)</param>
    /// <param name="player"></param>
    private void Local_RequestMapData(Vector3Int center, PlayerRef player)
    {
        Debug.Log("请求服务器地图信息/位置坐标" + center);
        RPC_LocalInput_RequestMapData(center, player);
    }
    /// <summary>
    /// 本地端更新地图中心
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_UpdateMapCenter(Vector3Int center, int width, int height, int seed)
    {
        Debug.Log("收到服务器地图信息(地图绘制中心)/中心" + center + "尺寸" + width + "/" + height);
        MapManager.Instance.UpdateMapCenter(center, width, height, seed);
    }
    /// <summary>
    /// 本地端获得地图数据(建筑类别)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveBuildTypeData(int[] tileList, Vector3Int center, int width, int height)
    {
        //Debug.Log("收到服务器地图信息(建筑类别)/中心" + center + "尺寸" + width + "/" + height);
        if (MapManager.Instance.AddNewAreaInMap(center, width, height))
        {
            int index = 0;
            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = -height / 2; y < height / 2; y++)
                {
                    MapManager.Instance.CreateBuilding(new Vector3Int(center.x + x, center.y + y, 0), tileList[index]);
                    index++;
                }
            }
        }
    }
    /// <summary>
    /// 本地端获得地图数据(建筑信息)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveBuildInfoData(string[] tileList, Vector3Int center, int width, int height)
    {
        Debug.Log("收到服务器地图信息(建筑信息)/中心" + center + "尺寸" + width + "/" + height);
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                if (tileList[index].Length > 0)
                {
                    MapManager.Instance.GetBuildObj(new Vector3Int(center.x + x, center.y + y, 0), out TileObj obj);
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
    /// 本地端获得地图数据(地块类别)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveFloorTypeData(int[] tileList, Vector3Int center, int width, int height)
    {
        Debug.Log("收到服务器地图信息(地块类别)/中心" + center + "尺寸" + width + "/" + height);
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                MapManager.Instance.CreateFloor(new Vector3Int(center.x + x, center.y + y, 0), tileList[index]);
                index++;
            }
        }
    }
    #endregion
    #region//服务端
    /// <summary>
    /// 为某人初始化地图
    /// </summary>
    /// <param name="center">中心</param>
    /// <param name="center">尺寸</param>
    /// <param name="player">目标玩家</param>
    /// <returns></returns>
    private async Task State_TryInitMapForSomeone(Vector3Int center, int size, PlayerRef player)
    {
        State_TrySendMapCenter(center, size, size, player, mapSeed);
        for (int i = 0; i < 25; i++)
        {
            Vector3Int tempPos = new Vector3Int();
            switch (i)
            {
                case 0:
                    tempPos = center;
                    break;
                case 1:
                    tempPos = center + new Vector3Int(size, 0, 0);
                    break;
                case 2:
                    tempPos = center + new Vector3Int(size, size, 0);
                    break;
                case 3:
                    tempPos = center + new Vector3Int(0, size, 0);
                    break;
                case 4:
                    tempPos = center + new Vector3Int(-size, size, 0);
                    break;
                case 5:
                    tempPos = center + new Vector3Int(-size, 0, 0);
                    break;
                case 6:
                    tempPos = center + new Vector3Int(-size, -size, 0);
                    break;
                case 7:
                    tempPos = center + new Vector3Int(0, -size, 0);
                    break;
                case 8:
                    tempPos = center + new Vector3Int(size, -size, 0);
                    break;
                case 9:
                    tempPos = center + new Vector3Int(size * 2, -size * 2, 0);
                    break;
                case 10:
                    tempPos = center + new Vector3Int(size * 2, -size, 0);
                    break;
                case 11:
                    tempPos = center + new Vector3Int(size * 2, 0, 0);
                    break;
                case 12:
                    tempPos = center + new Vector3Int(size * 2, size, 0);
                    break;
                case 13:
                    tempPos = center + new Vector3Int(size * 2, size * 2, 0);
                    break;
                case 14:
                    tempPos = center + new Vector3Int(size, size * 2, 0);
                    break;
                case 15:
                    tempPos = center + new Vector3Int(0, size * 2, 0);
                    break;
                case 16:
                    tempPos = center + new Vector3Int(-size, size * 2, 0);
                    break;
                case 17:
                    tempPos = center + new Vector3Int(-size * 2, size * 2, 0);
                    break;
                case 18:
                    tempPos = center + new Vector3Int(-size * 2, size, 0);
                    break;
                case 19:
                    tempPos = center + new Vector3Int(-size * 2, 0, 0);
                    break;
                case 20:
                    tempPos = center + new Vector3Int(-size * 2, -size, 0);
                    break;
                case 21:
                    tempPos = center + new Vector3Int(-size * 2, -size * 2, 0);
                    break;
                case 22:
                    tempPos = center + new Vector3Int(-size, -size * 2, 0);
                    break;
                case 23:
                    tempPos = center + new Vector3Int(0, -size * 2, 0);
                    break;
                case 24:
                    tempPos = center + new Vector3Int(size, -size * 2, 0);
                    break;
            }
            await Task.Delay(200);
            State_TrySendBuildingTileTypeData(tempPos, size, size, player);
            State_TrySendFloorTileTypeData(tempPos, size, size, player);
            State_TrySendMapTileInfoData(tempPos, size, size, player);
        }
    }
    private void State_TrySendMapCenter(Vector3Int center, int width, int height, PlayerRef player, int seed)
    {
        RPC_StateCall_UpdateMapCenter(player, center, width, height, seed);
    }
    private string tempKey;
    /// <summary>
    /// 发送建筑地块类型数据
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="width">区域宽</param>
    /// <param name="height">区域高</param>
    /// <param name="player">目标客户端</param>
    private void State_TrySendBuildingTileTypeData(Vector3Int center, int width, int height, PlayerRef player)
    {
        int[] tempTileArray = new int[width * height];
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int tempX = center.x + x + 30000;
                int tempY = center.y + y + 30000;
                int tempIndex;
                if (tempY > tempX)
                {
                    tempIndex = tempY * tempY + tempY + tempY - tempX;
                }
                else
                {
                    tempIndex = tempX * tempX + tempY;
                }

                if (buildingTileTypeData.tileDic.ContainsKey(tempIndex))
                {
                    int tileType = buildingTileTypeData.tileDic[tempIndex];
                    tempTileArray[index] = tileType;
                }
                else
                {
                    buildingTileTypeData.tileDic.Add(tempIndex, 0);
                    tempTileArray[index] = 0;
                }
                index++;
            }
        }

        RPC_StateCall_SendBuildingTileTypeData(player, tempTileArray, center, width, height);
    }
    /// <summary>
    /// 发送地板地块类型数据
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="player"></param>
    private void State_TrySendFloorTileTypeData(Vector3Int center, int width, int height, PlayerRef player)
    {
        int[] tempTileArray = new int[width * height];
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int tempX = center.x + x + 30000;
                int tempY = center.y + y + 30000;
                int tempIndex;
                if (tempY > tempX)
                {
                    tempIndex = tempY * tempY + tempY + tempY - tempX;
                }
                else
                {
                    tempIndex = tempX * tempX + tempY;
                }

                if (floorTileTypeData.tileDic.ContainsKey(tempIndex))
                {
                    int tileType = floorTileTypeData.tileDic[tempIndex];
                    tempTileArray[index] = tileType;
                }
                else
                {
                    floorTileTypeData.tileDic.Add(tempIndex, 1001);
                    tempTileArray[index] = 1001;
                }
                index++;
            }
        }
        RPC_StateCall_SendFloorTileTypeData(player, tempTileArray, center, width, height);
    }
    /// <summary>
    /// 发送建筑地块信息数据
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="player"></param>
    private void State_TrySendMapTileInfoData(Vector3Int center, int width, int height, PlayerRef player)
    {
        string[] tempTileArray = new string[width * height];
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int tempX = center.x + x + 30000;
                int tempY = center.y + y + 30000;
                int tempIndex;
                if (tempY > tempX)
                {
                    tempIndex = tempY * tempY + tempY + tempY - tempX;
                }
                else
                {
                    tempIndex = tempX * tempX + tempY;
                }

                if (buildingTileInfoData.tileDic.ContainsKey(tempIndex))
                {
                    string tileInfo = buildingTileInfoData.tileDic[tempIndex];
                    tempTileArray[index] = tileInfo;
                }
                else
                {
                    buildingTileInfoData.tileDic.Add(tempIndex, "");
                    tempTileArray[index] = "";
                }
                index++;
            }
        }
        RPC_StateCall_SendBuildingTileInfoData(player, tempTileArray, center, width, height);
    }

    #endregion

    #region//RPC
    /// <summary>
    /// 客户端请求地图数据
    /// </summary>
    /// <param name="center"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private async void RPC_LocalInput_RequestMapData(Vector3Int center, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            Debug.Log("服务器尝试发送地图数据");
            if (!mapDataAlready)
            {
                Debug.Log("服务器地图未初始化,先初始化");
                mapDataAlready = true;
                await LoadMap();
            }
            else
            {
                Debug.Log("服务器地图已经初始化");
            }
            await State_TryInitMapForSomeone(center, 20, player);
        }
    }
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
            if (MapManager.Instance.GetBuildObj(pos, out TileObj obj))
            {
                int Hp = obj.CurHp - damage;
                RPC_StateCall_TileUpdateHp(pos, Hp, player);
                if (Hp <= 0)
                {
                    RPC_LocalInput_BuildingChange(pos, 0);
                }
            }
            else
            {
                Debug.LogError("未找到目标tile(" + pos + ")");
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
            int tempX = pos.x + 30000;
            int tempY = pos.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            buildingTileInfoData.tileDic[tempIndex] = info;
            Debug.Log(info);
        }
    }
    /// <summary>
    /// 客户端输入对地块的改变
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_BuildingChange(Vector3Int pos, int id)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_BuildingChange(pos, id);
            int tempX = pos.x + 30000;
            int tempY = pos.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            buildingTileTypeData.tileDic[tempIndex] = id;
        }
    }
    /// <summary>
    /// 客户端输入对地块的改变
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_FloorChange(Vector3Int pos, int id)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_FloorChange(pos, id);

            int tempX = pos.x + 30000;
            int tempY = pos.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }

            floorTileTypeData.tileDic[tempIndex] = id;
        }
    }

    /// <summary>
    /// 服务端发送地图中心
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_UpdateMapCenter([RpcTarget] PlayerRef target,Vector3Int center, int width, int height,int seed)
    {
        Local_UpdateMapCenter(center, width, height, seed);
    }
    /// <summary>
    /// 服务端发送建筑类别数据
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendBuildingTileTypeData([RpcTarget] PlayerRef target, int[] tileList, Vector3Int center, int width, int height)
    {
        Local_ReceiveBuildTypeData(tileList, center, width, height);
    }
    /// <summary>
    /// 服务端发送建筑信息数据
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendBuildingTileInfoData([RpcTarget] PlayerRef target, string[] tileList, Vector3Int center, int width, int height)
    {
        Local_ReceiveBuildInfoData(tileList, center, width, height);
    }
    /// <summary>
    /// 服务端发送地板类别数据
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendFloorTileTypeData([RpcTarget] PlayerRef target, int[] tileList, Vector3Int center, int width, int height)
    {
        Local_ReceiveFloorTypeData(tileList, center, width, height);
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
        MapManager.Instance.GetBuildObj(pos, out TileObj obj);
        obj.TryToUpdateHp(hp);
    }
    /// <summary>
    /// 服务器通知改变建筑
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_BuildingChange(Vector3Int pos, int id)
    {
        MapManager.Instance.GetBuildObj(pos, out TileObj obj);
        if (obj != null)
        {
            obj.TryToDestroyMyObj();
        }
        MapManager.Instance.CreateBuilding(pos, id);
    }
    /// <summary>
    /// 服务器通知更新建筑信息
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_BuildingUpdateInfo(Vector3Int pos, string info)
    {
        MapManager.Instance.GetBuildObj(pos, out TileObj obj);
        obj.TryToUpdateInfo(info);
    }
    /// <summary>
    /// 服务器通知改变地板
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_FloorChange(Vector3Int pos, int id)
    {
        MapManager.Instance.CreateFloor(pos, id);
    }

    #endregion
    #region//Network
    [Networked, OnChangedRender(nameof(OnHourChange)), HideInInspector]
    public int Hour { get; set; }
    [Networked, HideInInspector]
    public int Date { get; set; }

    public void OnHourChange()
    {
        WorldManager.Instance.UpdateTime(Hour, Date);
    }

    #endregion
}
