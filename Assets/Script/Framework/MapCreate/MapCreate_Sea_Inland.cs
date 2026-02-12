using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Sea_Inland 
{
    /// <summary>
    /// 内陆海相对中心
    /// </summary>
    private Vector2 inlandSea_Center = new Vector2(0.5f, 0.5f);
    /// <summary>
    /// 内陆海相对整体尺寸
    /// </summary>
    private float inlandSea_WholeSize = 0.2f;
    /// <summary>
    /// 内陆海边界宽度
    /// </summary>
    private float inlandSea_Width = 20f;
    /// <summary>
    /// 内陆海噪声半径
    /// </summary>
    private float inlandSea_NoiseSacle = 10;
    /// <summary>
    /// 内陆海海洋权重
    /// </summary>
    private float inlandSea_SnowlandWeight = 0.3f;
    /// <summary>
    /// 内陆海沙滩权重
    /// </summary>
    private float inlandSea_TundraWeight = 0.2f;
    /// <summary>
    /// 生成内陆海
    /// </summary>
    /// <returns></returns>
    public async Task CreateInlandSea(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在生成内陆海";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = inlandSea_Center * mapCreater.config_Map.map_Size,
            area_OuterRadius = mapCreater.config_Map.map_Size * inlandSea_WholeSize,
            area_FillRadius = mapCreater.config_Map.map_Size * inlandSea_WholeSize - inlandSea_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = inlandSea_NoiseSacle,
        };
        await mapCreater.GenerateArea(config, (index, perlinNoise, realNoise) =>
        {
            /*有这个地块且不为海洋*/
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] != 9000)
            {
                if (realNoise > (1 - inlandSea_SnowlandWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 9000;
                }
                else if (realNoise > (1 - inlandSea_SnowlandWeight - inlandSea_TundraWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1005;
                }
            }
        });

    }

    /// <summary>
    /// 群岛相对中心
    /// </summary>
    private Vector2 islet_Center = new Vector2(0.5f, 0.5f);
    /// <summary>
    /// 群岛相对整体尺寸
    /// </summary>
    private float islet_WholeSize = 0.1f;
    /// <summary>
    /// 内陆海边界宽度
    /// </summary>
    private float islet_Width = 20f;
    /// <summary>
    /// 群岛噪声半径
    /// </summary>
    private float islet_NoiseSacle = 20;
    /// <summary>
    /// 群岛大陆权重
    /// </summary>
    private float islet_LandWeight = 0.3f;
    /// <summary>
    /// 群岛沙滩权重
    /// </summary>
    private float islet_SandWeight = 0.2f;
    /// <summary>
    /// 生成小岛
    /// </summary>
    /// <returns></returns>
    public async Task CreateIslet(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在生成神秘小岛";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = islet_Center * mapCreater.config_Map.map_Size,
            area_OuterRadius = mapCreater.config_Map.map_Size * islet_WholeSize,
            area_FillRadius = mapCreater.config_Map.map_Size * islet_WholeSize - islet_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = islet_NoiseSacle,
        };
        await mapCreater.GenerateArea(config, (index, perlinNoise, realNoise) =>
        {
            if (realNoise > (1 - islet_LandWeight))
            {
                mapCreater.data_mapGroundData.tileDic[index] = 1000;
            }
            else if (realNoise > (1 - islet_LandWeight - islet_SandWeight))
            {
                mapCreater.data_mapGroundData.tileDic[index] = 1005;
            }
            else
            {

            }
        });

    }

}

