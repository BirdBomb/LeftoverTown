using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using UniRx;

public class NavManager : SingleTon<NavManager>, ISingleTon
{

    // 【修改1】使用 Dictionary 快速查找节点在池中的索引，解决 O(N^2) 性能问题
    private Dictionary<long, int> nodeIndexMap;
    // 【修改2】OpenList 只存 int 索引，避免 Struct 复制导致的数据不同步
    private int[] openList;
    private int openCount;

    private PathNode[] nodePool;
    private int nodePoolSize;
    private const int INVALID_INDEX = -1; // 添加无效索引常量
    // 邻居偏移缓存
    private static readonly Vector3Int[] neighborOffsets = new Vector3Int[]
    {
        new Vector3Int(0, 1, 0),   // up
        new Vector3Int(0, -1, 0),  // down
        new Vector3Int(1, 0, 0),   // right
        new Vector3Int(-1, 0, 0),  // left
        new Vector3Int(1, 1, 0),   // rightUp
        new Vector3Int(-1, 1, 0),  // leftUp
        new Vector3Int(1, -1, 0),  // rightDown
        new Vector3Int(-1, -1, 0)  // leftDown
    };
    // 对象池
    private Stack<List<Vector3Int>> listPool;

    public void Init()
    {
        // 初始化关闭列表
        nodeIndexMap = new Dictionary<long, int>(1024); // 初始化容量

        openList = new int[1024]; // 存索引
        openCount = 0;

        // 初始化节点池
        nodePool = new PathNode[10000];
        nodePoolSize = 0;
        for (int i = 0; i < nodePool.Length; i++)
        {
            nodePool[i] = new PathNode(i); // 传入索引
        }

        // 初始化列表池
        listPool = new Stack<List<Vector3Int>>();
        for (int i = 0; i < 10; i++)
        {
            listPool.Push(new List<Vector3Int>());
        }
    }

