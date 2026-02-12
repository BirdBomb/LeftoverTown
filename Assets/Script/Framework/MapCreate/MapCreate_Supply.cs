using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class MapCreate_Supply 
{
    /// <summary>
    /// 资源点密度(平方米/个)
    /// </summary>
    private float supply_Density = 2000f;
    System.Random random = new System.Random();
    /// <summary>
    /// 生成地图资源点
    /// </summary>
    /// <returns></returns>
    public async Task CreateSupply(MapCreate mapCreater)
    {
        mapCreater.text_Waiting.text = "正在生成地图资源点";
        random = new System.Random(mapCreater.seed_Offset + mapCreater.GetAccIndex());
        MapCreate.RandomPointsAreaConfig config = new MapCreate.RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2) / supply_Density),
            pointsArea_SizeWhole = mapCreater.config_Map.map_Size,
            pointsArea_SizeFill = mapCreater.config_Map.map_Size,
        };
        await mapCreater.GenerateRandomPointsArea_Hard(config, (x,y) =>
        {
            int index = mapCreater.Vector2ToIndex(x, y);
            if (mapCreater.data_mapGroundData.tileDic.ContainsKey(index) && mapCreater.data_mapGroundData.tileDic[index] == 1001)
            {
                CreateGrassSupply(mapCreater, x, y, index);
            }
        });
    }
    /// <summary>
    /// 草地资源点种类
    /// </summary>
    private List<short> supplyIDs_Grass = new List<short>() { 1004 };

    public void CreateGrassSupply(MapCreate mapCreater,int x,int y,int index)
    {
        short stuffID = supplyIDs_Grass[random.Next(0, supplyIDs_Grass.Count)];
        if (!mapCreater.data_mapBuildingData.tileDic.ContainsKey(index))
        {
            mapCreater.data_mapBuildingData.tileDic.Add(index, stuffID);
        }
    }
}
