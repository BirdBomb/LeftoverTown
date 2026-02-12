using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using UnityEngine.UIElements;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Drawing;

public class GameNetManager : NetworkBehaviour
{
    public void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SpawnActor>().Subscribe(_ =>
        {
            RPC_Local_SpawnActor(_.name, _.pos);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SpawnItem>().Subscribe(_ =>
        {
            RPC_Local_SpawnItem(_.itemData, _.itemOwner, _.pos);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_SpawnItem>().Subscribe(_ =>
        {
            SpawnItem(_.itemData, _.itemOwner, _.pos);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_SpawnActor>().Subscribe(_ =>
        {
            SpawnActor(_.name, _.pos, _.callBack);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_SaveMapData>().Subscribe(_ =>
        {
            if (Object.HasStateAuthority)
            {
                SaveMap();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_ChangeBuildingHp>().Subscribe(_ =>
        {
            RPC_LocalInput_ChangeBuildingHp(_.pos, _.offset, Runner.LocalPlayer);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_ChangeBuildingInfo>().Subscribe(_ =>
        {
            RPC_LocalInput_ChangeBuildingInfo(_.pos, _.info);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_CreateBuildingArea>().Subscribe(_ =>
        {
            RPC_LocalInput_CreateBuildingArea(_.buildingPos, _.buildingID, (int)_.areaSize);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_State_CreateBuildingArea>().Subscribe(_ =>
        {
            if (Object.HasStateAuthority)
            {
                OnlyState_CreateBuildingArea(_.buildingPos, _.buildingID, _.areaSize);
            }
        }).AddTo(this);

        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_CreateGround>().Subscribe(_ =>
        {
            RPC_LocalInput_CreateFloor(_.groundPos, _.groundID);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_ChangeSunLight>().Subscribe(_ =>
        {
            RPC_LocalInput_ChangeSun(_.distance);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_AddOneHour>().Subscribe(_ =>
        {
            AddOneHour();
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_ChangeTime>().Subscribe(_ =>
        {
            ChangeTime(_.hour);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_ChangeWeather>().Subscribe(_ =>
        {
            ChangeWeather(_.index);
        }).AddTo(this);

    }

    public override void Spawned()
    {
        base.Spawned();
        OnSecondChange();
        OnHourChange();
        OnWeatherChange();
        OnDistanceChange();
    }

    #region//创建角色与物体
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="name"></param>
    /// <param name="postion"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_SpawnActor(string name, Vector3 postion)
    {
        SpawnActor(name, postion, null);
    }
    private void SpawnActor(string name, Vector3 postion, System.Action<ActorManager> action)
    {
        if (Object.HasStateAuthority)
        {
            GameObject obj = Resources.Load<GameObject>(name);
            NetworkObject networkObject = Runner.Spawn(obj, postion, Quaternion.identity);
            networkObject.AssignInputAuthority(Runner.LocalPlayer);
            if (action != null)
            {
                action.Invoke(networkObject.GetComponent<ActorManager>());
            }
        }
    }
    /// <summary>
    /// 创建物体
    /// </summary>
    /// <param name="data"></param>
    /// <param name="owner"></param>
    /// <param name="postion"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_SpawnItem(ItemData data, NetworkId owner, Vector3 postion)
    {
        SpawnItem(data, owner, postion);
    }
    private void SpawnItem(ItemData data, NetworkId owner, Vector3 postion)
    {
        if (Object.HasStateAuthority)
        {
            GameObject obj = Resources.Load<GameObject>("ItemObj/ItemNetObj");
            NetworkObject networkPlayerObject = Runner.Spawn(obj, postion, Quaternion.identity, Object.StateAuthority);
            networkPlayerObject.GetComponent<ItemNetObj>().State_Init(data);
            networkPlayerObject.GetComponent<ItemNetObj>().State_BindOwner(owner);
            //networkPlayerObject.GetComponent<ItemNetObj>().State_CombineItem();
        }
    }

    #endregion
    #region//地图保存和读取
    private MapInfoData bind_MapInfoData = null;
    private MapTileTypeData bind_BuildingTileTypeData = null;
    private MapTileInfoData bind_BuildingTileInfoData = null;
    private MapTileTypeData bind_GroundTileTypeData = null;
    private bool mapDataAlready = false;
    private int mapSeed;

    private async Task LoadMap()
    {
        GameDataManager.Instance.LoadMap(out MapInfoData mapInfoData, out MapTileTypeData buildingTypeData, out MapTileInfoData buildingInfoData, out MapTileTypeData floorTypeData);
        bind_MapInfoData = mapInfoData;
        bind_BuildingTileTypeData = buildingTypeData;
        bind_BuildingTileInfoData = buildingInfoData;
        bind_GroundTileTypeData = floorTypeData;
        await Task.Delay(100);
        Debug.Log("服务器地图初始化成功");
        InitMap();
    }
    private void SaveMap()
    {
        if (bind_BuildingTileTypeData != null && bind_BuildingTileInfoData != null && bind_GroundTileTypeData != null)
        {
            GameDataManager.Instance.SaveMap(bind_MapInfoData, bind_BuildingTileTypeData, bind_BuildingTileInfoData, bind_GroundTileTypeData);
            Debug.Log("服务器地图保存成功");
        }
    }
    private void InitMap()
    {
        InitLoop();
        InitSeed();
        InitDistance();
    }
    private void InitLoop()
    {
        Debug.Log("开始计算世界时间");
        Hour = bind_MapInfoData.hour;
        Day = bind_MapInfoData.date;
        InvokeRepeating("AddOneSecond", 1, 1);
    }
    private void InitSeed()
    {
        string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int seedInt = 0;
        StringBuilder seedStr = new StringBuilder(bind_MapInfoData.seed);
        for (int i = 0; i < seedStr.Length; i++)
        {
            char c = seedStr[i];
            int temp = str.IndexOf(c);
            seedInt += temp * (int)Mathf.Pow(10, i);
        }
        mapSeed = seedInt;
    }
    private void InitDistance()
    {
        Distance = bind_MapInfoData.distance;
    }
    #endregion
    #region//时间周期
    public void AddOneSecond()
    {
        if (Object.HasStateAuthority)
        {
            if (Second < 60)
            {
                Second += 1;
            }
            else
            {
                AddOneHour();
            }
        }
    }
    public void AddOneHour()
    {
        if (Object.HasStateAuthority)
        {
            if (Hour < 10)
            {
                Hour += 1;
            }
            else
            {
                AddOneDay();
            }
            Second = 0;
            bind_MapInfoData.hour = Hour;
        }
    }
    public void AddOneDay()
    {
        if (Object.HasStateAuthority)
        {
            Day += 1;
            Hour = 0;
            bind_MapInfoData.date = Day;
        }
    }
    public void ChangeTime(short val)
    {
        if (Object.HasStateAuthority)
        {
            if (Hour < val)
            {
                int offset = val - Hour;
                for (short i = 0; i < offset; i++)
                {
                    AddOneHour();
                }
            }
            else if (Hour > val)
            {
                int offset = val + 10 - Hour;
                for (short i = 0; i < offset; i++)
                {
                    AddOneHour();
                }
            }
        }
    }
    public void ChangeWeather(short index)
    {
        RPC_LocalInput_ChangeWeather(index);
    }
    #endregion
    #region//本地端
    /// <summary>
    /// 本地端请求地图数据
    /// </summary>
    /// <param name="center">区域中心(只能是20的倍数)</param>
    /// <param name="player"></param>
    public void Local_RequestMapData(Vector3Int center, short size)
    {
        RPC_LocalInput_RequestMapData(center, size, Runner.LocalPlayer);
    }
    /// <summary>
    /// 本地端更改地图中心
    /// </summary>
    /// <param name="center">区域中心(只能是20的倍数)</param>
    /// <param name="player"></param>
    public void Local_SendMapCenter(Vector2Int center, short size)
    {
        RPC_LocalInput_SendMapCneter(center, size, Runner.LocalPlayer);
    }
    /// <summary>
    /// 本地端更新地图中心
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_UpdateMapCenter(PlayerRef player, Vector3Int center, int width, int height, int seed)
    {
        //Debug.Log("收到服务器地图信息(地图绘制中心)/中心" + center + "尺寸" + width + "/" + height);
        MapManager.Instance.UpdatePlayerCenterInMap(player, center, width, height, seed);
    }
    /// <summary>
    /// 本地端获得地图数据(建筑类别)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveBuildTypeData(short[] tileTypeList, Vector3Int center, short width, short height)
    {
        //Debug.Log("收到服务器地图信息(建筑类别)/中心" + center + "尺寸" + width + "/" + height);
        if (MapManager.Instance.AddBuildingAreaInMap(center))
        {
            int index = 0;
            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = -height / 2; y < height / 2; y++)
                {
                    if (tileTypeList[index] > 0)
                    {
                        int x1 = center.x + x;
                        int y1 = center.y + y;
                        MapManager.Instance.InitBuilding(tileTypeList[index], new Vector3Int(x1, y1, 0), out BuildingTile buildingTile);
                    }
                    index++;
                }
            }
            MapManager.Instance.DrawBuilding(center, width + 2, height + 2);
        }
    }
    /// <summary>
    /// 本地端获得地图数据(建筑信息)
    /// </summary>
    private void Local_ReceiveBuildInfoData(string tileInfo, Vector2Int pos)
    {
        if (tileInfo.Length > 0)
        {
            MapManager.Instance.GetBuilding((Vector3Int)pos, out BuildingTile buildingTile);
            buildingTile.tileObj.All_UpdateInfo(tileInfo);
        }
    }
    /// <summary>
    /// 本地端获得地图数据(地块类别)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveGroundTypeData(short[] tileList, Vector3Int center, short width, short height)
    {
        //Debug.Log("收到服务器地图信息(地块类别)/中心" + center + "尺寸" + width + "/" + height);
        if (MapManager.Instance.AddGroundAreaInMap(center))
        {
            int index = 0;
            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = -height / 2; y < height / 2; y++)
                {
                    int x1 = center.x + x;
                    int y1 = center.y + y;
                    MapManager.Instance.CreateGround(tileList[index], new Vector3Int(x1, y1, 0));
                    index++;
                }
            }
            MapManager.Instance.DrawGround(center, width + 2, height + 2);
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
    private async Task State_TryInitMapForSomeone(Vector3Int center, short size, PlayerRef player)
    {
        State_TrySendGroundTileTypeData(center, size, size, player);
        State_TrySendBuildingTileTypeData(center, size, size, player);
        await State_TrySendBuildingTileInfoData(center, size, size, player);

    }
    /// <summary>
    /// 发送地图中心
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="player"></param>
    /// <param name="seed"></param>
    private void State_TrySendMapCenter(Vector3Int center, int width, int height, PlayerRef player, int seed)
    {
        RPC_StateCall_UpdateMapCenter(player, center, width, height, seed);
    }
    /// <summary>
    /// 发送建筑类型数据
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="width">区域宽</param>
    /// <param name="height">区域高</param>
    /// <param name="player">目标客户端</param>
    private void State_TrySendBuildingTileTypeData(Vector3Int center, short width, short height, PlayerRef player)
    {
        short[] tempTileTypeArray = new short[width * height];
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

                if (bind_BuildingTileTypeData.tileDic.ContainsKey(tempIndex))
                {
                    short tileType = bind_BuildingTileTypeData.tileDic[tempIndex];
                    tempTileTypeArray[index] = tileType;
                }
                else
                {
                    //bind_BuildingTileTypeData.tileDic.Add(tempIndex, 0);
                    tempTileTypeArray[index] = 0;
                }
                index++;
            }
        }
        RPC_StateCall_SendBuildingTileTypeData(player, tempTileTypeArray, (Vector2Int)center, width, height);
    }
    /// <summary>
    /// 发送建筑信息数据
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="width">区域宽</param>
    /// <param name="height">区域高</param>
    /// <param name="player">目标客户端</param>
    private async Task State_TrySendBuildingTileInfoData(Vector3Int center, short width, short height, PlayerRef player)
    {
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
                string tileInfo = "";
                if (bind_BuildingTileInfoData.tileDic.ContainsKey(tempIndex))
                {
                    tileInfo = bind_BuildingTileInfoData.tileDic[tempIndex];
                }
                RPC_StateCall_SendBuildingTileInfoData(player, tileInfo, new Vector2Int(center.x + x, center.y + y));
                index++;
            }
            await Task.Delay(1);
        }
    }
    /// <summary>
    /// 发送地面类型数据
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="player"></param>
    private void State_TrySendGroundTileTypeData(Vector3Int center, short width, short height, PlayerRef player)
    {
        short[] tempTileArray = new short[width * height];
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

                if (bind_GroundTileTypeData.tileDic.ContainsKey(tempIndex))
                {
                    short tileType = bind_GroundTileTypeData.tileDic[tempIndex];
                    tempTileArray[index] = tileType;
                }
                else
                {
                    //bind_GroundTileTypeData.tileDic.Add(tempIndex, 1001);
                    tempTileArray[index] = 9000;
                }
                index++;
            }
        }
        RPC_StateCall_SendGroundTileTypeData(player, tempTileArray, (Vector2Int)center, width, height);
    }
    #endregion

    #region//RPC
    /// <summary>
    /// 客户端请求地图数据
    /// </summary>
    /// <param name="center"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private async void RPC_LocalInput_RequestMapData(Vector3Int center, short size, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            //Debug.Log("服务器尝试发送地图数据");
            if (!mapDataAlready)
            {
                //Debug.Log("服务器地图未初始化,先初始化");
                mapDataAlready = true;
                await LoadMap();
            }
            else
            {
                //Debug.Log("服务器地图已经初始化");
            }
            await State_TryInitMapForSomeone(center, size, player);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_SendMapCneter(Vector2Int center, short size, PlayerRef player)
    {
        State_TrySendMapCenter((Vector3Int)center, size, size, player, mapSeed);
    }
    /// <summary>
    /// 客户端更改地块生命值
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="offset"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_ChangeBuildingHp(Vector3Int pos, int offset, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            if (MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile))
            {
                int newHp = buildingTile.tileObj.local_Hp + offset;
                RPC_StateCall_TileUpdateHp(pos, newHp, player);
            }
            else
            {
                Debug.Log("未找到目标tile(" + pos + ")");
            }
        }
    }
    /// <summary>
    /// 客户端输入对地块的更新
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_ChangeBuildingInfo(Vector3Int pos, string info)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_ChangeBuildingInfo(pos, info);
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
            bind_BuildingTileInfoData.tileDic[tempIndex] = info;
        }
    }
    /// <summary>
    /// 客户端输入对区域的改变
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_CreateBuildingArea(Vector3Int pos, short id, int size)
    {
        if (Object.HasStateAuthority)
        {
            OnlyState_CreateBuildingArea(pos, id, (AreaSize)size);
        }
    }
    /// <summary>
    /// 服务器改变一个区域的建筑
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="id"></param>
    /// <param name="size"></param>
    private void OnlyState_CreateBuildingArea(Vector3Int pos, short id, AreaSize size)
    {
        OnlyState_CreateBuilding(pos, id);
        switch (size)
        {
            case AreaSize._1X1:
                break;
            case AreaSize._1X2:
                OnlyState_CreateBuilding(pos + Vector3Int.up, 99);
                break;
            case AreaSize._2X1:
                OnlyState_CreateBuilding(pos + Vector3Int.right, 99);
                break;
            case AreaSize._2X2:
                OnlyState_CreateBuilding(pos + Vector3Int.up, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.right, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.up + Vector3Int.right, 99);
                break;
            case AreaSize._3X3:
                OnlyState_CreateBuilding(pos + Vector3Int.up, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.down, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.left, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.right, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.up + Vector3Int.left, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.up + Vector3Int.right, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.down + Vector3Int.left, 99);
                OnlyState_CreateBuilding(pos + Vector3Int.down + Vector3Int.right, 99);
                break;
        }
    }
    /// <summary>
    /// 服务器改变单个建筑
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="id"></param>
    private void OnlyState_CreateBuilding(Vector3Int pos, short id)
    {
        MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile);
        if (buildingTile != null)
        {
            AreaSize size = BuildingConfigData.GetBuildingConfig(buildingTile.tileID).Building_Size;
            switch (size)
            {
                case AreaSize._1X1:
                    break;
                case AreaSize._1X2:
                    OnlyState_CreateBuilding(pos + Vector3Int.up, 0);
                    break;
                case AreaSize._2X1:
                    OnlyState_CreateBuilding(pos + Vector3Int.right, 0);
                    break;
                case AreaSize._2X2:
                    OnlyState_CreateBuilding(pos + Vector3Int.up, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.right, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.up + Vector3Int.right, 0);
                    break;
                case AreaSize._3X3:
                    OnlyState_CreateBuilding(pos + Vector3Int.up, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.down, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.left, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.right, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.up + Vector3Int.right, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.up + Vector3Int.left, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.down + Vector3Int.right, 0);
                    OnlyState_CreateBuilding(pos + Vector3Int.down + Vector3Int.left, 0);
                    break;
            }

        }
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
        RPC_StateCall_CreateBuilding(pos, id);
        if (id > 0)
        {
            bind_BuildingTileTypeData.tileDic[tempIndex] = id;
        }
        else
        {
            bind_BuildingTileTypeData.tileDic.Remove(tempIndex);
        }
    }
    /// <summary>
    /// 客户端输入对地块的改变
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_CreateFloor(Vector3Int pos, short id)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_CreateGround(pos, id);

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

            bind_GroundTileTypeData.tileDic[tempIndex] = id;
        }
    }

    /// <summary>
    /// 服务端给某人发送地图中心
    /// </summary>
    /// <param name="target"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_UpdateMapCenter(/*[RpcTarget]*/PlayerRef target, Vector3Int center, int width, int height, int seed)
    {
        Local_UpdateMapCenter(target, center, width, height, seed);
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
    private void RPC_StateCall_SendBuildingTileTypeData(/*[RpcTarget]*/PlayerRef target, short[] tileTypeList, Vector2Int center, short width, short height)
    {
        Local_ReceiveBuildTypeData(tileTypeList, (Vector3Int)center, width, height);
    }
    /// <summary>
    /// 服务端发送建筑信息数据
    /// </summary>
    /// <param name="target"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendBuildingTileInfoData(/*[RpcTarget]*/PlayerRef target, string tileInfo, Vector2Int pos)
    {
        Local_ReceiveBuildInfoData(tileInfo, pos);
    }
    /// <summary>
    /// 服务端发送地面类别数据
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendGroundTileTypeData(/*[RpcTarget]*/ PlayerRef target, short[] tileList, Vector2Int center, short width, short height)
    {
        Local_ReceiveGroundTypeData(tileList, (Vector3Int)center, width, height);
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
        if (MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile))
        {
            buildingTile.tileObj.All_UpdateHP(hp);
        }
    }
    /// <summary>
    /// 服务器通知改变建筑
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_CreateBuilding(Vector3Int pos, int id)
    {
        MapManager.Instance.CreateBuilding(id, pos, out _);
        MapManager.Instance.DrawBuilding(pos, 2, 2);
    }
    /// <summary>
    /// 服务器通知更新建筑信息
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_ChangeBuildingInfo(Vector3Int pos, string info)
    {
        MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile);
        buildingTile.tileObj.All_UpdateInfo(info);
    }
    /// <summary>
    /// 服务器通知改变地板
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_CreateGround(Vector3Int pos, int id)
    {
        MapManager.Instance.CreateGround(id, pos);
        MapManager.Instance.DrawGround(pos, 2, 2);
    }
    /// <summary>
    /// 客户端改变太阳
    /// </summary>
    /// <param name="distance"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_ChangeSun(short distance)
    {
        bind_MapInfoData.distance = distance;
        Distance = distance;
    }
    /// <summary>
    /// 客户端改变天气
    /// </summary>
    /// <param name="distance"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_ChangeWeather(short index)
    {
        Weather = index;
    }

    #endregion
    #region//Network
    [Networked, OnChangedRender(nameof(OnSecondChange)), HideInInspector]
    public short Second { get; set; }
    [Networked, OnChangedRender(nameof(OnHourChange)), HideInInspector]
    public short Hour { get; set; }
    [Networked, HideInInspector]
    public short Day { get; set; }
    [Networked, OnChangedRender(nameof(OnDistanceChange)), HideInInspector]
    public short Distance { get; set; }
    [Networked, OnChangedRender(nameof(OnWeatherChange)), HideInInspector]
    public short Weather { get; set; }
    public void OnSecondChange()
    {
        WorldManager.Instance.UpdateSecond(Second, Hour, Day);
    }
    public void OnHourChange()
    {
        WorldManager.Instance.UpdateHour(Hour, Day);
    }
    public void OnDistanceChange()
    {
        WorldManager.Instance.UpdateDistance(Distance);
    }
    public void OnWeatherChange()
    {
        EnvironmentManager.Instance.ChangeWeather((Weather)Weather);
        WorldManager.Instance.UpdateWeather((Weather)Weather);
    }
    #endregion

}
