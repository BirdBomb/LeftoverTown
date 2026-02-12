using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Roads 
{
    MapCreate creater;
    public int Vector2ToIndex(int x, int y)
    {
        int tempX = x + 30000;
        int tempY = y + 30000;
        int tempIndex;
        if (tempY > tempX)
        {
            tempIndex = tempY * tempY + tempY + tempY - tempX;
        }
        else
        {
            tempIndex = tempX * tempX + tempY;
        }
        return tempIndex;
    }
    /// <summary>
    /// 生成道路
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateRoad(MapCreate mapCreater)
    {
        creater = mapCreater;
        creater.text_Waiting.text = "正在生成道路";

        MapCreate.RiverConfig config_River0;
        config_River0 = new MapCreate.RiverConfig()
        {
            vector2_Start = new Vector2(0, -creater.config_Map.map_Size),
            vector2_End = new Vector2(-creater.config_Map.map_Size, creater.config_Map.map_Size),
            river_NoiseScale = 40f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1f,
            river_DirectionInfluence = 0.5f,
            river_Width = 2,
            river_Forking = 3,
            river_ForkCount = 10,
            river_ForkMinDistance = 100,
            river_MaxStep = 9999,
            river_ArrivalTolerance = 10,
            river_Bounds = new Rect(-creater.config_Map.map_Size, -creater.config_Map.map_Size, creater.config_Map.map_Size * 2, creater.config_Map.map_Size * 2),
        };
        MapCreate.RiverConfig config_River1;
        config_River1 = new MapCreate.RiverConfig()
        {
            vector2_Start = new Vector2(creater.config_Map.map_Size, 0),
            vector2_End = new Vector2(-creater.config_Map.map_Size, -creater.config_Map.map_Size),
            river_NoiseScale = 40f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1f,
            river_DirectionInfluence = 0.5f,
            river_Width = 2,
            river_Forking = 3,
            river_ForkCount = 10,
            river_ForkMinDistance = 100,
            river_MaxStep = 9999,
            river_ArrivalTolerance = 10,
            river_Bounds = new Rect(-creater.config_Map.map_Size, -creater.config_Map.map_Size, creater.config_Map.map_Size * 2, creater.config_Map.map_Size * 2),
        };
        MapCreate.RiverConfig config_River2;
        config_River2 = new MapCreate.RiverConfig()
        {
            vector2_Start = new Vector2(0, 0),
            vector2_End = new Vector2(0, -creater.config_Map.map_Size),
            river_NoiseScale = 40f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1f,
            river_DirectionInfluence = 0.5f,
            river_Width = 2,
            river_Forking = 3,
            river_ForkCount = 10,
            river_ForkMinDistance = 100,
            river_MaxStep = 9999,
            river_ArrivalTolerance = 10,
            river_Bounds = new Rect(-creater.config_Map.map_Size, -creater.config_Map.map_Size, creater.config_Map.map_Size * 2, creater.config_Map.map_Size * 2),
        };

        await mapCreater.GenerateRiverSystemAsync(config_River0, DrawRoad, DrawBridge, DammingRoad);
        await mapCreater.GenerateRiverSystemAsync(config_River1, DrawRoad, DrawBridge, DammingRoad);
        await mapCreater.GenerateRiverSystemAsync(config_River2, DrawRoad, DrawBridge, DammingRoad);
    }
    private void DrawRoad(MapCreate.RiverSegment riverSegment)
    {
        int halfWidth_left = riverSegment.width / 2;
        int halfWidth_right = riverSegment.width - halfWidth_left;
        int index;
        for (int x = -halfWidth_left; x < halfWidth_right; x++)
        {
            for (int y = -halfWidth_left; y < halfWidth_right; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) == riverSegment.width) continue;
                index = Vector2ToIndex(Mathf.RoundToInt(riverSegment.position.x + x), Mathf.RoundToInt(riverSegment.position.y + y));
                if (creater.data_mapGroundData.tileDic.ContainsKey(index))
                {
                    creater.data_mapGroundData.tileDic[index] = 2001;
                }
                if (creater.data_mapBuildingData.tileDic.ContainsKey(index))
                {
                    creater.data_mapBuildingData.tileDic.Remove(index);
                }
            }
        }

    }
    /// <summary>
    /// 截断道路
    /// </summary>
    /// <param name="riverSegment"></param>
    /// <returns>是否截断</returns>
    private bool DammingRoad(MapCreate.RiverSegment riverSegment)
    {
        int index = Vector2ToIndex(Mathf.RoundToInt(riverSegment.position.x), Mathf.RoundToInt(riverSegment.position.y));
        if (creater.data_mapGroundData.tileDic.ContainsKey(index) && creater.data_mapGroundData.tileDic[index] != 2001)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    /// <summary>
    /// 架设桥梁
    /// </summary>
    /// <param name="riverSegment"></param>
    private void DrawBridge(MapCreate.RiverSegment riverSegment)
    {

    }
}
