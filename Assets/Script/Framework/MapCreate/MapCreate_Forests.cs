using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Forests 
{
    MapCreate mapCreate_Bind;

    private System.Random random_Temp = new System.Random();
    private readonly Dictionary<int, List<short>> miningConfig = new()
    {
        [1001] = new List<short> { 1000, 1001 }, //草地
        [1003] = new List<short> { 1002 }, //雪地
        [1004] = new List<short> { 1002 }, //雪地
        [1005] = new List<short> { 1003 }, //沙漠
    };
    private float minDistance = 1;
    private float maxDistance = 10;
    private float noiseScale = 30;
    public async Task CreateForests(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在铺设树林";

        MapCreate.PoissonPointsAreaConfig config = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = mapCreate_Bind.config_Map.map_Size,
            pointsArea_MinDistance = minDistance,
            pointsArea_MaxDistance = maxDistance,
            pointsArea_NoiseScale = noiseScale,
            pointsArea_NoiseOffset = mapCreate_Bind.GetRandomOffset(),
            pointsArea_MaxSamples = 20
        };
        await mapCreate_Bind.GeneratePoissonPointsAsync(config, CreateTree);

    }

    public void CreateTree(Vector2Int pos)
    {
        int index = mapCreate_Bind.Vector2ToIndex(pos.x,pos.y);
        if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index, out short groundID) && miningConfig.TryGetValue(groundID,out List<short> treeList))
        {
            mapCreate_Bind.data_mapBuildingData.tileDic[index] = treeList[random_Temp.Next(0, treeList.Count)];
        }
    }

}
