using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Monster 
{
    public MapCreate mapCreate_Bind;
    private System.Random random = new System.Random();
    private int minDistance = 30;
    private int maxDistance = 40;
    public async Task CreateMonster(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        random = new System.Random(mapCreate_Bind.seed_Offset + mapCreate_Bind.GetAccIndex());
        mapCreate_Bind.text_Waiting.text = "正在诞生不高兴";
        MapCreate.PoissonPointsAreaConfig config = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = mapCreate_Bind.config_Map.map_Size,
            pointsArea_MinDistance = minDistance,
            pointsArea_MaxDistance = maxDistance,
            pointsArea_NoiseScale = 50,
            pointsArea_NoiseOffset = mapCreate_Bind.GetRandomOffset(),
            pointsArea_MaxSamples = 20
        };
        await mapCreate_Bind.GeneratePoissonPointsAsync(config, TryToCreateMonster);
    }
    private void TryToCreateMonster(Vector2Int pos)
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
                            CreateZombie(mapCreate_Bind, pos, index);
                        }
                        else
                        {
                            CreateDog(mapCreate_Bind, pos, index);
                        }
                    }
                    break;
            }
        }

    }
    private void CreateZombie(MapCreate mapCreater, Vector2Int pos, int index)
    {
        mapCreater.data_mapBuildingData.tileDic.TryAdd(index, 2400);
    }
    private void CreateDog(MapCreate mapCreater, Vector2Int pos, int index)
    {
        if(mapCreater.data_mapBuildingData.tileDic.TryAdd(index, 2401))
        {
            int index_0 = mapCreater.Vector2ToIndex(pos.x + 1, pos.y);
            mapCreater.data_mapBuildingData.tileDic[index_0] = 99;
            mapCreater.data_mapGroundData.tileDic[index_0] = 1001;
        }
    }
}
