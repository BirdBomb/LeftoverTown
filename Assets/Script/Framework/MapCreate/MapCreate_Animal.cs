using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Animal 
{
    /// <summary>
    /// 中立生物最小距离
    /// </summary>
    private int minDistance = 20;
    private System.Random random = new System.Random();
    public async Task CreateAnimal(MapCreate mapCreater)
    {
        random = new System.Random(mapCreater.seed_Offset + mapCreater.GetAccIndex());
        mapCreater.text_Waiting.text = "正在诞生没头脑";
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
                    CreateRabbit(mapCreater, pos, index);
                }
                else
                {
                    CreateChicken(mapCreater, pos, index);
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
    private void CreateRabbit(MapCreate mapCreater, Vector2Int pos, int index)
    {
        if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
        {
            mapCreater.data_mapBuildingData.tileDic.Add(index, 2001);
        }
    }
    private void CreateChicken(MapCreate mapCreater, Vector2Int pos, int index)
    {
        if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
        {
            mapCreater.data_mapBuildingData.tileDic.Add(index, 2018);
        }
    }
}
