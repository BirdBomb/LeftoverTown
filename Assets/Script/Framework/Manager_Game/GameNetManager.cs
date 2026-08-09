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
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_SpawnActor>().Subscribe(_ =>
        {
            ForState_AddActorSpawnQuest(_.name, _.pos, _.callBack);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_SpawnItem>().Subscribe(_ =>
        {
            SpawnItem(_.itemData, _.itemOwner, _.pos);
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
            RPC_LocalInput_ChangeBuildingInfo(_.pos, _.data);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_State_ChangeBuildingInfo>().Subscribe(_ =>
        {
            State_TrySendBuildingTileInfoData(_.pos, _.data);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_CreateBuildingArea>().Subscribe(_ =>
        {
            RPC_LocalInput_TryToCreateBuilding(_.buildingPos, _.buildingID, (int)_.areaSize);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_State_CreateBuildingArea>().Subscribe(_ =>
        {
            if (Object.HasStateAuthority)
            {
                State_TryToCreateBuilding(_.buildingPos, _.buildingID, _.areaSize);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_CreateGround>().Subscribe(_ =>
        {
            RPC_LocalInput_TryToCreateGround(_.groundPos, _.groundID);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_ChangeSunLight>().Subscribe(_ =>
        {
            RPC_LocalInput_ChangeSun(_.range);
        }).AddTo(this);
    }

    public override void Spawned()
    {
        OnHourChange();
        OnDayChange();
        OnSunLightChange();
        OnWeatherChange();
        base.Spawned();
    }
    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority) ForState_ProcessSpawnBatch(Runner.DeltaTime);
        base.FixedUpdateNetwork();
    }
    #region//创建角色
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="name"></param>
    /// <param name="postion"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPC_Local_SpawnActor(string name, Vector3 postion)
    {
        ForState_AddActorSpawnQuest(name, postion, null);
    }
    private Queue<ActorSpawnQuest> queue_ActorSpawnQuests = new Queue<ActorSpawnQuest>();
    private Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> prefabLoading = new Dictionary<string, bool>(); // 防止重复加载
    private const float FLOAT_SPAWN_INTERVAL = 0.2f;
    private float float_SpawnTimer;
    private int int_SpawnsPerBatch = 1; 
    private void ForState_ProcessSpawnBatch(float dt)
    {
        if (queue_ActorSpawnQuests.Count <= 0 || MapManager.Instance.queue_BuildingPending.Count > 0) return;

        if (float_SpawnTimer >= FLOAT_SPAWN_INTERVAL)
        {
            float_SpawnTimer = 0f;

            int processedCount = 0;
            while (processedCount < int_SpawnsPerBatch && queue_ActorSpawnQuests.Count > 0)
            {
                var quest = queue_ActorSpawnQuests.Dequeue();

                // 检查预制体是否已缓存
                if (prefabCache.ContainsKey(quest.path))
                {
                    // 已缓存，直接生成
                    ForState_SpawnFromCache(quest, prefabCache[quest.path]);
                    processedCount++;
                }
                else if (!prefabLoading.ContainsKey(quest.path))
                {
                    // 未缓存且未在加载中，开始异步加载
                    StartCoroutine(ForState_LoadAndCachePrefab(quest));
                    processedCount++;
                }
                else
                {
                    // 正在加载中，重新放回队列头部等待
                    var tempList = new List<ActorSpawnQuest> { quest };
                    tempList.AddRange(queue_ActorSpawnQuests);
                    queue_ActorSpawnQuests = new Queue<ActorSpawnQuest>(tempList);
                    break;
                }
            }
        }
        else
        {
            float_SpawnTimer += dt;
        }
    }
    private void ForState_AddActorSpawnQuest(string name, Vector3 postion, System.Action<ActorManager> action)
    {
        queue_ActorSpawnQuests.Enqueue(new ActorSpawnQuest(name, postion, action));
    }
    /// <summary>
    /// 异步加载并缓存预制体
    /// </summary>
    private IEnumerator ForState_LoadAndCachePrefab(ActorSpawnQuest quest)
    {
        string path = quest.path;

        // 标记正在加载
        prefabLoading[path] = true;

        // 异步加载
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        yield return request;

        GameObject prefab = request.asset as GameObject;
        if (prefab != null)
        {
            // 缓存预制体
            prefabCache[path] = prefab;
            //Debug.Log($"Cached prefab: {path}");
        }
        else
        {
            Debug.LogError($"Failed to load prefab: {path}");
        }

        // 移除加载标记
        prefabLoading.Remove(path);

        // 重新尝试生成该角色（如果需要）
        if (prefab != null && Object.HasStateAuthority)
        {
            ForState_SpawnFromCache(quest, prefab);
        }
    }
    private void ForState_SpawnFromCache(ActorSpawnQuest quest, GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab is null for: {quest.path}");
            return;
        }
        if (!Object.HasStateAuthority)
        {
            Debug.LogWarning("Lost state authority during spawn");
            return;
        }

        // 实例化并生成网络对象
        NetworkObject networkObject = Runner.Spawn(prefab, quest.pos, Quaternion.identity);
        if (networkObject == null)
        {
            Debug.LogError($"Failed to spawn actor: {quest.path}");
            return;
        }

        networkObject.AssignInputAuthority(Runner.LocalPlayer);
        quest.callBack?.Invoke(networkObject.GetComponent<ActorManager>());
    }
    private void ForState_SpawnFromCacheAsync(ActorSpawnQuest quest, GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab is null for: {quest.path}");
            return;
        }
        if (!Object.HasStateAuthority)
        {
            Debug.LogWarning("Lost state authority during spawn");
            return;
        }


        Fusion.NetworkSpawnOp networkSpawnOp = 
            Runner.SpawnAsync(prefab, quest.pos, Quaternion.identity, Runner.LocalPlayer,
            (runner, netobj) => { },
            (NetworkSpawnFlags)0,
            (result) => { quest.callBack?.Invoke(result.Object.GetComponent<ActorManager>()); });
    }
    #endregion
    #region//创建物体
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
        }
    }
    #endregion
    #region//地图保存和读取
    private MapInfoData bind_MapInfoData = null;
    private MapTileTypeData bind_BuildingTileTypeData = null;
    private MapTileInfoData bind_BuildingTileInfoData = null;
    private MapTileTypeData bind_GroundTileTypeData = null;
    private bool mapDataAlready = false;
    private int bind_MapSeed;

    private async Task LoadMap()
    {
        GameDataManager.Instance.LoadMap(out MapInfoData mapInfoData, out MapTileTypeData buildingTypeData, out MapTileInfoData buildingInfoData, out MapTileTypeData floorTypeData);
        bind_MapInfoData = mapInfoData;
        bind_BuildingTileTypeData = buildingTypeData;
        bind_BuildingTileInfoData = buildingInfoData;
        bind_GroundTileTypeData = floorTypeData;
        await Task.Delay(100);
        Debug.Log("服务器地图初始化成功");
        InitTimerLoop();
        InitMapSeed();
        InitSunLight();
    }
    private void SaveMap()
    {
        if (bind_BuildingTileTypeData != null && bind_BuildingTileInfoData != null && bind_GroundTileTypeData != null)
        {
            GameDataManager.Instance.SaveMap(bind_MapInfoData, bind_BuildingTileTypeData, bind_BuildingTileInfoData, bind_GroundTileTypeData);
            Debug.Log("服务器地图保存成功");
        }
    }
    private void InitMapSeed()
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
        bind_MapSeed = seedInt;
    }
    private void InitSunLight()
    {
        SunLight = bind_MapInfoData.distance;
    }
    #endregion
    #region//时间周期
    public readonly int int_SecondsPerHour = 120;
    public readonly int int_HourPerDay = 10;
    public void InitTimerLoop()
    {
        Debug.Log("开始计算世界时间");
        Hour = bind_MapInfoData.hour;
        Day = bind_MapInfoData.date;
        InvokeRepeating("AddOneSecond", 1, 1);
    }
    public void AddOneSecond()
    {
        if (Object.HasStateAuthority)
        {
            if (Second < int_SecondsPerHour)
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
            if (Hour < int_HourPerDay)
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
    /// <param name="blockArray">地块列表</param>
    /// <param name="size">地块尺寸</param>
    public void Local_RequestMapData(NetPos[] blockArray, short size)
    {
        RPC_LocalInput_RequestMapData(blockArray, size, Runner.LocalPlayer);
    }
    /// <summary>
    /// 本地端发送本地玩家位置
    /// </summary>
    /// <param name="pos">区域中心(只能是20的倍数)</param>
    /// <param name="size"></param>
    public void Local_SendPlayerPos(NetPos pos, short size)
    {
        RPC_LocalInput_ChangePlayerPos(pos, size, Runner.LocalPlayer);
    }
    #endregion
    #region//服务端发送
    private void State_TryToCreateGround(NetPos pos, short id)
    {
        RPC_StateCall_SendGroundTileType(pos, id);
        int tempIndex = CalculateTileIndex(pos.X, pos.Y);
        if (id > 0)
        {
            bind_GroundTileTypeData.tileDic[tempIndex] = id;
        }
        else
        {
            bind_GroundTileTypeData.tileDic[tempIndex] = 9999;
        }
    }
    private void State_TryToCreateBuilding(NetPos pos, short id, AreaSize size)
    {
        State_CreateBuilding(pos, id);
        State_FillArea(pos, 99, BuildingSizeHelper.GetOffsets(size));
    }
    private void State_CreateBuilding(NetPos pos, short id)
    {
        if (MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile))
        {
            AreaSize size = BuildingConfigData.GetBuildingConfig(buildingTile.tileID).Building_Size;
            State_FillArea(pos, 0, BuildingSizeHelper.GetOffsets(size));
        }
        int tempIndex = CalculateTileIndex(pos.X, pos.Y);
        RPC_StateCall_SendBuildingTileType(pos, id);
        if (id > 0)
        {
            bind_BuildingTileTypeData.tileDic[tempIndex] = id;
        }
        else
        {
            bind_BuildingTileTypeData.tileDic.Remove(tempIndex);
        }
    }
    private void State_FillArea(NetPos pos, short id, NetPos[] offsets)
    {
        // 跳过第一个位置（主位置），因为主位置会在外部删除
        for (int i = 1; i < offsets.Length; i++)
        {
            State_CreateBuilding(pos + (NetPos)offsets[i], id);
        }
    }

    /// <summary>
    /// 发送地图信息
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    private async Task State_TrySendMap(NetPos center, short size, PlayerRef player)
    {
        State_TrySendGroundTileTypeData(center, size, size, player);
        State_TrySendBuildingTileTypeData(center, size, size, player);
        await State_TrySendBuildingTileInfoData(center, size, size, player);
    }
    /// <summary>
    /// 发送地面类型数据(批量)
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="player"></param>
    private void State_TrySendGroundTileTypeData(NetPos center, short width, short height, PlayerRef player)
    {
        short[] tempTileArray = new short[width * height];
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int tempIndex = CalculateTileIndex(center.X + x, center.Y + y);
                if (bind_GroundTileTypeData.tileDic.TryGetValue(tempIndex,out short val))
                {
                    tempTileArray[index] = val;
                }
                else
                {
                    tempTileArray[index] = 9000;
                }
                index++;
            }
        }
        RPC_StateCall_SendGroundTileTypeData(player, tempTileArray, (Vector2Int)center, width, height);
    }
    /// <summary>
    /// 发送建筑类型数据(批量)
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="width">区域宽</param>
    /// <param name="height">区域高</param>
    /// <param name="player">目标客户端</param>
    private void State_TrySendBuildingTileTypeData(NetPos center, short width, short height, PlayerRef player)
    {
        short[] tempTileArray = new short[width * height];
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int tempIndex = CalculateTileIndex(center.X + x, center.Y + y);
                if (bind_BuildingTileTypeData.tileDic.TryGetValue(tempIndex, out short val))
                {
                    tempTileArray[index] = val;
                }
                else
                {
                    tempTileArray[index] = 0;
                }
                index++;
            }
        }
        RPC_StateCall_SendBuildingTileTypeData(player, tempTileArray, (Vector2Int)center, width, height);
    }
    /// <summary>
    /// 发送建筑信息数据(批量)
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="width">区域宽</param>
    /// <param name="height">区域高</param>
    /// <param name="player">目标客户端</param>
    private async Task State_TrySendBuildingTileInfoData(NetPos center, short width, short height, PlayerRef player)
    {
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int tempIndex = CalculateTileIndex(center.X + x, center.Y + y);
                if (bind_BuildingTileInfoData.mapData.TryGetValue(tempIndex, out byte[] data))
                {
                    RPC_StateCall_SendBuildingTileInfoData(player, data, new Vector2Int(center.X + x, center.Y + y));
                    index++;
                    if (index % 10 == 0) { await Task.Delay(1); }
                }
            }
        }
    }
    /// <summary>
    /// 发送建筑信息数据
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="info"></param>
    private void State_TrySendBuildingTileInfoData(NetPos pos, byte[] data)
    {
        RPC_StateCall_SendBuildingTileInfoData(Runner.LocalPlayer, data, (Vector2Int)pos);
        int tempIndex = CalculateTileIndex(pos.X, pos.Y);
        bind_BuildingTileInfoData.mapData[tempIndex] = data;
    }

    private int CalculateTileIndex(int x,int y)
    {
        int tempX = x + 30000;
        int tempY = y + 30000;
        return tempY > tempX
            ? tempY * tempY + tempY + tempY - tempX
            : tempX * tempX + tempY;
    }
    #endregion
    #region//客户端接收
    /// <summary>
    /// 本地端获得地图数据(基本信息)
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceivePlayerPos(PlayerRef player, NetPos center, int width, int height, int seed)
    {
        MapManager.Instance.UpdatePlayerPosInMapGrid(player, center, width, height, seed);
    }
    /// <summary>
    /// 本地端获得地图数据(地块类别)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveGroundTypeData(short[] tileList, NetPos center, short width, short height)
    {
        if (MapManager.Instance.AddGroundAreaInMap(center))
        {
            MapManager.Instance.AddPendingGround(tileList, center, width, height);
        }
    }
    /// <summary>
    /// 本地端获得地图数据(地块类别)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="center"></param>
    private void Local_ReceiveGroundTypeData(short id, NetPos center)
    {
        MapManager.Instance.AddPendingGround(id, center);
    }
    /// <summary>
    /// 本地端获得地图数据(建筑类别)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveBuildingTypeData(short[] tileList, NetPos center, short width, short height)
    {
        if (MapManager.Instance.AddBuildingAreaInMap(center))
        {
            MapManager.Instance.AddPendingBuildings(tileList, center, width, height);
        }
    }
    /// <summary>
    /// 本地端获得地图数据(建筑类别)
    /// </summary>
    private void Local_ReceiveBuildingTypeData(short id, NetPos center)
    {
        MapManager.Instance.AddPendingBuilding(id, center);
    }
    /// <summary>
    /// 本地端获得地图数据(建筑信息)
    /// </summary>
    private void Local_ReceiveBuildingInfoData(byte[] tileInfo, NetPos pos)
    {
        MapManager.Instance.AddPendingBuildingInfo((Vector3Int)pos, tileInfo);
    }
    /// <summary>
    /// 本地端获得地图数据(建筑生命值)
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="hp"></param>
    private void Local_ReceiveBuildingHpData(NetPos pos, int hp)
    {
        if (MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile))
        {
            buildingTile.tileObj.All_UpdateHP(hp);
        }
    }
    #endregion
    #region//RPC(服务器→客户端)
    /// <summary>
    /// 服务端给某人发送地图中心
    /// </summary>
    /// <param name="target"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_UpdatePlayerPos(/*[RpcTarget]*/PlayerRef target, NetPos center, int width, int height, int seed)
    {
        Local_ReceivePlayerPos(target, center, width, height, seed);
    }
    /// <summary>
    /// 服务端发送建筑类别数据(批量)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendBuildingTileTypeData(/*[RpcTarget]*/PlayerRef target, short[] tileTypeList, NetPos center, short width, short height)
    {
        Local_ReceiveBuildingTypeData(tileTypeList, (Vector3Int)center, width, height);
    }
    /// <summary>
    /// 服务端发送建筑类别数据
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendBuildingTileType(NetPos pos, short id)
    {
        Local_ReceiveBuildingTypeData(id, pos);
    }
    /// <summary>
    /// 服务端发送建筑信息数据
    /// </summary>
    /// <param name="target"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendBuildingTileInfoData(/*[RpcTarget]*/PlayerRef target, byte[] tileInfo, NetPos pos)
    {
        Local_ReceiveBuildingInfoData(tileInfo, pos);
    }
    /// <summary>
    /// 服务端发送建筑生命值数据
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="hp"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendBuildingTileHp(NetPos pos, int hp, PlayerRef player)
    {
        Local_ReceiveBuildingHpData(pos, hp);
    }
    /// <summary>
    /// 服务端发送地面类别数据(批量)
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendGroundTileTypeData(/*[RpcTarget]*/ PlayerRef target, short[] tileList, NetPos center, short width, short height)
    {
        Local_ReceiveGroundTypeData(tileList, (Vector3Int)center, width, height);
    }
    /// <summary>
    /// 服务端发送地面类别数据
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendGroundTileType(NetPos pos, short id)
    {
        Local_ReceiveGroundTypeData(id, pos);
    }
    #endregion
    #region//RPC(客户端→服务器)
    /// <summary>
    /// 客户端请求地图数据
    /// </summary>
    /// <param name="center"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private async void RPC_LocalInput_RequestMapData(NetPos[] centers, short size, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            if (!mapDataAlready)
            {
                mapDataAlready = true;
                await LoadMap();
            }
            for (int i = 0; i < centers.Length; i++)
            {
                await State_TrySendMap(centers[i], size, player);
            }
        }
    }
    /// <summary>
    /// 客户端修改地图中心
    /// </summary>
    /// <param name="center"></param>
    /// <param name="size"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_ChangePlayerPos(NetPos center, short size, PlayerRef player)
    {
        RPC_StateCall_UpdatePlayerPos(player, center, size, size, bind_MapSeed);
    }

    /// <summary>
    /// 客户端更改地块生命值
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="offset"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_ChangeBuildingHp(NetPos pos, int offset, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            if (MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile))
            {
                int newHp = buildingTile.tileObj.local_Hp + offset;
                RPC_StateCall_SendBuildingTileHp(pos, newHp, player);
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
    private void RPC_LocalInput_ChangeBuildingInfo(NetPos pos, byte[] data)
    {
        if (Object.HasStateAuthority)
        {
            State_TrySendBuildingTileInfoData(pos, data);
        }
    }
    /// <summary>
    /// 客户端输入对区域的改变
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_TryToCreateBuilding(NetPos pos, short id, int size)
    {
        if (Object.HasStateAuthority)
        {
            State_TryToCreateBuilding(pos, id, (AreaSize)size);
        }
    }
    /// <summary>
    /// 客户端改变地面
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_TryToCreateGround(NetPos pos, short id)
    {
        if (Object.HasStateAuthority)
        {
            State_TryToCreateGround(pos, id);
        }
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
    /// <summary>
    /// 客户端改变光照范围
    /// </summary>
    /// <param name="distance"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_ChangeSun(short distance)
    {
        bind_MapInfoData.distance = distance;
        SunLight = distance;
    }

    #endregion
    #region//Network
    [Networked, OnChangedRender(nameof(OnSecondChange)), HideInInspector]
    public short Second { get; set; }
    [Networked, OnChangedRender(nameof(OnHourChange)), HideInInspector]
    public short Hour { get; set; }
    [Networked, OnChangedRender(nameof(OnDayChange)), HideInInspector]
    public short Day { get; set; }
    [Networked, OnChangedRender(nameof(OnSunLightChange)), HideInInspector]
    public short SunLight { get; set; }
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
    public void OnDayChange()
    {
        
    }
    public void OnSunLightChange()
    {
        WorldLightManager.Instance.UpdateSunLight(SunLight);
    }
    public void OnWeatherChange()
    {
        EnvironmentManager.Instance.ChangeWeather((Weather)Weather);
        WorldManager.Instance.UpdateWeather((Weather)Weather);
    }
    #endregion

    #region //ReliableData
    #endregion
}
