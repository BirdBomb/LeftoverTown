using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_GroupPlant 
{
    private MapCreate mapCreate_Bind;
    private System.Random random = new System.Random();
    private float groupPlantWeight = 0.2f;//荒地权重
    private readonly Dictionary<int, List<short>> plantSingleConfig = new()
    {
        [1001] = new List<short> { 1200 }, //草地
    };

    public async Task CreateGroupPlant(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在生成大型植被";
        random = new System.Random(mapCreate_Bind.seed_Offset + mapCreate_Bind.GetAccIndex());

        MapCreate.PerlinEvenlyConfig config = new MapCreate.PerlinEvenlyConfig()
        {
            area_Center = Vector2.zero,
            area_OuterRadius = mapCreate_Bind.config_Map.map_Size,
            area_FillRadius = mapCreate_Bind.config_Map.map_Size - 10,
            noise_Offset = mapCreate_Bind.GetRandomOffset(),
            noise_Scale = 3,
        };
        await mapCreate_Bind.GenerateEvenlyArea(config, DrawGroupPlant);
    }
    public void DrawGroupPlant(int index,float perlinNoise,float realNoise)
    {

        if (realNoise < groupPlantWeight && mapCreate_Bind.data_mapBuildingData.tileDic.TryGetValue(index,out short groundID) && plantSingleConfig.TryGetValue(groundID, out List<short> list))
        {
            mapCreate_Bind.data_mapBuildingData.tileDic[index] = list[random.Next(0, list.Count)];
        }
    }
}
