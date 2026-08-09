using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Threading.Tasks;
using UniRx;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.UIElements;
/// <summary>
/// 地图管理器(只用于生成与绘制地图)
/// </summary>
public class MapManager : SingleTon<MapManager>,ISingleTon
{
    public bool updateMiniMap = false;
    private Dictionary<int, GroundTile> GroundTilePool = new Dictionary<int, GroundTile>();
    private Dictionary<int, BuildingTile> BuildingTilePool = new Dictionary<int, BuildingTile>();
    public static readonly NetPos[] EightDirections = new NetPos[]
    {
        new NetPos(0,0),
        new NetPos(0,1),
        new NetPos(0,-1),
        new NetPos(-1,0),
        new NetPos(1,0),
        new NetPos(1,1),
        new NetPos(-1,1),
        new NetPos(-1,-1),
        new NetPos(1,-1),
    };
    public static readonly NetPos[] FourDirections = new NetPos[]
    {
        new NetPos(0,0),
        new NetPos(0,1),
        new NetPos(0,-1),
        new NetPos(-1,0),
        new NetPos(1,0),
    };

    public int mapSeed;
    public void Init() 
    {
        StartCoroutine(ProcessGroundContinuously());
        StartCoroutine(ProcessBuildingContinuously());
    }
    #region//玩家中心
    /// <summary>
    /// 所有玩家的位置
    /// </summary>
    private Dictionary<PlayerRef, NetPos> dic_PlayerPos = new Dictionary<PlayerRef, NetPos>();
    /// <summary>
    /// 所有玩家
    /// </summary>
    private List<PlayerRef> list_PlayerRefs = new List<PlayerRef>();
    private NetPos netPos_LocalPlayerPos = new NetPos(short.MinValue, short.MinValue);
    /// <summary>
    /// 最大绘制距离平方
    /// </summary>
    private const float float_MaxLoadDistanceSqr = 200f * 200f;
    /// <summary>
    /// 检查本地玩家当前所在区块
    /// </summary>
    /// <param name="pos"></param>
    public void CheckPlayerPosInMapGrid(NetPos pos)
    {
        if (Mathf.Abs(pos.X - netPos_LocalPlayerPos.X) > float_AreaGridSize || Mathf.Abs(pos.Y - netPos_LocalPlayerPos.Y) > float_AreaGridSize)//超出范围，进入新区快
        {
            netPos_LocalPlayerPos = new NetPos((int)(Math.Round(pos.X / float_AreaGridSize) * float_AreaGridSize), (int)(Math.Round(pos.Y / float_AreaGridSize) * float_AreaGridSize));
            WorldManager.Instance.gameNetManager.Local_SendPlayerPos(netPos_LocalPlayerPos, (short)float_AreaGridSize);

            RequestAroundAreaInMap(netPos_LocalPlayerPos);
        }
    }
    /// <summary>
    /// 更新所有玩家当前所在区块
    /// </summary>
    public void UpdatePlayerPosInMapGrid(PlayerRef player, Vector3Int center, int width, int height, int seed)
    {
        if (dic_PlayerPos.ContainsKey(player)) 
        {
            dic_PlayerPos[player] = center;
        }
        else
        {
            Debug.Log("地图里添加一个玩家中心" + player + "pos" + center);
            dic_PlayerPos.Add(player, center);
            list_PlayerRefs.Add(player);
        }
        CullFarAreas(width, height);
        mapSeed = seed;
    }
    /// <summary>
    /// 剔除远距离区域
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void CullFarAreas(int width, int height)
    {
        List<Vector3Int> cullingList = new List<Vector3Int>();
        /*遍历所有已经绘制的区域并记录不靠近任何玩家的区域*/
        for (int i = 0; i < list_GroundArea.Count; i++)
        {
            bool isNearAnyPlayer = false;
            for (int j = 0; j < list_PlayerRefs.Count; j++)
            {
                if ((list_GroundArea[i] - dic_PlayerPos[list_PlayerRefs[j]]).sqrMagnitude <= float_MaxLoadDistanceSqr)
                {
                    /*地块有任意玩家使用*/
                    isNearAnyPlayer = true;
                    break;
                }
            }
            if (!isNearAnyPlayer)
            {
                Vector3Int temp = list_GroundArea[i];
                SubAreaInMap(temp, width, height);
                cullingList.Add(temp);
            }
        }
        /*遍历所有已经绘制的区域并剔除已经记录的区域*/
        for (int i = 0; i < cullingList.Count; i++)
        {
            list_GroundArea.Remove(cullingList[i]);
            hashSet_GroundArea.Remove(cullingList[i]);
            list_BuildingArea.Remove(cullingList[i]);
            hashSet_BuildingArea.Remove(cullingList[i]);

        }
    }
    #endregion
    #region//区域
    /// <summary>
    /// 地图网格大小
    /// </summary>
    private float float_AreaGridSize = 10;
    /// <summary>
    /// 已经加载的建筑区域
    /// </summary>
    private List<NetPos> list_BuildingArea = new List<NetPos>();
    private HashSet<NetPos> hashSet_BuildingArea = new HashSet<NetPos>();//自动去重
    /// <summary>
    /// 已经加载的地块区域
    /// </summary>
    private List<NetPos> list_GroundArea = new List<NetPos>();
    private HashSet<NetPos> hashSet_GroundArea = new HashSet<NetPos>();//自动去重

