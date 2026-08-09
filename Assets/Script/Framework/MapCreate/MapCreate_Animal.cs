using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Animal 
{
    public MapCreate mapCreate_Bind;
    private System.Random random = new System.Random();
    /// <summary>
    /// 中立生物最小距离
    /// </summary>
    private int minDistance = 20;
    private int maxDistance = 30;

    private const short id_RabbitHome = 2100;
    private const short id_ChickenHome = 2101;
    public async Task CreateAnimal(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        random = new System.Random(mapCreate_Bind.seed_Offset + mapCreate_Bind.GetAccIndex());
        mapCreate_Bind.text_Waiting.text = "正在诞生没头脑";
        MapCreate.PoissonPointsAreaConfig config = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = mapCreate_Bind.config_Map.map_Size,
            pointsArea_MinDistance = minDistance,
            pointsArea_MaxDistance = maxDistance,
            pointsArea_NoiseScale = 25,
            pointsArea_NoiseOffset = mapCreate_Bind.GetRandomOffset(),
            pointsArea_MaxSamples = 20,
        };
        await mapCreate_Bind.GeneratePoissonPointsAsync(config, TryToCreateAnimal);
    }
    private void TryToCreateAnimal(Vector2Int pos)
    {
        int index = mapCreate_Bind.Vector2ToIndex(pos.x, pos.y);
        if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index, out short groundID))
        {
            switch (groundID)
            {
                case 1001:
                    {
                        if (random.Next(0, 2) <= 0)
                        {
                            CreateRabbit(mapCreate_Bind, pos, index);
                        }
                        else
                        {
                            CreateChicken(mapCreate_Bind, pos, index);
                        }
                    }
                    break;
            }
        }
    }
    private void CreateRabbit(MapCreate mapCreater, Vector2Int pos, int index)
    {
        mapCreater.data_mapBuildingData.tileDic.TryAdd(index, id_RabbitHome);
    }
    private void CreateChicken(MapCreate mapCreater, Vector2Int pos, int index)
    {
        mapCreater.data_mapBuildingData.tileDic.TryAdd(index, id_ChickenHome);
    }
}
