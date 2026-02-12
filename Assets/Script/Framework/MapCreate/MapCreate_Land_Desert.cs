using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MapCreate_Land_Desert 
{
    /// <summary>
    /// 沙漠相对中心
    /// </summary>
    private Vector2 desert_Center = new Vector2(0.5f, -0.5f);
    /// <summary>
    /// 沙漠相对整体尺寸
    /// </summary>
    private float desert_WholeSize = 0.33f;
    /// <summary>
    /// 沙漠边界宽度
    /// </summary>
    private float desert_Width = 50f;
    /// <summary>
    /// 沙漠噪声半径
    /// </summary>
    private float desert_NoiseSacle = 10;
    /// <summary>
    /// 沙漠权重
    /// </summary>
    private float desert_SandWeight = 0.3f;
    /// <summary>
    /// 荒地权重
    /// </summary>
    private float desert_GroundWeight = 0.2f;
    /// <summary>
    /// 生成沙漠
    /// </summary>
    /// <returns></returns>
    public async Task CreateDesert(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在生成沙漠";
        MapCreate.PerlinSolidConfig config;
        config = new MapCreate.PerlinSolidConfig()
        {
            area_Center = desert_Center * mapCreater.config_Map.map_Size,
            area_OuterRadius = mapCreater.config_Map.map_Size * desert_WholeSize,
            area_FillRadius = mapCreater.config_Map.map_Size * desert_WholeSize - desert_Width,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = desert_NoiseSacle,
        };
        await mapCreater.GenerateArea(config, (index, perlinNoise, realNoise) =>
        {
            /*有这个地块且不为海洋*/
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] != 9000)
            {
                if (realNoise > (1 - desert_SandWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1005;
                }
                else if (realNoise > (1 - desert_SandWeight - desert_GroundWeight))
                {
                    mapCreater.data_mapGroundData.tileDic[index] = 1000;
                }
            }
        });
    }

}
