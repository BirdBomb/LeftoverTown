using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MapCreate_Land_Desert 
{
    private Vector2 desert_Center = new Vector2(0.5f, -0.5f);//沙漠相对中心
    private float desert_WholeSize = 0.33f;//沙漠相对整体尺寸
    private float desert_Width = 50f;//沙漠边界宽度
    private float desert_NoiseSacle = 10;//沙漠噪声半径
    private float desert_SandWeight = 0.3f;//沙漠权重
    private float desert_GroundWeight = 0.2f;//荒地权重
    private MapCreate mapCreate_Bind;
    /// <summary>
    /// 生成沙漠
    /// </summary>
    /// <returns></returns>
    public async Task CreateDesert(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在生成沙漠";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = desert_Center * mapCreate_Bind.config_Map.map_Size,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size * desert_WholeSize,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size * desert_WholeSize - desert_Width,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = desert_NoiseSacle,
        };
        await mapCreate_Bind.GenerateSolidArea(config, DrawDesertLandArea);
    }
    public void DrawDesertLandArea(int index, float perlinNoise, float realNoise)
    {
        if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index, out short id) && id != 9000)
        {
            if (realNoise > (1 - desert_SandWeight))
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = 1005;
            }
            else if (realNoise > (1 - desert_SandWeight - desert_GroundWeight))
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = 1000;
            }
        }
    }
}
