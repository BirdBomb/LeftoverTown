using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MapCreate_Land_Main 
{
    public float edge_Radius = 50f;//大陆边界宽度
    public float noise_Scale = 20;//大陆边界噪声半径
    public float land_LakeWeight = 0.5f;//边界水域占比
    public float land_LakeEdgeWeight = 0.1f;//边界水域河岸占比
    private MapCreate mapCreate_Bind;
    /// <summary>
    /// 生成大陆
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateLandArea(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在生成大陆";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size - edge_Radius,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = noise_Scale,
        };
        await mapCreate_Bind.GenerateSolidArea(config, DrawLandArea);
    }
    public void DrawLandArea(int index,float perlinNoise,float realNoise)
    {
        if (realNoise < land_LakeWeight)
        {
            mapCreate_Bind.data_mapGroundData.tileDic[index] = 9000;
        }
        else if (realNoise < land_LakeWeight + land_LakeEdgeWeight)
        {
            mapCreate_Bind.data_mapGroundData.tileDic[index] = 1000;
        }
        else
        {
            mapCreate_Bind.data_mapGroundData.tileDic[index] = 1001;
        }
    }
}
