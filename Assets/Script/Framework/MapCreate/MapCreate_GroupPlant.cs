using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_GroupPlant 
{
    private System.Random random = new System.Random();
    private float plant_Proportion = 0.2f;
    public async Task CreateGroupPlant(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在生成大型植被";
        random = new System.Random(mapCreater.seed_Offset + mapCreater.GetAccIndex());

        MapCreate.PerlinHollowConfig config = new MapCreate.PerlinHollowConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreater.config_Map.map_Size,
            area_FillRadius = mapCreater.config_Map.map_Size - 10,
            noise_Offset = mapCreater.GetRandomOffset(),
            noise_Sacle = 3,
        };
        await mapCreater.GenerateArea(config, (index, perlinNoise, realNoise) =>
        {
            if (realNoise < plant_Proportion)
            {
                if (mapCreater.data_mapGroundData.tileDic[index] == 1001)
                {
                    mapCreater.data_mapBuildingData.tileDic.TryAdd(index, 1001);
                }
                else if (mapCreater.data_mapGroundData.tileDic[index] == 1004)
                {
                }
                else if (mapCreater.data_mapGroundData.tileDic[index] == 1005)
                {
                }
            }
        });
    }
}
