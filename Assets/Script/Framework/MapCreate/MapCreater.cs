using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static Fusion.Allocator;
using static Fusion.Sockets.NetBitBuffer;

public class MapCreater 
{
    public MapConfig config_Map;
    public int seed_Offset;
    public Slider slider_Waiting;
    public TextMeshProUGUI text_Waiting;
    /// <summary>
    /// ��������
    /// </summary>
    public MapTileTypeData data_mapGroundData;
    /// <summary>
    /// ��������
    /// </summary>
    public MapTileTypeData data_mapBuildingData;
    public MapCreate_Land mapCreate_Land = new MapCreate_Land();
    public MapCreate_Snowland mapCreate_Snowland = new MapCreate_Snowland();
    public MapCreate_Desert mapCreate_Desert = new MapCreate_Desert();
    public MapCreate_InlandSea mapCreate_InlandSea = new MapCreate_InlandSea();
    public MapCreate_River mapCreate_River = new MapCreate_River();
    public MapCreate_Lake mapCreate_Lake = new MapCreate_Lake();
    public MapCreate_Road mapCreate_Road = new MapCreate_Road();
    public MapCreate_Forest mapCreate_Forest = new MapCreate_Forest();
    public MapCreate_Mining mapCreate_Mining = new MapCreate_Mining();
    public MapCreate_Animal mapCreate_Animal = new MapCreate_Animal();
    public MapCreate_Monster mapCreate_Monster = new MapCreate_Monster();
    public MapCreate_RandomBuilding mapCreate_RandomBuilding = new MapCreate_RandomBuilding();
    public MapCreate_ConfigBuilding mapCreate_ConfigBuilding = new MapCreate_ConfigBuilding();
    public MapCreate_Supply mapCreate_Supply = new MapCreate_Supply();
    public MapCreate_GroupPlant mapCreate_GroupPlant = new MapCreate_GroupPlant();
    public MapCreate_SinglePlant mapCreate_SinglePlant = new MapCreate_SinglePlant();
    public async Task CreateMapGroundAndBuilding(MapConfig config, Slider slider, TextMeshProUGUI text, Action<MapTileTypeData, MapTileTypeData> callBack_ReturnData)
    {
        seed_Offset = config.map_Seed;
        config_Map = config;
        slider_Waiting = slider;
        text_Waiting = text;
        data_mapGroundData = new MapTileTypeData();
        data_mapBuildingData = new MapTileTypeData();

        await mapCreate_Land.CreateIslandArea(this);
        await mapCreate_Snowland.CreateSnowland(this);
        await mapCreate_Desert.CreateDesert(this);
        await mapCreate_InlandSea.CreateInlandSea(this);
        await mapCreate_InlandSea.CreateIslet(this);
        await mapCreate_River.CreateRiver(this);
        await mapCreate_Lake.CreateLake(this);
        await mapCreate_Mining.CreateMining(this);
        await mapCreate_Forest.CreateForest(this);
        await mapCreate_Road.CreateRoad(this);
        await mapCreate_Animal.CreateAnimal(this);
        await mapCreate_Monster.CreateMonster(this);
        await mapCreate_RandomBuilding.CreateRandomBuilding(this);
        await mapCreate_ConfigBuilding.CreateConfigBuilding(this);
        await mapCreate_Supply.CreateSupply(this);
        await mapCreate_GroupPlant.CreateGroupPlant(this);
        await mapCreate_SinglePlant.CreateSinglePlant(this);
        callBack_ReturnData.Invoke(data_mapGroundData, data_mapBuildingData);
    }
    #region//��������
    /// <summary>
    /// ���һ������(��)
    /// </summary>
    /// <param name="targetArea">Ŀ������</param>
    /// <param name="targetNoiseConfig_Main">һ������</param>
    /// <param name="targetNoiseConfig_Child">��������</param>
    /// <param name="fillID">�������</param>
    /// <returns></returns>
    private async Task FillArea(MapTileTypeData data, AreaConfig targetArea, NoiseConfig targetNoiseConfig, short fillID)
    {
        float size = targetArea.area_Size;
        int from_x = targetArea.area_Center.x - targetArea.area_Size / 2;
        int to_x = targetArea.area_Center.x + targetArea.area_Size / 2;
        int from_y = targetArea.area_Center.y - targetArea.area_Size / 2;
        int to_y = targetArea.area_Center.y + targetArea.area_Size / 2;

        int index_x = 0;
        int index_y = 0;
        float distanceToBoundary_x;
        float distanceToBoundary_y;
        float distanceToBoundary_min;


        float scale_Main = 1f / targetNoiseConfig.noise_Sacle;
        float prob_Main = 1 - targetNoiseConfig.noise_Prob;
        float noiseVal_Main;
        float offsetX_Main = targetNoiseConfig.noise_Offset.x * targetNoiseConfig.noise_Sacle;
        float offsetY_Main = targetNoiseConfig.noise_Offset.y * targetNoiseConfig.noise_Sacle;

        for (int i = from_x; i < to_x; i++)
        {
            index_x++;
            index_y = 0;
            distanceToBoundary_x = MathF.Min(index_x, size - index_x);
            for (int j = from_y; j < to_y; j++)
            {
                index_y++;
                distanceToBoundary_y = Mathf.Min(index_y, size - index_y);
                distanceToBoundary_min = Mathf.Min(distanceToBoundary_y, distanceToBoundary_x);
                noiseVal_Main = Mathf.PerlinNoise(offsetX_Main + index_x * scale_Main, offsetY_Main + index_y * scale_Main);
                if (noiseVal_Main > Mathf.Lerp(1, prob_Main, distanceToBoundary_min * 0.1f))
                {
                    int tempX = i + 30000;
                    int tempY = j + 30000;
                    int tempIndex;
                    if (tempY > tempX)
                    {
                        tempIndex = tempY * tempY + tempY + tempY - tempX;
                    }
                    else
                    {
                        tempIndex = tempX * tempX + tempY;
                    }
                    if (data.tileDic.ContainsKey(tempIndex))
                    {
                        data.tileDic[tempIndex] = fillID;
                    }
                    else
                    {
                        data.tileDic.Add(tempIndex, fillID);
                    }
                }
            }
            await Task.Yield();
            slider_Waiting.value = index_x / size;
        }
    }
    /// <summary>
    /// ���һ������(����)
    /// </summary>
    /// <param name="targetArea">Ŀ������</param>
    /// <param name="targetNoiseConfig_Main">һ������</param>
    /// <param name="targetNoiseConfig_Child">��������</param>
    /// <param name="fillID">�������</param>
    /// <returns></returns>
    private async Task FillArea(MapTileTypeData data, AreaConfig targetArea, NoiseConfig targetNoiseConfig_Main, NoiseConfig targetNoiseConfig_Child, short fillID)
    {
        float size = targetArea.area_Size;
        int from_x = targetArea.area_Center.x - targetArea.area_Size / 2;
        int to_x = targetArea.area_Center.x + targetArea.area_Size / 2;
        int from_y = targetArea.area_Center.y - targetArea.area_Size / 2;
        int to_y = targetArea.area_Center.y + targetArea.area_Size / 2;

        int index_x = 0;
        int index_y = 0;
        float distanceToBoundary_x;
        float distanceToBoundary_y;
        float distanceToBoundary_min;


        float scale_Main = 1f / targetNoiseConfig_Main.noise_Sacle;
        float prob_Main = 1 - targetNoiseConfig_Main.noise_Prob;
        float noiseVal_Main;
        float offsetX_Main = targetNoiseConfig_Main.noise_Offset.x * targetNoiseConfig_Main.noise_Sacle;
        float offsetY_Main = targetNoiseConfig_Main.noise_Offset.y * targetNoiseConfig_Main.noise_Sacle;

        float scale_Child = 1f / targetNoiseConfig_Child.noise_Sacle;
        float prob_Child = 1 - targetNoiseConfig_Child.noise_Prob;
        float noiseVal_Child;
        float offsetX_Child = targetNoiseConfig_Child.noise_Offset.x * targetNoiseConfig_Child.noise_Sacle;
        float offsetY_Child = targetNoiseConfig_Child.noise_Offset.y * targetNoiseConfig_Child.noise_Sacle;

        for (int i = from_x; i < to_x; i++)
        {
            index_x++;
            index_y = 0;
            distanceToBoundary_x = MathF.Min(index_x, size - index_x);
            for (int j = from_y; j < to_y; j++)
            {
                index_y++;
                distanceToBoundary_y = Mathf.Min(index_y, size - index_y);
                distanceToBoundary_min = Mathf.Min(distanceToBoundary_y, distanceToBoundary_x);
                noiseVal_Main = Mathf.PerlinNoise(offsetX_Main + index_x * scale_Main, offsetY_Main + index_y * scale_Main);
                noiseVal_Child = Mathf.PerlinNoise(offsetX_Child + index_x * scale_Child, offsetY_Child + index_y * scale_Child);
                if (noiseVal_Main > Mathf.Lerp(1, prob_Main, distanceToBoundary_min * 0.1f) && noiseVal_Child > prob_Child)
                {
                    int tempX = i + 30000;
                    int tempY = j + 30000;
                    int tempIndex;
                    if (tempY > tempX)
                    {
                        tempIndex = tempY * tempY + tempY + tempY - tempX;
                    }
                    else
                    {
                        tempIndex = tempX * tempX + tempY;
                    }
                    if (data.tileDic.ContainsKey(tempIndex))
                    {
                        data.tileDic[tempIndex] = fillID;
                    }
                    else
                    {
                        data.tileDic.Add(tempIndex, fillID);
                    }
                }
            }
            await Task.Yield();
            slider_Waiting.value = index_x / size;
        }
    }
    /// <summary>
    /// ������ƫ��
    /// </summary>
    /// <returns></returns>
    public Vector2 GetRandomOffset()
    {
        seed_Offset++;
        UnityEngine.Random.InitState(seed_Offset);
        float x = UnityEngine.Random.Range(0f, 0.5f);
        seed_Offset++;
        UnityEngine.Random.InitState(seed_Offset);
        float y = UnityEngine.Random.Range(0f, 0.5f);
        return new Vector2(x, y);
    }

