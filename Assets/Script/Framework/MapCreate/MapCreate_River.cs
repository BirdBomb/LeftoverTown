using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_River 
{
    MapCreate creater;
    /// <summary>
    /// 生成河流
    /// </summary>
    /// <param name="mapCreater"></param>
    /// <returns></returns>
    public async Task CreateRiver(MapCreate mapCreater)
    {
        creater = mapCreater;
        creater.text_Waiting.text = "正在生成河流";
        ///
        MapCreate.RiverConfig config_RiverFromCenterToLeftDown;
        config_RiverFromCenterToLeftDown = new MapCreate.RiverConfig()
        {
            vector2_Start = new Vector2(0, 0),
            vector2_End = new Vector2(-creater.config_Map.map_Size, -creater.config_Map.map_Size),
            river_NoiseScale = 40f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1f,
            river_DirectionInfluence = 0.25f,
            river_Width = 4,
            river_Forking = 5,
            river_ForkCount = 25,
            river_ForkMinDistance = 50,
            river_MaxStep = 9999,
            river_ArrivalTolerance = 10,
            river_Bounds = new Rect(-creater.config_Map.map_Size, -creater.config_Map.map_Size, creater.config_Map.map_Size * 2, creater.config_Map.map_Size * 2),
        };
        MapCreate.RiverConfig config_RiverFromCenterToRightUp;
        config_RiverFromCenterToRightUp = new MapCreate.RiverConfig()
        {
            vector2_Start = new Vector2(0, 0),
            vector2_End = new Vector2(creater.config_Map.map_Size, creater.config_Map.map_Size),
            river_NoiseScale = 40f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1f,
            river_DirectionInfluence = 0.25f,
            river_Width = 4,
            river_Forking = 5,
            river_ForkCount = 25,
            river_ForkMinDistance = 50,
            river_MaxStep = 9999,
            river_ArrivalTolerance = 10,
            river_Bounds = new Rect(-creater.config_Map.map_Size, -creater.config_Map.map_Size, creater.config_Map.map_Size * 2, creater.config_Map.map_Size * 2),
        };
        MapCreate.RiverConfig config_RiverFromLeftUpToCenter;
        config_RiverFromLeftUpToCenter = new MapCreate.RiverConfig()
        {
            vector2_Start = new Vector2(-creater.config_Map.map_Size, creater.config_Map.map_Size),
            vector2_End = new Vector2(0, 0),
            river_NoiseScale = 40f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1f,
            river_DirectionInfluence = 0.25f,
            river_Width = 4,
            river_Forking = 5,
            river_ForkCount = 25,
            river_ForkMinDistance = 50,
            river_MaxStep = 9999,
            river_ArrivalTolerance = 10,
            river_Bounds = new Rect(-creater.config_Map.map_Size, -creater.config_Map.map_Size, creater.config_Map.map_Size * 2, creater.config_Map.map_Size * 2),
        };
        MapCreate.RiverConfig config_RiverFromCenterToRightDown;
        config_RiverFromCenterToRightDown = new MapCreate.RiverConfig()
        {
            vector2_Start = new Vector2(0, 0),
            vector2_End = new Vector2(creater.config_Map.map_Size, -creater.config_Map.map_Size),
            river_NoiseScale = 40f,
            river_NoiseOffset = creater.GetRandomOffset(),
            river_Curvature = 1f,
            river_DirectionInfluence = 0.25f,
            river_Width = 4,
            river_Forking = 5,
            river_ForkCount = 25,
            river_ForkMinDistance = 50,
            river_MaxStep = 9999,
            river_ArrivalTolerance = 10,
            river_Bounds = new Rect(-creater.config_Map.map_Size, -creater.config_Map.map_Size, creater.config_Map.map_Size * 2, creater.config_Map.map_Size * 2),
        };

        await mapCreater.GenerateRiverSystemAsync(config_RiverFromCenterToLeftDown, DrawRiver, DrawBridge, DammingRiver);
        await mapCreater.GenerateRiverSystemAsync(config_RiverFromCenterToRightUp, DrawRiver, DrawBridge, DammingRiver);
        await mapCreater.GenerateRiverSystemAsync(config_RiverFromLeftUpToCenter, DrawRiver, DrawBridge, DammingRiver);
        await mapCreater.GenerateRiverSystemAsync(config_RiverFromCenterToRightDown, DrawRiver, DrawBridge, DammingRiver);
    }

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
    /// 填充河流
    /// </summary>
    /// <param name="riverSegment"></param>
    private void DrawRiver(MapCreate.RiverSegment riverSegment)
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
                if (creater.data_mapGroundData.tileDic.ContainsKey(index) && creater.data_mapGroundData.tileDic[index] != 9000)
                {
                    creater.data_mapGroundData.tileDic[index] = 9000;
                }
            }
        }
    }
    /// <summary>
    /// 截断河流
    /// </summary>
    /// <param name="riverSegment"></param>
    /// <returns>是否截断</returns>
    private bool DammingRiver(MapCreate.RiverSegment riverSegment)
    {
        int index = Vector2ToIndex(Mathf.RoundToInt(riverSegment.position.x), Mathf.RoundToInt(riverSegment.position.y));
        if (creater.data_mapGroundData.tileDic.ContainsKey(index) && creater.data_mapGroundData.tileDic[index] != 9000)
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
        //if (riverSegment.index % 500 == 0)
        //{
        //    Vector2 perpendicular = new Vector2(-riverSegment.direction.y, riverSegment.direction.x).normalized;
        //    int halfWidth_left = riverSegment.width / 2;
        //    int halfWidth_right = riverSegment.width - halfWidth_left;

        //    for (int i = -halfWidth_left - 3; i <= halfWidth_right + 3; i++) // 只处理两侧
        //    {
        //        Vector2 bridgePos = riverSegment.position + perpendicular * i;
        //        for (int x = -1; x < 1; x++)
        //        {
        //            for (int y = -1; y < 1; y++)
        //            {
        //                int index = Vector2ToIndex(Mathf.RoundToInt(bridgePos.x + x), Mathf.RoundToInt(bridgePos.y + y));
        //                if (creater.data_mapGroundData.tileDic.ContainsKey(index))
        //                {
        //                    creater.data_mapGroundData.tileDic[index] = 2000;
        //                }
        //            }
        //        }
        //    }
        //}
    }

}
