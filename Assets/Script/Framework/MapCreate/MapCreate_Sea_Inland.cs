using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Sea_Inland 
{
    private MapCreate mapCreate_Bind;
    private Vector2 inlandSea_Center = new Vector2(0.5f, 0.5f);//内陆海相对中心
    private float inlandSea_WholeSize = 0.2f;//内陆海相对整体尺寸
    private float inlandSea_Width = 20f;//内陆海边界宽度
    private float inlandSea_NoiseSacle = 10;//内陆海噪声半径
    private float inlandSea_SnowlandWeight = 0.3f;//内陆海海洋权重
    private float inlandSea_TundraWeight = 0.2f;//内陆海沙滩权重
    /// <summary>
    /// 生成内陆海
    /// </summary>
    /// <returns></returns>
    public async Task CreateInlandSea(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreater.text_Waiting.text = "正在生成内陆海";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = inlandSea_Center * mapCreater.config_Map.map_Size,
            area_OuterRadius = mapCreater.config_Map.map_Size * inlandSea_WholeSize,
            area_FillRadius = mapCreater.config_Map.map_Size * inlandSea_WholeSize - inlandSea_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Scale = inlandSea_NoiseSacle,
        };
        await mapCreater.GenerateSolidArea(config, DrawInlandSeaArea);
    }
    private void DrawInlandSeaArea(int index, float perlinNoise, float realNoise)
    {
        /*有这个地块且不为海洋*/
        if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index, out short id) && id != 9000)
        {
            if (realNoise > (1 - inlandSea_SnowlandWeight))
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = 9000;
            }
            else if (realNoise > (1 - inlandSea_SnowlandWeight - inlandSea_TundraWeight))
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = 1005;
            }
        }
    }


    private Vector2 islet_Center = new Vector2(0.5f, 0.5f);//群岛相对中心
    private float islet_WholeSize = 0.1f;//群岛相对整体尺寸
    private float islet_Width = 20f;//内陆海边界宽度
    private float islet_NoiseSacle = 20;//群岛噪声半径
    private float islet_LandWeight = 0.3f;//群岛大陆权重
    /// <summary>
    /// 群岛沙滩权重
    /// </summary>
    private float islet_SandWeight = 0.2f;//
    /// <summary>
    /// 生成小岛
    /// </summary>
    /// <returns></returns>
    public async Task CreateIslet(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在生成神秘小岛";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = islet_Center * mapCreate_Bind.config_Map.map_Size,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size * islet_WholeSize,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size * islet_WholeSize - islet_Width,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = islet_NoiseSacle,
        };
        await mapCreate_Bind.GenerateSolidArea(config, DrawIsletArea);

    }
    private void DrawIsletArea(int index, float perlinNoise, float realNoise)
    {
        if (realNoise > (1 - islet_LandWeight))
        {
            mapCreate_Bind.data_mapGroundData.tileDic[index] = 1000;
        }
        else if (realNoise > (1 - islet_LandWeight - islet_SandWeight))
        {
            mapCreate_Bind.data_mapGroundData.tileDic[index] = 1005;
        }
    }
}