    #endregion
    #region//���ֵ�״������ɢ����
    public struct IslandAreaConfig
    {
        /// <summary>
        /// ��������ߴ�
        /// </summary>
        public float island_WholeSize;
        /// <summary>
        /// ������ȫ���ߴ�
        /// </summary>
        public float island_FillSize;
        /// <summary>
        /// ��������
        /// </summary>
        public Vector2 island_Center;
        /// <summary>
        /// �����ߴ� 1-20(��Եë��뾶)
        /// </summary>
        public float noise_Sacle;
        /// <summary>
        /// ����ƫ��(0,1)
        /// </summary>
        public Vector2 noise_Offset;
    }
    public async Task GenerateIslandArea_Large(IslandAreaConfig islandArea, Action<int, float> action)
    {
        float reciprocal_size = 1f / islandArea.island_WholeSize;
        float size_Empty = islandArea.island_WholeSize - islandArea.island_FillSize;
        int from_x = (int)(islandArea.island_Center.x - islandArea.island_WholeSize);
        int to_x = (int)(islandArea.island_Center.x + islandArea.island_WholeSize);
        int from_y = (int)(islandArea.island_Center.y - islandArea.island_WholeSize);
        int to_y = (int)(islandArea.island_Center.y + islandArea.island_WholeSize);
        Debug.Log(from_x + "/" + to_x);
        Debug.Log(from_y + "/" + to_y);

        /*����ͼ��������*/
        Vector2 noise_Center = islandArea.noise_Offset;
        /*����ͼ�����ߴ�(1-0)*/
        float noise_Scale = (1f / islandArea.noise_Sacle);
        /*�����ĵľ���*/
        float distance;
        /*��������(0-1)*/
        float perlinNoise;
        /*����ֵ(0-1)*/
        float distanceValue;
        /*ʵ������(0-1)*/
        float realNoise;
        for (int i = from_x; i < to_x; i++)
        {
            for (int j = from_y; j < to_y; j++)
            {
                int x = i;
                int y = j;
                distance = Vector2.Distance(new Vector2(x, y), islandArea.island_Center);
                if (distance > islandArea.island_FillSize)
                {
                    /*0-1*/
                    distanceValue = Mathf.Lerp(0, 1, (islandArea.island_WholeSize - distance) / size_Empty);
                    /*0-1*/
                    perlinNoise = Mathf.PerlinNoise(noise_Center.x + (x + from_x) * noise_Scale, noise_Center.y + (y + from_y) * noise_Scale);
                    realNoise = Mathf.Lerp(perlinNoise, distanceValue, Mathf.Abs(distanceValue - 0.5f) * 2);
                }
                else
                {
                    realNoise = 1;
                }
                action.Invoke(Vector2ToIndex(x, y), realNoise);
            }
            await Task.Yield();
            slider_Waiting.value = (i - from_x) * reciprocal_size;
        }
    }
    public async Task GenerateIslandArea_Tiny(IslandAreaConfig islandArea, Action<int, float, float> action)
    {
        float reciprocal_size = 1f / islandArea.island_WholeSize;
        float size_Empty = islandArea.island_WholeSize - islandArea.island_FillSize;
        int from_x = (int)(islandArea.island_Center.x - islandArea.island_WholeSize);
        int to_x = (int)(islandArea.island_Center.x + islandArea.island_WholeSize);
        int from_y = (int)(islandArea.island_Center.y - islandArea.island_WholeSize);
        int to_y = (int)(islandArea.island_Center.y + islandArea.island_WholeSize);
        Debug.Log(from_x + "/" + to_x);
        Debug.Log(from_y + "/" + to_y);

        /*����ͼ��������*/
        Vector2 noise_Center = islandArea.noise_Offset;
        /*����ͼ�����ߴ�(1-0)*/
        float noise_Scale = (1f / islandArea.noise_Sacle);
        /*�����ĵľ���*/
        float distance;
        /*��������(0-1)*/
        float perlinNoise;
        /*����ֵ(0-1)*/
        float distanceValue;
        /*ʵ������(0-1)*/
        float realNoise;
        for (int i = from_x; i < to_x; i++)
        {
            for (int j = from_y; j < to_y; j++)
            {
                int x = i;
                int y = j;
                distance = Vector2.Distance(new Vector2(x, y), islandArea.island_Center);
                perlinNoise = Mathf.PerlinNoise(noise_Center.x + (x + from_x) * noise_Scale, noise_Center.y + (y + from_y) * noise_Scale);
                if (distance > islandArea.island_FillSize)
                {
                    /*0-1*/
                    distanceValue = Mathf.Lerp(0, 1, (islandArea.island_WholeSize - distance) / size_Empty);
                    /*0-1*/
                    realNoise = Mathf.Lerp(perlinNoise, distanceValue, Mathf.Abs(distanceValue - 0.5f) * 2);
                }
                else
                {
                    realNoise = 1;
                }
                action.Invoke(Vector2ToIndex(x, y), perlinNoise, realNoise);
            }
        }
        await Task.Yield();
    }
    #endregion
    #region//���ɵ�����ȷֲ�����
    public struct PoissonPointsAreaConfig
    {
        /// <summary>
        /// ����
        /// </summary>
        public Vector2Int pointsArea_Center;
        /// <summary>
        /// �ߴ�
        /// </summary>
        public int pointsArea_Size;
        /// <summary>
        /// ������֮�����С����
        /// </summary>
        public float pointsArea_MinDistance;
        /// <summary>
        /// ����������
        /// </summary>
        public int pointsArea_MaxSamples;
    }
    public async Task GeneratePoissonPointsArea(PoissonPointsAreaConfig areaConfig, Action<int> action)
    {
        List<Vector2Int> poissonPoints = new List<Vector2Int>();
        List<Vector2Int> spawnPoints = new List<Vector2Int>();
        float cellSize = areaConfig.pointsArea_MinDistance / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(areaConfig.pointsArea_Size / cellSize), Mathf.CeilToInt(areaConfig.pointsArea_Size / cellSize)];
        int step = 0;
        int val = 0;

        spawnPoints.Add(Vector2Int.zero); // �ӵ�ͼ���Ŀ�ʼ
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            Vector2Int spawnCenter = spawnPoints[spawnIndex];
            bool validCandidateFound = false;
            for (int i = 0; i < areaConfig.pointsArea_MaxSamples; i++)
            {
                float angle = UnityEngine.Random.value * Mathf.PI * 2;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                direction = direction * UnityEngine.Random.Range(areaConfig.pointsArea_MinDistance, 2 * areaConfig.pointsArea_MinDistance);
                Vector2Int candidate = spawnCenter + new Vector2Int((int)direction.x, (int)direction.y);

                if (IsValid(candidate, new Vector2(areaConfig.pointsArea_Size, areaConfig.pointsArea_Size), cellSize, areaConfig.pointsArea_MinDistance, grid, poissonPoints))
                {
                    poissonPoints.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = poissonPoints.Count;
                    validCandidateFound = true;
                    break;
                }
            }

