using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate_RandomBuilding 
{
    /// <summary>
    /// 建筑之间最小距离
    /// </summary>
    private float building_MinDistance = 40;
    private MapCreate bind_MapCreater;
    /// <summary>
    /// 生成随机建筑
    /// </summary>
    /// <returns></returns>
    public async Task CreateRandomBuilding(MapCreate mapCreater)
    {
        bind_MapCreater = mapCreater;
        bind_MapCreater.text_Waiting.text = "正在添加人类踪迹";
        System.Random random = new System.Random(mapCreater.GetAccIndex() + mapCreater.seed_Offset);
        MapCreate.PoissonPointsAreaConfig config = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = bind_MapCreater.config_Map.map_Size,
            pointsArea_MinDistance = building_MinDistance,
            pointsArea_MaxSamples = 20
        };
        await bind_MapCreater.GeneratePoissonPointsAsync(config, (pos) =>
        {
            int index = bind_MapCreater.Vector2ToIndex(pos.x, pos.y);
            if (bind_MapCreater.data_mapGroundData.tileDic.ContainsKey(index))
            {
                List<MapModConfig> list = MapModConfigData.mapModConfigs.FindAll((x) => { return !x.MapMod_Ruins && x.MapMod_BaseGround == bind_MapCreater.data_mapGroundData.tileDic[index]; });
                if (list.Count > 0)
                {
                    MapModConfig mapModConfig = list[random.Next(0, list.Count)];
                    CreateMapMod(pos, mapModConfig);
                }
            }
        });
    }
    private void CreateMapMod(Vector2Int center, MapModConfig mapModConfig)
    {
        MapModData data_Map = Resources.Load<MapModData>($"MapModData/MapModData{mapModConfig.MapMod_ID}");
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapFloor)
        {
            int tempX = pair.key.x + (int)center.x + 30000;
            int tempY = pair.key.y + (int)center.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            if (bind_MapCreater.data_mapGroundData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapGroundData.tileDic[tempIndex] = pair.value;
            }
            else
            {
                bind_MapCreater.data_mapGroundData.tileDic.Add(tempIndex, pair.value);
            }
            if (bind_MapCreater.data_mapBuildingData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapBuildingData.tileDic.Remove(tempIndex);
            }
        }
        foreach (KeyValuePair<Vector2Int, short> pair in data_Map.data_mapBuilding)
        {
            int tempX = pair.key.x + (int)center.x + 30000;
            int tempY = pair.key.y + (int)center.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            if (bind_MapCreater.data_mapBuildingData.tileDic.ContainsKey(tempIndex))
            {
                bind_MapCreater.data_mapBuildingData.tileDic[tempIndex] = pair.value;
            }
            else
            {
                bind_MapCreater.data_mapBuildingData.tileDic.Add(tempIndex, pair.value);
            }
        }

    }
}
