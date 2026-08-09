using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Lakes 
{
    MapCreate mapCreate_Bind;
    public float edgeRadius = 100f;//边界宽度
    public float noiseScale = 20;//噪声半径
    public float landLakeWeight = 0.25f;//大陆湖泊占比
    public float landLakeEdgeWeight = 0.025f;//大陆湖泊边界占比

    /// <summary>
    /// 生成湖泊
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateLake(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在生成湖泊";
        MapCreate.PerlinEvenlyConfig config;
        config = new MapCreate.PerlinEvenlyConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size - edgeRadius,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = noiseScale,
        };
        await mapCreate_Bind.GenerateEvenlyArea(config, DrawLake);
    }
    public void DrawLake(int index, float perlinNoise, float realNoise)
    {
        if (realNoise < landLakeWeight)
        {
            if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index, out var value) && value != 9000)
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = 9000;
                mapCreate_Bind.data_mapBuildingData.tileDic.Remove(index);
            }
        }
        else if (realNoise < landLakeWeight + landLakeEdgeWeight)
        {
            if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index, out var value) && value < 9000)
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = 1000;
            }
        }
    }
}
