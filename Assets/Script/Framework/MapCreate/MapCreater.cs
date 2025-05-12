using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static Fusion.Allocator;
using static Fusion.Sockets.NetBitBuffer;

public class MapCreater 
{
    private MapConfig config_Map;
    private int seed_Offset;
    private Slider slider_Waiting;
    private TextMeshProUGUI text_Waiting;
    private MapTileTypeData data_mapGroundData;
    private MapTileTypeData data_mapBuildingData;

    public async Task CreateMapGroundAndBuilding(MapConfig config, Slider slider, TextMeshProUGUI text, Action<MapTileTypeData, MapTileTypeData> callBack_ReturnData)
    {
        Debug.Log(config.map_Seed);
        seed_Offset = config.map_Seed;
        config_Map = config;
        slider_Waiting = slider;
        text_Waiting = text;
        data_mapGroundData = new MapTileTypeData();
        data_mapBuildingData = new MapTileTypeData();

        await CreateIslandArea();
        await CreateLake();
        await CreateForest();
        await CreateMining();
        await CreateAnimal();
        await CreateMonster();
        await CreateRandomBuilding();
        await CreateConfigBuilding();
        await CreateSupply();
        await CreateGroupPlant();
        await CreateSinglePlant();
        callBack_ReturnData.Invoke(data_mapGroundData, data_mapBuildingData);
    }
    #region//���ɴ�½
    /// <summary>
    /// ��Ȩ��
    /// </summary>
    private float ocean_Weight = 0.5f;
    /// <summary>
    /// ����Ȩ��
    /// </summary>
    private float ocean_EdgeWeight = 0.15f;
    /// <summary>
    /// ���ɴ�½
    /// </summary>
    /// <returns></returns>
    private async Task CreateIslandArea()
    {
        text_Waiting.text = "�������ɴ�½";
        IslandAreaConfig config;
        config = new IslandAreaConfig()
        {
            island_Center = Vector2.zero,
            island_WholeSize = config_Map.map_Size,
            island_FillSize = config_Map.map_Size - 20f,
            island_DistanceWeight = 0.5f,
            noise_Offset = GetRandomOffset(),
            noise_Sacle = 10,
        };
        await GenerateIslandArea(config, (index, val) =>
        {
            if (val < ocean_Weight)
            {
                if (data_mapGroundData.tileDic.ContainsKey(index))
                {
                    data_mapGroundData.tileDic[index] = 9000;
                }
                else
                {
                    data_mapGroundData.tileDic.Add(index, 9000);
                }
            }
            else if (val < ocean_Weight + ocean_EdgeWeight)
            {
                if (data_mapGroundData.tileDic.ContainsKey(index))
                {
                    data_mapGroundData.tileDic[index] = 1000;
                }
                else
                {
                    data_mapGroundData.tileDic.Add(index, 1000);
                }
            }
            else
            {
                if (data_mapGroundData.tileDic.ContainsKey(index))
                {
                    data_mapGroundData.tileDic[index] = 1001;
                }
                else
                {
                    data_mapGroundData.tileDic.Add(index, 1001);
                }
            }
        });
    }
    #endregion
    #region//���ɺ���
    /// <summary>
    /// �����ܶ�(ƽ����/��)
    /// </summary>
    private int lake_Density = 4000;
    /// <summary>
    /// �����뾶
    /// </summary>
    private float lake_Size = 10;
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
    private async Task CreateLake()
    {
        int lake_Count = (config_Map.map_Size * 2) * (config_Map.map_Size * 2) / lake_Density;
        text_Waiting.text = "�������ɺ���" + lake_Count;
        for (int i = 0;i< lake_Count; i++)
        {
            IslandAreaConfig config;
            config = new IslandAreaConfig()
            {
                island_Center = new Vector2(new System.Random().Next(-config_Map.map_Size + 20, config_Map.map_Size - 20), new System.Random().Next(-config_Map.map_Size + 20, config_Map.map_Size - 20)),
                island_WholeSize = lake_Size,
                island_FillSize = lake_Size - 5,
                island_DistanceWeight = 0.5f,
                noise_Offset = GetRandomOffset(),
                noise_Sacle = 5,
            };
            await GenerateIslandArea(config, (index, val) =>
            {
                if (val > (1 - lake_Weight))
                {
                    if (data_mapGroundData.tileDic.ContainsKey(index))
                    {
                        data_mapGroundData.tileDic[index] = 9000;
                    }
                    else
                    {
                        data_mapGroundData.tileDic.Add(index, 9000);
                    }
                }
                if(val> (1 - lake_Weight - lake_EdgeWeight))
                {
                    if (data_mapGroundData.tileDic.ContainsKey(index))
                    {
                        if (data_mapGroundData.tileDic[index] != 9000)
                        {
                            data_mapGroundData.tileDic[index] = 1000;
                        }
                    }
                    else
                    {
                        data_mapGroundData.tileDic.Add(index, 1000);
                    }
                }
            });
        }
    }
    #endregion
    #region //����ɭ��
    /// <summary>
    /// �����ܶ�(ƽ����/��)
    /// </summary>
    private int forest_Density = 3000;
    /// <summary>
    /// ɭ�����
    /// </summary>
    private int forest_Size = 40;
    /// <summary>
    /// ɭ�ֽ��ܶ�0-1
    /// </summary>
    private float forest_Compactness = 0.15f;
    /// <summary>
    /// �ݵ���ľ����
    /// </summary>
    private List<short> treeIDs_GrassTreeID = new List<short>() { 1000,1003 };
    /// <summary>
    /// ������ľ
    /// </summary>
    /// <returns></returns>
    private async Task CreateForest()
    {
        int forest_Count = (config_Map.map_Size * 2) * (config_Map.map_Size * 2) / forest_Density;
        text_Waiting.text = "������������" + forest_Count;

        for (int i = 0; i < forest_Count; i++)
        {
            RandomPointsAreaConfig config = new RandomPointsAreaConfig()
            {
                pointsArea_Center = new Vector2Int(new System.Random().Next(-config_Map.map_Size, config_Map.map_Size), new System.Random().Next(-config_Map.map_Size, config_Map.map_Size)),
                pointsArea_Count = (int)(forest_Compactness * forest_Size * forest_Size),
                pointsArea_SizeWhole = forest_Size,
                pointsArea_SizeFill = forest_Size - 10,
            };
            await GenerateRandomPointsArea_Gradual(config, (index) =>
            {
                if (data_mapGroundData.tileDic.ContainsKey(index) && data_mapGroundData.tileDic[index] == 1001)
                {
                    short treeID = treeIDs_GrassTreeID[new System.Random().Next(0, treeIDs_GrassTreeID.Count)];
                    if (data_mapBuildingData.tileDic.ContainsKey(index))
                    {
                        data_mapBuildingData.tileDic[index] = treeID;
                    }
                    else
                    {
                        data_mapBuildingData.tileDic.Add(index, treeID);
                    }
                }
            });
        }
    }
    #endregion

