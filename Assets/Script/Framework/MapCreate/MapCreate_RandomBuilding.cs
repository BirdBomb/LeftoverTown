using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapCreate_RandomBuilding 
{
    /// <summary>
    /// 建筑之间最小距离
    /// </summary>
    private float building_MinDistance = 40;
    private float building_MaxDistance = 50;
    private float ruin_MinDistance = 20;
    private float ruin_MaxDistance = 30;
    private MapCreate mapCreater_Bind;
    private System.Random random = new System.Random();
    /// <summary>
    /// 生成随机建筑
    /// </summary>
    /// <returns></returns>
    public async Task CreateRandomBuilding(MapCreate mapCreater)
    {
        mapCreater_Bind = mapCreater;
        mapCreater_Bind.text_Waiting.text = "正在添加人类踪迹";
        random = new System.Random(mapCreater.GetAccIndex() + mapCreater.seed_Offset);
        MapCreate.PoissonPointsAreaConfig config = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = mapCreater_Bind.config_Map.map_Size,
            pointsArea_MinDistance = building_MinDistance,
            pointsArea_MaxDistance = building_MaxDistance,
            pointsArea_NoiseScale = 50,
            pointsArea_NoiseOffset = mapCreater_Bind.GetRandomOffset(),
            pointsArea_MaxSamples = 20,
        };
        await mapCreater_Bind.GeneratePoissonPointsAsync(config, TryToCreateRandomBuilding);
    }
    private void TryToCreateRandomBuilding(Vector2Int pos)
    {
        int index = mapCreater_Bind.Vector2ToIndex(pos.x, pos.y);
        if (mapCreater_Bind.data_mapBuildingData.tileDic.TryGetValue(index, out short id_Building) && id_Building != 0)
        {
            List<MapModConfig> list = MapModConfigData.mapModConfigs.FindAll((x) => { return !x.MapMod_Ruins && x.MapMod_BaseBuilding != null && x.MapMod_BaseBuilding.Contains(id_Building); });
            if (list.Count > 0)
            {
                MapModConfig mapModConfig = list[random.Next(0, list.Count)];
                CreateMapMod(pos, mapModConfig);
                return;
            }
        }
        if (mapCreater_Bind.data_mapGroundData.tileDic.TryGetValue(index, out short id_Ground) && id_Ground != 0)
        {
            List<MapModConfig> list = MapModConfigData.mapModConfigs.FindAll((x) => { return !x.MapMod_Ruins && x.MapMod_BaseGround != null && x.MapMod_BaseGround.Contains(id_Ground); });
            if (list.Count > 0)
            {
                MapModConfig mapModConfig = list[random.Next(0, list.Count)];
                CreateMapMod(pos, mapModConfig);
                return;
            }
        }

    }
    public async Task CreateRandomRuins(MapCreate mapCreater)
    {
        mapCreater_Bind = mapCreater;
        mapCreater_Bind.text_Waiting.text = "正在添加过时的人类踪迹";
        random = new System.Random(mapCreater.GetAccIndex() + mapCreater.seed_Offset);
        MapCreate.PoissonPointsAreaConfig config = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = mapCreater_Bind.config_Map.map_Size,
            pointsArea_MinDistance = ruin_MinDistance,
            pointsArea_MaxDistance = ruin_MaxDistance,
            pointsArea_NoiseScale = 50,
            pointsArea_NoiseOffset = mapCreater_Bind.GetRandomOffset(),
            pointsArea_MaxSamples = 20,
        };
        await mapCreater_Bind.GeneratePoissonPointsAsync(config, TryToCreateRandomRuin);

    }
    private void TryToCreateRandomRuin(Vector2Int pos)
    {
        int index = mapCreater_Bind.Vector2ToIndex(pos.x, pos.y);
        if (mapCreater_Bind.data_mapBuildingData.tileDic.TryGetValue(index, out short id_Building) && id_Building != 0)
        {
            List<MapModConfig> list = MapModConfigData.mapModConfigs.FindAll((x) => { return x.MapMod_Ruins && x.MapMod_BaseBuilding != null && x.MapMod_BaseBuilding.Contains(id_Building); });
            if (list.Count > 0)
            {
                MapModConfig mapModConfig = list[random.Next(0, list.Count)];
                CreateMapMod(pos, mapModConfig);
                return;
            }
        }
        if (mapCreater_Bind.data_mapGroundData.tileDic.TryGetValue(index, out short id_Ground) && id_Ground != 0)
        {
            List<MapModConfig> list = MapModConfigData.mapModConfigs.FindAll((x) => { return x.MapMod_Ruins && x.MapMod_BaseGround != null && x.MapMod_BaseGround.Contains(id_Ground); });
            if (list.Count > 0)
            {
                MapModConfig mapModConfig = list[random.Next(0, list.Count)];
                CreateMapMod(pos, mapModConfig);
                return;
            }
        }

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
            mapCreater_Bind.data_mapGroundData.tileDic[tempIndex] = pair.value;
            if (mapCreater_Bind.data_mapBuildingData.tileDic.ContainsKey(tempIndex)) mapCreater_Bind.data_mapBuildingData.tileDic.Remove(tempIndex);
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
            mapCreater_Bind.data_mapBuildingData.tileDic[tempIndex] = pair.value;
        }
    }
}
