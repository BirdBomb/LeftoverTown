using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class MapCreate_Mines 
{
    MapCreate bindMapCreater;
    private System.Random random_Temp = new System.Random();

    /// <summary>
    /// 矿区密度(平方米/个)
    /// </summary>
    private int mining_Density = 3500;
    /// <summary>
    /// 矿区最小尺寸
    /// </summary>
    private int mining_MinSize = 20;
    /// <summary>
    /// 矿区最大尺寸
    /// </summary>
    private int mining_MaxSize = 40;
    /// <summary>
    /// 生成矿区
    /// </summary>
    /// <returns></returns>
    public async Task CreateMining(MapCreate mapCreater)
    {
        random_Temp = new System.Random(mapCreater.GetAccIndex() + mapCreater.seed_Offset);

        bindMapCreater = mapCreater;
        int mining_Count = (bindMapCreater.config_Map.map_Size * 2) * (bindMapCreater.config_Map.map_Size * 2) / mining_Density;
        List<Vector2Int> points = new List<Vector2Int>();
        bindMapCreater.text_Waiting.text = "正在铺设矿区";
        MapCreate.PoissonPointsAreaConfig config0 = new MapCreate.PoissonPointsAreaConfig()
        {
            pointsArea_Center = Vector2Int.zero,
            pointsArea_Size = bindMapCreater.config_Map.map_Size,
            pointsArea_MinDistance = 60,
            pointsArea_MaxSamples = 20,
        };
        await bindMapCreater.GeneratePoissonPointsAsync(config0, (point) =>
        {
            points.Add(point);
        });

        for (int i = 0; i < points.Count; i++)
        {
            bindMapCreater.text_Waiting.text = "正在生成矿区" + i + "/" + points.Count;
            int mine_Size = random_Temp.Next(mining_MinSize, mining_MaxSize);
            Vector2 center = points[i];
            MapCreate.PerlinSolidConfig mineConfig = new MapCreate.PerlinSolidConfig()
            {
                area_Center = center,
                area_OuterRadius = mine_Size,
                area_FillRadius = 5,
                noise_Offset = bindMapCreater.GetRandomOffset(),
                noise_Sacle = 5,
            };
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(bindMapCreater.Vector2ToIndex((int)(center.x), (int)(center.y))))
            {
                int centerGroundId = bindMapCreater.data_mapGroundData.tileDic[bindMapCreater.Vector2ToIndex((int)(center.x), (int)(center.y))];
                if (centerGroundId == 1000)
                {
                    /*荒地*/
                    //await mapCreater.GenerateIslandArea_Tiny(config, CreateRockMining);
                }
                else if (centerGroundId == 1001)
                {
                    /*草地*/
                    await mapCreater.GenerateArea(mineConfig,
                        (int index, float perlinNoise, float realNoise) =>
                        {
                            CreateDualMining(index, perlinNoise, realNoise,
                                1110, 0.4f,
                                1111, 0.3f,
                                1000, 0.2f);
                        });
                }
                else if (centerGroundId == 1004 || centerGroundId == 1003)
                {
                    /*苔原地与雪地*/
                    await mapCreater.GenerateArea(mineConfig,
                        (int index, float perlinNoise, float realNoise) =>
                        {
                            CreateDualMining(index, perlinNoise, realNoise,
                                1110, 0.4f,
                                1111, 0.3f,
                                1000, 0.2f);
                        });
                }
                else if (centerGroundId == 1005)
                {
                    /*沙漠*/
                    await mapCreater.GenerateArea(mineConfig,
                        (int index, float perlinNoise, float realNoise) =>
                        {
                            CreateDualMining(index, perlinNoise, realNoise,
                                1110, 0.4f,
                                1111, 0.3f,
                                1000, 0.2f);
                        });
                }
            }
            mapCreater.slider_Waiting.value = (float)i / (float)points.Count;
        }
    }

    /// <summary>
    /// 双重矿区
    /// </summary>
    /// <param name="index">位置</param>
    /// <param name="mainMiningID">主要矿石ID</param>
    /// <param name="mainMiningWeight">主要矿石权重</param>
    /// <param name="minorMiningID">次要矿石ID</param>
    /// <param name="minorMiningWeight">次要矿石权重</param>
    /// <param name="edegWeight"></param>
    private void CreateDualMining(int index, float perlinNoise, float realNoise,
        short mainMiningID, float mainMiningWeight,
        short minorMiningID, float minorMiningWeight,
         short edegGroundID, float edegWeight)
    {
        if (realNoise > (1 - mainMiningWeight) && perlinNoise > (1 - minorMiningWeight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.TryGetValue(index, out short val) && val < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = edegGroundID;
                if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, mainMiningID))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = mainMiningID;
                }
            }
        }
        else if (realNoise > (1 - mainMiningWeight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.TryGetValue(index, out short val) && val < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = edegGroundID;
                if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, minorMiningID))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = minorMiningID;
                }
            }
        }
        else if (realNoise > (1 - mainMiningWeight - edegWeight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.TryGetValue(index, out short val) && val < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = edegGroundID;
            }
        }
    }
    /// <summary>
    /// 三重矿区
    /// </summary>
    /// <param name="index">位置</param>
    /// <param name="mainMiningID">主要矿石ID</param>
    /// <param name="mainMiningWeight">主要矿石权重</param>
    /// <param name="minorMiningID">次要矿石ID</param>
    /// <param name="minorMiningWeight">次要矿石权重</param>
    /// <param name="superMiningID">超级矿石ID</param>
    /// <param name="superMiningWeight">超级矿石权重</param>
    /// <param name="edegWeight"></param>
    private void CreateTripleMining(int index, float perlinNoise, float realNoise,
        short mainMiningID, float mainMiningWeight,
        short minorMiningID, float minorMiningWeight,
        short superMiningID, float superMiningWeight,
         short edegGroundID, float edegWeight)
    {
        if (realNoise > (1 - mainMiningWeight) && perlinNoise > (1 - minorMiningWeight - superMiningWeight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = edegGroundID;
                if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, superMiningID))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = superMiningID;
                }
            }
        }
        else if (realNoise > (1 - mainMiningWeight) && perlinNoise > (1 - minorMiningWeight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = edegGroundID;
                if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, minorMiningID))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = minorMiningID;
                }
            }
        }
        else if (realNoise > (1 - mainMiningWeight))
        {
            if (bindMapCreater.data_mapGroundData.tileDic.ContainsKey(index) && bindMapCreater.data_mapGroundData.tileDic[index] < 9000)
            {
                bindMapCreater.data_mapGroundData.tileDic[index] = edegGroundID;
                if (bindMapCreater.data_mapBuildingData.tileDic.TryAdd(index, mainMiningID))
                {
                    bindMapCreater.data_mapBuildingData.tileDic[index] = mainMiningID;
                }
            }
        }
        else if (realNoise > (1 - mainMiningWeight - edegWeight))
        {
            bindMapCreater.data_mapGroundData.tileDic[index] = edegGroundID;
        }
    }


}