    #region//������Ⱥֲ��
    /// <summary>
    /// ֲ���ܶ�0-1
    /// </summary>
    private float plantGroup_Density = 0.01f;
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
    private async Task CreateGroupPlant()
    {
        text_Waiting.text = "�������ɴ���ֲ��";
        RandomExpandPointsAreaConfig config = new RandomExpandPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)(plantGroup_Density * (config_Map.map_Size * 2) * (config_Map.map_Size * 2)),
            pointsArea_SizeWhole = config_Map.map_Size,
            pointsArea_Expand = plantGroup_Expand,
        };
        await GenerateRandomExpandPointsArea(config, (index) =>
        {
            if (data_mapGroundData.tileDic.ContainsKey(index) && data_mapGroundData.tileDic[index] == 1001)
            {
                short stuffID = plantGroupIDs_Grass[new System.Random().Next(0, plantGroupIDs_Grass.Count)];
                if (!data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    data_mapBuildingData.tileDic.Add(index, stuffID);
                }
            }
        });
    }
    #endregion
    #region//���ɵ���ֲ��
    /// <summary>
    /// ֲ���ܶ�0-1
    /// </summary>
    private float plantSingle_Density = 0.01f;
    /// <summary>
    /// �ݵ��ϵ�ֲ��
    /// </summary>
    private List<short> plantSingleIDs_Grass = new List<short>() { 1000 };
    /// <summary>
    /// ���ɵ���ֲ��
    /// </summary>
    /// <returns></returns>
    private async Task CreateSinglePlant()
    {
        text_Waiting.text = "��������ֲ��";
        RandomPointsAreaConfig config = new RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)(plantSingle_Density * (config_Map.map_Size * 2) * (config_Map.map_Size * 2)),
            pointsArea_SizeWhole = config_Map.map_Size,
            pointsArea_SizeFill = config_Map.map_Size,
        };
        await GenerateRandomPointsArea_Hard(config, (index) =>
        {
            if (data_mapGroundData.tileDic.ContainsKey(index) && data_mapGroundData.tileDic[index] == 1001)
            {
                short stuffID = plantSingleIDs_Grass[new System.Random().Next(0, plantSingleIDs_Grass.Count)];
                if (!data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    data_mapBuildingData.tileDic.Add(index, stuffID);
                }
            }
        });
    }
    #endregion
    #region//���ɿ���
    /// <summary>
    /// �����ܶ�(ƽ����/��)
    /// </summary>
    private int mining_Density = 3500;
    /// <summary>
    /// ����Ȩ��
    /// </summary>
    private float mining_Weight = 0.5f;
    /// <summary>
    /// ������Χ���Ȩ��
    /// </summary>
    private float mining_EdgeWeight = 0.1f;
    /// <summary>
    /// �����ߴ�
    /// </summary>
    private float mining_Size = 15;
    /// <summary>
    /// ���ɿ���
    /// </summary>
    /// <returns></returns>
    private async Task CreateMining()
    {
        int mining_Count = (config_Map.map_Size * 2) * (config_Map.map_Size * 2) / mining_Density;
        text_Waiting.text = "�������ɿ���" + mining_Count;

        for (int i = 0; i < mining_Count; i++)
        {
            IslandAreaConfig config;
            config = new IslandAreaConfig()
            {
                island_Center = new Vector2(new System.Random().Next(-config_Map.map_Size + 20, config_Map.map_Size - 20), new System.Random().Next(-config_Map.map_Size + 20, config_Map.map_Size - 20)),
                island_WholeSize = mining_Size,
                island_FillSize = 5,
                island_DistanceWeight = 0.4f,
                noise_Offset = GetRandomOffset(),
                noise_Sacle = 10,
            };
            await GenerateIslandArea(config, (index, val) =>
            {
                if (val > (1 - mining_Weight))
                {
                    if (data_mapGroundData.tileDic.ContainsKey(index) && data_mapGroundData.tileDic[index] < 9000)
                    {
                        if (data_mapBuildingData.tileDic.ContainsKey(index))
                        {
                            data_mapBuildingData.tileDic[index] = 1002;
                        }
                        else
                        {
                            data_mapBuildingData.tileDic.Add(index, 1002);
                        }
                        data_mapGroundData.tileDic[index] = 1000;
                    }
                }
                else if (val > (1 - mining_Weight - mining_EdgeWeight))
                {
                    data_mapBuildingData.tileDic.Remove(index);
                    data_mapGroundData.tileDic[index] = 1000;
                }
            });
        }
    }
    #endregion
    #region//��ӵ�ͼ��Դ��
    /// <summary>
    /// ��Դ���ܶ�0-1
    /// </summary>
    private float supply_Density = 0.005f;
    /// <summary>
    /// �ݵ���Դ������
    /// </summary>
    private List<short> supplyIDs_Grass = new List<short>() { 1004 };
    /// <summary>
    /// ���ɵ�ͼ��Դ��
    /// </summary>
    /// <returns></returns>
    private async Task CreateSupply()
    {
        text_Waiting.text = "�������ɵ�ͼ��Դ��";
        RandomPointsAreaConfig config = new RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)(supply_Density * (config_Map.map_Size * 2) * (config_Map.map_Size * 2)),
            pointsArea_SizeWhole = config_Map.map_Size,
            pointsArea_SizeFill = config_Map.map_Size,
        };
        await GenerateRandomPointsArea_Hard(config, (index) =>
        {
            if (data_mapGroundData.tileDic.ContainsKey(index) && data_mapGroundData.tileDic[index] == 1001)
            {
                short stuffID = supplyIDs_Grass[new System.Random().Next(0, supplyIDs_Grass.Count)];
                if (!data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    data_mapBuildingData.tileDic.Add(index, stuffID);
                }
            }
        });
    }
    #endregion

    #region//�ḻ����
    /// <summary>
    /// С�����ܶ�0-1
    /// </summary>
    private float stuff_Density = 0.2f;
    /// <summary>
    /// �ݵ�С��������
    /// </summary>
    private List<short> stuffIDs_Grass = new List<short>() { 1004 };
    /// <summary>
    /// ��������С����
    /// </summary>
    /// <returns></returns>
    private async Task CreateOtherStuff()
    {
        text_Waiting.text = "������������С����";
        RandomPointsAreaConfig config = new RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)(stuff_Density * (config_Map.map_Size * 2) * (config_Map.map_Size * 2)),
            pointsArea_SizeWhole = config_Map.map_Size,
            pointsArea_SizeFill = config_Map.map_Size,
        };
        await GenerateRandomPointsArea_Hard(config, (index) =>
        {
            if (data_mapGroundData.tileDic.ContainsKey(index) && data_mapGroundData.tileDic[index] == 1001)
            {
                short stuffID = stuffIDs_Grass[new System.Random().Next(0, stuffIDs_Grass.Count)];
                if (!data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    data_mapBuildingData.tileDic.Add(index, stuffID);
                }
            }
        });
    }
    #endregion
    #region//������������
    /// <summary>
    /// ������������
    /// </summary>
    /// <returns></returns>
    private async Task CreateAnimal()
    {
        text_Waiting.text = "���ڵ���ûͷ��";
        PoissonPointsAreaConfig config = new PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = config_Map.map_Size,
            pointsArea_MinDistance = 20,
            pointsArea_MaxSamples = 20
        };
        await GeneratePoissonPointsArea(config, (index) =>
        {
            if (data_mapGroundData.tileDic.ContainsKey(index) && data_mapGroundData.tileDic[index] == 1001)
            {
                if (!data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    data_mapBuildingData.tileDic.Add(index, 2001);
                }
            }
        });
    }
    #endregion
    #region//���ɵж�����
    /// <summary>
    /// ���ɵж�����
    /// </summary>
    /// <returns></returns>
    private async Task CreateMonster()
    {
        text_Waiting.text = "���ڵ���������";
        PoissonPointsAreaConfig config = new PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = config_Map.map_Size,
            pointsArea_MinDistance = 30,
            pointsArea_MaxSamples = 20
        };
        await GeneratePoissonPointsArea(config, (index) =>
        {
            if (data_mapGroundData.tileDic.ContainsKey(index) && data_mapGroundData.tileDic[index] == 1001)
            {
                if (!data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    data_mapBuildingData.tileDic.Add(index, 2002);
                }
            }
        });
    }

    #endregion
    #region//�����������
    /// <summary>
    /// ����֮����С����
    /// </summary>
    private float building_MinDistance = 40;
    /// <summary>
    /// �����������
    /// </summary>
    /// <returns></returns>
    private async Task CreateRandomBuilding()
    {
        text_Waiting.text = "������������ټ�";
        PoissonPointsAreaConfig config = new PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = config_Map.map_Size,
            pointsArea_MinDistance = building_MinDistance,
            pointsArea_MaxSamples = 20
        };
        await GeneratePoissonPointsArea(config, (pos) =>
        {
            int index = Vector2ToIndex(pos.x, pos.y);
            if (data_mapGroundData.tileDic.ContainsKey(index))
            {
                List<MapModConfig> list = MapModConfigData.mapModConfigs.FindAll((x) => { return x.MapMod_Base == data_mapGroundData.tileDic[index]; });
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
            if (data_mapGroundData.tileDic.ContainsKey(tempIndex))
            {
                data_mapGroundData.tileDic[tempIndex] = pair.value;
            }
            else
            {
                data_mapGroundData.tileDic.Add(tempIndex, pair.value);
            }
            if (data_mapBuildingData.tileDic.ContainsKey(tempIndex))
            {
                data_mapBuildingData.tileDic.Remove(tempIndex);
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
            if (data_mapBuildingData.tileDic.ContainsKey(tempIndex))
            {
                data_mapBuildingData.tileDic[tempIndex] = pair.value;
            }
            else
            {
                data_mapBuildingData.tileDic.Add(tempIndex, pair.value);
            }
        }

    }
    #endregion
    #region//���ɹ̶�����
    /// <summary>
    /// ���ɹ̶�����
    /// </summary>
    /// <returns></returns>
    private async Task CreateConfigBuilding()
    {
        text_Waiting.text = "̫������׹��";
        MapModConfig mapConfig_0 = MapModConfigData.GetMapModConfig(0);
        CreateMapMod(Vector2Int.zero, mapConfig_0);
        await Task.Yield();
        text_Waiting.text = "���ڽ���һ��ʧ�ܵ�ʵ��";
        MapModConfig mapConfig_100 = MapModConfigData.GetMapModConfig(100);
        CreateMapMod(new Vector2Int((int)(-config_Map.map_Size * 0.5f), (int)(config_Map.map_Size * 0.5f)), mapConfig_100);
    }

    #endregion

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
    private Vector2 GetRandomOffset()
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
    struct IslandAreaConfig
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
        /// �������Ȩ��0-1
        /// </summary>
        public float island_DistanceWeight;
        /// <summary>
        /// ��������
        /// </summary>
        public Vector2 island_Center;
        /// <summary>
        /// �����ߴ� 1-20
        /// </summary>
        public float noise_Sacle;
        /// <summary>
        /// ����ƫ��(0,1)
        /// </summary>
        public Vector2 noise_Offset;
    }
    private async Task GenerateIslandArea(IslandAreaConfig islandArea, Action<int, float> action)
    {
        float reciprocal_size = 1f / islandArea.island_WholeSize;
        float size_Empty = islandArea.island_WholeSize - islandArea.island_FillSize;
        int from_x = (int)(islandArea.island_Center.x - islandArea.island_WholeSize);
        int to_x = (int)(islandArea.island_Center.x + islandArea.island_WholeSize);
        int from_y = (int)(islandArea.island_Center.y - islandArea.island_WholeSize);
        int to_y = (int)(islandArea.island_Center.y + islandArea.island_WholeSize);

        /*����ͼ��������*/
        Vector2 noise_Center = islandArea.noise_Offset;
        /*����ͼ�����ߴ�(1-0)*/
        float noise_Scale = (1f / islandArea.noise_Sacle);
        /*��õ�����ֵ(0-1)*/
        float perlinNoise;
        /*�����ĵľ���*/
        float distance;
        for (int i = from_x; i < to_x; i++)
        {
            for (int j = from_y; j < to_y; j++)
            {
                int x = i;
                int y = j;
                distance = Vector2.Distance(new Vector2(x, y), islandArea.island_Center);
                if (distance > islandArea.island_FillSize)
                {
                    float distanceLerp = Mathf.Lerp(0, 1, (islandArea.island_WholeSize - distance) / size_Empty);
                    perlinNoise = Mathf.PerlinNoise(noise_Center.x + (x + from_x) * noise_Scale, noise_Center.y + (y + from_y) * noise_Scale);
                    perlinNoise = distanceLerp * islandArea.island_DistanceWeight + perlinNoise * (1 - islandArea.island_DistanceWeight);
                }
                else
                {
                    perlinNoise = 1;
                }
                action.Invoke(Vector2ToIndex(x, y), perlinNoise);
            }
            await Task.Yield();
            slider_Waiting.value = (i - from_x) * reciprocal_size;
        }
    }
    #endregion
    #region//���ɵ�����ȷֲ�����
    private struct PoissonPointsAreaConfig
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
    private async Task GeneratePoissonPointsArea(PoissonPointsAreaConfig areaConfig, Action<int> action)
    {
        List<Vector2Int> poissonPoints = new List<Vector2Int>();
        List<Vector2Int> spawnPoints = new List<Vector2Int>();
        float cellSize = areaConfig.pointsArea_MinDistance / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(areaConfig.pointsArea_Size / cellSize), Mathf.CeilToInt(areaConfig.pointsArea_Size / cellSize)];

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
            await Task.Yield();
        }
        Vector2Int offset = new Vector2Int(areaConfig.pointsArea_Center.x - areaConfig.pointsArea_Size / 2, areaConfig.pointsArea_Center.y - areaConfig.pointsArea_Size / 2);
        for (int i = 0; i < poissonPoints.Count; i++)
        {
            action.Invoke(Vector2ToIndex(offset.x + poissonPoints[i].x, offset.y + poissonPoints[i].y));
        }
    }
    private async Task GeneratePoissonPointsArea(PoissonPointsAreaConfig areaConfig, Action<Vector2Int> action)
    {
        List<Vector2Int> poissonPoints = new List<Vector2Int>();
        List<Vector2Int> spawnPoints = new List<Vector2Int>();
        float cellSize = areaConfig.pointsArea_MinDistance / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(areaConfig.pointsArea_Size / cellSize), Mathf.CeilToInt(areaConfig.pointsArea_Size / cellSize)];

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
            await Task.Yield();
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
    #region//�������ֲ�����(����/������)
    private struct RandomPointsAreaConfig
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
    private async Task GenerateRandomPointsArea_Gradual(RandomPointsAreaConfig areaConfig, Action<int> action)
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
            await Task.Yield();
            slider_Waiting.value = (i * reciprocal_Count);
        }
    }
    private async Task GenerateRandomPointsArea_Hard(RandomPointsAreaConfig areaConfig, Action<int> action)
    {
        float reciprocal_Count = 1f / areaConfig.pointsArea_Count;

        int from_x = (areaConfig.pointsArea_Center.x - areaConfig.pointsArea_SizeWhole);
        int to_x = (areaConfig.pointsArea_Center.x + areaConfig.pointsArea_SizeWhole);
        int from_y = (areaConfig.pointsArea_Center.y - areaConfig.pointsArea_SizeWhole);
        int to_y = (areaConfig.pointsArea_Center.y + areaConfig.pointsArea_SizeWhole);
        /*�����ĵľ���*/
        float distance;

        for (int i = 0; i < areaConfig.pointsArea_Count; i++)
        {
            // �������λ��
            int randomX = new System.Random().Next(from_x, to_x);
            int randomY = new System.Random().Next(from_y, to_y);
            distance = Vector2.Distance(new Vector2(randomX, randomY), areaConfig.pointsArea_Center);
            action.Invoke(Vector2ToIndex(randomX, randomY)); await Task.Yield();
            slider_Waiting.value = (i * reciprocal_Count);
        }
    }
    #endregion
    #region//������ŵ�����ֲ�����(������)
    private struct RandomExpandPointsAreaConfig
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
    private async Task GenerateRandomExpandPointsArea(RandomExpandPointsAreaConfig areaConfig, Action<int> action)
    {
        float reciprocal_Count = 1f / areaConfig.pointsArea_Count;

        int from_x = (areaConfig.pointsArea_Center.x - areaConfig.pointsArea_SizeWhole);
        int to_x = (areaConfig.pointsArea_Center.x + areaConfig.pointsArea_SizeWhole);
        int from_y = (areaConfig.pointsArea_Center.y - areaConfig.pointsArea_SizeWhole);
        int to_y = (areaConfig.pointsArea_Center.y + areaConfig.pointsArea_SizeWhole);

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
            await Task.Yield();
            slider_Waiting.value = (i * reciprocal_Count);
        }
    }

    #endregion
    private int Vector2ToIndex(int x,int y)
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
public struct MapConfig
{
    /// <summary>
    /// ��ͼ�ߴ�(�뾶)
    /// </summary>
    public int map_Size;
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