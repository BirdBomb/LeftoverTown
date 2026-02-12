using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MapCreate_Land_Main 
{
    /// <summary>
    /// 大陆边界宽度
    /// </summary>
    public float edge_Radius = 50f;
    /// <summary>
    /// 大陆边界噪声半径
    /// </summary>
    public float noise_Scale = 20;
    /// <summary>
    /// 边界水域占比
    /// </summary>
    public float land_LakeWeight = 0.5f;
    /// <summary>
    /// 边界水域河岸占比
    /// </summary>
    public float land_LakeEdgeWeight = 0.1f;

    /// <summary>
    /// 生成大陆
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateLandArea(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在生成大陆";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreater.config_Map.map_Size,
            area_FillRadius = mapCreater.config_Map.map_Size - edge_Radius,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = noise_Scale,
        };
        await mapCreater.GenerateArea(config, (index, perlinNoise, realNoise) =>
        {
            if (realNoise < land_LakeWeight)
            {
                if (!mapCreater.data_mapGroundData.tileDic.TryAdd(index, 9000))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 9000;
                }
            }
            else if (realNoise < land_LakeWeight + land_LakeEdgeWeight)
            {
                if (!mapCreater.data_mapGroundData.tileDic.TryAdd(index, 1000))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1000;
                }
            }
            else
            {
                if (mapCreater.data_mapGroundData.tileDic.TryAdd(index, 1001))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1001;
                }
            }
        });
    }
}
