using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Land_Snow 
{
    private Vector2 snowland_Center = new Vector2(-0.5f, 0.5f);//雪原相对中心
    private float snowland_WholeSize = 0.33f;//雪原相对整体尺寸
    private float snowland_Width = 50f;//雪原边界宽度
    private float snowland_NoiseSacle = 10;//雪原噪声半径
    private float snowland_SnowlandWeight = 0.3f;//雪原雪地权重
    private float snowland_TundraWeight = 0.2f;//雪原苔原权重
    private MapCreate mapCreate_Bind;
    /// <summary>
    /// 生成雪原
    /// </summary>
    /// <returns></returns>
    public async Task CreateSnowland(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在生成雪原";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = snowland_Center * mapCreate_Bind.config_Map.map_Size,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size * snowland_WholeSize,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size * snowland_WholeSize - snowland_Width,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = snowland_NoiseSacle,
        };
        await mapCreate_Bind.GenerateSolidArea(config, DrawSnowLandArea);
    }
    public void DrawSnowLandArea(int index, float perlinNoise, float realNoise)
    {
        if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index,out short id) && id != 9000)
        {
            if (realNoise > (1 - snowland_SnowlandWeight))
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = 1004;
            }
            else if (realNoise > (1 - snowland_SnowlandWeight - snowland_TundraWeight))
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = 1003;
            }
        }
    }
}
