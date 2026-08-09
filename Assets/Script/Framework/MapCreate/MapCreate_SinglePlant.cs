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
    private List<short> plantLargeIDs_Ground1001 = new List<short>() { 1201 };
    private System.Random random = new System.Random();

    private readonly Dictionary<int, List<short>> plantSingleConfig = new()
    {
        [1001] = new List<short> { 1201, 1203, 1300, 1301, 1302, }, //草地
        [1003] = new List<short> { 1002 }, //雪地
        [1004] = new List<short> { 1002 }, //雪地
        [1005] = new List<short> { 1202 }, //沙漠
        [1006] = new List<short> { 1205 }, //沙漠
    };

    private MapCreate mapCreate_Bind;
    /// <summary>
    /// 生成单个植物
    /// </summary>
    /// <returns></returns>
    public async Task CreateSinglePlant(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        random = new System.Random(mapCreate_Bind.seed_Offset + mapCreate_Bind.GetAccIndex());
        mapCreate_Bind.text_Waiting.text = "正在生成小型植物" + (int)((mapCreate_Bind.config_Map.map_Size * 2) * (mapCreate_Bind.config_Map.map_Size * 2) / plantSingle_Density);
        MapCreate.RandomPointsAreaConfig config = new MapCreate.RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreate_Bind.config_Map.map_Size * 2) * (mapCreate_Bind.config_Map.map_Size * 2) / plantSingle_Density),
            pointsArea_SizeWhole = mapCreate_Bind.config_Map.map_Size,
            pointsArea_SizeFill = mapCreate_Bind.config_Map.map_Size,
        };
        await mapCreate_Bind.GenerateRandomPointsArea_Hard(config, SetSinglePlant);
    }
    /// <summary>
    /// 生成大型植物
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateLargePlant(MapCreate mapCreater)
    {
        mapCreate_Bind = mapCreater;
        random = new System.Random(mapCreate_Bind.seed_Offset + mapCreate_Bind.GetAccIndex());
        mapCreate_Bind.text_Waiting.text = "正在生成大型植物" + (int)((mapCreate_Bind.config_Map.map_Size * 2) * (mapCreate_Bind.config_Map.map_Size * 2) / plantSingle_Density);
        MapCreate.RandomPointsAreaConfig config = new MapCreate.RandomPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Count = (int)((mapCreate_Bind.config_Map.map_Size * 2) * (mapCreate_Bind.config_Map.map_Size * 2) / plantLarge_Density),
            pointsArea_SizeWhole = mapCreate_Bind.config_Map.map_Size,
            pointsArea_SizeFill = mapCreate_Bind.config_Map.map_Size,
        };
        await mapCreate_Bind.GenerateRandomPointsArea_Hard(config, SetLargePlant);
    }
    private void SetSinglePlant(int x, int y)
    {
        int index = mapCreate_Bind.Vector2ToIndex(x, y);
        if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index, out short groundID) && plantSingleConfig.TryGetValue(groundID, out List<short> list))
        {
            mapCreate_Bind.data_mapBuildingData.tileDic[index] = list[random.Next(0, list.Count)];
        }
    }
    private void SetLargePlant(int x,int y)
    {
        int index_0 = mapCreate_Bind.Vector2ToIndex(x, y);
        int index_1 = mapCreate_Bind.Vector2ToIndex(x + 1, y);
        if (mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index_0, out short groundID_0) && mapCreate_Bind.data_mapGroundData.tileDic.TryGetValue(index_1, out short groundID_1))
        {
            if (groundID_0 == groundID_1 && plantSingleConfig.TryGetValue(groundID_0, out List<short> list))
            {
                mapCreate_Bind.data_mapBuildingData.tileDic[index_0] = list[random.Next(0, list.Count)];
                mapCreate_Bind.data_mapBuildingData.tileDic[index_1] = 99;
            }
        }
    }
}
