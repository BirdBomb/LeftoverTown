using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static Fusion.Sockets.NetBitBuffer;
using static UnityEngine.Rendering.DebugUI;

public class MapCreater 
{
    private MapConfig config_Map;
    private int seed_Offset;
    private UnityEngine.UI.Slider slider_Waiting;
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

        //await CreateFloorArea();
        await CreateTree();
        await CreateMountainsArea();
        
        await CreateLake();
        //await CreateBuilding();
        await CreateHome();
        callBack_ReturnData.Invoke(data_mapGroundData, data_mapBuildingData);
    }
    #region//�����߼�
    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    private async Task CreateFloorArea()
    {
        AreaConfig areaConfig;
        NoiseConfig noiseConfig;

        text_Waiting.text = "��������ѩԭ";
        /*ѩԭ����9/1*/
        areaConfig = new AreaConfig() { area_Size = config_Map.map_Size / 3, area_Center = new Vector2Int(-config_Map.map_Size / 3, config_Map.map_Size / 3) };
        noiseConfig = new NoiseConfig() { noise_Sacle = 10, noise_Prob = 0.8f, noise_Offset = GetRandomOffset() };
        await FillArea(data_mapGroundData, areaConfig, noiseConfig, 1009);
        text_Waiting.text = "��������ɳ��";
        /*ѩԭ����9/1*/
        areaConfig = new AreaConfig() { area_Size = config_Map.map_Size / 3, area_Center = new Vector2Int(config_Map.map_Size / 3, -config_Map.map_Size / 3) };
        noiseConfig = new NoiseConfig() { noise_Sacle = 10, noise_Prob = 0.8f, noise_Offset = GetRandomOffset() };
        await FillArea(data_mapGroundData, areaConfig, noiseConfig, 1008);
    }
    /// <summary>
    /// ����ɽ��
    /// </summary>
    /// <returns></returns>
    private async Task CreateMountainsArea()
    {
        AreaConfig areaConfig;
        NoiseConfig noiseConfig_Main;
        NoiseConfig noiseConfig_Child;
        areaConfig = new AreaConfig() { area_Center = Vector2Int.zero, area_Size = config_Map.map_Size };
        text_Waiting.text = "��������ɽ��";
        /*ɽ�����noise_Sacle��ɽ������noise_Prob����ʯ���noise_Sacle����ʯ����noise_Prob*/
        noiseConfig_Main = new NoiseConfig() { noise_Sacle = 30, noise_Prob = 0.5f, noise_Offset = GetRandomOffset() };
        noiseConfig_Child = new NoiseConfig() { noise_Sacle = 5, noise_Prob = 0.7f, noise_Offset = GetRandomOffset() };
        await FillArea(data_mapBuildingData, areaConfig, noiseConfig_Main, noiseConfig_Child, 9100);
        text_Waiting.text = "��������С����";
        /*ɽ�����noise_Sacle��ɽ������noise_Prob����ʯ���noise_Sacle����ʯ����noise_Prob*/
        noiseConfig_Main = new NoiseConfig() { noise_Sacle = 7, noise_Prob = 0.3f, noise_Offset = GetRandomOffset() };
        noiseConfig_Child = new NoiseConfig() { noise_Sacle = 2, noise_Prob = 0.6f, noise_Offset = GetRandomOffset() };
        await FillArea(data_mapBuildingData, areaConfig, noiseConfig_Main, noiseConfig_Child, 9100);
    }
    /// <summary>
    /// ������ľ
    /// </summary>
    /// <returns></returns>
    private async Task CreateTree()
    {
        AreaConfig areaConfig;
        NoiseConfig noiseConfig_Main;
        NoiseConfig noiseConfig_Child;
        areaConfig = new AreaConfig() { area_Center = Vector2Int.zero, area_Size = config_Map.map_Size };
        text_Waiting.text = "�������ɴ�Ƭ����";
        /*�������noise_Sacle�����ּ���noise_Prob������ï�̶ܳ�noise_Prob*/
        noiseConfig_Main = new NoiseConfig() { noise_Sacle = 30, noise_Prob = 0.5f, noise_Offset = GetRandomOffset() };
        noiseConfig_Child = new NoiseConfig() { noise_Sacle = 1, noise_Prob = 0.2f, noise_Offset = GetRandomOffset() };
        await FillArea(data_mapBuildingData, areaConfig, noiseConfig_Main, noiseConfig_Child, 9000);
        text_Waiting.text = "��������СƬ����";
        /*�������noise_Sacle�����ּ���noise_Prob������ï�̶ܳ�noise_Prob*/
        noiseConfig_Main = new NoiseConfig() { noise_Sacle = 7, noise_Prob = 0.4f, noise_Offset = GetRandomOffset() };
        noiseConfig_Child = new NoiseConfig() { noise_Sacle = 1, noise_Prob = 0.4f, noise_Offset = GetRandomOffset() };
        await FillArea(data_mapBuildingData, areaConfig, noiseConfig_Main, noiseConfig_Child, 9000);
    }
    /// <summary>
    /// ���ɺ���
    /// </summary>
    /// <returns></returns>
    private async Task CreateLake()
    {
        AreaConfig areaConfig = new AreaConfig() { area_Center = Vector2Int.zero, area_Size = config_Map.map_Size };
        NoiseConfig noiseConfig = new NoiseConfig() { noise_Sacle = 30, noise_Prob = 0.3f, noise_Offset = GetRandomOffset() };
        text_Waiting.text = "�����ھ����";
        await FillArea(data_mapBuildingData, areaConfig, noiseConfig, 0);
        text_Waiting.text = "���ڸ�������ˮ";
        await FillArea(data_mapGroundData, areaConfig, noiseConfig, 9000);
    }
    /// <summary>
    /// ���ɽ���
    /// </summary>
    /// <returns></returns>
    private async Task CreateBuilding()
    {
        text_Waiting.text = "������������ټ�";
        for (int i = 0; i < 1; i++)
        {
            Vector2 offset = GetRandomOffset() * 50;
            MapModConfig config_Map = MapModConfigData.mapModConfigs[UnityEngine.Random.Range(0, MapModConfigData.mapModConfigs.Count)];
            MapModData data_Map = Resources.Load<MapModData>($"MapModData/MapModData{config_Map.MapMod_ID}");
            Debug.Log(data_Map.data_mapBuilding.Count);
            foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapFloor)
            {
                int tempX = pair.key.x + (int)offset.x + 30000;
                int tempY = pair.key.y + (int)offset.y + 30000;
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
            }
            foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapBuilding)
            {
                int tempX = pair.key.x + (int)offset.x + 30000;
                int tempY = pair.key.y + (int)offset.y + 30000;
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
            slider_Waiting.value = (i / 5f);
        }
    }
    /// <summary>
    /// ������ʼ��
    /// </summary>
    /// <returns></returns>
    private async Task CreateHome()
    {
        text_Waiting.text = "̫������׹��";
        MapModData data_Map = Resources.Load<MapModData>($"MapModData/MapModData{0}");


        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapFloor)
        {
            int tempX = pair.key.x + 30000;
            int tempY = pair.key.y + 30000;
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
        }
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapBuilding)
        {
            int tempX = pair.key.x + 30000;
            int tempY = pair.key.y + 30000;
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
}
public struct MapConfig
{
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