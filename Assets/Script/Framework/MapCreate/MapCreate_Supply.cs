using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class MapCreate_Supply 
{
    private MapCreate mapCreate_Bind;
    System.Random random = new System.Random();
    private float supply_Density = 2000f;//资源点密度(平方米/个)
    /// <summary>
    /// 草地资源点种类
    /// </summary>
    private List<short> supplyIDs_Grass = new List<short>() { 1204 };

    /// <summary>
    /// 生成地图资源点
    /// </summary>
    /// <returns></returns>
    public async Task CreateSupply(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        mapCreate_Bind.text_Waiting.text = "正在生成地图资源点";
        random = new System.Random(mapCreate_Bind.seed_Offset + mapCreate_Bind.GetAccIndex());
        MapCreate.RandomPointsAreaConfig config = new MapCreate.RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreate_Bind.config_Map.map_Size * 2) * (mapCreate_Bind.config_Map.map_Size * 2) / supply_Density),
            pointsArea_SizeWhole = mapCreate_Bind.config_Map.map_Size,
            pointsArea_SizeFill = mapCreate_Bind.config_Map.map_Size,
        };
        await mapCreate_Bind.GenerateRandomPointsArea_Hard(config, CreateGrassSupply);
    }
    public void CreateGrassSupply(int x, int y)
    {
        int index = mapCreate_Bind.Vector2ToIndex(x, y);
        if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index, out short groundID))
        {
            switch (groundID)
            {
                case 1001:
                    {
                        mapCreate_Bind.data_mapBuildingData.tileDic.TryAdd(index, supplyIDs_Grass[random.Next(0, supplyIDs_Grass.Count)]);
                    }
                    break;
            }
        }
    }
}