    /// <summary>
    /// 优化的A*寻路
    /// </summary>
    public List<Vector3Int> FindPath(Vector3Int toPos, Vector3Int fromPos, int maxStep)
    {
        // 快速检查
        if (!QuickValidate(toPos, fromPos, maxStep))
        {
            return new List<Vector3Int>();
        }

        // 重置节点池索引
        nodePoolSize = 0;
        nodeIndexMap.Clear();
        openCount = 0;

        // 起点处理
        ref PathNode startNode = ref GetNode(fromPos, out int startIndex);
        startNode.G = 0;
        startNode.H = CalcHeuristic(fromPos, toPos);
        startNode.ParentIndex = INVALID_INDEX;
        startNode.IsValid = true;

        // 添加到开放列表
        PushToOpen(startIndex);

        int step = 0;
        while (openCount > 0 && step < maxStep)
        {
            step++;

            // 取出最小F值的节点
            int currentIndex = PopFromOpen();
            ref PathNode current = ref nodePool[currentIndex];

            // 检查是否到达终点
            if (current.Pos == toPos)
            {
                var finalPath = new List<Vector3Int>();
                ReconstructPath(currentIndex, maxStep, finalPath);
                return finalPath;
            }
            // 遍历邻居
            for (int i = 0; i < 8; i++)
            {
                Vector3Int neighborPos = current.Pos + neighborOffsets[i];

                // 快速检查邻居是否可行
                if (!IsNeighborValid(current.Pos, neighborPos, i)) continue;

                // 获取或创建节点
                ref PathNode neighbor = ref GetNode(neighborPos, out int neighborNodeIndex);
                // 检查是否closed中
                if (neighbor.InClose) continue;
                // 计算新的G值
                float moveCost = GetMoveCost(current.Pos, neighborPos, i);
                float newG = current.G + moveCost;


                // 核心 A* 逻辑
                // 如果找到更好的路径
                if (newG < neighbor.G || !neighbor.IsValid)
                {
                    bool isBetter = newG < neighbor.G;
                    neighbor.G = newG;
                    neighbor.H = CalcHeuristic(neighborPos, toPos);
                    neighbor.ParentIndex = currentIndex;
                    neighbor.IsValid = true;

                    // 添加到开放列表（如果不在列表中）
                    if (!neighbor.InOpen)
                    {
                        neighbor.InOpen = true;
                        PushToOpen(neighborNodeIndex);
                    }
                    else if (isBetter)
                    {
                        //如果已经在 OpenList 中且 G 值变小了，需要重新排序（上浮）
                        BubbleUp(neighbor.OpenIndex);
                    }
                }
            }
        }
        return GetEmptyPath();
    }
    private bool QuickValidate(Vector3Int toPos, Vector3Int fromPos, int maxStep)
    {
        if (!MapManager.Instance.GetGround(toPos, out GroundTile to) || !to.offset_Pass)
            return false;

        if (!MapManager.Instance.GetGround(fromPos, out GroundTile from) /*|| !from.offset_Pass*/)
            return false;

        if (toPos == fromPos)
            return false;

        // 曼哈顿距离快速检查
        int manhattanDist = Mathf.Abs(toPos.x - fromPos.x) + Mathf.Abs(toPos.y - fromPos.y);
        if (manhattanDist > maxStep)
            return false;

        return true;
    }
    private bool IsNeighborValid(Vector3Int current, Vector3Int neighbor, int direction)
    {
        // 获取邻居格子
        if (!MapManager.Instance.GetGround(neighbor, out GroundTile tile) || !tile.offset_Pass)
            return false;

        // 对角线移动需要检查相邻格子
        if (direction >= 4) // 对角线方向
        {
            Vector3Int horizontal = new Vector3Int(neighborOffsets[direction].x, 0, 0);
            Vector3Int vertical = new Vector3Int(0, neighborOffsets[direction].y, 0);

            if (!MapManager.Instance.GetGround(current + horizontal, out GroundTile hTile) || !hTile.offset_Pass)
                return false;
            if (!MapManager.Instance.GetGround(current + vertical, out GroundTile vTile) || !vTile.offset_Pass)
                return false;
        }

        return true;
    }
    private float GetMoveCost(Vector3Int from, Vector3Int to, int direction)
    {
        // 基础移动成本
        float baseCost = direction >= 4 ? 1.414f : 1f; // 对角线≈√2

        // 获取地形拖拽成本
        if (MapManager.Instance.GetGround(from, out GroundTile tile))
        {
            baseCost += tile.offset_Drag;
        }

        return baseCost;
    }
    private float CalcHeuristic(Vector3Int from, Vector3Int to)
    {
        // 使用对角线距离（更精确的启发式函数）
        int dx = Mathf.Abs(from.x - to.x);
        int dy = Mathf.Abs(from.y - to.y);

        // 对角线距离 = 直线距离 + (√2 - 1) * 对角线步数
        return Mathf.Max(dx, dy) + 0.414f * Mathf.Min(dx, dy);
    }

    #region 节点池管理
    private ref PathNode GetNode(Vector3Int pos, out int index)
    {
        long key = GetPositionKey(pos);
        // 1. 尝试从字典快速查找
        if (nodeIndexMap.TryGetValue(key, out int existingIndex))
        {
            index = existingIndex;
            return ref nodePool[existingIndex];
        }
        // 2. 新建节点
        if (nodePoolSize < nodePool.Length)
        {
            index = nodePoolSize;
            ref PathNode node = ref nodePool[nodePoolSize];
            node.Reset();
            node.Pos = pos;
            node.Index = index;
            node.IsValid = true;

            // 记录到字典
            nodeIndexMap.Add(key, index);

            nodePoolSize++;
            return ref node;
        }
        Debug.LogError("Node Pool Overflow!");
        index = 0;
        return ref nodePool[0];
    }
    #endregion