            if (!validCandidateFound)
            {
                spawnPoints.RemoveAt(spawnIndex); // ���û����Ч�㣬�Ƴ���ǰ���ɵ�
            }
            if (step < 200)
            {
                step += 1;
            }
            else
            {
                if (val < 100)
                {
                    val += 1;
                    slider_Waiting.value = val * 0.01f;
                }
                else
                {
                    val = 0;
                }
                await Task.Yield();
                step = 0;
            }
        }
        Vector2Int offset = new Vector2Int(areaConfig.pointsArea_Center.x - areaConfig.pointsArea_Size / 2, areaConfig.pointsArea_Center.y - areaConfig.pointsArea_Size / 2);
        for (int i = 0; i < poissonPoints.Count; i++)
        {
            action.Invoke(Vector2ToIndex(offset.x + poissonPoints[i].x, offset.y + poissonPoints[i].y));
        }
    }
    public async Task GeneratePoissonPointsArea(PoissonPointsAreaConfig areaConfig, Action<Vector2Int> action)
    {
        List<Vector2Int> poissonPoints = new List<Vector2Int>();
        List<Vector2Int> spawnPoints = new List<Vector2Int>();
        float cellSize = areaConfig.pointsArea_MinDistance / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(areaConfig.pointsArea_Size / cellSize), Mathf.CeilToInt(areaConfig.pointsArea_Size / cellSize)];
        int step = 0;
        int val = 0;
        spawnPoints.Add(Vector2Int.zero); // �ӵ�ͼ���Ŀ�ʼ
        while (spawnPoints.Count > 0)
        {
            int spawnIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            Vector2Int spawnCenter = spawnPoints[spawnIndex];
            bool validCandidateFound = false;
            for (int i = 0; i < areaConfig.pointsArea_MaxSamples; i++)
            {
                float angle = UnityEngine.Random.value * Mathf.PI * 2;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                direction = direction * UnityEngine.Random.Range(areaConfig.pointsArea_MinDistance, 2 * areaConfig.pointsArea_MinDistance);
                Vector2Int candidate = spawnCenter + new Vector2Int((int)direction.x, (int)direction.y);

                if (IsValid(candidate, new Vector2(areaConfig.pointsArea_Size, areaConfig.pointsArea_Size), cellSize, areaConfig.pointsArea_MinDistance, grid, poissonPoints))
                {
                    poissonPoints.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = poissonPoints.Count;
                    validCandidateFound = true;
                    break;
                }
            }

            if (!validCandidateFound)
            {
                spawnPoints.RemoveAt(spawnIndex); // ���û����Ч�㣬�Ƴ���ǰ���ɵ�
            }
            if (step < 200)
            {
                step += 1;
            }
            else
            {
                if (val < 100)
                {
                    val += 1;
                    slider_Waiting.value = val * 0.01f;
                }
                else
                {
                    val = 0;
                }
                await Task.Yield();
                step = 0;
            }
        }
        Vector2Int offset = new Vector2Int(areaConfig.pointsArea_Center.x - areaConfig.pointsArea_Size / 2, areaConfig.pointsArea_Center.y - areaConfig.pointsArea_Size / 2);
        for (int i = 0; i < poissonPoints.Count; i++)
        {
            action.Invoke(new Vector2Int(offset.x + poissonPoints[i].x, offset.y + poissonPoints[i].y));
        }
    }

    bool IsValid(Vector2 candidate, Vector2 regionSize, float cellSize, float radius, int[,] grid, List<Vector2Int> points)
    {
        if (candidate.x < 0 || candidate.x >= regionSize.x || candidate.y < 0 || candidate.y >= regionSize.y)
        {
            return false; // ��ѡ�㳬������Χ
        }

        int cellX = (int)(candidate.x / cellSize);
        int cellY = (int)(candidate.y / cellSize);

        int searchStartX = Mathf.Max(0, cellX - 2);
        int searchEndX = Mathf.Min(grid.GetLength(0) - 1, cellX + 2);
        int searchStartY = Mathf.Max(0, cellY - 2);
        int searchEndY = Mathf.Min(grid.GetLength(1) - 1, cellY + 2);

        for (int x = searchStartX; x <= searchEndX; x++)
        {
            for (int y = searchStartY; y <= searchEndY; y++)
            {
                int pointIndex = grid[x, y] - 1;
                if (pointIndex != -1)
                {
                    float sqrDistance = (candidate - points[pointIndex]).sqrMagnitude;
                    if (sqrDistance < radius * radius)
                    {
                        return false; // ��ѡ�������е�̫�ӽ�
                    }
                }
            }
        }

        return true; // ��ѡ����Ч
    }
    #endregion
    #region//���ֺ�����������
    public struct RiverConfig
    {
        /// <summary>
        /// �������
        /// </summary>
        public Vector2 vector2_Start;
        /// <summary>
        /// �����յ�
        /// </summary>
        public Vector2 vector2_End;
        /// <summary>
        /// ���������ߴ�
        /// </summary>
        public float river_NoiseScale;
        /// <summary>
        /// ��������ƫ��
        /// </summary>
        public Vector2 river_NoiseOffset;
        /// <summary>
        /// ��������
        /// </summary>
        public float river_Curvature;
        /// <summary>
        /// �������·���Ӱ��
        /// </summary>
        public float river_DirectionInfluence;
        /// <summary>
        /// �������
        /// </summary>
        public int river_Width;
        /// <summary>
        /// �����ֲ����1/1000
        /// </summary>
        public int river_Forking;
        /// <summary>
        /// �������ֲ����
        /// </summary>
        public int river_ForkCount;
    }
    /// <summary>
    /// ������
    /// </summary>
    /// <param name="mainRiverConfig"></param>
    /// <param name="mainRiverAction"></param>
    /// <returns></returns>
    public async Task GenerateMainRiver(RiverConfig mainRiverConfig, RiverConfig childRiverConfig, Action<int, bool> mainRiverAction, Action<int, bool> childRiverAction)
    {
        Vector2 currentPos = mainRiverConfig.vector2_Start;
        Vector2 direction = (mainRiverConfig.vector2_End - mainRiverConfig.vector2_Start).normalized;

        /*����ͼ��������*/
        Vector2 noise_Center = mainRiverConfig.river_NoiseOffset;
        /*����ͼ�����ߴ�(1-0)*/
        float noise_Scale = (1f / mainRiverConfig.river_NoiseScale);
        /*�������*/
        int width = 0;
        /*�����ֲ�*/
        int fork = 0;
        int step = 0;
        int val = 0;
        while (Vector2.Distance(currentPos, mainRiverConfig.vector2_End) > 1.0f)
        {
            if (fork < mainRiverConfig.river_ForkCount && new System.Random().Next(0, 1000) < mainRiverConfig.river_Forking)
            {
                fork++;
                Vector2 vector2_Start = currentPos;
                Vector2 vector2_End = UnityEngine.Random.insideUnitCircle.normalized * config_Map.map_Size;
                childRiverConfig.vector2_Start = vector2_Start;
                childRiverConfig.vector2_End = vector2_End;
                await GenerateChildRiver(childRiverConfig, childRiverAction);
            }
            // �������ʹ·������
            float noise = Mathf.PerlinNoise(currentPos.x * noise_Scale + noise_Center.x, currentPos.y * noise_Scale + noise_Center.y);
            float angle = (noise - 0.5f) * mainRiverConfig.river_Curvature;

            // ��ת����
            direction = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * direction;

            // ȷ�����³����յ�
            Vector2 toEnd = (mainRiverConfig.vector2_End - currentPos).normalized;
            direction = (direction * (1 - mainRiverConfig.river_DirectionInfluence) + toEnd * mainRiverConfig.river_DirectionInfluence).normalized;

            // �ƶ���ǰλ��
            currentPos += direction;
            // �������
            width = mainRiverConfig.river_Width;

            // ���ƺ�����������Χ����ʹ��������
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Vector2Int pixelPos = new Vector2Int(Mathf.RoundToInt(currentPos.x + x), Mathf.RoundToInt(currentPos.y + y));
                    if (y == 0 || y == width - 1 || x == 0 || x == width - 1)
                    {
                        mainRiverAction.Invoke(Vector2ToIndex(pixelPos.x, pixelPos.y), true);
                    }
                    else
                    {
                        mainRiverAction.Invoke(Vector2ToIndex(pixelPos.x, pixelPos.y), false);
                    }
                }
            }
            if (step < 200)
            {
                step += 1;
            }
            else 
            {
                if (val < 100) 
                {
                    val += 1;
                    slider_Waiting.value = val * 0.01f;
                }
                else
                {
                    val = 0;
                }
                await Task.Yield();
                step = 0; 
            }
        }
    }
    /// <summary>
    /// �Ӻ���
    /// </summary>
    /// <param name="riverConfig"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    private async Task GenerateChildRiver(RiverConfig riverConfig, Action<int, bool> action)
    {
        Vector2 currentPos = riverConfig.vector2_Start;
        Vector2 direction = (riverConfig.vector2_End - riverConfig.vector2_Start).normalized;
        /*����ͼ��������*/
        Vector2 noise_Center = riverConfig.river_NoiseOffset;
        /*����ͼ�����ߴ�(1-0)*/
        float noise_Scale = (1f / riverConfig.river_NoiseScale);
        /*�������*/
        int width;
        /*���д���*/
        int step = 0;
        int val = 0;
        while (Vector2.Distance(currentPos, riverConfig.vector2_End) > 1.0f)
        {
            // �������ʹ·������(0,1)
            float noise = Mathf.PerlinNoise(currentPos.x * noise_Scale + noise_Center.x, currentPos.y * noise_Scale + noise_Center.y);
            float angle = (noise - 0.5f) * riverConfig.river_Curvature;


            // ��ת����
            direction = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * direction;

            // ȷ�����³����յ�
            Vector2 toEnd = (riverConfig.vector2_End - currentPos).normalized;
            direction = (direction * (1 - riverConfig.river_DirectionInfluence) + toEnd * riverConfig.river_DirectionInfluence).normalized;

            // �ƶ���ǰλ��
            currentPos = currentPos + direction;
            // �������
            width = riverConfig.river_Width;
            // ���ƺ�����������Χ����ʹ��������
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Vector2Int pixelPos = new Vector2Int(Mathf.RoundToInt(currentPos.x + x), Mathf.RoundToInt(currentPos.y + y));
                    if (y == 0 || y == width - 1 || x == 0 || x == width - 1)
                    {
                        action.Invoke(Vector2ToIndex(pixelPos.x, pixelPos.y), true);
                    }
                    else
                    {
                        action.Invoke(Vector2ToIndex(pixelPos.x, pixelPos.y), false);
                    }
                }
            }
            if (step < 200)
            {
                step += 1;
            }
            else
            {
                if (val < 100)
                {
                    val += 1;
                    slider_Waiting.value = val * 0.01f;
                }
                else
                {
                    val = 0;
                }
                await Task.Yield();
                step = 0;
            }
        }
    }
    #endregion
    #region//�������ֲ�����(����/������)
    public struct RandomPointsAreaConfig
    {
        /// <summary>
        /// ����
        /// </summary>
        public Vector2Int pointsArea_Center;
        /// <summary>
        /// ������
        /// </summary>
        public int pointsArea_Count;
        /// <summary>
        /// ����ߴ�
        /// </summary>
        public int pointsArea_SizeWhole;
        /// <summary>
        /// ���ߴ�
        /// </summary>
        public int pointsArea_SizeFill;
    }
    public async Task GenerateRandomPointsArea_Gradual(RandomPointsAreaConfig areaConfig, Action<int> action)
    {
        float reciprocal_size = 1f / areaConfig.pointsArea_SizeWhole;
        float reciprocal_Count = 1f / areaConfig.pointsArea_Count;
        float size_Empty = areaConfig.pointsArea_SizeWhole - areaConfig.pointsArea_SizeFill;

        int from_x = (areaConfig.pointsArea_Center.x - areaConfig.pointsArea_SizeWhole);
        int to_x = (areaConfig.pointsArea_Center.x + areaConfig.pointsArea_SizeWhole);
        int from_y = (areaConfig.pointsArea_Center.y - areaConfig.pointsArea_SizeWhole);
        int to_y = (areaConfig.pointsArea_Center.y + areaConfig.pointsArea_SizeWhole);
        /*�����ĵľ���*/
        float distance;
        int step = 0;
        int val = 0;
        for (int i = 0; i < areaConfig.pointsArea_Count; i++)
        {
            // �������λ��
            int randomX = new System.Random().Next(from_x, to_x);
            int randomY = new System.Random().Next(from_y, to_y);
            distance = Vector2.Distance(new Vector2(randomX, randomY), areaConfig.pointsArea_Center);
            if (distance > areaConfig.pointsArea_SizeFill)
            {
                float distanceLerp = Mathf.Lerp(0, 1, (areaConfig.pointsArea_SizeWhole - distance) / size_Empty);
                if (new System.Random().Next(0, 100) < distanceLerp * 100)
                {
                    action.Invoke(Vector2ToIndex(randomX, randomY));
                }
            }
            else
            {
                action.Invoke(Vector2ToIndex(randomX, randomY));
            }
            if (step < 500)
            {
                step += 1;
            }
            else
            {
                if (val < 100)
                {
                    val += 1;
                    slider_Waiting.value = (i * reciprocal_Count);
                }
                else
                {
                    val = 0;
                }
                await Task.Yield();
                step = 0;
            }
        }
    }
    public async Task GenerateRandomPointsArea_Hard(RandomPointsAreaConfig areaConfig, Action<int> action)
    {
        float reciprocal_Count = 1f / areaConfig.pointsArea_Count;

        int from_x = (areaConfig.pointsArea_Center.x - areaConfig.pointsArea_SizeWhole);
        int to_x = (areaConfig.pointsArea_Center.x + areaConfig.pointsArea_SizeWhole);
        int from_y = (areaConfig.pointsArea_Center.y - areaConfig.pointsArea_SizeWhole);
        int to_y = (areaConfig.pointsArea_Center.y + areaConfig.pointsArea_SizeWhole);
        int step = 0;
        int val = 0;
        for (int i = 0; i < areaConfig.pointsArea_Count; i++)
        {
            // �������λ��
            int randomX = new System.Random().Next(from_x, to_x);
            int randomY = new System.Random().Next(from_y, to_y);
            action.Invoke(Vector2ToIndex(randomX, randomY)); 
            if (step < 100)
            {
                step += 1;
            }
            else
            {
                if (val < 100)
                {
                    val += 1;
                    slider_Waiting.value = (i * reciprocal_Count);
                }
                else
                {
                    val = 0;
                }
                await Task.Yield();
                step = 0;
            }
        }
    }
    #endregion
    #region//������ŵ�����ֲ�����(������)
    public struct RandomExpandPointsAreaConfig
    {
        /// <summary>
        /// ����
        /// </summary>
        public Vector2Int pointsArea_Center;
        /// <summary>
        /// ������
        /// </summary>
        public int pointsArea_Count;
        /// <summary>
        /// ����������
        /// </summary>
        public int pointsArea_Expand;
        /// <summary>
        /// ����ߴ�
        /// </summary>
        public int pointsArea_SizeWhole;
    }
    public async Task GenerateRandomExpandPointsArea(RandomExpandPointsAreaConfig areaConfig, Action<int> action)
    {
        float reciprocal_Count = 1f / areaConfig.pointsArea_Count;

        int from_x = (areaConfig.pointsArea_Center.x - areaConfig.pointsArea_SizeWhole);
        int to_x = (areaConfig.pointsArea_Center.x + areaConfig.pointsArea_SizeWhole);
        int from_y = (areaConfig.pointsArea_Center.y - areaConfig.pointsArea_SizeWhole);
        int to_y = (areaConfig.pointsArea_Center.y + areaConfig.pointsArea_SizeWhole);
        int step = 0;
        int val = 0;
        for (int i = 0; i < areaConfig.pointsArea_Count; i++)
        {
            // �������λ��
            int randomX = new System.Random().Next(from_x, to_x);
            int randomY = new System.Random().Next(from_y, to_y);
            action.Invoke(Vector2ToIndex(randomX, randomY));
            //����
            int tempX = 0;
            int tempY = 0;
            int index = 0;
            int random = new System.Random().Next(0, 100);
            while (random < areaConfig.pointsArea_Expand && index < 10)
            {
                index++;
                tempX += new System.Random().Next(-1, 2);
                tempY += new System.Random().Next(-1, 2);
                action.Invoke(Vector2ToIndex(randomX + tempX, randomY + tempY));
                random = new System.Random().Next(0, 100);
            }

            if (step < 200)
            {
                step += 1;
            }
            else
            {
                if (val < 100)
                {
                    val += 1;
                    slider_Waiting.value = (i * reciprocal_Count);
                }
                else
                {
                    val = 0;
                }
                await Task.Yield();
                step = 0;
            }
        }
    }

    #endregion
    public int Vector2ToIndex(int x,int y)
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
}
/// <summary>
/// ��½������
/// </summary>
public class MapCreate_Land
{
    /// <summary>
    /// ��½���������
    /// </summary>
    public float land_EdgeWidth = 50f;
    /// <summary>
    /// ��½�߽������뾶
    /// </summary>
    public float land_NoiseSacle = 10;
    /// <summary>
    /// ��½�߽纣��Ȩ��
    /// </summary>
    public float land_OceanWeight = 0.5f;
    /// <summary>
    /// ��½�߽�ɳ̲Ȩ��
    /// </summary>
    public float land_BeachWeight = 0.2f;