    /// <summary>
    /// 请求附近所有地图网格
    /// </summary>
    /// <param name="center">区域中心</param>
    /// <param name="blockSize">区块体积</param>
    /// <param name="blockCount">区块数量</param>
    /// <returns></returns>
    public void RequestAroundAreaInMap(NetPos center)
    {
        var newPositions = new List<NetPos>();
        var tempSet = new HashSet<NetPos>(); // 用于临时去重

        int step = (int)float_AreaGridSize;

        AddPositionIfNeeded(center, tempSet, newPositions);
        for (int radius = 0; radius < 4; radius ++)
        {
            // 只遍历当前环上的点，避免重复
            for (int i = -radius; i <= radius; i++)
            {
                /*上下*/
                if (radius > 0)
                {
                    AddPositionIfNeeded(center + new NetPos(i * step, radius * step), tempSet, newPositions);
                    AddPositionIfNeeded(center + new NetPos(i * step, -radius * step), tempSet, newPositions);
                }
                // 左边和右边（排除角落，避免重复）
                if (i > -radius && i < radius)
                {
                    AddPositionIfNeeded(center + new NetPos(radius * step, i * step), tempSet, newPositions);
                    AddPositionIfNeeded(center + new NetPos(-radius * step, i * step), tempSet, newPositions);
                }
            }
        }
        if (newPositions.Count > 0)
        {
            WorldManager.Instance.gameNetManager.Local_RequestMapData(newPositions.ToArray(), (short)float_AreaGridSize);
        }
    }
    private void AddPositionIfNeeded(NetPos pos, HashSet<NetPos> tempSet, List<NetPos> result)
    {
        // 使用 HashSet 的 O(1) 查找
        if (!hashSet_GroundArea.Contains(pos) && tempSet.Add(pos))
        {
            result.Add(pos);
        }
    }
    /// <summary>
    /// 去除一个绘制好的区域
    /// </summary>
    /// <param name="center"></param>
    /// <param name="widht"></param>
    /// <param name="height"></param>
    public void SubAreaInMap(NetPos center, int width, int height)
    {
        int halfWidth = width / 2;
        int halfHeight = height / 2;

        // 预计算所有需要移除的位置
        for (int x = -halfWidth; x < halfWidth; x++)
        {
            for (int y = -halfHeight; y < halfHeight; y++)
            {
                Vector3Int pos = new Vector3Int(center.X + x, center.Y + y, 0);

                // 先删除图形
                if (GetBuilding(pos, out BuildingTile buildingTile)) DeleteBuilding(pos, buildingTile);
                if (GetGround(pos,out GroundTile groundTile)) DeleteGround(pos, groundTile);
            }
        }
    }
    /// <summary>
    /// 尝试增加一个未绘制的建筑区域
    /// </summary>
    /// <param name="center"></param>
    /// <returns>可以绘制</returns>
    public bool AddBuildingAreaInMap(NetPos center)
    {
        if (hashSet_BuildingArea.Add(center)) // HashSet 自动去重
        {
            list_BuildingArea.Add(center);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 尝试增加一个未绘制的地板区域
    /// </summary>
    /// <param name="center"></param>
    /// <returns>可以绘制</returns>
    public bool AddGroundAreaInMap(NetPos center)
    {
        if (hashSet_GroundArea.Add(center))
        {
            list_GroundArea.Add(center);
            return true;
        }
        return false;
    }
    #endregion
    #region//地面
    [Header("地面map")]
    public Tilemap tilemap_Ground;
    [Header("地面Grid")]
    public Grid grid_Ground;
    private Queue<NetPos> queue_GroundPending = new Queue<NetPos>();
    private Dictionary<NetPos, short> dic_GroundPending = new Dictionary<NetPos, short>();
    private HashSet<NetPos> dirty_Ground = new HashSet<NetPos>();
    private Dictionary<NetPos, GroundTile> dic_GroundCache = new Dictionary<NetPos, GroundTile>();
    private Stack<GroundTile> stack_GroundPool = new Stack<GroundTile>();
    /// <summary>
    /// 每批处理数量
    /// </summary>
    private const int const_GroundBatchSize = 20;
    /// <summary>
    /// 每批处理时间
    /// </summary>
    private const float const_GroundBatchTime = 0.02f;
    /*-----添加待生成------*/
    public void AddPendingGround(short[] tileList, NetPos center, short width, short height)
    {
        int index = 0;
        NetPos pos;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                pos.X = (short)(center.X + x);
                pos.Y = (short)(center.Y + y);
                AddPendingGround(tileList[index], pos);
                index++;
            }
        }
    }
    public void AddPendingGround(short tileId, NetPos pos)
    {
        queue_GroundPending.Enqueue(pos);
        dic_GroundPending[pos] = tileId;
    }
    /*---------------------*/
    private IEnumerator ProcessGroundContinuously()
    {
        WaitForSeconds waitTime = new WaitForSeconds(const_GroundBatchTime); // 等待时间
        int processedCount; NetPos pos;
        while (true)
        {
            if (queue_GroundPending.Count > 0)
            {
                processedCount = 0;
                while (queue_GroundPending.Count > 0 && processedCount < const_GroundBatchSize * 2)
                {
                    pos = queue_GroundPending.Dequeue();
                    try { ProcessSingleGround(pos); }
                    catch (Exception ex) { CatchErrorGround(pos, ex); }
                    processedCount++;
                    // 每批处理就让出一帧，避免卡顿
                    if (processedCount % const_GroundBatchSize == 0)
                        yield return null;
                }
                
            }
            else
            {
                if (dirty_Ground.Count > 0) { DrawGround(); }
                yield return waitTime; // 每等待时间检查一次
            }
        }
    }
    private void ProcessSingleGround(NetPos pos)
    {
        if (dic_GroundPending.TryGetValue(pos, out short id))
        {
            CreateGround(id, pos);
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    dirty_Ground.Add(new NetPos(pos.X + x, pos.Y + y));
                }
            }
        }
    }
    private void CatchErrorGround(NetPos pos, Exception ex)
    {
        Debug.Log($"{pos.X}{pos.Y}{ex.Message}");
    }
    /// <summary>
    /// 生成地面
    /// </summary>
    public void CreateGround(int id, NetPos pos)
    {
        if (GetGround(pos, out GroundTile groundTile)) DeleteGround(pos, groundTile);
        groundTile = stack_GroundPool.Count > 0 ? stack_GroundPool.Pop() : ScriptableObject.CreateInstance<GroundTile>();
        groundTile.InitData(GetGroundConfig(id), pos, id);
        tilemap_Ground.SetTile(pos, groundTile);
        groundTile.BindObj(GetGroundObj(pos));

        dic_GroundCache[pos] = groundTile;

        if (updateMiniMap) GameUI_MiniMap.Instance.ChangeGroundOnTex(pos, id);
    }
    /// <summary>
    /// 删除地面
    /// </summary>
    /// <param name="tilePos"></param>
    public void DeleteGround(NetPos tilePos,GroundTile groundTile)
    {
        Destroy(tilemap_Ground.GetInstantiatedObject(tilePos));
        stack_GroundPool.Push(groundTile);

        tilemap_Ground.SetTile(tilePos, null);
        dic_GroundCache.Remove(tilePos);
    }
    /// <summary>
    /// 绘制地面
    /// </summary>
    public void DrawGround()
    {
        foreach (NetPos pos in dirty_Ground) 
        {
            if (GetGround(pos, out GroundTile groundTile))
            {
                groundTile.tileObj.Draw();
            }
        }
        dirty_Ground.Clear();
    }
    /// <summary>
    /// 获得地面
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public bool GetGround(NetPos tilePos, out GroundTile groundTile)
    {
        return dic_GroundCache.TryGetValue(tilePos, out groundTile);
    }
    public GameObject GetGroundObj(NetPos tilePos)
    {
        return tilemap_Ground.GetInstantiatedObject(tilePos);
    }
    private GroundTile GetGroundConfig(int id)
    {
        if (GroundTilePool.TryGetValue(id, out GroundTile groundTile))
        {
            return groundTile;
        }
        else
        {
            GroundTile config = Resources.Load<GroundTile>("TileScript/Ground/" + id);
            if (config != null)
            {
                GroundTilePool.Add(id, config);
            }
            return config;
        }
    }
    public Around CheckAround_Ground(NetPos pos, Func<int, bool> condition, DirectionType directionType)
    {
        Around around = new Around();
        var directions = directionType == DirectionType.Eight ? EightDirections : FourDirections;

        for (int i = 0; i < directions.Length; i++)
        {
            if (GetGround(pos + directions[i], out GroundTile ground) && condition(ground.tileID))
            {
                around.Set(i, true);
            }
        }
        return around;
    }
    #endregion
    #region//建筑
    [Header("建筑map")]
    public Tilemap tilemap_Building;
    [Header("建筑grid")]
    public Grid grid_Building;
    /// <summary>
    /// 待处理建筑坐标
    /// </summary>
    public Queue<NetPos> queue_BuildingPending = new Queue<NetPos>();
    private Dictionary<NetPos, short> dic_BuildingPending = new Dictionary<NetPos, short>();
    private Dictionary<NetPos, byte[]> dic_BuildingPendingInfos = new Dictionary<NetPos, byte[]>();
    private HashSet<NetPos> dirty_Building = new HashSet<NetPos>();
    private Dictionary<NetPos, BuildingTile> dic_BuildingCache = new Dictionary<NetPos, BuildingTile>();
    private Stack<BuildingTile> stack_BuildingPool = new Stack<BuildingTile>();
    /// <summary>
    /// 每批处理数量
    /// </summary>
    private const int const_BuildingBatchSize = 20;
    /// <summary>
    /// 每批处理时间
    /// </summary>
    private const float const_BuildingBatchTime = 0.02f;

    /*-----添加待生成------*/
    public void AddPendingBuildings(short[] tileList, NetPos center, short width, short height)
    {
        int index = 0;
        NetPos pos;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                pos.X = (short)(center.X + x);
                pos.Y = (short)(center.Y + y);
                AddPendingBuilding(tileList[index], pos);
                index++;
            }
        }
    }
    public void AddPendingBuilding(short tileId, NetPos pos)
    {
        queue_BuildingPending.Enqueue(pos);
        dic_BuildingPending[pos] = tileId;
    }
    public void AddPendingBuildingInfo(NetPos tilePos, byte[] tileInfo)
    {
        dic_BuildingPendingInfos[tilePos] = tileInfo;
        if (GetBuilding(tilePos, out BuildingTile buildingTile)) buildingTile.tileObj.All_ReceiveData(tileInfo);
    }
    /*---------------------*/
    private IEnumerator ProcessBuildingContinuously()
    {
        WaitForSeconds waitTime = new WaitForSeconds(const_BuildingBatchTime); // 等待时间
        int processedCount; NetPos pos;
        while (true)
        {
            if (queue_BuildingPending.Count > 0)
            {
                processedCount = 0;
                while (queue_BuildingPending.Count > 0 && processedCount < const_BuildingBatchSize * 2)
                {
                    pos = queue_BuildingPending.Dequeue();

                    try { ProcessSingleBuilding(pos); }
                    catch (Exception ex) { CatchErrorBuilding(pos, ex); }

                    processedCount++;

                    // 每批处理就让出一帧，避免卡顿
                    if (processedCount % const_BuildingBatchSize == 0)
                        yield return null;
                }
            }
            else
            {
                if (dirty_Building.Count > 0) { DrawBuilding(); }
                yield return waitTime; // 每等待时间检查一次
            }

        }
    }
    private void ProcessSingleBuilding(NetPos pos)
    {
        if (dic_BuildingPending.TryGetValue(pos, out short id))
        {
            InitBuilding(id, pos, false, out _);
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    dirty_Building.Add(new NetPos(pos.X + x, pos.Y + y));
                }
            }
        }
    }
    private void CatchErrorBuilding(NetPos pos, Exception ex)
    {
        Debug.Log($"{pos.X}{pos.Y}{ex.Message}");
    }
    /// <summary>
    /// 生成建筑
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    private void InitBuilding(int id, NetPos tilePos, bool isCreate, out BuildingTile buildingTile)
    {
        if (GetBuilding(tilePos, out buildingTile)) DeleteBuilding(tilePos, buildingTile);
        if (id <= 0)
        {
            buildingTile = null;
            return;
        }

        buildingTile = stack_BuildingPool.Count > 0 ? stack_BuildingPool.Pop() : ScriptableObject.CreateInstance<BuildingTile>();
        buildingTile.InitData(GetBuildingConfig(id), tilePos, id);
        tilemap_Building.SetTile(tilePos, buildingTile);
        var buildingObj = buildingTile.BindObj(GetBuildingObj(tilePos));

        dic_BuildingCache[tilePos] = buildingTile;

        UpdateGroundDrag(tilePos, buildingTile.config_Pass, buildingTile.config_Drag);

        if (isCreate) buildingObj.All_OnCreate();
        if (dic_BuildingPendingInfos.TryGetValue(tilePos, out byte[] data)) buildingObj.All_ReceiveData(data); 
    }
    /// <summary>
    /// 删除建筑
    /// </summary>
    /// <param name="tilePos"></param>
    private void DeleteBuilding(NetPos tilePos, BuildingTile buildingTile)
    {
        buildingTile.OnDelete();
        Destroy(tilemap_Building.GetInstantiatedObject(tilePos));
        DeleteBuildingPlaceHolding(tilePos, BuildingConfigData.GetBuildingConfig(buildingTile.tileID).Building_Size);
        dic_BuildingPendingInfos.Remove(tilePos);
        stack_BuildingPool.Push(buildingTile);
        UpdateGroundDrag(tilePos, true, 0);
        tilemap_Building.SetTile(tilePos, null);
        dic_BuildingCache.Remove(tilePos);
    }

    /// <summary>
    /// 更新地面阻力
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="pass"></param>
    /// <param name="drag"></param>
    private void UpdateGroundDrag(NetPos pos,bool pass,int drag)
    {
        if (GetGround(pos, out GroundTile groundTile))
        {
            groundTile.offset_Pass = pass && groundTile.config_Pass;
            groundTile.offset_Drag = drag + groundTile.config_Drag;  
        }
    }
    /// <summary>
    /// 删除建筑占位
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="size"></param>
    private void DeleteBuildingPlaceHolding(NetPos tilePos, AreaSize size)
    {
        var offsets = BuildingSizeHelper.GetOffsets(size);
        // 跳过第一个位置（主位置），因为主位置会在外部删除
        for (int i = 1; i < offsets.Length; i++)
        {
            if (GetBuilding(tilePos + offsets[i], out BuildingTile buildingTile)) DeleteBuilding(tilePos + offsets[i], buildingTile);
        }
    }
    /// <summary>
    /// 绘制建筑
    /// </summary>
    private void DrawBuilding()
    {
        foreach (NetPos pos in dirty_Building)
        {
            if (GetBuilding(pos, out BuildingTile buildingTile))
            {
                buildingTile.tileObj.All_OnDraw();
            }
        }
        dirty_Building.Clear();
    }
    /// <summary>
    /// 获得建筑
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public bool GetBuilding(NetPos tilePos, out BuildingTile buildingTile)
    {
        return dic_BuildingCache.TryGetValue(tilePos, out buildingTile);
    }
    /// <summary>
    /// 获得建筑实例
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public GameObject GetBuildingObj(NetPos tilePos)
    {
        return tilemap_Building.GetInstantiatedObject(tilePos);
    }
    /// <summary>
    /// 获得建筑配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BuildingTile GetBuildingConfig(int id)
    {
        if (BuildingTilePool.TryGetValue(id,out BuildingTile val))
        {
            return val;
        }
        else
        {
            BuildingTile config = Resources.Load<BuildingTile>("TileScript/Building/" + id);
            BuildingTilePool.Add(id, config);
            return config;
        }
    }

    public Around CheckAround_Building(NetPos pos, Func<int, bool> condition, DirectionType directionType)
    {
        Around around = new Around();
        var directions = directionType == DirectionType.Eight ? EightDirections : FourDirections;

        for (int i = 0; i < directions.Length; i++)
        {
            if (GetBuilding(pos + directions[i], out BuildingTile building) && condition(building.tileID))
            {
                around.Set(i, true);
            }
        }

        return around;
    }
    private const int ground_LimitID = 9000;
    /// <summary>
    /// 检查是否有建筑地基
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool CheckGround(NetPos pos, AreaSize size)
    {
        var offsets = BuildingSizeHelper.GetOffsets(size);

        foreach (var offset in offsets)
        {
            Vector3Int checkPos = pos + offset;
            if (!GetGround(checkPos, out GroundTile groundTile) ||groundTile.tileID >= ground_LimitID)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 检查是否有建筑空位
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool CheckBuildingEmpty(NetPos pos, AreaSize size)
    {
        var offsets = BuildingSizeHelper.GetOffsets(size);

        foreach (var offset in offsets)
        {
            if (GetBuilding(pos + offset, out _))
            {
                return false;
            }
        }
        return true;
    }
    List<BuildingTile> buildingTiles_Temp = new List<BuildingTile>();
    public List<BuildingTile> GetNearbyBuildings(NetPos position, NetPos offset,DirectionType directionType)
    {
        buildingTiles_Temp.Clear();
        NetPos pos = position + offset;

        var directions = (directionType == DirectionType.Four) ? FourDirections : EightDirections;

        foreach (var dir in directions)
        {
            if (GetBuilding(pos + dir, out BuildingTile buildingTile))
            {
                buildingTiles_Temp.Add(buildingTile);
            }
        }

        return buildingTiles_Temp;
    }
    #endregion
}
public struct Around
{
    public bool C;
    public bool U;
    public bool D;
    public bool L;
    public bool R;
    public bool UR;
    public bool UL;
    public bool DL;
    public bool DR;

    public void Set(int index, bool value)
    {
        switch (index)
        {
            case 0: C = value; break;
            case 1: U = value; break;
            case 2: D = value; break;
            case 3: L = value; break;
            case 4: R = value; break;
            case 5: UR = value; break;
            case 6: UL = value; break;
            case 7: DL = value; break;
            case 8: DR = value; break;
        }
    }
}
public enum DirectionType
{
    Four, Eight
}
public static class IndexCalculator
{
    private static readonly int[] PrecomputedIndices = new int[256];
    static IndexCalculator()
    {
        InitializeFromOriginalCode();
    }

    // 用于不修改Around类的情况
    public static int GetIndex(Around around)
    {
        // 计算位掩码
        byte flags = 0;
        if (around.U) flags |= 128;
        if (around.D) flags |= 64;
        if (around.L) flags |= 32;
        if (around.R) flags |= 16;
        if (around.UL) flags |= 8;
        if (around.UR) flags |= 4;
        if (around.DL) flags |= 2;
        if (around.DR) flags |= 1;
        return PrecomputedIndices[flags];
    }
    private static void InitializeFromOriginalCode()
    {
        Array.Fill(PrecomputedIndices, -1);

        // ============ U = true, D = true, L = true, R = true 的分支 ============
        #region//16
        // UL = true 的分支
        // UL=true, UR=true 的分支
        PrecomputedIndices[GetMask(true, true, true, true, true, true, true, true)] = 8;      // DL=true, DR=true
        PrecomputedIndices[GetMask(true, true, true, true, true, true, true, false)] = 11;    // DL=true, DR=false

        PrecomputedIndices[GetMask(true, true, true, true, true, true, false, true)] = 12;    // DL=false, DR=true
        PrecomputedIndices[GetMask(true, true, true, true, true, true, false, false)] = 37;   // DL=false, DR=false

        // UL=true, UR=false 的分支
        PrecomputedIndices[GetMask(true, true, true, true, true, false, true, true)] = 18;    // DL=true, DR=true
        PrecomputedIndices[GetMask(true, true, true, true, true, false, true, false)] = 44;   // DL=true, DR=false

        PrecomputedIndices[GetMask(true, true, true, true, true, false, false, true)] = 34;   // DL=false, DR=true
        PrecomputedIndices[GetMask(true, true, true, true, true, false, false, false)] = 46;  // DL=false, DR=false

        // UL = false 的分支
        // UL=false, UR=true 的分支
        PrecomputedIndices[GetMask(true, true, true, true, false, true, true, true)] = 19;    // DL=true, DR=true
        PrecomputedIndices[GetMask(true, true, true, true, false, true, true, false)] = 27;   // DL=true, DR=false

        PrecomputedIndices[GetMask(true, true, true, true, false, true, false, true)] = 38;   // DL=false, DR=true
        PrecomputedIndices[GetMask(true, true, true, true, false, true, false, false)] = 39;  // DL=false, DR=false

        // UL=false, UR=false 的分支
        PrecomputedIndices[GetMask(true, true, true, true, false, false, true, true)] = 45;   // DL=true, DR=true
        PrecomputedIndices[GetMask(true, true, true, true, false, false, true, false)] = 47;  // DL=true, DR=false

        PrecomputedIndices[GetMask(true, true, true, true, false, false, false, true)] = 40;  // DL=false, DR=true
        PrecomputedIndices[GetMask(true, true, true, true, false, false, false, false)] = 20; // DL=false, DR=false

        #endregion
        // ============ U = true, D = true, L = true, R = false 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(true, true, true, false, true, true, true, true)] = 9;      // 任何UR,DR值都映射到UL/DL决定的值
        PrecomputedIndices[GetMask(true, true, true, false, true, true, true, false)] = 9;
        PrecomputedIndices[GetMask(true, true, true, false, true, false, true, true)] = 9;
        PrecomputedIndices[GetMask(true, true, true, false, true, false, true, false)] = 9;

        PrecomputedIndices[GetMask(true, true, true, false, true, true, false, true)] = 24;      // 任何UR,DR值都映射到UL/DL决定的值
        PrecomputedIndices[GetMask(true, true, true, false, true, true, false, false)] = 24;
        PrecomputedIndices[GetMask(true, true, true, false, true, false, false, true)] = 24;
        PrecomputedIndices[GetMask(true, true, true, false, true, false, false, false)] = 24;


        PrecomputedIndices[GetMask(true, true, true, false, false, true, true, true)] = 29;
        PrecomputedIndices[GetMask(true, true, true, false, false, true, true, false)] = 29;
        PrecomputedIndices[GetMask(true, true, true, false, false, false, true, true)] = 29;
        PrecomputedIndices[GetMask(true, true, true, false, false, false, true, false)] = 29;

        PrecomputedIndices[GetMask(true, true, true, false, false, true, false, true)] = 33;
        PrecomputedIndices[GetMask(true, true, true, false, false, true, false, false)] = 33;
        PrecomputedIndices[GetMask(true, true, true, false, false, false, false, true)] = 33;
        PrecomputedIndices[GetMask(true, true, true, false, false, false, false, false)] = 33;
        #endregion
        // ============ U = true, D = true, L = false, R = true 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(true, true, false, true, true, true, true, true)] = 7;
        PrecomputedIndices[GetMask(true, true, false, true, true, true, false, true)] = 7;
        PrecomputedIndices[GetMask(true, true, false, true, false, true, true, true)] = 7;
        PrecomputedIndices[GetMask(true, true, false, true, false, true, false, true)] = 7;

        PrecomputedIndices[GetMask(true, true, false, true, true, true, true, false)] = 21;
        PrecomputedIndices[GetMask(true, true, false, true, true, true, false, false)] = 21;
        PrecomputedIndices[GetMask(true, true, false, true, false, true, true, false)] = 21;
        PrecomputedIndices[GetMask(true, true, false, true, false, true, false, false)] = 21;


        PrecomputedIndices[GetMask(true, true, false, true, true, false, true, true)] = 30;
        PrecomputedIndices[GetMask(true, true, false, true, true, false, false, true)] = 30;
        PrecomputedIndices[GetMask(true, true, false, true, false, false, true, true)] = 30;
        PrecomputedIndices[GetMask(true, true, false, true, false, false, false, true)] = 30;

        PrecomputedIndices[GetMask(true, true, false, true, true, false, true, false)] = 25;
        PrecomputedIndices[GetMask(true, true, false, true, true, false, false, false)] = 25;
        PrecomputedIndices[GetMask(true, true, false, true, false, false, true, false)] = 25;
        PrecomputedIndices[GetMask(true, true, false, true, false, false, false, false)] = 25;
        #endregion
        // ============ U = true, D = true, L = false, R = false 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(true, true, false, false, false, false, false, false)] = 10;
        // 其他UL,UR,DL,DR组合都映射到10
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        PrecomputedIndices[GetMask(true, true, false, false, ul == 1, ur == 1, dl == 1, dr == 1)] = 10;
                    }
        #endregion
        // ============ U = true, D = false, L = true, R = true 的分支 ============
        #region//16
        // UL=true, UR=true
        PrecomputedIndices[GetMask(true, false, true, true, true, true, false, false)] = 15;
        // UL=true, UR=false
        PrecomputedIndices[GetMask(true, false, true, true, true, false, false, false)] = 28;
        // UL=false, UR=true
        PrecomputedIndices[GetMask(true, false, true, true, false, true, false, false)] = 31;
        // UL=false, UR=false
        PrecomputedIndices[GetMask(true, false, true, true, false, false, false, false)] = 32;

        // 填充所有DL,DR组合（在D=false时忽略）
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        int val;
                        if (ul == 1 && ur == 1) val = 15;
                        else if (ul == 1 && ur == 0) val = 28;
                        else if (ul == 0 && ur == 1) val = 31;
                        else val = 32;
                        PrecomputedIndices[GetMask(true, false, true, true, ul == 1, ur == 1, dl == 1, dr == 1)] = val;
                    }
        #endregion
        // ============ U = true, D = false, L = true, R = false 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(true, false, true, false, true, false, false, false)] = 16;  // UL=true
        PrecomputedIndices[GetMask(true, false, true, false, false, false, false, false)] = 43; // UL=false

        // 填充其他组合
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        int val = (ul == 1) ? 16 : 43;
                        PrecomputedIndices[GetMask(true, false, true, false, ul == 1, ur == 1, dl == 1, dr == 1)] = val;
                    }
        #endregion
        // ============ U = true, D = false, L = false, R = true 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(true, false, false, true, false, true, false, false)] = 14;  // UR=true
        PrecomputedIndices[GetMask(true, false, false, true, false, false, false, false)] = 42; // UR=false

        // 填充其他组合
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        int val = (ur == 1) ? 14 : 42;
                        PrecomputedIndices[GetMask(true, false, false, true, ul == 1, ur == 1, dl == 1, dr == 1)] = val;
                    }
        #endregion
        // ============ U = true, D = false, L = false, R = false 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(true, false, false, false, false, false, false, false)] = 17;
        // 填充其他组合
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        PrecomputedIndices[GetMask(true, false, false, false, ul == 1, ur == 1, dl == 1, dr == 1)] = 17;
                    }
        #endregion
        // ============ U = false, D = true, L = true, R = true 的分支 ============
        #region//16
        // DL=true, DR=true
        PrecomputedIndices[GetMask(false, true, true, true, false, false, true, true)] = 1;
        // DL=true, DR=false
        PrecomputedIndices[GetMask(false, true, true, true, false, false, true, false)] = 23;
        // DL=false, DR=true
        PrecomputedIndices[GetMask(false, true, true, true, false, false, false, true)] = 22;
        // DL=false, DR=false
        PrecomputedIndices[GetMask(false, true, true, true, false, false, false, false)] = 26;

        // 填充其他UL,UR组合（在U=false时忽略）
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        int val;
                        if (dl == 1 && dr == 1) val = 1;
                        else if (dl == 1 && dr == 0) val = 23;
                        else if (dl == 0 && dr == 1) val = 22;
                        else val = 26;
                        PrecomputedIndices[GetMask(false, true, true, true, ul == 1, ur == 1, dl == 1, dr == 1)] = val;
                    }
        #endregion
        // ============ U = false, D = true, L = true, R = false 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(false, true, true, false, false, false, true, false)] = 2;   // DL=true
        PrecomputedIndices[GetMask(false, true, true, false, false, false, true, true)] = 2;   // DL=true
        PrecomputedIndices[GetMask(false, true, true, false, false, false, false, false)] = 36; // DL=false
        PrecomputedIndices[GetMask(false, true, true, false, false, false, false, true)] = 36; // DL=false

        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        int val = (dl == 1) ? 2 : 36;
                        PrecomputedIndices[GetMask(false, true, true, false, ul == 1, ur == 1, dl == 1, dr == 1)] = val;
                    }
        #endregion
        // ============ U = false, D =true, L = false, R = true 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(false, true, false, true, false, false, false, true)] = 0;   // DR=true
        PrecomputedIndices[GetMask(false, true, false, true, false, false, false, false)] = 35; // DR=false
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        int val = (dr == 1) ? 0 : 35;
                        PrecomputedIndices[GetMask(false, true, false, true, ul == 1, ur == 1, dl == 1, dr == 1)] = val;
                    }
        #endregion
        // ============ U = false, D = true, L = false, R = false 的分支 ============
        #region//16
        PrecomputedIndices[GetMask(false, true, false, false, false, false, false, false)] = 3;
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        PrecomputedIndices[GetMask(false, true, false, false, ul == 1, ur == 1, dl == 1, dr == 1)] = 3;
                    }

        #endregion
        // ============ U = false, D = false, L = true, R = true 的分支 ============
        #region//16
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        PrecomputedIndices[GetMask(false, false, true, true, ul == 1, ur == 1, dl == 1, dr == 1)] = 5;
                    }
        #endregion
        // ============ U = false, D = false, L = true, R = false 的分支 ============
        #region//16
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        PrecomputedIndices[GetMask(false, false, true, false, ul == 1, ur == 1, dl == 1, dr == 1)] = 6;
                    }
        #endregion
        // ============ U = false, D = false, L = false, R = true 的分支 ============
        #region//16
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        PrecomputedIndices[GetMask(false, false, false, true, ul == 1, ur == 1, dl == 1, dr == 1)] = 4;
                    }
        #endregion
        // ============ U = false, D = false, L = false, R = false 的分支 ============
        #region//16
        for (byte ul = 0; ul <= 1; ul++)
            for (byte ur = 0; ur <= 1; ur++)
                for (byte dl = 0; dl <= 1; dl++)
                    for (byte dr = 0; dr <= 1; dr++)
                    {
                        PrecomputedIndices[GetMask(false, false, false, false, ul == 1, ur == 1, dl == 1, dr == 1)] = 13;
                    }
        #endregion
    }
    private static byte GetMask(bool u, bool d, bool l, bool r, bool ul, bool ur, bool dl, bool dr)
    {
        byte mask = 0;
        if (u) mask |= 128;
        if (d) mask |= 64;
        if (l) mask |= 32;
        if (r) mask |= 16;
        if (ul) mask |= 8;
        if (ur) mask |= 4;
        if (dl) mask |= 2;
        if (dr) mask |= 1;
        return mask;
    }
}
public static class BuildingSizeHelper
{
    private static readonly Dictionary<AreaSize, NetPos[]> SizeOffsets = new Dictionary<AreaSize, NetPos[]>
    {
        [AreaSize._1X1] = new[] { new NetPos(0, 0) },
        [AreaSize._1X2] = new[] { new NetPos(0, 0), new NetPos(0, 1) },
        [AreaSize._2X1] = new[] { new NetPos(0, 0), new NetPos(1, 0) },
        [AreaSize._2X2] = new[] { new NetPos(0, 0), new NetPos(0, 1), new NetPos(1, 0), new NetPos(1, 1) },
        [AreaSize._3X3] = Generate3x3Offset()
    };
    private static NetPos[] Generate3x3Offset()
    {
        var offsets = new List<NetPos>() { new NetPos(0, 0) };
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                offsets.Add(new NetPos(x, y));
            }
        }
        return offsets.ToArray();
    } 
    public static NetPos[] GetOffsets(AreaSize size)
    {
        return SizeOffsets.TryGetValue(size, out var offsets) ? offsets : new[] { new NetPos(0, 0) };
    }
}
[System.Serializable]
public struct NetPos : INetworkStruct
{
    public short X;
    public short Y;

