using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_SinglePlant 
{
    /// <summary>
    /// 植物密度(平方米/个)
    /// </summary>
    private float plantSingle_Density = 200f;
    private float plantLarge_Density = 400f;
    private List<short> plantSingleIDs_Ground1001 = new List<short>() { 1005, 1006, 1007, 1010 };
    private List<short> plantSingleIDs_Ground1003 = new List<short>() { 1018 };
    private List<short> plantSingleIDs_Ground1004 = new List<short>() { 1018 };
    private List<short> plantSingleIDs_Ground1005 = new List<short>() { 1008, 1019 };
    private List<short> plantLargeIDs_Ground1001 = new List<short>() { 1120 };
    private System.Random random = new System.Random();
    /// <summary>
    /// 生成单个植物
    /// </summary>
    /// <returns></returns>
    public async Task CreateSinglePlant(MapCreate mapCreater)
    {
        random = new System.Random(mapCreater.seed_Offset + mapCreater.GetAccIndex());
        mapCreater.text_Waiting.text = "正在生成小型植物" + (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2) / plantSingle_Density);
        MapCreate.RandomPointsAreaConfig config = new MapCreate.RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2) / plantSingle_Density),
            pointsArea_SizeWhole = mapCreater.config_Map.map_Size,
            pointsArea_SizeFill = mapCreater.config_Map.map_Size,
        };
        await mapCreater.GenerateRandomPointsArea_Hard(config, (x, y) =>
        {
            SetSinglePlant(mapCreater, x, y);
        });
    }

    public async Task CreateLargePlant(MapCreate mapCreater)
    {
        random = new System.Random(mapCreater.seed_Offset + mapCreater.GetAccIndex());
        mapCreater.text_Waiting.text = "正在生成大型植物" + (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2) / plantSingle_Density);
        MapCreate.RandomPointsAreaConfig config = new MapCreate.RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreater.config_Map.map_Size * 2) * (mapCreater.config_Map.map_Size * 2) / plantLarge_Density),
            pointsArea_SizeWhole = mapCreater.config_Map.map_Size,
            pointsArea_SizeFill = mapCreater.config_Map.map_Size,
        };
        await mapCreater.GenerateRandomPointsArea_Hard(config, (x, y) =>
        {
            SetLargePlant(mapCreater, x, y);
        });
    }
    private void SetSinglePlant(MapCreate mapCreate, int x, int y)
    {
        int index_0 = mapCreate.Vector2ToIndex(x, y);
        if (mapCreate.data_mapGroundData.tileDic.TryGetValue(index_0, out short val_0))
        {
            short stuffID = 0;
            if (val_0 == 1001)
            {
                stuffID = plantSingleIDs_Ground1001[random.Next(0, plantSingleIDs_Ground1001.Count)];
                if (!mapCreate.data_mapBuildingData.tileDic.ContainsKey(index_0))
                {
                    mapCreate.data_mapBuildingData.tileDic.Add(index_0, stuffID);
                }
            }
            else if (val_0 == 1003)
            {
                stuffID = plantSingleIDs_Ground1003[random.Next(0, plantSingleIDs_Ground1003.Count)];
                if (!mapCreate.data_mapBuildingData.tileDic.ContainsKey(index_0))
                {
                    mapCreate.data_mapBuildingData.tileDic.Add(index_0, stuffID);
                }
            }
            else if (val_0 == 1004)
            {
                stuffID = plantSingleIDs_Ground1004[random.Next(0, plantSingleIDs_Ground1004.Count)];
                if (!mapCreate.data_mapBuildingData.tileDic.ContainsKey(index_0))
                {
                    mapCreate.data_mapBuildingData.tileDic.Add(index_0, stuffID);
                }
            }
            else if (val_0 == 1005)
            {
                stuffID = plantSingleIDs_Ground1005[random.Next(0, plantSingleIDs_Ground1005.Count)];
                
            }
            if (!mapCreate.data_mapBuildingData.tileDic.ContainsKey(index_0) && stuffID > 0)
            {
                mapCreate.data_mapBuildingData.tileDic.Add(index_0, stuffID);
            }
        }

    }
    private void SetLargePlant(MapCreate mapCreate, int x,int y)
    {
        int index_0 = mapCreate.Vector2ToIndex(x, y);
        int index_1 = mapCreate.Vector2ToIndex(x + 1, y);
        if (mapCreate.data_mapGroundData.tileDic.TryGetValue(index_0, out short val_0)&&
            mapCreate.data_mapGroundData.tileDic.TryGetValue(index_1, out short val_1))
        {
            short stuffID = 0;
            if (val_0 == 1001)
            {
                stuffID = plantLargeIDs_Ground1001[random.Next(0, plantLargeIDs_Ground1001.Count)];
                
            }
            if (!mapCreate.data_mapBuildingData.tileDic.ContainsKey(index_0) && stuffID > 0)
            {
                mapCreate.data_mapBuildingData.tileDic.Add(index_0, stuffID);
                if (!mapCreate.data_mapBuildingData.tileDic.TryAdd(index_1, 99))
                {
                    mapCreate.data_mapBuildingData.tileDic[index_1] = 99;
                }
            }
        }
    }
}