    public async Task CreateIslandArea(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "�������ɴ�½";
        MapCreater.IslandAreaConfig config;
        config = new MapCreater.IslandAreaConfig()
        {
            island_Center = Vector2.zero,
            island_WholeSize = mapCreater.config_Map.map_Size,
            island_FillSize = mapCreater.config_Map.map_Size - land_EdgeWidth,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = land_NoiseSacle,
        };
        await mapCreater.GenerateIslandArea_Large(config, (index, val) =>
        {
            if (val < land_OceanWeight)
            {
                if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 9000;
                }
                else
                {
                    mapCreater.data_mapGroundData.tileDic.Add(index, 9000);
                }
            }
            else if (val < land_OceanWeight + land_BeachWeight)
            {
                if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1000;
                }
                else
                {
                    mapCreater.data_mapGroundData.tileDic.Add(index, 1000);
                }
            }
            else
            {
                if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1001;
                }
                else
                {
                    mapCreater.data_mapGroundData.tileDic.Add(index, 1001);
                }
            }
        });
    }
} 
/// <summary>
/// ѩԭ������
/// </summary>
public class MapCreate_Snowland
{
    /// <summary>
    /// ѩԭ�������
    /// </summary>
    private Vector2 snowland_Center = new Vector2(-0.5f, 0.5f);
    /// <summary>
    /// ѩԭ�������ߴ�
    /// </summary>
    private float snowland_WholeSize = 0.33f;
    /// <summary>
    /// ѩԭ�߽���
    /// </summary>
    private float snowland_Width = 50f;
    /// <summary>
    /// ѩԭ�����뾶
    /// </summary>
    private float snowland_NoiseSacle = 10;
    /// <summary>
    /// ѩԭѩ��Ȩ��
    /// </summary>
    private float snowland_SnowlandWeight = 0.3f;
    /// <summary>
    /// ѩԭ̦ԭȨ��
    /// </summary>
    private float snowland_TundraWeight = 0.2f;
    /// <summary>
    /// ����ѩԭ
    /// </summary>
    /// <returns></returns>
    public async Task CreateSnowland(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "��������ѩԭ";
        MapCreater.IslandAreaConfig config;
        config = new MapCreater.IslandAreaConfig()
        {
            island_Center = snowland_Center * mapCreater.config_Map.map_Size,
            island_WholeSize = mapCreater.config_Map.map_Size * snowland_WholeSize,
            island_FillSize = mapCreater.config_Map.map_Size * snowland_WholeSize - snowland_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = snowland_NoiseSacle,
        };
        await mapCreater.GenerateIslandArea_Large(config, (index, val) =>
        {
            /*������ؿ��Ҳ�Ϊ����*/
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] != 9000)
            {
                if (val > (1 - snowland_SnowlandWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1004;
                }
                else if (val > (1 - snowland_SnowlandWeight - snowland_TundraWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1003;
                }
                else
                {

                }
            }
        });

    }

}
/// <summary>
/// ɳĮ������
/// </summary>
public class MapCreate_Desert
{
    /// <summary>
    /// ɳĮ�������
    /// </summary>
    private Vector2 desert_Center = new Vector2(0.5f, -0.5f);
    /// <summary>
    /// ɳĮ�������ߴ�
    /// </summary>
    private float desert_WholeSize = 0.33f;
    /// <summary>
    /// ɳĮ�߽���
    /// </summary>
    private float desert_Width = 50f;
    /// <summary>
    /// ɳĮ�����뾶
    /// </summary>
    private float desert_NoiseSacle = 10;
    /// <summary>
    /// ɳĮȨ��
    /// </summary>
    private float desert_SandWeight = 0.3f;
    /// <summary>
    /// �ĵ�Ȩ��
    /// </summary>
    private float desert_GroundWeight = 0.2f;
    /// <summary>
    /// ����ѩԭ
    /// </summary>
    /// <returns></returns>
    public async Task CreateDesert(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "��������ɳĮ";
        MapCreater.IslandAreaConfig config;
        config = new MapCreater.IslandAreaConfig()
        {
            island_Center = desert_Center * mapCreater.config_Map.map_Size,
            island_WholeSize = mapCreater.config_Map.map_Size * desert_WholeSize,
            island_FillSize = mapCreater.config_Map.map_Size * desert_WholeSize - desert_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = desert_NoiseSacle,
        };
        await mapCreater.GenerateIslandArea_Large(config, (index, val) =>
        {
            /*������ؿ��Ҳ�Ϊ����*/
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] != 9000)
            {
                if (val > (1 - desert_SandWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1005;
                }
                else if (val > (1 - desert_SandWeight - desert_GroundWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1000;
                }
                else
                {

                }
            }
        });
    }
}
/// <summary>
/// ��½��������
/// </summary>
public class MapCreate_InlandSea
{
    /// <summary>
    /// ��½���������
    /// </summary>
    private Vector2 inlandSea_Center = new Vector2(0.5f, 0.5f);
    /// <summary>
    /// ��½���������ߴ�
    /// </summary>
    private float inlandSea_WholeSize = 0.2f;
    /// <summary>
    /// ��½���߽���
    /// </summary>
    private float inlandSea_Width = 20f;
    /// <summary>
    /// ��½�������뾶
    /// </summary>
    private float inlandSea_NoiseSacle = 10;
    /// <summary>
    /// ��½������Ȩ��
    /// </summary>
    private float inlandSea_SnowlandWeight = 0.3f;
    /// <summary>
    /// ��½��ɳ̲Ȩ��
    /// </summary>
    private float inlandSea_TundraWeight = 0.2f;
    /// <summary>
    /// ������½��
    /// </summary>
    /// <returns></returns>
    public async Task CreateInlandSea(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "����������½��";
        MapCreater.IslandAreaConfig config;
        config = new MapCreater.IslandAreaConfig()
        {
            island_Center = inlandSea_Center * mapCreater.config_Map.map_Size,
            island_WholeSize = mapCreater.config_Map.map_Size * inlandSea_WholeSize,
            island_FillSize = mapCreater.config_Map.map_Size * inlandSea_WholeSize - inlandSea_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = inlandSea_NoiseSacle,
        };
        await mapCreater.GenerateIslandArea_Large(config, (index, val) =>
        {
            /*������ؿ��Ҳ�Ϊ����*/
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] != 9000)
            {
                if (val > (1 - inlandSea_SnowlandWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 9000;
                }
                else if (val > (1 - inlandSea_SnowlandWeight - inlandSea_TundraWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1005;
                }
                else
                {

                }
            }
        });

    }

