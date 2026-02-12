using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Monster 
{
    /// <summary>
    /// 中立生物最小距离
    /// </summary>
    private int minDistance = 30;
    private System.Random random = new System.Random();
    public async Task CreateMonster(MapCreate mapCreater)
    {
        random = new System.Random(mapCreater.seed_Offset + mapCreater.GetAccIndex());
        mapCreater.text_Waiting.text = "正在诞生不高兴";
        MapCreate.PoissonPointsAreaConfig config = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = mapCreater.config_Map.map_Size,
            pointsArea_MinDistance = minDistance,
            pointsArea_MaxSamples = 20
        };
        await mapCreater.GeneratePoissonPointsAsync(config, (pos) =>
        {
            int index = mapCreater.Vector2ToIndex(pos.x, pos.y);
            if (!mapCreater.data_mapGroundData.tileDic.ContainsKey(index)) return;
            if (mapCreater.data_mapGroundData.tileDic[index] == 1001)
            {
                if (random.Next(0, 2) <= 0)
                {
                    CreateZombie(mapCreater, pos, index);
                }
                else
                {
                    CreateDog(mapCreater, pos, index);
                }
            }
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] == 1001)
            {
                if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    mapCreater.data_mapBuildingData.tileDic.Add(index, 2001);
                }
            }
        });
    }
    private void CreateZombie(MapCreate mapCreater, Vector2Int pos, int index)
    {
        if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
        {
            mapCreater.data_mapBuildingData.tileDic.Add(index, 2002);
        }
    }
    private void CreateDog(MapCreate mapCreater, Vector2Int pos, int index)
    {
        if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
        {
            mapCreater.data_mapBuildingData.tileDic.Add(index, 2016);
        }
        int index_0 = mapCreater.Vector2ToIndex(pos.x + 1, pos.y);
        if (!mapCreater.data_mapBuildingData.tileDic.TryAdd(index_0, 99)) 
        {
            mapCreater.data_mapBuildingData.tileDic[index_0] = 99;
        }
        if (!mapCreater.data_mapGroundData.tileDic.TryAdd(index_0, 1001))
        {
            mapCreater.data_mapGroundData.tileDic[index_0] = 1001;
        }
    }
}