    #region 开放列表（最小堆实现）
    private void PushToOpen(int nodeIndex)
    {
        if (openCount >= openList.Length) Array.Resize(ref openList, openList.Length * 2);

        int heapIndex = openCount;
        openList[heapIndex] = nodeIndex;
        nodePool[nodeIndex].OpenIndex = heapIndex; // 记录自己在堆中的位置
        openCount++;

        BubbleUp(heapIndex);
    }
    private int PopFromOpen()
    {
        if (openCount == 0) return INVALID_INDEX;

        int resultNodeIndex = openList[0];//获取openList里成本最低的node
        nodePool[resultNodeIndex].InOpen = false;
        nodePool[resultNodeIndex].InClose = true;
        nodePool[resultNodeIndex].OpenIndex = INVALID_INDEX;

        openCount--;

        if (openCount > 0)
        {
            int lastNodeIndex = openList[openCount];
            openList[0] = lastNodeIndex;
            nodePool[lastNodeIndex].OpenIndex = 0;
            BubbleDown(0);
        }

        return resultNodeIndex;
    }
    // 上浮操作
    private void BubbleUp(int heapIndex)
    {
        while (heapIndex > 0)
        {
            int parentHeapIndex = (heapIndex - 1) / 2;

            // 通过 nodePool 获取最新的 F 值进行比较
            int currentNodeIndex = openList[heapIndex];
            int parentNodeIndex = openList[parentHeapIndex];

            if (nodePool[parentNodeIndex].F <= nodePool[currentNodeIndex].F)
                break;

            // 交换 OpenList 中的内容
            SwapHeap(heapIndex, parentHeapIndex);

            heapIndex = parentHeapIndex;
        }
    }
    // 下沉操作
    private void BubbleDown(int heapIndex)
    {
        while (true)
        {
            int left = heapIndex * 2 + 1;
            int right = heapIndex * 2 + 2;
            int smallest = heapIndex;

            if (left < openCount &&
                nodePool[openList[left]].F < nodePool[openList[smallest]].F)
                smallest = left;

            if (right < openCount &&
                nodePool[openList[right]].F < nodePool[openList[smallest]].F)
                smallest = right;

            if (smallest == heapIndex) break;

            SwapHeap(heapIndex, smallest);
            heapIndex = smallest;
        }
    }
    private void SwapHeap(int idxA, int idxB)
    {
        int nodeIdxA = openList[idxA];
        int nodeIdxB = openList[idxB];

        // 交换数组
        openList[idxA] = nodeIdxB;
        openList[idxB] = nodeIdxA;

        // 更新节点中记录的堆索引
        nodePool[nodeIdxA].OpenIndex = idxB;
        nodePool[nodeIdxB].OpenIndex = idxA;
    }
    #endregion

    #region 路径重建和缓存

    private void ReconstructPath(int endNodeIndex, int maxStep, List<Vector3Int> result)
    {
        int currentIndex = endNodeIndex;
        while (currentIndex != INVALID_INDEX && maxStep > 0)
        {
            maxStep--;
            ref PathNode current = ref nodePool[currentIndex];
            result.Add(current.Pos);
            currentIndex = current.ParentIndex;
        }
        result.Reverse();
    }
    private List<Vector3Int> GetEmptyPath()
    {
        if (listPool.Count > 0)
        {
            var list = listPool.Pop();
            list.Clear();
            return list;
        }
        return new List<Vector3Int>();
    }

    public void ReturnListToPool(List<Vector3Int> list)
    {
        if (listPool.Count < 20)
        {
            list.Clear();
            listPool.Push(list);
        }
    }

    #endregion

    // 使用long类型编码位置，比Dictionary<Vector3Int>更高效
    private long GetPositionKey(Vector3Int pos)
    {
        return ((long)pos.x << 32) | (uint)pos.y;
    }
}
/// <summary>
/// 路径节点（结构体，使用索引引用）
/// </summary>
public struct PathNode
{
    public Vector3Int Pos;
    /// <summary>
    /// 到起点成本
    /// </summary>
    public float G;
    /// <summary>
    /// 到终点成本
    /// </summary>
    public float H;
    /// <summary>
    /// 总计成本
    /// </summary>
    public float F => G + H;
    public int ParentIndex;  // 父节点索引，-1表示无父节点
    public int Index;         // 自身在节点池中的索引
    public int OpenIndex;     // 在开放列表中的索引，-1表示不在开放列表
    public bool InOpen;       // 是否在开放列表中
    public bool InClose;
    public bool IsValid;      // 节点是否有效

    public PathNode(int index)
    {
        Pos = Vector3Int.zero;
        G = float.MaxValue;
        H = 0;
        ParentIndex = -1;
        Index = index;
        OpenIndex = -1;
        InOpen = false;
        InClose = false;
        IsValid = false;
    }

    public void Reset()
    {
        G = float.MaxValue;
        H = 0;
        ParentIndex = -1;
        OpenIndex = -1;
        InOpen = false;
        InClose = false;
        IsValid = false;
    }
}