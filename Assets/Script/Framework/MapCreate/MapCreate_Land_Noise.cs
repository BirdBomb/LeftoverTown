using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
/// <summary>
/// 地块复杂度
/// </summary>
public class MapCreate_Land_Noise 
{
    /// <summary>
    /// 草地上的异色地块
    /// </summary>
    private short groundID_Grass = 1002;
    /// <summary>
    /// 草地上的异色地块
    /// </summary>
    private short groundID_Snow = 1003;
    /// <summary>
    /// 草地上的异色地块
    /// </summary>
    private short groundID_Desert = 1000;
    private System.Random random = new System.Random();


    /// <summary>
    /// 涂色噪声半径
    /// </summary>
    public float land_NoiseSacle = 4;
    /// <summary>
    /// 涂色占比
    /// </summary>
    public float land_LakeWeight = 0.33f;

    /// <summary>
    /// 大陆涂色
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateNoise(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在给大陆涂色";
        MapCreate.PerlinHollowConfig config;
        config = new MapCreate.PerlinHollowConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreater.config_Map.map_Size,
            area_FillRadius = mapCreater.config_Map.map_Size - 10,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = land_NoiseSacle,
        };
        await mapCreater.GenerateArea(config, (index, perlinNoise, realNoise) =>
        {
            if (realNoise < land_LakeWeight)
            {
                if (mapCreater.data_mapGroundData.tileDic[index] == 1001)
                {
                    mapCreater.data_mapGroundData.tileDic[index] = groundID_Grass;
                }
                else if (mapCreater.data_mapGroundData.tileDic[index] == 1004)
                {
                    mapCreater.data_mapGroundData.tileDic[index] = groundID_Snow;
                }
                else if (mapCreater.data_mapGroundData.tileDic[index] == 1005)
                {
                    mapCreater.data_mapGroundData.tileDic[index] = groundID_Desert;
                }
            }
        });
    }
}
