using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MapCalculate { }
public partial class MapCreate
{
    #region//通用
    /// <summary>
    /// 获得随机偏移
    /// </summary>
    /// <returns></returns>
    public Vector2 GetRandomOffset()
    {
        UnityEngine.Random.InitState(seed_Offset + GetAccIndex());
        float x = UnityEngine.Random.Range(0f, 0.5f);
        UnityEngine.Random.InitState(seed_Offset + GetAccIndex());
        float y = UnityEngine.Random.Range(0f, 0.5f);
        return new Vector2(x, y);
    }
    public int GetAccIndex()
    {
        seed_Acc++;
        return seed_Acc;
    }
    public Vector2 GetRandomOffset(int offset)
    {
        UnityEngine.Random.InitState(seed_Offset + offset * offset);
        float x = UnityEngine.Random.Range(0f, 0.5f);
        UnityEngine.Random.InitState(seed_Offset - offset * offset);
        float y = UnityEngine.Random.Range(0f, 0.5f);
        return new Vector2(x, y);
    }
    public float GetRandomRange(int offset)
    {
        UnityEngine.Random.InitState(seed_Offset + offset * offset);
        return UnityEngine.Random.Range(0, 1);
    }
    /// <summary>
    /// 计算最优的批次大小
    /// </summary>
    /// <param name="totalPoints">总数</param>
    /// <returns></returns>
    private int CalculateOptimalBatchSize(int totalPoints)
    {
        // 根据总点数和CPU核心数动态调整
        int processorCount = SystemInfo.processorCount;

        // 确保每个CPU核心有足够的工作量
        int minBatchSize = Mathf.Max(1000, totalPoints / (processorCount * 10));
        int maxBatchSize = 10000; // 防止批次太大导致卡顿

        // 确保批次大小是合理的
        return Mathf.Clamp(totalPoints / 100, minBatchSize, maxBatchSize);
    }
    /// <summary>
    /// V2转Index
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int Vector2ToIndex(int x, int y)
    {
        int tempX = x + 30000;
        int tempY = y + 30000;
        int tempIndex;
        if (tempY > tempX)
        {
            tempIndex = tempY * tempY + tempY + tempY - tempX;
        }
        else
        {
            tempIndex = tempX * tempX + tempY;
        }
        return tempIndex;
    }

    #endregion
    #region 柏林噪声区域生成(中心聚集)
    public struct PerlinSolidConfig
    {
        /// <summary>
        /// 整体半径
        /// </summary>
        public float area_OuterRadius;
        /// <summary>
        /// 填充半径
        /// </summary>
        public float area_FillRadius;
        /// <summary>
        /// 中心
        /// </summary>
        public Vector2 area_Center;
        /// <summary>
        /// 噪声尺寸 1-20(边缘毛躁半径)
        /// </summary>
        public float noise_Scale;
        /// <summary>
        /// 噪声偏移(0,1)
        /// </summary>
        public Vector2 noise_Offset;
    }
    /// <summary>
    /// 填充一个区域(中心聚集)
    /// </summary>
    /// <param name="config"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task GenerateSolidArea(PerlinSolidConfig config, Action<int, float, float> action)
    {
        float outerRadiusSqr = config.area_OuterRadius * config.area_OuterRadius;
        float fillRadiusSqr = config.area_FillRadius * config.area_FillRadius;
        float transitionZone = config.area_OuterRadius - config.area_FillRadius;
        if (transitionZone <= 0)
        {
            Debug.LogError("outerRadius 不能大于或等于 fillRadius");
            return;
        }
        int fromX = (int)(config.area_Center.x - config.area_OuterRadius);
        int toX = (int)(config.area_Center.x + config.area_OuterRadius);
        int fromY = (int)(config.area_Center.y - config.area_OuterRadius);
        int toY = (int)(config.area_Center.y + config.area_OuterRadius);

        int width = toX - fromX;
        int height = toY - fromY;
        int totalPoints = width * height;

        /*距离缓存*/
        float[] distanceCache = new float[totalPoints];
        /*噪声缓存*/
        float[] noiseCache = new float[totalPoints];
        /*坐标缓存*/
        int[] xCoordinates = new int[totalPoints];
        /*坐标缓存*/
        int[] yCoordinates = new int[totalPoints];

        float noiseStep = 1f / config.noise_Scale;
        Vector2 center = config.area_Center;

        System.Random rand = new System.Random(config.noise_Offset.GetHashCode());
        float randomOffsetX = (float)rand.NextDouble() * 1000f;
        float randomOffsetY = (float)rand.NextDouble() * 1000f;

        await Task.Run(() =>
        {
            // 并行计算所有点的距离和噪声
            Parallel.For(0, totalPoints, index =>
            {
                // 计算坐标
                int localX = index % width;
                int localY = index / width;
                int worldX = fromX + localX;
                int worldY = fromY + localY;

                // 计算距离平方
                float dx = worldX - config.area_Center.x;
                float dy = worldY - config.area_Center.y;
                float distanceSqr = dx * dx + dy * dy;

                // 关键修复：打破对称性的噪声计算
                float noiseX = (worldX + config.noise_Offset.x + randomOffsetX) * noiseStep;
                float noiseY = (worldY + config.noise_Offset.y + randomOffsetY) * noiseStep;

                // 添加微小扰动，彻底打破对称性
                float epsilon = 0.001f;
                noiseX += epsilon * (worldX % 7); // 使用质数避免模式重复
                noiseY += epsilon * (worldY % 11);
                distanceCache[index] = distanceSqr;
                xCoordinates[index] = worldX;
                yCoordinates[index] = worldY;

                noiseCache[index] = Mathf.PerlinNoise(noiseX, noiseY);
            });
        });

        int batchSize = CalculateOptimalBatchSize(totalPoints);
        int totalBatches = Mathf.CeilToInt((float)totalPoints / batchSize);

        // 处理每个批次
        for (int batch = 0; batch < totalBatches; batch++)
        {
            int startIndex = batch * batchSize;
            int endIndex = Mathf.Min(startIndex + batchSize, totalPoints);

            // 异步处理当前批次
            await Task.Run(() =>
            {
                GenerateSolidArea_ProcessBatch(startIndex, endIndex, xCoordinates, yCoordinates, distanceCache, noiseCache,
                           fillRadiusSqr, outerRadiusSqr, transitionZone, config, action);
            });

            // 更新进度
            if (totalPoints > 1000) slider_Waiting.value = (float)batch / totalBatches;

            // 每处理几个批次让出控制权
            if (batch % 10 == 5)
                await Task.Yield();
        }
    }
    /// <summary>
    /// 处理一个批次的数据(中心聚集)
    /// </summary>
    private void GenerateSolidArea_ProcessBatch(
        int startIndex, int endIndex,
        int[] xCoords, int[] yCoords, float[] distanceCache, float[] noiseCache,
        float fillRadiusSqr, float outerRadiusSqr, float transitionZone,
        PerlinSolidConfig config, Action<int, float, float> action)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            float distanceSqr = distanceCache[i];
            float perlinNoise = noiseCache[i];
            float realNoise;

