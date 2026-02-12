using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Land_Snow 
{
    /// <summary>
    /// 雪原相对中心
    /// </summary>
    private Vector2 snowland_Center = new Vector2(-0.5f, 0.5f);
    /// <summary>
    /// 雪原相对整体尺寸
    /// </summary>
    private float snowland_WholeSize = 0.33f;
    /// <summary>
    /// 雪原边界宽度
    /// </summary>
    private float snowland_Width = 50f;
    /// <summary>
    /// 雪原噪声半径
    /// </summary>
    private float snowland_NoiseSacle = 10;
    /// <summary>
    /// 雪原雪地权重
    /// </summary>
    private float snowland_SnowlandWeight = 0.3f;
    /// <summary>
    /// 雪原苔原权重
    /// </summary>
    private float snowland_TundraWeight = 0.2f;
    /// <summary>
    /// 生成雪原
    /// </summary>
    /// <returns></returns>
    public async Task CreateSnowland(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在生成雪原";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = snowland_Center * mapCreater.config_Map.map_Size,
            area_OuterRadius = mapCreater.config_Map.map_Size * snowland_WholeSize,
            area_FillRadius = mapCreater.config_Map.map_Size * snowland_WholeSize - snowland_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = snowland_NoiseSacle,
        };
        await mapCreater.GenerateArea(config, (index, perlinNoise, realNoise) =>
        {
            /*有这个地块且不为海洋*/
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] != 9000)
            {
                if (realNoise > (1 - snowland_SnowlandWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1004;
                }
                else if (realNoise > (1 - snowland_SnowlandWeight - snowland_TundraWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1003;
                }
            }
        });

    }

}
