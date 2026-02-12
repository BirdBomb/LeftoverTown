using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate_ConfigBuilding 
{
    private MapCreate bind_MapCreater;
    public async Task CreateConfigBuilding(MapCreate mapCreater)
    {
        bind_MapCreater = mapCreater;
        bind_MapCreater.text_Waiting.text = "正在坠落太阳";
        MapModConfig mapConfig_0 = MapModConfigData.GetMapModConfig(0);
        CreateMapMod(Vector2Int.zero, mapConfig_0);
        await Task.Yield();
        bind_MapCreater.text_Waiting.text = "正在进行一场失败的实验";
        MapModConfig mapConfig_100 = MapModConfigData.GetMapModConfig(100);
        CreateMapMod(new Vector2Int((int)(-bind_MapCreater.config_Map.map_Size * 0.5f), (int)(bind_MapCreater.config_Map.map_Size * 0.5f)), mapConfig_100);
        await Task.Yield();
        bind_MapCreater.text_Waiting.text = "正在定居";
        MapModConfig mapConfig_200 = MapModConfigData.GetMapModConfig(200);
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