            // 根据位置分类处理
            if (distanceSqr <= fillRadiusSqr)
            {
                // 核心区域：完全填充
                realNoise = 1f;
            }
            else if (distanceSqr >= outerRadiusSqr)
            {
                // 外部区域：完全为空
                realNoise = 0f;
            }
            else
            {
                // 边缘过渡区域
                float distance = Mathf.Sqrt(distanceSqr);

                // 计算距离衰减因子 (0-1)
                // 从fill到outer，t从1降到0
                float t = (config.area_OuterRadius - distance) / transitionZone;

                // 确保在合法范围内
                t = Mathf.Clamp01(t);

                // 改进的混合函数：使用平滑过渡
                float blendFactor = GenerateArea_CalculateBlendFactor(t);

                // 混合噪声和距离衰减
                realNoise = Mathf.Lerp(perlinNoise, t, blendFactor);

                // 可选：添加额外控制
                //realNoise = ApplyPostProcessing(realNoise, t);
            }
            // 返回结果
            action.Invoke(Vector2ToIndex(xCoords[i], yCoords[i]), perlinNoise, realNoise);
        }
    }
    /// <summary>
    /// 计算混合因子（核心优化）
    /// </summary>
    private float GenerateArea_CalculateBlendFactor(float t)
    {
        // 原始公式：Mathf.Abs(t - 0.5f) * 2
        // 问题：在中间区域(t=0.5)时混合因子为0，噪声完全不起作用

        // 改进方案1：使用平滑曲线
        // return Mathf.SmoothStep(0, 1, Mathf.Abs(t - 0.5f) * 2);

        // 改进方案2：更灵活的控制
        float normalized = Mathf.Abs(t - 0.5f) * 2;

        // 使用三次平滑函数
        // 在边缘(t接近0或1)时混合因子接近1
        // 在中间(t接近0.5)时混合因子较小但不是0
        float blend = normalized * normalized * (3f - 2f * normalized);

        // 确保中间区域仍有一定噪声影响
        blend = Mathf.Max(blend, 0.2f);

        return blend;
    }
    #endregion
    #region 柏林噪声区域生成(均匀分布)
    public struct PerlinEvenlyConfig
    {
        /// <summary>
        /// 整体半径
        /// </summary>
        public float area_OuterRadius;
        /// <summary>
        /// 填充半径
        /// </summary>
        public float area_FillRadius;
        /// <summary>
        /// 中心
        /// </summary>
        public Vector2 area_Center;
        /// <summary>
        /// 噪声尺寸 1-20(边缘毛躁半径)
        /// </summary>
        public float noise_Scale;
        /// <summary>
        /// 噪声偏移(0,1)
        /// </summary>
        public Vector2 noise_Offset;
    }
    /// <summary>
    /// 填充一个区域(均匀填充)
    /// </summary>
    /// <param name="config"></param>
    /// <param name="action"></param>
    /// <returns></returns> 
    public async Task GenerateEvenlyArea(PerlinEvenlyConfig config, Action<int, float, float> action)
    {
        float outerRadiusSqr = config.area_OuterRadius * config.area_OuterRadius;
        float fillRadiusSqr = config.area_FillRadius * config.area_FillRadius;
        float transitionZone = config.area_OuterRadius - config.area_FillRadius;
        if (transitionZone <= 0)
        {
            Debug.LogError("outerRadius 不能大于或等于 fillRadius");
            return;
        }
        int fromX = (int)(config.area_Center.x - config.area_OuterRadius);
        int toX = (int)(config.area_Center.x + config.area_OuterRadius);
        int fromY = (int)(config.area_Center.y - config.area_OuterRadius);
        int toY = (int)(config.area_Center.y + config.area_OuterRadius);

        int width = toX - fromX;
        int height = toY - fromY;
        int totalPoints = width * height;

        /*距离缓存*/
        float[] distanceCache = new float[totalPoints];
        /*噪声缓存*/
        float[] noiseCache = new float[totalPoints];
        /*坐标缓存*/
        int[] xCoordinates = new int[totalPoints];
        /*坐标缓存*/
        int[] yCoordinates = new int[totalPoints];

        float noiseStep = 1f / config.noise_Scale;
        Vector2 center = config.area_Center;

        System.Random rand = new System.Random(config.noise_Offset.GetHashCode());
        float randomOffsetX = (float)rand.NextDouble() * 1000f;
        float randomOffsetY = (float)rand.NextDouble() * 1000f;

        await Task.Run(() =>
        {
            // 并行计算所有点的距离和噪声
            Parallel.For(0, totalPoints, index =>
            {
                // 计算坐标
                int localX = index % width;
                int localY = index / width;
                int worldX = fromX + localX;
                int worldY = fromY + localY;

                // 计算距离平方
                float dx = worldX - config.area_Center.x;
                float dy = worldY - config.area_Center.y;
                float distanceSqr = dx * dx + dy * dy;

                // 关键修复：打破对称性的噪声计算
                float noiseX = (worldX + config.noise_Offset.x + randomOffsetX) * noiseStep;
                float noiseY = (worldY + config.noise_Offset.y + randomOffsetY) * noiseStep;

                // 添加微小扰动，彻底打破对称性
                float epsilon = 0.001f;
                noiseX += epsilon * (worldX % 7); // 使用质数避免模式重复
                noiseY += epsilon * (worldY % 11);
                distanceCache[index] = distanceSqr;
                xCoordinates[index] = worldX;
                yCoordinates[index] = worldY;

                noiseCache[index] = Mathf.PerlinNoise(noiseX, noiseY);
            });
        });

        int batchSize = CalculateOptimalBatchSize(totalPoints);
        int totalBatches = Mathf.CeilToInt((float)totalPoints / batchSize);

        // 处理每个批次
        for (int batch = 0; batch < totalBatches; batch++)
        {
            int startIndex = batch * batchSize;
            int endIndex = Mathf.Min(startIndex + batchSize, totalPoints);

            // 异步处理当前批次
            await Task.Run(() =>
            {
                GenerateEvenlyArea_ProcessBatch(startIndex, endIndex, xCoordinates, yCoordinates, distanceCache, noiseCache,
                           fillRadiusSqr, outerRadiusSqr, transitionZone, config, action);
            });

            // 更新进度
            if (totalPoints > 1000) slider_Waiting.value = (float)batch / totalBatches;

            // 每处理几个批次让出控制权
            if (batch % 10 == 5)
                await Task.Yield();
        }
    }
    /// <summary>
    /// 处理一个批次的数据(均匀填充)
    /// </summary>
    private void GenerateEvenlyArea_ProcessBatch(
        int startIndex, int endIndex,
        int[] xCoords, int[] yCoords, float[] distanceCache, float[] noiseCache,
        float fillRadiusSqr, float outerRadiusSqr, float transitionZone,
        PerlinEvenlyConfig config, Action<int, float, float> action)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            float distanceSqr = distanceCache[i];
            float perlinNoise = noiseCache[i];
            float realNoise;

            // 根据位置分类处理
            if (distanceSqr <= fillRadiusSqr)
            {
                // 核心区域：完全填充
                realNoise = perlinNoise;
            }
            else if (distanceSqr >= outerRadiusSqr)
            {
                // 外部区域：完全为空
                realNoise = 0f;
            }
            else
            {
                // 边缘过渡区域
                float distance = Mathf.Sqrt(distanceSqr);

                // 计算距离衰减因子 (0-1)
                // 从fill到outer，t从1降到0
                float t = (config.area_OuterRadius - distance) / transitionZone * perlinNoise;

                // 确保在合法范围内
                t = Mathf.Clamp01(t);

                // 混合噪声和距离衰减
                realNoise = t;
            }
            // 返回结果
            action.Invoke(Vector2ToIndex(xCoords[i], yCoords[i]), perlinNoise, realNoise);
        }
    }

    #endregion
    #region 柏林河流曲线生成
    public struct RiverConfig
    {
        /// <summary>
        /// 河流起点
        /// </summary>
        public Vector2 vector2_Start;
        /// <summary>
        /// 河流终点
        /// </summary>
        public Vector2 vector2_End;
        /// <summary>
        /// 河流噪声尺寸
        /// </summary>
        public float river_NoiseScale;
        /// <summary>
        /// 河流噪声偏移
        /// </summary>
        public Vector2 river_NoiseOffset;
        /// <summary>
        /// 河流曲度
        /// </summary>
        public float river_Curvature;
        /// <summary>
        /// 河流大致方向影响
        /// </summary>
        public float river_DirectionInfluence;
        /// <summary>
        /// 河流宽度(直径) 
        /// </summary>
        public int river_Width;
        /// <summary>
        /// 河流分叉概率1/1000
        /// </summary>
        public int river_Forking;
        /// <summary>
        /// 河流最大分叉次数
        /// </summary>
        public int river_ForkCount;
        /// <summary>
        /// 河流分叉最小距离
        /// </summary>
        public float river_ForkMinDistance;
        /// <summary>
        /// 单段河流最大步数限制
        /// </summary>
        public int river_MaxStep;
        /// <summary>
        /// 河流到达终点的容差距离
        /// </summary>
        public float river_ArrivalTolerance;
        /// <summary>
        /// 河流边界
        /// </summary>
        public Rect river_Bounds;
        /// <summary>
        /// 是否合规
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return vector2_Start != vector2_End &&
                   river_NoiseScale > 0 &&
                   river_Width > 0 &&
                   river_Bounds.width > 0 && river_Bounds.height > 0 &&
                   river_MaxStep > 0;
        }
        /// <summary>
        /// 预计长度
        /// </summary>
        /// <returns></returns>
        public float EstimatedLength()
        {
            return Vector2.Distance(vector2_Start, vector2_End) * 1.5f; // 考虑弯曲
        }
    }

    /// <summary>
    /// 河流分段数据
    /// </summary>
    public struct RiverSegment
    {
        public Vector2 position;
        public int width;
        public Vector2 direction;
        public bool isBorder; // 是否为边缘（河岸）
        public int parentId;  // 父河流ID（用于分叉）
        public int index;     // 编号
        public int depth;     // 深度（主河流为0，分叉递增）
    }
    /// <summary>
    /// 河流分支任务
    /// </summary>
    private struct RiverBranchTask
    {
        public RiverConfig config;
        public int branchId;
        public int depth;
        public Vector2 startPoint;
        public Vector2 endPoint;
    }
    public async Task GenerateRiverSystemAsync(RiverConfig config, Action<RiverSegment> onSegmentCreated, Action<RiverSegment> onBridgeCreated, Func<RiverSegment, bool> cancellation = null)
    {
        if (!config.IsValid())
            throw new ArgumentException("河流配置无效");
        /*河流信息*/
        List<RiverSegment> allSegments = new List<RiverSegment>();
        /*河流任务*/
        Queue<RiverBranchTask> branchQueue = new Queue<RiverBranchTask>();
        //添加主河流任务
        branchQueue.Enqueue(new RiverBranchTask
        {
            config = config,
            branchId = 0,
            depth = 0,
            startPoint = config.vector2_Start,
            endPoint = config.vector2_End
        });
        int totalBranches = 1; // 至少有一条主河流
        int processedBranches = 0;

        while (branchQueue.Count > 0)
        {
            var task = branchQueue.Dequeue();

            /*生成单条河流*/
            var segments = await GenerateSingleRiverAsync(
                task.config,
                task.startPoint,
                task.endPoint,
                task.depth,
                task.branchId,
                onSegmentCreated, onBridgeCreated, cancellation);

            allSegments.AddRange(segments);

            /*获取支流分叉点*/
            var forkPoints = FindForkPoints(segments, task.config, task.depth, task.branchId);
            foreach (var forkPoint in forkPoints)
            {
                /*检查是否达到支流上限*/
                if (totalBranches < task.config.river_ForkCount + 1) // +1 包含主河流
                {
                    /*获取支流方向*/
                    Vector2 forkDirection = CalculateForkDirection(forkPoint.direction, forkPoint.position, task.config.river_Bounds, totalBranches);
                    /*创建支流任务*/
                    branchQueue.Enqueue(new RiverBranchTask
                    {
                        config = task.config,
                        branchId = totalBranches,
                        depth = task.depth + 1,
                        startPoint = forkPoint.position,
                        endPoint = forkPoint.position + forkDirection * 200f // 暂时长度
                    });
                    totalBranches++;
                }
            }
            processedBranches++;
            slider_Waiting.value = (float)processedBranches / branchQueue.Count;
        }

    }
    /// <summary>
    /// 生成单条河流
    /// </summary>
    /// <param name="config"></param>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <param name="depth"></param>
    /// <param name="branchId"></param>
    /// <param name="onSegmentCreated"></param>
    /// <returns></returns>
    private async Task<List<RiverSegment>> GenerateSingleRiverAsync(RiverConfig config, Vector2 startPoint, Vector2 endPoint, int depth, int branchId, Action<RiverSegment> onSegmentCreated, Action<RiverSegment> onBridgeCreated, Func<RiverSegment, bool> cancellation = null)
    {

        List<RiverSegment> segments = new List<RiverSegment>();
        Vector2 currentPos = startPoint;
        Vector2 direction = (endPoint - startPoint).normalized;

        float noiseScale = 1 / config.river_NoiseScale;
        Vector2 noiseOffset = GetRandomOffset(branchId);

        // 动态宽度计算
        float currentWidth = config.river_Width - depth; // 深度越大，宽度越小
        currentWidth = Mathf.Max(2f, currentWidth);

        int stepCount = 0;
        /*总计距离*/
        float totalDistance = 0;
        /*最大距离*/
        float maxDistance = config.EstimatedLength() * 2f; // 安全限制
        float arrivalToleranceSpr = config.river_ArrivalTolerance * config.river_ArrivalTolerance;
        while (stepCount < config.river_MaxStep && totalDistance < maxDistance)
        {

            // 终点检查
            float distanceToEndSpr = Vector2.SqrMagnitude(currentPos - endPoint);
            if (distanceToEndSpr < arrivalToleranceSpr) break;
            // 宽度变化
            if (stepCount % 10 == 0)
            {
                //越靠近终点越窄
                if (distanceToEndSpr < arrivalToleranceSpr + 100) { currentWidth = Mathf.Min(1, currentWidth); }
                else if (distanceToEndSpr < arrivalToleranceSpr + 900) { currentWidth = Mathf.Min(2, currentWidth); }
                else if (distanceToEndSpr < arrivalToleranceSpr + 2500) { currentWidth = Mathf.Min(3, currentWidth); }
            }
            // 边界检查

            if (!config.river_Bounds.Contains(currentPos)) break;

            // 添加噪声弯曲
            float noise = Mathf.PerlinNoise(currentPos.x * noiseScale + noiseOffset.x, currentPos.y * noiseScale + noiseOffset.y);

            float noiseAngle = (noise - 0.5f) * 2f * Mathf.PI * config.river_Curvature;

            // 旋转当前方向
            Vector2 noiseDirection = RotateVector(direction, noiseAngle);

            // 朝向终点方向
            Vector2 toEnd = (endPoint - currentPos).normalized;

            // 混合噪声方向和目标方向
            direction = Vector3.Slerp(noiseDirection, toEnd, config.river_DirectionInfluence).normalized;

            // 移动位置
            Vector2 prevPos = currentPos;
            currentPos += direction;
            totalDistance += Vector2.Distance(prevPos, currentPos);

            // 创建河流分段
            var segment = new RiverSegment
            {
                position = currentPos,
                width = Mathf.RoundToInt(currentWidth),
                direction = direction,
                isBorder = false,
                parentId = branchId,
                index = stepCount,
                depth = depth
            };
            //交汇检查(步长大于一定值才检测)
            if (cancellation != null && cancellation.Invoke(segment) && stepCount > 5) break;

            segments.Add(segment);

            stepCount++;
            // 定期让出控制权
            if (stepCount % 50 == 25)
                await Task.Yield();
        }
        for (int i = 0; i < segments.Count; i++)
        {
            onSegmentCreated?.Invoke(segments[i]);
            stepCount++;
            // 定期让出控制权
            if (stepCount % 50 == 25)
            {
                slider_Waiting.value = (float)i / segments.Count;
                await Task.Yield();
            }
        }
        for (int i = 0; i < segments.Count; i++)
        {
            onBridgeCreated?.Invoke(segments[i]);
            stepCount++;
            // 定期让出控制权
            if (stepCount % 50 == 25)
            {
                slider_Waiting.value = (float)i / segments.Count;
                await Task.Yield();
            }
        }
        return segments;
    }
    private List<RiverSegment> FindForkPoints(List<RiverSegment> segments, RiverConfig config, int currentDepth, int branchId)
    {
        List<RiverSegment> forkPoints = new List<RiverSegment>();

        if (currentDepth >= 3) // 限制分叉深度
            return forkPoints;

        float minForkDistSqr = config.river_ForkMinDistance * config.river_ForkMinDistance;

        for (int i = 10; i < segments.Count - 10; i += 5) // 每隔5个点检查一次
        {
            // 检查分叉概率
            if (GetRandomRange(branchId + i) * 1000 >= config.river_Forking) continue;
            var segment = segments[i];

            // 确保不是太靠近起点或终点
            if (i < 20 || i > segments.Count - 20)
                continue;

            // 检查与已有分叉点的距离
            bool tooClose = false;
            foreach (var existing in forkPoints)
            {
                if (Vector2.SqrMagnitude(segment.position - existing.position) < minForkDistSqr)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                forkPoints.Add(segment);
        }

        return forkPoints;
    }
    private Vector2 CalculateForkDirection(Vector2 currentDirection, Vector2 position, Rect bounds, int index)
    {
        // 尝试不同的分叉角度
        float[] possibleAngles = { 90f, -90f, 60f, -60f };

        foreach (var angle in possibleAngles)
        {
            Vector2 forkDir = RotateVector(currentDirection, angle * Mathf.Deg2Rad);

            // 检查这个方向是否合理（不立即超出边界）
            Vector2 testPos = position + forkDir * 20f;
            if (bounds.Contains(testPos))
                return forkDir;
        }

        // 如果没有合适方向，随机选择
        float randomAngle = (float)(GetRandomRange(index) * Mathf.PI * 2);
        return RotateVector(currentDirection, randomAngle);
    }
    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
    }
    #endregion
    #region 泊松单点分布区域
    private float[,] distanceCache; // 缓存距离场
    public struct PoissonPointsAreaConfig
    {
        /// <summary>
        /// 中心
        /// </summary>
        public Vector2Int pointsArea_Center;
        /// <summary>
        /// 尺寸
        /// </summary>
        public int pointsArea_Size;
        /// <summary>
        /// 两个点之间的最小距离
        /// </summary>
        public float pointsArea_MinDistance;
        /// <summary>
        /// 两个点之间的最大距离
        /// </summary>
        public float pointsArea_MaxDistance;
        /// <summary>
        /// 噪声缩放
        /// </summary>
        public float pointsArea_NoiseScale;
        /// <summary>
        /// 噪声偏移(0,1)
        /// </summary>
        public Vector2 pointsArea_NoiseOffset;

        /// <summary>
        /// 采样最大次数
        /// </summary>
        public int pointsArea_MaxSamples;
        public bool IsValid()
        {
            return pointsArea_Size > 0 && pointsArea_MinDistance > 0 && pointsArea_MaxSamples > 0;
        }
        public int EstimatedMaxPoints()
        {
            float area = pointsArea_Size * pointsArea_Size * 4;
            float distance = (pointsArea_MinDistance + pointsArea_MaxDistance) * 0.5f;
            float circleArea = Mathf.PI * distance * distance;
            return Mathf.CeilToInt(area / circleArea * 0.8f); // 80%填充率估计
        }
    }
    public async Task GeneratePoissonPointsAsync(PoissonPointsAreaConfig config, Action<Vector2Int> action)
    {
        if (!config.IsValid())
            throw new ArgumentException("配置参数无效");
        List<Vector2Int> activePoints = new List<Vector2Int>();
        List<Vector2Int> finalPoints = new List<Vector2Int>();

        /*单位尺寸*/
        float cellSize = config.pointsArea_MinDistance / Mathf.Sqrt(2);
        /*网格宽度*/
        int gridWidth = Mathf.CeilToInt(config.pointsArea_Size * 2 / cellSize);
        /*网格高度*/
        int gridHeight = Mathf.CeilToInt(config.pointsArea_Size * 2 / cellSize);
        int?[,] grid = new int?[gridWidth, gridHeight];

        // ========== 绘制距离缓存 ==========
        PrecomputeDistanceField(config);
        // ========== 添加第一个点 ==========
        Vector2Int firstPoint = new Vector2Int(config.pointsArea_Size, config.pointsArea_Size);
        finalPoints.Add(firstPoint);
        activePoints.Add(firstPoint);
        SetGridValue(grid, cellSize, firstPoint, 0);

        /*总数*/
        int totalProcessed = 0;
        int maxIterations = config.EstimatedMaxPoints() * config.pointsArea_MaxSamples;

        while (activePoints.Count > 0 /*&& !cancellationToken.IsCancellationRequested*/)
        {
            totalProcessed++;

            // 随机选择一个活跃点
            int activeIndex = UnityEngine.Random.Range(0, activePoints.Count);
            Vector2Int activePoint = activePoints[activeIndex];

            bool foundValidPoint = false;

            // 尝试生成新点
            for (int sample = 0; sample < config.pointsArea_MaxSamples; sample++)
            {
                // 在环形区域中生成随机点
                Vector2Int candidate = GenerateCandidatePoint(activePoint, distanceCache[activePoint.x, activePoint.y], config.pointsArea_Size);

                // 检查候选点是否有效
                if (IsPointValid(candidate, config, cellSize, grid, finalPoints))
                {
                    // 添加新点
                    finalPoints.Add(candidate);
                    activePoints.Add(candidate);
                    SetGridValue(grid, cellSize, candidate, finalPoints.Count - 1);

                    foundValidPoint = true;
                    break;
                }
            }
            // 如果当前活跃点无法生成新点，则移除
            if (!foundValidPoint)
            {
                activePoints.RemoveAt(activeIndex);
            }

            // 更新进度（更准确的方式）
            if (totalProcessed % 100 == 50)
            {
                float currentProgress = 1f - ((float)activePoints.Count / config.EstimatedMaxPoints());
                slider_Waiting.value = Mathf.Clamp01(currentProgress);
                // 每100次迭代让出控制权
                await Task.Yield();
            }
        }
        // ========== 应用偏移并返回结果 ==========
        Vector2Int offset = CalculateOffset(config);
        List<Vector2Int> worldPoints = ApplyOffset(finalPoints, offset);
        for (int i = 0; i < worldPoints.Count; i++)
        {
            action.Invoke(worldPoints[i]);
        }
    }
    /// <summary>
    /// 在网格中设置点的索引
    /// </summary>
    private void SetGridValue(int?[,] grid, float cellSize, Vector2Int point, int index)
    {
        int gridX = Mathf.FloorToInt(point.x / cellSize);
        int gridY = Mathf.FloorToInt(point.y / cellSize);

        if (gridX >= 0 && gridX < grid.GetLength(0) && gridY >= 0 && gridY < grid.GetLength(1))
        {
            grid[gridX, gridY] = index;
        }
    }
    /// <summary>
    /// 检查点是否有效
    /// </summary>
    private bool IsPointValid(Vector2Int candidate, PoissonPointsAreaConfig config, float cellSize, int?[,] grid, List<Vector2Int> existingPoints)
    {
        // 检查边界
        if (candidate.x < 0 || candidate.x >= config.pointsArea_Size * 2 ||
            candidate.y < 0 || candidate.y >= config.pointsArea_Size * 2)
            return false;// 候选点超出区域范围

        // 计算网格坐标
        int cellX = Mathf.FloorToInt(candidate.x / cellSize);
        int cellY = Mathf.FloorToInt(candidate.y / cellSize);

        // 获取候选点的动态最小距离
        float minDistance = distanceCache[candidate.x, candidate.y];
        float minDistSqr = minDistance * minDistance;

        // 搜索周围的网格单元
        int searchRadius = Mathf.CeilToInt(2f * minDistance / cellSize);

        int startX = Mathf.Max(0, cellX - searchRadius);
        int endX = Mathf.Min(grid.GetLength(0) - 1, cellX + searchRadius);
        int startY = Mathf.Max(0, cellY - searchRadius);
        int endY = Mathf.Min(grid.GetLength(1) - 1, cellY + searchRadius);

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                int? pointIndex = grid[x, y];
                if (pointIndex.HasValue)
                {
                    Vector2Int existingPoint = existingPoints[pointIndex.Value];
                    float dx = candidate.x - existingPoint.x;
                    float dy = candidate.y - existingPoint.y;
                    float sqrDist = dx * dx + dy * dy;


                    float existingMinDistance = distanceCache[existingPoint.x, existingPoint.y];
                    float combinedMinDistSqr = Mathf.Max(minDistSqr, existingMinDistance * existingMinDistance);

                    if (sqrDist < combinedMinDistSqr) return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// 生成候选点（在环形区域中随机采样）
    /// </summary>
    private Vector2Int GenerateCandidatePoint(Vector2Int center, float minDistance, int regionSize)
    {
        // 在[minDistance, 2*minDistance]的环形区域内采样
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        float distance = UnityEngine.Random.Range(minDistance, minDistance * 2f);

        float x = center.x + Mathf.Cos(angle) * distance;
        float y = center.y + Mathf.Sin(angle) * distance;

        // 确保点在区域内
        x = Mathf.Clamp(x, 0, regionSize * 2 - 1);
        y = Mathf.Clamp(y, 0, regionSize * 2 - 1);

        return new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }
    /// <summary>
    /// 计算世界坐标偏移
    /// </summary>
    private Vector2Int CalculateOffset(PoissonPointsAreaConfig config)
    {
        int offsetX = config.pointsArea_Center.x - config.pointsArea_Size;
        int offsetY = config.pointsArea_Center.y - config.pointsArea_Size;
        return new Vector2Int(offsetX, offsetY);
    }
    /// <summary>
    /// 应用坐标偏移
    /// </summary>
    private List<Vector2Int> ApplyOffset(List<Vector2Int> localPoints, Vector2Int offset)
    {
        List<Vector2Int> worldPoints = new List<Vector2Int>(localPoints.Count);

        foreach (var point in localPoints)
        {
            worldPoints.Add(new Vector2Int(
                point.x + offset.x,
                point.y + offset.y
            ));
        }

        return worldPoints;
    }
    public void PrecomputeDistanceField(PoissonPointsAreaConfig config)
    {
        int areaSize = config.pointsArea_Size * 2;
        distanceCache = new float[areaSize, areaSize];

        float noiseStep = 1f / config.pointsArea_NoiseScale;

        System.Random rand = new System.Random(config.pointsArea_NoiseOffset.GetHashCode());
        float randomOffsetX = (float)rand.NextDouble() * 1000f;
        float randomOffsetY = (float)rand.NextDouble() * 1000f;


        for (int x = 0; x < areaSize; x++)
        {
            for (int y = 0; y < areaSize; y++)
            {
                Vector2Int point = new Vector2Int(x, y);
                // 关键修复：打破对称性的噪声计算
                float noiseX = (point.x + config.pointsArea_NoiseOffset.x + randomOffsetX) * noiseStep;
                float noiseY = (point.y + config.pointsArea_NoiseOffset.y + randomOffsetY) * noiseStep;

                // 采样Perlin Noise (范围 0-1)
                float noiseValue = Mathf.PerlinNoise(noiseX, noiseY);
                //if (noiseValue < 0.3f) noiseValue = 0;
                //else if (noiseValue > 0.7f) noiseValue = 1;
                // 映射到密度系数范围
                distanceCache[x, y] = Mathf.Lerp(config.pointsArea_MinDistance, config.pointsArea_MaxDistance, noiseValue);
            }
        }
    }

    #endregion
    #region 随机单点分布区域(渐变/不渐变)
    public struct RandomPointsAreaConfig
    {
        /// <summary>
        /// 中心
        /// </summary>
        public Vector2Int pointsArea_Center;
        /// <summary>
        /// 点数量
        /// </summary>
        public int pointsArea_Count;
        /// <summary>
        /// 整体尺寸
        /// </summary>
        public int pointsArea_SizeWhole;
        /// <summary>
        /// 填充尺寸
        /// </summary>
        public int pointsArea_SizeFill;
    }
    public async Task GenerateRandomPointsArea_Gradual(RandomPointsAreaConfig areaConfig, Action<int> action)
    {
        System.Random random = new System.Random(GetAccIndex() + seed_Offset);
        float reciprocal_Count = 1f / areaConfig.pointsArea_Count;

        int from_x = (areaConfig.pointsArea_Center.x - areaConfig.pointsArea_SizeWhole);
        int to_x = (areaConfig.pointsArea_Center.x + areaConfig.pointsArea_SizeWhole);
        int from_y = (areaConfig.pointsArea_Center.y - areaConfig.pointsArea_SizeWhole);
        int to_y = (areaConfig.pointsArea_Center.y + areaConfig.pointsArea_SizeWhole);

        float sqrSizeFill = areaConfig.pointsArea_SizeFill * areaConfig.pointsArea_SizeFill;
        float sizeWhole = areaConfig.pointsArea_SizeWhole;
        float sizeFill = areaConfig.pointsArea_SizeFill;
        float sizeEmpty = sizeWhole - sizeFill;
        for (int i = 0; i < areaConfig.pointsArea_Count; i++)
        {
            // 随机生成位置
            int randomX = random.Next(from_x, to_x);
            int randomY = random.Next(from_y, to_y);
            float dx = randomX - areaConfig.pointsArea_Center.x;
            float dy = randomY - areaConfig.pointsArea_Center.y;
            float sqrDistance = dx * dx + dy * dy;

            if (sqrDistance > sqrSizeFill)
            {
                float distance = Mathf.Sqrt(sqrDistance);
                float probability = Mathf.Clamp01((sizeWhole - distance) / sizeEmpty);
                if (random.NextDouble() < probability)
                {
                    action.Invoke(Vector2ToIndex(randomX, randomY));
                }
            }
            else
            {
                action.Invoke(Vector2ToIndex(randomX, randomY));
            }
            // 进度更新
            if (i % 500 == 250)
            {
                slider_Waiting.value = i * reciprocal_Count;
                await Task.Yield();
            }
        }
    }
    public async Task GenerateRandomPointsArea_Hard(RandomPointsAreaConfig areaConfig, Action<int, int> action)
    {
        System.Random random = new System.Random(GetAccIndex() + seed_Offset);
        float reciprocal_Count = 1f / areaConfig.pointsArea_Count;

        int from_x = (areaConfig.pointsArea_Center.x - areaConfig.pointsArea_SizeWhole);
        int to_x = (areaConfig.pointsArea_Center.x + areaConfig.pointsArea_SizeWhole);
        int from_y = (areaConfig.pointsArea_Center.y - areaConfig.pointsArea_SizeWhole);
        int to_y = (areaConfig.pointsArea_Center.y + areaConfig.pointsArea_SizeWhole);

        for (int i = 0; i < areaConfig.pointsArea_Count; i++)
        {
            // 随机生成位置
            int randomX = random.Next(from_x, to_x);
            int randomY = random.Next(from_y, to_y);

            action.Invoke(randomX, randomY);
            // 进度更新
            if (i % 100 == 50)
            {
                slider_Waiting.value = i * reciprocal_Count;
                await Task.Yield();
            }
        }
    }
    #endregion
    #region 随机扩张单点组分布区域(不渐变)
    public struct RandomExpandPointsAreaConfig
    {
        /// <summary>
        /// 中心
        /// </summary>
        public Vector2Int pointsArea_Center;
        /// <summary>
        /// 点数量
        /// </summary>
        public int pointsArea_Count;
        /// <summary>
        /// 点扩张趋势1-100
        /// </summary>
        public int pointsArea_Expand;
        /// <summary>
        /// 整体尺寸
        /// </summary>
        public int pointsArea_SizeWhole;
    }
    public async Task GenerateRandomExpandPointsArea(RandomExpandPointsAreaConfig areaConfig, Action<int, int> action)
    {
        System.Random random = new System.Random(GetAccIndex() + seed_Offset);
        float reciprocal_Count = 1f / areaConfig.pointsArea_Count;

        int from_x = (areaConfig.pointsArea_Center.x - areaConfig.pointsArea_SizeWhole);
        int to_x = (areaConfig.pointsArea_Center.x + areaConfig.pointsArea_SizeWhole);
        int from_y = (areaConfig.pointsArea_Center.y - areaConfig.pointsArea_SizeWhole);
        int to_y = (areaConfig.pointsArea_Center.y + areaConfig.pointsArea_SizeWhole);


        for (int i = 0; i < areaConfig.pointsArea_Count; i++)
        {
            // 随机生成位置
            int randomX = random.Next(from_x, to_x);
            int randomY = random.Next(from_y, to_y);
            int expandChance = Math.Clamp(areaConfig.pointsArea_Expand, 1, 100);
            int maxExpansionSteps = 10;
            action.Invoke(randomX, randomY);
            ExpandFromPoint(randomX, randomY, random, expandChance, maxExpansionSteps, action);
            // 进度更新
            if (i % 100 == 50)
            {
                slider_Waiting.value = i * reciprocal_Count;
                await Task.Yield();
            }
        }
    }
    private void ExpandFromPoint(int startX, int startY, System.Random random, int expandChance, int maxSteps, Action<int, int> action)
    {
        int currentX = 0;
        int currentY = 0;
        int steps = 0;

        // 第一阶段：主要向右上扩张
        while (steps < maxSteps && random.Next(100) < expandChance)
        {
            int dx = random.Next(0, 2);
            int dy = random.Next(0, 2);

            if (dx == 0 && dy == 0)
                continue;

            currentX += dx;
            currentY += dy;
            steps++;

            action.Invoke(startX + currentX, startY + currentY);
        }

        // 第二阶段：允许更多方向的扩张
        while (steps < maxSteps && random.Next(100) < expandChance)
        {
            int dx = random.Next(-1, 2);
            int dy = random.Next(0, 2);

            if (dx == 0 && dy == 0)
                continue;

            currentX += dx;
            currentY += dy;
            steps++;

            action.Invoke(startX + currentX, startY + currentY);
        }
    }
    #endregion
}