    public NetPos(short x, short y)
    {
        X = x;
        Y = y;
    }
    public NetPos(int x, int y)
    {
        X = (short)x;
        Y = (short)y;
    }
    // 从 Vector2Int 隐式转换
    public static implicit operator NetPos(Vector2Int vec)
        => new NetPos((short)vec.x, (short)vec.y);

    // 转换为 Vector2Int
    public static implicit operator Vector2Int(NetPos coord)
        => new Vector2Int(coord.X, coord.Y);

    // 转换为 Vector3Int（z=0）
    public static implicit operator Vector3Int(NetPos coord)
        => new Vector3Int(coord.X, coord.Y, 0);

    // 从 Vector3Int 转换（忽略 z）
    public static implicit operator NetPos(Vector3Int vec)
        => new NetPos((short)vec.x, (short)vec.y);

    // 加法运算
    public static NetPos operator +(NetPos a, NetPos b)
        => new NetPos((short)(a.X + b.X), (short)(a.Y + b.Y));

    // 减法运算
    public static NetPos operator -(NetPos a, NetPos b)
        => new NetPos((short)(a.X - b.X), (short)(a.Y - b.Y));

    // 乘法运算（用于 step * radius）
    public static NetPos operator *(NetPos a, int multiplier)
        => new NetPos((short)(a.X * multiplier), (short)(a.Y * multiplier));
    // 平方长度
    public int sqrMagnitude => X * X + Y * Y;
    public override string ToString() => $"({X}, {Y})";

    public override bool Equals(object obj)
    {
        if (obj is NetPos other)
            return X == other.X && Y == other.Y;
        return false;
    }

    public override int GetHashCode() => (X << 16) ^ Y;
}