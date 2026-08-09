using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
/// <summary>
/// 子地形
/// </summary>
public class MapCreate_Land_SubBiome 
{
    private MapCreate mapCreate_Bind;
    public float edge_Radius = 100f;//边界宽度
    public float noise_Scale = 20;//噪声半径
    public float land_SubBiomeWeight = 0.25f;//子地形占比
    /// <summary>
    /// 生成子地形
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateSubBiome(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在生成子地形";
        MapCreate.PerlinEvenlyConfig config;
        config = new MapCreate.PerlinEvenlyConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size - edge_Radius,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = noise_Scale,
        };
        await mapCreate_Bind.GenerateEvenlyArea(config, DrawSubBiome);
    }
    public void DrawSubBiome(int index, float perlinNoise, float realNoise)
    {
        if (realNoise < land_SubBiomeWeight)
        {
            mapCreate_Bind.data_mapGroundData.tileDic[index] = GetSubBiomeId(mapCreate_Bind.data_mapGroundData.tileDic[index]);
        }
    }
    private short GetSubBiomeId(short originalGroundID)
    {
        switch(originalGroundID)
        {
            case 1001: return 1006;
            case 1002: return 1006;
        }
        return originalGroundID;
    }
}
