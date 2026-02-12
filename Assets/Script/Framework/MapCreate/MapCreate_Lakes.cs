using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Lakes 
{
    /// <summary>
    /// 边界宽度
    /// </summary>
    public float edge_Radius = 100f;
    /// <summary>
    /// 噪声半径
    /// </summary>
    public float noise_Scale = 20;
    /// <summary>
    /// 大陆水域占比
    /// </summary>
    public float land_LakeWeight = 0.25f;
    /// <summary>
    /// 大陆水域边界占比
    /// </summary>
    public float land_LakeEdgeWeight = 0.025f;

    /// <summary>
    /// 生成湖泊
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateLake(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在生成湖泊";
        MapCreate.PerlinHollowConfig config;
        config = new MapCreate.PerlinHollowConfig()
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
            else if (realNoise < land_LakeWeight + land_LakeEdgeWeight )
            {
                if (mapCreater.data_mapGroundData.tileDic.TryGetValue(index, out short val) && val < 9000)
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1000;
                }
            }
        });
    }

}
