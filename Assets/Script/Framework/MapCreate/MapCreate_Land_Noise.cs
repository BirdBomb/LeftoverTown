using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
/// <summary>
/// 地块复杂度
/// </summary>
public class MapCreate_Land_Noise 
{
    private MapCreate mapCreate_Bind;
    private short groundID_Grass = 1002;//草地上的异色地块
    private short groundID_Snow = 1003;//雪地上的异色地块
    private short groundID_Desert = 1000;//沙漠上的异色地块
    public float land_NoiseSacle = 4;//涂色噪声半径
    public float land_LakeWeight = 0.33f;//涂色占比

    /// <summary>
    /// 大陆涂色
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateNoise(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在给大陆涂色";
        MapCreate.PerlinEvenlyConfig config;
        config = new MapCreate.PerlinEvenlyConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size - 10,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = land_NoiseSacle,
        };
        await mapCreate_Bind.GenerateEvenlyArea(config, DrawNoise);
    }
    public void DrawNoise(int index, float perlinNoise, float realNoise)
    {
        if (realNoise < land_LakeWeight)
        {
            if (mapCreate_Bind.data_mapGroundData.tileDic[index] == 1001)
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = groundID_Grass;
            }
            else if (mapCreate_Bind.data_mapGroundData.tileDic[index] == 1004)
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = groundID_Snow;
            }
            else if (mapCreate_Bind.data_mapGroundData.tileDic[index] == 1005)
            {
                mapCreate_Bind.data_mapGroundData.tileDic[index] = groundID_Desert;
            }
        }
    }
}