    /// <summary>
    /// Ⱥ���������
    /// </summary>
    private Vector2 islet_Center = new Vector2(0.5f, 0.5f);
    /// <summary>
    /// Ⱥ���������ߴ�
    /// </summary>
    private float islet_WholeSize = 0.1f;
    /// <summary>
    /// ��½���߽���
    /// </summary>
    private float islet_Width = 20f;
    /// <summary>
    /// Ⱥ�������뾶
    /// </summary>
    private float islet_NoiseSacle = 20;
    /// <summary>
    /// Ⱥ����½Ȩ��
    /// </summary>
    private float islet_LandWeight = 0.3f;
    /// <summary>
    /// Ⱥ��ɳ̲Ȩ��
    /// </summary>
    private float islet_SandWeight = 0.2f;
    /// <summary>
    /// ����С��
    /// </summary>
    /// <returns></returns>
    public async Task CreateIslet(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "������������С��";
        MapCreater.IslandAreaConfig config;
        config = new MapCreater.IslandAreaConfig()
        {
            island_Center = islet_Center * mapCreater.config_Map.map_Size,
            island_WholeSize = mapCreater.config_Map.map_Size * islet_WholeSize,
            island_FillSize = mapCreater.config_Map.map_Size * islet_WholeSize - islet_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = islet_NoiseSacle,
        };
        await mapCreater.GenerateIslandArea_Large(config, (index, val) =>
        {
            if (val > (1 - islet_LandWeight))
            {
                mapCreater.data_mapGroundData.tileDic[index] = 1000;
            }
            else if (val > (1 - islet_LandWeight - islet_SandWeight))
            {
                mapCreater.data_mapGroundData.tileDic[index] = 1005;
            }
            else
            {

            }
        });

    }

}
/// <summary>
/// ����������
/// </summary>
public class MapCreate_River
{
    MapCreater creater;
    public async Task CreateRiver(MapCreater mapCreater)
    {
        creater = mapCreater;
        creater.text_Waiting.text = "�������ɺ���";

        MapCreater.RiverConfig config_River0;
        config_River0 = new MapCreater.RiverConfig()
        {
            vector2_Start = new Vector2(-creater.config_Map.map_Size, 0),
            vector2_End = new Vector2(creater.config_Map.map_Size, creater.config_Map.map_Size),
            river_NoiseScale = 20f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 3,
            river_DirectionInfluence = 0.25f,
            river_Width = 6,
            river_Forking = 3,
            river_ForkCount = 4
        };
        MapCreater.RiverConfig config_River1;
        config_River1 = new MapCreater.RiverConfig()
        {
            vector2_Start = new Vector2(creater.config_Map.map_Size, -creater.config_Map.map_Size),
            vector2_End = new Vector2(-creater.config_Map.map_Size, 0),
            river_NoiseScale = 20f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 3,
            river_DirectionInfluence = 0.25f,
            river_Width = 6,
            river_Forking = 3,
            river_ForkCount = 4
        };
        MapCreater.RiverConfig config_River2;
        config_River2 = new MapCreater.RiverConfig()
        {
            vector2_Start = new Vector2(0, 0),
            vector2_End = new Vector2(0, -creater.config_Map.map_Size),
            river_NoiseScale = 20f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 3,
            river_DirectionInfluence = 0.25f,
            river_Width = 6,
            river_Forking = 3,
            river_ForkCount = 4
        };


        MapCreater.RiverConfig config_ChildRiver;
        config_ChildRiver = new MapCreater.RiverConfig()
        {
            river_NoiseScale = 25f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 5,
            river_DirectionInfluence = 0.25f,
            river_Width = 3,
        };

        await mapCreater.GenerateMainRiver(config_River0, config_ChildRiver, DrawMainRiver, DrawChildRiver);
        await mapCreater.GenerateMainRiver(config_River1, config_ChildRiver, DrawMainRiver, DrawChildRiver);
        await mapCreater.GenerateMainRiver(config_River2, config_ChildRiver, DrawMainRiver, DrawChildRiver);
    }
    private void DrawMainRiver(int index, bool edge)
    {
        if (edge)
        {
            if (creater.data_mapGroundData.tileDic.ContainsKey(index) && creater.data_mapGroundData.tileDic[index] != 9000)
            {
                creater.data_mapGroundData.tileDic[index] = 1000;
            }
        }
        else
        {
            if (creater.data_mapGroundData.tileDic.ContainsKey(index) && creater.data_mapGroundData.tileDic[index] != 9000)
            {
                creater.data_mapGroundData.tileDic[index] = 9000;
            }
        }
    }
    private void DrawChildRiver(int index, bool edge)
    {
        if (creater.data_mapGroundData.tileDic.ContainsKey(index) && creater.data_mapGroundData.tileDic[index] != 9000)
        {
            creater.data_mapGroundData.tileDic[index] = 9000;
        }
    }

}
/// <summary>
/// ����������
/// </summary>
public class MapCreate_Lake
{
    /// <summary>
    /// �����ܶ�(ƽ����/��)
    /// </summary>
    private int lake_Density = 5000;
    /// <summary>
    /// ������С�뾶
    /// </summary>
    private int lake_MinSize = 10;
    /// <summary>
    /// �������뾶
    /// </summary>
    private int lake_MaxSize = 20;
    /// <summary>
    /// ����Ȩ��
    /// </summary>
    private float lake_Weight = 0.5f;
    /// <summary>
    /// ������ԵȨ��
    /// </summary>
    private float lake_EdgeWeight = 0.2f;
    /// <summary>
    /// ���ɺ���
    /// </summary>
    /// <returns></returns>
    public async Task CreateLake(MapCreater mapCreater)
    {
        int lake_Count = (mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2) / lake_Density;
        for (int i = 0; i < lake_Count; i++)
        {
            mapCreater.text_Waiting.text = "�������ɺ���" + i;
            MapCreater.IslandAreaConfig config;
            float lakeSize = (float)new System.Random().Next(lake_MinSize, lake_MaxSize);
            config = new MapCreater.IslandAreaConfig()
            {
                island_Center = new Vector2(new System.Random().Next(-mapCreater.config_Map.map_Size + 20, mapCreater.config_Map.map_Size - 20), new System.Random().Next(-mapCreater.config_Map.map_Size + 20, mapCreater.config_Map.map_Size - 20)),
                island_WholeSize = lakeSize,
                island_FillSize = lakeSize * 0.5f,
                noise_Offset = mapCreater.GetRandomOffset(),
                noise_Sacle = 5,
            };
            await mapCreater.GenerateIslandArea_Tiny(config, (index, perlinNoise, realNoise) =>
            {
                if (realNoise > (1 - lake_Weight))
                {
                    if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index))
                    {
                        mapCreater.data_mapGroundData.tileDic[index] = 9000;
                    }
                    else
                    {
                        mapCreater.data_mapGroundData.tileDic.Add(index, 9000);
                    }
                }
                if (realNoise > (1 - lake_Weight - lake_EdgeWeight))
                {
                    if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index))
                    {
                        if (mapCreater.data_mapGroundData.tileDic[index] != 9000)
                        {
                            mapCreater.data_mapGroundData.tileDic[index] = 1000;
                        }
                    }
                    else
                    {
                        mapCreater.data_mapGroundData.tileDic.Add(index, 1000);
                    }
                }
            });
            mapCreater.slider_Waiting.value = (float)i / (float)lake_Count;
        }
    }

}
/// <summary>
/// ɭ��������
/// </summary>
public class MapCreate_Forest
{
    /// <summary>
    /// ɭ���ܶ�(ƽ����/��)
    /// </summary>
    private int forest_Density = 10000;
    /// <summary>
    /// ɭ����С���
    /// </summary>
    private int forest_MinSize = 20;
    /// <summary>
    /// ɭ��������
    /// </summary>
    private int forest_MaxSize = 50;
    /// <summary>
    /// �ݵ���ľ�ܶ�(ƽ����/��)
    /// </summary>
    private float forest_Compactness1001 = 10f;
    /// <summary>
    /// �ݵ���ľ����
    /// </summary>
    private List<short> treeIDs_Ground1001 = new List<short>() { 1000, 1003 };
    /// <summary>
    /// ѩ����ľ�ܶ�(ƽ����/��)
    /// </summary>
    private float forest_Compactness1004 = 20f;
    /// <summary>
    /// ̦ԭ����ľ����
    /// </summary>
    private List<short> treeIDs_Ground1003 = new List<short>() { 1018 };
    /// <summary>
    /// ѩ����ľ����
    /// </summary>
    private List<short> treeIDs_Ground1004 = new List<short>() { 1018 };
    /// <summary>
    /// ɳĮ��ľ�ܶ�(ƽ����/��)
    /// </summary>
    private float forest_Compactness1005 = 20f;
    /// <summary>
    /// ɳĮ��ľ����
    /// </summary>
    private List<short> treeIDs_Ground1005 = new List<short>() { 1019 };

    MapCreater bindMapCreater;

    /// <summary>
    /// ������ľ
    /// </summary>
    /// <returns></returns>
    public async Task CreateForest(MapCreater mapCreater)
    {
        bindMapCreater = mapCreater;
        int forest_Count = (bindMapCreater.config_Map.map_Size * 2) * (bindMapCreater.config_Map.map_Size * 2) / forest_Density;
        bindMapCreater.text_Waiting.text = "������������" + forest_Count;

        for (int i = 0; i < forest_Count; i++)
        {
            UnityEngine.Random.InitState((int)(bindMapCreater.GetRandomOffset().x * 10000));
            Vector2Int center = new Vector2Int(new System.Random().Next(-bindMapCreater.config_Map.map_Size, bindMapCreater.config_Map.map_Size), new System.Random().Next(-bindMapCreater.config_Map.map_Size, bindMapCreater.config_Map.map_Size));
            int index = bindMapCreater.Vector2ToIndex(center.x, center.y);
            if (mapCreater.data_mapGroundData.tileDic[index] == 1000 || mapCreater.data_mapGroundData.tileDic[index] == 1001)
            {
                await CreateForestOn1001(center);
            }
            else if (mapCreater.data_mapGroundData.tileDic[index] == 1003 || mapCreater.data_mapGroundData.tileDic[index] == 1004)
            {
                await CreateForestOn1004(center);
            }
            else if (mapCreater.data_mapGroundData.tileDic[index] == 1005)
            {
                await CreateForestOn1005(center);
            }
            else
            {
                await CreateForestOnOther(center);
            }
        }
    }
    public async Task CreateForestOn1001(Vector2Int center)
    {
        int randomVal = UnityEngine.Random.Range(0, 1000);
        int forest_Size = UnityEngine.Random.Range(forest_MinSize, forest_MaxSize);
        MapCreater.RandomPointsAreaConfig config = new MapCreater.RandomPointsAreaConfig()
        {
            pointsArea_Center = center,
            pointsArea_Count = (int)(forest_Size * forest_Size / forest_Compactness1001),
            pointsArea_SizeWhole = forest_Size,
            pointsArea_SizeFill = forest_Size - 10,
        };
        await bindMapCreater.GenerateRandomPointsArea_Gradual(config, (index) =>
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index))
            {
                if (bindMapCreater.data_mapGroundData.tileDic[index] == 1001)
                {
                    short treeID = treeIDs_Ground1001[new System.Random().Next(0, treeIDs_Ground1001.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1003)
                {
                    short treeID = treeIDs_Ground1003[new System.Random().Next(0, treeIDs_Ground1003.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1004)
                {
                    short treeID = treeIDs_Ground1004[new System.Random().Next(0, treeIDs_Ground1004.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1005)
                {
                    short treeID = treeIDs_Ground1005[new System.Random().Next(0, treeIDs_Ground1005.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
            }
        });
    }
    public async Task CreateForestOn1004(Vector2Int center)
    {
        int randomVal = UnityEngine.Random.Range(0, 1000);
        int forest_Size = UnityEngine.Random.Range(forest_MinSize, forest_MaxSize);
        MapCreater.RandomPointsAreaConfig config = new MapCreater.RandomPointsAreaConfig()
        {
            pointsArea_Center = center,
            pointsArea_Count = (int)(forest_Size * forest_Size / forest_Compactness1004),
            pointsArea_SizeWhole = forest_Size,
            pointsArea_SizeFill = forest_Size - 10,
        };
        await bindMapCreater.GenerateRandomPointsArea_Gradual(config, (index) =>
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index))
            {
                if (bindMapCreater.data_mapGroundData.tileDic[index] == 1001)
                {
                    short treeID = treeIDs_Ground1001[new System.Random().Next(0, treeIDs_Ground1001.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1003)
                {
                    short treeID = treeIDs_Ground1003[new System.Random().Next(0, treeIDs_Ground1003.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1004)
                {
                    short treeID = treeIDs_Ground1004[new System.Random().Next(0, treeIDs_Ground1004.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1005)
                {
                    short treeID = treeIDs_Ground1005[new System.Random().Next(0, treeIDs_Ground1005.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
            }
        });
    }
    public async Task CreateForestOn1005(Vector2Int center)
    {
        int randomVal = UnityEngine.Random.Range(0, 1000);
        int forest_Size = UnityEngine.Random.Range(forest_MinSize, forest_MaxSize);
        MapCreater.RandomPointsAreaConfig config = new MapCreater.RandomPointsAreaConfig()
        {
            pointsArea_Center = center,
            pointsArea_Count = (int)(forest_Size * forest_Size / forest_Compactness1005),
            pointsArea_SizeWhole = forest_Size,
            pointsArea_SizeFill = forest_Size - 10,
        };
        await bindMapCreater.GenerateRandomPointsArea_Gradual(config, (index) =>
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index))
            {
                if (bindMapCreater.data_mapGroundData.tileDic[index] == 1001)
                {
                    short treeID = treeIDs_Ground1001[new System.Random().Next(0, treeIDs_Ground1001.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1003)
                {
                    short treeID = treeIDs_Ground1003[new System.Random().Next(0, treeIDs_Ground1003.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1004)
                {
                    short treeID = treeIDs_Ground1004[new System.Random().Next(0, treeIDs_Ground1004.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1005)
                {
                    short treeID = treeIDs_Ground1005[new System.Random().Next(0, treeIDs_Ground1005.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
            }
        });
    }
    public async Task CreateForestOnOther(Vector2Int center)
    {
        int randomVal = UnityEngine.Random.Range(0, 1000);
        int forest_Size = UnityEngine.Random.Range(forest_MinSize, forest_MaxSize);
        MapCreater.RandomPointsAreaConfig config = new MapCreater.RandomPointsAreaConfig()
        {
            pointsArea_Center = center,
            pointsArea_Count = (int)(forest_Size * forest_Size / forest_Compactness1001),
            pointsArea_SizeWhole = forest_Size,
            pointsArea_SizeFill = forest_Size - 10,
        };
        await bindMapCreater.GenerateRandomPointsArea_Gradual(config, (index) =>
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index))
            {
                if (bindMapCreater.data_mapGroundData.tileDic[index] == 1001)
                {
                    short treeID = treeIDs_Ground1001[new System.Random().Next(0, treeIDs_Ground1001.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1003)
                {
                    short treeID = treeIDs_Ground1003[new System.Random().Next(0, treeIDs_Ground1003.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1004)
                {
                    short treeID = treeIDs_Ground1004[new System.Random().Next(0, treeIDs_Ground1004.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
                else if (bindMapCreater.data_mapGroundData.tileDic[index] == 1005)
                {
                    short treeID = treeIDs_Ground1005[new System.Random().Next(0, treeIDs_Ground1005.Count)];
                    if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        bindMapCreater.data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        bindMapCreater.data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
            }
        });
    }
}
/// <summary>
/// ����������
/// </summary>
public class MapCreate_Mining
{
    MapCreater bindMapCreater;
    /// <summary>
    /// �����ܶ�(ƽ����/��)
    /// </summary>
    private int mining_Density = 3500;
    /// <summary>
    /// ������С�ߴ�
    /// </summary>
    private int mining_MinSize = 15;
    /// <summary>
    /// �������ߴ�
    /// </summary>
    private int mining_MaxSize = 25;
    /// <summary>
    /// ���ɿ���
    /// </summary>
    /// <returns></returns>
    public async Task CreateMining(MapCreater mapCreater)
    {
        bindMapCreater = mapCreater;
        int mining_Count = (bindMapCreater.config_Map.map_Size * 2) * (bindMapCreater.config_Map.map_Size * 2) / mining_Density;
        bindMapCreater.text_Waiting.text = "�������ɿ���" + mining_Count;

        for (int i = 0; i < mining_Count; i++)
        {
            UnityEngine.Random.InitState((int)(bindMapCreater.GetRandomOffset().x * 10000));
            int randomVal = UnityEngine.Random.Range(0, 1000);
            int mining_Size = UnityEngine.Random.Range(mining_MinSize, mining_MaxSize);
            Vector2 center = new Vector2(new System.Random().Next(-bindMapCreater.config_Map.map_Size + 20, bindMapCreater.config_Map.map_Size - 20), new System.Random().Next(-bindMapCreater.config_Map.map_Size + 20, bindMapCreater.config_Map.map_Size - 20));
            MapCreater.IslandAreaConfig config = new MapCreater.IslandAreaConfig()
            {
                island_Center = center,
                island_WholeSize = mining_Size,
                island_FillSize = 5,
                noise_Offset = bindMapCreater.GetRandomOffset(),
                noise_Sacle = 5,
            };
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(bindMapCreater.Vector2ToIndex((int)(center.x), (int)(center.y))))
            {
                int centerGroundId = bindMapCreater.data_mapGroundData.tileDic[bindMapCreater.Vector2ToIndex((int)(center.x), (int)(center.y))];
                if (centerGroundId == 1000)
                {
                    /*�ĵ�*/
                    //await mapCreater.GenerateIslandArea_Tiny(config, CreateRockMining);
                }
                else if (centerGroundId == 1001)
                {
                    /*�ݵ�*/
                    await mapCreater.GenerateIslandArea_Tiny(config, CreateCoalMining);
                }
                else if (centerGroundId == 1004 || centerGroundId == 1003)
                {
                    /*̦ԭ����ѩ��*/
                    await mapCreater.GenerateIslandArea_Tiny(config, CreateIronMining);
                }
                else if (centerGroundId == 1005)
                {
                    /*ɳĮ*/
                    await mapCreater.GenerateIslandArea_Tiny(config, CreateGoldMining);
                }
            }
            mapCreater.slider_Waiting.value = (float)i / (float)mining_Count;
        }
    }

    /// <summary>
    /// ʯɽȨ��
    /// </summary>
    float rock_Weight = 0.3f;
    float rockEdeg_Weight = 0.2f;
    /// <summary>
    /// ����ʯɽ
    /// </summary>
    public void CreateRockMining(int index,float perlinNoise, float realNoise)
    {
        if (realNoise > (1 - rock_Weight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
                if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = 1002;
                }
                else
                {
                    bindMapCreater.data_mapBuildingData.tileDic.Add(index, 1002);
                }
            }
        }
        else if (realNoise > (1 - coalMining_Weight - rock_Weight - rockEdeg_Weight))
        {
            bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
        }
    }


    /// <summary>
    /// ú��Ȩ��
    /// </summary>
    float coalMining_Weight = 0.4f;
    /// <summary>
    /// ú��ɽȨ��
    /// </summary>
    float coalRock_Weight = 0.3f;
    /// <summary>
    /// ú��ɽ�߽�Ȩ��
    /// </summary>
    float coalRockEdeg_Weight = 0.2f;
    /// <summary>
    /// ����ú��
    /// </summary>
    public void CreateCoalMining(int index, float perlinNoise, float realNoise)
    {
        if (realNoise > (1 - coalRock_Weight) && perlinNoise > (1 - coalMining_Weight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
                if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = 1011;
                }
                else
                {
                    bindMapCreater.data_mapBuildingData.tileDic.Add(index, 1011);
                }
            }
        }
        else if (realNoise > (1 - coalRock_Weight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
                if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = 1002;
                }
                else
                {
                    bindMapCreater.data_mapBuildingData.tileDic.Add(index, 1002);
                }
            }
        }
        else if (realNoise > (1 - coalRock_Weight - coalRockEdeg_Weight))
        {
            bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
        }
    }
    /// <summary>
    /// ����Ȩ��
    /// </summary>
    float ironMining_Weight = 0.4f;
    /// <summary>
    /// ����ɽȨ��
    /// </summary>
    float ironRock_Weight = 0.3f;
    /// <summary>
    /// ����ɽ�߽�Ȩ��
    /// </summary>
    float ironRockEdeg_Weight = 0.2f;
    /// <summary>
    /// ��������
    /// </summary>
    public void CreateIronMining(int index, float perlinNoise, float realNoise)
    {
        if (realNoise > (1 - ironRock_Weight) && perlinNoise > (1 - ironMining_Weight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
                if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = 1012;
                }
                else
                {
                    bindMapCreater.data_mapBuildingData.tileDic.Add(index, 1012);
                }
            }
        }
        else if (realNoise > (1 - ironRock_Weight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
                if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = 1002;
                }
                else
                {
                    bindMapCreater.data_mapBuildingData.tileDic.Add(index, 1002);
                }
            }
        }
        else if (realNoise > (1 - ironRock_Weight - ironRockEdeg_Weight))
        {
            bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
        }
    }

    /// <summary>
    /// ���Ȩ��
    /// </summary>
    float goldMining_Weight = 0.4f;
    /// <summary>
    /// ���ɽȨ��
    /// </summary>
    float goldRock_Weight = 0.3f;
    /// <summary>
    /// ���ɽ�߽�Ȩ��
    /// </summary>
    float goldRockEdeg_Weight = 0.2f;
    /// <summary>
    /// ���ɽ��
    /// </summary>
    /// <param name="index"></param>
    /// <param name="val"></param>
    public void CreateGoldMining(int index, float perlinNoise, float realNoise)
    {
        if (realNoise > (1 - goldRock_Weight) && perlinNoise > (1 - goldMining_Weight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
                if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = 1013;
                }
                else
                {
                    bindMapCreater.data_mapBuildingData.tileDic.Add(index, 1013);
                }
            }
        }
        else if (realNoise > (1 - goldRock_Weight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
                if (bindMapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = 1002;
                }
                else
                {
                    bindMapCreater.data_mapBuildingData.tileDic.Add(index, 1002);
                }
            }
        }
        else if (realNoise > (1 - goldRock_Weight - goldRockEdeg_Weight))
        {
            bindMapCreater.data_mapGroundData.tileDic[index] = 1000;
        }


    }

}
/// <summary>
/// ��·������
/// </summary>
public class MapCreate_Road
{
    MapCreater creater;
    public async Task CreateRoad(MapCreater mapCreater)
    {
        creater = mapCreater;
        creater.text_Waiting.text = "�������ɵ�·";

        MapCreater.RiverConfig config_River0;
        config_River0 = new MapCreater.RiverConfig()
        {
            vector2_Start = new Vector2(0, -creater.config_Map.map_Size),
            vector2_End = new Vector2(-creater.config_Map.map_Size, creater.config_Map.map_Size),
            river_NoiseScale = 25f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1,
            river_DirectionInfluence = 0.25f,
            river_Width = 3,
            river_Forking = 3,
            river_ForkCount = 4
        };
        MapCreater.RiverConfig config_River1;
        config_River1 = new MapCreater.RiverConfig()
        {
            vector2_Start = new Vector2(creater.config_Map.map_Size, 0),
            vector2_End = new Vector2(-creater.config_Map.map_Size, -creater.config_Map.map_Size),
            river_NoiseScale = 25f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1,
            river_DirectionInfluence = 0.25f,
            river_Width = 3,
            river_Forking = 3,
            river_ForkCount = 4
        };
        MapCreater.RiverConfig config_River2;
        config_River2 = new MapCreater.RiverConfig()
        {
            vector2_Start = new Vector2(0, creater.config_Map.map_Size),
            vector2_End = new Vector2(0, -creater.config_Map.map_Size),
            river_NoiseScale = 25f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1,
            river_DirectionInfluence = 0.25f,
            river_Width = 3,
            river_Forking = 3,
            river_ForkCount = 4
        };


        MapCreater.RiverConfig config_ChildRiver;
        config_ChildRiver = new MapCreater.RiverConfig()
        {
            river_NoiseScale = 30f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 2,
            river_DirectionInfluence = 0.25f,
            river_Width = 2,
            river_Forking = 0,
            river_ForkCount = 0
        };

        await mapCreater.GenerateMainRiver(config_River0, config_ChildRiver, DrawMainRiver, DrawChildRiver);
        await mapCreater.GenerateMainRiver(config_River1, config_ChildRiver, DrawMainRiver, DrawChildRiver);
        await mapCreater.GenerateMainRiver(config_River2, config_ChildRiver, DrawMainRiver, DrawChildRiver);
    }
    private void DrawMainRiver(int index, bool edge)
    {
        if (creater.data_mapGroundData.tileDic.ContainsKey(index))
        {
            creater.data_mapGroundData.tileDic[index] = 2001;
        }
        if (creater.data_mapBuildingData.tileDic.ContainsKey(index))
        {
            creater.data_mapBuildingData.tileDic.Remove(index);
        }
    }
    private void DrawChildRiver(int index, bool edge)
    {
        if (creater.data_mapGroundData.tileDic.ContainsKey(index))
        {
            creater.data_mapGroundData.tileDic[index] = 2001;
        }
        if (creater.data_mapBuildingData.tileDic.ContainsKey(index))
        {
            creater.data_mapBuildingData.tileDic.Remove(index);
        }
    }

}
/// <summary>
/// ���ɶ���
/// </summary>
public class MapCreate_Animal
{
    /// <summary>
    /// ����������С����
    /// </summary>
    private int minDistance = 20;
    public async Task CreateAnimal(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "���ڵ���ûͷ��";
        MapCreater.PoissonPointsAreaConfig config = new MapCreater.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = mapCreater.config_Map.map_Size,
            pointsArea_MinDistance = minDistance,
            pointsArea_MaxSamples = 20
        };
        await mapCreater.GeneratePoissonPointsArea(config, (index) =>
        {
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] == 1001)
            {
                if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    mapCreater.data_mapBuildingData.tileDic.Add(index, 2001);
                }
            }
        });
    }
}
/// <summary>
/// ���ɹ���
/// </summary>
public class MapCreate_Monster
{
    /// <summary>
    /// ����������С����
    /// </summary>
    private int minDistance = 30;
    public async Task CreateMonster(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "���ڵ���������";
        MapCreater.PoissonPointsAreaConfig config = new MapCreater.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = mapCreater.config_Map.map_Size,
            pointsArea_MinDistance = minDistance,
            pointsArea_MaxSamples = 20
        };
        await mapCreater.GeneratePoissonPointsArea(config, (index) =>
        {
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] == 1001)
            {
                if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    mapCreater.data_mapBuildingData.tileDic.Add(index, 2002);
                }
            }
        });
    }
}
/// <summary>
/// �����������
/// </summary>
public class MapCreate_RandomBuilding
{
    /// <summary>
    /// ����֮����С����
    /// </summary>
    private float building_MinDistance = 40;
    private MapCreater bind_MapCreater;
    /// <summary>
    /// �����������
    /// </summary>
    /// <returns></returns>
    public async Task CreateRandomBuilding(MapCreater mapCreater)
    {
        bind_MapCreater = mapCreater;
        bind_MapCreater.text_Waiting.text = "������������ټ�";
        MapCreater.PoissonPointsAreaConfig config = new MapCreater.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = bind_MapCreater.config_Map.map_Size,
            pointsArea_MinDistance = building_MinDistance,
            pointsArea_MaxSamples = 20
        };
        await bind_MapCreater.GeneratePoissonPointsArea(config, (pos) =>
        {
            int index = bind_MapCreater.Vector2ToIndex(pos.x, pos.y);
            if (bind_MapCreater.data_mapGroundData.tileDic.ContainsKey(index))
            {
                List<MapModConfig> list = MapModConfigData.mapModConfigs.FindAll((x) => { return x.MapMod_BaseGround == bind_MapCreater.data_mapGroundData.tileDic[index]; });
                if (list.Count > 0)
                {
                    MapModConfig mapModConfig = list[new System.Random().Next(0, list.Count)];
                    CreateMapMod(pos, mapModConfig);
                }
            }
        });
    }
    private void CreateMapMod(Vector2Int center, MapModConfig mapModConfig)
    {
        MapModData data_Map = Resources.Load<MapModData>($"MapModData/MapModData{mapModConfig.MapMod_ID}");
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapFloor)
        {
            int tempX = pair.key.x + (int)center.x + 30000;
            int tempY = pair.key.y + (int)center.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            if (bind_MapCreater.data_mapGroundData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapGroundData.tileDic[tempIndex] = pair.value;
            }
            else
            {
                bind_MapCreater.data_mapGroundData.tileDic.Add(tempIndex, pair.value);
            }
            if (bind_MapCreater.data_mapBuildingData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapBuildingData.tileDic.Remove(tempIndex);
            }
        }
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapBuilding)
        {
            int tempX = pair.key.x + (int)center.x + 30000;
            int tempY = pair.key.y + (int)center.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            if (bind_MapCreater.data_mapBuildingData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapBuildingData.tileDic[tempIndex] = pair.value;
            }
            else
            {
                bind_MapCreater.data_mapBuildingData.tileDic.Add(tempIndex, pair.value);
            }
        }

    }
}
/// <summary>
/// ���ɹ̶�����
/// </summary>
public class MapCreate_ConfigBuilding
{
    private MapCreater bind_MapCreater;
    public async Task CreateConfigBuilding(MapCreater mapCreater)
    {
        bind_MapCreater = mapCreater;
        bind_MapCreater.text_Waiting.text = "����׹��̫��";
        MapModConfig mapConfig_0 = MapModConfigData.GetMapModConfig(0);
        CreateMapMod(Vector2Int.zero, mapConfig_0);
        await Task.Yield();
        bind_MapCreater.text_Waiting.text = "���ڽ���һ��ʧ�ܵ�ʵ��";
        MapModConfig mapConfig_100 = MapModConfigData.GetMapModConfig(100);
        CreateMapMod(new Vector2Int((int)(-bind_MapCreater.config_Map.map_Size * 0.5f), (int)(bind_MapCreater.config_Map.map_Size * 0.5f)), mapConfig_100);
        await Task.Yield();
        bind_MapCreater.text_Waiting.text = "���ڶ���";
        MapModConfig mapConfig_200 = MapModConfigData.GetMapModConfig(200);
        CreateMapMod(new Vector2Int(0, -(int)(bind_MapCreater.config_Map.map_Size * 0.1f)), mapConfig_200);
    }
    private void CreateMapMod(Vector2Int center, MapModConfig mapModConfig)
    {
        MapModData data_Map = Resources.Load<MapModData>($"MapModData/MapModData{mapModConfig.MapMod_ID}");
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapFloor)
        {
            int tempX = pair.key.x + (int)center.x + 30000;
            int tempY = pair.key.y + (int)center.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            if (bind_MapCreater.data_mapGroundData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapGroundData.tileDic[tempIndex] = pair.value;
            }
            else
            {
                bind_MapCreater.data_mapGroundData.tileDic.Add(tempIndex, pair.value);
            }
            if (bind_MapCreater.data_mapBuildingData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapBuildingData.tileDic.Remove(tempIndex);
            }
        }
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapBuilding)
        {
            int tempX = pair.key.x + (int)center.x + 30000;
            int tempY = pair.key.y + (int)center.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            if (bind_MapCreater.data_mapBuildingData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapBuildingData.tileDic[tempIndex] = pair.value;
            }
            else
            {
                bind_MapCreater.data_mapBuildingData.tileDic.Add(tempIndex, pair.value);
            }
        }

    }
}
/// <summary>
/// ������Դ��
/// </summary>
public class MapCreate_Supply
{
    /// <summary>
    /// ��Դ���ܶ�(ƽ����/��)
    /// </summary>
    private float supply_Density = 5000f;
    /// <summary>
    /// �ݵ���Դ������
    /// </summary>
    private List<short> supplyIDs_Grass = new List<short>() { 1004 };
    /// <summary>
    /// ���ɵ�ͼ��Դ��
    /// </summary>
    /// <returns></returns>
    public async Task CreateSupply(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "�������ɵ�ͼ��Դ��";
        MapCreater.RandomPointsAreaConfig config = new MapCreater.RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2)/ supply_Density),
            pointsArea_SizeWhole = mapCreater.config_Map.map_Size,
            pointsArea_SizeFill = mapCreater.config_Map.map_Size,
        };
        await mapCreater.GenerateRandomPointsArea_Hard(config, (index) =>
        {
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] == 1001)
            {
                short stuffID = supplyIDs_Grass[new System.Random().Next(0, supplyIDs_Grass.Count)];
                if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    mapCreater.data_mapBuildingData.tileDic.Add(index, stuffID);
                }
            }
        });
    }

}
/// <summary>
/// ���ɳ�Ⱥֲ��
/// </summary>
public class MapCreate_GroupPlant
{
    /// <summary>
    /// ֲ���ܶ�(ƽ����/��)
    /// </summary>
    private float plantGroup_Density = 100;
    /// <summary>
    /// ֲ����������0 - 100
    /// </summary>
    private int plantGroup_Expand = 70;
    /// <summary>
    /// �ݵ��ϵ�ֲ��
    /// </summary>
    private List<short> plantGroupIDs_Grass = new List<short>() { 1001 };
    /// <summary>
    /// ���ɳ�Ⱥֲ��
    /// </summary>
    /// <returns></returns>
    public async Task CreateGroupPlant(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "�������ɴ���ֲ��";
        MapCreater.RandomExpandPointsAreaConfig config = new MapCreater.RandomExpandPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2) / plantGroup_Density),
            pointsArea_SizeWhole = mapCreater.config_Map.map_Size,
            pointsArea_Expand = plantGroup_Expand,
        };
        await mapCreater.GenerateRandomExpandPointsArea(config, (index) =>
        {
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] == 1001)
            {
                short stuffID = plantGroupIDs_Grass[new System.Random().Next(0, plantGroupIDs_Grass.Count)];
                if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    mapCreater.data_mapBuildingData.tileDic.Add(index, stuffID);
                }
            }
        });
    }
}
/// <summary>
/// ���ɵ���ֲ��
/// </summary>
public class MapCreate_SinglePlant
{
    /// <summary>
    /// ֲ���ܶ�(ƽ����/��)
    /// </summary>
    private float plantSingle_Density = 200f;
    private List<short> plantSingleIDs_Ground1001 = new List<short>() { 1000, 1003 };
    private List<short> plantSingleIDs_Ground1003 = new List<short>() { 1018 };
    private List<short> plantSingleIDs_Ground1004 = new List<short>() { 1018 };
    private List<short> plantSingleIDs_Ground1005 = new List<short>() { 1008, 1019 };
    /// <summary>
    /// ���ɵ���ֲ��
    /// </summary>
    /// <returns></returns>
    public async Task CreateSinglePlant(MapCreater mapCreater)
    {
        mapCreater.text_Waiting.text = "��������ֲ��" + (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2) / plantSingle_Density);
        MapCreater.RandomPointsAreaConfig config = new MapCreater.RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2)/ plantSingle_Density),
            pointsArea_SizeWhole = mapCreater.config_Map.map_Size,
            pointsArea_SizeFill = mapCreater.config_Map.map_Size,
        };
        await mapCreater.GenerateRandomPointsArea_Hard(config, (index) =>
        {
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index))
            {
                if (mapCreater.data_mapGroundData.tileDic[index] == 1001)
                {
                    short stuffID = plantSingleIDs_Ground1001[new System.Random().Next(0, plantSingleIDs_Ground1001.Count)];
                    if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        mapCreater.data_mapBuildingData.tileDic.Add(index, stuffID);
                    }
                }
                else if (mapCreater.data_mapGroundData.tileDic[index] == 1003)
                {
                    short stuffID = plantSingleIDs_Ground1003[new System.Random().Next(0, plantSingleIDs_Ground1003.Count)];
                    if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        mapCreater.data_mapBuildingData.tileDic.Add(index, stuffID);
                    }
                }
                else if (mapCreater.data_mapGroundData.tileDic[index] == 1004)
                {
                    short stuffID = plantSingleIDs_Ground1004[new System.Random().Next(0, plantSingleIDs_Ground1004.Count)];
                    if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        mapCreater.data_mapBuildingData.tileDic.Add(index, stuffID);
                    }
                }
                else if (mapCreater.data_mapGroundData.tileDic[index] == 1005)
                {
                    short stuffID = plantSingleIDs_Ground1005[new System.Random().Next(0, plantSingleIDs_Ground1005.Count)];
                    if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        mapCreater.data_mapBuildingData.tileDic.Add(index, stuffID);
                    }
                }
            }
        });
    }
}
public struct MapConfig
{
    /// <summary>
    /// ��ͼ�ߴ�(�뾶)
    /// </summary>
    public int map_Size;
    /// <summary>
    /// ��ͼ����
    /// </summary>
    public int map_Seed;
}
public struct NoiseConfig
{
    /// <summary>
    /// �����ߴ� 1-20
    /// </summary>
    public int noise_Sacle;
    /// <summary>
    /// �������ּ��� 0-1
    /// </summary>
    public float noise_Prob;
    /// <summary>
    /// ����ƫ��
    /// </summary>
    public Vector2 noise_Offset;
}
public struct AreaConfig
{
    /// <summary>
    /// �����ߴ�
    /// </summary>
    public int area_Size;
    /// <summary>
    /// ��������
    /// </summary>
    public Vector2Int area_Center;
}